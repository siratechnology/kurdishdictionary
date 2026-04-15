"use client";

import { useState, useCallback, useEffect, useRef } from "react";
import type { WordBasic } from "@/app/page";

// ── Types ──────────────────────────────────────────────────────────────────
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

// Slot = either loading (shows skeleton) or fully loaded
type Slot =
  | { kind: "loading"; id: number; kurdish: string }
  | { kind: "loaded"; word: Word };

// ── Relation colours ───────────────────────────────────────────────────────
const RTYPE: Record<string, { fill: string; stroke: string; text: string; glow: string; label: string }> = {
  synonym: { fill: "#dcfce7", stroke: "#16a34a", text: "#14532d", glow: "#16a34a40", label: "هاوماناکە" },
  antonym: { fill: "#fee2e2", stroke: "#dc2626", text: "#7f1d1d", glow: "#dc262640", label: "دژەوشە" },
  related: { fill: "#dbeafe", stroke: "#2563eb", text: "#1e3a8a", glow: "#2563eb40", label: "پەیوەندیدار" },
  example: { fill: "#fef9c3", stroke: "#ca8a04", text: "#713f12", glow: "#ca8a0440", label: "نموونە" },
};
const FB = { fill: "#f1f5f9", stroke: "#64748b", text: "#1e293b", glow: "#64748b30", label: "other" };
const FONT = "'NRT', system-ui, sans-serif";

// ── Per-card colour palettes (hex gradient + ring) ─────────────────────────
const CARD_PALETTES = [
  { from: "#818cf8", to: "#4f46e5", ring: "#4338ca", bg: "#eef2ff", glow: "#6366f120" }, // indigo
  { from: "#34d399", to: "#059669", ring: "#047857", bg: "#ecfdf5", glow: "#10b98120" }, // emerald
  { from: "#f472b6", to: "#db2777", ring: "#be185d", bg: "#fdf2f8", glow: "#ec489920" }, // pink
  { from: "#fb923c", to: "#ea580c", ring: "#c2410c", bg: "#fff7ed", glow: "#f9731620" }, // orange
  { from: "#38bdf8", to: "#0284c7", ring: "#0369a1", bg: "#f0f9ff", glow: "#0ea5e920" }, // sky
  { from: "#a78bfa", to: "#7c3aed", ring: "#6d28d9", bg: "#f5f3ff", glow: "#8b5cf620" }, // violet
  { from: "#fbbf24", to: "#d97706", ring: "#b45309", bg: "#fffbeb", glow: "#f59e0b20" }, // amber
  { from: "#f87171", to: "#dc2626", ring: "#b91c1c", bg: "#fef2f2", glow: "#ef444420" }, // red
];

function cardPalette(wordId: number) {
  return CARD_PALETTES[wordId % CARD_PALETTES.length];
}

// ── Helpers ────────────────────────────────────────────────────────────────
function clip(s: string | undefined, n: number): string {
  if (!s) return "?";
  return s.length > n ? s.slice(0, n - 1) + "\u2026" : s;
}

// Flat-top hexagon
function hexPts(cx: number, cy: number, r: number): string {
  return Array.from({ length: 6 }, (_, i) => {
    const a = (Math.PI / 3) * i;
    return `${(cx + r * Math.cos(a)).toFixed(1)},${(cy + r * Math.sin(a)).toFixed(1)}`;
  }).join(" ");
}

// Radial position around a center
function radial(i: number, n: number, cx: number, cy: number, r: number) {
  const a = (2 * Math.PI * i) / Math.max(n, 1) - Math.PI / 2;
  return { x: cx + r * Math.cos(a), y: cy + r * Math.sin(a) };
}

// Deduplicate relations — keep one per relatedWordId (prefer outgoing)
function dedupeRels(out: RelDto[], inc: RelDto[]): RelDto[] {
  const map = new Map<number, RelDto>();
  // outgoing first (higher priority)
  for (const r of out) map.set(r.relatedWordId, r);
  // incoming only if not already present
  for (const r of inc) if (!map.has(r.relatedWordId)) map.set(r.relatedWordId, r);
  // sort by weight ascending (closest first)
  return Array.from(map.values()).sort((a, b) => a.weight - b.weight);
}

