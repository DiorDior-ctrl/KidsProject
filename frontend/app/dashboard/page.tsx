"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import Image from "next/image";

interface Child {
  id: string;
  name: string;
  age: number;
  level: string;
  progressPercent: number;
  lessonsCompleted: number;
  totalLessons: number;
  avatarEmoji: string;
}

interface Activity {
  id: string;
  childName: string;
  action: string;
  time: string;
  icon: string;
}

const MOCK_CHILDREN: Child[] = [
  {
    id: "1",
    name: "Dritëro Krasniqi",
    age: 7,
    level: "Fillestar",
    progressPercent: 35,
    lessonsCompleted: 7,
    totalLessons: 20,
    avatarEmoji: "🧒",
  },
];

const MOCK_ACTIVITIES: Activity[] = [
  {
    id: "1",
    childName: "Dritëro",
    action: "Mësoi alfabetin shqip – Shkronjat A, B, C",
    time: "Sot, 10:30",
    icon: "🔤",
  },
  {
    id: "2",
    childName: "Dritëro",
    action: "Kreu ushtrimet me zanore",
    time: "Dje, 16:00",
    icon: "✅",
  },
  {
    id: "3",
    childName: "Dritëro",
    action: "Lexoi historinë: \"Ora e Skënderbeut\"",
    time: "Dje, 15:30",
    icon: "📖",
  },
  {
    id: "4",
    childName: "Dritëro",
    action: "Fitoi badge: Fjalë të para 🏅",
    time: "3 ditë më parë",
    icon: "🏅",
  },
];

function ProgressBar({ percent }: { percent: number }) {
  return (
    <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-2.5 overflow-hidden">
      <div
        className="h-full rounded-full bg-gradient-to-r from-red-500 to-red-600 transition-all duration-700"
        style={{ width: `${percent}%` }}
      />
    </div>
  );
}

function ChildCard({ child }: { child: Child }) {
  return (
    <div className="bg-white dark:bg-gray-900 rounded-2xl border border-gray-200 dark:border-gray-800 p-6 hover:shadow-md hover:border-red-200 dark:hover:border-red-900 transition-all">
      <div className="flex items-start gap-4">
        <div className="w-14 h-14 rounded-2xl bg-red-50 dark:bg-red-900/20 flex items-center justify-center text-3xl flex-shrink-0">
          {child.avatarEmoji}
        </div>
        <div className="flex-1 min-w-0">
          <div className="flex items-center justify-between gap-2 mb-0.5">
            <h3 className="font-bold text-gray-900 dark:text-white truncate">
              {child.name}
            </h3>
            <span className="px-2.5 py-0.5 rounded-full text-xs font-semibold bg-red-100 dark:bg-red-900/30 text-red-700 dark:text-red-300 flex-shrink-0">
              {child.level}
            </span>
          </div>
          <p className="text-sm text-gray-500 dark:text-gray-400 mb-4">
            {child.age} vjeç
          </p>

          <div className="space-y-2">
            <div className="flex items-center justify-between text-sm">
              <span className="text-gray-600 dark:text-gray-400">Progres</span>
              <span className="font-semibold text-gray-900 dark:text-white">
                {child.lessonsCompleted}/{child.totalLessons} mësime
              </span>
            </div>
            <ProgressBar percent={child.progressPercent} />
            <p className="text-xs text-gray-400 dark:text-gray-500 text-right">
              {child.progressPercent}% i kompletuar
            </p>
          </div>
        </div>
      </div>

      <div className="mt-5 pt-4 border-t border-gray-100 dark:border-gray-800 flex gap-3">
        <button className="flex-1 py-2 px-4 text-sm font-medium rounded-xl bg-red-600 text-white hover:bg-red-700 transition-colors">
          Shiko progresin
        </button>
        <Link
          href="/dashboard/lesson/alphabet"
          className="flex-1 py-2 px-4 text-sm font-medium rounded-xl border border-gray-200 dark:border-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-800 transition-colors text-center"
        >
          Fillo mësimin
        </Link>
      </div>
    </div>
  );
}

