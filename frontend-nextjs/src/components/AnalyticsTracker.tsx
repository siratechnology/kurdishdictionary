"use client";

import { useEffect } from "react";
import { usePathname } from "next/navigation";
import { track } from "@/lib/analytics";

/**
 * Records a page_view on first load and on every client-side route change.
 *
 * Renders nothing — it exists only for the effect. Mounted once in the root layout so it survives
 * navigation instead of remounting per page.
 */
export default function AnalyticsTracker() {
  const pathname = usePathname();

  useEffect(() => {
    if (!pathname) return;

    // A word page is a page view *and* a word view: the dashboard wants "most opened words"
    // separately from raw traffic.
    const wordMatch = pathname.match(/^\/word\/(\d+)$/);

    track({
      eventType: "page_view",
      path: pathname,
      wordId: wordMatch ? Number(wordMatch[1]) : undefined,
    });

    if (wordMatch) {
      track({
        eventType: "word_view",
        path: pathname,
        wordId: Number(wordMatch[1]),
      });
    }
  }, [pathname]);

  return null;
}