// ── SVG constants ──────────────────────────────────────────────────────────
const VW = 420;
const VH = 390;
const CCX = 210;
const CCY = 182;
const HEX_R = 52;
const REL_R = 143;
const PW = 84; // pill width
const PH = 32; // pill height
const MAX_SHOW = 8;

// Pick a diverse set of relations — round-robin across relation types
// so we don't end up with 8 synonyms and no antonyms shown
function diverseRels(sorted: RelDto[], max: number): RelDto[] {
  const groups = new Map<string, RelDto[]>();
  for (const r of sorted) {
    if (!groups.has(r.relationType)) groups.set(r.relationType, []);
    groups.get(r.relationType)!.push(r);
  }
  const buckets = Array.from(groups.values());
  const result: RelDto[] = [];
  for (let i = 0; result.length < max && buckets.some((b) => b.length > 0); i++) {
    const bucket = buckets[i % buckets.length];
    if (bucket.length > 0) result.push(bucket.shift()!);
  }
  return result;
}

// ── WordCluster card ───────────────────────────────────────────────────────
interface ClusterProps {
  word: Word;
  onRelClick: (rel: RelDto) => void;
  onRemove: () => void;
  flash: boolean;
}

function WordCluster({ word, onRelClick, onRemove, flash }: ClusterProps) {
  const [hoveredRel, setHoveredRel] = useState<number | null>(null);

  const rels = dedupeRels(word.outgoingRelations ?? [], word.incomingRelations ?? []);
  const shown = diverseRels(rels, MAX_SHOW);
  const extra = rels.length - shown.length;
  const N = shown.length;
  const uid = `w${word.id}`;
  const pal = cardPalette(word.id);

  return (
    <div
      className={[
        "group relative flex flex-col rounded-3xl overflow-hidden select-none",
        "transition-all duration-300 ease-out",
        flash
          ? "ring-2 shadow-2xl scale-[1.02]"
          : "ring-1 ring-slate-200 shadow-md hover:shadow-xl hover:scale-[1.01]",
        "bg-white",
      ].join(" ")}
      style={flash ? { boxShadow: `0 0 0 2px ${pal.ring}, 0 20px 40px ${pal.glow}` } : undefined}
    >
      {/* Remove × */}
      <button
        onClick={onRemove}
        className="absolute top-2.5 left-2.5 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-150 w-6 h-6 rounded-full bg-white/80 border border-gray-200 hover:bg-red-50 hover:border-red-300 hover:text-red-500 text-gray-400 text-sm flex items-center justify-center shadow-sm"
        aria-label="Remove"
      >
        ×
      </button>

      {/* Relation count badge — uses palette accent */}
      {N > 0 && (
        <div
          className="absolute top-2.5 right-2.5 z-10 text-white text-[10px] font-bold px-2 py-0.5 rounded-full shadow-md leading-tight"
          style={{ background: pal.ring }}
        >
          {rels.length}
        </div>
      )}

      {/* ── SVG cluster ── */}
      <svg
        viewBox={`0 0 ${VW} ${VH}`}
        width="100%"
        className="block"
        aria-label={`${word.kurdish} cluster`}
      >
        <defs>
          {/* Background tinted with palette colour */}
          <radialGradient id={`bg-${uid}`} cx="50%" cy="44%" r="56%">
            <stop offset="0%" stopColor="#ffffff" />
            <stop offset="100%" stopColor={pal.bg} />
          </radialGradient>
          {/* Hex gradient unique per card */}
          <linearGradient id={`hg-${uid}`} x1="0" y1="0" x2="1" y2="1">
            <stop offset="0%" stopColor={pal.from} />
            <stop offset="100%" stopColor={pal.to} />
          </linearGradient>
          {/* Pill glow filter */}
          <filter id={`pglow-${uid}`} x="-30%" y="-60%" width="160%" height="220%">
            <feGaussianBlur stdDeviation="4" result="blur" />
            <feComposite in="SourceGraphic" in2="blur" operator="over" />
          </filter>
        </defs>

        <rect width={VW} height={VH} fill={`url(#bg-${uid})`} />

        {/* Empty state */}
        {N === 0 && (
          <text x={CCX} y={CCY + HEX_R + 38}
            textAnchor="middle" fill="#94a3b8" fontSize="13"
            fontFamily={FONT}>
            هیچ وشەی پەیوەندیدارێک نییە
          </text>
        )}

        {/* Connecting lines */}
        {shown.map((rel, i) => {
          const { x, y } = radial(i, N, CCX, CCY, REL_R);
          const c = RTYPE[rel.relationType] ?? FB;
          const isHov = hoveredRel === rel.id;
          const lw = isHov ? 2.5 : Math.max(1, 3.2 - (rel.weight - 1) * 0.25);
          return (
            <line key={`l${rel.id}`}
              x1={CCX} y1={CCY} x2={x} y2={y}
              stroke={c.stroke}
              strokeWidth={lw}
              strokeLinecap="round"
              opacity={isHov ? 0.65 : 0.22}
              strokeDasharray={rel.weight > 6 ? "5 3.5" : undefined}
              style={{ transition: "opacity 0.2s, stroke-width 0.2s" }}
            />
          );
        })}

        {/* Related word pills */}
        {shown.map((rel, i) => {
          const { x, y } = radial(i, N, CCX, CCY, REL_R);
          const c = RTYPE[rel.relationType] ?? FB;
          const isHov = hoveredRel === rel.id;
          const barW = Math.max(4, (PW - 14) * (rel.weight / 10));

          return (
            <g key={`r${rel.id}`}
              onClick={() => onRelClick(rel)}
              onMouseEnter={() => setHoveredRel(rel.id)}
              onMouseLeave={() => setHoveredRel(null)}
              className="cursor-pointer"
              role="button" tabIndex={0}
              aria-label={rel.relatedKurdish}
              onKeyDown={(e) => e.key === "Enter" && onRelClick(rel)}
            >
              <rect x={x - PW / 2 - 12} y={y - PH / 2 - 10}
                width={PW + 24} height={PH + 20} fill="transparent" rx={10} />
              {isHov && (
                <rect x={x - PW / 2 - 4} y={y - PH / 2 - 4}
                  width={PW + 8} height={PH + 8} fill={c.glow} rx={12} />
              )}
              <rect x={x - PW / 2 + 1} y={y - PH / 2 + 3}
                width={PW} height={PH}
                fill={c.stroke} opacity={isHov ? 0.15 : 0.07} rx={9} />
              <rect x={x - PW / 2} y={y - PH / 2}
                width={PW} height={PH}
                fill={isHov ? c.stroke : c.fill}
                stroke={c.stroke} strokeWidth={isHov ? 0 : 1.5}
                rx={9} style={{ transition: "fill 0.18s" }} />
              <rect x={x - PW / 2 + 7} y={y + PH / 2 - 6}
                width={barW} height={3}
                fill={isHov ? "rgba(255,255,255,0.5)" : c.stroke}
                opacity={isHov ? 1 : 0.4} rx={1.5} />
              <text x={x} y={y - 1}
                textAnchor="middle" dominantBaseline="middle"
                fill={isHov ? "white" : c.text}
                fontSize="12" fontWeight="700" fontFamily={FONT}
                className="pointer-events-none"
                style={{ transition: "fill 0.18s" }}>
                {clip(rel.relatedKurdish, 8)}
              </text>
            </g>
          );
        })}

        {/* Centre hexagon */}
        <polygon points={hexPts(CCX, CCY, HEX_R + 8)} fill={pal.glow} stroke="none" />
        <polygon points={hexPts(CCX, CCY, HEX_R)}
          fill={`url(#hg-${uid})`} stroke={pal.ring} strokeWidth="2" />
        <polygon points={hexPts(CCX, CCY, HEX_R - 7)}
          fill="none" stroke="rgba(255,255,255,0.25)" strokeWidth="1.5" />

        {/* Center word */}
        <text x={CCX} y={CCY - (word.meaning ? 9 : 0)}
          textAnchor="middle" dominantBaseline="middle"
          fill="white" fontSize="15.5" fontWeight="800" fontFamily={FONT}
          className="pointer-events-none">
          {clip(word.kurdish, 9)}
        </text>
        {word.meaning && (
          <text x={CCX} y={CCY + 12}
            textAnchor="middle" dominantBaseline="middle"
            fill="rgba(255,255,255,0.6)" fontSize="9.5" fontFamily={FONT}
            className="pointer-events-none">
            {clip(word.meaning, 18)}
          </text>
        )}

        {extra > 0 && (
          <text x={VW - 10} y={VH - 10}
            textAnchor="end" fill="#94a3b8" fontSize="10" fontFamily={FONT}>
            +{extra} وشەی دیکە
          </text>
        )}
      </svg>

      {/* ── Footer strip — tinted with palette bg ── */}
      <div className="flex items-center justify-between px-4 pb-3 pt-2 border-t gap-2"
        style={{ borderColor: pal.bg, background: `${pal.bg}80` }}>
        <div dir="rtl" className="min-w-0">
          <span className="text-sm font-bold text-slate-800 truncate block"
            style={{ fontFamily: FONT }}>
            {word.kurdish}
          </span>
          {word.meaning && (
            <span className="text-xs text-slate-400 truncate block"
              style={{ fontFamily: FONT }}>
              {clip(word.meaning, 28)}
            </span>
          )}
        </div>
        {word.category && (
          <span className="shrink-0 text-xs font-semibold px-2.5 py-1 rounded-full border"
            style={{ fontFamily: FONT, background: pal.bg, color: pal.ring, borderColor: `${pal.ring}40` }}>
            {word.category}
          </span>
        )}
      </div>
    </div>
  );
}

