"use client";

import { useRef, useState, useEffect, useCallback } from "react";
import dynamic from "next/dynamic";
import type { UniverseGraphHandle } from "./UniverseGraph";

const UniverseGraph = dynamic(() => import("./UniverseGraph"), { ssr: false });

// ── Types ─────────────────────────────────────────────────────────────────────

interface Meaning { id: number; meaning: string; locate?: string; }

interface FocalWord {
  id: number;
  kurdish: string;
  speechPaneKurdish?: string;
  category?: string;
  description?: string;
  meanings: Meaning[];
}

interface SearchHit {
  id: number;
  kurdish: string;
  speechPane: number;
  speechPaneKurdish?: string;
  category?: string;
  totalRelations: number;
  meanings: Meaning[];
}

// ── Legend data ───────────────────────────────────────────────────────────────

const REL_LEGEND = [
  { type: "synonym",    label: "هاوماناکە",  color: "#22d36d", dash: "none" },
  { type: "antonym",    label: "دژەوشە",     color: "#f87171", dash: "8,4" },
  { type: "related",    label: "پەیوەندیدار", color: "#60a5fa", dash: "none" },
  { type: "example",    label: "نموونە",     color: "#fbbf24", dash: "3,5" },
  { type: "usage",      label: "بکارهێنان",  color: "#a78bfa", dash: "10,3,2,3" },
  { type: "contextual", label: "چارچێوەیی",  color: "#34d4f0", dash: "18,5" },
  { type: "partof",     label: "بەشێکە لە",  color: "#fb923c", dash: "6,3,1,3" },
];

const SHAPE_LEGEND = [
  { label: "ناو",       color: "#6366f1", shape: "●" },
  { label: "کریا",      color: "#ec4899", shape: "◆" },
  { label: "ناوستانی",  color: "#14b8a6", shape: "⬡" },
  { label: "ئاوەڵ",    color: "#f59e0b", shape: "▲" },
  { label: "دەستەواژە", color: "#8b5cf6", shape: "⬠" },
  { label: "تر",        color: "#475569", shape: "●" },
];

// ── Stars (deterministic, no random flicker on SSR) ───────────────────────────

const STARS = Array.from({ length: 130 }, (_, i) => {
  const s = i * 137.508;
  return {
    cx: `${((s * 1.618) % 100).toFixed(1)}%`,
    cy: `${((s * 2.414) % 100).toFixed(1)}%`,
    r:  (0.5 + (i % 5) * 0.25).toFixed(2),
    op: (0.06 + (i % 9) * 0.05).toFixed(2),
    dur: `${2.5 + (i % 6) * 0.8}s`,
    del: `${((i * 0.35) % 5).toFixed(1)}s`,
  };
});

// ── Component ─────────────────────────────────────────────────────────────────

