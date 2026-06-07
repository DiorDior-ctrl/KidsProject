import Link from "next/link";
import Navbar from "@/app/components/Navbar";

const features = [
  {
    icon: "🎮",
    title: "Mësim Ndërveprues",
    description:
      "Lojëra dhe aktivitete argëtuese që e mbajnë fëmijën të angazhuar me gjuhën shqipe.",
  },
  {
    icon: "📚",
    title: "Kurrikul i Strukturuar",
    description:
      "Mësime të hartuara nga mësues të certifikuar, nga alfabeti deri tek foljet.",
  },
  {
    icon: "👨‍👩‍👧",
    title: "Kontroll Prindëror",
    description:
      "Ndiqni progresin e fëmijës, shikoni aktivitetin dhe menaxhoni seancat e mësimit.",
  },
];

export default function LandingPage() {
  return (
    <div className="flex flex-col min-h-screen">
      <Navbar />

      <main className="flex-1">
        {/* Hero */}
        <section className="relative overflow-hidden bg-gradient-to-br from-red-50 to-white dark:from-gray-900 dark:to-gray-950">
          <div className="absolute inset-0 bg-[url('data:image/svg+xml,%3Csvg%20width%3D%2260%22%20height%3D%2260%22%20viewBox%3D%220%200%2060%2060%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%3Cg%20fill%3D%22none%22%20fill-rule%3D%22evenodd%22%3E%3Cg%20fill%3D%22%23ef4444%22%20fill-opacity%3D%220.04%22%3E%3Cpath%20d%3D%22M36%2034v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6%2034v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6%204V0H4v4H0v2h4v4h2V6h4V4H6z%22%2F%3E%3C%2Fg%3E%3C%2Fg%3E%3C%2Fsvg%3E')] opacity-30" />

          <div className="relative max-w-6xl mx-auto px-4 sm:px-6 py-24 sm:py-36 text-center">
            <div className="text-8xl sm:text-9xl mb-8 animate-bounce-slow select-none">
              🦅
            </div>

            <h1 className="text-5xl sm:text-7xl font-extrabold tracking-tight text-gray-900 dark:text-white mb-6">
              Mëso{" "}
              <span className="text-red-600 dark:text-red-400">Shqipen</span>
            </h1>

            <p className="max-w-2xl mx-auto text-xl sm:text-2xl text-gray-600 dark:text-gray-400 mb-10 leading-relaxed">
              Platforma e mësimit të gjuhës shqipe për fëmijët e diasporës.
              Argëtim, progres dhe lidhje me rrënjët.
            </p>

            <div className="flex flex-col sm:flex-row items-center justify-center gap-4">
              <Link
                href="/register"
                className="w-full sm:w-auto px-10 py-4 text-lg font-bold rounded-xl bg-red-600 text-white hover:bg-red-700 active:scale-95 transition-all shadow-lg shadow-red-200 dark:shadow-red-900/30"
              >
                Fillo Falas 🚀
              </Link>
              <Link
                href="/login"
                className="w-full sm:w-auto px-10 py-4 text-lg font-semibold rounded-xl border-2 border-gray-300 dark:border-gray-700 text-gray-700 dark:text-gray-300 hover:border-red-400 hover:text-red-600 dark:hover:text-red-400 transition-all"
              >
                Hyr në llogari
              </Link>
            </div>

            <p className="mt-6 text-sm text-gray-500 dark:text-gray-500">
              Pa kartë krediti • Falas për 30 ditë
            </p>
          </div>
        </section>

        {/* Features */}
        <section className="max-w-6xl mx-auto px-4 sm:px-6 py-20">
          <h2 className="text-3xl sm:text-4xl font-bold text-center text-gray-900 dark:text-white mb-4">
            Pse të zgjidhni KidsProject?
          </h2>
          <p className="text-center text-gray-500 dark:text-gray-400 mb-14 text-lg">
            Ndërtuar posaçërisht për familjet shqiptare jashtë vendit.
          </p>

          <div className="grid grid-cols-1 sm:grid-cols-3 gap-8">
            {features.map((f) => (
              <div
                key={f.title}
                className="flex flex-col items-center text-center p-8 rounded-2xl border border-gray-100 dark:border-gray-800 bg-gray-50 dark:bg-gray-900 hover:border-red-200 dark:hover:border-red-900 hover:shadow-md transition-all"
              >
                <span className="text-5xl mb-5">{f.icon}</span>
                <h3 className="text-xl font-bold text-gray-900 dark:text-white mb-3">
                  {f.title}
                </h3>
                <p className="text-gray-600 dark:text-gray-400 leading-relaxed">
                  {f.description}
                </p>
              </div>
            ))}
          </div>
        </section>

        {/* CTA Banner */}
        <section className="bg-red-600 dark:bg-red-700">
          <div className="max-w-4xl mx-auto px-4 sm:px-6 py-16 text-center">
            <h2 className="text-3xl sm:text-4xl font-extrabold text-white mb-4">
              Gati të filloni?
            </h2>
            <p className="text-red-100 text-lg mb-8">
              Bashkohuni me qindra familje që ruajnë gjuhën shqipe.
            </p>
            <Link
              href="/register"
              className="inline-block px-10 py-4 text-lg font-bold rounded-xl bg-white text-red-600 hover:bg-red-50 active:scale-95 transition-all shadow-lg"
            >
              Fillo Falas
            </Link>
          </div>
        </section>
      </main>

      {/* Footer */}
      <footer className="border-t border-gray-200 dark:border-gray-800 py-8">
        <div className="max-w-6xl mx-auto px-4 sm:px-6 text-center text-sm text-gray-500 dark:text-gray-400">
          © 2026 KidsProject · Bërë me ❤️ për diasporën shqiptare
        </div>
      </footer>
    </div>
  );
}
