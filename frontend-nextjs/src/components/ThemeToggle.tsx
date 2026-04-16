"use client";
import { useTheme } from "./ThemeProvider";

export default function ThemeToggle() {
  const { theme, toggle } = useTheme();
  const isDark = theme === "dark";

  // Sun ray angles
  const rays = [0, 45, 90, 135, 180, 225, 270, 315];

  return (
    <button
      onClick={toggle}
      aria-label={isDark ? "Switch to light mode" : "Switch to dark mode"}
      title={isDark ? "Light mode" : "Dark mode"}
      className="relative shrink-0 w-14 h-7 rounded-full transition-all duration-500 focus:outline-none focus-visible:ring-2 focus-visible:ring-offset-2"
      style={{
        background: isDark
          ? "linear-gradient(135deg, #1e1b4b 0%, #312e81 100%)"
          : "linear-gradient(135deg, #dbeafe 0%, #e0e7ff 100%)",
        border: isDark
          ? "1px solid rgba(99,102,241,0.45)"
          : "1px solid rgba(99,102,241,0.28)",
        boxShadow: isDark
          ? "0 2px 14px rgba(99,102,241,0.35), inset 0 1px 0 rgba(255,255,255,0.06)"
          : "0 2px 14px rgba(99,102,241,0.18), inset 0 1px 0 rgba(255,255,255,0.8)",
      }}
    >
      {/* Track background icons */}
      <span className="absolute inset-0 flex items-center justify-between px-[5px] pointer-events-none select-none">
        {/* Moon — left side, bright in dark mode */}
        <svg
          width="14" height="14" viewBox="0 0 24 24" fill="none"
          style={{ opacity: isDark ? 1 : 0.25, transition: "opacity 0.4s", flexShrink: 0 }}
        >
          <path
            d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"
            fill={isDark ? "#a5b4fc" : "#94a3b8"}
          />
        </svg>
        {/* Sun — right side, bright in light mode */}
        <svg
          width="14" height="14" viewBox="0 0 24 24" fill="none"
          style={{ opacity: isDark ? 0.25 : 1, transition: "opacity 0.4s", flexShrink: 0 }}
        >
          <circle cx="12" cy="12" r="4" fill={isDark ? "#94a3b8" : "#f59e0b"} />
          {rays.map((deg) => {
            const rad = (deg * Math.PI) / 180;
            return (
              <line
                key={deg}
                x1={(12 + 6.5 * Math.cos(rad)).toFixed(2)}
                y1={(12 + 6.5 * Math.sin(rad)).toFixed(2)}
                x2={(12 + 9 * Math.cos(rad)).toFixed(2)}
                y2={(12 + 9 * Math.sin(rad)).toFixed(2)}
                stroke={isDark ? "#94a3b8" : "#f59e0b"}
                strokeWidth="2"
                strokeLinecap="round"
              />
            );
          })}
        </svg>
      </span>

      {/* Sliding knob */}
      <span
        className="absolute top-[3px] w-[22px] h-[22px] rounded-full"
        style={{
          left: isDark ? "3px" : "calc(100% - 25px)",
          transition: "left 0.45s cubic-bezier(0.34, 1.56, 0.64, 1)",
          background: isDark
            ? "linear-gradient(135deg, #818cf8 0%, #6366f1 100%)"
            : "linear-gradient(135deg, #fde68a 0%, #f59e0b 100%)",
          boxShadow: isDark
            ? "0 2px 8px rgba(99,102,241,0.55), inset 0 1px 0 rgba(255,255,255,0.25)"
            : "0 2px 8px rgba(251,191,36,0.45), inset 0 1px 0 rgba(255,255,255,0.6)",
        }}
      />
    </button>
  );
}
