interface RelatedWordDto {
  id: number;
  targetWordId: number;
  targetKurdish?: string;
  relationType: string;
}

export interface WordDto {
  id: number;
  kurdish: string;
  meaning?: string;
  category?: string;
  description?: string;
  relatedWords: RelatedWordDto[];
}

const relationStyle: Record<string, { bg: string; text: string; label: string }> = {
  synonym: { bg: "bg-emerald-50", text: "text-emerald-700", label: "هاوماناکە" },
  antonym: { bg: "bg-red-50",     text: "text-red-600",     label: "دژەوشە"   },
  related: { bg: "bg-blue-50",    text: "text-blue-700",    label: "پەیوەندیدار" },
  example: { bg: "bg-amber-50",   text: "text-amber-700",   label: "نموونە"   },
};

export default function WordCard({ word }: { word: WordDto }) {
  const rel = (type: string) =>
    relationStyle[type] ?? { bg: "bg-gray-100", text: "text-gray-600", label: type };

  return (
    <div className="bg-white rounded-2xl border border-gray-100 shadow-sm hover:shadow-md transition-all duration-200 p-5">
      {/* Header */}
      <div className="flex items-start justify-between gap-3 mb-3">
        <h2 className="text-2xl font-bold text-gray-900 leading-tight tracking-wide" dir="rtl">
          {word.kurdish}
        </h2>
        {word.category && (
          <span className="shrink-0 mt-1 text-xs font-semibold px-3 py-1 bg-indigo-50 text-indigo-600 rounded-full border border-indigo-100">
            {word.category}
          </span>
        )}
      </div>

      {/* Meaning */}
      {word.meaning && (
        <p
          className="text-gray-700 text-base leading-relaxed border-r-4 border-indigo-300 pr-3 mb-3"
          dir="rtl"
        >
          {word.meaning}
        </p>
      )}

      {/* Description */}
      {word.description && (
        <p className="text-gray-400 text-sm mb-3">{word.description}</p>
      )}

      {/* Related words flow */}
      {word.relatedWords?.length > 0 && (
        <div className="mt-3 pt-3 border-t border-gray-50">
          <p className="text-xs text-gray-400 font-medium mb-2 uppercase tracking-wide">
            وشە پەیوەندیدارەکان
          </p>
          <div className="flex flex-wrap gap-2">
            {word.relatedWords.map((rw) => {
              const s = rel(rw.relationType);
              return (
                <div key={rw.id} className="flex items-center gap-1">
                  <span
                    className={`text-sm font-semibold px-3 py-1 rounded-full ${s.bg} ${s.text} border border-current/10`}
                    dir="rtl"
                  >
                    {rw.targetKurdish}
                  </span>
                  <span className="text-xs text-gray-300 font-light">{s.label}</span>
                </div>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
}
