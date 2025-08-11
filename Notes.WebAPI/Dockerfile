FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["./*", "/src/"]
#COPY ["Notes.Application/*", "Notes.Domain/"]
#COPY ["Notes.Application/*", "Notes.Application/"]
#COPY ["Notes.Persistence/*", "Notes.Persistence/"]

# RUN dotnet restore -s "/src/Notes.Backend.sln"

COPY . .

RUN dotnet build "Notes.WebAPI/Notes.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notes.WebAPI/Notes.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notes.WebAPI.dll"]