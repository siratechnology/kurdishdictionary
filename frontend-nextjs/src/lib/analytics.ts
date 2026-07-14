/**
 * Client-side visitor tracking for the public dictionary.
 *
 * Events go to our own /api/track route rather than straight to the backend, so the browser never
 * needs to know the backend's address and the server can attach the real client IP.
 *
 * Fire-and-forget by design: analytics must never slow down, block, or break the page it measures.
 * Every failure path here is a silent no-op.
 */

export type EventType = "page_view" | "search" | "word_view" | "share";

export interface TrackPayload {
  eventType: EventType;
  path?: string;
  searchTerm?: string;
  resultCount?: number;
  wordId?: number;
  sessionId?: string;
  referrer?: string;
  screenWidth?: number;
  screenHeight?: number;
  viewportWidth?: number;
  viewportHeight?: number;
  language?: string;
}

const SESSION_KEY = "kd-session-id";

/**
 * An anonymous per-tab id that groups one visit's events together. sessionStorage, not
 * localStorage: it dies with the tab, so it identifies a visit rather than following a person
 * around forever.
 */
function sessionId(): string {
  try {
    let id = sessionStorage.getItem(SESSION_KEY);
    if (!id) {
      id =
        typeof crypto !== "undefined" && crypto.randomUUID
          ? crypto.randomUUID()
          : `${Date.now()}-${Math.random().toString(36).slice(2)}`;
      sessionStorage.setItem(SESSION_KEY, id);
    }
    return id;
  } catch {
    // Private mode / storage disabled — the event is still worth sending, just unattributed.
    return "";
  }
}

export function track(event: TrackPayload): void {
  if (typeof window === "undefined") return;

  try {
    const body: TrackPayload = {
      ...event,
      path: event.path ?? window.location.pathname,
      sessionId: sessionId(),
      referrer: document.referrer || undefined,
      screenWidth: window.screen?.width,
      screenHeight: window.screen?.height,
      viewportWidth: window.innerWidth,
      viewportHeight: window.innerHeight,
      language: navigator.language,
    };

    const json = JSON.stringify(body);

    // sendBeacon survives the page being closed mid-request, which is exactly when the last and
    // most interesting event of a visit fires.
    if (navigator.sendBeacon) {
      const blob = new Blob([json], { type: "application/json" });
      if (navigator.sendBeacon("/api/track", blob)) return;
    }

    // Fallback for browsers without sendBeacon (and if the beacon queue is full).
    void fetch("/api/track", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: json,
      keepalive: true,
    }).catch(() => {});
  } catch {
    // Never let a tracking bug take the page down with it.
  }
}
