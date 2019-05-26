FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /build

COPY *.sln .
COPY src ./src
RUN cd ./src/MindNote.API && dotnet publish -c Release -o ../../out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /build/out ./

ENTRYPOINT ["dotnet", "MindNote.API.dll"]