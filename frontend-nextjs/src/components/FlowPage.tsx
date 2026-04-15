"use client";

import { useState, useEffect, useMemo, useCallback, useRef } from "react";

// ── Types ──────────────────────────────────────────────────────────────
interface RelDto {
  id: number;
  relatedWordId: number;
  relatedKurdish?: string;
  relationType: string;
  isIncoming: boolean;
  weight: number;
}
interface Word {
  id: number;
  kurdish: string;
  meaning?: string;
  category?: string;
  outgoingRelations: RelDto[];
  incomingRelations: RelDto[];
}

// ── Colours ────────────────────────────────────────────────────────────
const RTYPE: Record<string, { fill: string; stroke: string; text: string }> = {
  synonym: { fill: "#d1fae5", stroke: "#10b981", text: "#065f46" },
  antonym: { fill: "#fee2e2", stroke: "#ef4444", text: "#991b1b" },
  related: { fill: "#dbeafe", stroke: "#3b82f6", text: "#1e3a8a" },
  example: { fill: "#fef3c7", stroke: "#f59e0b", text: "#78350f" },
};
const FB = { fill: "#f3f4f6", stroke: "#9ca3af", text: "#374151" };

// ── Layout ─────────────────────────────────────────────────────────────
const CX = 600, CY = 500;     // SVG centre
const R0 = 66;                 // diamond half-diagonal
const L1_MIN = 148, L1_MAX = 315;  // level-1 radius from centre by weight
const L2_MIN = 118, L2_MAX = 200;  // level-2 distance from parent by weight
const NR1 = 46, NR2 = 32;         // node circle radii
const MAX_L2 = 5;                  // max level-2 children per level-1 node

function r1(w: number) { return L1_MIN + ((w - 1) / 9) * (L1_MAX - L1_MIN); }
function r2(w: number) { return L2_MIN + ((w - 1) / 9) * (L2_MAX - L2_MIN); }
function lw1(w: number) { return Math.max(1.5, 5.5 - (w - 1) * 0.45); }
function lw2(w: number) { return Math.max(1,   3.2 - (w - 1) * 0.24); }
function clip(s: string | undefined, n: number) {
  if (!s) return "?";
  return s.length > n ? s.slice(0, n - 1) + "…" : s;
}

// ── Graph node / edge ──────────────────────────────────────────────────
interface GNode {
  key: string; wordId: number; label: string; meaning?: string;
  level: 1 | 2; x: number; y: number; nr: number; weight: number;
  fill: string; stroke: string; tc: string;
}
interface GEdge {
  key: string; x1: number; y1: number; x2: number; y2: number;
  stroke: string; w: number;
}

