import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
  const params = request.nextUrl.searchParams.toString();
  const url = `${process.env.API_URL}/api/words${params ? `?${params}` : ""}`;

  try {
    const res = await fetch(url, { cache: "no-store" });
    const data = await res.json();
    return NextResponse.json(data);
  } catch {
    return NextResponse.json({ error: "API unavailable" }, { status: 503 });
  }
}
