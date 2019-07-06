FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /source

COPY *.sln .
COPY ./src ./src
COPY ./test ./test

RUN dotnet restore

WORKDIR /source/src/Client/Host

RUN cd ./MindNote.Client.Host.Server && dotnet publish -c Release -o /build \
     && cp -r /build/MindNote.Client.Host.Client/dist/* /build/wwwroot && rm -rf /build/MindNote.Client.Host.Client \
     && cp -r /build/wwwroot/_content/mindnoteclienthostclient/* /build/wwwroot && rm -rf /build/wwwroot/_content

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /build ./
COPY ./docker/wait-for-it.sh ./
RUN chmod +x ./wait-for-it.sh

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "MindNote.Client.Host.Server.dll"]