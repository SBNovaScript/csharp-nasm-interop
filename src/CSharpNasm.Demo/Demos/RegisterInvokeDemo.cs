// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using System.Runtime.InteropServices;

namespace CSharpNasm.Demo.Demos;

internal static class RegisterInvokeDemo
{
    internal static void Run()
    {
        Console.WriteLine("=== NASM -> C#: Register and Invoke ===\n");

        CallbackDelegates.UnaryOp square = x => x * x;
        CallbackDelegates.UnaryOp negate = x => -x;
        CallbackDelegates.UnaryOp cube = x => x * x * x;

        (string name, CallbackDelegates.UnaryOp op, long input)[] cases =
        [
            ("square", square, 9),
            ("negate", negate, 42),
            ("cube",   cube,   5),
        ];

        foreach (var (name, op, input) in cases)
        {
            var handle = GCHandle.Alloc(op);
            try
            {
                IntPtr fnPtr = Marshal.GetFunctionPointerForDelegate(op);
                long result = NativeInterop.RegisterAndInvoke(fnPtr, input);
                Console.WriteLine($"  asm_register_and_invoke({name}, {input}) = {result}");
            }
            finally
            {
                handle.Free();
            }
        }

        Console.WriteLine();
    }
}