export default function FlowPage({ initialWord }: { initialWord: Word | null }) {
  const [main,    setMain   ] = useState<Word | null>(initialWord);
  const [l2Map,   setL2Map  ] = useState<Map<number, Word>>(new Map());
  const [history, setHistory] = useState<Word[]>([]);
  const [navBusy, setNavBusy] = useState(false);
  const [l2Busy,  setL2Busy ] = useState(false);

  // search
  const [query,    setQuery   ] = useState("");
  const [results,  setResults ] = useState<Word[]>([]);
  const [showDrop, setShowDrop] = useState(false);
  const debRef   = useRef<ReturnType<typeof setTimeout> | null>(null);
  const dropRef  = useRef<HTMLDivElement>(null);

  // ── Fetch level-2 words whenever main changes ──────────────────────
  useEffect(() => {
    if (!main) { setL2Map(new Map()); return; }
    setL2Map(new Map());
    const rels = [...(main.outgoingRelations ?? []), ...(main.incomingRelations ?? [])];
    if (!rels.length) return;
    setL2Busy(true);
    Promise.allSettled(
      rels.map(r =>
        fetch(`/api/words/${r.relatedWordId}`)
          .then(res => (res.ok ? res.json() as Promise<Word> : null))
          .catch(() => null)
      )
    ).then(settled => {
      const m = new Map<number, Word>();
      settled.forEach(s => {
        if (s.status === "fulfilled" && s.value) m.set(s.value.id, s.value);
      });
      setL2Map(m);
      setL2Busy(false);
    });
  }, [main]);

  // ── Build graph from main + l2Map ──────────────────────────────────
  const { nodes, edges } = useMemo<{ nodes: GNode[]; edges: GEdge[] }>(() => {
    if (!main) return { nodes: [], edges: [] };
    const nodes: GNode[] = [];
    const edges: GEdge[] = [];

    const l1Rels = [
      ...(main.outgoingRelations ?? []),
      ...(main.incomingRelations ?? []),
    ];
    const N = l1Rels.length;

    l1Rels.forEach((rel, i) => {
      const ang = (2 * Math.PI * i) / Math.max(N, 1) - Math.PI / 2;
      const rad = r1(rel.weight);
      const nx  = CX + rad * Math.cos(ang);
      const ny  = CY + rad * Math.sin(ang);
      const c   = RTYPE[rel.relationType] ?? FB;
      const k1  = `l1-${rel.id}`;

      nodes.push({ key: k1, wordId: rel.relatedWordId, label: rel.relatedKurdish ?? "?",
        level: 1, x: nx, y: ny, nr: NR1, weight: rel.weight,
        fill: c.fill, stroke: c.stroke, tc: c.text });

      edges.push({ key: `e-${k1}`, x1: CX, y1: CY, x2: nx, y2: ny,
        stroke: c.stroke, w: lw1(rel.weight) });

      // ── Level-2 ──────────────────────────────────────────────────
      const l2Word = l2Map.get(rel.relatedWordId);
      if (!l2Word) return;

      const l2Rels = [
        ...(l2Word.outgoingRelations ?? []),
        ...(l2Word.incomingRelations ?? []),
      ]
        .filter(r2 => r2.relatedWordId !== main.id)
        .sort((a, b) => a.weight - b.weight)
        .slice(0, MAX_L2);

      const M = l2Rels.length;
      const spread = Math.min(Math.PI * 0.6, M * 0.4);

      l2Rels.forEach((r2rel, j) => {
        const childAng =
          M === 1 ? ang : ang + (j - (M - 1) / 2) * (spread / Math.max(M - 1, 1));
        const dist = r2(r2rel.weight);
        const px   = nx + dist * Math.cos(childAng);
        const py   = ny + dist * Math.sin(childAng);
        const c2   = RTYPE[r2rel.relationType] ?? FB;
        const k2   = `l2-${rel.id}-${r2rel.id}`;

        nodes.push({ key: k2, wordId: r2rel.relatedWordId, label: r2rel.relatedKurdish ?? "?",
          level: 2, x: px, y: py, nr: NR2, weight: r2rel.weight,
          fill: c2.fill, stroke: c2.stroke, tc: c2.text });

        edges.push({ key: `e-${k2}`, x1: nx, y1: ny, x2: px, y2: py,
          stroke: c2.stroke, w: lw2(r2rel.weight) });
      });
    });

    return { nodes, edges };
  }, [main, l2Map]);

  // ── Navigation ─────────────────────────────────────────────────────
  const navigateTo = useCallback(async (wordId: number) => {
    if (navBusy || wordId === main?.id) return;
    setNavBusy(true);
    try {
      const res = await fetch(`/api/words/${wordId}`);
      if (!res.ok) return;
      const next: Word = await res.json();
      if (main) setHistory(h => [...h, main]);
      setMain(next);
      setQuery(next.kurdish);
    } finally {
      setNavBusy(false);
    }
  }, [main, navBusy]);

  const goBack = useCallback(() => {
    setHistory(h => {
      const copy = [...h];
      const prev = copy.pop();
      if (prev) { setMain(prev); setQuery(prev.kurdish); }
      return copy;
    });
  }, []);

  // ── Search autocomplete ────────────────────────────────────────────
  const onSearchInput = (v: string) => {
    setQuery(v);
    setShowDrop(true);
    if (debRef.current) clearTimeout(debRef.current);
    if (!v.trim()) { setResults([]); return; }
    debRef.current = setTimeout(async () => {
      try {
        const res = await fetch(`/api/words?search=${encodeURIComponent(v)}&pageSize=10`);
        const d = await res.json();
        setResults(d.items ?? []);
      } catch { setResults([]); }
    }, 240);
  };

  const pickResult = (w: Word) => {
    setMain(w);
    setHistory([]);
    setQuery(w.kurdish);
    setShowDrop(false);
  };

  // close dropdown on outside click
  useEffect(() => {
    const h = (e: MouseEvent) => {
      if (dropRef.current && !dropRef.current.contains(e.target as Node))
        setShowDrop(false);
    };
    document.addEventListener("mousedown", h);
    return () => document.removeEventListener("mousedown", h);
  }, []);

  const busy = navBusy || l2Busy;

  // ── Render ─────────────────────────────────────────────────────────
  return (
    <div className="h-screen flex flex-col bg-gray-50 overflow-hidden">

      {/* ── Top bar ──────────────────────────────────────────────── */}
      <div className="bg-white border-b border-gray-100 shadow-sm px-4 py-2.5 flex items-center gap-3 z-20">

        {/* Back */}
        {history.length > 0 && (
          <button onClick={goBack}
            className="shrink-0 flex items-center gap-1 text-sm font-semibold text-indigo-500 hover:text-indigo-700 transition whitespace-nowrap">
            ← {clip(history[history.length - 1].kurdish, 12)}
          </button>
        )}

        {/* Search */}
        <div ref={dropRef} className="relative flex-1 max-w-lg mx-auto">
          <span className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-300 text-base pointer-events-none">🔍</span>
          <input
            type="text" value={query}
            onChange={e => onSearchInput(e.target.value)}
            onFocus={() => results.length > 0 && setShowDrop(true)}
            placeholder="گەڕان... Search"
            dir="rtl"
            className="w-full bg-gray-50 border border-gray-200 rounded-xl pl-4 pr-9 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:bg-white transition"
          />
          {showDrop && results.length > 0 && (
            <ul className="absolute z-50 top-full mt-1 left-0 right-0 bg-white border border-gray-200 rounded-xl shadow-xl overflow-hidden max-h-72 overflow-y-auto">
              {results.map(w => (
                <li key={w.id} onMouseDown={() => pickResult(w)}
                  className="px-4 py-2.5 hover:bg-indigo-50 cursor-pointer flex items-center justify-between gap-3">
                  <span className="font-bold text-gray-800 text-sm" dir="rtl">{w.kurdish}</span>
                  {w.meaning && (
                    <span className="text-xs text-gray-400 truncate max-w-[160px]" dir="rtl">{w.meaning}</span>
                  )}
                </li>
              ))}
            </ul>
          )}
        </div>

        {/* Current word badge */}
        {main && (
          <div className="shrink-0 hidden sm:flex flex-col items-end leading-tight">
            <span className="text-sm font-extrabold text-gray-800" dir="rtl">{main.kurdish}</span>
            {main.meaning && (
              <span className="text-xs text-gray-400 max-w-[140px] truncate" dir="rtl">{main.meaning}</span>
            )}
          </div>
        )}

        {/* Spinner */}
        {busy && (
          <div className="shrink-0 w-5 h-5 border-2 border-indigo-400 border-t-transparent rounded-full animate-spin" />
        )}

        {/* Node count */}
        {main && (
          <span className="shrink-0 hidden md:block text-xs text-gray-400 whitespace-nowrap">
            {nodes.length} node · {history.length > 0 && `${history.length} history`}
          </span>
        )}
      </div>

      {/* ── SVG Graph ────────────────────────────────────────────── */}
      {main ? (
        <div className="flex-1 overflow-hidden">
          <svg viewBox="0 0 1200 1000" width="100%" height="100%" className="w-full h-full">

            {/* edges */}
            {edges.map(e => (
              <line key={e.key}
                x1={e.x1} y1={e.y1} x2={e.x2} y2={e.y2}
                stroke={e.stroke} strokeWidth={e.w}
                strokeLinecap="round" opacity={0.4}
              />
            ))}

            {/* level-2 nodes (render first so level-1 is on top) */}
            {nodes.filter(n => n.level === 2).map(n => (
              <g key={n.key} onClick={() => navigateTo(n.wordId)}
                className="cursor-pointer" role="button" aria-label={n.label}>
                {/* invisible larger hit area */}
                <circle cx={n.x} cy={n.y} r={n.nr + 8} fill="transparent" />
                <circle cx={n.x} cy={n.y} r={n.nr}
                  fill={n.fill} stroke={n.stroke} strokeWidth="1.5" opacity={0.82} />
                <text x={n.x} y={n.y} textAnchor="middle" dominantBaseline="middle"
                  fill={n.tc} fontSize="10" fontWeight="600"
                  className="pointer-events-none select-none">
                  {clip(n.label, 8)}
                </text>
              </g>
            ))}

            {/* level-1 nodes */}
            {nodes.filter(n => n.level === 1).map(n => {
              const ringR   = n.nr - 9;
              const ringC   = 2 * Math.PI * ringR;
              const ringDash = (n.weight / 10) * ringC;
              return (
                <g key={n.key} onClick={() => navigateTo(n.wordId)}
                  className="cursor-pointer group" role="button" aria-label={n.label}>
                  <circle cx={n.x} cy={n.y} r={n.nr + 10} fill="transparent" />
                  <circle cx={n.x} cy={n.y} r={n.nr}
                    fill={n.fill} stroke={n.stroke} strokeWidth="2.5"
                    className="group-hover:opacity-90 transition" />
                  {/* weight arc */}
                  <circle cx={n.x} cy={n.y} r={ringR}
                    fill="none" stroke={n.stroke} strokeWidth="2.5" opacity={0.3}
                    strokeDasharray={`${ringDash} ${ringC}`}
                    transform={`rotate(-90 ${n.x} ${n.y})`} />
                  <text x={n.x} y={n.y - 5} textAnchor="middle" dominantBaseline="middle"
                    fill={n.tc} fontSize="12.5" fontWeight="800"
                    className="pointer-events-none select-none">
                    {clip(n.label, 9)}
                  </text>
                  <text x={n.x} y={n.y + 12} textAnchor="middle" dominantBaseline="middle"
                    fill={n.tc} fontSize="9" opacity={0.55}
                    className="pointer-events-none select-none">
                    ⚖ {n.weight}
                  </text>
                </g>
              );
            })}

            {/* centre diamond */}
            <polygon
              points={`${CX},${CY - R0} ${CX + R0},${CY} ${CX},${CY + R0} ${CX - R0},${CY}`}
              fill={navBusy ? "#a5b4fc" : "#6366f1"}
              stroke="#4338ca" strokeWidth="3"
              className="transition-colors duration-200"
            />
            <text x={CX} y={CY - 10} textAnchor="middle" dominantBaseline="middle"
              fill="white" fontSize="16" fontWeight="900">
              {clip(main.kurdish, 12)}
            </text>
            {main.meaning && (
              <text x={CX} y={CY + 14} textAnchor="middle" dominantBaseline="middle"
                fill="white" fontSize="9.5" opacity={0.8}>
                {clip(main.meaning, 18)}
              </text>
            )}

            {/* corner stats */}
            <text x={1185} y={990} textAnchor="end" fill="#e5e7eb" fontSize="11">
              {nodes.length} nodes · {edges.length} edges
            </text>
          </svg>
        </div>
      ) : (
        <div className="flex-1 flex flex-col items-center justify-center gap-3 text-gray-300 select-none">
          <span className="text-7xl">🌐</span>
          <p className="text-xl font-semibold text-gray-400">وشەیەک بگەڕێ بۆ دەستپێکردن</p>
          <p className="text-sm text-gray-300">Search a word to begin</p>
        </div>
      )}

      {/* ── Legend ───────────────────────────────────────────────── */}
      <div className="bg-white border-t border-gray-100 px-4 py-2 flex flex-wrap justify-center items-center gap-4">
        {Object.entries(RTYPE).map(([type, c]) => (
          <span key={type} className="flex items-center gap-1.5 text-xs text-gray-500">
            <span className="w-3 h-3 rounded-full border-2"
              style={{ background: c.fill, borderColor: c.stroke }} />
            {type}
          </span>
        ))}
        <span className="text-xs text-gray-400 border-l border-gray-200 pl-4">
          ⚖ weight 1 = نزیک · 10 = دوور
        </span>
        <span className="text-xs text-gray-400">
          کلیک لەسەر هەر وشەیەک = گشت بوون بۆ ئەو وشەیە
        </span>
      </div>
    </div>
  );
}
