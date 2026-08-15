/*
 * نەخشەی ژینزار — the schema map, on D3.
 *
 * The same tool the public site's mind map uses, for the same reason: d3.zoom gives real wheel
 * zoom and drag-to-pan, and d3.tree gives a tidy layout whose spacing is correct by construction.
 * The hand-rolled SVG this replaces had neither — every zoom step was a full Blazor re-render, and
 * node spacing was a formula I had to keep proving could not collide.
 *
 * ── Why a radial TREE and not a force simulation ──
 * The public site draws a word GRAPH: words related to words, no root, no levels, and a force
 * simulation is exactly right for that. This is a HIERARCHY — ژینزار → بەشی ئاخاوتن → تەوەر → نرخ
 * → تەوەر — where depth is the whole point. A force layout would let ناو's axes drift to the same
 * radius as کار's values and destroy the one thing the picture is for. d3.tree() keeps the levels
 * as rings and still gets the drag/zoom that makes the other one feel good.
 *
 * Loaded as a plain script (window.schemaMap) rather than an ES module: the admin has no bundler,
 * and d3.min.js is a classic script that defines window.d3, so a module here would only add an
 * import-order problem to solve.
 */
(function () {
  "use strict";

  const NODE_H = 26;

  /** Floor for the canvas height, so the collapsed views do not sit in a short box. */
  const MIN_H = 1000;
  const state = { svg: null, resize: null };

  /**
   * @param {HTMLElement} host   container the SVG is drawn into
   * @param {object} data        {label, count, family, kind, children:[…]}
   * @param {object} opts        {openDepth:int}
   */
  function render(host, data, opts) {
    if (!host || !window.d3) return;
    dispose();

    const d3 = window.d3;
    const openDepth = (opts && opts.openDepth) || 2;

    const root = d3.hierarchy(data);

    // Collapse below the requested depth. D3's convention is that _children holds the hidden
    // ones — the layout only ever sees `children`, so collapsing is a move between the two.
    root.each((d) => {
      d.id = d.data.key;
      if (d.depth >= openDepth && d.children) {
        d._children = d.children;
        d.children = null;
      }
    });

    const svg = d3.select(host).append("svg")
      .style("display", "block")
      .style("font-family", "var(--font-main)")
      .style("user-select", "none");

    state.svg = svg;

    const g = svg.append("g");

    // Rings live under everything and are redrawn per layout, since the radius per level changes
    // as branches open.
    const gRings = g.append("g").attr("fill", "none")
      .attr("stroke", "var(--border)").attr("stroke-dasharray", "2 6").attr("opacity", 0.7);
    const gLinks = g.append("g").attr("fill", "none");
    const gNodes = g.append("g");

    const zoom = d3.zoom()
      .scaleExtent([0.25, 4])
      .on("zoom", (ev) => g.attr("transform", ev.transform));

    svg.call(zoom).on("dblclick.zoom", null);

    // ── Layout ──────────────────────────────────────────────────────────
    //
    // Angle is separation-weighted: siblings under one parent sit closer than nodes from
    // different branches, which is what stops two limbs' labels running into each other where
    // their wedges meet. Radius is driven by the deepest open level so the map grows outward as
    // branches open rather than cramming more rings into a fixed circle.
    function layout() {
      const depth = d3.max(root.descendants(), (d) => d.depth) || 1;
      const leaves = root.leaves().length || 1;

      const radius = Math.max(220, (leaves * 46) / (2 * Math.PI), depth * 130);

      d3.tree()
        .size([2 * Math.PI, radius])
        .separation((a, b) => (a.parent === b.parent ? 1 : 2.2) / Math.max(a.depth, 1))(root);

      // ── Rings are re-spaced after the layout ──────────────────────────
      //
      // d3.tree spreads depth evenly across the radius, which puts ring 1 at radius/maxDepth —
      // and ring 1 is exactly where the labels are longest and the wedges narrowest, because a
      // part of speech with nothing under it claims a single leaf's worth of angle. Measured on
      // the real taxonomy that left هاوەڵناو and هاوەڵکار 61px apart with ~100px labels.
      //
      // So the first ring gets a floor of its own and the rest step outward from it. The map gets
      // bigger; fit() scales it to the viewport and the wheel takes you back in, which is the
      // whole reason for moving to d3.zoom.
      // These two floors are a direct trade against LEGIBILITY, not free spacing. Every pixel of
      // radius is a pixel the fit-to-view scale has to give back on the fully-open map, and the
      // labels shrink with it — so they are set as tight as the measured node gaps allow rather
      // than as loose as looks comfortable in the collapsed view.
      const first = Math.max(280, (countAtDepth(1) * 132) / (2 * Math.PI));
      const gap = Math.max(125, radius / Math.max(depth, 1));

      root.each((d) => { if (d.depth > 0) d.y = first + (d.depth - 1) * gap; });

      const ring = (i) => first + (i - 1) * gap;
      return { depth, ring };
    }

    function countAtDepth(n) {
      return root.descendants().filter((d) => d.depth === n).length || 1;
    }

    function pos(d) {
      // d.x is the angle, d.y the radius. -90° so the first branch starts at the top.
      const a = d.x - Math.PI / 2;
      return [Math.cos(a) * d.y, Math.sin(a) * d.y];
    }

    const linkGen = d3.linkRadial().angle((d) => d.x).radius((d) => d.y);

    function draw(source) {
      const { depth, ring } = layout();

      const nodes = root.descendants();
      const links = root.links();

      const t = svg.transition().duration(400);

      // ── Rings ────────────────────────────────────────────────────────
      gRings.selectAll("circle")
        .data(d3.range(1, depth + 1))
        .join("circle")
        .transition(t)
        .attr("r", (i) => ring(i));

      // ── Links ────────────────────────────────────────────────────────
      gLinks.selectAll("path")
        .data(links, (d) => d.target.id)
        .join(
          (enter) => enter.append("path")
            .attr("stroke", (d) => d.target.data.line)
            .attr("stroke-opacity", 0.55)
            .attr("stroke-width", (d) => (d.target.depth <= 1 ? 2.5 : 1.4))
            .attr("d", () => {
              const o = { x: source.x0 ?? source.x, y: source.y0 ?? source.y };
              return linkGen({ source: o, target: o });
            }),
          (update) => update,
          (exit) => exit.transition(t).remove()
            .attr("d", () => {
              const o = { x: source.x, y: source.y };
              return linkGen({ source: o, target: o });
            })
        )
        .transition(t)
        .attr("d", linkGen);

      // ── Nodes ────────────────────────────────────────────────────────
      const node = gNodes.selectAll("g.node")
        .data(nodes, (d) => d.id)
        .join(
          (enter) => {
            const ng = enter.append("g")
              .attr("class", "node")
              .attr("transform", () => {
                const [x, y] = pos({ x: source.x0 ?? source.x, y: source.y0 ?? source.y });
                return `translate(${x},${y})`;
              })
              .attr("opacity", 0)
              .style("cursor", (d) => (d.children || d._children ? "pointer" : "default"))
              .on("click", (ev, d) => {
                ev.stopPropagation();
                if (!d.children && !d._children) return;
                if (d.children) { d._children = d.children; d.children = null; }
                else { d.children = d._children; d._children = null; }
                draw(d);
              });

            // Dragging a branch is how you pull a crowded limb clear to read it. The node keeps
            // the position it was left at until the next expand, which is the behaviour the word
            // map has and the reason it feels manipulable rather than fixed.
            ng.call(d3.drag()
              .on("start", function () { d3.select(this).raise(); })
              .on("drag", function (ev, d) {
                d.fx = ev.x; d.fy = ev.y;
                d3.select(this).attr("transform", `translate(${ev.x},${ev.y})`);
              }));

            ng.append("rect")
              .attr("height", NODE_H)
              .attr("rx", NODE_H / 2)
              .attr("fill", (d) => d.data.bg)
              .attr("stroke", (d) => d.data.line)
              .attr("stroke-width", (d) => (d.data.kind === "value" ? 1 : 1.6));

            ng.append("text")
              .attr("class", "label")
              .attr("dominant-baseline", "central")
              .attr("text-anchor", "middle")
              .attr("font-size", (d) => (d.depth === 0 ? 14 : d.depth === 1 ? 13 : 12))
              .attr("fill", (d) => d.data.ink)
              .text((d) => d.data.label);

            // The count is its own element and pinned LTR: a Latin number inside one RTL text run
            // with a Kurdish label gets reordered by the bidi algorithm and reads backwards.
            ng.append("text")
              .attr("class", "count")
              .attr("dominant-baseline", "central")
              .attr("text-anchor", "middle")
              .attr("font-size", 10)
              .attr("opacity", 0.75)
              .attr("fill", (d) => d.data.ink)
              .style("direction", "ltr")
              .style("unicode-bidi", "isolate")
              .text((d) => d.data.count || "");

            // Hidden-child badge, so a closed branch says how much is behind it rather than
            // looking like a leaf.
            ng.append("circle").attr("class", "badge")
              .attr("r", 8).attr("stroke", "var(--card)").attr("stroke-width", 2);
            ng.append("text").attr("class", "badge-num")
              .attr("dominant-baseline", "central").attr("text-anchor", "middle")
              .attr("font-size", 9).attr("fill", "#fff")
              .style("direction", "ltr").style("unicode-bidi", "isolate");

            return ng;
          }
        );

      // Width follows the label, measured from the real glyphs rather than estimated per
      // character — which is what an estimate can never get right across two scripts.
      node.each(function (d) {
        const sel = d3.select(this);
        const label = sel.select("text.label").node();
        const w = Math.max(64, Math.min(210, (label ? label.getComputedTextLength() : 60) + 30));
        d.w = w;

        const twoLine = !!d.data.count;
        sel.select("rect")
          .attr("x", -w / 2)
          .attr("y", (twoLine ? 34 : NODE_H) / -2)
          .attr("width", w)
          .attr("height", twoLine ? 34 : NODE_H);

        sel.select("text.label").attr("y", twoLine ? -6 : 0);
        sel.select("text.count").attr("y", 9);

        const hidden = d._children ? countAll(d._children) : 0;
        sel.select("circle.badge")
          .attr("cy", (twoLine ? 34 : NODE_H) / 2)
          .attr("fill", d.data.line)
          .attr("display", hidden ? null : "none");
        sel.select("text.badge-num")
          .attr("y", (twoLine ? 34 : NODE_H) / 2)
          .attr("display", hidden ? null : "none")
          .text(hidden || "");
      });

      node.transition(t)
        .attr("opacity", 1)
        .attr("transform", (d) => {
          if (d.fx != null) return `translate(${d.fx},${d.fy})`;
          const [x, y] = pos(d);
          return `translate(${x},${y})`;
        });

      nodes.forEach((d) => { d.x0 = d.x; d.y0 = d.y; });

      fit();
    }

    function countAll(kids) {
      return kids.reduce((n, k) => n + 1 + countAll(k.children || k._children || []), 0);
    }

    /**
     * Sizes the canvas to the diagram at FULL SIZE. The card takes whatever height that needs.
     *
     * Scaling to fit was the mistake, twice over. Fitting both axes into a 72vh box, and then
     * fitting the width, both did the same thing: the more branches you opened, the smaller the
     * labels got — 62% and 7.5px text on the fully-open map. A diagram you cannot read is not
     * showing you anything, however completely it fits.
     *
     * So the scale is always 1 and the HEIGHT follows the drawing, without a cap. At five rings
     * that is a tall card and the page scrolls, which is the right trade: vertical scrolling is
     * how every other long page in this admin works, and the labels stay at 12px throughout.
     *
     * Width is the one axis that cannot always be satisfied — a five-ring radial map is ~1800px
     * across and no card is that wide — so .map-canvas keeps overflow-x for exactly that case,
     * and the wheel zooms out for anyone who wants the whole shape at once instead.
     */
    function fit() {
      const box = g.node().getBBox();
      if (!box.width || !box.height) return;

      const pad = 40;

      // Never narrower than the card, so a small map centres instead of hugging the start edge.
      const w = Math.max(host.clientWidth || 900, box.width + pad * 2);

      // 1000px floor. The collapsed views only need 620-880px, and a canvas that resizes between
      // three very different heights as you click through the depth buttons makes the page jump
      // under the cursor. A constant floor keeps بەشەکان and تەوەرەکان in one stable frame; past
      // it the height still grows with the drawing, so nothing is ever cut off.
      const h = Math.max(MIN_H, box.height + pad * 2);

      svg.attr("width", w).attr("height", h);

      const tx = w / 2 - (box.x + box.width / 2);
      const ty = h / 2 - (box.y + box.height / 2);

      svg.transition().duration(400)
        .call(zoom.transform, d3.zoomIdentity.translate(tx, ty));

      // Centre the horizontal scroll. Left-aligned, a wide map opens on empty margin with ژینزار
      // off to one side, which reads as the diagram having failed to load.
      requestAnimationFrame(() => {
        host.scrollLeft = Math.max(0, (w - host.clientWidth) / 2);
      });
    }

    root.x0 = 0;
    root.y0 = 0;
    draw(root);

    state.resize = () => fit();
    window.addEventListener("resize", state.resize);
  }

  function dispose() {
    if (state.resize) window.removeEventListener("resize", state.resize);
    if (state.svg) state.svg.remove();
    state.svg = null;
    state.resize = null;
  }

  window.schemaMap = { render, dispose };
})();
