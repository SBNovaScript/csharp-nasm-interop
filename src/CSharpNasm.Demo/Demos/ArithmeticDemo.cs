// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

namespace CSharpNasm.Demo.Demos;

internal static class ArithmeticDemo
{
    internal static void Run()
    {
        Console.WriteLine("=== C# -> NASM: Arithmetic ===\n");

        long a = 42, b = 58;
        long sum = NativeInterop.Add(a, b);
        Console.WriteLine($"  asm_add({a}, {b}) = {sum}");

        for (long n = 0; n <= 12; n++)
        {
            long fact = NativeInterop.Factorial(n);
            Console.WriteLine($"  asm_factorial({n}) = {fact}");
        }

        Console.WriteLine();
    }
}
