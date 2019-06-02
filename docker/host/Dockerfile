FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /build

COPY *.sln .
COPY ./src/MindNote.Server.Host/MindNote.Server.Host.csproj ./src/MindNote.Server.Host/MindNote.Server.Host.csproj
COPY ./src/MindNote.Server.API/MindNote.Server.API.csproj ./src/MindNote.Server.API/MindNote.Server.API.csproj
COPY ./src/MindNote.Server.Identity/MindNote.Server.Identity.csproj ./src/MindNote.Server.Identity/MindNote.Server.Identity.csproj
COPY ./src/MindNote.Data/MindNote.Data.csproj ./src/MindNote.Data/MindNote.Data.csproj
COPY ./src/MindNote.Client.API/MindNote.Client.API.csproj ./src/MindNote.Client.API/MindNote.Client.API.csproj
COPY ./src/MindNote.Data.Providers.SqlServer/MindNote.Data.Providers.SqlServer.csproj ./src/MindNote.Data.Providers.SqlServer/MindNote.Data.Providers.SqlServer.csproj
COPY ./test/Test.Unit/Test.Unit.csproj ./test/Test.Unit/Test.Unit.csproj

RUN dotnet restore

COPY ./src ./src

RUN cd ./src/MindNote.Server.Host && dotnet publish -c Release -o /build/out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /build/out ./

EXPOSE 80/tcp

ENTRYPOINT sleep 40 && dotnet MindNote.Server.Host.dll