FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS runtime

WORKDIR /source

COPY *.sln .
COPY ./src ./src
COPY ./test ./test

RUN dotnet restore

WORKDIR /source/src/Frontend/Client

RUN cd ./MindNote.Frontend.Client.Server && dotnet build

RUN apk update \
        && apk upgrade \
        && apk add --no-cache bash \
        && rm -rf /var/cache/apk/*

COPY ./docker/wait-for-it.sh ./
RUN chmod +x ./wait-for-it.sh

EXPOSE 8150/tcp

ENTRYPOINT ["dotnet", "run", "-p", "./MindNote.Frontend.Client.Server"]