export default function UniversePage() {
  const graphRef = useRef<UniverseGraphHandle>(null);

  const [ready,   setReady]   = useState(false);    // first word loaded
  const [loading, setLoading] = useState(false);
  const [query,   setQuery]   = useState("");
  const [hits,    setHits]    = useState<SearchHit[]>([]);
  const [searching, setSearching] = useState(false);
  const [focal,   setFocal]   = useState<FocalWord | null>(null);
  const [focalScrn, setFocalScrn] = useState({ x: 0, y: 0 });
  const [showLeg, setShowLeg] = useState(false);

  const abortRef = useRef<AbortController | null>(null);
  const timerRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const scrThrottle = useRef(0);

  // ── Debounced search ──────────────────────────────────────────────────────
  useEffect(() => {
    if (!query.trim()) { setHits([]); return; }
    if (timerRef.current) clearTimeout(timerRef.current);
    timerRef.current = setTimeout(async () => {
      setSearching(true);
      abortRef.current?.abort();
      abortRef.current = new AbortController();
      try {
        const r = await fetch(
          `/api/words?search=${encodeURIComponent(query)}&pageSize=8`,
          { signal: abortRef.current.signal }
        );
        if (r.ok) { const d = await r.json(); setHits(d.items ?? []); }
      } catch { /* aborted */ }
      setSearching(false);
    }, 220);
    return () => { if (timerRef.current) clearTimeout(timerRef.current); };
  }, [query]);

  // ── Load word ─────────────────────────────────────────────────────────────
  const selectWord = useCallback(async (wordId: number) => {
    setLoading(true);
    setHits([]);
    setQuery("");
    try {
      const [, word] = await Promise.all([
        graphRef.current?.loadWord(wordId),
        fetch(`/api/words/${wordId}`).then(r => r.ok ? r.json() : null),
      ]);
      if (word) {
        setFocal({
          id: word.id, kurdish: word.kurdish,
          speechPaneKurdish: word.speechPaneKurdish,
          category: word.category, description: word.description,
          meanings: word.meanings ?? [],
        });
        setReady(true);
      }
    } finally {
      setLoading(false);
    }
  }, []);

  const handleNodeClick = useCallback((wordId: number) => {
    selectWord(wordId);
  }, [selectWord]);

  // Throttle focal position updates to ~20fps
  const handleFocalPos = useCallback((sx: number, sy: number) => {
    const now = Date.now();
    if (now - scrThrottle.current < 50) return;
    scrThrottle.current = now;
    setFocalScrn({ x: sx, y: sy });
  }, []);

  // ── Render ────────────────────────────────────────────────────────────────
  return (
    <div className="fixed inset-0 overflow-hidden select-none"
      style={{ background: "var(--background)" }}>

      {/* ── Starfield ──────────────────────────────────────────────────────── */}
      <div className="absolute inset-0 pointer-events-none overflow-hidden">
        {/* Nebula glows */}
        <div className="absolute inset-0" style={{ background:
          "radial-gradient(ellipse 80% 60% at 12% 55%, rgba(99,102,241,.07) 0%, transparent 60%)," +
          "radial-gradient(ellipse 65% 55% at 88% 18%, rgba(139,92,246,.05) 0%, transparent 55%)," +
          "radial-gradient(ellipse 75% 65% at 50% 92%, rgba(59,130,246,.04) 0%, transparent 60%)"
        }} />
        <svg className="absolute inset-0 w-full h-full" aria-hidden>
          {STARS.map((s, i) => (
            <circle key={i} cx={s.cx} cy={s.cy} r={s.r} fill="white" opacity={s.op}
              style={{ animation: `twinkle ${s.dur} ease-in-out ${s.del} infinite alternate` }} />
          ))}
        </svg>
      </div>

      {/* ── D3 Graph ───────────────────────────────────────────────────────── */}
      <UniverseGraph ref={graphRef} onNodeClick={handleNodeClick} onFocalPos={handleFocalPos} />

      {/* ── Landing search (center, visible before first word) ────────────── */}
      <div
        className="absolute z-20 flex flex-col items-center gap-5 transition-all duration-700 ease-out"
        style={ready ? { opacity: 0, pointerEvents: "none", top: "50%", left: "50%", transform: "translate(-50%, -50%)" }
                     : { opacity: 1, top: "50%", left: "50%", transform: "translate(-50%, -50%)", width: "min(92vw, 560px)" }}
      >
        <div className="text-center pointer-events-none">
          <h1 className="text-4xl font-black text-white leading-tight tracking-tight"
            style={{ fontFamily: "'NRT', system-ui, sans-serif", textShadow: "0 0 40px rgba(99,102,241,.6)" }}>
            فەرهەنگی کوردی
          </h1>
          <p className="text-sm text-slate-500 mt-1.5 tracking-widest uppercase">Kurdish Dictionary Universe</p>
        </div>
        <SearchBox
          value={query} onChange={setQuery}
          hits={hits} searching={searching}
          onSelect={selectWord}
          placeholder="وشەیەک بنووسە بۆ دەستپێکردن..."
        />
      </div>

      {/* ── Compact search (top-left, visible after first word) ───────────── */}
      <div
        className="absolute top-4 left-4 z-20 transition-all duration-500"
        style={{ opacity: ready ? 1 : 0, pointerEvents: ready ? "auto" : "none", width: "260px" }}
      >
        <SearchBox
          value={query} onChange={setQuery}
          hits={hits} searching={searching}
          onSelect={selectWord}
          compact
          placeholder="گەڕانی وشەی نوێ..."
        />
      </div>

      {/* ── Loading spinner ────────────────────────────────────────────────── */}
      {loading && (
        <div className="absolute inset-0 flex items-center justify-center z-30 pointer-events-none">
          <div className="w-14 h-14 rounded-full border-2 border-indigo-500/20 border-t-indigo-500 animate-spin" />
        </div>
      )}

      {/* ── Focal word panel (top-right) ───────────────────────────────────── */}
      {focal && (
        <WordPanel word={focal} />
      )}

      {/* ── Meaning cards (near focal node) ───────────────────────────────── */}
      {focal && focal.meanings.length > 0 && (
        <MeaningCards meanings={focal.meanings} pos={focalScrn} />
      )}

      {/* ── Legend ─────────────────────────────────────────────────────────── */}
      <button
        onClick={() => setShowLeg(v => !v)}
        className="absolute bottom-4 left-4 z-20 glass rounded-xl px-3 py-2 text-xs font-semibold text-slate-300 hover:text-white transition-colors flex items-center gap-2"
      >
        <span>🕸</span><span>ڕێنمایی</span>
      </button>

      {showLeg && (
        <div className="absolute bottom-14 left-4 z-20 glass-raised rounded-2xl p-4 space-y-4 w-56 pointer-events-auto"
          onClick={e => e.stopPropagation()}>
          <div>
            <p className="text-[10px] font-bold uppercase tracking-widest text-slate-500 mb-2.5">جۆری پەیوەندی</p>
            <div className="space-y-2">
              {REL_LEGEND.map(r => (
                <div key={r.type} className="flex items-center gap-2.5">
                  <svg width="28" height="8" className="shrink-0">
                    <line x1="0" y1="4" x2="28" y2="4"
                      stroke={r.color} strokeWidth="2.5"
                      strokeDasharray={r.dash === "none" ? undefined : r.dash}
                      strokeLinecap="round" />
                  </svg>
                  <span className="text-xs text-slate-300" style={{ fontFamily: "'NRT', system-ui, sans-serif" }}>
                    {r.label}
                  </span>
                </div>
              ))}
            </div>
          </div>
          <div>
            <p className="text-[10px] font-bold uppercase tracking-widest text-slate-500 mb-2.5">جۆری وشە</p>
            <div className="grid grid-cols-2 gap-1.5">
              {SHAPE_LEGEND.map(s => (
                <div key={s.label} className="flex items-center gap-1.5">
                  <span style={{ color: s.color, fontSize: "13px", lineHeight: 1 }}>{s.shape}</span>
                  <span className="text-[11px] text-slate-400" style={{ fontFamily: "'NRT', system-ui, sans-serif" }}>
                    {s.label}
                  </span>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}

      {/* ── Hint ──────────────────────────────────────────────────────────── */}
      {!ready && (
        <p className="absolute bottom-6 left-1/2 -translate-x-1/2 z-10 text-xs text-slate-600 pointer-events-none"
          style={{ animation: "pulse 2s ease-in-out infinite" }}>
          وشەیەک بگەڕێ بۆ دەستپێکردن
        </p>
      )}
    </div>
  );
}

// ── SearchBox ─────────────────────────────────────────────────────────────────

function SearchBox({
  value, onChange, hits, searching, onSelect, placeholder, compact = false,
}: {
  value: string;
  onChange(v: string): void;
  hits: SearchHit[];
  searching: boolean;
  onSelect(id: number): void;
  placeholder: string;
  compact?: boolean;
}) {
  return (
    <div className="relative w-full">
      <div className="glass-raised rounded-2xl flex items-center overflow-hidden"
        style={{ boxShadow: "0 0 40px rgba(99,102,241,.18), 0 8px 40px rgba(0,0,0,.3)" }}>
        <span className="pl-4 text-slate-500 shrink-0 text-sm">🔍</span>
        <input
          value={value}
          onChange={e => onChange(e.target.value)}
          placeholder={placeholder}
          dir="rtl"
          className={`w-full bg-transparent pr-4 pl-2 ${compact ? "py-2.5 text-sm" : "py-4 text-base"} text-white placeholder-slate-500 outline-none`}
          style={{ fontFamily: "'NRT', system-ui, sans-serif" }}
        />
      </div>

      {/* Dropdown */}
      {(hits.length > 0 || searching) && (
        <div className="absolute top-full mt-2 w-full rounded-2xl overflow-hidden z-40 shadow-2xl"
          style={{ background: "var(--t-dropdown-bg)", border: "1px solid var(--border-strong)" }}>
          {searching && !hits.length && (
            <div className="px-4 py-3 text-sm text-slate-500 text-center"
              style={{ fontFamily: "'NRT', system-ui, sans-serif" }}>
              گەڕان...
            </div>
          )}
          {hits.map(h => (
            <button key={h.id}
              onMouseDown={e => { e.preventDefault(); onSelect(h.id); }}
              className="w-full flex items-center justify-between gap-3 px-4 py-3 text-right hover:bg-white/5 transition-colors border-b border-white/5 last:border-0">
              <div className="flex-1 min-w-0">
                <p className="text-sm font-bold text-white truncate"
                  dir="rtl" style={{ fontFamily: "'NRT', system-ui, sans-serif" }}>
                  {h.kurdish}
                </p>
                {h.meanings[0] && (
                  <p className="text-xs text-slate-400 truncate mt-0.5">
                    {h.meanings[0].locate && (
                      <span className="text-indigo-400 text-[10px] font-semibold ml-1">
                        {h.meanings[0].locate}
                      </span>
                    )}
                    {h.meanings[0].meaning}
                  </p>
                )}
              </div>
              <div className="flex items-center gap-1.5 shrink-0">
                {h.speechPaneKurdish && (
                  <span className="text-[10px] px-2 py-0.5 rounded-full bg-indigo-500/20 text-indigo-300 font-semibold">
                    {h.speechPaneKurdish}
                  </span>
                )}
                {h.totalRelations > 0 && (
                  <span className="text-[10px] px-1.5 py-0.5 rounded-full bg-white/8 text-slate-500">
                    {h.totalRelations}🕸
                  </span>
                )}
              </div>
            </button>
          ))}
        </div>
      )}
    </div>
  );
}

// ── WordPanel ─────────────────────────────────────────────────────────────────

function WordPanel({ word }: { word: FocalWord }) {
  return (
    <div className="absolute top-4 right-4 z-20 glass-raised rounded-2xl p-4 w-72 pointer-events-auto"
      style={{ maxHeight: "calc(100vh - 2rem)", overflowY: "auto" }}>
      <div dir="rtl" className="space-y-3">
        {/* Header */}
        <div>
          <h2 className="text-2xl font-black text-white leading-tight"
            style={{ fontFamily: "'NRT', system-ui, sans-serif" }}>
            {word.kurdish}
          </h2>
          <div className="flex flex-wrap gap-1.5 mt-1.5">
            {word.speechPaneKurdish && (
              <span className="text-[11px] font-semibold px-2.5 py-0.5 rounded-full bg-indigo-500/20 text-indigo-300 border border-indigo-500/20">
                {word.speechPaneKurdish}
              </span>
            )}
            {word.category && (
              <span className="text-[11px] font-medium px-2.5 py-0.5 rounded-full bg-violet-500/15 text-violet-300 border border-violet-500/20">
                {word.category}
              </span>
            )}
          </div>
        </div>

        {/* Description */}
        {word.description && (
          <p className="text-xs text-slate-400 leading-relaxed">{word.description}</p>
        )}

        {/* Meanings */}
        {word.meanings.length > 0 && (
          <div className="space-y-1.5">
            <p className="text-[10px] font-bold uppercase tracking-widest text-slate-500">مانا</p>
            {word.meanings.map((m, i) => (
              <div key={i} className="rounded-xl px-3 py-2"
                style={{ background: "rgba(99,102,241,0.07)", border: "1px solid rgba(99,102,241,0.12)" }}>
                {m.locate && (
                  <span className="text-[9px] font-bold uppercase tracking-wider text-indigo-400 block mb-0.5">
                    {m.locate}
                  </span>
                )}
                <p className="text-xs text-slate-200 leading-snug"
                  style={{ fontFamily: "'NRT', system-ui, sans-serif" }}>
                  {m.meaning}
                </p>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

// ── MeaningCards ─────────────────────────────────────────────────────────────

function MeaningCards({ meanings, pos }: { meanings: Meaning[]; pos: { x: number; y: number } }) {
  const items = meanings.slice(0, 4);
  const W = typeof window !== "undefined" ? window.innerWidth  : 1280;
  const H = typeof window !== "undefined" ? window.innerHeight : 800;

  return (
    <div className="absolute inset-0 pointer-events-none z-10 overflow-hidden">
      {items.map((m, i) => {
        // Fan cards in a semicircle below the focal node
        const total = items.length;
        const angle = (Math.PI * 0.5) + (i - (total - 1) / 2) * (Math.PI / Math.max(total, 3));
        const dist  = 145;
        const rawX  = pos.x + Math.cos(angle) * dist;
        const rawY  = pos.y + Math.sin(angle) * dist;
        const cardX = Math.max(8, Math.min(W - 215, rawX - 100));
        const cardY = Math.max(8, Math.min(H - 80,  rawY - 30));

        return (
          <div key={i} className="absolute" style={{
            left: cardX, top: cardY,
            transition: "left 0.25s ease-out, top 0.25s ease-out",
          }}>
            <div className="rounded-xl px-3 py-2 text-center shadow-xl"
              style={{
                background: "rgba(8,12,26,.72)",
                backdropFilter: "blur(14px)",
                border: "1px solid rgba(99,102,241,.2)",
                minWidth: "110px", maxWidth: "195px",
              }}>
              {m.locate && (
                <p className="text-[9px] font-bold uppercase tracking-wider text-indigo-400 mb-0.5">
                  {m.locate}
                </p>
              )}
              <p className="text-xs text-slate-200 leading-snug"
                dir="rtl" style={{ fontFamily: "'NRT', system-ui, sans-serif" }}>
                {m.meaning}
              </p>
            </div>
          </div>
        );
      })}
    </div>
  );
}
