"use client";

// Lightweight, lazy-loading SVG mini-map for word cards.
// Uses IntersectionObserver — only fetches graph data when card enters viewport.
// No D3 force simulation: pure radial math layout.

import { useRef, useEffect, useState } from "react";

const W = 180, H = 104, CX = 90, CY = 52;
const R_ORBIT = 38, R_CENTER = 8, R_SAT = 4;

export const REL5_COLOR: Record<string, string> = {
  synonym:    "#22d3ee",
  antonym:    "#f87171",
  partof:     "#fb923c",
  related:    "#60a5fa",
  contextual: "#a78bfa",
  usage:      "#a78bfa",
  example:    "#fbbf24",
};

interface RawNode { id: string; label: string; isCenter: boolean; relationType?: string; }
interface RawLink { source: string; target: string; relationType: string; }
interface GraphRes { nodes: RawNode[]; links: RawLink[]; }

export default function MiniMap({ wordId }: { wordId: number }) {
  const divRef = useRef<HTMLDivElement>(null);
  const [res,    setRes]    = useState<GraphRes | null>(null);
  const [status, setStatus] = useState<"idle" | "loading" | "done" | "error">("idle");

  useEffect(() => {
    const el = divRef.current;
    if (!el) return;
    const obs = new IntersectionObserver(
      entries => {
        if (!entries[0].isIntersecting) return;
        obs.disconnect();
        setStatus("loading");
        fetch(`/api/words/${wordId}/graph`)
          .then(r => r.ok ? (r.json() as Promise<GraphRes>) : null)
          .then(d => { if (d) { setRes(d); setStatus("done"); } else setStatus("error"); })
          .catch(() => setStatus("error"));
      },
      { rootMargin: "400px", threshold: 0 }
    );
    obs.observe(el);
    return () => obs.disconnect();
  }, [wordId]);

  const center  = res?.nodes.find(n => n.isCenter);
  const sats    = (res?.nodes.filter(n => !n.isCenter) ?? []).slice(0, 7);
  const satPos  = sats.map((n, i) => {
    const angle = (2 * Math.PI * i / Math.max(sats.length, 1)) - Math.PI / 2;
    return { node: n, x: CX + R_ORBIT * Math.cos(angle), y: CY + R_ORBIT * Math.sin(angle) };
  });

  const posMap = new Map<string, { x: number; y: number }>();
  if (center) posMap.set(center.id, { x: CX, y: CY });
  satPos.forEach(({ node, x, y }) => posMap.set(node.id, { x, y }));

  return (
    <div ref={divRef} style={{ width: W, height: H, position: "relative" }}>

      {status === "done" && center && (
        <svg width={W} height={H} style={{ display: "block", overflow: "visible" }}>
          {/* Links */}
          {res!.links.map((l, i) => {
            const s = posMap.get(l.source);
            const t = posMap.get(l.target);
            if (!s || !t) return null;
            const col = REL5_COLOR[l.relationType] ?? "#475569";
            return (
              <line key={i}
                x1={s.x} y1={s.y} x2={t.x} y2={t.y}
                stroke={col} strokeWidth={0.85} strokeOpacity={0.5} />
            );
          })}

          {/* Satellite nodes */}
          {satPos.map(({ node, x, y }) => {
            const relLink = res!.links.find(l =>
              (l.source === center.id && l.target === node.id) ||
              (l.target === center.id && l.source === node.id)
            );
            const col = REL5_COLOR[relLink?.relationType ?? node.relationType ?? "related"] ?? "#60a5fa";
            return (
              <g key={node.id}>
                <circle cx={x} cy={y} r={R_SAT}
                  fill={`${col}28`} stroke={col} strokeWidth={0.7} />
                <text x={x} y={y - R_SAT - 2.5}
                  textAnchor="middle" fontSize={5.5} fill={col}
                  style={{ fontFamily: "'NRT',system-ui,sans-serif" }}>
                  {node.label.length > 7 ? node.label.slice(0, 6) + "…" : node.label}
                </text>
              </g>
            );
          })}

          {/* Center node */}
          <circle cx={CX} cy={CY} r={R_CENTER + 4}
            fill="rgba(6,182,212,.10)" stroke="none" />
          <circle cx={CX} cy={CY} r={R_CENTER}
            fill="#06b6d4" stroke="rgba(255,255,255,.45)" strokeWidth={1.2} />
          <circle cx={CX - 2} cy={CY - 2} r={2.5}
            fill="rgba(255,255,255,.28)" />
        </svg>
      )}

      {(status === "idle" || status === "loading") && (
        <div className="absolute inset-0 flex items-center justify-center">
          <div className="w-3 h-3 rounded-full border border-cyan-500/30 border-t-cyan-500 animate-spin" />
        </div>
      )}
    </div>
  );
}
