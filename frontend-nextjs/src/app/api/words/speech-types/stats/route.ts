import { NextRequest, NextResponse } from "next/server";

// Every part of speech with its word count — backs the browse grid.
export async function GET(_request: NextRequest) {
  try {
    const res = await fetch(
      `${process.env.API_URL}/api/words/speech-types/stats`,
      { cache: "no-store" }
    );
    if (!res.ok)
      return NextResponse.json({ error: "Not found" }, { status: res.status });
    const data = await res.json();
    return NextResponse.json(data);
  } catch {
    return NextResponse.json({ error: "API unavailable" }, { status: 503 });
  }
}
