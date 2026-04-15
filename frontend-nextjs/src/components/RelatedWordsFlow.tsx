"use client";

import { useState, useCallback } from "react";
import { WordDto, RelatedWordDto } from "./WordCard";

/* ── Relation type colours ───────────────────────────────────────── */
const TYPE: Record<string, { fill: string; stroke: string; text: string; label: string }> = {
  synonym: { fill: "#d1fae5", stroke: "#10b981", text: "#065f46", label: "هاوماناکە" },
  antonym: { fill: "#fee2e2", stroke: "#ef4444", text: "#991b1b", label: "دژەوشە"   },
  related: { fill: "#dbeafe", stroke: "#3b82f6", text: "#1e3a8a", label: "پەیوەندیدار" },
  example: { fill: "#fef3c7", stroke: "#f59e0b", text: "#78350f", label: "نموونە"   },
};
const FALLBACK = { fill: "#f3f4f6", stroke: "#9ca3af", text: "#374151", label: "other" };

/* ── Layout constants ────────────────────────────────────────────── */
const CX    = 290;   // svg centre x
const CY    = 290;   // svg centre y
const D     = 58;    // diamond half-diagonal
const MIN_R = 105;   // weight 1  → 105 px from centre
const MAX_R = 225;   // weight 10 → 225 px from centre
const NR    = 44;    // related-word circle radius
const VB    = 580;   // viewBox size

function radius(weight: number) {
  return MIN_R + ((Math.min(10, Math.max(1, weight)) - 1) / 9) * (MAX_R - MIN_R);
}
function angle(i: number, n: number) {
  return (2 * Math.PI * i) / n - Math.PI / 2;
}
function cut(s: string | undefined, max = 9) {
  if (!s) return "?";
  return s.length > max ? s.slice(0, max - 1) + "…" : s;
}

/* ── Component ───────────────────────────────────────────────────── */
interface Props {
  word: WordDto;
  onBack?: () => void;
}

