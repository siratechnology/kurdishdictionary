"use client";

import { useState, useCallback, useEffect, useRef } from "react";
import dynamic from "next/dynamic";
export interface WordBasic { id: number; kurdish: string; meaning?: string; category?: string; }
import { useTheme } from "./ThemeProvider";
import ThemeToggle from "./ThemeToggle";

// Lazy-load MindMap so D3 never runs on the server
const MindMap = dynamic(() => import("./MindMap"), { ssr: false });

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
type Slot =
  | { kind: "loading"; id: number; kurdish: string }
  | { kind: "loaded";  word: Word };

// ── Relation colour palettes (dark / light) ───────────────────────────────
const RTYPE_DARK: Record<string, { fill: string; stroke: string; text: string; glow: string; label: string }> = {
  synonym: { fill: "rgba(34,211,109,0.10)",  stroke: "#22d36d", text: "#86efac", glow: "#22d36d30", label: "هاوماناکە" },
  antonym: { fill: "rgba(248,113,113,0.10)", stroke: "#f87171", text: "#fca5a5", glow: "#f8717130", label: "دژەوشە" },
  related: { fill: "rgba(96,165,250,0.10)",  stroke: "#60a5fa", text: "#93c5fd", glow: "#60a5fa30", label: "پەیوەندیدار" },
  example: { fill: "rgba(251,191,36,0.10)",  stroke: "#fbbf24", text: "#fde68a", glow: "#fbbf2430", label: "نموونە" },
};
const FB_DARK = { fill: "rgba(148,163,184,0.10)", stroke: "#94a3b8", text: "#cbd5e1", glow: "#94a3b830", label: "other" };

const RTYPE_LIGHT: Record<string, { fill: string; stroke: string; text: string; glow: string; label: string }> = {
  synonym: { fill: "rgba(22,163,74,0.13)",  stroke: "#16a34a", text: "#15803d", glow: "#16a34a22", label: "هاوماناکە" },
  antonym: { fill: "rgba(220,38,38,0.13)",  stroke: "#dc2626", text: "#b91c1c", glow: "#dc262622", label: "دژەوشە" },
  related: { fill: "rgba(37,99,235,0.13)",  stroke: "#2563eb", text: "#1d4ed8", glow: "#2563eb22", label: "پەیوەندیدار" },
  example: { fill: "rgba(180,83,9,0.13)",   stroke: "#b45309", text: "#92400e", glow: "#b4530922", label: "نموونە" },
};
const FB_LIGHT = { fill: "rgba(100,116,139,0.13)", stroke: "#475569", text: "#334155", glow: "#47556922", label: "other" };

// ── Card palettes (dark / light) ──────────────────────────────────────────
const CARD_PALETTES_DARK = [
  { from: "#818cf8", to: "#3730a3", ring: "#6366f1", bg: "rgba(30,27,75,0.5)",   glow: "rgba(99,102,241,0.18)"  },
  { from: "#34d399", to: "#065f46", ring: "#10b981", bg: "rgba(6,78,59,0.45)",   glow: "rgba(16,185,129,0.16)"  },
  { from: "#f472b6", to: "#831843", ring: "#ec4899", bg: "rgba(131,24,67,0.40)", glow: "rgba(236,72,153,0.16)"  },
  { from: "#fb923c", to: "#7c2d12", ring: "#f97316", bg: "rgba(124,45,18,0.40)", glow: "rgba(249,115,22,0.16)"  },
  { from: "#38bdf8", to: "#0c4a6e", ring: "#0ea5e9", bg: "rgba(12,74,110,0.40)", glow: "rgba(14,165,233,0.16)"  },
  { from: "#a78bfa", to: "#4c1d95", ring: "#8b5cf6", bg: "rgba(76,29,149,0.40)", glow: "rgba(139,92,246,0.16)"  },
  { from: "#fbbf24", to: "#78350f", ring: "#f59e0b", bg: "rgba(120,53,15,0.40)", glow: "rgba(245,158,11,0.16)"  },
  { from: "#f87171", to: "#7f1d1d", ring: "#ef4444", bg: "rgba(127,29,29,0.40)", glow: "rgba(239,68,68,0.16)"   },
];
const CARD_PALETTES_LIGHT = [
  { from: "#6366f1", to: "#4338ca", ring: "#6366f1", bg: "rgba(238,240,255,0.88)", glow: "rgba(99,102,241,0.12)"  },
  { from: "#059669", to: "#047857", ring: "#10b981", bg: "rgba(236,253,245,0.88)", glow: "rgba(16,185,129,0.10)"  },
  { from: "#db2777", to: "#be185d", ring: "#ec4899", bg: "rgba(253,242,248,0.88)", glow: "rgba(236,72,153,0.10)"  },
  { from: "#ea580c", to: "#c2410c", ring: "#f97316", bg: "rgba(255,247,237,0.88)", glow: "rgba(249,115,22,0.10)"  },
  { from: "#0284c7", to: "#0369a1", ring: "#0ea5e9", bg: "rgba(240,249,255,0.88)", glow: "rgba(14,165,233,0.10)"  },
  { from: "#7c3aed", to: "#6d28d9", ring: "#8b5cf6", bg: "rgba(245,243,255,0.88)", glow: "rgba(139,92,246,0.10)"  },
  { from: "#d97706", to: "#b45309", ring: "#f59e0b", bg: "rgba(255,251,235,0.88)", glow: "rgba(245,158,11,0.10)"  },
  { from: "#dc2626", to: "#b91c1c", ring: "#ef4444", bg: "rgba(254,242,242,0.88)", glow: "rgba(239,68,68,0.10)"   },
];

