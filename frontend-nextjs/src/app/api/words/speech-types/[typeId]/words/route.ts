import { NextRequest, NextResponse } from "next/server";

// Paged words for one part of speech — backs the speech-type feed in the
// mobile app. Read-only, matching the category route alongside it.
export async function GET(
  request: NextRequest,
  context: { params: Promise<{ typeId: string }> }
) {
  const { typeId } = await context.params;
  const params = request.nextUrl.searchParams.toString();
  const url = `${process.env.API_URL}/api/words/speech-types/${typeId}/words${
    params ? `?${params}` : ""
  }`;

  try {
    const res = await fetch(url, { cache: "no-store" });
    if (!res.ok)
      return NextResponse.json({ error: "Not found" }, { status: res.status });
    const data = await res.json();
    return NextResponse.json(data);
  } catch {
    return NextResponse.json({ error: "API unavailable" }, { status: 503 });
  }
}
