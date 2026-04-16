"use client";
import { createContext, useContext, useEffect, useState, ReactNode } from "react";

type Theme = "dark" | "light";

interface ThemeCtxType {
  theme: Theme;
  toggle: () => void;
}

const ThemeCtx = createContext<ThemeCtxType>({ theme: "dark", toggle: () => {} });

export function ThemeProvider({ children }: { children: ReactNode }) {
  const [theme, setTheme] = useState<Theme>("dark");

  useEffect(() => {
    try {
      const saved = localStorage.getItem("kd-theme") as Theme | null;
      const sysDark = window.matchMedia("(prefers-color-scheme: dark)").matches;
      const initial: Theme = saved ?? (sysDark ? "dark" : "light");
      setTheme(initial);
      document.documentElement.setAttribute("data-theme", initial);
      document.documentElement.style.colorScheme = initial;
    } catch {
      // localStorage blocked (private/sandboxed) — keep default
    }
  }, []);

  const toggle = () => {
    setTheme((t) => {
      const next: Theme = t === "dark" ? "light" : "dark";
      try { localStorage.setItem("kd-theme", next); } catch { /* ignore */ }
      document.documentElement.setAttribute("data-theme", next);
      document.documentElement.style.colorScheme = next;
      return next;
    });
  };

  return <ThemeCtx.Provider value={{ theme, toggle }}>{children}</ThemeCtx.Provider>;
}

export function useTheme() {
  return useContext(ThemeCtx);
}
