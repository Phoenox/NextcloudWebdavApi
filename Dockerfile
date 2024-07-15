﻿FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . ./
RUN dotnet restore
RUN dotnet publish -c $BUILD_CONFIGURATION -o out

FROM scratch as final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /src/out .
ENTRYPOINT ["./NextcloudWebdavApi.dll"]
