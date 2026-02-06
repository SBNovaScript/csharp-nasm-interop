// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using System.Runtime.InteropServices;

namespace CSharpNasm.Demo.Demos;

internal static class FibonacciDemo
{
    internal static void Run()
    {
        Console.WriteLine("=== NASM -> C#: Fibonacci Sequence via Callback ===\n");

        CallbackDelegates.FibonacciObserver observer = (index, value) =>
        {
            Console.WriteLine($"  fib({index}) = {value}");
        };

        var handle = GCHandle.Alloc(observer);
        try
        {
            IntPtr fnPtr = Marshal.GetFunctionPointerForDelegate(observer);
            NativeInterop.FibonacciCallback(15, fnPtr);
        }
        finally
        {
            handle.Free();
        }

        Console.WriteLine();
    }
}
