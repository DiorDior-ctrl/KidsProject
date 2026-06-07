"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import Image from "next/image";

interface LetterCard {
  letter: string;
  word: string;
  emoji: string;
}

const ALPHABET: LetterCard[] = [
  { letter: "A", word: "Arush", emoji: "🐻" },
  { letter: "B", word: "Bukë", emoji: "🍞" },
  { letter: "C", word: "Cjap", emoji: "🐐" },
  { letter: "Ç", word: "Çantë", emoji: "🎒" },
  { letter: "D", word: "Deti", emoji: "🌊" },
  { letter: "Dh", word: "Dhelpër", emoji: "🦊" },
  { letter: "E", word: "Enë", emoji: "🍽️" },
  { letter: "Ë", word: "Ëmbël", emoji: "🍯" },
  { letter: "F", word: "Fëmijë", emoji: "👶" },
  { letter: "G", word: "Gaforre", emoji: "🦀" },
  { letter: "Gj", word: "Gjarpër", emoji: "🐍" },
  { letter: "H", word: "Hënë", emoji: "🌙" },
  { letter: "I", word: "Ishull", emoji: "🏝️" },
  { letter: "J", word: "Jastëk", emoji: "🛏️" },
  { letter: "K", word: "Kalë", emoji: "🐴" },
  { letter: "L", word: "Lule", emoji: "🌸" },
  { letter: "Ll", word: "Llambë", emoji: "💡" },
  { letter: "M", word: "Mollë", emoji: "🍎" },
  { letter: "N", word: "Nënë", emoji: "👩" },
  { letter: "Nj", word: "Njeri", emoji: "🧑" },
  { letter: "O", word: "Orë", emoji: "⏰" },
  { letter: "P", word: "Pulë", emoji: "🐔" },
  { letter: "Q", word: "Qeni", emoji: "🐕" },
  { letter: "R", word: "Resë", emoji: "🌫️" },
  { letter: "Rr", word: "Rrugë", emoji: "🛣️" },
  { letter: "S", word: "Sorrë", emoji: "🐦" },
  { letter: "Sh", word: "Shtëpi", emoji: "🏠" },
  { letter: "T", word: "Topi", emoji: "⚽" },
  { letter: "Th", word: "Thi", emoji: "🐷" },
  { letter: "U", word: "Ujk", emoji: "🐺" },
  { letter: "V", word: "Vezë", emoji: "🥚" },
  { letter: "X", word: "Xixë", emoji: "✨" },
  { letter: "Xh", word: "Xhufkë", emoji: "🧢" },
  { letter: "Y", word: "Ylli", emoji: "⭐" },
  { letter: "Z", word: "Zog", emoji: "🐣" },
  { letter: "Zh", word: "Zhurmë", emoji: "🔊" },
];

