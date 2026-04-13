interface WordDto {
  id: number;
  kurdish: string;
  meaning?: string;
  category?: string;
  description?: string;
}

interface PagedResult {
  items: WordDto[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

async function getWords(): Promise<PagedResult> {
  const res = await fetch(`${process.env.API_URL}/api/words?page=1&pageSize=20`, {
    cache: "no-store",
  });

  if (!res.ok) throw new Error("Failed to fetch words");
  return res.json();
}

export default async function HomePage() {
  let data: PagedResult | null = null;
  let error: string | null = null;

  try {
    data = await getWords();
  } catch {
    error = "Could not connect to the API.";
  }

  return (
    <main className="max-w-3xl mx-auto px-4 py-10">
      <h1 className="text-3xl font-bold mb-2">Kurdish Dictionary</h1>
      <p className="text-sm text-gray-500 mb-8">فەرھەنگی کوردی بۆ کوردی</p>

      {error && (
        <p className="text-red-500 text-sm">{error}</p>
      )}

      {data && (
        <>
          <p className="text-sm text-gray-400 mb-4">{data.totalCount} word(s) found</p>
          <ul className="divide-y divide-gray-200 dark:divide-gray-700">
            {data.items.map((word) => (
              <li key={word.id} className="py-4">
                <div className="flex items-center justify-between gap-4">
                  <span className="text-xl font-semibold">{word.kurdish}</span>
                  {word.category && (
                    <span className="text-xs px-2 py-0.5 rounded-full bg-gray-100 dark:bg-gray-800 text-gray-500">
                      {word.category}
                    </span>
                  )}
                </div>
                {word.meaning && (
                  <p className="mt-1 text-gray-600 dark:text-gray-300">{word.meaning}</p>
                )}
                {word.description && (
                  <p className="mt-1 text-sm text-gray-400">{word.description}</p>
                )}
              </li>
            ))}
          </ul>

          {data.items.length === 0 && (
            <p className="text-gray-400 text-sm">No words added yet.</p>
          )}
        </>
      )}
    </main>
  );
}
