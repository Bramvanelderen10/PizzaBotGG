FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /source

COPY *.sln .
COPY PizzaBotGG.App/*.csproj ./PizzaBotGG.App/

RUN dotnet restore -r linux-x64

COPY PizzaBotGG.App/. ./PizzaBotGG.App/
WORKDIR /source/PizzaBotGG.App
RUN rm -rf ./bin
RUN rm -rf ./obj
RUN dotnet clean
RUN dotnet build -r linux-x64
RUN dotnet publish -c release -o /app -r linux-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-focal-amd64
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["./PizzaBotGG.App"]
