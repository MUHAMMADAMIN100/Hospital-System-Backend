# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 10000

# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Сначала копируем только проект для быстрого restore
COPY "Back-end Hospital/Hospital_System/WebApi/WebApi.csproj" ./WebApi.csproj
RUN dotnet restore ./WebApi.csproj

# Копируем весь код
COPY "Back-end Hospital/Hospital_System/WebApi" ./WebApi
WORKDIR /src/WebApi

# Публикуем
RUN dotnet publish WebApi.csproj -c Release -o /app/publish

# ---------- Final stage ----------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "WebApi.dll"]