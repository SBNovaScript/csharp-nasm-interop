// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using System.Runtime.InteropServices;
using System.Text;

namespace CSharpNasm.Demo.Demos;

internal static class StringDemo
{
    internal static void Run()
    {
        Console.WriteLine("=== C# -> NASM: String Operations ===\n");

        string input = "Hello from C# to NASM!";
        byte[] utf8 = Encoding.UTF8.GetBytes(input + '\0');

        var srcHandle = GCHandle.Alloc(utf8, GCHandleType.Pinned);
        try
        {
            IntPtr srcPtr = srcHandle.AddrOfPinnedObject();

            long len = NativeInterop.StringLength(srcPtr);
            Console.WriteLine($"  Input:  \"{input}\"");
            Console.WriteLine($"  asm_string_length() = {len}");

            byte[] dstBuffer = new byte[len + 1];
            var dstHandle = GCHandle.Alloc(dstBuffer, GCHandleType.Pinned);
            try
            {
                IntPtr dstPtr = dstHandle.AddrOfPinnedObject();
                NativeInterop.StringToUpper(srcPtr, dstPtr, len);
                string upper = Encoding.UTF8.GetString(dstBuffer, 0, (int)len);
                Console.WriteLine($"  asm_string_to_upper() = \"{upper}\"");
            }
            finally
            {
                dstHandle.Free();
            }
        }
        finally
        {
            srcHandle.Free();
        }

        Console.WriteLine();
    }
}
