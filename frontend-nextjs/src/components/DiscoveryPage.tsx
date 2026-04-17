"use client";

import {
  useRef, useState, useEffect, useCallback,
} from "react";
import dynamic from "next/dynamic";
import { useTheme } from "./ThemeProvider";
import ThemeToggle from "./ThemeToggle";
import RichWordCard, { type WordItem } from "./RichWordCard";
import type { UniverseGraphHandle } from "./UniverseGraph";

const UniverseGraph = dynamic(() => import("./UniverseGraph"), { ssr: false });

// ── Types ─────────────────────────────────────────────────────────────────────

interface Meaning { id: number; meaning: string; locate?: string; }

interface FocalWord {
  id: number; kurdish: string;
  speechPaneKurdish?: string; category?: string;
  description?: string; meanings: Meaning[];
}

interface SearchHit {
  id: number; kurdish: string;
  speechPane: number; speechPaneKurdish?: string;
  category?: string; totalRelations: number;
  meanings: Meaning[];
}

interface PagedResult {
  items: WordItem[];
  totalCount: number;
  totalPages: number;
  page: number;
}

const GRID_SIZE = 18;

// ── Main component ────────────────────────────────────────────────────────────

export default function DiscoveryPage() {
  const { theme } = useTheme();

  // ── View mode ─────────────────────────────────────────────────────────────
  const [mode, setMode] = useState<"discovery" | "galaxy">("discovery");

  // ── Discovery state ───────────────────────────────────────────────────────
  const [categories,      setCategories]      = useState<string[]>([]);
  const [activeCategory,  setActiveCategory]  = useState("");
  const [discSearch,      setDiscSearch]      = useState("");
  const [words,           setWords]           = useState<WordItem[]>([]);
  const [totalCount,      setTotalCount]      = useState(0);
  const [curPage,         setCurPage]         = useState(1);
  const [hasMore,         setHasMore]         = useState(false);
  const [initLoading,     setInitLoading]     = useState(true);
  const [loadingMore,     setLoadingMore]     = useState(false);

  // ── Galaxy state ──────────────────────────────────────────────────────────
  const graphRef      = useRef<UniverseGraphHandle>(null);
  const [galaxyFocal, setGalaxyFocal] = useState<FocalWord | null>(null);
  const [focalScrn,   setFocalScrn]   = useState({ x: 0, y: 0 });
  const [gSearch,     setGSearch]     = useState("");
  const [gHits,       setGHits]       = useState<SearchHit[]>([]);
  const [gSearching,  setGSearching]  = useState(false);
  const [initWordId,  setInitWordId]  = useState<number | null>(null);
  const [gLoading,    setGLoading]    = useState(false);

  const sentinelRef   = useRef<HTMLDivElement>(null);
  const searchDebRef  = useRef<ReturnType<typeof setTimeout> | null>(null);
  const gSearchDebRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const abortRef      = useRef<AbortController | null>(null);
  const scrThrottle   = useRef(0);

  // ── Fetch helpers ─────────────────────────────────────────────────────────

  const fetchWords = useCallback(async (
    search: string, category: string, page: number, append: boolean
  ) => {
    if (append) setLoadingMore(true);
    else        setInitLoading(true);

    const p = new URLSearchParams({ page: String(page), pageSize: String(GRID_SIZE) });
    if (search.trim())   p.set("search",   search.trim());
    if (category)        p.set("category", category);

    try {
      const r = await fetch(`/api/words?${p}`);
      if (!r.ok) return;
      const d: PagedResult = await r.json();
      setWords(prev => append ? [...prev, ...d.items] : d.items);
      setTotalCount(d.totalCount);
      setHasMore(d.items.length === GRID_SIZE && page < (d.totalPages ?? 1));
      setCurPage(page);
    } finally {
      if (append) setLoadingMore(false);
      else        setInitLoading(false);
    }
  }, []);

  // ── Initial load ──────────────────────────────────────────────────────────

  useEffect(() => {
    fetch("/api/words/categories").then(r => r.json()).then(setCategories).catch(() => {});
    fetchWords("", "", 1, false);
  }, [fetchWords]);

  // ── Discovery search ──────────────────────────────────────────────────────

  const handleDiscSearch = useCallback((v: string) => {
    setDiscSearch(v);
    if (searchDebRef.current) clearTimeout(searchDebRef.current);
    searchDebRef.current = setTimeout(() => {
      fetchWords(v, activeCategory, 1, false);
    }, 280);
  }, [activeCategory, fetchWords]);

  const handleCategorySelect = useCallback((cat: string) => {
    setActiveCategory(cat);
    setDiscSearch("");
    fetchWords("", cat, 1, false);
  }, [fetchWords]);

  // ── Infinite scroll ───────────────────────────────────────────────────────

  useEffect(() => {
    const el = sentinelRef.current;
    if (!el) return;
    const obs = new IntersectionObserver(entries => {
      if (entries[0].isIntersecting && hasMore && !loadingMore && !initLoading) {
        fetchWords(discSearch, activeCategory, curPage + 1, true);
      }
    }, { threshold: 0.1 });
    obs.observe(el);
    return () => obs.disconnect();
  }, [hasMore, loadingMore, initLoading, curPage, discSearch, activeCategory, fetchWords]);

  // ── Explore word → galaxy mode ────────────────────────────────────────────

  const handleExplore = useCallback((wordId: number) => {
    setInitWordId(wordId);
    setGalaxyFocal(null);
    setMode("galaxy");
  }, []);

  // ── Galaxy: auto-load initial word ────────────────────────────────────────

  useEffect(() => {
    if (mode !== "galaxy" || !initWordId) return;
    const t = setTimeout(async () => {
      setGLoading(true);
      try {
        const [, wRes] = await Promise.all([
          graphRef.current?.loadWord(initWordId),
          fetch(`/api/words/${initWordId}`),
        ]);
        if (wRes?.ok) {
          const w = await wRes.json();
          setGalaxyFocal({
            id: w.id, kurdish: w.kurdish,
            speechPaneKurdish: w.speechPaneKurdish,
            category: w.category, description: w.description,
            meanings: w.meanings ?? [],
          });
        }
      } finally { setGLoading(false); }
    }, 180);
    return () => clearTimeout(t);
  }, [mode, initWordId]);

  // ── Galaxy search ─────────────────────────────────────────────────────────

  useEffect(() => {
    if (!gSearch.trim()) { setGHits([]); return; }
    if (gSearchDebRef.current) clearTimeout(gSearchDebRef.current);
    gSearchDebRef.current = setTimeout(async () => {
      setGSearching(true);
      abortRef.current?.abort();
      abortRef.current = new AbortController();
      try {
        const r = await fetch(`/api/words?search=${encodeURIComponent(gSearch)}&pageSize=8`, {
          signal: abortRef.current.signal,
        });
        if (r.ok) { const d = await r.json(); setGHits(d.items ?? []); }
      } catch { /* aborted */ }
      setGSearching(false);
    }, 220);
    return () => { if (gSearchDebRef.current) clearTimeout(gSearchDebRef.current); };
  }, [gSearch]);

  const handleGalaxySelect = useCallback(async (wordId: number) => {
    setGHits([]); setGSearch(""); setGLoading(true);
    try {
      const [, wRes] = await Promise.all([
        graphRef.current?.loadWord(wordId),
        fetch(`/api/words/${wordId}`),
      ]);
      if (wRes?.ok) {
        const w = await wRes.json();
        setGalaxyFocal({
          id: w.id, kurdish: w.kurdish,
          speechPaneKurdish: w.speechPaneKurdish,
          category: w.category, description: w.description,
          meanings: w.meanings ?? [],
        });
      }
    } finally { setGLoading(false); }
  }, []);

  const handleNodeClick = useCallback((wordId: number) => {
    handleGalaxySelect(wordId);
  }, [handleGalaxySelect]);

  const handleFocalPos = useCallback((sx: number, sy: number) => {
    const now = Date.now();
    if (now - scrThrottle.current < 50) return;
    scrThrottle.current = now;
    setFocalScrn({ x: sx, y: sy });
  }, []);

  // ── Galaxy nebula ─────────────────────────────────────────────────────────

  const nebula = theme === "light"
    ? "radial-gradient(ellipse 80% 60% at 15% 20%, rgba(186,230,255,.55) 0%, transparent 60%)," +
      "radial-gradient(ellipse 60% 50% at 85% 15%, rgba(224,242,254,.45) 0%, transparent 55%)"
    : "radial-gradient(ellipse 80% 60% at 12% 55%, rgba(99,102,241,.07) 0%, transparent 60%)," +
      "radial-gradient(ellipse 65% 55% at 88% 18%, rgba(139,92,246,.05) 0%, transparent 55%)";

  // ── Render ────────────────────────────────────────────────────────────────

  return (
    <div style={{ minHeight: "100vh", background: "var(--background)" }}>

      {/* ════════════════════════════════════════════════════════════
          DISCOVERY MODE
      ════════════════════════════════════════════════════════════ */}

      {/* Top bar — sticky */}
      <header className="sticky top-0 z-30 flex items-center gap-3 px-4 py-3 border-b"
        style={{
          background:     "var(--t-header-bg)",
          backdropFilter: "blur(20px)",
          borderColor:    "var(--border)",
        }}>
        {/* Logo */}
        <span className="font-black text-lg shrink-0"
          style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-1)" }}>
          فەرهەنگی کوردی
        </span>

        {/* Discovery search */}
        <div className="flex-1 relative max-w-md">
          <span className="absolute right-3 top-1/2 -translate-y-1/2 text-sm"
            style={{ color: "var(--t-text-5)" }}>🔍</span>
          <input
            value={discSearch}
            onChange={e => handleDiscSearch(e.target.value)}
            placeholder="گەڕان بە وشە..."
            dir="rtl"
            className="w-full rounded-xl py-2 pr-9 pl-3 text-sm outline-none transition-all"
            style={{
              background:  "var(--t-input-bg)",
              border:      `1px solid var(--t-input-border)`,
              color:       "var(--t-text-1)",
              fontFamily:  "'NRT',system-ui,sans-serif",
            }}
          />
        </div>

        {/* Word count */}
        <span className="text-xs shrink-0 hidden sm:block" style={{ color: "var(--t-text-4)" }}>
          {totalCount.toLocaleString()} وشە
        </span>

        {/* Galaxy toggle */}
        <button
          onClick={() => setMode(mode === "discovery" ? "galaxy" : "discovery")}
          className="shrink-0 flex items-center gap-1.5 text-xs font-semibold px-3 py-1.5 rounded-xl transition-all duration-200"
          style={{
            background: mode === "galaxy"
              ? "rgba(6,182,212,.18)" : "var(--surface-raised)",
            border:  `1px solid ${mode === "galaxy" ? "rgba(6,182,212,.4)" : "var(--border)"}`,
            color:   mode === "galaxy" ? "#22d3ee" : "var(--t-text-2)",
          }}>
          {mode === "galaxy" ? "◉ گەلاکسی" : "✦ گەلاکسی"}
        </button>

        <ThemeToggle />
      </header>

      {/* Category pills — horizontal scroll on mobile */}
      {categories.length > 0 && (
        <div className="sticky top-[53px] z-20 flex items-center gap-2 px-4 py-2 overflow-x-auto border-b"
          style={{
            background:  "var(--t-header-bg)",
            backdropFilter: "blur(16px)",
            borderColor: "var(--border)",
          }}>
          <button
            onClick={() => handleCategorySelect("")}
            className="shrink-0 text-xs font-semibold px-3 py-1 rounded-full transition-all"
            style={{
              background: !activeCategory ? "rgba(6,182,212,.18)" : "var(--surface)",
              border:     `1px solid ${!activeCategory ? "rgba(6,182,212,.4)" : "var(--border)"}`,
              color:      !activeCategory ? "#22d3ee" : "var(--t-text-3)",
            }}>
            هەموو
          </button>
          {categories.map(cat => (
            <button key={cat}
              onClick={() => handleCategorySelect(cat)}
              className="shrink-0 text-xs font-semibold px-3 py-1 rounded-full transition-all whitespace-nowrap"
              style={{
                fontFamily: "'NRT',system-ui,sans-serif",
                background: activeCategory === cat ? "rgba(6,182,212,.18)" : "var(--surface)",
                border:     `1px solid ${activeCategory === cat ? "rgba(6,182,212,.4)" : "var(--border)"}`,
                color:      activeCategory === cat ? "#22d3ee" : "var(--t-text-3)",
              }}>
              {cat}
            </button>
          ))}
        </div>
      )}

      {/* Word grid */}
      <main className="px-4 py-5 max-w-[1600px] mx-auto">
        {initLoading ? (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 2xl:grid-cols-4 gap-4">
            {Array.from({ length: 12 }).map((_, i) => (
              <SkeletonCard key={i} />
            ))}
          </div>
        ) : words.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-28 text-center">
            <span className="text-5xl mb-4">📖</span>
            <p className="text-lg font-semibold" style={{ color: "var(--t-text-3)" }}>
              هیچ وشەیەک نەدۆزرایەوە
            </p>
            <p className="text-sm mt-1" style={{ color: "var(--t-text-5)" }}>
              گەڕانەکەت بگۆڕە
            </p>
          </div>
        ) : (
          <>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 2xl:grid-cols-4 gap-4">
              {words.map(w => (
                <RichWordCard key={w.id} word={w} onExplore={handleExplore} />
              ))}
            </div>

            {/* Infinite scroll sentinel */}
            <div ref={sentinelRef} className="h-6 mt-4" />

            {loadingMore && (
              <div className="flex justify-center py-6">
                <div className="w-8 h-8 rounded-full border-2 border-cyan-500/20 border-t-cyan-500 animate-spin" />
              </div>
            )}

            {!hasMore && words.length > 0 && (
              <p className="text-center text-xs py-6" style={{ color: "var(--t-text-5)" }}>
                {words.length} وشە نیشاندراو
              </p>
            )}
          </>
        )}
      </main>

      {/* ════════════════════════════════════════════════════════════
          GALAXY MODE — always mounted, CSS-hidden when inactive
          This keeps the D3 simulation alive across mode switches.
      ════════════════════════════════════════════════════════════ */}

      <div
        className="fixed inset-0 z-40 overflow-hidden"
        style={{
          opacity:       mode === "galaxy" ? 1 : 0,
          pointerEvents: mode === "galaxy" ? "auto" : "none",
          transition:    "opacity 0.35s ease",
        }}>

        {/* BG */}
        <div className="absolute inset-0" style={{ background: "var(--background)" }} />
        <div className="absolute inset-0 pointer-events-none" style={{ background: nebula }} />

        {/* Dark-mode stars */}
        {theme === "dark" && (
          <div className="absolute inset-0 pointer-events-none overflow-hidden">
            <svg className="w-full h-full" aria-hidden>
              {STAR_DOTS.map((s, i) => (
                <circle key={i} cx={s.cx} cy={s.cy} r={s.r} fill="white" opacity={s.op}
                  style={{ animation: `twinkle ${s.dur} ease-in-out ${s.del} infinite alternate` }} />
              ))}
            </svg>
          </div>
        )}

        {/* D3 Graph — always rendered */}
        <UniverseGraph ref={graphRef} onNodeClick={handleNodeClick} onFocalPos={handleFocalPos} />

        {/* Back button */}
        <button
          onClick={() => setMode("discovery")}
          className="absolute top-4 left-4 z-50 flex items-center gap-2 text-sm font-semibold px-3 py-2 rounded-xl transition-all"
          style={{
            background:     "var(--surface)",
            backdropFilter: "blur(20px)",
            border:         "1px solid var(--border-strong)",
            color:          "var(--t-text-2)",
          }}>
          ← بەرپاشگەزبوون
        </button>

        {/* Galaxy search — centered top */}
        <div className="absolute top-4 left-1/2 -translate-x-1/2 z-50"
          style={{ width: "min(90vw, 380px)" }}>
          <GalaxySearchBox
            value={gSearch} onChange={setGSearch}
            hits={gHits} searching={gSearching}
            onSelect={id => { handleGalaxySelect(id); }}
          />
        </div>

        {/* Theme toggle — top right */}
        <div className="absolute top-4 right-4 z-50">
          <ThemeToggle />
        </div>

        {/* Loading */}
        {gLoading && (
          <div className="absolute inset-0 flex items-center justify-center z-30 pointer-events-none">
            <div className="w-12 h-12 rounded-full border-2 border-cyan-500/20 border-t-cyan-500 animate-spin" />
          </div>
        )}

        {/* Word panel */}
        {galaxyFocal && <GalaxyWordPanel word={galaxyFocal} />}

        {/* Meaning cards */}
        {galaxyFocal && galaxyFocal.meanings.length > 0 && (
          <GalaxyMeaningCards meanings={galaxyFocal.meanings} pos={focalScrn} />
        )}
      </div>
    </div>
  );
}