function ActivityItem({ activity }: { activity: Activity }) {
  return (
    <div className="flex items-start gap-3 py-3">
      <div className="w-9 h-9 rounded-xl bg-gray-100 dark:bg-gray-800 flex items-center justify-center text-lg flex-shrink-0">
        {activity.icon}
      </div>
      <div className="flex-1 min-w-0">
        <p className="text-sm text-gray-900 dark:text-white">
          <span className="font-semibold">{activity.childName}</span>{" "}
          {activity.action}
        </p>
        <p className="text-xs text-gray-400 dark:text-gray-500 mt-0.5">
          {activity.time}
        </p>
      </div>
    </div>
  );
}

export default function DashboardPage() {
  const router = useRouter();
  const [parentName, setParentName] = useState<string>("");
  const [ready, setReady] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("mock_token");
    if (!token) {
      router.replace("/login");
      return;
    }
    const name = localStorage.getItem("parent_name") ?? "Prind";
    setParentName(name.charAt(0).toUpperCase() + name.slice(1));
    setReady(true);
  }, [router]);

  function handleLogout() {
    localStorage.removeItem("mock_token");
    localStorage.removeItem("parent_name");
    router.push("/login");
  }

  if (!ready) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-950">
        <Image src="/eagle_mascot.png" alt="Duke ngarkuar..." width={64} height={64} className="animate-pulse" />
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-950">
      {/* Top navbar */}
      <header className="sticky top-0 z-50 border-b border-gray-200 dark:border-gray-800 bg-white/90 dark:bg-gray-900/90 backdrop-blur-sm">
        <div className="max-w-6xl mx-auto px-4 sm:px-6 h-16 flex items-center justify-between">
          <Link href="/" className="flex items-center gap-2">
            <Image src="/eagle_mascot.png" alt="KidsProject" width={32} height={32} className="rounded-lg" />
            <span className="font-bold text-gray-900 dark:text-white hidden sm:block">
              KidsProject
            </span>
          </Link>
          <div className="flex items-center gap-4">
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-full bg-red-100 dark:bg-red-900/30 flex items-center justify-center text-sm font-bold text-red-600 dark:text-red-400">
                {parentName.charAt(0)}
              </div>
              <span className="text-sm font-medium text-gray-700 dark:text-gray-300 hidden sm:block">
                {parentName}
              </span>
            </div>
            <button
              onClick={handleLogout}
              className="text-sm text-gray-500 dark:text-gray-400 hover:text-red-600 dark:hover:text-red-400 transition-colors"
            >
              Dil
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-6xl mx-auto px-4 sm:px-6 py-8 space-y-8">
        {/* Welcome */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div>
            <h1 className="text-2xl sm:text-3xl font-bold text-gray-900 dark:text-white">
              Mirë se erdhe, {parentName}! 👋
            </h1>
            <p className="text-gray-500 dark:text-gray-400 mt-1">
              Këtu mund të ndiqni progresin dhe aktivitetet e fëmijës suaj.
            </p>
          </div>
          <button className="self-start sm:self-auto px-5 py-2.5 rounded-xl bg-red-600 text-white font-semibold hover:bg-red-700 active:scale-95 transition-all text-sm shadow-md shadow-red-100 dark:shadow-none flex items-center gap-2">
            <span>+</span>
            <span>Shto Fëmijë</span>
          </button>
        </div>

        {/* Stats strip */}
        <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
          {[
            { label: "Fëmijë aktiv", value: "1", icon: "👦" },
            { label: "Mësime sot", value: "3", icon: "📚" },
            { label: "Ditë radhazi", value: "5", icon: "🔥" },
            { label: "Badge-et", value: "1", icon: "🏅" },
          ].map((stat) => (
            <div
              key={stat.label}
              className="bg-white dark:bg-gray-900 rounded-2xl border border-gray-200 dark:border-gray-800 p-4 text-center"
            >
              <div className="text-2xl mb-1">{stat.icon}</div>
              <div className="text-2xl font-bold text-gray-900 dark:text-white">
                {stat.value}
              </div>
              <div className="text-xs text-gray-500 dark:text-gray-400 mt-0.5">
                {stat.label}
              </div>
            </div>
          ))}
        </div>

        {/* Lessons section */}
        <div className="space-y-4">
          <div className="flex items-center justify-between">
            <h2 className="text-lg font-bold text-gray-900 dark:text-white">
              Mësimet
            </h2>
            <span className="text-xs text-gray-400 dark:text-gray-500">
              1 nga 6 të disponueshme
            </span>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            <Link
              href="/dashboard/lesson/alphabet"
              className="group flex items-center gap-4 bg-white dark:bg-gray-900 rounded-2xl border-2 border-purple-200 dark:border-purple-800 p-5 hover:border-purple-400 dark:hover:border-purple-600 hover:shadow-lg transition-all"
            >
              <div className="w-14 h-14 rounded-2xl bg-gradient-to-br from-purple-400 to-pink-400 flex items-center justify-center text-3xl flex-shrink-0 shadow-md">
                🔤
              </div>
              <div className="flex-1 min-w-0">
                <p className="font-bold text-gray-900 dark:text-white group-hover:text-purple-600 dark:group-hover:text-purple-400 transition-colors">
                  Alfabeti Shqip
                </p>
                <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5">
                  36 shkronja • Fillestar
                </p>
                <div className="mt-2 w-full bg-gray-100 dark:bg-gray-800 rounded-full h-1.5 overflow-hidden">
                  <div className="h-full w-[35%] rounded-full bg-gradient-to-r from-purple-400 to-pink-400" />
                </div>
              </div>
              <span className="text-purple-400 dark:text-purple-500 text-xl group-hover:translate-x-1 transition-transform">
                →
              </span>
            </Link>

            {[
              { emoji: "🗣️", title: "Fjalët e Para", sub: "50 fjalë • Fillestar" },
              { emoji: "📖", title: "Leximi Bazë", sub: "10 histori • Mesatar" },
            ].map((lesson) => (
              <div
                key={lesson.title}
                className="flex items-center gap-4 bg-white dark:bg-gray-900 rounded-2xl border-2 border-gray-100 dark:border-gray-800 p-5 opacity-60 cursor-not-allowed"
              >
                <div className="w-14 h-14 rounded-2xl bg-gray-100 dark:bg-gray-800 flex items-center justify-center text-3xl flex-shrink-0">
                  {lesson.emoji}
                </div>
                <div className="flex-1 min-w-0">
                  <p className="font-bold text-gray-900 dark:text-white">
                    {lesson.title}
                  </p>
                  <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5">
                    {lesson.sub}
                  </p>
                  <span className="inline-block mt-2 text-xs bg-gray-100 dark:bg-gray-800 text-gray-500 dark:text-gray-400 px-2 py-0.5 rounded-full">
                    Së shpejti
                  </span>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Children section */}
          <div className="lg:col-span-2 space-y-4">
            <div className="flex items-center justify-between">
              <h2 className="text-lg font-bold text-gray-900 dark:text-white">
                Fëmijët
              </h2>
              <button className="text-sm text-red-600 dark:text-red-400 hover:underline font-medium">
                Shto fëmijë +
              </button>
            </div>
            {MOCK_CHILDREN.map((child) => (
              <ChildCard key={child.id} child={child} />
            ))}
          </div>

          {/* Recent activity */}
          <div className="space-y-4">
            <h2 className="text-lg font-bold text-gray-900 dark:text-white">
              Aktiviteti i fundit
            </h2>
            <div className="bg-white dark:bg-gray-900 rounded-2xl border border-gray-200 dark:border-gray-800 px-5 divide-y divide-gray-100 dark:divide-gray-800">
              {MOCK_ACTIVITIES.map((activity) => (
                <ActivityItem key={activity.id} activity={activity} />
              ))}
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
