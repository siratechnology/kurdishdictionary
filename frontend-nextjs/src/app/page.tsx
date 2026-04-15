import FlowPage from "@/components/FlowPage";

async function getFirstWord() {
  try {
    const listRes = await fetch(
      `${process.env.API_URL}/api/words?page=1&pageSize=1`,
      { cache: "no-store" }
    );
    if (!listRes.ok) return null;
    const list = await listRes.json();
    const first = list.items?.[0];
    if (!first) return null;

    const wordRes = await fetch(
      `${process.env.API_URL}/api/words/${first.id}`,
      { cache: "no-store" }
    );
    if (!wordRes.ok) return null;
    return wordRes.json();
  } catch {
    return null;
  }
}

export default async function HomePage() {
  const initialWord = await getFirstWord();
  return <FlowPage initialWord={initialWord} />;
}
