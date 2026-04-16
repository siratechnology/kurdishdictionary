import { NextRequest, NextResponse } from "next/server";

export async function GET(
  _request: NextRequest,
  context: { params: Promise<{ id: string }> }
) {
  const { id } = await context.params;
  try {
    const res = await fetch(`${process.env.API_URL}/api/words/${id}/graph`, {
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
