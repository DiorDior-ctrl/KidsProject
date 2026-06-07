"use client";

import Link from "next/link";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useRouter } from "next/navigation";
import { useState } from "react";

const loginSchema = z.object({
  email: z.string().email("Adresë email-i e pavlefshme"),
  password: z.string().min(1, "Fjalëkalimi është i detyrueshëm"),
});

type LoginFormData = z.infer<typeof loginSchema>;

export default function LoginPage() {
  const router = useRouter();
  const [serverError, setServerError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  async function onSubmit(data: LoginFormData) {
    setServerError(null);
    await new Promise((resolve) => setTimeout(resolve, 800));

    if (data.email === "gabim@test.com") {
      setServerError("Email-i ose fjalëkalimi është i gabuar.");
      return;
    }

    localStorage.setItem("mock_token", "mock_jwt_eyJhbGciOiJSUzI1NiJ9");
    localStorage.setItem("parent_name", data.email.split("@")[0]);

    router.push("/dashboard");
  }

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50 dark:bg-gray-950 px-4 py-12">
      <div className="w-full max-w-md">
        {/* Logo */}
        <div className="text-center mb-8">
          <Link href="/" className="inline-flex items-center gap-2">
            <span className="text-4xl">🦅</span>
            <span className="text-2xl font-bold text-gray-900 dark:text-white">
              KidsProject
            </span>
          </Link>
          <h1 className="mt-4 text-2xl font-bold text-gray-900 dark:text-white">
            Mirë se u kthyet!
          </h1>
          <p className="mt-1 text-gray-500 dark:text-gray-400">
            Hyni për të parë progresin e fëmijës suaj
          </p>
        </div>

        <form
          onSubmit={handleSubmit(onSubmit)}
          className="bg-white dark:bg-gray-900 rounded-2xl border border-gray-200 dark:border-gray-800 p-8 shadow-sm space-y-5"
        >
          {serverError && (
            <div className="rounded-xl bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 px-4 py-3 text-sm text-red-600 dark:text-red-400">
              {serverError}
            </div>
          )}

          {/* Email */}
          <div>
            <label
              htmlFor="email"
              className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5"
            >
              Email
            </label>
            <input
              id="email"
              type="email"
              autoComplete="email"
              placeholder="ju@shembull.com"
              {...register("email")}
              className="w-full px-4 py-3 rounded-xl border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-transparent transition"
            />
            {errors.email && (
              <p className="mt-1.5 text-sm text-red-500">
                {errors.email.message}
              </p>
            )}
          </div>

          {/* Password */}
          <div>
            <div className="flex items-center justify-between mb-1.5">
              <label
                htmlFor="password"
                className="block text-sm font-medium text-gray-700 dark:text-gray-300"
              >
                Fjalëkalimi
              </label>
              <span className="text-xs text-red-600 dark:text-red-400 hover:underline cursor-pointer">
                Harruat fjalëkalimin?
              </span>
            </div>
            <input
              id="password"
              type="password"
              autoComplete="current-password"
              placeholder="Fjalëkalimi juaj"
              {...register("password")}
              className="w-full px-4 py-3 rounded-xl border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-transparent transition"
            />
            {errors.password && (
              <p className="mt-1.5 text-sm text-red-500">
                {errors.password.message}
              </p>
            )}
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full py-3 px-6 rounded-xl bg-red-600 text-white font-semibold hover:bg-red-700 active:scale-95 disabled:opacity-60 disabled:cursor-not-allowed transition-all shadow-md shadow-red-100 dark:shadow-none"
          >
            {isSubmitting ? "Duke u kyçur..." : "Hyr"}
          </button>

          <p className="text-center text-sm text-gray-500 dark:text-gray-400">
            Nuk keni llogari?{" "}
            <Link
              href="/register"
              className="font-medium text-red-600 dark:text-red-400 hover:underline"
            >
              Regjistrohuni falas
            </Link>
          </p>
        </form>

        <p className="mt-6 text-center text-xs text-gray-400 dark:text-gray-600">
          Për demo: çdo email funksionon (jo gabim@test.com)
        </p>
      </div>
    </div>
  );
}