const FONT = "'NRT', system-ui, sans-serif";

// ── Helpers ────────────────────────────────────────────────────────────────
function clip(s: string | undefined, n: number): string {
  if (!s) return "?";
  return s.length > n ? s.slice(0, n - 1) + "\u2026" : s;
}
function hexPts(cx: number, cy: number, r: number): string {
  return Array.from({ length: 6 }, (_, i) => {
    const a = (Math.PI / 3) * i;
    return `${(cx + r * Math.cos(a)).toFixed(1)},${(cy + r * Math.sin(a)).toFixed(1)}`;
  }).join(" ");
}
function radial(i: number, n: number, cx: number, cy: number, r: number) {
  const a = (2 * Math.PI * i) / Math.max(n, 1) - Math.PI / 2;
  return { x: cx + r * Math.cos(a), y: cy + r * Math.sin(a) };
}
function dedupeRels(out: RelDto[], inc: RelDto[]): RelDto[] {
  const map = new Map<number, RelDto>();
  for (const r of out) map.set(r.relatedWordId, r);
  for (const r of inc) if (!map.has(r.relatedWordId)) map.set(r.relatedWordId, r);
  return Array.from(map.values()).sort((a, b) => a.weight - b.weight);
}
function diverseRels(sorted: RelDto[], max: number): RelDto[] {
  const groups = new Map<string, RelDto[]>();
  for (const r of sorted) {
    if (!groups.has(r.relationType)) groups.set(r.relationType, []);
    groups.get(r.relationType)!.push(r);
  }
  const buckets = Array.from(groups.values());
  const result: RelDto[] = [];
  for (let i = 0; result.length < max && buckets.some((b) => b.length > 0); i++) {
    const b = buckets[i % buckets.length];
    if (b.length > 0) result.push(b.shift()!);
  }
  return result;
}

// ── SVG constants ──────────────────────────────────────────────────────────
const VW = 420, VH = 390, CCX = 210, CCY = 182;
const HEX_R = 52, REL_R = 143, PW = 84, PH = 32, MAX_SHOW = 8;

// ── MindMap Modal ─────────────────────────────────────────────────────────
interface MapModalProps {
  word: Word;
  onClose: () => void;
  onNodeAdd: (id: number) => void;
}
function MindMapModal({ word, onClose, onNodeAdd }: MapModalProps) {
  const { theme } = useTheme();
  const isDark = theme === "dark";

  useEffect(() => {
    const h = (e: KeyboardEvent) => { if (e.key === "Escape") onClose(); };
    document.addEventListener("keydown", h);
    return () => document.removeEventListener("keydown", h);
  }, [onClose]);

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4"
         role="dialog" aria-modal="true" aria-label="Mind Map">
      {/* Backdrop */}
      <div
        className="absolute inset-0 backdrop-blur-sm"
        style={{ background: isDark ? "rgba(0,0,0,0.72)" : "rgba(15,23,42,0.45)" }}
        onClick={onClose}
      />

      {/* Panel */}
      <div className="relative w-full max-w-4xl rounded-2xl overflow-hidden shadow-2xl"
           style={{
             background: "var(--t-modal-bg)",
             border: "1px solid var(--t-border-2)",
             boxShadow: isDark
               ? "0 0 80px rgba(99,102,241,0.12)"
               : "0 0 60px rgba(99,102,241,0.10)",
           }}>

        {/* Header */}
        <div className="flex items-center justify-between px-5 py-3.5"
             style={{ borderBottom: "1px solid var(--t-border-1)" }}>
          <div dir="rtl" style={{ fontFamily: FONT }}>
            <span className="text-base font-bold" style={{ color: "var(--t-text-1)" }}>
              {word.kurdish}
            </span>
            {word.meaning && (
              <span className="ml-2 text-xs" style={{ color: "var(--t-text-4)" }}>
                {clip(word.meaning, 40)}
              </span>
            )}
          </div>
          <button
            onClick={onClose}
            className="w-7 h-7 flex items-center justify-center rounded-full transition-colors text-lg"
            style={{
              color: "var(--t-text-4)",
              background: "transparent",
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.color = "var(--t-text-1)";
              e.currentTarget.style.background = isDark ? "rgba(255,255,255,0.06)" : "rgba(0,0,0,0.06)";
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.color = "var(--t-text-4)";
              e.currentTarget.style.background = "transparent";
            }}
          >
            ×
          </button>
        </div>

        {/* D3 Graph */}
        <div className="p-3">
          <MindMap rootWordId={word.id} onNodeAdd={onNodeAdd} height={500} />
        </div>

        {/* Footer hint */}
        <p className="text-center text-[10px] pb-3" style={{ color: "var(--t-text-5)", fontFamily: FONT }}>
          کلیک = زیادکردن بۆ کارتەکان &nbsp;·&nbsp; دووجار کلیک = فراوانکردنی نۆد
        </p>
      </div>
    </div>
  );
}

