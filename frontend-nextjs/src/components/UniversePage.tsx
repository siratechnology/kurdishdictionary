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

interface WordStar {
  id: number;
  kurdish: string;
  x: number;
  y: number;
  opacity: number;
  size: number;
  animDur: string;
  animDel: string;
}

// ── Legend data ───────────────────────────────────────────────────────────────

const REL_LEGEND = [
  { type: "synonym",    label: "هاوماناکە",   color: "#22d3ee", dash: "none" },
  { type: "antonym",    label: "دژەوشە",      color: "#f87171", dash: "8,4" },
  { type: "related",    label: "پەیوەندیدار",  color: "#60a5fa", dash: "none" },
  { type: "example",    label: "نموونە",      color: "#fbbf24", dash: "3,5" },
  { type: "usage",      label: "بکارهێنان",   color: "#a78bfa", dash: "10,3,2,3" },
  { type: "contextual", label: "چارچێوەیی",   color: "#34d4f0", dash: "18,5" },
  { type: "partof",     label: "بەشێکە لە",   color: "#fb923c", dash: "6,3,1,3" },
];

const SPEECH_LEGEND = [
  { label: "ناو",       color: "#06b6d4" },
  { label: "کریا",      color: "#0ea5e9" },
  { label: "ناوستانی",  color: "#3b82f6" },
  { label: "ئاوەڵ",    color: "#22d3ee" },
  { label: "دەستەواژە", color: "#38bdf8" },
  { label: "تر",        color: "#475569" },
];

// ── Static star dots ──────────────────────────────────────────────────────────

const STAR_DOTS = Array.from({ length: 110 }, (_, i) => {
  const s = i * 137.508;
  return {
    cx:  `${((s * 1.618) % 100).toFixed(1)}%`,
    cy:  `${((s * 2.414) % 100).toFixed(1)}%`,
    r:   (0.5 + (i % 5) * 0.22).toFixed(2),
    op:  (0.05 + (i % 8) * 0.045).toFixed(2),
    dur: `${2.5 + (i % 6) * 0.8}s`,
    del: `${((i * 0.35) % 5).toFixed(1)}s`,
  };
});

// ── Theme hook ────────────────────────────────────────────────────────────────

function useTheme() {
  const [theme, setTheme] = useState<"dark" | "light">("dark");

  useEffect(() => {
    const saved = (typeof localStorage !== "undefined" && localStorage.getItem("universe-theme")) as "dark" | "light" | null;
    const init  = saved ?? "dark";
    setTheme(init);
    if (init === "light") document.documentElement.setAttribute("data-theme", "light");
    else                  document.documentElement.removeAttribute("data-theme");
  }, []);

  const toggle = useCallback(() => {
    setTheme(prev => {
      const next = prev === "dark" ? "light" : "dark";
      localStorage.setItem("universe-theme", next);
      if (next === "light") document.documentElement.setAttribute("data-theme", "light");
      else                  document.documentElement.removeAttribute("data-theme");
      return next;
    });
  }, []);

  return { theme, toggle };
}

// ── Component ─────────────────────────────────────────────────────────────────

