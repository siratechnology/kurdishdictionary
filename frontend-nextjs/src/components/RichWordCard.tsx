"use client";

import dynamic from "next/dynamic";
import { REL5_COLOR } from "./MiniMap";

const MiniMap = dynamic(() => import("./MiniMap"), { ssr: false });

// ── Types ─────────────────────────────────────────────────────────────────────

export interface WordItem {
  id: number;
  kurdish: string;
  speechPane: number;
  speechPaneKurdish?: string;
  category?: string;
  description?: string;
  totalRelations: number;
  meanings: { id: number; meaning: string; locate?: string }[];
}

// ── Lookup tables ──────────────────────────────────────────────────────────────

const CAT_ICON: Record<string, string> = {
  "خواردن": "🍽", "ئامراز": "🔧", "ئاژەڵ": "🐾", "ئەدەب": "📚",
  "زانست": "🔬", "جوانکاری": "🎨", "کۆمەڵ": "👥", "ئاسمان": "🌙",
  "دار": "🌿",   "دەنگ": "🎵",  "ئاو": "💧",   "گوند": "🏡",
  "وەرزش": "⚽", "جوگرافیا": "🗺", "ناوچە": "📍",
};
const catIcon = (c: string) => CAT_ICON[c] ?? "📌";

const SP_BG:   Record<number, string> = { 1:"rgba(6,182,212,.14)",  2:"rgba(14,165,233,.14)", 3:"rgba(59,130,246,.14)", 4:"rgba(34,211,238,.14)" };
const SP_BD:   Record<number, string> = { 1:"rgba(6,182,212,.38)",  2:"rgba(14,165,233,.38)", 3:"rgba(59,130,246,.38)", 4:"rgba(34,211,238,.38)" };
const SP_TEXT: Record<number, string> = { 1:"#22d3ee", 2:"#38bdf8", 3:"#60a5fa", 4:"#22d3ee" };

// ── Dialect grouping ───────────────────────────────────────────────────────────

function groupByDialect(meanings: WordItem["meanings"]) {
  const map = new Map<string, string[]>();
  meanings.forEach(m => {
    const k = m.locate?.trim() || "—";
    if (!map.has(k)) map.set(k, []);
    map.get(k)!.push(m.meaning);
  });
  return Array.from(map.entries());
}

// ── Relation legend (5 types) ──────────────────────────────────────────────────

const REL5 = [
  { key: "synonym",    label: "هاومانا",   color: REL5_COLOR.synonym    },
  { key: "antonym",    label: "دژواتا",    color: REL5_COLOR.antonym    },
  { key: "partof",     label: "بەشێکن",    color: REL5_COLOR.partof     },
  { key: "related",    label: "پەیوەندی",  color: REL5_COLOR.related    },
  { key: "contextual", label: "بەیەکەوە", color: REL5_COLOR.contextual },
];

// ── Component ──────────────────────────────────────────────────────────────────

interface Props { word: WordItem; onExplore: (id: number) => void; }

export default function RichWordCard({ word, onExplore }: Props) {
  const dialects  = groupByDialect(word.meanings);
  const spBg      = SP_BG[word.speechPane]   ?? "rgba(99,102,241,.14)";
  const spBd      = SP_BD[word.speechPane]   ?? "rgba(99,102,241,.38)";
  const spText    = SP_TEXT[word.speechPane] ?? "#818cf8";

  return (
    <article
      className="rounded-2xl overflow-hidden flex flex-col transition-transform duration-200 hover:-translate-y-0.5 hover:shadow-lg"
      style={{
        background:     "var(--surface-card)",
        backdropFilter: "blur(20px) saturate(180%)",
        WebkitBackdropFilter: "blur(20px) saturate(180%)",
        border:         "1px solid var(--border)",
        boxShadow:      "0 2px 16px rgba(0,0,0,.10)",
      }}
    >
      {/* ── Header ──────────────────────────────────────────────── */}
      <div className="flex items-start justify-between gap-2 px-4 pt-3 pb-2">
        <h2 className="text-xl font-black leading-snug flex-1"
          dir="rtl"
          style={{ fontFamily: "'NRT',system-ui,sans-serif", color: "var(--t-text-1)" }}>
          {word.kurdish}
        </h2>

        <div className="flex items-center flex-wrap gap-1 shrink-0 pt-0.5">
          {/* Speech type */}
          {word.speechPaneKurdish && (
            <span className="text-[10px] font-bold px-2 py-0.5 rounded-full"
              style={{ background: spBg, border: `1px solid ${spBd}`, color: spText }}>
              {word.speechPaneKurdish}
            </span>
          )}
          {/* Category */}
          {word.category && (
            <span className="text-[10px] font-semibold px-2 py-0.5 rounded-full"
              style={{ background: "rgba(99,102,241,.10)", border: "1px solid rgba(99,102,241,.22)", color: "var(--accent-light)" }}>
              {catIcon(word.category)} {word.category}
            </span>
          )}
          {/* Galaxy explore */}
          <button
            onClick={() => onExplore(word.id)}
            className="text-[10px] font-semibold px-2 py-0.5 rounded-full transition-all duration-150 hover:scale-105"
            style={{
              background: "rgba(6,182,212,.14)",
              border: "1px solid rgba(6,182,212,.32)",
              color: "#22d3ee",
            }}>
            ↗ گەلاکسی
          </button>
        </div>
      </div>

      {/* ── Meanings by dialect ──────────────────────────────────── */}
      {dialects.length > 0 && (
        <div className="px-4 pb-2 space-y-1.5" dir="rtl">
          {dialects.slice(0, 4).map(([dialect, meanings]) => (
            <div key={dialect} className="flex items-start gap-2">
              <span className="shrink-0 text-[9px] font-bold px-1.5 py-0.5 rounded-full mt-0.5"
                style={{
                  background: "rgba(6,182,212,.10)",
                  border: "1px solid rgba(6,182,212,.20)",
                  color: "#22d3ee",
                }}>
                {dialect}
              </span>
              <p className="text-[11px] leading-relaxed" style={{ color: "var(--t-text-2)" }}>
                {meanings.join(" · ")}
              </p>
            </div>
          ))}
        </div>
      )}

      {/* Description */}
      {word.description && (
        <p className="px-4 pb-2 text-[11px] leading-relaxed line-clamp-1"
          style={{ color: "var(--t-text-4)" }}>
          {word.description}
        </p>
      )}

      {/* ── Inline mini-map ──────────────────────────────────────── */}
      {word.totalRelations > 0 && (
        <div className="mt-auto mx-4 mb-3 rounded-xl overflow-hidden"
          style={{
            background: "rgba(6,182,212,.04)",
            border: "1px solid rgba(6,182,212,.10)",
          }}>
          <div className="flex justify-center py-1">
            <MiniMap wordId={word.id} />
          </div>
          {/* 5-type legend */}
          <div className="flex flex-wrap justify-center gap-x-2 gap-y-0.5 px-3 pb-2 pt-0.5 border-t"
            style={{ borderColor: "rgba(6,182,212,.08)" }}>
            {REL5.map(r => (
              <span key={r.key} className="flex items-center gap-0.5 text-[8.5px]"
                style={{ color: r.color }}>
                <span className="w-1.5 h-1.5 rounded-full inline-block" style={{ background: r.color }} />
                {r.label}
              </span>
            ))}
          </div>
        </div>
      )}
    </article>
  );
}
