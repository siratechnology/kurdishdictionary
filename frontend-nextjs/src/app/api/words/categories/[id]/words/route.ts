import { NextRequest, NextResponse } from "next/server";

// Paged words within a category — backs the category feed in the mobile app.
// Read-only: only GET is proxied, so the write endpoints on the same controller
// stay off the public internet.
export async function GET(
  request: NextRequest,
  context: { params: Promise<{ id: string }> }
) {
  const { id } = await context.params;
  const params = request.nextUrl.searchParams.toString();
  const url = `${process.env.API_URL}/api/words/categories/${id}/words${
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
