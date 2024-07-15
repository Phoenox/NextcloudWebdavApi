FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN apt update && apt install -y clang zlib1g-dev
COPY . ./
RUN dotnet restore
RUN dotnet publish -c $BUILD_CONFIGURATION -o out NextcloudWebdavApi/NextcloudWebdavApi.csproj

FROM scratch AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /src/out/appsettings.json .
COPY --from=build /src/out/NextcloudWebdavApi .
ENTRYPOINT ["./NextcloudWebdavApi"]
