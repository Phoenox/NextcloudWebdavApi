FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN apt update && apt install -y clang
COPY . ./
RUN dotnet restore
RUN dotnet publish -c $BUILD_CONFIGURATION -o out NextcloudWebdavApi/NextcloudWebdavApi.csproj

FROM scratch as final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /src/out .
ENTRYPOINT ["./NextcloudWebdavApi.dll"]
