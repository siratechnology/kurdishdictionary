"use client";

import { useState, useCallback, useRef, useTransition } from "react";
import WordCard, { WordDto } from "./WordCard";
import RelatedWordsFlow from "./RelatedWordsFlow";

interface PagedResult {
  items: WordDto[];
  totalCount: number;
  totalPages: number;
  page: number;
}

const PAGE_SIZE = 16;

/* ── Skeleton card shown while loading ─────────────────────────── */
function SkeletonCard() {
  return (
    <div className="bg-white rounded-2xl border border-gray-100 shadow-sm p-5 animate-pulse">
      <div className="flex justify-between gap-3 mb-3">
        <div className="h-7 w-32 bg-gray-200 rounded-lg" />
        <div className="h-5 w-16 bg-gray-100 rounded-full" />
      </div>
      <div className="h-4 w-full bg-gray-100 rounded mb-2" />
      <div className="h-4 w-3/4 bg-gray-100 rounded" />
    </div>
  );
}

/* ── Main component ─────────────────────────────────────────────── */
interface Props {
  initialData: PagedResult;
}

export default function WordsView({ initialData }: Props) {
  const [data,      setData     ] = useState<PagedResult>(initialData);
  const [search,    setSearch   ] = useState("");
  const [page,      setPage     ] = useState(1);
  const [flowWord,  setFlowWord ] = useState<WordDto | null>(null);
  const [isPending, startTransition] = useTransition();
  const debounceRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  const load = useCallback(async (q: string, p: number) => {
    const params = new URLSearchParams({ page: String(p), pageSize: String(PAGE_SIZE) });
    if (q.trim()) params.set("search", q.trim());
    const res  = await fetch(`/api/words?${params}`);
    const json: PagedResult = await res.json();
    setData(json);
    setPage(p);
  }, []);

  const handleSearch = (value: string) => {
    setSearch(value);
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      startTransition(() => { load(value, 1); });
    }, 280);
  };

  const goPage = (p: number) => {
    startTransition(() => { load(search, p); });
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  /* ── Flow chart mode ─────────────────────────────────────────── */
  if (flowWord) {
    return (
      <div className="max-w-[620px] mx-auto px-4 py-6">
        <RelatedWordsFlow word={flowWord} onBack={() => setFlowWord(null)} />
      </div>
    );
  }

  /* ── List mode ───────────────────────────────────────────────── */
  return (
    <>
      {/* sticky search */}
      <div className="sticky top-0 z-20 bg-white/90 backdrop-blur-md border-b border-gray-100 shadow-sm py-3 mb-6">
        <div className="max-w-3xl mx-auto px-4">
          <div className="relative">
            <span className="absolute right-4 top-1/2 -translate-y-1/2 text-gray-300 text-lg">🔍</span>
            <input
              type="text"
              value={search}
              onChange={(e) => handleSearch(e.target.value)}
              placeholder="گەڕان بە وشە..."
              dir="rtl"
              className="w-full bg-gray-50 border border-gray-200 rounded-2xl px-5 py-3 pr-11 text-base text-gray-800 placeholder-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:bg-white transition"
            />
            {search && (
              <button
                onClick={() => handleSearch("")}
                className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-300 hover:text-gray-500 text-lg"
              >✕</button>
            )}
          </div>
          <div className="flex items-center justify-between mt-2 px-1">
            <p className="text-xs text-gray-400">
              {isPending
                ? <span className="inline-flex items-center gap-1.5">
                    <span className="w-3 h-3 border-2 border-indigo-400 border-t-transparent rounded-full animate-spin" />
                    چاوەڕوانبە...
                  </span>
                : <>{data.totalCount} وشە دۆزرایەوە</>
              }
            </p>
            {data.totalPages > 1 && (
              <p className="text-xs text-gray-400">{page} / {data.totalPages}</p>
            )}
          </div>
        </div>
      </div>

      {/* word grid */}
      <div className="max-w-3xl mx-auto px-4 pb-12">
        {isPending ? (
          /* skeleton loader */
          <div className="flex flex-col gap-4">
            {Array.from({ length: 6 }).map((_, i) => <SkeletonCard key={i} />)}
          </div>
        ) : data.items.length === 0 ? (
          <div className="text-center py-24 text-gray-300">
            <p className="text-5xl mb-3">📖</p>
            <p className="text-lg font-medium text-gray-400">هیچ وشەیەک نەدۆزرایەوە</p>
          </div>
        ) : (
          <div className="flex flex-col gap-4">
            {data.items.map((word) => (
              <WordCard
                key={word.id}
                word={word}
                onFlowClick={() => setFlowWord(word)}
              />
            ))}
          </div>
        )}

        {/* pagination */}
        {!isPending && data.totalPages > 1 && (
          <div className="flex justify-center items-center gap-2 mt-10">
            <button
              onClick={() => goPage(page - 1)}
              disabled={page === 1}
              className="px-4 py-2 text-sm font-medium rounded-xl border border-gray-200 disabled:opacity-30 hover:bg-gray-50 transition"
            >← پێشتر</button>

            <div className="flex gap-1">
              {Array.from({ length: data.totalPages }, (_, i) => i + 1)
                .filter((p) => p === 1 || p === data.totalPages || Math.abs(p - page) <= 2)
                .reduce<(number | "…")[]>((acc, p, i, arr) => {
                  if (i > 0 && (p as number) - (arr[i - 1] as number) > 1) acc.push("…");
                  acc.push(p);
                  return acc;
                }, [])
                .map((p, i) =>
                  p === "…"
                    ? <span key={`el-${i}`} className="px-2 py-2 text-sm text-gray-300">…</span>
                    : <button key={p}
                        onClick={() => goPage(p as number)}
                        className={`w-9 h-9 text-sm rounded-lg font-medium transition
                          ${p === page ? "bg-indigo-600 text-white shadow-sm" : "text-gray-500 hover:bg-gray-100"}`}
                      >{p}</button>
                )}
            </div>

            <button
              onClick={() => goPage(page + 1)}
              disabled={page === data.totalPages}
              className="px-4 py-2 text-sm font-medium rounded-xl border border-gray-200 disabled:opacity-30 hover:bg-gray-50 transition"
            >دواتر →</button>
          </div>
        )}
      </div>
    </>
  );
}