// ── Skeleton card (while loading detail) ───────────────────────────────────
function SkeletonCard({ label }: { label: string }) {
  return (
    <div className="flex flex-col rounded-3xl overflow-hidden ring-1 ring-slate-200 bg-white shadow-sm">
      {/* Animated shimmer area */}
      <div className="relative overflow-hidden bg-slate-50" style={{ aspectRatio: `${VW}/${VH}` }}>
        <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/60 to-transparent animate-[shimmer_1.5s_infinite]"
          style={{ backgroundSize: "200% 100%" }} />

        {/* Ghost hex */}
        <svg viewBox={`0 0 ${VW} ${VH}`} width="100%" className="absolute inset-0">
          <polygon
            points={hexPts(CCX, CCY, HEX_R)}
            fill="#e2e8f0"
          />
          <text x={CCX} y={CCY}
            textAnchor="middle" dominantBaseline="middle"
            fill="#94a3b8" fontSize="13" fontFamily={FONT}>
            {clip(label, 9)}
          </text>
          {/* Ghost pills */}
          {Array.from({ length: 5 }, (_, i) => {
            const { x, y } = radial(i, 5, CCX, CCY, REL_R);
            return (
              <rect key={i} x={x - PW / 2} y={y - PH / 2}
                width={PW} height={PH} fill="#e2e8f0" rx={9} />
            );
          })}
        </svg>
      </div>
      <div className="px-4 pb-3 pt-2 border-t border-slate-100">
        <div className="h-4 w-24 bg-slate-200 rounded animate-pulse" />
      </div>
    </div>
  );
}

