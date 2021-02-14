restore:
	dotnet restore

example:
	cd exampleApp/
	make restore
	dotnet build
	exampleApp/bin/Debug/netcoreapp3.1/exampleApp

library:
	cd CLI_Sharp/
	make restore
	dotnet build -c release
