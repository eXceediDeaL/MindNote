FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /build

COPY *.sln .
COPY ./src ./src
COPY ./test ./test

RUN dotnet restore

RUN cd ./src/Server/MindNote.Server.Identity && dotnet publish -c Release -o /build

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /build ./
COPY ./docker/wait-for-it.sh ./
RUN chmod +x ./wait-for-it.sh

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "MindNote.Server.Identity.dll"]