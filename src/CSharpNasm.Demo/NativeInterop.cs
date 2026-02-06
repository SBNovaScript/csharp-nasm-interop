// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using System.Runtime.InteropServices;

namespace CSharpNasm.Demo;

internal static partial class NativeInterop
{
    private const string LibName = "libinterop";

    [LibraryImport(LibName, EntryPoint = "asm_add")]
    internal static partial long Add(long a, long b);

    [LibraryImport(LibName, EntryPoint = "asm_factorial")]
    internal static partial long Factorial(long n);

    [LibraryImport(LibName, EntryPoint = "asm_sum_array")]
    internal static partial long SumArray(IntPtr array, int length);

    [LibraryImport(LibName, EntryPoint = "asm_transform_array")]
    internal static partial void TransformArray(IntPtr array, int length, IntPtr callback);

    [LibraryImport(LibName, EntryPoint = "asm_apply_binary_op")]
    internal static partial long ApplyBinaryOp(long a, long b, IntPtr callback);

    [LibraryImport(LibName, EntryPoint = "asm_string_length")]
    internal static partial long StringLength(IntPtr str);

    [LibraryImport(LibName, EntryPoint = "asm_string_to_upper")]
    internal static partial void StringToUpper(IntPtr src, IntPtr dst, long length);

    [LibraryImport(LibName, EntryPoint = "asm_fibonacci_callback")]
    internal static partial void FibonacciCallback(int count, IntPtr callback);

    [LibraryImport(LibName, EntryPoint = "asm_register_and_invoke")]
    internal static partial long RegisterAndInvoke(IntPtr callback, long value);
}
