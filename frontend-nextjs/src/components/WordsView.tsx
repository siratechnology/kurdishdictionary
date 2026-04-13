"use client";

import { useState, useEffect, useRef, useCallback } from "react";
import WordCard, { WordDto } from "./WordCard";

interface PagedResult {
  items: WordDto[];
  totalCount: number;
  totalPages: number;
}

export default function WordsView() {
  const [words, setWords] = useState<WordDto[]>([]);
  const [search, setSearch] = useState("");
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const debounceRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  const fetchWords = useCallback(async (q: string, p: number) => {
    setLoading(true);
    const params = new URLSearchParams({ page: String(p), pageSize: "16" });
    if (q.trim()) params.set("search", q.trim());
    const res = await fetch(`/api/words?${params}`);
    const data: PagedResult = await res.json();
    setWords(data.items ?? []);
    setTotal(data.totalCount ?? 0);
    setTotalPages(data.totalPages ?? 1);
    setLoading(false);
  }, []);

  useEffect(() => {
    fetchWords("", 1);
  }, [fetchWords]);

  const handleSearch = (value: string) => {
    setSearch(value);
    setPage(1);
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => fetchWords(value, 1), 300);
  };

  const goPage = (p: number) => {
    setPage(p);
    fetchWords(search, p);
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  return (
    <>
      {/* Sticky search bar */}
      <div className="sticky top-0 z-10 bg-white/90 backdrop-blur border-b border-gray-100 py-3 mb-6">
        <div className="max-w-3xl mx-auto px-4">
          <div className="relative">
            <span className="absolute right-4 top-1/2 -translate-y-1/2 text-gray-300 text-lg">🔍</span>
            <input
              type="text"
              value={search}
              onChange={(e) => handleSearch(e.target.value)}
              placeholder="گەڕان بە وشە..."
              dir="rtl"
              className="w-full border border-gray-200 rounded-2xl px-5 py-3 pr-11 text-base text-gray-800 placeholder-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:border-transparent shadow-sm transition"
            />
            {search && (
              <button
                onClick={() => handleSearch("")}
                className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-300 hover:text-gray-500 text-lg"
              >
                ✕
              </button>
            )}
          </div>
          <p className="text-xs text-gray-400 mt-2 text-center">
            {loading ? "گەڕان..." : `${total} وشە دۆزرایەوە`}
          </p>
        </div>
      </div>

      {/* Word list */}
      <div className="max-w-3xl mx-auto px-4 pb-12">
        {loading ? (
          <div className="flex justify-center py-20">
            <div className="w-8 h-8 border-4 border-indigo-400 border-t-transparent rounded-full animate-spin" />
          </div>
        ) : words.length === 0 ? (
          <div className="text-center py-20 text-gray-300">
            <p className="text-5xl mb-3">📖</p>
            <p className="text-lg font-medium text-gray-400">هیچ وشەیەک نەدۆزرایەوە</p>
          </div>
        ) : (
          <div className="flex flex-col gap-4">
            {words.map((word) => (
              <WordCard key={word.id} word={word} />
            ))}
          </div>
        )}

        {/* Pagination */}
        {totalPages > 1 && !loading && (
          <div className="flex justify-center items-center gap-2 mt-8">
            <button
              onClick={() => goPage(page - 1)}
              disabled={page === 1}
              className="px-4 py-2 text-sm rounded-xl border border-gray-200 disabled:opacity-30 hover:bg-gray-50 transition"
            >
              ← پێشتر
            </button>
            <span className="text-sm text-gray-400">{page} / {totalPages}</span>
            <button
              onClick={() => goPage(page + 1)}
              disabled={page === totalPages}
              className="px-4 py-2 text-sm rounded-xl border border-gray-200 disabled:opacity-30 hover:bg-gray-50 transition"
            >
              دواتر →
            </button>
          </div>
        )}
      </div>
    </>
  );
}
