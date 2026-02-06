.PHONY: all build run clean

all: build

build:
	dotnet build

run: build
	dotnet run --project src/CSharpNasm.Demo

clean:
	dotnet clean
	$(MAKE) -C asm clean