// ── WordCluster (single card) ──────────────────────────────────────────────
interface ClusterProps {
  word: Word;
  onRelClick: (rel: RelDto) => void;
  onRemove: () => void;
  onMapOpen: () => void;
  flash: boolean;
}
function WordCluster({ word, onRelClick, onRemove, onMapOpen, flash }: ClusterProps) {
  const { theme } = useTheme();
  const isDark = theme === "dark";
  const RTYPE = isDark ? RTYPE_DARK : RTYPE_LIGHT;
  const FB    = isDark ? FB_DARK    : FB_LIGHT;
  const pals  = isDark ? CARD_PALETTES_DARK : CARD_PALETTES_LIGHT;

  const [hoveredRel, setHoveredRel] = useState<number | null>(null);
  const rels  = dedupeRels(word.outgoingRelations ?? [], word.incomingRelations ?? []);
  const shown = diverseRels(rels, MAX_SHOW);
  const extra = rels.length - shown.length;
  const N     = shown.length;
  const uid   = `w${word.id}`;
  const pal   = pals[word.id % pals.length];

  const dimTextColor     = isDark ? "#475569"              : "#94a3b8";
  const hexInnerStroke   = isDark ? "rgba(255,255,255,0.15)" : "rgba(255,255,255,0.35)";
  const hexMeaningFill   = isDark ? "rgba(255,255,255,0.50)" : "rgba(255,255,255,0.75)";
  const weightBarHover   = isDark ? "rgba(255,255,255,0.45)" : "rgba(0,0,0,0.5)";
  const pillHoverText    = isDark ? "#ffffff"              : "#ffffff";

  return (
    <div
      className={[
        "group relative flex flex-col rounded-3xl overflow-hidden select-none",
        "transition-all duration-300 ease-out",
      ].join(" ")}
      style={{
        background: pal.bg,
        backdropFilter: "blur(20px) saturate(160%)",
        WebkitBackdropFilter: "blur(20px) saturate(160%)",
        border: flash
          ? `1px solid ${pal.ring}`
          : "1px solid var(--t-border-1)",
        boxShadow: flash
          ? `0 0 0 1px ${pal.ring}40, 0 20px 50px ${pal.glow}`
          : isDark
            ? `0 4px 24px rgba(0,0,0,0.4), inset 0 1px 0 rgba(255,255,255,0.03)`
            : `0 4px 24px rgba(99,102,241,0.08), inset 0 1px 0 rgba(255,255,255,0.6)`,
        transform: flash ? "scale(1.015)" : undefined,
      }}>

      {/* Remove × */}
      <button
        onClick={onRemove}
        className="absolute top-2.5 left-2.5 z-10 opacity-0 group-hover:opacity-100 transition-opacity duration-150 w-6 h-6 rounded-full flex items-center justify-center text-sm"
        style={{
          background: "var(--t-remove-btn-bg)",
          border: "1px solid var(--t-remove-btn-border)",
          color: "var(--t-text-4)",
        }}
        onMouseEnter={(e) => { e.currentTarget.style.color = "#f87171"; }}
        onMouseLeave={(e) => { e.currentTarget.style.color = "var(--t-text-4)"; }}
        aria-label="Remove">
        ×
      </button>

      {/* Relation count badge */}
      {N > 0 && (
        <div className="absolute top-2.5 right-2.5 z-10 text-white text-[10px] font-bold px-2 py-0.5 rounded-full shadow-md leading-tight"
             style={{ background: pal.ring }}>
          {rels.length}
        </div>
      )}

      {/* ── SVG cluster ── */}
      <svg viewBox={`0 0 ${VW} ${VH}`} width="100%" className="block"
           aria-label={`${word.kurdish} cluster`}>
        <defs>
          <radialGradient id={`bg-${uid}`} cx="50%" cy="44%" r="56%">
            <stop offset="0%" stopColor="rgba(30,41,59,0.0)" />
            <stop offset="100%" stopColor={pal.bg} />
          </radialGradient>
          <linearGradient id={`hg-${uid}`} x1="0" y1="0" x2="1" y2="1">
            <stop offset="0%" stopColor={pal.from} />
            <stop offset="100%" stopColor={pal.to} />
          </linearGradient>
          <filter id={`hglow-${uid}`} x="-50%" y="-50%" width="200%" height="200%">
            <feGaussianBlur stdDeviation="6" result="blur" />
            <feMerge><feMergeNode in="blur" /><feMergeNode in="SourceGraphic" /></feMerge>
          </filter>
        </defs>

        <rect width={VW} height={VH} fill="transparent" />

        {/* Empty state */}
        {N === 0 && (
          <text x={CCX} y={CCY + HEX_R + 38} textAnchor="middle"
                style={{ fill: dimTextColor }} fontSize="13" fontFamily={FONT}>
            هیچ وشەی پەیوەندیدارێک نییە
          </text>
        )}

        {/* Connecting lines */}
        {shown.map((rel, i) => {
          const { x, y } = radial(i, N, CCX, CCY, REL_R);
          const c = RTYPE[rel.relationType] ?? FB;
          const isHov = hoveredRel === rel.id;
          const lw = isHov ? 2 : Math.max(1, 2.8 - (rel.weight - 1) * 0.2);
          return (
            <line key={`l${rel.id}`}
              x1={CCX} y1={CCY} x2={x} y2={y}
              stroke={c.stroke} strokeWidth={lw} strokeLinecap="round"
              opacity={isHov ? 0.75 : 0.28}
              strokeDasharray={rel.weight > 5 ? "5 3.5" : "6 0"}
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
               role="button" tabIndex={0} aria-label={rel.relatedKurdish}
               onKeyDown={(e) => e.key === "Enter" && onRelClick(rel)}>
              <rect x={x - PW / 2 - 12} y={y - PH / 2 - 10}
                    width={PW + 24} height={PH + 20} fill="transparent" rx={10} />
              {isHov && (
                <rect x={x - PW / 2 - 4} y={y - PH / 2 - 4}
                      width={PW + 8} height={PH + 8} fill={c.glow} rx={12} />
              )}
              <rect x={x - PW / 2 + 1} y={y - PH / 2 + 3}
                    width={PW} height={PH} fill={c.stroke}
                    opacity={isHov ? 0.12 : 0.06} rx={9} />
              <rect x={x - PW / 2} y={y - PH / 2}
                    width={PW} height={PH}
                    fill={isHov ? c.stroke : c.fill}
                    stroke={c.stroke} strokeWidth={isHov ? 0 : 1.2}
                    rx={9} style={{ transition: "fill 0.18s" }} />
              <rect x={x - PW / 2 + 7} y={y + PH / 2 - 6}
                    width={barW} height={2.5}
                    fill={isHov ? weightBarHover : c.stroke}
                    opacity={isHov ? 1 : 0.35} rx={1.5} />
              <text x={x} y={y - 1}
                    textAnchor="middle" dominantBaseline="middle"
                    fill={isHov ? pillHoverText : c.text}
                    fontSize="11.5" fontWeight="700" fontFamily={FONT}
                    className="pointer-events-none"
                    style={{ transition: "fill 0.18s" }}>
                {clip(rel.relatedKurdish, 8)}
              </text>
            </g>
          );
        })}

        {/* Centre hexagon — outer glow */}
        <polygon points={hexPts(CCX, CCY, HEX_R + 10)}
                 fill={pal.glow} stroke="none"
                 filter={`url(#hglow-${uid})`} />
        {/* main hex */}
        <polygon points={hexPts(CCX, CCY, HEX_R)}
                 fill={`url(#hg-${uid})`} stroke={pal.ring} strokeWidth="1.5" />
        {/* inner shine */}
        <polygon points={hexPts(CCX, CCY, HEX_R - 7)}
                 fill="none" stroke={hexInnerStroke} strokeWidth="1.2" />

        {/* Center word */}
        <text x={CCX} y={CCY - (word.meaning ? 9 : 0)}
              textAnchor="middle" dominantBaseline="middle"
              fill="white" fontSize="15" fontWeight="800" fontFamily={FONT}
              className="pointer-events-none">
          {clip(word.kurdish, 9)}
        </text>
        {word.meaning && (
          <text x={CCX} y={CCY + 12}
                textAnchor="middle" dominantBaseline="middle"
                fill={hexMeaningFill} fontSize="9" fontFamily={FONT}
                className="pointer-events-none">
            {clip(word.meaning, 20)}
          </text>
        )}

        {extra > 0 && (
          <text x={VW - 10} y={VH - 10}
                textAnchor="end" style={{ fill: dimTextColor }} fontSize="10" fontFamily={FONT}>
            +{extra} وشەی دیکە
          </text>
        )}
      </svg>

      {/* ── Footer ── */}
      <div className="flex items-center justify-between px-4 pb-3 pt-2 gap-2"
           style={{ borderTop: "1px solid var(--t-border-subtle)" }}>
        <div dir="rtl" className="min-w-0">
          <span className="text-sm font-bold truncate block"
                style={{ fontFamily: FONT, color: "var(--t-text-1)" }}>
            {word.kurdish}
          </span>
          {word.meaning && (
            <span className="text-xs truncate block"
                  style={{ fontFamily: FONT, color: "var(--t-text-4)" }}>
              {clip(word.meaning, 28)}
            </span>
          )}
        </div>

        <div className="flex items-center gap-2 shrink-0">
          {word.category && (
            <span className="text-[11px] font-semibold px-2 py-0.5 rounded-full"
                  style={{
                    fontFamily: FONT,
                    background: `${pal.ring}18`,
                    color: pal.from,
                    border: `1px solid ${pal.ring}30`,
                  }}>
              {word.category}
            </span>
          )}
          {/* Mind Map button */}
          <button
            onClick={onMapOpen}
            title="نەخشەی مایند"
            className="w-7 h-7 flex items-center justify-center rounded-xl transition-all hover:scale-110"
            style={{
              background: "var(--t-load-more-bg)",
              border: "1px solid var(--t-load-more-border)",
              color: "var(--t-accent-color)",
            }}>
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <circle cx="12" cy="12" r="3" />
              <circle cx="4"  cy="6"  r="2" /><line x1="4"  y1="8"  x2="10" y2="11" />
              <circle cx="20" cy="6"  r="2" /><line x1="20" y1="8"  x2="14" y2="11" />
              <circle cx="4"  cy="18" r="2" /><line x1="4"  y1="16" x2="10" y2="13" />
              <circle cx="20" cy="18" r="2" /><line x1="20" y1="16" x2="14" y2="13" />
            </svg>
          </button>
        </div>
      </div>
    </div>
  );
}

