FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for restore (include referenced .csproj files)
COPY ["LedgerFlow/LedgerFlow.API/LedgerFlow.API.csproj", "LedgerFlow.API/"]
COPY ["LedgerFlow/LedgerFlow.Infrastructure/LedgerFlow.Infrastructure.csproj", "LedgerFlow.Infrastructure/"]
COPY ["LedgerFlow/LedgerFlow.Domain/LedgerFlow.Domain.csproj", "LedgerFlow.Domain/"]

RUN dotnet restore "LedgerFlow.API/LedgerFlow.API.csproj"

# Copy project source folders so Program.cs and other sources are in the correct locations
COPY ["LedgerFlow/LedgerFlow.API/", "LedgerFlow.API/"]
COPY ["LedgerFlow/LedgerFlow.Infrastructure/", "LedgerFlow.Infrastructure/"]
COPY ["LedgerFlow/LedgerFlow.Domain/", "LedgerFlow.Domain/"]

WORKDIR "/src/LedgerFlow.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LedgerFlow.API.dll"]
