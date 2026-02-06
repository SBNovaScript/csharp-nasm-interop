// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using System.Runtime.InteropServices;

namespace CSharpNasm.Demo.Demos;

internal static class CallbackDemo
{
    internal static void Run()
    {
        Console.WriteLine("=== NASM -> C#: Binary Operation Callbacks ===\n");

        long a = 100, b = 7;

        (string name, CallbackDelegates.BinaryOp op)[] operations =
        [
            ("add",      (x, y) => x + y),
            ("subtract", (x, y) => x - y),
            ("multiply", (x, y) => x * y),
            ("divide",   (x, y) => y != 0 ? x / y : 0),
            ("modulo",   (x, y) => y != 0 ? x % y : 0),
            ("max",      Math.Max),
        ];

        foreach (var (name, op) in operations)
        {
            var handle = GCHandle.Alloc(op);
            try
            {
                IntPtr fnPtr = Marshal.GetFunctionPointerForDelegate(op);
                long result = NativeInterop.ApplyBinaryOp(a, b, fnPtr);
                Console.WriteLine($"  asm_apply_binary_op({a}, {b}, {name}) = {result}");
            }
            finally
            {
                handle.Free();
            }
        }

        Console.WriteLine();
    }
}
