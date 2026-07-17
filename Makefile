CLI_PROJECT := src/LittleWoodTracker.Cli/LittleWoodTracker.Cli.csproj
PACKAGE_ID  := LittleWoodTracker.Cli
PACKAGE_SRC := src/LittleWoodTracker.Cli/bin/Release

.PHONY: install uninstall

install:
	dotnet pack $(CLI_PROJECT) -c Release
	dotnet tool install -g $(PACKAGE_ID) --add-source $(PACKAGE_SRC)

uninstall:
	dotnet tool uninstall -g $(PACKAGE_ID)
