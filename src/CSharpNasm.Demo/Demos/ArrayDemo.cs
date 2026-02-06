// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using System.Runtime.InteropServices;

namespace CSharpNasm.Demo.Demos;

internal static class ArrayDemo
{
    internal static void Run()
    {
        Console.WriteLine("=== C# -> NASM: Array Sum ===\n");

        long[] data = [10, 20, 30, 40, 50];

        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            IntPtr ptr = handle.AddrOfPinnedObject();
            long sum = NativeInterop.SumArray(ptr, data.Length);
            Console.WriteLine($"  Array: [{string.Join(", ", data)}]");
            Console.WriteLine($"  asm_sum_array() = {sum}");
        }
        finally
        {
            handle.Free();
        }

        Console.WriteLine();
        Console.WriteLine("=== NASM -> C#: Transform Array (each element doubled) ===\n");

        long[] transformData = [1, 2, 3, 4, 5, 6, 7, 8];
        Console.WriteLine($"  Before: [{string.Join(", ", transformData)}]");

        CallbackDelegates.UnaryOp doubleIt = value => value * 2;

        var arrayHandle = GCHandle.Alloc(transformData, GCHandleType.Pinned);
        var delegateHandle = GCHandle.Alloc(doubleIt);
        try
        {
            IntPtr arrayPtr = arrayHandle.AddrOfPinnedObject();
            IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(doubleIt);
            NativeInterop.TransformArray(arrayPtr, transformData.Length, callbackPtr);
            Console.WriteLine($"  After:  [{string.Join(", ", transformData)}]");
        }
        finally
        {
            delegateHandle.Free();
            arrayHandle.Free();
        }

        Console.WriteLine();
    }
}
