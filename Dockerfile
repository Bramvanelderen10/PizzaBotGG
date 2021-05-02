FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

WORKDIR /source

COPY src/*.sln .
COPY src/PizzaBotGG.App/*.csproj ./PizzaBotGG.App/

COPY src/PizzaBotGG.App/. ./PizzaBotGG.App/
WORKDIR /source/PizzaBotGG.App
RUN rm -rf ./bin
RUN rm -rf ./obj
RUN dotnet clean
RUN dotnet build -r linux-x64
RUN dotnet publish -c release -o /app -r linux-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-focal-amd64
WORKDIR /app
COPY --from=build /app ./
COPY start-bot.sh ./start-bot.sh

COPY tools/Lavalink/Lavalink.jar Lavalink.jar
COPY tools/Lavalink/application.yml application.yml

RUN apt-get update \
    && apt-get install -y default-jre

ENTRYPOINT ["./start-bot.sh"]