// ── SkeletonCard ──────────────────────────────────────────────────────────────

function SkeletonCard() {
  return (
    <div className="rounded-2xl p-4 animate-pulse"
      style={{ background: "var(--surface-card)", border: "1px solid var(--border)" }}>
      <div className="flex justify-between mb-3 gap-2">
        <div className="h-6 rounded-lg w-32" style={{ background: "var(--t-skeleton-cell)" }} />
        <div className="h-5 rounded-full w-16" style={{ background: "var(--t-skeleton-cell)" }} />
      </div>
      <div className="h-3.5 rounded w-full mb-1.5" style={{ background: "var(--t-border-3)" }} />
      <div className="h-3.5 rounded w-3/4 mb-3"    style={{ background: "var(--t-border-3)" }} />
      <div className="h-24 rounded-xl"              style={{ background: "var(--t-border-3)" }} />
    </div>
  );
}

// ── Galaxy SearchBox ──────────────────────────────────────────────────────────

function GalaxySearchBox({
  value, onChange, hits, searching, onSelect,
}: {
  value: string; onChange(v: string): void;
  hits: SearchHit[]; searching: boolean;
  onSelect(id: number): void;
}) {
  return (
    <div className="relative w-full">
      <div className="flex items-center overflow-hidden"
        style={{
          background:     "var(--t-modal-bg)",
          backdropFilter: "blur(32px) saturate(200%)",
          WebkitBackdropFilter: "blur(32px) saturate(200%)",
          border:         "1px solid var(--border-strong)",
          borderRadius:   "14px",
          boxShadow:      "0 0 32px rgba(6,182,212,.10), 0 8px 32px rgba(0,0,0,.18)",
        }}>
        <span className="pl-4 shrink-0 text-sm" style={{ color: "var(--t-text-4)" }}>🔍</span>
        <input value={value} onChange={e => onChange(e.target.value)}
          placeholder="وشەیەک بگەڕێ..."
          dir="rtl"
          className="w-full bg-transparent pr-4 pl-2 py-2.5 text-sm outline-none"
          style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-1)" }} />
      </div>

      {(hits.length > 0 || searching) && (
        <div className="absolute top-full mt-2 w-full rounded-xl overflow-hidden z-40 shadow-2xl"
          style={{
            background:     "var(--t-dropdown-bg)",
            backdropFilter: "blur(28px)",
            border:         "1px solid var(--border-strong)",
          }}>
          {searching && !hits.length && (
            <div className="px-4 py-3 text-sm text-center"
              style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-4)" }}>
              گەڕان...
            </div>
          )}
          {hits.map(h => (
            <button key={h.id}
              onMouseDown={e => { e.preventDefault(); onSelect(h.id); }}
              className="w-full flex items-center justify-between gap-3 px-4 py-2.5 text-right border-b last:border-0 transition-colors"
              style={{ borderColor: "var(--t-dropdown-divider)" }}
              onMouseEnter={e => (e.currentTarget.style.background = "var(--t-dropdown-hover)")}
              onMouseLeave={e => (e.currentTarget.style.background = "transparent")}>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-bold truncate" dir="rtl"
                  style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-1)" }}>
                  {h.kurdish}
                </p>
                {h.meanings[0] && (
                  <p className="text-xs truncate mt-0.5" style={{ color: "var(--t-text-3)" }}>
                    {h.meanings[0].meaning}
                  </p>
                )}
              </div>
              {h.speechPaneKurdish && (
                <span className="text-[10px] px-2 py-0.5 rounded-full shrink-0"
                  style={{ background: "rgba(6,182,212,.14)", color: "#22d3ee" }}>
                  {h.speechPaneKurdish}
                </span>
              )}
            </button>
          ))}
        </div>
      )}
    </div>
  );
}

