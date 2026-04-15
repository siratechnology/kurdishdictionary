import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "فەرهەنگی کوردی",
  description: "Kurdish to Kurdish dictionary",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="ku" className="h-full antialiased" style={{ colorScheme: "light" }}>
      <body className="min-h-full flex flex-col">{children}</body>
    </html>
  );
}
