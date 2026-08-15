import { NextRequest, NextResponse } from "next/server";

// `/api/words/categories` used to be served incidentally by the `[id]` route
// (with id="categories"). Now that `categories/` exists as a real segment for
// the nested route below it, that fallback no longer applies — this handler
// keeps the endpoint working.
export async function GET(_request: NextRequest) {
  try {
    const res = await fetch(`${process.env.API_URL}/api/words/categories`, {
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
