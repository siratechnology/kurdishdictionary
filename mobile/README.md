# فەرهەنگی کوردی — Mobile

Flutter client for the Kurdish Dictionary. Same data, same design language and
the same NRT typeface as the Next.js site and the Blazor admin.

## Where it gets its data

The app talks to **`https://jinzar.krd`** out of the box —
see `AppConfig.productionApi` in `lib/core/config.dart`. There is nothing to
configure and no server settings in the normal UI: a dictionary user should
never see a URL field.

The server section is still reachable for debugging — open ڕێکخستن and tap the
version line at the bottom **7 times**. That reveals the API address field,
one-tap presets (production / USB / emulator / LAN) and a "دۆزینەوەی خۆکار"
button that probes every candidate in parallel and keeps whichever answers.

### Endpoints that need deploying

The backend is not internet-facing — it sits behind Cloudflare with only the
Next.js site exposed, so the app reaches the API through
`jinzar.krd/api/*`, which is proxied by route handlers in
`frontend-nextjs/src/app/api/`.

Three of those handlers are **new and not yet deployed**. Until the Next.js site
ships again, Browse's part-of-speech grid and the category / part-of-speech
feeds will come back empty on production:

| Route file (new) | Endpoint |
| --- | --- |
| `api/words/speech-types/stats/route.ts` | `/api/words/speech-types/stats` |
| `api/words/speech-types/[typeId]/words/route.ts` | `/api/words/speech-types/{id}/words` |
| `api/words/categories/[id]/words/route.ts` | `/api/words/categories/{id}/words` |

`api/words/categories/route.ts` and `api/words/speech-types/route.ts` were added
alongside them. Those two endpoints previously worked only because
`/api/words/[id]` happened to match them with `id="categories"`; now that real
`categories/` and `speech-types/` segments exist, they need their own handlers.

All five are **GET-only on purpose**. Login and the create/update/delete
endpoints are deliberately not proxied, so the write surface stays off the
public internet. In-app editing therefore works only when the app is pointed at
a backend directly (dev settings → USB or LAN).

## Developing against a local backend

```bash
# 1. Start the API
dotnet run --project backend/backend.csproj --urls "http://0.0.0.0:6000"

# 2. USB-connected phone — tunnel 6000 over the cable.
#    Avoids the Windows Firewall and any question of which LAN IP to use.
adb reverse tcp:6000 tcp:6000

# 3. Run
cd mobile && flutter run
```

Then unlock the dev settings (7 taps on the version line) and pick
**کۆمپیوتەر / USB**. `adb reverse` must be re-run after every reconnect or reboot.

Note that `.env.local` in `frontend-nextjs` points `API_URL` at
`http://localhost:13934` (the IIS Express port) — change it if you run the
backend on 6000 instead.

## What's in it

| Screen | What it does |
| --- | --- |
| **گەڕان** | The main feed — every word, infinite scroll, debounced live search, category filter, pull to refresh |
| **پۆلەکان** | Every category and part of speech with counts; tap through to a scoped feed |
| **پاشەکەوت** | Bookmarks and view history, stored on-device and readable offline |
| Word detail | Headword, senses with dialect labels, categories, relations grouped and colour-coded |
| **نەخشەی بیر** | Force-directed mind map — pinch, pan, drag nodes, tap to focus, tap again to open |
| **ڕێکخستن** | Theme, text size, account, local data (+ hidden server section) |
| Editor | Create / edit / delete words, for Admin and Editor accounts |

Every word is shareable: the share button on a card, a long-press on a card, a
swipe on a saved word, or the detail screen. Shared text carries the word, its
parts of speech, every sense and its categories, then a link to
`/word/{id}` on the public site so it unfurls with an OG image.

## Build environment notes

Two workarounds are in place for this machine. Both are documented where they
live; remove them if the constraint goes away.

**`android/gradle.properties` — `kotlin.incremental=false`.** The repo is on `D:`
and the pub cache is on `C:`. Kotlin's incremental compiler stores relocatable
paths and cannot compute one across two Windows drive roots, so the build dies
with `this and base files have different roots`.

**`mobile/build` is a junction to `C:\KurdishDictionaryBuild\build`.** `D:` is
full and an Android build needs several GB of intermediates. Recreate with:

```powershell
cmd /c mklink /J "D:\Systems\KurdishDictionary\mobile\build" "C:\KurdishDictionaryBuild\build"
```

`rmdir` cannot delete the build tree once Gradle writes paths longer than 260
characters into it — mirror an empty directory over it first:

```powershell
robocopy "$env:TEMP\empty" "mobile\build" /MIR
cmd /c rmdir /s /q "mobile\build"
```

## Layout

```
lib/
  core/          theme tokens, API client, config, share text, server discovery
  models/        Dart mirrors of shared/Dtos/*.cs
  data/          repositories + SharedPreferences store
  state/         Riverpod providers; WordFeed (the pagination engine)
  ui/
    graph/       force simulation + painter for the mind map
    screens/     one file per screen
    widgets/     glass surfaces, chips, cards, skeletons, feed view
```

Riverpod handles dependency injection and shared state. The paginated feed is a
plain `ChangeNotifier` (`state/word_feed.dart`) because several feeds are alive
at once with different filters and the scroll listener drives it directly.

The app is RTL throughout — `Directionality` is set once in `app.dart`.
