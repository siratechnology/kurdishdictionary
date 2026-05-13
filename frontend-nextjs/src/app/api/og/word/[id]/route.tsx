import { ImageResponse } from "next/og";
import { NextRequest } from "next/server";
import { readFileSync } from "fs";
import { join } from "path";

export const dynamic = "force-dynamic";

// Load NRT Kurdish font once at module init
const nrtFont = readFileSync(join(process.cwd(), "public", "fonts", "NRT-Reg.ttf"));

interface WordMeta {
  id: number;
  kurdish: string;
  speechPanes: { id: number; kurdish: string }[];
  categories: { id: number; name: string }[];
  genderKurdish?: string;
  firstMeaning?: string;
  description?: string;
}

function fontSize(word: string) {
  const len = word.length;
  if (len <= 4)  return 136;
  if (len <= 7)  return 112;
  if (len <= 10) return 90;
  if (len <= 14) return 72;
  return 58;
}

export async function GET(
  _request: NextRequest,
  context: { params: Promise<{ id: string }> }
) {
  const { id } = await context.params;

  let word: WordMeta | null = null;
  try {
    const res = await fetch(`${process.env.API_URL}/api/words/${id}/meta`, {
      cache: "no-store",
    });
    if (res.ok) word = await res.json();
  } catch {
    // render brand fallback below
  }

  const kurdish   = word?.kurdish ?? "فەرهەنگی کوردی";
  const speechPane = word?.speechPanes[0]?.kurdish ?? "";
  const category  = word?.categories[0]?.name ?? "";
  const gender    = word?.genderKurdish && word.genderKurdish !== "نادیار" ? word.genderKurdish : "";
  const meaning   = word?.firstMeaning ?? word?.description ?? "";
  const shortMeaning = meaning.length > 110 ? meaning.slice(0, 107) + "…" : meaning;

  return new ImageResponse(
    (
      <div
        style={{
          display: "flex",
          width: "1200px",
          height: "630px",
          background: "linear-gradient(135deg, #080c1a 0%, #1e1b4b 55%, #0c1527 100%)",
          fontFamily: "NRT, sans-serif",
          position: "relative",
          overflow: "hidden",
        }}
      >
        {/* Indigo glow — top right */}
        <div style={{
          display: "flex",
          position: "absolute",
          top: -130, right: -90,
          width: 500, height: 500,
          borderRadius: "50%",
          background: "radial-gradient(circle, rgba(99,102,241,0.32) 0%, transparent 68%)",
        }} />

        {/* Emerald glow — bottom left */}
        <div style={{
          display: "flex",
          position: "absolute",
          bottom: -100, left: 60,
          width: 420, height: 420,
          borderRadius: "50%",
          background: "radial-gradient(circle, rgba(16,185,129,0.22) 0%, transparent 68%)",
        }} />

        {/* Purple glow — bottom right */}
        <div style={{
          display: "flex",
          position: "absolute",
          bottom: -60, right: 200,
          width: 260, height: 260,
          borderRadius: "50%",
          background: "radial-gradient(circle, rgba(139,92,246,0.18) 0%, transparent 68%)",
        }} />

        {/* Glass card */}
        <div style={{
          display: "flex",
          flexDirection: "column",
          position: "absolute",
          top: 36, left: 36, right: 36, bottom: 36,
          background: "rgba(13, 20, 40, 0.78)",
          border: "1.5px solid rgba(99,102,241,0.28)",
          borderRadius: "28px",
          padding: "44px 52px",
        }}>

          {/* ── Top row: brand | badges ── */}
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>

            <div style={{ display: "flex", flexDirection: "column", gap: "6px" }}>
              <span style={{ color: "#818cf8", fontSize: "21px", fontWeight: 600, letterSpacing: "2px" }}>
                فەرهەنگی کوردی
              </span>
              <span style={{ color: "rgba(100,116,139,0.8)", fontSize: "14px", letterSpacing: "1px" }}>
                Kurdish Dictionary
              </span>
            </div>

            <div style={{ display: "flex", gap: "10px" }}>
              {speechPane && (
                <div style={{
                  display: "flex",
                  background: "rgba(99,102,241,0.14)",
                  border: "1px solid rgba(99,102,241,0.42)",
                  borderRadius: "100px",
                  padding: "8px 20px",
                  color: "#a5b4fc",
                  fontSize: "18px",
                  fontWeight: 600,
                }}>
                  {speechPane}
                </div>
              )}
              {category && (
                <div style={{
                  display: "flex",
                  background: "rgba(16,185,129,0.12)",
                  border: "1px solid rgba(16,185,129,0.38)",
                  borderRadius: "100px",
                  padding: "8px 20px",
                  color: "#6ee7b7",
                  fontSize: "18px",
                  fontWeight: 600,
                }}>
                  {category}
                </div>
              )}
              {gender && (
                <div style={{
                  display: "flex",
                  background: "rgba(139,92,246,0.12)",
                  border: "1px solid rgba(139,92,246,0.38)",
                  borderRadius: "100px",
                  padding: "8px 20px",
                  color: "#c4b5fd",
                  fontSize: "18px",
                  fontWeight: 600,
                }}>
                  {gender}
                </div>
              )}
            </div>
          </div>

          {/* Emerald accent line */}
          <div style={{
            display: "flex",
            height: "2px",
            background: "linear-gradient(90deg, rgba(16,185,129,0.75), rgba(99,102,241,0.35), transparent)",
            marginTop: "28px",
            marginBottom: "24px",
          }} />

          {/* ── Main Kurdish word ── */}
          <div style={{ display: "flex", flex: 1, alignItems: "center", justifyContent: "flex-end" }}>
            <span style={{
              fontSize: `${fontSize(kurdish)}px`,
              fontWeight: 700,
              color: "#f1f5f9",
              textAlign: "right",
              direction: "rtl",
              lineHeight: 1.15,
            }}>
              {kurdish}
            </span>
          </div>

          {/* Meaning */}
          {shortMeaning && (
            <div style={{ display: "flex", justifyContent: "flex-end", marginTop: "14px" }}>
              <span style={{
                fontSize: "24px",
                color: "rgba(148,163,184,0.82)",
                textAlign: "right",
                direction: "rtl",
                lineHeight: 1.5,
                maxWidth: "720px",
              }}>
                {shortMeaning}
              </span>
            </div>
          )}

          {/* ── Bottom bar ── */}
          <div style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            marginTop: "26px",
            paddingTop: "18px",
            borderTop: "1px solid rgba(99,102,241,0.12)",
          }}>
            <div style={{
              display: "flex",
              background: "linear-gradient(90deg, #6366f1, #8b5cf6)",
              borderRadius: "100px",
              padding: "10px 26px",
              color: "white",
              fontSize: "16px",
              fontWeight: 700,
              letterSpacing: "0.5px",
            }}>
              کلیک بکە ← وشەی تەواو ببینە
            </div>
            <span style={{ color: "rgba(99,102,241,0.45)", fontSize: "15px" }}>
              #{word?.id ?? "—"}
            </span>
          </div>
        </div>
      </div>
    ),
    {
      width: 1200,
      height: 630,
      fonts: [
        { name: "NRT", data: nrtFont, weight: 400, style: "normal" },
      ],
    }
  );
}
