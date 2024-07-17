FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

RUN apk add clang binutils musl-dev build-base zlib-static openssl-dev
COPY . ./
RUN dotnet publish -c Release -o out -r linux-musl-x64 NextcloudWebdavApi/NextcloudWebdavApi.csproj

FROM scratch AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /src/out/NextcloudWebdavApi .
ENTRYPOINT ["/app/NextcloudWebdavApi"]
