import { NextRequest, NextResponse } from "next/server";

/**
 * Forwards a visitor event to the backend.
 *
 * The browser cannot reach the backend directly (it lives on the internal Docker network), and we
 * would not want it to: the client must not be the one asserting its own IP. This route runs
 * server-side, so it can read the real client address and pass it on for geo lookup.
 */
export async function POST(request: NextRequest) {
  try {
    const body = await request.json();

    // Cloudflare's header is the most trustworthy; x-forwarded-for's left-most entry is the
    // original client when we sit behind a proxy chain.
    const forwardedFor = request.headers.get("x-forwarded-for");
    const clientIp =
      request.headers.get("cf-connecting-ip") ??
      forwardedFor?.split(",")[0]?.trim() ??
      "";

    const headers: Record<string, string> = {
      "Content-Type": "application/json",
      "User-Agent": request.headers.get("user-agent") ?? "",
    };

    // Hand the backend the client's IP, not this server's.
    if (clientIp) {
      headers["X-Forwarded-For"] = clientIp;
      headers["CF-Connecting-IP"] = clientIp;
    }

    const country = request.headers.get("cf-ipcountry");
    if (country) headers["CF-IPCountry"] = country;

    await fetch(`${process.env.API_URL}/api/analytics/event`, {
      method: "POST",
      headers,
      body: JSON.stringify(body),
      cache: "no-store",
    });

    return new NextResponse(null, { status: 204 });
  } catch {
    // A failed analytics write is not the visitor's problem — swallow it and return success so
    // the client never retries or surfaces an error.
    return new NextResponse(null, { status: 204 });
  }
}
