// SPDX-License-Identifier: Apache-2.0
// Copyright (c) 2026 Steven Baumann (SBNovaScript)
// See LICENSE and NOTICE in the repository root for details.

using CSharpNasm.Demo.Demos;

Console.WriteLine("╔══════════════════════════════════════════════════════╗");
Console.WriteLine("║   Bidirectional C# <-> NASM x86-64 Interop Demo    ║");
Console.WriteLine("║   Target: Linux x86-64, SysV AMD64 ABI             ║");
Console.WriteLine("╚══════════════════════════════════════════════════════╝");
Console.WriteLine();

ArithmeticDemo.Run();
StringDemo.Run();
ArrayDemo.Run();
CallbackDemo.Run();
FibonacciDemo.Run();
RegisterInvokeDemo.Run();

Console.WriteLine("All demos completed successfully.");
