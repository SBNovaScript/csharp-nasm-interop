ASM_SRC  := interop.asm
OBJ      := $(ASM_SRC:.asm=.o)
LIB      := libinterop.so

NASM     := nasm
NASMFLAGS := -f elf64 -g -F dwarf
CC       := gcc

.PHONY: all clean

all: $(LIB)

$(LIB): $(OBJ)
	$(CC) -shared -o $@ $^

%.o: %.asm
	$(NASM) $(NASMFLAGS) -o $@ $<

clean:
	rm -f $(OBJ) $(LIB)