const CARD_THEMES = [
  { base: "border-red-300 bg-red-50 dark:bg-red-950/30 dark:border-red-800", letter: "text-red-500 dark:text-red-400", done: "bg-red-400 dark:bg-red-600" },
  { base: "border-orange-300 bg-orange-50 dark:bg-orange-950/30 dark:border-orange-800", letter: "text-orange-500 dark:text-orange-400", done: "bg-orange-400 dark:bg-orange-600" },
  { base: "border-amber-300 bg-amber-50 dark:bg-amber-950/30 dark:border-amber-800", letter: "text-amber-500 dark:text-amber-400", done: "bg-amber-400 dark:bg-amber-600" },
  { base: "border-yellow-300 bg-yellow-50 dark:bg-yellow-950/30 dark:border-yellow-800", letter: "text-yellow-600 dark:text-yellow-400", done: "bg-yellow-400 dark:bg-yellow-600" },
  { base: "border-lime-300 bg-lime-50 dark:bg-lime-950/30 dark:border-lime-800", letter: "text-lime-600 dark:text-lime-400", done: "bg-lime-500 dark:bg-lime-600" },
  { base: "border-green-300 bg-green-50 dark:bg-green-950/30 dark:border-green-800", letter: "text-green-600 dark:text-green-400", done: "bg-green-500 dark:bg-green-600" },
  { base: "border-teal-300 bg-teal-50 dark:bg-teal-950/30 dark:border-teal-800", letter: "text-teal-600 dark:text-teal-400", done: "bg-teal-500 dark:bg-teal-600" },
  { base: "border-sky-300 bg-sky-50 dark:bg-sky-950/30 dark:border-sky-800", letter: "text-sky-600 dark:text-sky-400", done: "bg-sky-500 dark:bg-sky-600" },
  { base: "border-blue-300 bg-blue-50 dark:bg-blue-950/30 dark:border-blue-800", letter: "text-blue-600 dark:text-blue-400", done: "bg-blue-500 dark:bg-blue-600" },
  { base: "border-violet-300 bg-violet-50 dark:bg-violet-950/30 dark:border-violet-800", letter: "text-violet-600 dark:text-violet-400", done: "bg-violet-500 dark:bg-violet-600" },
  { base: "border-purple-300 bg-purple-50 dark:bg-purple-950/30 dark:border-purple-800", letter: "text-purple-600 dark:text-purple-400", done: "bg-purple-500 dark:bg-purple-600" },
  { base: "border-pink-300 bg-pink-50 dark:bg-pink-950/30 dark:border-pink-800", letter: "text-pink-600 dark:text-pink-400", done: "bg-pink-500 dark:bg-pink-600" },
];

