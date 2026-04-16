"use client";

import { useEffect, useRef, useCallback, useState } from "react";
import * as d3 from "d3";

// ── Types ──────────────────────────────────────────────────────────────────
interface GraphNode extends d3.SimulationNodeDatum {
  id: string;
  label: string;
  meaning?: string;
  category?: string;
  isCenter: boolean;
  weight: number;
  color?: string;
  relationType?: string;
}

interface GraphLink extends d3.SimulationLinkDatum<GraphNode> {
  source: string | GraphNode;
  target: string | GraphNode;
  relationType: string;
  weight: number;
  isIncoming: boolean;
}

interface GraphData {
  nodes: GraphNode[];
  links: GraphLink[];
}

// ── Relation colour map ────────────────────────────────────────────────────
const RTYPE: Record<string, { stroke: string; fill: string; label: string }> = {
  synonym:    { stroke: "#22d36d", fill: "rgba(34,211,109,0.12)",  label: "هاوماناکە" },
  antonym:    { stroke: "#f87171", fill: "rgba(248,113,113,0.12)", label: "دژەوشە" },
  related:    { stroke: "#60a5fa", fill: "rgba(96,165,250,0.12)",  label: "پەیوەندیدار" },
  example:    { stroke: "#fbbf24", fill: "rgba(251,191,36,0.12)",  label: "نموونە" },
  usage:      { stroke: "#a78bfa", fill: "rgba(167,139,250,0.12)", label: "بکارهێنان" },
  contextual: { stroke: "#34d4f0", fill: "rgba(52,212,240,0.12)",  label: "چارچێوەیی" },
};
const FB = { stroke: "#94a3b8", fill: "rgba(148,163,184,0.10)", label: "other" };

function rc(type: string) {
  return RTYPE[type] ?? FB;
}

// Flat-top hex path around origin
function hexPath(r: number): string {
  return Array.from({ length: 6 }, (_, i) => {
    const a = (Math.PI / 3) * i;
    return `${(r * Math.cos(a)).toFixed(2)},${(r * Math.sin(a)).toFixed(2)}`;
  })
    .map((p, i) => (i === 0 ? `M${p}` : `L${p}`))
    .join(" ") + "Z";
}

const FONT = "'NRT', system-ui, sans-serif";

// ── Props ──────────────────────────────────────────────────────────────────
interface MindMapProps {
  rootWordId: number;
  onNodeAdd?: (wordId: number) => void;
  height?: number;
}