export default function UniversePage() {
  const graphRef = useRef<UniverseGraphHandle>(null);
  const { theme, toggle: toggleTheme } = useTheme();

  const [ready,     setReady]     = useState(false);
  const [loading,   setLoading]   = useState(false);
  const [query,     setQuery]     = useState("");
  const [hits,      setHits]      = useState<SearchHit[]>([]);
  const [searching, setSearching] = useState(false);
  const [focal,     setFocal]     = useState<FocalWord | null>(null);
  const [focalScrn, setFocalScrn] = useState({ x: 0, y: 0 });
  const [showLeg,   setShowLeg]   = useState(false);
  const [wordStars, setWordStars] = useState<WordStar[]>([]);

  const abortRef    = useRef<AbortController | null>(null);
  const timerRef    = useRef<ReturnType<typeof setTimeout> | null>(null);
  const scrThrottle = useRef(0);

  // ── Load word star field ──────────────────────────────────────────────────
  useEffect(() => {
    (async () => {
      try {
        const r = await fetch("/api/words?pageSize=100");
        if (!r.ok) return;
        const d  = await r.json();
        const W  = window.innerWidth, H = window.innerHeight;
        const cx = W / 2, cy = H / 2;
        const minDist = Math.min(W, H) * 0.24;

        const stars: WordStar[] = (d.items ?? []).map(
          (w: { id: number; kurdish: string }, i: number) => {
            const phi = i * 137.508;
            let x = (phi * 1.618034) % W;
            let y = (phi * 2.41421)  % H;
            // Push away from center search area
            const dx = x - cx, dy = y - cy;
            const dist = Math.sqrt(dx * dx + dy * dy);
            if (dist < minDist && dist > 0) {
              const s = minDist / dist;
              x = cx + dx * s;
              y = cy + dy * s;
            }
            x = Math.max(70, Math.min(W - 130, x));
            y = Math.max(24, Math.min(H - 36,  y));
            return {
              id: w.id, kurdish: w.kurdish, x, y,
              opacity: 0.07 + (i % 9) * 0.022,
              size:    11   + (i % 6) * 1.5,
              animDur: `${6 + (i % 7) * 1.6}s`,
              animDel: `${((i * 0.41) % 6).toFixed(1)}s`,
            };
          }
        );
        setWordStars(stars);
      } catch { /* ignore */ }
    })();
  }, []);

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

  // ── Select word ───────────────────────────────────────────────────────────
  const selectWord = useCallback(async (wordId: number) => {
    setLoading(true); setHits([]); setQuery("");
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
    } finally { setLoading(false); }
  }, []);

  const handleNodeClick = useCallback((wordId: number) => selectWord(wordId), [selectWord]);

  const handleFocalPos = useCallback((sx: number, sy: number) => {
    const now = Date.now();
    if (now - scrThrottle.current < 50) return;
    scrThrottle.current = now;
    setFocalScrn({ x: sx, y: sy });
  }, []);

  // ── Nebula gradient (theme-aware) ─────────────────────────────────────────
  const nebula = theme === "light"
    ? "radial-gradient(ellipse 80% 60% at 15% 20%, rgba(186,230,255,.55) 0%, transparent 60%)," +
      "radial-gradient(ellipse 60% 50% at 85% 15%, rgba(224,242,254,.45) 0%, transparent 55%)," +
      "radial-gradient(ellipse 70% 65% at 50% 95%, rgba(240,249,255,.35) 0%, transparent 60%)"
    : "radial-gradient(ellipse 80% 60% at 12% 55%, rgba(99,102,241,.07) 0%, transparent 60%)," +
      "radial-gradient(ellipse 65% 55% at 88% 18%, rgba(139,92,246,.05) 0%, transparent 55%)," +
      "radial-gradient(ellipse 75% 65% at 50% 92%, rgba(59,130,246,.04) 0%, transparent 60%)";

  // ── Render ────────────────────────────────────────────────────────────────
  return (
    <div className="fixed inset-0 overflow-hidden select-none"
      style={{ background: "var(--background)" }}>

      {/* ── Background: nebula + star dots ───────────────────────────────── */}
      <div className="absolute inset-0 pointer-events-none overflow-hidden">
        <div className="absolute inset-0" style={{ background: nebula }} />
        {theme === "dark" && (
          <svg className="absolute inset-0 w-full h-full" aria-hidden>
            {STAR_DOTS.map((s, i) => (
              <circle key={i} cx={s.cx} cy={s.cy} r={s.r} fill="white" opacity={s.op}
                style={{ animation: `twinkle ${s.dur} ease-in-out ${s.del} infinite alternate` }} />
            ))}
          </svg>
        )}
      </div>

      {/* ── Word star field (pre-search) ─────────────────────────────────── */}
      <div className="absolute inset-0 overflow-hidden"
        style={{
          opacity: ready ? 0 : 1,
          pointerEvents: ready ? "none" : "auto",
          transition: "opacity 0.8s ease",
        }}>
        {wordStars.map(w => (
          <button key={w.id}
            onClick={() => selectWord(w.id)}
            className="absolute hover:opacity-100 transition-all duration-300 hover:scale-110"
            style={{
              left:       w.x,
              top:        w.y,
              opacity:    w.opacity,
              fontSize:   `${w.size}px`,
              fontFamily: "'NRT', system-ui, sans-serif",
              color:      theme === "light" ? "#334155" : "#e2e8f0",
              animation:  `float-word ${w.animDur} ease-in-out ${w.animDel} infinite`,
              textShadow: theme === "dark"
                ? `0 0 12px rgba(6,182,212,0.35)`
                : `0 0 8px rgba(14,165,233,0.25)`,
              whiteSpace: "nowrap",
              cursor:     "pointer",
            }}>
            {w.kurdish}
          </button>
        ))}
      </div>

      {/* ── D3 Graph ─────────────────────────────────────────────────────── */}
      <UniverseGraph ref={graphRef} onNodeClick={handleNodeClick} onFocalPos={handleFocalPos} />

      {/* ── Landing search (center) ───────────────────────────────────────── */}
      <div className="absolute z-20 flex flex-col items-center gap-5 transition-all duration-700 ease-out"
        style={ready
          ? { opacity: 0, pointerEvents: "none", top: "50%", left: "50%", transform: "translate(-50%, -50%)" }
          : { opacity: 1, top: "50%",  left: "50%",  transform: "translate(-50%, -50%)", width: "min(92vw, 540px)" }}>
        <div className="text-center pointer-events-none">
          <h1 className="text-4xl font-black leading-tight tracking-tight"
            style={{
              fontFamily: "'NRT', system-ui, sans-serif",
              color: "var(--t-text-1)",
              textShadow: theme === "dark"
                ? "0 0 40px rgba(6,182,212,.5), 0 0 80px rgba(6,182,212,.2)"
                : "0 2px 12px rgba(14,165,233,.25)",
            }}>
            فەرهەنگی کوردی
          </h1>
          <p className="text-sm mt-1.5 tracking-widest uppercase"
            style={{ color: "var(--t-text-4)" }}>
            Kurdish Dictionary Universe
          </p>
        </div>
        <SearchBox
          value={query} onChange={setQuery}
          hits={hits} searching={searching}
          onSelect={selectWord}
          placeholder="وشەیەک بنووسە بۆ دەستپێکردن..."
        />
      </div>

      {/* ── Compact search (top-left after ready) ────────────────────────── */}
      <div className="absolute top-4 left-4 z-20 transition-all duration-500"
        style={{ opacity: ready ? 1 : 0, pointerEvents: ready ? "auto" : "none", width: "250px" }}>
        <SearchBox
          value={query} onChange={setQuery}
          hits={hits} searching={searching}
          onSelect={selectWord} compact
          placeholder="گەڕانی وشەی نوێ..."
        />
      </div>

      {/* ── Theme toggle ─────────────────────────────────────────────────── */}
      <button
        onClick={toggleTheme}
        className="absolute top-4 right-4 z-30 rounded-xl px-3 py-2 text-sm font-semibold transition-all duration-300"
        style={{
          background:     "var(--surface)",
          backdropFilter: "blur(20px)",
          border:         "1px solid var(--border-strong)",
          color:          "var(--t-text-2)",
          boxShadow:      "0 2px 12px rgba(0,0,0,.15)",
        }}>
        {theme === "dark" ? "☀ ڕووناکی" : "🌙 ژیرۆکی"}
      </button>

      {/* ── Loading spinner ───────────────────────────────────────────────── */}
      {loading && (
        <div className="absolute inset-0 flex items-center justify-center z-30 pointer-events-none">
          <div className="w-12 h-12 rounded-full border-2 border-cyan-500/20 border-t-cyan-500 animate-spin" />
        </div>
      )}

      {/* ── Word panel (top-right, shifts down for theme toggle) ─────────── */}
      {focal && <WordPanel word={focal} />}

      {/* ── Meaning cards near focal node ────────────────────────────────── */}
      {focal && focal.meanings.length > 0 && (
        <MeaningCards meanings={focal.meanings} pos={focalScrn} />
      )}

      {/* ── Legend toggle ────────────────────────────────────────────────── */}
      <button
        onClick={() => setShowLeg(v => !v)}
        className="absolute bottom-4 left-4 z-20 rounded-xl px-3 py-2 text-xs font-semibold transition-colors flex items-center gap-2"
        style={{
          background:     "var(--surface)",
          backdropFilter: "blur(16px)",
          border:         "1px solid var(--border)",
          color:          "var(--t-text-3)",
        }}>
        <span>🕸</span><span>ڕێنمایی</span>
      </button>

      {showLeg && (
        <div className="absolute bottom-14 left-4 z-20 rounded-2xl p-4 space-y-4 w-52 pointer-events-auto"
          style={{
            background:     "var(--t-modal-bg)",
            backdropFilter: "blur(32px) saturate(200%)",
            WebkitBackdropFilter: "blur(32px) saturate(200%)",
            border:         "1px solid var(--border-strong)",
            boxShadow:      "0 8px 40px rgba(0,0,0,.2)",
          }}
          onClick={e => e.stopPropagation()}>
          <div>
            <p className="text-[10px] font-bold uppercase tracking-widest mb-2.5"
              style={{ color: "var(--t-text-4)" }}>جۆری پەیوەندی</p>
            <div className="space-y-1.5">
              {REL_LEGEND.map(r => (
                <div key={r.type} className="flex items-center gap-2">
                  <svg width="26" height="8" className="shrink-0">
                    <line x1="0" y1="4" x2="26" y2="4"
                      stroke={r.color} strokeWidth="2"
                      strokeDasharray={r.dash === "none" ? undefined : r.dash}
                      strokeLinecap="round" />
                  </svg>
                  <span className="text-xs" style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-2)" }}>
                    {r.label}
                  </span>
                </div>
              ))}
            </div>
          </div>
          <div>
            <p className="text-[10px] font-bold uppercase tracking-widest mb-2.5"
              style={{ color: "var(--t-text-4)" }}>جۆری وشە</p>
            <div className="grid grid-cols-2 gap-1.5">
              {SPEECH_LEGEND.map(s => (
                <div key={s.label} className="flex items-center gap-1.5">
                  <svg width="10" height="10">
                    <circle cx="5" cy="5" r="4" fill={s.color} />
                  </svg>
                  <span className="text-[11px]"
                    style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-3)" }}>
                    {s.label}
                  </span>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}

      {/* ── Hint ─────────────────────────────────────────────────────────── */}
      {!ready && (
        <p className="absolute bottom-6 left-1/2 -translate-x-1/2 z-10 text-xs pointer-events-none"
          style={{ color: "var(--t-text-5)", animation: "pulse 2s ease-in-out infinite" }}>
          وشەیەک بگەڕێ یان کلیک لەسەر وشەیەک بکە
        </p>
      )}
    </div>
  );
}

// ── SearchBox ─────────────────────────────────────────────────────────────────

function SearchBox({
  value, onChange, hits, searching, onSelect, placeholder, compact = false,
}: {
  value: string; onChange(v: string): void; hits: SearchHit[];
  searching: boolean; onSelect(id: number): void;
  placeholder: string; compact?: boolean;
}) {
  return (
    <div className="relative w-full">
      <div className="flex items-center overflow-hidden"
        style={{
          background:     "var(--t-modal-bg)",
          backdropFilter: "blur(32px) saturate(200%)",
          WebkitBackdropFilter: "blur(32px) saturate(200%)",
          border:         "1px solid var(--border-strong)",
          borderRadius:   "16px",
          boxShadow:      "0 0 40px rgba(6,182,212,.12), 0 8px 40px rgba(0,0,0,.2)",
        }}>
        <span className="pl-4 shrink-0 text-sm" style={{ color: "var(--t-text-4)" }}>🔍</span>
        <input
          value={value}
          onChange={e => onChange(e.target.value)}
          placeholder={placeholder}
          dir="rtl"
          className={`w-full bg-transparent pr-4 pl-2 ${compact ? "py-2.5 text-sm" : "py-3.5 text-base"} outline-none`}
          style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-1)" }}
        />
      </div>

      {(hits.length > 0 || searching) && (
        <div className="absolute top-full mt-2 w-full rounded-2xl overflow-hidden z-40 shadow-2xl"
          style={{
            background:     "var(--t-dropdown-bg)",
            backdropFilter: "blur(28px)",
            border:         "1px solid var(--border-strong)",
          }}>
          {searching && !hits.length && (
            <div className="px-4 py-3 text-sm text-center"
              style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-4)" }}>
              گەڕان...
            </div>
          )}
          {hits.map(h => (
            <button key={h.id}
              onMouseDown={e => { e.preventDefault(); onSelect(h.id); }}
              className="w-full flex items-center justify-between gap-3 px-4 py-3 text-right transition-colors border-b last:border-0"
              style={{ borderColor: "var(--t-dropdown-divider)" }}
              onMouseEnter={e => (e.currentTarget.style.background = "var(--t-dropdown-hover)")}
              onMouseLeave={e => (e.currentTarget.style.background = "transparent")}>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-bold truncate" dir="rtl"
                  style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-1)" }}>
                  {h.kurdish}
                </p>
                {h.meanings[0] && (
                  <p className="text-xs truncate mt-0.5" style={{ color: "var(--t-text-3)" }}>
                    {h.meanings[0].locate && (
                      <span className="text-[10px] font-semibold mr-1" style={{ color: "var(--accent)" }}>
                        {h.meanings[0].locate}
                      </span>
                    )}
                    {h.meanings[0].meaning}
                  </p>
                )}
              </div>
              <div className="flex items-center gap-1.5 shrink-0">
                {h.speechPaneKurdish && (
                  <span className="text-[10px] px-2 py-0.5 rounded-full font-semibold"
                    style={{ background: "rgba(6,182,212,.15)", color: "#22d3ee" }}>
                    {h.speechPaneKurdish}
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
    <div className="absolute top-14 right-4 z-20 w-60 rounded-2xl p-3 pointer-events-auto"
      style={{
        background:     "var(--t-modal-bg)",
        backdropFilter: "blur(36px) saturate(220%)",
        WebkitBackdropFilter: "blur(36px) saturate(220%)",
        border:         "1px solid var(--border-strong)",
        boxShadow:      "0 8px 40px rgba(0,0,0,.2), inset 0 1px 0 rgba(255,255,255,.06)",
        maxHeight:      "calc(100vh - 5rem)",
        overflowY:      "auto",
      }}>
      <div dir="rtl" className="space-y-2.5">
        {/* Header */}
        <div>
          <h2 className="text-xl font-black leading-tight"
            style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-1)" }}>
            {word.kurdish}
          </h2>
          <div className="flex flex-wrap gap-1 mt-1">
            {word.speechPaneKurdish && (
              <span className="text-[10px] font-semibold px-2 py-0.5 rounded-full"
                style={{ background: "rgba(6,182,212,.15)", border: "1px solid rgba(6,182,212,.25)", color: "#22d3ee" }}>
                {word.speechPaneKurdish}
              </span>
            )}
            {word.category && (
              <span className="text-[10px] font-medium px-2 py-0.5 rounded-full"
                style={{ background: "rgba(99,102,241,.12)", border: "1px solid rgba(99,102,241,.2)", color: "var(--accent-light)" }}>
                {word.category}
              </span>
            )}
          </div>
        </div>

        {/* Description */}
        {word.description && (
          <p className="text-[11px] leading-relaxed line-clamp-2"
            style={{ color: "var(--t-text-4)" }}>
            {word.description}
          </p>
        )}

        {/* Meanings */}
        {word.meanings.length > 0 && (
          <div className="space-y-1">
            <p className="text-[9px] font-bold uppercase tracking-widest"
              style={{ color: "var(--t-text-5)" }}>مانا</p>
            {word.meanings.map((m, i) => (
              <div key={i} className="rounded-lg px-2.5 py-1.5"
                style={{ background: "rgba(6,182,212,.06)", border: "1px solid rgba(6,182,212,.1)" }}>
                {m.locate && (
                  <span className="text-[8px] font-bold uppercase tracking-wider block mb-0.5"
                    style={{ color: "#22d3ee" }}>
                    {m.locate}
                  </span>
                )}
                <p className="text-[11px] leading-snug"
                  style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-2)" }}>
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
        const total = items.length;
        const angle = (Math.PI * 0.5) + (i - (total - 1) / 2) * (Math.PI / Math.max(total, 3));
        const dist  = 150;
        const rawX  = pos.x + Math.cos(angle) * dist;
        const rawY  = pos.y + Math.sin(angle) * dist;
        const cardX = Math.max(8, Math.min(W - 210, rawX - 95));
        const cardY = Math.max(8, Math.min(H - 76,  rawY - 28));

        return (
          <div key={i} className="absolute"
            style={{ left: cardX, top: cardY, transition: "left .25s ease-out, top .25s ease-out" }}>
            <div className="rounded-xl px-3 py-2 text-center shadow-xl"
              style={{
                background:     "var(--t-modal-bg)",
                backdropFilter: "blur(28px) saturate(200%)",
                WebkitBackdropFilter: "blur(28px) saturate(200%)",
                border:         "1px solid var(--border-strong)",
                minWidth: "100px", maxWidth: "185px",
              }}>
              {m.locate && (
                <p className="text-[8px] font-bold uppercase tracking-wider mb-0.5"
                  style={{ color: "#22d3ee" }}>
                  {m.locate}
                </p>
              )}
              <p className="text-[11px] leading-snug"
                dir="rtl"
                style={{ fontFamily: "'NRT', system-ui, sans-serif", color: "var(--t-text-2)" }}>
                {m.meaning}
              </p>
            </div>
          </div>
        );
      })}
    </div>
  );
}
