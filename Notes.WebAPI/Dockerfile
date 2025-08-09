#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /app
#COPY . .
#
#WORKDIR /app/Notes.WebApi
#RUN ls -la
#RUN dotnet publish -c Release -o /app/publish
#


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем sln и все проекты
COPY ["Notes.Backend.sln", "."]
COPY ["Notes.WebAPI/Notes.WebAPI.csproj", "Notes.WebAPI/"]
COPY ["Notes.Application/Notes.Domain.csproj", "Notes.Domain/"]
COPY ["Notes.Application/Notes.Application.csproj", "Notes.Application/"]
COPY ["Notes.Persistence/Notes.Persistence.csproj", "Notes.Persistence/"]

# Восстанавливаем зависимости
RUN dotnet restore "Notes.Backend.sln"

# Копируем всё остальное
COPY . .

# Сборка
RUN dotnet build "Notes.WebAPI/Notes.WebAPI.csproj" -c Release -o /app/build

# Публикация
FROM build AS publish
RUN dotnet publish "Notes.WebAPI/Notes.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Запуск
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notes.WebAPI.dll"]



#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#WORKDIR /app
#COPY --from=build /app/publish .
#ENTRYPOINT ["dotnet", "Notes.WebAPI.dll"]
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["Notes.WebAPI/Notes.WebAPI.csproj", "src/Notes.WebAPI/"]
#RUN dotnet restore "Notes.WebAPI/Notes.WebAPI.csproj"
#COPY . .
#WORKDIR "/src/src/Notes.WebAPI"
#RUN dotnet build "./Notes.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./Notes.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Notes.WebAPI.dll"]