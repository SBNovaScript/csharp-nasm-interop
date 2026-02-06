# CSharpNasm

Bidirectional interop between C# (.NET 10) and NASM x86-64 assembly on Linux.

## Motivation

Managed runtimes like .NET provide safety, garbage collection, and productivity, but
sometimes it is necessary to compute lower-level operations. SIMD-heavy transforms, cycle-counted hot loops,
or direct hardware interaction are all cases where hand-written assembly is favorable.

Going the other direction is equally useful: assembly routines that need to report
progress, log events, or evaluate dynamic logic can call back into managed code through
function pointers can do so through C#.

This project demonstrates both directions end-to-end with no C shim layer — C# talks
directly to NASM-assembled shared libraries via P/Invoke, and NASM calls managed delegates
through raw function pointers using the System V AMD64 calling convention.

## What it shows

### C# calling into NASM

| Function | Description |
|---|---|
| `asm_add` | Integer addition |
| `asm_factorial` | Iterative factorial |
| `asm_sum_array` | Sum a pinned `long[]` |
| `asm_string_length` | Byte-level string length |
| `asm_string_to_upper` | ASCII lowercase to uppercase |

### NASM calling back into C#

| Function | Description |
|---|---|
| `asm_transform_array` | Applies a C# `Func<long, long>` to each array element in-place |
| `asm_apply_binary_op` | Passes two operands to a C# binary delegate and returns the result |
| `asm_fibonacci_callback` | Generates Fibonacci numbers, calling a C# observer for each |
| `asm_register_and_invoke` | Receives a function pointer and immediately invokes it |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [NASM](https://nasm.us/) (2.15+)
- GCC (for linking the shared library)
- Linux x86-64

```
sudo apt install nasm gcc    # Debian/Ubuntu
```

## Build and run

```
dotnet build
dotnet run --project src/CSharpNasm.Demo
```

`dotnet build` automatically assembles the NASM source and produces `libinterop.so` via
an MSBuild target — no separate step needed. A top-level `Makefile` is also provided:

```
make run
```

## How it works

### C# to NASM

`NativeInterop.cs` declares P/Invoke bindings using the source-generated `[LibraryImport]`
attribute. The .NET runtime loads `libinterop.so` and calls directly into the exported
NASM symbols. Arrays are pinned with `GCHandle.Alloc(..., GCHandleType.Pinned)` so the
GC cannot relocate them during the native call.

### NASM to C#

C# creates a delegate (e.g. `CallbackDelegates.UnaryOp`), converts it to a function
pointer with `Marshal.GetFunctionPointerForDelegate`, and passes the raw `IntPtr` to NASM.
The assembly treats it as a standard SysV function pointer and issues a `call` through the
register. A `GCHandle` keeps the delegate alive for the duration of the native call.

### Build pipeline

```
nasm -f elf64 -g -F dwarf  ->  .o  ->  gcc -shared  ->  libinterop.so
                                                              |
                     MSBuild CopyNativeLib target  ->  bin/Debug/net10.0/
```

## Project structure

```
.
├── asm/
│   ├── interop.asm          # All NASM routines
│   └── Makefile             # nasm + gcc -> libinterop.so
├── src/CSharpNasm.Demo/
│   ├── NativeInterop.cs     # P/Invoke declarations
│   ├── CallbackDelegates.cs # Delegate types for NASM -> C# callbacks
│   ├── Program.cs           # Entry point
│   └── Demos/               # One file per demo scenario
├── Makefile                 # Top-level convenience targets
└── CSharpNasm.slnx          # .NET solution
```
