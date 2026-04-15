export interface RelatedWordDto {
  id: number;
  relatedWordId: number;
  relatedKurdish?: string;
  relationType: string;
  isIncoming: boolean;
  weight: number;
}

export interface WordDto {
  id: number;
  kurdish: string;
  meaning?: string;
  category?: string;
  description?: string;
  createdAt: string;
  outgoingRelations: RelatedWordDto[];
  incomingRelations: RelatedWordDto[];
}

const CAT_COLOR: Record<string, string> = {
  verb:      "bg-violet-50 text-violet-700 border-violet-200",
  noun:      "bg-sky-50    text-sky-700    border-sky-200",
  adjective: "bg-amber-50  text-amber-700  border-amber-200",
  adverb:    "bg-teal-50   text-teal-700   border-teal-200",
  phrase:    "bg-rose-50   text-rose-700   border-rose-200",
};

interface Props {
  word: WordDto;
  onFlowClick: () => void;
}

export default function WordCard({ word, onFlowClick }: Props) {
  const catCls =
    CAT_COLOR[word.category?.toLowerCase() ?? ""] ??
    "bg-indigo-50 text-indigo-700 border-indigo-200";

  const relCount =
    (word.outgoingRelations?.length ?? 0) +
    (word.incomingRelations?.length ?? 0);

  return (
    <article className="bg-white rounded-2xl border border-gray-100 shadow-sm hover:shadow-md transition-shadow duration-200 overflow-hidden">

      {/* header */}
      <div className="flex items-start justify-between gap-3 px-5 pt-4 pb-2">
        <h2 className="text-2xl font-extrabold text-gray-900 tracking-wide leading-tight" dir="rtl">
          {word.kurdish}
        </h2>
        {word.category && (
          <span className={`shrink-0 mt-1 text-xs font-semibold px-3 py-1 rounded-full border ${catCls}`}>
            {word.category}
          </span>
        )}
      </div>

      {word.meaning && (
        <p className="mx-5 text-gray-700 text-base leading-relaxed border-r-4 border-indigo-200 pr-3 mb-2" dir="rtl">
          {word.meaning}
        </p>
      )}

      {word.description && (
        <p className="mx-5 text-gray-400 text-sm mb-2">{word.description}</p>
      )}

      {/* footer */}
      <div className="flex items-center justify-between px-5 py-3 border-t border-gray-50">
        <span className="text-xs text-gray-400">
          {relCount > 0 ? `${relCount} وشەی پەیوەندیدار` : "وشەی پەیوەندیدار نییە"}
        </span>
        {relCount > 0 && (
          <button
            onClick={onFlowClick}
            className="flex items-center gap-1.5 text-xs font-semibold text-indigo-600 hover:text-indigo-800 hover:bg-indigo-50 px-3 py-1.5 rounded-lg transition"
          >
            <span>🗺</span> نیشاندانی فلۆ
          </button>
        )}
      </div>
    </article>
  );
}
