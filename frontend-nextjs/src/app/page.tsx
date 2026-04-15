import FlowPage from "@/components/FlowPage";

export interface WordBasic {
  id: number;
  kurdish: string;
  meaning?: string;
  category?: string;
}

// Only fetch the lightweight list — details load progressively on the client
async function getWordList(): Promise<WordBasic[]> {
  try {
    const res = await fetch(
      `${process.env.API_URL}/api/words?page=1&pageSize=9`,
      { cache: "no-store" }
    );
    if (!res.ok) return [];
    const data = await res.json();
    const items: WordBasic[] = data.items ?? [];
    // Deduplicate by id in case API returns repeats
    const seen = new Set<number>();
    return items.filter((w) => {
      if (seen.has(w.id)) return false;
      seen.add(w.id);
      return true;
    });
  } catch {
    return [];
  }
}

export default async function HomePage() {
  const initialList = await getWordList();
  return <FlowPage initialList={initialList} />;
}