// ── Skeleton card ──────────────────────────────────────────────────────────
function SkeletonCard({ label }: { label: string }) {
  const { theme } = useTheme();
  const isDark = theme === "dark";
  return (
    <div className="flex flex-col rounded-3xl overflow-hidden"
         style={{
           background: "var(--t-skeleton-bg)",
           border: "1px solid var(--t-border-1)",
         }}>
      <div className="relative overflow-hidden" style={{ aspectRatio: `${VW}/${VH}` }}>
        <div className="absolute inset-0 bg-gradient-to-r from-transparent to-transparent animate-[shimmer_1.6s_infinite]"
             style={{
               backgroundImage: `linear-gradient(90deg, transparent 0%, var(--t-shimmer) 50%, transparent 100%)`,
               backgroundSize: "200% 100%",
             }} />
        <svg viewBox={`0 0 ${VW} ${VH}`} width="100%" className="absolute inset-0">
          <polygon points={hexPts(CCX, CCY, HEX_R)} style={{ fill: "var(--t-skeleton-cell)" }} />
          <text x={CCX} y={CCY} textAnchor="middle" dominantBaseline="middle"
                style={{ fill: isDark ? "#334155" : "#94a3b8" }} fontSize="13" fontFamily={FONT}>
            {clip(label, 9)}
          </text>
          {Array.from({ length: 5 }, (_, i) => {
            const { x, y } = radial(i, 5, CCX, CCY, REL_R);
            return <rect key={i} x={x - PW / 2} y={y - PH / 2}
                         width={PW} height={PH} style={{ fill: "var(--t-skeleton-cell)" }} rx={9} />;
          })}
        </svg>
      </div>
      <div className="px-4 pb-3 pt-2" style={{ borderTop: "1px solid var(--t-border-subtle)" }}>
        <div className="h-3.5 w-24 rounded animate-pulse" style={{ background: "var(--t-skeleton-cell)" }} />
      </div>
    </div>
  );
}