export default function AlphabetLessonPage() {
  const router = useRouter();
  const [ready, setReady] = useState(false);
  const [learned, setLearned] = useState<Set<string>>(new Set());
  const [justLearned, setJustLearned] = useState<string | null>(null);

  useEffect(() => {
    const token = localStorage.getItem("mock_token");
    if (!token) {
      router.replace("/login");
      return;
    }
    setReady(true);
  }, [router]);

  function handleCardClick(letter: string) {
    setLearned((prev) => {
      const next = new Set(prev);
      if (next.has(letter)) {
        next.delete(letter);
      } else {
        next.add(letter);
        setJustLearned(letter);
        setTimeout(() => setJustLearned(null), 600);
      }
      return next;
    });
  }

  if (!ready) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-purple-50 dark:bg-gray-950">
        <Image
          src="/eagle_mascot.png"
          alt="Duke ngarkuar..."
          width={64}
          height={64}
          className="animate-pulse"
        />
      </div>
    );
  }

  const progressPercent = Math.round((learned.size / ALPHABET.length) * 100);
  const allLearned = learned.size === ALPHABET.length;

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-50 via-blue-50 to-pink-50 dark:from-gray-950 dark:via-gray-900 dark:to-gray-950">
      {/* Sticky header */}
      <header className="sticky top-0 z-50 bg-white/90 dark:bg-gray-900/90 backdrop-blur-sm border-b border-gray-200 dark:border-gray-800 shadow-sm">
        <div className="max-w-5xl mx-auto px-4 sm:px-6 py-3">
          <div className="flex items-center gap-4 mb-3">
            <Link
              href="/dashboard"
              className="flex items-center gap-1.5 text-sm font-semibold text-gray-600 dark:text-gray-400 hover:text-purple-600 dark:hover:text-purple-400 transition-colors"
            >
              <span className="text-lg">←</span>
              <span>Kthehu</span>
            </Link>

            <div className="flex-1 text-center">
              <h1 className="text-base sm:text-lg font-extrabold text-gray-900 dark:text-white">
                🔤 Alfabeti Shqip
              </h1>
            </div>

            <div className="text-sm font-bold text-purple-600 dark:text-purple-400 min-w-[48px] text-right">
              {learned.size}/{ALPHABET.length}
            </div>
          </div>

          {/* Progress bar */}
          <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-3 overflow-hidden">
            <div
              className="h-full rounded-full bg-gradient-to-r from-purple-500 to-pink-500 transition-all duration-500"
              style={{ width: `${progressPercent}%` }}
            />
          </div>
          <div className="flex justify-between text-xs text-gray-400 dark:text-gray-500 mt-1">
            <span>Fillimi</span>
            <span>{progressPercent}% e kompletuar</span>
            <span>Fundi</span>
          </div>
        </div>
      </header>

      <main className="max-w-5xl mx-auto px-3 sm:px-6 py-6 pb-16">
        {/* Celebration banner */}
        {allLearned && (
          <div className="mb-6 rounded-2xl bg-gradient-to-r from-yellow-400 to-orange-400 p-5 text-center shadow-lg">
            <div className="text-4xl mb-2">🎉🏆🎉</div>
            <p className="text-xl font-extrabold text-white">
              Bravo! Mësuat të gjitha 36 shkronjat!
            </p>
            <p className="text-yellow-100 text-sm mt-1">
              Jeni kampion i alfabetit shqip!
            </p>
          </div>
        )}

        {/* Instruction */}
        {!allLearned && (
          <p className="text-center text-sm text-gray-500 dark:text-gray-400 mb-6">
            Kliko mbi çdo shkronjë kur ta mësosh ✋
          </p>
        )}

        {/* Alphabet grid */}
        <div className="grid grid-cols-4 sm:grid-cols-6 md:grid-cols-9 gap-2 sm:gap-3">
          {ALPHABET.map((card, i) => {
            const theme = CARD_THEMES[i % CARD_THEMES.length];
            const isLearned = learned.has(card.letter);
            const isJustLearned = justLearned === card.letter;

            return (
              <button
                key={card.letter}
                onClick={() => handleCardClick(card.letter)}
                className={[
                  "relative flex flex-col items-center justify-center rounded-2xl border-2 p-2 sm:p-3 cursor-pointer select-none",
                  "transition-all duration-300 ease-in-out",
                  "focus:outline-none focus-visible:ring-4 focus-visible:ring-purple-400",
                  isLearned
                    ? `${theme.done} border-transparent text-white shadow-md scale-95`
                    : `${theme.base} hover:scale-105 hover:shadow-lg active:scale-90`,
                  isJustLearned ? "scale-110" : "",
                ].join(" ")}
                aria-pressed={isLearned}
                aria-label={`${card.letter} — ${card.word}`}
              >
                {/* Checkmark badge */}
                {isLearned && (
                  <span className="absolute -top-1.5 -right-1.5 w-5 h-5 rounded-full bg-white text-green-500 text-xs flex items-center justify-center font-bold shadow">
                    ✓
                  </span>
                )}

                {/* Letter */}
                <span
                  className={[
                    "font-black leading-none",
                    card.letter.length === 1
                      ? "text-3xl sm:text-4xl"
                      : "text-xl sm:text-2xl",
                    isLearned ? "text-white" : theme.letter,
                  ].join(" ")}
                >
                  {card.letter}
                </span>

                {/* Emoji */}
                <span className="text-xl sm:text-2xl mt-1 leading-none">
                  {card.emoji}
                </span>

                {/* Word */}
                <span
                  className={[
                    "text-xs font-semibold mt-1 leading-tight text-center",
                    isLearned
                      ? "text-white/90"
                      : "text-gray-600 dark:text-gray-400",
                  ].join(" ")}
                >
                  {card.word}
                </span>
              </button>
            );
          })}
        </div>

        {/* Reset button */}
        {learned.size > 0 && (
          <div className="mt-8 text-center">
            <button
              onClick={() => setLearned(new Set())}
              className="text-sm text-gray-400 dark:text-gray-500 hover:text-red-500 dark:hover:text-red-400 transition-colors underline underline-offset-2"
            >
              Fillo sërish nga fillimi
            </button>
          </div>
        )}
      </main>
    </div>
  );
}
