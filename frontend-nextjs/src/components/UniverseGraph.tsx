"use client";

import {
  useRef, useEffect, useCallback,
  forwardRef, useImperativeHandle,
} from "react";
import * as d3 from "d3";

// ── Simulation types ──────────────────────────────────────────────────────────

export interface SimNode extends d3.SimulationNodeDatum {
  id: string;
  label: string;
  speechPane: number;
  category?: string;
  relationType?: string;
  weight: number;
  isFocal: boolean;
  wordId: number;
}

interface SimLink extends d3.SimulationLinkDatum<SimNode> {
  uid: string;
  relationType: string;
  weight: number;
}

// ── Visual config ─────────────────────────────────────────────────────────────

interface NodeCfg {
  fill: string; stroke: string; glow: string;
  shape: "circle" | "diamond" | "hexagon" | "triangle" | "pentagon";
  r: number;
}

const SPEECH: Record<number, NodeCfg> = {
  1:  { fill: "#6366f1", stroke: "#a5b4fc", glow: "#6366f1", shape: "circle",   r: 22 },
  2:  { fill: "#ec4899", stroke: "#f9a8d4", glow: "#ec4899", shape: "diamond",  r: 20 },
  3:  { fill: "#14b8a6", stroke: "#5eead4", glow: "#14b8a6", shape: "hexagon",  r: 20 },
  4:  { fill: "#f59e0b", stroke: "#fcd34d", glow: "#f59e0b", shape: "triangle", r: 18 },
  5:  { fill: "#8b5cf6", stroke: "#c4b5fd", glow: "#8b5cf6", shape: "pentagon", r: 20 },
  6:  { fill: "#06b6d4", stroke: "#67e8f9", glow: "#06b6d4", shape: "circle",   r: 16 },
  7:  { fill: "#10b981", stroke: "#6ee7b7", glow: "#10b981", shape: "circle",   r: 16 },
  8:  { fill: "#f97316", stroke: "#fdba74", glow: "#f97316", shape: "diamond",  r: 17 },
  9:  { fill: "#ef4444", stroke: "#fca5a5", glow: "#ef4444", shape: "circle",   r: 17 },
  10: { fill: "#84cc16", stroke: "#bef264", glow: "#84cc16", shape: "hexagon",  r: 16 },
  11: { fill: "#a78bfa", stroke: "#ddd6fe", glow: "#a78bfa", shape: "circle",   r: 15 },
  12: { fill: "#fb923c", stroke: "#fed7aa", glow: "#fb923c", shape: "pentagon", r: 17 },
  13: { fill: "#38bdf8", stroke: "#bae6fd", glow: "#38bdf8", shape: "circle",   r: 15 },
  14: { fill: "#475569", stroke: "#94a3b8", glow: "#475569", shape: "circle",   r: 14 },
};
const DEF_CFG: NodeCfg = { fill: "#475569", stroke: "#94a3b8", glow: "#475569", shape: "circle", r: 14 };

const REL_COLOR: Record<string, string> = {
  synonym: "#22d36d", antonym: "#f87171", related: "#60a5fa",
  example: "#fbbf24", usage: "#a78bfa", contextual: "#34d4f0",
};
const REL_DASH: Record<string, string> = {
  synonym: "none", antonym: "8,4", related: "none",
  example: "3,5",  usage: "10,3,2,3", contextual: "18,5",
};

// ── Shape helpers ─────────────────────────────────────────────────────────────

const hex = (r: number) =>
  Array.from({ length: 6 }, (_, i) => {
    const a = (Math.PI / 3) * i - Math.PI / 6;
    return `${i ? "L" : "M"}${(r * Math.cos(a)).toFixed(1)},${(r * Math.sin(a)).toFixed(1)}`;
  }).join(" ") + " Z";

const diamond = (r: number) =>
  `M0,${-r} L${(r * .7).toFixed(1)},0 L0,${r} L${(-r * .7).toFixed(1)},0 Z`;

const triangle = (r: number) => {
  const h = r * 1.1;
  return `M0,${-h} L${(r * .95).toFixed(1)},${(h * .6).toFixed(1)} L${(-r * .95).toFixed(1)},${(h * .6).toFixed(1)} Z`;
};