// ── Main FlowPage ──────────────────────────────────────────────────────────
export default function FlowPage({ initialList }: { initialList: WordBasic[] }) {
  const { theme } = useTheme();
  const isDark = theme === "dark";
  const RTYPE  = isDark ? RTYPE_DARK  : RTYPE_LIGHT;

  const [slots,       setSlots]       = useState<Slot[]>(
    initialList.map((w) => ({ kind: "loading", id: w.id, kurdish: w.kurdish }))
  );
  const [flashId,     setFlashId]     = useState<number | null>(null);
  const [addBusy,     setAddBusy]     = useState(false);
  const [loadingMore, setLoadingMore] = useState(false);
  const [nextPage,    setNextPage]    = useState(2);
  const [hasMore,     setHasMore]     = useState(true);
  const [mapWord,     setMapWord]     = useState<Word | null>(null);

  // Search
  const [query,    setQuery]    = useState("");
  const [results,  setResults]  = useState<WordBasic[]>([]);
  const [showDrop, setShowDrop] = useState(false);
  const debRef   = useRef<ReturnType<typeof setTimeout> | null>(null);
  const dropRef  = useRef<HTMLDivElement>(null);
  const cardRefs = useRef<Map<number, HTMLDivElement>>(new Map());

  // ── Fetch word detail ───────────────────────────────────────────────────
  const fetchDetail = useCallback(async (id: number): Promise<Word | null> => {
    try {
      const res = await fetch(`/api/words/${id}`);
      return res.ok ? (res.json() as Promise<Word>) : null;
    } catch { return null; }
  }, []);

  // ── Load initial slots ──────────────────────────────────────────────────
  useEffect(() => {
    let cancelled = false;
    initialList.forEach(async (item) => {
      const word = await fetchDetail(item.id);
      if (cancelled || !word) return;
      setSlots((prev) =>
        prev.map((s) => s.kind === "loading" && s.id === item.id ? { kind: "loaded", word } : s)
      );
    });
    return () => { cancelled = true; };
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const currentIds = useCallback((): Set<number> => {
    const ids = new Set<number>();
    slots.forEach((s) => ids.add(s.kind === "loaded" ? s.word.id : s.id));
    return ids;
  }, [slots]);

  const flash = useCallback((id: number) => {
    setFlashId(id);
    setTimeout(() => setFlashId((f) => (f === id ? null : f)), 2000);
  }, []);

  // ── Add word ────────────────────────────────────────────────────────────
  const addWord = useCallback(async (wordId: number) => {
    if (currentIds().has(wordId)) {
      const el = cardRefs.current.get(wordId);
      el?.scrollIntoView({ behavior: "smooth", block: "center" });
      flash(wordId);
      return;
    }
    setAddBusy(true);
    setSlots((prev) => [...prev, { kind: "loading", id: wordId, kurdish: "..." }]);
    try {
      const word = await fetchDetail(wordId);
      if (!word) {
        setSlots((prev) => prev.filter((s) => !(s.kind === "loading" && s.id === wordId)));
        return;
      }
      setSlots((prev) =>
        prev.map((s) => s.kind === "loading" && s.id === wordId ? { kind: "loaded", word } : s)
      );
      flash(word.id);
      setTimeout(() => {
        cardRefs.current.get(word.id)?.scrollIntoView({ behavior: "smooth", block: "nearest" });
      }, 60);
    } finally { setAddBusy(false); }
  }, [currentIds, fetchDetail, flash]);

  const removeSlot = useCallback((id: number) => {
    setSlots((prev) => prev.filter((s) => (s.kind === "loaded" ? s.word.id : s.id) !== id));
    cardRefs.current.delete(id);
  }, []);

  // ── Load more ───────────────────────────────────────────────────────────
  const loadMore = useCallback(async () => {
    if (loadingMore || !hasMore) return;
    setLoadingMore(true);
    try {
      const listRes = await fetch(`/api/words?page=${nextPage}&pageSize=6`);
      if (!listRes.ok) { setHasMore(false); return; }
      const list = await listRes.json();
      const items: WordBasic[] = list.items ?? [];
      if (items.length === 0) { setHasMore(false); return; }

      const ids   = currentIds();
      const fresh = items.filter((w) => !ids.has(w.id));
      setSlots((prev) => [...prev, ...fresh.map((w): Slot => ({ kind: "loading", id: w.id, kurdish: w.kurdish }))]);
      setNextPage((p) => p + 1);
      if (items.length < 6) setHasMore(false);

      fresh.forEach(async (item) => {
        const word = await fetchDetail(item.id);
        if (!word) return;
        setSlots((prev) =>
          prev.map((s) => s.kind === "loading" && s.id === item.id ? { kind: "loaded", word } : s)
        );
      });
    } finally { setLoadingMore(false); }
  }, [loadingMore, hasMore, nextPage, currentIds, fetchDetail]);

  // ── Search ──────────────────────────────────────────────────────────────
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
  const totalCount  = slots.length;

  // ── Render ─────────────────────────────────────────────────────────────
  return (
    <>
      {/* ── Mind Map Modal ─────────────────────────────────────────────── */}
      {mapWord && (
        <MindMapModal
          word={mapWord}
          onClose={() => setMapWord(null)}
          onNodeAdd={(id) => { addWord(id); setMapWord(null); }}
        />
      )}

      <div className="relative min-h-[100svh] flex flex-col" style={{ fontFamily: FONT }}>

        {/* ── Header ──────────────────────────────────────────────────── */}
        <header className="sticky top-0 z-40 shadow-lg"
                style={{
                  background: "var(--t-header-bg)",
                  backdropFilter: "blur(24px) saturate(180%)",
                  WebkitBackdropFilter: "blur(24px) saturate(180%)",
                  borderBottom: "1px solid var(--t-border-1)",
                  transition: "background 0.4s ease, border-color 0.3s ease",
                }}>
          <div className="max-w-7xl mx-auto px-4 py-3 flex items-center gap-4">

            {/* Brand */}
            <div className="shrink-0 flex items-center gap-2.5">
              <div className="w-9 h-9 rounded-xl flex items-center justify-center shadow-lg"
                   style={{
                     background: "linear-gradient(135deg, #818cf8, #4338ca)",
                     boxShadow: "0 4px 16px rgba(99,102,241,0.40)",
                   }}>
                <span className="text-white text-base font-black select-none" style={{ fontFamily: FONT }}>ک</span>
              </div>
              <div className="hidden sm:block">
                <p className="text-sm font-bold leading-tight" style={{ fontFamily: FONT, color: "var(--t-text-1)" }}>
                  فەرهەنگی کوردی
                </p>
                <p className="text-[10px]" style={{ color: "var(--t-text-4)" }}>Kurdish Dictionary</p>
              </div>
            </div>

            {/* Search */}
            <div ref={dropRef} className="relative flex-1 max-w-2xl mx-auto">
              <div className="relative">
                <svg className="absolute right-3.5 top-1/2 -translate-y-1/2 w-4 h-4 pointer-events-none"
                     style={{ color: "var(--t-text-4)" }}
                     fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.2}
                        d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
                <input
                  type="text" value={query}
                  onChange={(e) => onSearch(e.target.value)}
                  onFocus={(e) => {
                    results.length > 0 && setShowDrop(true);
                    e.currentTarget.style.borderColor = "var(--t-input-border-focus)";
                  }}
                  onBlur={(e) => {
                    e.currentTarget.style.borderColor = "var(--t-input-border)";
                  }}
                  placeholder="وشەیەک بگەڕێ...  Search a word"
                  dir="rtl"
                  className="w-full rounded-2xl pl-4 pr-11 py-2.5 text-sm outline-none transition-all"
                  style={{
                    fontFamily: FONT,
                    background: "var(--t-input-bg)",
                    backdropFilter: "blur(16px)",
                    WebkitBackdropFilter: "blur(16px)",
                    border: "1px solid var(--t-input-border)",
                    color: "var(--t-input-color)",
                    caretColor: "#818cf8",
                  }}
                />
              </div>

              {/* Dropdown */}
              {showDrop && results.length > 0 && (
                <ul className="absolute top-full mt-2 left-0 right-0 rounded-2xl overflow-hidden max-h-64 overflow-y-auto z-50"
                    style={{
                      background: "var(--t-dropdown-bg)",
                      backdropFilter: "blur(28px) saturate(200%)",
                      WebkitBackdropFilter: "blur(28px) saturate(200%)",
                      border: "1px solid var(--t-border-2)",
                      boxShadow: isDark
                        ? "0 20px 60px rgba(0,0,0,0.5)"
                        : "0 20px 50px rgba(99,102,241,0.12)",
                    }}>
                  {results.map((w) => (
                    <li key={w.id} onMouseDown={() => pickResult(w)}
                        className="px-4 py-2.5 cursor-pointer flex items-center justify-between gap-3 transition-colors"
                        style={{ borderBottom: "1px solid var(--t-dropdown-divider)" }}
                        onMouseEnter={(e) => (e.currentTarget.style.background = "var(--t-dropdown-hover)")}
                        onMouseLeave={(e) => (e.currentTarget.style.background = "transparent")}>
                      <span className="font-bold text-sm" dir="rtl"
                            style={{ fontFamily: FONT, color: "var(--t-text-1)" }}>
                        {w.kurdish}
                      </span>
                      {w.meaning && (
                        <span className="text-xs truncate max-w-[180px]" dir="rtl"
                              style={{ fontFamily: FONT, color: "var(--t-text-4)" }}>
                          {w.meaning}
                        </span>
                      )}
                    </li>
                  ))}
                </ul>
              )}
            </div>

            {/* Right controls: stats + spinner + theme toggle */}
            <div className="shrink-0 flex items-center gap-3">
              {totalCount > 0 && (
                <span className="hidden md:block text-xs tabular-nums" style={{ color: "var(--t-text-4)" }}>
                  {loadedCount}/{totalCount}
                </span>
              )}
              {addBusy && (
                <div className="w-4 h-4 border-2 rounded-full animate-spin"
                     style={{
                       borderColor: "var(--t-load-more-border)",
                       borderTopColor: "var(--t-accent-color)",
                     }} />
              )}
              <ThemeToggle />
            </div>
          </div>
        </header>

        {/* ── Grid ────────────────────────────────────────────────────── */}
        <main className="relative z-10 flex-1 max-w-7xl mx-auto w-full px-4 pt-7 pb-24">
          {slots.length > 0 ? (
            <>
              <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-5 items-start">
                {slots.map((slot) => {
                  const id    = slot.kind === "loaded" ? slot.word.id : slot.id;
                  const label = slot.kind === "loaded" ? slot.word.kurdish : slot.kurdish;
                  return (
                    <div key={id}
                         ref={(el: HTMLDivElement | null) => {
                           if (el) cardRefs.current.set(id, el);
                           else cardRefs.current.delete(id);
                         }}>
                      {slot.kind === "loaded" ? (
                        <WordCluster
                          word={slot.word}
                          onRelClick={(rel) => addWord(rel.relatedWordId)}
                          onRemove={() => removeSlot(slot.word.id)}
                          onMapOpen={() => setMapWord(slot.word)}
                          flash={flashId === slot.word.id}
                        />
                      ) : (
                        <div className="relative">
                          <SkeletonCard label={label} />
                          <button onClick={() => removeSlot(id)}
                                  className="absolute top-2.5 left-2.5 z-10 opacity-0 hover:opacity-100 transition-opacity w-6 h-6 rounded-full flex items-center justify-center text-sm"
                                  style={{
                                    background: "var(--t-remove-btn-bg)",
                                    border: "1px solid var(--t-remove-btn-border)",
                                    color: "var(--t-text-4)",
                                  }}>
                            ×
                          </button>
                        </div>
                      )}
                    </div>
                  );
                })}
              </div>

              {/* Load more */}
              {hasMore && (
                <div className="mt-8 flex justify-center">
                  <button onClick={loadMore} disabled={loadingMore}
                          className="flex items-center gap-2 px-7 py-2.5 rounded-2xl text-sm font-semibold transition-all disabled:opacity-40"
                          style={{
                            fontFamily: FONT,
                            background: "var(--t-load-more-bg)",
                            border: "1px solid var(--t-load-more-border)",
                            color: "var(--t-accent-color)",
                          }}
                          onMouseEnter={(e) => (e.currentTarget.style.background = "var(--t-load-more-hover)")}
                          onMouseLeave={(e) => (e.currentTarget.style.background = "var(--t-load-more-bg)")}>
                    {loadingMore ? (
                      <div className="w-4 h-4 border-2 rounded-full animate-spin"
                           style={{
                             borderColor: "var(--t-load-more-border)",
                             borderTopColor: "var(--t-accent-color)",
                           }} />
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
              <div className="w-20 h-20 rounded-3xl flex items-center justify-center"
                   style={{
                     background: "var(--t-empty-icon-bg)",
                     border: "1px solid var(--t-empty-icon-border)",
                   }}>
                <svg className="w-9 h-9" style={{ color: "var(--t-empty-icon-color)" }}
                     fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5}
                        d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
              </div>
              <div>
                <p className="text-xl font-bold" dir="rtl"
                   style={{ fontFamily: FONT, color: "var(--t-text-3)" }}>
                  وشەیەک بگەڕێ بۆ دەستپێکردن
                </p>
                <p className="text-sm mt-1" style={{ color: "var(--t-text-5)" }}>Search a word to begin</p>
              </div>
            </div>
          )}
        </main>

        {/* ── Footer legend ────────────────────────────────────────────── */}
        <footer className="fixed bottom-0 inset-x-0 z-40"
                style={{
                  background: "var(--t-footer-bg)",
                  backdropFilter: "blur(16px)",
                  WebkitBackdropFilter: "blur(16px)",
                  borderTop: "1px solid var(--t-border-1)",
                  transition: "background 0.4s ease",
                }}>
          <div className="max-w-7xl mx-auto px-4 py-2.5 flex flex-wrap justify-center items-center gap-x-5 gap-y-1.5">
            {Object.entries(RTYPE).map(([type, c]) => (
              <span key={type} className="flex items-center gap-1.5 text-xs"
                    style={{ color: "var(--t-text-4)", fontFamily: FONT }}>
                <span className="w-2.5 h-2.5 rounded border inline-block"
                      style={{ background: c.fill, borderColor: c.stroke }} />
                <span style={{ color: c.stroke }}>{c.label}</span>
              </span>
            ))}
            <span className="text-[10px] border-l pl-4 hidden sm:inline"
                  style={{
                    color: "var(--t-text-5)",
                    borderColor: "var(--t-border-1)",
                    fontFamily: FONT,
                  }}>
              ⬡ وشەی بنەڕەت &nbsp;·&nbsp; 🕸 = مایند ماپ &nbsp;·&nbsp; کلیک = زیادکردن
            </span>
          </div>
        </footer>
      </div>
    </>
  );
}
