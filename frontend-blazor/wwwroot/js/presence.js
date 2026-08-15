// Client-side activity signal for پڕۆمپت ٩.
//
// The circuit knows when the connection opens and closes; it cannot know whether the person in
// front of it is doing anything. This watches for real input and reports it — THROTTLED HARD.
//
// Throttling is the whole design. Without it, pointermove alone fires hundreds of times a minute
// and every one of those would cross the circuit as a server call, which on Blazor Server means a
// round trip per mouse twitch. One call per 30 seconds is enough to distinguish "typing" from
// "walked away", which is the only question being asked.

window.kurdishPresence = (() => {
    const THROTTLE_MS = 30_000;

    // A tick while the tab is VISIBLE, independent of input.
    //
    // Input alone was too strict a test. Reading a definition, comparing two entries, thinking —
    // none of it moves a pointer, so after two minutes the server stopped hearing anything and
    // dropped the person to بێ‌چالاکی while they were sitting there looking at the screen. The
    // effect was one-way and looked broken: they still saw everyone who arrived after them,
    // because an arrival broadcasts, but nobody arriving later could see THEM.
    //
    // Visibility is what keeps this honest. A hidden tab does not tick, so the laptop closed on a
    // desk and the phone in a pocket still fall out of «چالاک» on their own — which is the case
    // the original input-only rule existed to catch.
    const TICK_MS = 60_000;

    let dotNetRef = null;
    let activeToken = null;
    let lastSent = 0;
    let listening = false;
    let timer = null;

    function report(reason) {
        if (!dotNetRef) return;

        const now = Date.now();
        if (now - lastSent < THROTTLE_MS) return;

        lastSent = now;
        dotNetRef.invokeMethodAsync('Heartbeat', location.pathname, reason)
            .catch(() => {
                // The circuit went away mid-flight. The server already knows via OnCircuitClosed;
                // there is nothing useful to do here and an unhandled rejection helps nobody.
            });
    }

    function onVisibility() {
        // Coming back to the tab is a real signal and should not wait out the throttle: someone
        // returning after twenty minutes is active NOW.
        if (document.visibilityState === 'visible') {
            lastSent = 0;
            report('visible');
        }
    }

    return {
        // token identifies WHICH tracker is calling. Navigation creates the new page's tracker
        // before disposing the old one, so without it the outgoing stop() nulls the reference the
        // incoming start() has just set — and the heartbeat is dead for the rest of the session.
        // That is the bug behind "time only counts while I stay on one page": the very first page
        // reported, and nothing after it ever did.
        start(ref, token) {
            dotNetRef = ref;
            activeToken = token;

            if (!listening) {
                listening = true;

                // passive: these never call preventDefault, and saying so keeps scrolling smooth.
                // Attached ONCE for the lifetime of the document — the listeners outlive any one
                // page and simply report through whichever reference is current.
                document.addEventListener('pointermove', () => report('pointer'), { passive: true });
                document.addEventListener('keydown', () => report('key'), { passive: true });
                document.addEventListener('click', () => report('click'), { passive: true });
                document.addEventListener('visibilitychange', onVisibility);
            }

            // One interval for the document, not one per tracker — navigation replaces the
            // tracker several times a session and each would otherwise leave a timer behind.
            if (!timer) {
                timer = setInterval(() => {
                    if (document.visibilityState === 'visible') report('tick');
                }, TICK_MS);
            }

            // Arriving on a page IS activity, and it must not wait out the throttle left over
            // from the page before it.
            lastSent = 0;
            report('start');
        },

        stop(token) {
            // Only the tracker that currently owns the reference may clear it.
            if (token && token !== activeToken) return;

            dotNetRef = null;
            activeToken = null;
        },
    };
})();
