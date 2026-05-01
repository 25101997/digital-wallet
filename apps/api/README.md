api$ docker run -it --rm -v "$PWD":/api -u $(id -u):$(id -g) -w /api -e DOTNET_CLI_HOME=/api -p 8080:8080 mcr.microsoft.com/dotnet/sdk:8.0 /bin/bash

dotnet new webapi --output .
dotnet clean
dotnet restore
dotnet build
dotnet run --urls "http://0.0.0.0:8080"