export default function RelatedWordsFlow({ word: initial, onBack }: Props) {
  const [current,  setCurrent]  = useState<WordDto>(initial);
  const [history,  setHistory]  = useState<WordDto[]>([]);
  const [loading,  setLoading]  = useState(false);

  const relations: RelatedWordDto[] = [
    ...(current.outgoingRelations ?? []),
    ...(current.incomingRelations ?? []),
  ];
  const n = relations.length;

  /* navigate to a related word */
  const navigate = useCallback(async (rel: RelatedWordDto) => {
    if (loading) return;
    setLoading(true);
    try {
      const res  = await fetch(`/api/words/${rel.relatedWordId}`);
      if (!res.ok) return;
      const next: WordDto = await res.json();
      setHistory((h) => [...h, current]);
      setCurrent(next);
    } finally {
      setLoading(false);
    }
  }, [current, loading]);

  const goBack = useCallback(() => {
    setHistory((h) => {
      const copy = [...h];
      const prev = copy.pop()!;
      setCurrent(prev);
      return copy;
    });
  }, []);

  return (
    <div className="flex flex-col items-center gap-3 w-full select-none">

      {/* ── breadcrumb / nav bar ────────────────────────────────── */}
      <div className="flex items-center w-full max-w-[580px] gap-2 px-1">
        {(history.length > 0 || onBack) && (
          <button
            onClick={history.length > 0 ? goBack : onBack}
            className="flex items-center gap-1 text-sm text-indigo-500 hover:text-indigo-700 font-medium shrink-0 transition"
          >
            ← {history.length > 0 ? cut(history[history.length - 1].kurdish, 14) : "پاشگەزبوونەوە"}
          </button>
        )}

        <div className="flex-1 text-center">
          <span className="text-base font-extrabold text-gray-800" dir="rtl">
            {current.kurdish}
          </span>
          {current.meaning && (
            <span className="ml-2 text-xs text-gray-400" dir="rtl">
              — {current.meaning}
            </span>
          )}
        </div>

        {/* history breadcrumb dots */}
        {history.length > 0 && (
          <div className="flex gap-1 shrink-0">
            {history.map((_, i) => (
              <span key={i} className="w-1.5 h-1.5 rounded-full bg-indigo-300" />
            ))}
          </div>
        )}
      </div>

      {/* ── SVG radial chart ────────────────────────────────────── */}
      <div className="relative w-full" style={{ maxWidth: VB }}>
        <svg
          viewBox={`0 0 ${VB} ${VB}`}
          width="100%"
          aria-label="word flow chart"
        >
          {/* ── Lines from centre to each node ── */}
          {relations.map((rel, i) => {
            const a  = angle(i, n);
            const r  = radius(rel.weight);
            const nx = CX + r * Math.cos(a);
            const ny = CY + r * Math.sin(a);
            const c  = TYPE[rel.relationType] ?? FALLBACK;
            /* thicker line = lower weight (closer) */
            const lw = Math.max(1.5, 5.5 - (rel.weight - 1) * 0.45);
            return (
              <line
                key={`ln-${rel.id}`}
                x1={CX} y1={CY} x2={nx} y2={ny}
                stroke={c.stroke} strokeWidth={lw}
                strokeLinecap="round" opacity={0.45}
              />
            );
          })}

          {/* ── Centre diamond ── */}
          <polygon
            points={`${CX},${CY - D} ${CX + D},${CY} ${CX},${CY + D} ${CX - D},${CY}`}
            fill={loading ? "#a5b4fc" : "#6366f1"}
            stroke="#4f46e5" strokeWidth="2"
            className="transition-colors duration-300"
          />
          <text
            x={CX} y={CY}
            textAnchor="middle" dominantBaseline="middle"
            fill="white" fontSize="15" fontWeight="800"
          >
            {cut(current.kurdish, 11)}
          </text>

          {/* ── Related word nodes ── */}
          {relations.map((rel, i) => {
            const a  = angle(i, n);
            const r  = radius(rel.weight);
            const nx = CX + r * Math.cos(a);
            const ny = CY + r * Math.sin(a);
            const c  = TYPE[rel.relationType] ?? FALLBACK;

            /* dash ring shows weight as partial arc */
            const ringR   = NR - 7;
            const ringCirc = 2 * Math.PI * ringR;
            const dash     = (rel.weight / 10) * ringCirc;

            return (
              <g
                key={`nd-${rel.id}`}
                onClick={() => navigate(rel)}
                className="cursor-pointer"
                role="button"
                tabIndex={0}
                aria-label={rel.relatedKurdish}
                onKeyDown={(e) => e.key === "Enter" && navigate(rel)}
              >
                {/* hover ring */}
                <circle cx={nx} cy={ny} r={NR + 5} fill="transparent"
                  className="group-hover:fill-indigo-50 transition" />

                {/* main circle */}
                <circle
                  cx={nx} cy={ny} r={NR}
                  fill={c.fill} stroke={c.stroke} strokeWidth="2.5"
                />

                {/* weight arc (dash) inside circle */}
                <circle
                  cx={nx} cy={ny} r={ringR}
                  fill="none" stroke={c.stroke} strokeWidth="2" opacity="0.35"
                  strokeDasharray={`${dash} ${ringCirc}`}
                  transform={`rotate(-90 ${nx} ${ny})`}
                />

                {/* word label */}
                <text
                  x={nx} y={ny - 5}
                  textAnchor="middle" dominantBaseline="middle"
                  fill={c.text} fontSize="11.5" fontWeight="700"
                  className="pointer-events-none"
                >
                  {cut(rel.relatedKurdish)}
                </text>

                {/* weight number */}
                <text
                  x={nx} y={ny + 13}
                  textAnchor="middle" dominantBaseline="middle"
                  fill={c.text} fontSize="9.5" opacity={0.65}
                  className="pointer-events-none"
                >
                  ⚖ {rel.weight}
                </text>
              </g>
            );
          })}

          {/* empty state */}
          {n === 0 && !loading && (
            <text
              x={CX} y={CY + D + 28}
              textAnchor="middle" fill="#9ca3af" fontSize="13"
            >
              هیچ وشەی پەیوەندیدارێک نییە
            </text>
          )}
        </svg>

        {/* loading overlay */}
        {loading && (
          <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
            <div className="w-10 h-10 border-4 border-indigo-500 border-t-transparent rounded-full animate-spin" />
          </div>
        )}
      </div>

      {/* ── Legend ── */}
      <div className="flex flex-wrap justify-center gap-4 pb-2">
        {Object.entries(TYPE).map(([type, c]) => (
          <span key={type} className="flex items-center gap-1.5 text-xs text-gray-500">
            <span className="w-3 h-3 rounded-full border-2"
              style={{ background: c.fill, borderColor: c.stroke }} />
            {c.label}
          </span>
        ))}
        <span className="text-xs text-gray-400">· خط کەڵک = نزیکایەتی · ⚖ = weight</span>
      </div>
    </div>
  );
}
