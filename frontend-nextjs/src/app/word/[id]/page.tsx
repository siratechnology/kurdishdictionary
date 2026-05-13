import { Metadata } from "next";
import { headers } from "next/headers";
import Link from "next/link";
import { notFound } from "next/navigation";

// ── Types ────────────────────────────────────────────────────────────────────

interface SpeechPane { id: number; kurdish: string; }
interface Category   { id: number; name: string; }
interface Meaning    { id: number; meaning: string; locate?: string; }
interface RelatedWord {
  id: number;
  relatedWordId: number;
  relatedKurdish?: string;
  relationType: string;
  isIncoming: boolean;
  weight: number;
}

interface Word {
  id: number;
  kurdish: string;
  speechPanes: SpeechPane[];
  categories: Category[];
  gender: number;
  genderKurdish?: string;
  description?: string;
  createdAt: string;
  totalRelations: number;
  meanings: Meaning[];
  outgoingRelations: RelatedWord[];
  incomingRelations: RelatedWord[];
}

// ── Data fetching ─────────────────────────────────────────────────────────────

async function getWord(id: string): Promise<Word | null> {
  try {
    const res = await fetch(`${process.env.API_URL}/api/words/${id}`, {
      next: { revalidate: 3600 },
    });
    if (!res.ok) return null;
    return res.json();
  } catch {
    return null;
  }
}

// ── Metadata (OG + Twitter cards) ────────────────────────────────────────────

export async function generateMetadata(
  { params }: { params: Promise<{ id: string }> }
): Promise<Metadata> {
  const { id } = await params;
  const word = await getWord(id);

  // Derive absolute base URL from request headers (works in any Docker env)
  const headersList = await headers();
  const host  = headersList.get("x-forwarded-host") ?? headersList.get("host") ?? "localhost:4000";
  const proto = headersList.get("x-forwarded-proto") ?? (host.startsWith("localhost") ? "http" : "https");
  const baseUrl = `${proto}://${host}`;

  if (!word) {
    return {
      title: "وشە نەدۆزرایەوە | فەرهەنگی کوردی",
      description: "ئەم وشەیە لە فەرهەنگی کوردی دا نییە.",
    };
  }

  const speechLabel = word.speechPanes[0]?.kurdish ?? "";
  const categoryLabel = word.categories[0]?.name ?? "";
  const firstMeaning = word.meanings[0]?.meaning ?? word.description ?? "";
  const description = [speechLabel, categoryLabel, firstMeaning]
    .filter(Boolean)
    .join(" · ")
    .slice(0, 200);

  const ogImageUrl = `${baseUrl}/api/og/word/${id}`;

  return {
    title: `${word.kurdish} | فەرهەنگی کوردی`,
    description,
    openGraph: {
      title: `${word.kurdish} — فەرهەنگی کوردی`,
      description,
      url: `${baseUrl}/word/${id}`,
      siteName: "فەرهەنگی کوردی",
      images: [
        {
          url: ogImageUrl,
          width: 1200,
          height: 630,
          alt: `وشەی کوردی: ${word.kurdish}`,
        },
      ],
      locale: "ckb_IQ",
      type: "article",
    },
    twitter: {
      card: "summary_large_image",
      title: `${word.kurdish} — فەرهەنگی کوردی`,
      description,
      images: [ogImageUrl],
    },
  };
}

// ── Relation badge colours ────────────────────────────────────────────────────

const REL_STYLE: Record<string, { bg: string; border: string; color: string; label: string }> = {
  synonym:    { bg: "rgba(34,197,94,.12)",  border: "rgba(34,197,94,.30)",  color: "#86efac", label: "هاومانا"  },
  antonym:    { bg: "rgba(239,68,68,.12)",  border: "rgba(239,68,68,.30)",  color: "#fca5a5", label: "دژواتا"   },
  partof:     { bg: "rgba(251,191,36,.10)", border: "rgba(251,191,36,.28)", color: "#fcd34d", label: "بەشێکن"   },
  related:    { bg: "rgba(99,102,241,.12)", border: "rgba(99,102,241,.30)", color: "#a5b4fc", label: "پەیوەندی" },
  contextual: { bg: "rgba(6,182,212,.12)",  border: "rgba(6,182,212,.30)",  color: "#67e8f9", label: "بەیەکەوە" },
};

// ── Page ─────────────────────────────────────────────────────────────────────

