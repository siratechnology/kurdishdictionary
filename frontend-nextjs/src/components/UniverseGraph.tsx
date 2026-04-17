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

// ── Visual config — all circles, cyan/electric-blue palette ──────────────────

interface NodeCfg { fill: string; stroke: string; glow: string; r: number; }

const SPEECH: Record<number, NodeCfg> = {
  1:  { fill: "#06b6d4", stroke: "#67e8f9", glow: "#06b6d4", r: 22 }, // cyan-500
  2:  { fill: "#0ea5e9", stroke: "#7dd3fc", glow: "#0ea5e9", r: 20 }, // sky-500
  3:  { fill: "#3b82f6", stroke: "#93c5fd", glow: "#3b82f6", r: 20 }, // blue-500
  4:  { fill: "#22d3ee", stroke: "#a5f3fc", glow: "#22d3ee", r: 18 }, // cyan-400
  5:  { fill: "#38bdf8", stroke: "#bae6fd", glow: "#38bdf8", r: 20 }, // sky-400
  6:  { fill: "#60a5fa", stroke: "#bfdbfe", glow: "#60a5fa", r: 16 }, // blue-400
  7:  { fill: "#2dd4bf", stroke: "#99f6e4", glow: "#2dd4bf", r: 16 }, // teal-400
  8:  { fill: "#818cf8", stroke: "#c7d2fe", glow: "#818cf8", r: 17 }, // indigo-400
  9:  { fill: "#a78bfa", stroke: "#ddd6fe", glow: "#a78bfa", r: 17 }, // violet-400
  10: { fill: "#0891b2", stroke: "#22d3ee", glow: "#0891b2", r: 16 }, // cyan-600
  11: { fill: "#0284c7", stroke: "#38bdf8", glow: "#0284c7", r: 15 }, // sky-600
  12: { fill: "#1d4ed8", stroke: "#60a5fa", glow: "#1d4ed8", r: 17 }, // blue-700
  13: { fill: "#0e7490", stroke: "#06b6d4", glow: "#0e7490", r: 15 }, // cyan-700
  14: { fill: "#475569", stroke: "#94a3b8", glow: "#475569", r: 14 }, // slate
};
const DEF_CFG: NodeCfg = { fill: "#475569", stroke: "#94a3b8", glow: "#475569", r: 14 };

const REL_COLOR: Record<string, string> = {
  synonym:    "#22d3ee",
  antonym:    "#f87171",
  related:    "#60a5fa",
  example:    "#fbbf24",
  usage:      "#a78bfa",
  contextual: "#34d4f0",
  partof:     "#fb923c",
};
const REL_DASH: Record<string, string> = {
  synonym:    "none",
  antonym:    "8,4",
  related:    "none",
  example:    "3,5",
  usage:      "10,3,2,3",
  contextual: "18,5",
  partof:     "6,3,1,3",
};

function cfg(n: SimNode)    { return SPEECH[n.speechPane] ?? DEF_CFG; }
function radius(n: SimNode) {
  const base = (SPEECH[n.speechPane] ?? DEF_CFG).r + Math.min(n.weight * 1.2, 10);
  return n.isFocal ? base * 1.7 : base;
}

// Deterministic stagger from uid string
function uidHash(s: string) {
  let h = 0;
  for (let i = 0; i < s.length; i++) h = (h * 31 + s.charCodeAt(i)) & 0xffffffff;
  return Math.abs(h);
}

// ── Component ─────────────────────────────────────────────────────────────────

export interface UniverseGraphHandle { loadWord(wordId: number): Promise<void>; }

interface Props {
  onNodeClick(wordId: number, label: string): void;
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