// ── Component ──────────────────────────────────────────────────────────────
export default function MindMap({ rootWordId, onNodeAdd, height = 560 }: MindMapProps) {
  const svgRef = useRef<SVGSVGElement>(null);
  const simRef  = useRef<d3.Simulation<GraphNode, GraphLink> | null>(null);
  const gRef    = useRef<d3.Selection<SVGGElement, unknown, null, undefined> | null>(null);

  const [graphData, setGraphData] = useState<GraphData | null>(null);
  const [loading,   setLoading]   = useState(true);
  const [expanded,  setExpanded]  = useState<Set<string>>(new Set([String(rootWordId)]));

  // ── Fetch graph data ────────────────────────────────────────────────────
  const fetchGraph = useCallback(async (wordId: number): Promise<GraphData | null> => {
    try {
      const res = await fetch(`/api/words/${wordId}/graph`);
      if (!res.ok) return null;
      return res.json() as Promise<GraphData>;
    } catch { return null; }
  }, []);

  // Initial load
  useEffect(() => {
    setLoading(true);
    fetchGraph(rootWordId).then((d) => {
      if (d) setGraphData(d);
      setLoading(false);
    });
  }, [rootWordId, fetchGraph]);

  // ── Expand a node (double-click) ────────────────────────────────────────
  const expandNode = useCallback(async (nodeId: string) => {
    const wordId = parseInt(nodeId, 10);
    if (isNaN(wordId) || expanded.has(nodeId)) return;
    setExpanded((prev) => new Set([...prev, nodeId]));

    const sub = await fetchGraph(wordId);
    if (!sub) return;

    setGraphData((prev) => {
      if (!prev) return prev;
      const existIds = new Set(prev.nodes.map((n) => n.id));
      const newNodes = sub.nodes.filter((n) => !existIds.has(n.id));

      type LinkKey = string;
      const existLinks = new Set<LinkKey>(
        prev.links.map((l) => {
          const s = typeof l.source === "string" ? l.source : (l.source as GraphNode).id;
          const t = typeof l.target === "string" ? l.target : (l.target as GraphNode).id;
          return `${s}→${t}`;
        })
      );
      const newLinks = sub.links.filter((l) => {
        const s = typeof l.source === "string" ? l.source : (l.source as GraphNode).id;
        const t = typeof l.target === "string" ? l.target : (l.target as GraphNode).id;
        return !existLinks.has(`${s}→${t}`);
      });

      return {
        nodes: [...prev.nodes, ...newNodes],
        links: [...prev.links, ...newLinks],
      };
    });
  }, [expanded, fetchGraph]);

  // ── Build / rebuild D3 graph ────────────────────────────────────────────
  useEffect(() => {
    if (!graphData || !svgRef.current) return;

    const el   = svgRef.current;
    const W    = el.clientWidth  || 800;
    const H    = height;

    // Stop any running simulation
    simRef.current?.stop();

    const svg = d3.select(el);
    svg.selectAll("*").remove();

    // ── Defs ──────────────────────────────────────────────────────────────
    const defs = svg.append("defs");

    // Per-type arrow markers
    Object.entries(RTYPE).forEach(([type, c]) => {
      defs.append("marker")
        .attr("id", `mm-arrow-${type}`)
        .attr("viewBox", "0 -4 8 8")
        .attr("refX", 22).attr("refY", 0)
        .attr("markerWidth", 5).attr("markerHeight", 5)
        .attr("orient", "auto")
        .append("path").attr("d", "M0,-4L8,0L0,4")
        .attr("fill", c.stroke).attr("opacity", 0.65);
    });

    // Center glow filter
    const glowF = defs.append("filter").attr("id", "mm-glow")
      .attr("x", "-60%").attr("y", "-60%").attr("width", "220%").attr("height", "220%");
    glowF.append("feGaussianBlur").attr("stdDeviation", 8).attr("result", "blur");
    const fm = glowF.append("feMerge");
    fm.append("feMergeNode").attr("in", "blur");
    fm.append("feMergeNode").attr("in", "SourceGraphic");

    // Expanded-node halo filter
    const haloF = defs.append("filter").attr("id", "mm-halo")
      .attr("x", "-40%").attr("y", "-40%").attr("width", "180%").attr("height", "180%");
    haloF.append("feGaussianBlur").attr("stdDeviation", 4).attr("result", "blur");
    const fm2 = haloF.append("feMerge");
    fm2.append("feMergeNode").attr("in", "blur");
    fm2.append("feMergeNode").attr("in", "SourceGraphic");

    // Center hex gradient
    const cg = defs.append("radialGradient").attr("id", "mm-center-grad")
      .attr("cx", "30%").attr("cy", "30%").attr("r", "70%");
    cg.append("stop").attr("offset", "0%").attr("stop-color", "#a5b4fc");
    cg.append("stop").attr("offset", "100%").attr("stop-color", "#3730a3");

    // ── Zoom / pan container ───────────────────────────────────────────────
    const g = svg.append("g").attr("class", "mm-root");
    gRef.current = g;

    svg.call(
      d3.zoom<SVGSVGElement, unknown>()
        .scaleExtent([0.25, 5])
        .on("zoom", (ev) => g.attr("transform", ev.transform.toString()))
    ).on("dblclick.zoom", null); // prevent zoom on node dblclick

    // ── Clone data so D3 can mutate freely ────────────────────────────────
    const nodes: GraphNode[] = graphData.nodes.map((n) => ({ ...n }));
    const links: GraphLink[] = graphData.links.map((l) => ({ ...l }));

    // ── Simulation ────────────────────────────────────────────────────────
    const sim = d3.forceSimulation<GraphNode>(nodes)
      .force("link",
        d3.forceLink<GraphNode, GraphLink>(links)
          .id((d) => d.id)
          .distance((l) => 90 + (l.weight as number) * 18)
          .strength(0.35)
      )
      .force("charge", d3.forceManyBody<GraphNode>().strength(-380))
      .force("center", d3.forceCenter(W / 2, H / 2))
      .force("collide", d3.forceCollide<GraphNode>().radius((d) => d.isCenter ? 60 : 32 + d.weight * 1.5))
      .alphaDecay(0.018);

    simRef.current = sim;

    // Pin center
    const center = nodes.find((n) => n.isCenter);
    if (center) { center.fx = W / 2; center.fy = H / 2; }

    // ── Links ─────────────────────────────────────────────────────────────
    const linkSel = g.append("g").attr("class", "mm-links")
      .selectAll<SVGPathElement, GraphLink>("path")
      .data(links)
      .join("path")
        .attr("fill", "none")
        .attr("stroke", (d) => rc(d.relationType).stroke)
        .attr("stroke-width", (d) => Math.max(1, 2.8 - (d.weight - 1) * 0.18))
        .attr("stroke-opacity", 0.45)
        .attr("stroke-dasharray", "8 4")
        .attr("marker-end", (d) => `url(#mm-arrow-${d.relationType})`);

    // ── Nodes ─────────────────────────────────────────────────────────────
    const nodeSel = g.append("g").attr("class", "mm-nodes")
      .selectAll<SVGGElement, GraphNode>("g.mm-node")
      .data(nodes)
      .join("g")
        .attr("class", "mm-node")
        .attr("cursor", (d) => d.isCenter ? "grab" : "pointer");

    // Center node — glowing hexagon
    nodeSel.filter((d) => d.isCenter).each(function() {
      const s = d3.select(this);
      // outer glow ring
      s.append("path")
        .attr("d", hexPath(58))
        .attr("fill", "rgba(99,102,241,0.08)")
        .attr("filter", "url(#mm-glow)");
      // main hex
      s.append("path")
        .attr("d", hexPath(48))
        .attr("fill", "url(#mm-center-grad)")
        .attr("stroke", "#6366f1")
        .attr("stroke-width", 2);
      // inner shine
      s.append("path")
        .attr("d", hexPath(40))
        .attr("fill", "none")
        .attr("stroke", "rgba(255,255,255,0.18)")
        .attr("stroke-width", 1.5);
    });

    // Outer nodes — circles
    nodeSel.filter((d) => !d.isCenter).each(function(d) {
      const s = d3.select(this);
      const r = 20 + Math.min(d.weight, 8) * 1.8;
      const c = rc(d.relationType ?? "related");
      const isExp = expanded.has(d.id);

      // Glow halo for expanded nodes
      if (isExp) {
        s.append("circle")
          .attr("r", r + 10)
          .attr("fill", c.fill)
          .attr("filter", "url(#mm-halo)");
      }

      s.append("circle")
        .attr("class", "mm-circle")
        .attr("r", r)
        .attr("fill", c.fill)
        .attr("stroke", c.stroke)
        .attr("stroke-width", isExp ? 2.5 : 1.8);

      // Pulse ring (hover, handled via CSS)
      s.append("circle")
        .attr("class", "mm-ring")
        .attr("r", r + 8)
        .attr("fill", "none")
        .attr("stroke", c.stroke)
        .attr("stroke-width", 1.5)
        .attr("opacity", 0);
    });

    // Labels for all nodes
    nodeSel.append("text")
      .attr("text-anchor", "middle")
      .attr("dominant-baseline", "middle")
      .attr("font-family", FONT)
      .attr("font-size", (d) => d.isCenter ? 13 : 10.5)
      .attr("font-weight", "700")
      .attr("fill", (d) => d.isCenter ? "white" : rc(d.relationType ?? "related").stroke)
      .attr("pointer-events", "none")
      .text((d) => d.label.length > 9 ? d.label.slice(0, 8) + "…" : d.label);

    // Meaning label below center word
    nodeSel.filter((d) => d.isCenter && !!d.meaning)
      .append("text")
        .attr("y", 17)
        .attr("text-anchor", "middle")
        .attr("dominant-baseline", "middle")
        .attr("font-family", FONT)
        .attr("font-size", 7.5)
        .attr("fill", "rgba(255,255,255,0.55)")
        .attr("pointer-events", "none")
        .text((d) => (d.meaning ?? "").slice(0, 22));

    // Tooltip
    nodeSel.filter((d) => !d.isCenter)
      .append("title")
      .text((d) => [
        d.label,
        d.meaning ? `— ${d.meaning}` : "",
        "",
        expanded.has(d.id) ? "✓ فراوانکراوە" : "دووجار کلیک بکە بۆ فراوانکردن",
        onNodeAdd ? "یەک جار کلیک = زیادکردن بە ناو کارتەکان" : ""
      ].filter(Boolean).join("\n"));

    // ── Interaction ───────────────────────────────────────────────────────
    nodeSel.filter((d) => !d.isCenter)
      .on("click", (_ev, d) => {
        const wid = parseInt(d.id, 10);
        if (!isNaN(wid) && onNodeAdd) onNodeAdd(wid);
      })
      .on("dblclick", (_ev, d) => {
        expandNode(d.id);
      })
      .on("mouseenter", function(_ev, d) {
        d3.select(this).select(".mm-ring").attr("opacity", 0.45);
        d3.select(this).select(".mm-circle").attr("stroke-width", 2.8);
        // dim unconnected links
        linkSel.attr("stroke-opacity", (l) => {
          const s = typeof l.source === "string" ? l.source : (l.source as GraphNode).id;
          const t = typeof l.target === "string" ? l.target : (l.target as GraphNode).id;
          return s === d.id || t === d.id ? 0.9 : 0.1;
        });
      })
      .on("mouseleave", function() {
        d3.select(this).select(".mm-ring").attr("opacity", 0);
        d3.select(this).select(".mm-circle").attr("stroke-width", 1.8);
        linkSel.attr("stroke-opacity", 0.45);
      });

    // Drag
    nodeSel.call(
      d3.drag<SVGGElement, GraphNode>()
        .on("start", (ev, d) => {
          if (!ev.active) sim.alphaTarget(0.3).restart();
          d.fx = d.x; d.fy = d.y;
        })
        .on("drag", (ev, d) => { d.fx = ev.x; d.fy = ev.y; })
        .on("end", (ev, d) => {
          if (!ev.active) sim.alphaTarget(0);
          if (!d.isCenter) { d.fx = null; d.fy = null; }
        })
    );

    // ── Tick ──────────────────────────────────────────────────────────────
    sim.on("tick", () => {
      // Curved paths for links
      linkSel.attr("d", (d) => {
        const s = d.source as GraphNode;
        const t = d.target as GraphNode;
        if (s.x == null || s.y == null || t.x == null || t.y == null) return "";
        const dx = t.x - s.x, dy = t.y - s.y;
        const dr = Math.hypot(dx, dy) * 1.5;
        return `M${s.x},${s.y}A${dr},${dr} 0 0,1 ${t.x},${t.y}`;
      });
      nodeSel.attr("transform", (d) => `translate(${d.x ?? 0},${d.y ?? 0})`);
    });

    return () => { sim.stop(); };
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [graphData, height]);

  return (
    <div className="relative w-full overflow-hidden rounded-2xl"
      style={{
        height,
        background: "rgba(8,12,26,0.97)",
        border: "1px solid rgba(99,102,241,0.2)",
        boxShadow: "0 0 60px rgba(99,102,241,0.08) inset",
      }}>

      {/* Loading spinner */}
      {loading && (
        <div className="absolute inset-0 flex items-center justify-center z-20">
          <div className="w-12 h-12 rounded-full border-2 border-indigo-500/20 border-t-indigo-400 animate-spin" />
        </div>
      )}

      {/* Top hint */}
      <div className="absolute top-3 right-4 text-[10px] text-slate-600 z-10 select-none"
           style={{ fontFamily: FONT }}>
        دووجار کلیک = فراوانکردن &nbsp;·&nbsp; کێشان = جوڵاندن &nbsp;·&nbsp; سکرۆڵ = زووم
      </div>

      {/* Legend */}
      <div className="absolute bottom-3 left-4 flex flex-wrap gap-x-3 gap-y-1 max-w-[220px] z-10">
        {Object.entries(RTYPE).map(([type, c]) => (
          <span key={type} className="flex items-center gap-1 text-[9px]"
                style={{ color: c.stroke, fontFamily: FONT }}>
            <span className="inline-block w-2 h-2 rounded-full" style={{ background: c.stroke }} />
            {c.label}
          </span>
        ))}
      </div>

      {/* SVG canvas */}
      <svg
        ref={svgRef}
        className="w-full h-full"
        style={{ background: "transparent" }}
      >
        <style>{`
          @keyframes mm-dash {
            from { stroke-dashoffset: 24; }
            to   { stroke-dashoffset: 0;  }
          }
          .mm-links path {
            animation: mm-dash 2.4s linear infinite;
          }
        `}</style>
      </svg>
    </div>
  );
}