// ── Galaxy Word Panel ─────────────────────────────────────────────────────────

function GalaxyWordPanel({ word }: { word: FocalWord }) {
  return (
    <div className="absolute top-14 right-4 z-20 w-60 rounded-2xl p-3 pointer-events-auto"
      style={{
        background:     "var(--t-modal-bg)",
        backdropFilter: "blur(36px) saturate(220%)",
        WebkitBackdropFilter: "blur(36px) saturate(220%)",
        border:         "1px solid var(--border-strong)",
        boxShadow:      "0 8px 40px rgba(0,0,0,.2)",
        maxHeight:      "calc(100vh - 5rem)",
        overflowY:      "auto",
      }}>
      <div dir="rtl" className="space-y-2">
        <div>
          <h2 className="text-xl font-black leading-tight"
            style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-1)" }}>
            {word.kurdish}
          </h2>
          <div className="flex flex-wrap gap-1 mt-1">
            {word.speechPaneKurdish && (
              <span className="text-[10px] font-semibold px-2 py-0.5 rounded-full"
                style={{ background: "rgba(6,182,212,.14)", border: "1px solid rgba(6,182,212,.28)", color: "#22d3ee" }}>
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

        {word.description && (
          <p className="text-[11px] leading-relaxed line-clamp-2"
            style={{ color: "var(--t-text-4)" }}>
            {word.description}
          </p>
        )}

        {word.meanings.length > 0 && (
          <div className="space-y-1">
            <p className="text-[9px] font-bold uppercase tracking-widest"
              style={{ color: "var(--t-text-5)" }}>مانا</p>
            {word.meanings.map((m, i) => (
              <div key={i} className="rounded-lg px-2 py-1.5"
                style={{ background: "rgba(6,182,212,.06)", border: "1px solid rgba(6,182,212,.10)" }}>
                {m.locate && (
                  <span className="text-[8px] font-bold uppercase tracking-wider block mb-0.5"
                    style={{ color: "#22d3ee" }}>
                    {m.locate}
                  </span>
                )}
                <p className="text-[11px] leading-snug"
                  style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-2)" }}>
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

// ── Galaxy Meaning Cards ──────────────────────────────────────────────────────

function GalaxyMeaningCards({ meanings, pos }: { meanings: Meaning[]; pos: { x: number; y: number } }) {
  const items = meanings.slice(0, 4);
  const W = typeof window !== "undefined" ? window.innerWidth  : 1280;
  const H = typeof window !== "undefined" ? window.innerHeight : 800;

  return (
    <div className="absolute inset-0 pointer-events-none z-10 overflow-hidden">
      {items.map((m, i) => {
        const n     = items.length;
        const angle = Math.PI * 0.5 + (i - (n - 1) / 2) * (Math.PI / Math.max(n, 3));
        const rawX  = pos.x + Math.cos(angle) * 148;
        const rawY  = pos.y + Math.sin(angle) * 148;
        return (
          <div key={i} className="absolute"
            style={{
              left: Math.max(8, Math.min(W - 200, rawX - 90)),
              top:  Math.max(8, Math.min(H - 72,  rawY - 26)),
              transition: "left .25s ease-out, top .25s ease-out",
            }}>
            <div className="rounded-xl px-3 py-2 text-center"
              style={{
                background:     "var(--t-modal-bg)",
                backdropFilter: "blur(28px) saturate(200%)",
                WebkitBackdropFilter: "blur(28px) saturate(200%)",
                border:         "1px solid var(--border-strong)",
                minWidth: "96px", maxWidth: "180px",
              }}>
              {m.locate && (
                <p className="text-[8px] font-bold uppercase tracking-wider mb-0.5"
                  style={{ color: "#22d3ee" }}>{m.locate}</p>
              )}
              <p className="text-[11px] leading-snug" dir="rtl"
                style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-2)" }}>
                {m.meaning}
              </p>
            </div>
          </div>
        );
      })}
    </div>
  );
}

// ── Static star dots for galaxy dark mode ─────────────────────────────────────

const STAR_DOTS = Array.from({ length: 100 }, (_, i) => {
  const s = i * 137.508;
  return {
    cx:  `${((s * 1.618) % 100).toFixed(1)}%`,
    cy:  `${((s * 2.414) % 100).toFixed(1)}%`,
    r:   (0.5 + (i % 5) * 0.22).toFixed(2),
    op:  (0.04 + (i % 8) * 0.04).toFixed(2),
    dur: `${2.5 + (i % 6) * 0.8}s`,
    del: `${((i * 0.35) % 5).toFixed(1)}s`,
  };
});