// ── Main FlowPage ──────────────────────────────────────────────────────────
export default function FlowPage({ initialList }: { initialList: WordBasic[] }) {
  const [slots, setSlots] = useState<Slot[]>(
    initialList.map((w) => ({ kind: "loading", id: w.id, kurdish: w.kurdish }))
  );
  const [flashId, setFlashId] = useState<number | null>(null);
  const [addBusy, setAddBusy] = useState(false);
  const [loadingMore, setLoadingMore] = useState(false);
  const [nextPage, setNextPage] = useState(2);
  const [hasMore, setHasMore] = useState(true);

  // Search
  const [query, setQuery] = useState("");
  const [results, setResults] = useState<WordBasic[]>([]);
  const [showDrop, setShowDrop] = useState(false);
  const debRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const dropRef = useRef<HTMLDivElement>(null);
  const cardRefs = useRef<Map<number, HTMLDivElement>>(new Map());

  // ── Load detail for a single word ID ──────────────────────────────────
  const fetchDetail = useCallback(async (id: number): Promise<Word | null> => {
    try {
      const res = await fetch(`/api/words/${id}`);
      return res.ok ? (res.json() as Promise<Word>) : null;
    } catch { return null; }
  }, []);

  // ── Progressively load details for initial slots ───────────────────────
  useEffect(() => {
    let cancelled = false;
    initialList.forEach(async (item) => {
      const word = await fetchDetail(item.id);
      if (cancelled || !word) return;
      setSlots((prev) =>
        prev.map((s) => (s.kind === "loading" && s.id === item.id
          ? { kind: "loaded", word }
          : s))
      );
    });
    return () => { cancelled = true; };
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []); // run once on mount

  // ── Get current word IDs in grid ──────────────────────────────────────
  const currentIds = useCallback((): Set<number> => {
    const ids = new Set<number>();
    slots.forEach((s) => ids.add(s.kind === "loaded" ? s.word.id : s.id));
    return ids;
  }, [slots]);

  // ── Flash helper ──────────────────────────────────────────────────────
  const flash = useCallback((id: number) => {
    setFlashId(id);
    setTimeout(() => setFlashId((f) => (f === id ? null : f)), 2000);
  }, []);

  // ── Add a word by ID (from clicking a relation pill) ─────────────────
  const addWord = useCallback(async (wordId: number) => {
    // Already in grid → scroll + flash
    if (currentIds().has(wordId)) {
      const el = cardRefs.current.get(wordId);
      el?.scrollIntoView({ behavior: "smooth", block: "center" });
      flash(wordId);
      return;
    }
    setAddBusy(true);
    // Append a loading slot at the end so existing cards stay in place
    setSlots((prev) => [...prev, { kind: "loading", id: wordId, kurdish: "..." }]);
    try {
      const word = await fetchDetail(wordId);
      if (!word) {
        setSlots((prev) => prev.filter((s) => !(s.kind === "loading" && s.id === wordId)));
        return;
      }
      setSlots((prev) =>
        prev.map((s) => (s.kind === "loading" && s.id === wordId
          ? { kind: "loaded", word }
          : s))
      );
      flash(word.id);
      // Scroll to the newly added card, not to the top
      setTimeout(() => {
        const el = cardRefs.current.get(word.id);
        el?.scrollIntoView({ behavior: "smooth", block: "nearest" });
      }, 60);
    } finally {
      setAddBusy(false);
    }
  }, [currentIds, fetchDetail, flash]);

  // ── Remove ────────────────────────────────────────────────────────────
  const removeSlot = useCallback((id: number) => {
    setSlots((prev) => prev.filter((s) => (s.kind === "loaded" ? s.word.id : s.id) !== id));
    cardRefs.current.delete(id);
  }, []);

  // ── Load more pages ───────────────────────────────────────────────────
  const loadMore = useCallback(async () => {
    if (loadingMore || !hasMore) return;
    setLoadingMore(true);
    try {
      const listRes = await fetch(`/api/words?page=${nextPage}&pageSize=6`);
      if (!listRes.ok) { setHasMore(false); return; }
      const list = await listRes.json();
      const items: WordBasic[] = list.items ?? [];
      if (items.length === 0) { setHasMore(false); return; }

      const ids = currentIds();
      const fresh = items.filter((w) => !ids.has(w.id));

      // Add loading placeholders
      const newSlots: Slot[] = fresh.map((w) => ({ kind: "loading", id: w.id, kurdish: w.kurdish }));
      setSlots((prev) => [...prev, ...newSlots]);
      setNextPage((p) => p + 1);
      if (items.length < 6) setHasMore(false);

      // Fetch details progressively
      fresh.forEach(async (item) => {
        const word = await fetchDetail(item.id);
        if (!word) return;
        setSlots((prev) =>
          prev.map((s) => (s.kind === "loading" && s.id === item.id
            ? { kind: "loaded", word }
            : s))
        );
      });
    } finally {
      setLoadingMore(false);
    }
  }, [loadingMore, hasMore, nextPage, currentIds, fetchDetail]);

  // ── Search ────────────────────────────────────────────────────────────
  const onSearch = (v: string) => {
    setQuery(v);
    setShowDrop(true);
    if (debRef.current) clearTimeout(debRef.current);
    if (!v.trim()) { setResults([]); return; }
    debRef.current = setTimeout(async () => {
      try {
        const res = await fetch(`/api/words?search=${encodeURIComponent(v)}&pageSize=8`);
        const d = await res.json();
        setResults(d.items ?? []);
      } catch { setResults([]); }
    }, 240);
  };

  const pickResult = (w: WordBasic) => {
    setShowDrop(false);
    setQuery(w.kurdish);
    addWord(w.id);
  };

  useEffect(() => {
    const h = (e: MouseEvent) => {
      if (dropRef.current && !dropRef.current.contains(e.target as Node))
        setShowDrop(false);
    };
    document.addEventListener("mousedown", h);
    return () => document.removeEventListener("mousedown", h);
  }, []);

  const loadedCount = slots.filter((s) => s.kind === "loaded").length;
  const totalCount = slots.length;

  // ── Render ─────────────────────────────────────────────────────────────
  return (
    <div className="min-h-[100svh] flex flex-col" style={{ background: "var(--background)", fontFamily: FONT }}>

      {/* ── Header ─────────────────────────────────────────────────────── */}
      <header className="sticky top-0 z-40 bg-white/90 backdrop-blur-xl border-b border-slate-200/60 shadow-sm">
        <div className="max-w-7xl mx-auto px-4 py-3 flex items-center gap-4">

          {/* Brand */}
          <div className="shrink-0 flex items-center gap-2.5">
            <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-indigo-500 to-indigo-700 flex items-center justify-center shadow-lg shadow-indigo-300/40">
              <span className="text-white text-base font-black select-none" style={{ fontFamily: FONT }}>ک</span>
            </div>
            <div className="hidden sm:block">
              <p className="text-sm font-bold text-slate-800 leading-tight" style={{ fontFamily: FONT }}>
                فەرهەنگی کوردی
              </p>
              <p className="text-[10px] text-slate-400">Kurdish Dictionary</p>
            </div>
          </div>

          {/* Search */}
          <div ref={dropRef} className="relative flex-1 max-w-2xl mx-auto">
            <div className="relative">
              <svg className="absolute right-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400 pointer-events-none"
                fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.2}
                  d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
              <input
                type="text"
                value={query}
                onChange={(e) => onSearch(e.target.value)}
                onFocus={() => results.length > 0 && setShowDrop(true)}
                placeholder="وشەیەک بگەڕێ...  Search a word"
                dir="rtl"
                style={{ fontFamily: FONT }}
                className="w-full bg-slate-50 border border-slate-200 rounded-2xl pl-4 pr-11 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:bg-white transition-all placeholder:text-slate-400"
              />
            </div>

            {showDrop && results.length > 0 && (
              <ul className="absolute top-full mt-2 left-0 right-0 bg-white border border-slate-200 rounded-2xl shadow-2xl shadow-slate-300/40 overflow-hidden max-h-64 overflow-y-auto z-50">
                {results.map((w) => (
                  <li key={w.id}
                    onMouseDown={() => pickResult(w)}
                    className="px-4 py-2.5 hover:bg-indigo-50 cursor-pointer flex items-center justify-between gap-3 transition-colors border-b border-slate-50 last:border-0">
                    <span className="font-bold text-slate-800 text-sm" dir="rtl"
                      style={{ fontFamily: FONT }}>{w.kurdish}</span>
                    {w.meaning && (
                      <span className="text-xs text-slate-400 truncate max-w-[180px]" dir="rtl"
                        style={{ fontFamily: FONT }}>{w.meaning}</span>
                    )}
                  </li>
                ))}
              </ul>
            )}
          </div>

          {/* Right: stats + spinner */}
          <div className="shrink-0 flex items-center gap-3">
            {totalCount > 0 && (
              <span className="hidden md:block text-xs text-slate-400 tabular-nums">
                {loadedCount}/{totalCount}
              </span>
            )}
            {addBusy && (
              <div className="w-4 h-4 border-2 border-indigo-500 border-t-transparent rounded-full animate-spin" />
            )}
          </div>
        </div>
      </header>

      {/* ── Grid ────────────────────────────────────────────────────────── */}
      <main className="flex-1 max-w-7xl mx-auto w-full px-4 pt-7 pb-20">
        {slots.length > 0 ? (
          <>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-5 items-start">
              {slots.map((slot) => {
                const id = slot.kind === "loaded" ? slot.word.id : slot.id;
                const label = slot.kind === "loaded" ? slot.word.kurdish : slot.kurdish;

                return (
                  <div
                    key={id}
                    ref={(el: HTMLDivElement | null) => {
                      if (el) cardRefs.current.set(id, el);
                      else cardRefs.current.delete(id);
                    }}
                  >
                    {slot.kind === "loaded" ? (
                      <WordCluster
                        word={slot.word}
                        onRelClick={(rel) => addWord(rel.relatedWordId)}
                        onRemove={() => removeSlot(slot.word.id)}
                        flash={flashId === slot.word.id}
                      />
                    ) : (
                      <div className="relative">
                        <SkeletonCard label={label} />
                        <button
                          onClick={() => removeSlot(id)}
                          className="absolute top-2.5 left-2.5 z-10 opacity-0 hover:opacity-100 transition-opacity w-6 h-6 rounded-full bg-white/80 border border-gray-200 text-gray-400 text-sm flex items-center justify-center"
                        >×</button>
                      </div>
                    )}
                  </div>
                );
              })}
            </div>

            {/* Load more */}
            {hasMore && (
              <div className="mt-8 flex justify-center">
                <button
                  onClick={loadMore}
                  disabled={loadingMore}
                  className="flex items-center gap-2 px-7 py-2.5 rounded-2xl border border-slate-300 bg-white text-sm font-semibold text-slate-600 hover:bg-indigo-50 hover:border-indigo-300 hover:text-indigo-700 disabled:opacity-50 transition-all shadow-sm hover:shadow-md"
                  style={{ fontFamily: FONT }}
                >
                  {loadingMore ? (
                    <div className="w-4 h-4 border-2 border-indigo-400 border-t-transparent rounded-full animate-spin" />
                  ) : (
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                    </svg>
                  )}
                  وشەی زیاتر
                </button>
              </div>
            )}
          </>
        ) : (
          /* Empty state */
          <div className="flex flex-col items-center justify-center h-72 gap-5 text-center select-none">
            <div className="w-20 h-20 rounded-3xl bg-indigo-50 flex items-center justify-center">
              <svg className="w-9 h-9 text-indigo-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5}
                  d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            </div>
            <div>
              <p className="text-xl font-bold text-slate-500" dir="rtl" style={{ fontFamily: FONT }}>
                وشەیەک بگەڕێ بۆ دەستپێکردن
              </p>
              <p className="text-sm text-slate-400 mt-1">Search a word to begin</p>
            </div>
          </div>
        )}
      </main>

      {/* ── Legend (fixed to bottom) ────────────────────────────────────── */}
      <footer className="fixed bottom-0 inset-x-0 z-40 bg-white/95 backdrop-blur-md border-t border-slate-200/60 px-4 py-3">
        <div className="max-w-7xl mx-auto flex flex-wrap justify-center items-center gap-x-5 gap-y-1.5">
          {Object.entries(RTYPE).map(([type, c]) => (
            <span key={type} className="flex items-center gap-1.5 text-xs text-slate-500"
              style={{ fontFamily: FONT }}>
              <span className="w-3 h-3 rounded border-2 inline-block"
                style={{ background: c.fill, borderColor: c.stroke }} />
              {c.label}
            </span>
          ))}
          <span className="text-xs text-slate-400 border-l border-slate-200 pl-4 hidden sm:inline"
            style={{ fontFamily: FONT }}>
            ⬡ وشەی بنەڕەت · پیلە = وشەی پەیوەندیدار · کلیک = زیادکردن
          </span>
        </div>
      </footer>

      {/* Shimmer keyframe */}
      <style>{`
        @keyframes shimmer {
          0% { background-position: -200% 0; }
          100% { background-position: 200% 0; }
        }
      `}</style>
    </div>
  );
}