      // Soft glow filter
      const mkGlow = (id: string, dev: number, spread: string) => {
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
      mkGlow("glow",       4,  "55%");
      mkGlow("strongGlow", 16, "100%");

      // Arrow markers — small, subtle
      Object.entries(REL_COLOR).forEach(([rel, color]) => {
        defs.append("marker").attr("id", `ar-${rel}`)
          .attr("viewBox", "0 0 10 10").attr("refX", 8).attr("refY", 5)
          .attr("markerWidth", 4).attr("markerHeight", 4)
          .attr("orient", "auto-start-reverse")
          .append("path").attr("d", "M0,0 L0,10 L10,5 Z")
          .attr("fill", color).attr("opacity", 0.5);
      });

      // ── Scene layers ─────────────────────────────────────────────────────────
      const g = svg.append("g");
      gRef.current = g.node();
      g.append("g").attr("class", "links-bg");     // static thin routes
      g.append("g").attr("class", "links-pulse");  // animated traveling dots
      g.append("g").attr("class", "nodes");
      g.append("g").attr("class", "labels");

      // ── Simulation ───────────────────────────────────────────────────────────
      const sim = d3.forceSimulation<SimNode>()
        .force("link",
          d3.forceLink<SimNode, SimLink>([]).id(d => d.id)
            .distance(d => Math.max(90, 180 / Math.max(0.5, (d as SimLink).weight)))
            .strength(0.45))
        .force("charge",
          d3.forceManyBody<SimNode>()
            .strength(d => d.isFocal ? -1200 : -280)
            .distanceMax(600).theta(0.85))
        .force("collision",
          d3.forceCollide<SimNode>().radius(d => radius(d) + 18).strength(0.8))
        .force("x", d3.forceX<SimNode>(W / 2).strength(d => d.isFocal ? 1 : 0.012))
        .force("y", d3.forceY<SimNode>(H / 2).strength(d => d.isFocal ? 1 : 0.012))
        .alphaDecay(0.016).velocityDecay(0.48)
        .on("tick", tick);

      simRef.current = sim;

      // ── Zoom / pan ───────────────────────────────────────────────────────────
      const zoom = d3.zoom<SVGSVGElement, unknown>()
        .scaleExtent([0.04, 8])
        .filter((ev: Event) => {
          if (ev.type === "mousedown" && (ev.target as Element).closest(".node-g")) return false;
          return true;
        })
        .on("zoom", ev => {
          xfRef.current = ev.transform;
          g.attr("transform", ev.transform);
          const k  = ev.transform.k;
          const op = k < 0.3 ? 0 : k < 0.65 ? (k - 0.3) / 0.35 : 1;
          g.select(".labels").style("opacity", op);
        });

      svg.call(zoom).on("dblclick.zoom", null);
      zoomRef.current = zoom;

      // ── Tick ─────────────────────────────────────────────────────────────────
      function tick() {
        const gg = d3.select(gRef.current!);

        const positionLine = (sel: d3.Selection<SVGLineElement, SimLink, SVGGElement, unknown>) =>
          sel.each(function(d) {
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

        positionLine(gg.select<SVGGElement>(".links-bg").selectAll<SVGLineElement, SimLink>(".link-bg"));
        positionLine(gg.select<SVGGElement>(".links-pulse").selectAll<SVGLineElement, SimLink>(".link-pulse"));

        gg.select(".nodes").selectAll<SVGGElement, SimNode>(".node-g")
          .attr("transform", d => `translate(${d.x ?? 0},${d.y ?? 0})`);

        gg.select(".labels").selectAll<SVGTextElement, SimNode>(".lbl")
          .attr("x", d => d.x ?? 0)
          .attr("y", d => (d.y ?? 0) + radius(d) + 9);

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

    // ── Load a word ───────────────────────────────────────────────────────────
    const loadWord = useCallback(async (wordId: number) => {
      const res = await fetch(`/api/words/${wordId}/graph`);
      if (!res.ok) return;
      const graph: {
        nodes: Array<{ id: string; label: string; category?: string; isCenter: boolean; weight: number; relationType?: string; speechPane: number }>;
        links: Array<{ source: string; target: string; relationType: string; weight: number }>;
      } = await res.json();

      const sim = simRef.current!;
      const W = window.innerWidth, H = window.innerHeight;
      const newFocalId = String(wordId);

      if (focalId.current) {
        const old = nodes.current.get(focalId.current);
        if (old) { old.isFocal = false; old.fx = undefined; old.fy = undefined; }
      }
      focalId.current = newFocalId;

      graph.nodes.forEach(n => {
        const isCenter = n.isCenter || n.id === newFocalId;
        if (!nodes.current.has(n.id)) {
          const angle  = Math.random() * Math.PI * 2;
          const spread = isCenter ? 0 : 60 + Math.random() * 80;
          nodes.current.set(n.id, {
            id: n.id, label: n.label, speechPane: n.speechPane ?? 14,
            category: n.category, relationType: n.relationType,
            weight: n.weight || 1, isFocal: isCenter, wordId: parseInt(n.id),
            x: W / 2 + Math.cos(angle) * spread,
            y: H / 2 + Math.sin(angle) * spread,
            vx: 0, vy: 0,
          });
        } else {
          nodes.current.get(n.id)!.isFocal = isCenter;
        }
      });

      const fn = nodes.current.get(newFocalId);
      if (fn) { fn.isFocal = true; fn.fx = W / 2; fn.fy = H / 2; fn.x = W / 2; fn.y = H / 2; }

      const existingUids = new Set(links.current.map(l => l.uid));
      graph.links.forEach(l => {
        const uid = `${l.source}→${l.target}:${l.relationType}`;
        if (!existingUids.has(uid))
          links.current.push({ uid, source: l.source, target: l.target, relationType: l.relationType, weight: l.weight || 1 });
      });

      const allNodes = Array.from(nodes.current.values());
      sim.nodes(allNodes);
      (sim.force("link") as d3.ForceLink<SimNode, SimLink>).links(links.current);
      renderGraph();
      sim.alpha(0.6).restart();

      if (svgRef.current && zoomRef.current) {
        const cur = xfRef.current;
        const targetScale = Math.min(1.3, Math.max(0.5, cur.k));
        d3.select(svgRef.current)
          .transition().duration(850).ease(d3.easeCubicOut)
          .call(zoomRef.current.transform,
            d3.zoomIdentity.translate(W / 2, H / 2).scale(targetScale).translate(-W / 2, -H / 2));
      }
    }, []);

    useImperativeHandle(ref, () => ({ loadWord }), [loadWord]);

    // ── Render graph (enter / update / exit) ──────────────────────────────────
    function renderGraph() {
      if (!gRef.current || !simRef.current) return;
      const gg       = d3.select(gRef.current);
      const sim      = simRef.current;
      const allNodes = Array.from(nodes.current.values());

      // ── Background route lines ───────────────────────────────────────────────
      const lbSel = gg.select<SVGGElement>(".links-bg")
        .selectAll<SVGLineElement, SimLink>(".link-bg")
        .data(links.current, d => d.uid);

      lbSel.exit().transition().duration(250).style("opacity", 0).remove();

      lbSel.enter().append("line").attr("class", "link-bg")
        .style("opacity", 0)
        .attr("stroke",        d => REL_COLOR[d.relationType] || "#475569")
        .attr("stroke-width",  0.7)
        .attr("stroke-dasharray", d => REL_DASH[d.relationType] || "none")
        .attr("stroke-linecap", "round")
        .attr("stroke-opacity", 0.18)
        .attr("marker-end", d => `url(#ar-${d.relationType})`)
        .on("mouseenter", function(_, d) {
          d3.select(this).attr("stroke-opacity", 0.55).attr("stroke-width", 1.5);
        })
        .on("mouseleave", function() {
          d3.select(this).attr("stroke-opacity", 0.18).attr("stroke-width", 0.7);
        })
        .transition().duration(600).style("opacity", 1);

      // ── Pulse overlay lines ──────────────────────────────────────────────────
      const lpSel = gg.select<SVGGElement>(".links-pulse")
        .selectAll<SVGLineElement, SimLink>(".link-pulse")
        .data(links.current, d => d.uid);

      lpSel.exit().remove();

      lpSel.enter().append("line").attr("class", "link-pulse")
        .attr("stroke",          d => REL_COLOR[d.relationType] || "#60a5fa")
        .attr("stroke-width",    1.6)
        .attr("stroke-linecap",  "round")
        .attr("stroke-dasharray", "5 300")
        .style("animation-name",             "link-pulse")
        .style("animation-timing-function",  "linear")
        .style("animation-iteration-count",  "infinite")
        .style("animation-duration",  d => `${2.2 + (uidHash(d.uid) % 28) / 10}s`)
        .style("animation-delay",     d => `${(uidHash(d.uid.slice(3)) % 25) / 10}s`);

      // ── Nodes ────────────────────────────────────────────────────────────────
      const nSel = gg.select<SVGGElement>(".nodes")
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
          .on("end",  (ev, d) => {
            if (!ev.active) sim.alphaTarget(0);
            if (!d.isFocal) { d.fx = undefined; d.fy = undefined; }
          }))
        .on("click", (ev, d) => { ev.stopPropagation(); clickRef.current(d.wordId, d.label); });

      // Outer halo (focal only) — pulsing dashed ring
      nEnter.append("circle").attr("class", "halo")
        .attr("r",             d => radius(d) + 20)
        .attr("fill",          d => `${cfg(d).glow}12`)
        .attr("stroke",        d => cfg(d).glow)
        .attr("stroke-width",  1)
        .attr("stroke-opacity", 0.4)
        .attr("stroke-dasharray", "3 5")
        .style("animation", "halo-pulse 3s ease-in-out infinite")
        .style("display", d => d.isFocal ? null : "none");

      // Ambient glow ring (focal only)
      nEnter.append("circle").attr("class", "ambient")
        .attr("r",    d => radius(d) + 10)
        .attr("fill", d => `${cfg(d).glow}18`)
        .attr("stroke", "none")
        .style("display", d => d.isFocal ? null : "none");

      // Core circle — the planet/star
      nEnter.append("circle").attr("class", "shape")
        .attr("r",            d => radius(d))
        .attr("fill",         d => cfg(d).fill)
        .attr("stroke",       d => cfg(d).stroke)
        .attr("stroke-width", d => d.isFocal ? 2.5 : 1.2)
        .attr("filter",       d => d.isFocal ? "url(#strongGlow)" : "url(#glow)");

      // Specular shine dot (top-left)
      nEnter.append("circle").attr("class", "shine")
        .attr("r",    d => radius(d) * 0.28)
        .attr("cx",   d => -radius(d) * 0.22)
        .attr("cy",   d => -radius(d) * 0.22)
        .attr("fill", "rgba(255,255,255,0.28)")
        .style("pointer-events", "none");

      // Hover
      nEnter.merge(nSel)
        .on("mouseenter", function(_, d) {
          d3.select(this).select<SVGCircleElement>(".shape")
            .attr("stroke-width", 3).attr("stroke", "#fff")
            .attr("r", radius(d) * 1.12);
          d3.select(this).raise();
        })
        .on("mouseleave", function(_, d) {
          d3.select(this).select<SVGCircleElement>(".shape")
            .attr("stroke-width", d.isFocal ? 2.5 : 1.2)
            .attr("stroke", cfg(d).stroke)
            .attr("r", radius(d));
        });

      nEnter.transition().duration(700)
        .delay((_, i) => Math.min(i * 20, 350))
        .style("opacity", 1);

      // Update existing nodes' focal state
      nSel.select(".halo")   .style("display", d => d.isFocal ? null : "none");
      nSel.select(".ambient").style("display", d => d.isFocal ? null : "none");
      nSel.select<SVGCircleElement>(".shape")
        .attr("stroke-width", d => d.isFocal ? 2.5 : 1.2)
        .attr("filter",       d => d.isFocal ? "url(#strongGlow)" : "url(#glow)");

      // ── Labels ───────────────────────────────────────────────────────────────
      const lblSel = gg.select<SVGGElement>(".labels")
        .selectAll<SVGTextElement, SimNode>(".lbl")
        .data(allNodes, d => d.id);

      lblSel.exit().remove();

      const lblEnter = lblSel.enter().append("text").attr("class", "lbl")
        .attr("text-anchor", "middle")
        .style("font-family", "'NRT', system-ui, sans-serif")
        .style("pointer-events", "none")
        .attr("paint-order", "stroke")
        // stroke color handled by .lbl CSS class (theme-aware)
        .text(d => d.label);

      lblEnter.merge(lblSel)
        .style("font-size",   d => `${d.isFocal ? 15 : 11}px`)
        .style("font-weight", d => d.isFocal ? "700" : "500")
        .style("fill",        d => d.isFocal ? "var(--t-text-1)" : "var(--t-text-3)")
        .attr("stroke-width", d => d.isFocal ? "4px" : "3px");
    }

    return (
      <svg ref={svgRef} className="absolute inset-0 w-full h-full"
        style={{ background: "transparent" }} />
    );
  }
);

export default UniverseGraph;
