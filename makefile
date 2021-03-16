clean:
	rm -rf CLI_Sharp/obj CLI_Sharp/bin exampleApp/obj exampleApp/bin

restore:
	dotnet restore

example:
	cd exampleApp/
	make restore
	dotnet build
	exampleApp/bin/Debug/net5.0/exampleApp

library:
	cd CLI_Sharp/
	make restore
	dotnet build -c release
	cp CLI_Sharp/bin/Release/net5.0/CLI_Sharp.dll .
	clear && echo "Compiled dll file is in your current directory!"
