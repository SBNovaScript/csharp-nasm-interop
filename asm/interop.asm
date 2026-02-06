; SPDX-License-Identifier: Apache-2.0
; Copyright (c) 2026 Steven Baumann (SBNovaScript)
; See LICENSE and NOTICE in the repository root for details.

; interop.asm — Linux x86-64, ELF64, SysV AMD64 ABI
;
; Exported:
;   asm_add(long, long) -> long
;   asm_factorial(long) -> long
;   asm_sum_array(long*, int) -> long
;   asm_transform_array(long*, int, fn(long)->long) -> void
;   asm_apply_binary_op(long, long, fn(long,long)->long) -> long
;   asm_string_length(byte*) -> long
;   asm_string_to_upper(byte*, byte*, long) -> void
;   asm_fibonacci_callback(int, fn(int,long)->void) -> void
;   asm_register_and_invoke(fn(long)->long, long) -> long

bits 64
default rel

section .text

; --- asm_add(rdi=a, rsi=b) -> rax ---
global asm_add
asm_add:
    lea rax, [rdi + rsi]
    ret

; --- asm_factorial(rdi=n) -> rax ---
global asm_factorial
asm_factorial:
    mov  rax, 1
    cmp  rdi, 1
    jle  .done
.loop:
    imul rax, rdi
    dec  rdi
    cmp  rdi, 1
    jg   .loop
.done:
    ret

; --- asm_sum_array(rdi=arr, esi=len) -> rax ---
global asm_sum_array
asm_sum_array:
    xor  rax, rax
    movsxd rcx, esi
    test rcx, rcx
    jle  .sum_done
.sum_loop:
    add  rax, [rdi]
    add  rdi, 8
    dec  rcx
    jnz  .sum_loop
.sum_done:
    ret

; --- asm_transform_array(rdi=arr, esi=len, rdx=fn) -> void ---
; Calls fn(element) for each element and stores the result back.
global asm_transform_array
asm_transform_array:
    push rbp
    mov  rbp, rsp
    push rbx
    push r12
    push r13
    push r14
    ; 5 pushes (incl rbp) = 40 bytes; entry RSP%16==8 -> aligned

    mov  rbx, rdi           ; array base
    movsxd r12, esi         ; count
    mov  r13, rdx           ; callback
    xor  r14, r14           ; index

    test r12, r12
    jle  .transform_done

.transform_loop:
    mov  rdi, [rbx + r14*8]
    call r13
    mov  [rbx + r14*8], rax

    inc  r14
    cmp  r14, r12
    jl   .transform_loop

.transform_done:
    pop  r14
    pop  r13
    pop  r12
    pop  rbx
    pop  rbp
    ret

; --- asm_apply_binary_op(rdi=a, rsi=b, rdx=fn) -> rax ---
global asm_apply_binary_op
asm_apply_binary_op:
    push rbp
    mov  rbp, rsp

    mov  rax, rdx
    call rax                ; rdi, rsi already hold a, b

    pop  rbp
    ret

; --- asm_string_length(rdi=str) -> rax ---
global asm_string_length
asm_string_length:
    xor  rax, rax
    test rdi, rdi
    jz   .strlen_done
.strlen_loop:
    cmp  byte [rdi + rax], 0
    je   .strlen_done
    inc  rax
    jmp  .strlen_loop
.strlen_done:
    ret

; --- asm_string_to_upper(rdi=src, rsi=dst, rdx=len) -> void ---
global asm_string_to_upper
asm_string_to_upper:
    xor  rcx, rcx
    test rdx, rdx
    jle  .upper_null
.upper_loop:
    movzx eax, byte [rdi + rcx]
    cmp  al, 'a'
    jb   .upper_store
    cmp  al, 'z'
    ja   .upper_store
    sub  al, 32
.upper_store:
    mov  [rsi + rcx], al
    inc  rcx
    cmp  rcx, rdx
    jl   .upper_loop
.upper_null:
    mov  byte [rsi + rcx], 0
    ret

; --- asm_fibonacci_callback(edi=count, rsi=fn) -> void ---
; Generates count fibonacci numbers, calling fn(index, value) for each.
global asm_fibonacci_callback
asm_fibonacci_callback:
    push rbp
    mov  rbp, rsp
    push rbx
    push r12
    push r13
    push r14
    push r15
    sub  rsp, 8             ; 6 pushes (incl rbp) = 48 bytes; need 8 more for 16-align

    movsxd r12, edi         ; count
    mov  r13, rsi           ; callback
    xor  r14, r14           ; fib(n-2) = 0
    mov  r15, 1             ; fib(n-1) = 1
    xor  ebx, ebx           ; index = 0

    test r12, r12
    jle  .fib_done

.fib_loop:
    mov  edi, ebx
    mov  rsi, r14
    call r13                ; callback(index, fib_value)

    mov  rax, r14
    add  rax, r15
    mov  r14, r15
    mov  r15, rax

    inc  ebx
    cmp  ebx, r12d
    jl   .fib_loop

.fib_done:
    add  rsp, 8
    pop  r15
    pop  r14
    pop  r13
    pop  r12
    pop  rbx
    pop  rbp
    ret

; --- asm_register_and_invoke(rdi=fn, rsi=value) -> rax ---
global asm_register_and_invoke
asm_register_and_invoke:
    push rbp
    mov  rbp, rsp

    mov  rax, rdi
    mov  rdi, rsi
    call rax

    pop  rbp
    ret

section .note.GNU-stack noalloc noexec nowrite progbits
