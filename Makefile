.PHONY: all run

all:
	rm -rf **/**/{obj,bin}
	dotnet run --project src/Console

check: 
	dotnet run --project src/Console -p:DefineConstants=check

run:
	dotnet run --project src/Console