const pentagon = (r: number) =>
  Array.from({ length: 5 }, (_, i) => {
    const a = (2 * Math.PI / 5) * i - Math.PI / 2;
    return `${i ? "L" : "M"}${(r * Math.cos(a)).toFixed(1)},${(r * Math.sin(a)).toFixed(1)}`;
  }).join(" ") + " Z";

function cfg(n: SimNode) { return SPEECH[n.speechPane] ?? DEF_CFG; }
function radius(n: SimNode) {
  const base = (SPEECH[n.speechPane] ?? DEF_CFG).r + Math.min(n.weight * 1.2, 10);
  return n.isFocal ? base * 1.7 : base;
}

// ── Component ─────────────────────────────────────────────────────────────────

export interface UniverseGraphHandle {
  loadWord(wordId: number): Promise<void>;
}

interface Props {
  onNodeClick(wordId: number, label: string): void;
  /** Called each simulation tick with the focal node's screen coordinates. */
  onFocalPos(sx: number, sy: number): void;
}

const UniverseGraph = forwardRef<UniverseGraphHandle, Props>(
  function UniverseGraph({ onNodeClick, onFocalPos }, ref) {
    const svgRef  = useRef<SVGSVGElement>(null);
    const gRef    = useRef<SVGGElement | null>(null);
    const simRef  = useRef<d3.Simulation<SimNode, SimLink> | null>(null);
    const zoomRef = useRef<d3.ZoomBehavior<SVGSVGElement, unknown> | null>(null);
    const xfRef   = useRef<d3.ZoomTransform>(d3.zoomIdentity);
    const nodes   = useRef(new Map<string, SimNode>());
    const links   = useRef<SimLink[]>([]);
    const focalId = useRef<string | null>(null);

    // Keep callbacks fresh without re-running D3 init
    const clickRef    = useRef(onNodeClick); clickRef.current    = onNodeClick;
    const focalPosRef = useRef(onFocalPos);  focalPosRef.current = onFocalPos;

    // ── Init D3 once ──────────────────────────────────────────────────────────
    useEffect(() => {
      if (!svgRef.current) return;
      const svg = d3.select(svgRef.current);
      let W = window.innerWidth, H = window.innerHeight;
      svg.attr("width", W).attr("height", H);

      // ── Defs ────────────────────────────────────────────────────────────────
      const defs = svg.append("defs");

      const mkFilter = (id: string, dev: number, spread = "60%") => {
        const f = defs.append("filter").attr("id", id)
          .attr("x", `-${spread}`).attr("y", `-${spread}`)
          .attr("width", `${100 + 2 * parseFloat(spread)}%`)
          .attr("height", `${100 + 2 * parseFloat(spread)}%`);
        f.append("feGaussianBlur").attr("in", "SourceGraphic")
          .attr("stdDeviation", dev).attr("result", "b");
        const m = f.append("feMerge");
        m.append("feMergeNode").attr("in", "b");
        m.append("feMergeNode").attr("in", "SourceGraphic");
      };
      mkFilter("glow", 3, "50%");
      mkFilter("strongGlow", 10, "80%");

      // Arrow markers per relation type
      Object.entries(REL_COLOR).forEach(([rel, color]) => {
        defs.append("marker").attr("id", `ar-${rel}`)
          .attr("viewBox", "0 0 10 10").attr("refX", 8).attr("refY", 5)
          .attr("markerWidth", 5).attr("markerHeight", 5)
          .attr("orient", "auto-start-reverse")
          .append("path").attr("d", "M0,0 L0,10 L10,5 Z")
          .attr("fill", color).attr("opacity", 0.85);
      });

      // ── Scene ────────────────────────────────────────────────────────────────
      const g = svg.append("g");
      gRef.current = g.node();
      g.append("g").attr("class", "links");
      g.append("g").attr("class", "nodes");
      g.append("g").attr("class", "labels");

      // ── Simulation ───────────────────────────────────────────────────────────
      const sim = d3.forceSimulation<SimNode>()
        .force("link",
          d3.forceLink<SimNode, SimLink>([])
            .id(d => d.id)
            .distance(d => Math.max(90, 180 / Math.max(0.5, (d as SimLink).weight)))
            .strength(0.45))
        .force("charge",
          d3.forceManyBody<SimNode>()
            .strength(d => d.isFocal ? -1200 : -280)
            .distanceMax(600)
            .theta(0.85))
        .force("collision",
          d3.forceCollide<SimNode>().radius(d => radius(d) + 18).strength(0.8))
        .force("x", d3.forceX<SimNode>(W / 2).strength(d => d.isFocal ? 1 : 0.012))
        .force("y", d3.forceY<SimNode>(H / 2).strength(d => d.isFocal ? 1 : 0.012))
        .alphaDecay(0.016)
        .velocityDecay(0.48)
        .on("tick", tick);

      simRef.current = sim;

      // ── Zoom / pan ───────────────────────────────────────────────────────────
      const zoom = d3.zoom<SVGSVGElement, unknown>()
        .scaleExtent([0.04, 8])
        .filter((ev: Event) => {
          // Allow zoom, but not when clicking inside a node
          if (ev.type === "mousedown" && (ev.target as Element).closest(".node-g")) return false;
          return true;
        })
        .on("zoom", ev => {
          xfRef.current = ev.transform;
          g.attr("transform", ev.transform);
          // Level-of-detail: hide labels when zoomed out
          const k = ev.transform.k;
          const op = k < 0.3 ? 0 : k < 0.65 ? (k - 0.3) / 0.35 : 1;
          g.select(".labels").style("opacity", op);
        });

      svg.call(zoom).on("dblclick.zoom", null);
      zoomRef.current = zoom;

      // ── Tick ─────────────────────────────────────────────────────────────────
      function tick() {
        const gg = d3.select(gRef.current!);

        // Links — offset endpoints so lines end at node boundary
        gg.select(".links").selectAll<SVGLineElement, SimLink>(".link")
          .each(function(d) {
            const s = d.source as SimNode, t = d.target as SimNode;
            if (s.x == null || s.y == null || t.x == null || t.y == null) return;
            const dx = t.x - s.x, dy = t.y - s.y;
            const len = Math.sqrt(dx * dx + dy * dy) || 1;
            const ux = dx / len, uy = dy / len;
            const sr = radius(s) + 2, tr = radius(t) + 9;
            d3.select(this)
              .attr("x1", s.x + ux * sr).attr("y1", s.y + uy * sr)
              .attr("x2", t.x - ux * tr).attr("y2", t.y - uy * tr);
          });

        // Nodes
        gg.select(".nodes").selectAll<SVGGElement, SimNode>(".node-g")
          .attr("transform", d => `translate(${d.x ?? 0},${d.y ?? 0})`);

        // Labels
        gg.select(".labels").selectAll<SVGTextElement, SimNode>(".lbl")
          .attr("x", d => d.x ?? 0)
          .attr("y", d => (d.y ?? 0) + radius(d) + 8);

        // Emit focal screen coords
        if (focalId.current) {
          const fn = nodes.current.get(focalId.current);
          if (fn?.x != null && fn.y != null) {
            const t = xfRef.current;
            focalPosRef.current(fn.x * t.k + t.x, fn.y * t.k + t.y);
          }
        }
      }

      // ── Resize ───────────────────────────────────────────────────────────────
      const onResize = () => {
        W = window.innerWidth; H = window.innerHeight;
        svg.attr("width", W).attr("height", H);
        (sim.force("x") as d3.ForceX<SimNode>).x(W / 2);
        (sim.force("y") as d3.ForceY<SimNode>).y(H / 2);
      };
      window.addEventListener("resize", onResize);
      return () => { sim.stop(); window.removeEventListener("resize", onResize); };
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    // ── Load a word into the universe ─────────────────────────────────────────
    const loadWord = useCallback(async (wordId: number) => {
      const res = await fetch(`/api/words/${wordId}/graph`);
      if (!res.ok) return;
      const graph: { nodes: Array<{ id: string; label: string; category?: string; isCenter: boolean; weight: number; relationType?: string; speechPane: number }>; links: Array<{ source: string; target: string; relationType: string; weight: number }> } = await res.json();

      const sim = simRef.current!;
      const W = window.innerWidth, H = window.innerHeight;
      const newFocalId = String(wordId);

      // Unpin old focal
      if (focalId.current) {
        const old = nodes.current.get(focalId.current);
        if (old) { old.isFocal = false; old.fx = undefined; old.fy = undefined; }
      }
      focalId.current = newFocalId;

      // Merge nodes (never remove, only add)
      graph.nodes.forEach(n => {
        const isCenter = n.isCenter || n.id === newFocalId;
        if (!nodes.current.has(n.id)) {
          const angle = Math.random() * Math.PI * 2;
          const spread = isCenter ? 0 : 60 + Math.random() * 80;
          nodes.current.set(n.id, {
            id: n.id, label: n.label, speechPane: n.speechPane ?? 14,
            category: n.category, relationType: n.relationType,
            weight: n.weight || 1, isFocal: isCenter,
            wordId: parseInt(n.id),
            x: W / 2 + Math.cos(angle) * spread,
            y: H / 2 + Math.sin(angle) * spread,
            vx: 0, vy: 0,
          });
        } else {
          const ex = nodes.current.get(n.id)!;
          ex.isFocal = isCenter;
        }
      });

      // Pin new focal at center
      const fn = nodes.current.get(newFocalId);
      if (fn) { fn.isFocal = true; fn.fx = W / 2; fn.fy = H / 2; fn.x = W / 2; fn.y = H / 2; }

      // Merge links
      const existingUids = new Set(links.current.map(l => l.uid));
      graph.links.forEach(l => {
        const uid = `${l.source}→${l.target}:${l.relationType}`;
        if (!existingUids.has(uid))
          links.current.push({ uid, source: l.source, target: l.target, relationType: l.relationType, weight: l.weight || 1 });
      });

      // Update simulation
      const allNodes = Array.from(nodes.current.values());
      sim.nodes(allNodes);
      (sim.force("link") as d3.ForceLink<SimNode, SimLink>).links(links.current);

      renderGraph();
      sim.alpha(0.6).restart();

      // Camera: smooth pan to center
      if (svgRef.current && zoomRef.current) {
        const cur = xfRef.current;
        const targetScale = Math.min(1.3, Math.max(0.5, cur.k));
        d3.select(svgRef.current)
          .transition().duration(850).ease(d3.easeCubicOut)
          .call(zoomRef.current.transform,
            d3.zoomIdentity
              .translate(W / 2, H / 2)
              .scale(targetScale)
              .translate(-W / 2, -H / 2)
          );
      }
    }, []);

    useImperativeHandle(ref, () => ({ loadWord }), [loadWord]);

    // ── Render graph elements (enter/update/exit) ─────────────────────────────
    function renderGraph() {
      if (!gRef.current || !simRef.current) return;
      const gg  = d3.select(gRef.current);
      const sim = simRef.current;
      const allNodes = Array.from(nodes.current.values());

      // ── Links ────────────────────────────────────────────────────────────────
      const lSel = gg.select(".links")
        .selectAll<SVGLineElement, SimLink>(".link")
        .data(links.current, d => d.uid);

      lSel.exit().transition().duration(250).style("opacity", 0).remove();

      lSel.enter().append("line").attr("class", "link")
        .style("opacity", 0)
        .attr("stroke", d => REL_COLOR[d.relationType] || "#475569")
        .attr("stroke-width", d => Math.max(1, d.weight * 0.65 + 0.7))
        .attr("stroke-dasharray", d => REL_DASH[d.relationType] || "none")
        .attr("stroke-linecap", "round")
        .attr("stroke-opacity", 0.5)
        .attr("marker-end", d => `url(#ar-${d.relationType})`)
        .on("mouseenter", function(_, d) {
          d3.select(this).attr("stroke-opacity", 1)
            .attr("stroke-width", Math.max(2, d.weight * 0.65 + 0.7) + 1.5);
        })
        .on("mouseleave", function(_, d) {
          d3.select(this).attr("stroke-opacity", 0.5)
            .attr("stroke-width", Math.max(1, d.weight * 0.65 + 0.7));
        })
        .transition().duration(600).style("opacity", 1);

      // ── Nodes ────────────────────────────────────────────────────────────────
      const nSel = gg.select(".nodes")
        .selectAll<SVGGElement, SimNode>(".node-g")
        .data(allNodes, d => d.id);

      nSel.exit().transition().duration(250).style("opacity", 0).remove();

      const nEnter = nSel.enter().append("g").attr("class", "node-g")
        .style("cursor", "pointer").style("opacity", 0)
        .call(d3.drag<SVGGElement, SimNode>()
          .on("start", (ev, d) => {
            if (!ev.active) sim.alphaTarget(0.3).restart();
            d.fx = d.x!; d.fy = d.y!;
          })
          .on("drag", (ev, d) => { if (!d.isFocal) { d.fx = ev.x; d.fy = ev.y; } })
          .on("end", (ev, d) => {
            if (!ev.active) sim.alphaTarget(0);
            if (!d.isFocal) { d.fx = undefined; d.fy = undefined; }
          })
        )
        .on("click", (ev, d) => {
          ev.stopPropagation();
          clickRef.current(d.wordId, d.label);
        });

      // Outer ring (focal only) — pulsing halo
      nEnter.append("circle").attr("class", "halo")
        .attr("r", d => radius(d) + 16)
        .attr("fill", "none")
        .attr("stroke", d => cfg(d).glow)
        .attr("stroke-width", 1.5)
        .attr("stroke-opacity", 0.45)
        .attr("stroke-dasharray", "4,3")
        .style("display", d => d.isFocal ? null : "none");

      // Node shape
      nEnter.each(function(d) {
        const el  = d3.select(this);
        const c   = cfg(d);
        const r   = radius(d);
        const flt = d.isFocal ? "url(#strongGlow)" : "url(#glow)";
        const sw  = d.isFocal ? 2.5 : 1.5;

        let s: d3.Selection<SVGElement, SimNode, null, undefined>;
        switch (c.shape) {
          case "diamond":  s = el.append("path").attr("d", diamond(r)) as never; break;
          case "hexagon":  s = el.append("path").attr("d", hex(r))     as never; break;
          case "triangle": s = el.append("path").attr("d", triangle(r))as never; break;
          case "pentagon": s = el.append("path").attr("d", pentagon(r))as never; break;
          default:         s = el.append("circle").attr("r", r)         as never;
        }
        s.attr("class", "shape")
          .attr("fill", c.fill).attr("stroke", c.stroke)
          .attr("stroke-width", sw).attr("filter", flt);
      });

      // Hover
      nEnter.merge(nSel)
        .on("mouseenter", function(_, d) {
          d3.select(this).select(".shape").attr("stroke-width", 3.5).attr("stroke", "#fff");
          d3.select(this).raise();
        })
        .on("mouseleave", function(_, d) {
          d3.select(this).select(".shape")
            .attr("stroke-width", d.isFocal ? 2.5 : 1.5)
            .attr("stroke", cfg(d).stroke);
        });

      nEnter.transition().duration(700)
        .delay((_, i) => Math.min(i * 20, 350))
        .style("opacity", 1);

      // Update existing nodes' focal state
      nSel.select(".halo").style("display", d => d.isFocal ? null : "none");
      nSel.select(".shape")
        .attr("stroke-width", d => d.isFocal ? 2.5 : 1.5)
        .attr("filter", d => d.isFocal ? "url(#strongGlow)" : "url(#glow)");

      // ── Labels ───────────────────────────────────────────────────────────────
      const lblSel = gg.select(".labels")
        .selectAll<SVGTextElement, SimNode>(".lbl")
        .data(allNodes, d => d.id);

      lblSel.exit().remove();

      const lblEnter = lblSel.enter().append("text").attr("class", "lbl")
        .attr("text-anchor", "middle")
        .style("font-family", "'NRT', system-ui, sans-serif")
        .style("pointer-events", "none")
        .attr("paint-order", "stroke")
        .attr("stroke", "rgba(8,12,26,0.95)")
        .text(d => d.label);

      lblEnter.merge(lblSel)
        .style("font-size", d => `${d.isFocal ? 15 : 11}px`)
        .style("font-weight", d => d.isFocal ? "700" : "500")
        .style("fill", d => d.isFocal ? "#f1f5f9" : "#94a3b8")
        .attr("stroke-width", d => d.isFocal ? "4px" : "3px");
    }

    return (
      <svg ref={svgRef} className="absolute inset-0 w-full h-full"
        style={{ background: "transparent" }} />
    );
  }
);

export default UniverseGraph;