export default async function WordDetailPage(
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const word = await getWord(id);
  if (!word) notFound();

  const allRelations = [
    ...word.outgoingRelations.map(r => ({ ...r, isIncoming: false })),
    ...word.incomingRelations.map(r => ({ ...r, isIncoming: true })),
  ];

  const dialectGroups = word.meanings.reduce<Map<string, string[]>>((acc, m) => {
    const key = m.locate?.trim() || "گشتی";
    if (!acc.has(key)) acc.set(key, []);
    acc.get(key)!.push(m.meaning);
    return acc;
  }, new Map());

  return (
    <main
      dir="rtl"
      style={{ minHeight: "100vh", background: "var(--background)", padding: "32px 16px" }}
    >
      <div style={{ maxWidth: 680, margin: "0 auto", display: "flex", flexDirection: "column", gap: 20 }}>

        {/* Back link */}
        <Link
          href="/"
          style={{
            display: "inline-flex",
            alignItems: "center",
            gap: 6,
            color: "var(--accent-light)",
            fontSize: 14,
            fontWeight: 600,
            textDecoration: "none",
            marginBottom: 4,
          }}
        >
          ← گەڕانەوە بۆ فەرهەنگەکە
        </Link>

        {/* ── Word header card ── */}
        <article
          style={{
            background: "var(--surface-card)",
            border: "1px solid var(--border-strong)",
            borderRadius: 20,
            padding: "32px 28px",
            backdropFilter: "blur(20px)",
          }}
        >
          {/* Kurdish word */}
          <h1
            style={{
              fontSize: 52,
              fontWeight: 800,
              color: "var(--t-text-1)",
              margin: 0,
              lineHeight: 1.2,
              fontFamily: "'NRT', system-ui, sans-serif",
            }}
          >
            {word.kurdish}
          </h1>

          {/* Badges row */}
          <div style={{ display: "flex", flexWrap: "wrap", gap: 8, marginTop: 16 }}>
            {word.speechPanes.map(sp => (
              <span
                key={sp.id}
                style={{
                  background: "rgba(99,102,241,0.14)",
                  border: "1px solid rgba(99,102,241,0.38)",
                  borderRadius: 100,
                  padding: "5px 14px",
                  color: "#a5b4fc",
                  fontSize: 13,
                  fontWeight: 600,
                }}
              >
                {sp.kurdish}
              </span>
            ))}
            {word.categories.map(cat => (
              <span
                key={cat.id}
                style={{
                  background: "rgba(16,185,129,0.12)",
                  border: "1px solid rgba(16,185,129,0.32)",
                  borderRadius: 100,
                  padding: "5px 14px",
                  color: "#6ee7b7",
                  fontSize: 13,
                  fontWeight: 600,
                }}
              >
                {cat.name}
              </span>
            ))}
            {word.genderKurdish && word.genderKurdish !== "نادیار" && (
              <span
                style={{
                  background: "rgba(139,92,246,0.12)",
                  border: "1px solid rgba(139,92,246,0.32)",
                  borderRadius: 100,
                  padding: "5px 14px",
                  color: "#c4b5fd",
                  fontSize: 13,
                  fontWeight: 600,
                }}
              >
                {word.genderKurdish}
              </span>
            )}
          </div>

          {/* Description */}
          {word.description && (
            <p style={{ marginTop: 16, fontSize: 15, color: "var(--t-text-3)", lineHeight: 1.6 }}>
              {word.description}
            </p>
          )}
        </article>

        {/* ── Meanings card ── */}
        {dialectGroups.size > 0 && (
          <section
            style={{
              background: "var(--surface-card)",
              border: "1px solid var(--border)",
              borderRadius: 20,
              padding: "24px 28px",
            }}
          >
            <h2 style={{ margin: "0 0 16px", fontSize: 16, fontWeight: 700, color: "var(--t-text-2)" }}>
              مانا
            </h2>
            <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
              {Array.from(dialectGroups.entries()).map(([dialect, meanings]) => (
                <div key={dialect} style={{ display: "flex", alignItems: "flex-start", gap: 10 }}>
                  <span
                    style={{
                      flexShrink: 0,
                      background: "rgba(6,182,212,0.10)",
                      border: "1px solid rgba(6,182,212,0.22)",
                      borderRadius: 100,
                      padding: "3px 10px",
                      color: "#22d3ee",
                      fontSize: 11,
                      fontWeight: 700,
                      marginTop: 2,
                    }}
                  >
                    {dialect}
                  </span>
                  <p style={{ margin: 0, fontSize: 15, color: "var(--t-text-1)", lineHeight: 1.6 }}>
                    {meanings.join(" · ")}
                  </p>
                </div>
              ))}
            </div>
          </section>
        )}

        {/* ── Related words card ── */}
        {allRelations.length > 0 && (
          <section
            style={{
              background: "var(--surface-card)",
              border: "1px solid var(--border)",
              borderRadius: 20,
              padding: "24px 28px",
            }}
          >
            <h2 style={{ margin: "0 0 16px", fontSize: 16, fontWeight: 700, color: "var(--t-text-2)" }}>
              وشە پەیوەندیدارەکان ({allRelations.length})
            </h2>
            <div style={{ display: "flex", flexWrap: "wrap", gap: 8 }}>
              {allRelations.map(rel => {
                const style = REL_STYLE[rel.relationType] ?? REL_STYLE.related;
                return (
                  <Link
                    key={`${rel.id}-${rel.isIncoming}`}
                    href={`/word/${rel.relatedWordId}`}
                    style={{
                      display: "inline-flex",
                      alignItems: "center",
                      gap: 5,
                      background: style.bg,
                      border: `1px solid ${style.border}`,
                      borderRadius: 100,
                      padding: "6px 14px",
                      textDecoration: "none",
                      fontSize: 14,
                      fontWeight: 600,
                      color: style.color,
                    }}
                  >
                    <span style={{ fontSize: 10, opacity: 0.7 }}>{style.label}</span>
                    {rel.relatedKurdish ?? `#${rel.relatedWordId}`}
                  </Link>
                );
              })}
            </div>
          </section>
        )}

        {/* ── CTA ── */}
        <div style={{ textAlign: "center", paddingTop: 8 }}>
          <Link
            href="/"
            style={{
              display: "inline-flex",
              alignItems: "center",
              gap: 8,
              background: "linear-gradient(90deg, #6366f1, #8b5cf6)",
              borderRadius: 100,
              padding: "12px 28px",
              color: "white",
              fontSize: 15,
              fontWeight: 700,
              textDecoration: "none",
            }}
          >
            فەرهەنگی کوردی بکە ↗
          </Link>
        </div>

      </div>
    </main>
  );
}
