FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /build

COPY *.sln .
COPY ./src/MindNote.API/MindNote.API.csproj ./src/MindNote.API/MindNote.API.csproj
COPY ./src/MindNote.Data/MindNote.Data.csproj ./src/MindNote.Data/MindNote.Data.csproj
COPY ./src/MindNote.Data.Providers.SqlServer/MindNote.Data.Providers.SqlServer.csproj ./src/MindNote.Data.Providers.SqlServer/MindNote.Data.Providers.SqlServer.csproj

RUN dotnet restore

COPY ./src ./src

RUN cd ./src/MindNote.API && dotnet publish -c Release -o /build/out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /build/out ./

EXPOSE 80/tcp

ENTRYPOINT sleep 5 && dotnet MindNote.API.dll