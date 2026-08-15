import { NextRequest, NextResponse } from "next/server";

// As with `categories`, this endpoint was previously served by the `[id]` route
// and needs its own handler now that `speech-types/` is a real segment.
export async function GET(_request: NextRequest) {
  try {
    const res = await fetch(`${process.env.API_URL}/api/words/speech-types`, {
      cache: "no-store",
    });
    if (!res.ok)
      return NextResponse.json({ error: "Not found" }, { status: res.status });
    const data = await res.json();
    return NextResponse.json(data);
  } catch {
    return NextResponse.json({ error: "API unavailable" }, { status: 503 });
  }
}
