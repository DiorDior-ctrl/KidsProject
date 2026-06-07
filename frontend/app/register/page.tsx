"use client";

import Link from "next/link";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useState } from "react";

const registerSchema = z
  .object({
    name: z
      .string()
      .min(2, "Emri duhet të ketë të paktën 2 karaktere")
      .max(50, "Emri është shumë i gjatë"),
    email: z.string().email("Adresë email-i e pavlefshme"),
    password: z
      .string()
      .min(8, "Fjalëkalimi duhet të ketë të paktën 8 karaktere"),
    confirmPassword: z.string(),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Fjalëkalimet nuk përputhen",
    path: ["confirmPassword"],
  });

type RegisterFormData = z.infer<typeof registerSchema>;

export default function RegisterPage() {
  const [submitted, setSubmitted] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  function onSubmit(_data: RegisterFormData) {
    setSubmitted(true);
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
            Krijo llogari
          </h1>
          <p className="mt-1 text-gray-500 dark:text-gray-400">
            Filloni udhëtimin me gjuhën shqipe
          </p>
        </div>

        {submitted ? (
          <div className="bg-white dark:bg-gray-900 rounded-2xl border border-gray-200 dark:border-gray-800 p-8 text-center shadow-sm">
            <div className="text-5xl mb-4">🎉</div>
            <h2 className="text-xl font-bold text-gray-900 dark:text-white mb-2">
              Llogaria u krijua!
            </h2>
            <p className="text-gray-500 dark:text-gray-400 mb-6">
              Mirë se erdhët në KidsProject.
            </p>
            <Link
              href="/login"
              className="inline-block w-full py-3 px-6 rounded-xl bg-red-600 text-white font-semibold hover:bg-red-700 transition-colors text-center"
            >
              Hyr tani
            </Link>
          </div>
        ) : (
          <form
            onSubmit={handleSubmit(onSubmit)}
            className="bg-white dark:bg-gray-900 rounded-2xl border border-gray-200 dark:border-gray-800 p-8 shadow-sm space-y-5"
          >
            {/* Name */}
            <div>
              <label
                htmlFor="name"
                className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5"
              >
                Emri i plotë
              </label>
              <input
                id="name"
                type="text"
                autoComplete="name"
                placeholder="p.sh. Ardit Krasniqi"
                {...register("name")}
                className="w-full px-4 py-3 rounded-xl border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-transparent transition"
              />
              {errors.name && (
                <p className="mt-1.5 text-sm text-red-500">
                  {errors.name.message}
                </p>
              )}
            </div>

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
              <label
                htmlFor="password"
                className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5"
              >
                Fjalëkalimi
              </label>
              <input
                id="password"
                type="password"
                autoComplete="new-password"
                placeholder="Minimum 8 karaktere"
                {...register("password")}
                className="w-full px-4 py-3 rounded-xl border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-transparent transition"
              />
              {errors.password && (
                <p className="mt-1.5 text-sm text-red-500">
                  {errors.password.message}
                </p>
              )}
            </div>

            {/* Confirm Password */}
            <div>
              <label
                htmlFor="confirmPassword"
                className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5"
              >
                Konfirmo fjalëkalimin
              </label>
              <input
                id="confirmPassword"
                type="password"
                autoComplete="new-password"
                placeholder="Përsërit fjalëkalimin"
                {...register("confirmPassword")}
                className="w-full px-4 py-3 rounded-xl border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-transparent transition"
              />
              {errors.confirmPassword && (
                <p className="mt-1.5 text-sm text-red-500">
                  {errors.confirmPassword.message}
                </p>
              )}
            </div>

            <button
              type="submit"
              disabled={isSubmitting}
              className="w-full py-3 px-6 rounded-xl bg-red-600 text-white font-semibold hover:bg-red-700 active:scale-95 disabled:opacity-60 disabled:cursor-not-allowed transition-all shadow-md shadow-red-100 dark:shadow-none"
            >
              {isSubmitting ? "Duke u regjistruar..." : "Regjistrohu falas"}
            </button>

            <p className="text-center text-sm text-gray-500 dark:text-gray-400">
              Keni llogari?{" "}
              <Link
                href="/login"
                className="font-medium text-red-600 dark:text-red-400 hover:underline"
              >
                Hyni këtu
              </Link>
            </p>
          </form>
        )}
      </div>
    </div>
  );
}
