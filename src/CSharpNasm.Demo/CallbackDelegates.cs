// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using System.Runtime.InteropServices;

namespace CSharpNasm.Demo;

internal static class CallbackDelegates
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long UnaryOp(long value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long BinaryOp(long a, long b);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FibonacciObserver(int index, long value);
}
