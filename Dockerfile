# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем весь проект
COPY . .

# Путь к csproj относительно build context
RUN dotnet publish "Back-end Hospital/Hospital_System/WebApi/WebApi.csproj" -c Release -o /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render использует переменную PORT
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE 10000

ENTRYPOINT ["dotnet", "WebApi.dll"]