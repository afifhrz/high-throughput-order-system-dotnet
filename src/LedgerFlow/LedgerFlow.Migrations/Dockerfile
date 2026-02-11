FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj(s) and restore
COPY ["LedgerFlow/LedgerFlow.Migrations/LedgerFlow.Migrations.csproj", "LedgerFlow/LedgerFlow.Migrations/"]
COPY ["LedgerFlow/LedgerFlow.Infrastructure/LedgerFlow.Infrastructure.csproj", "LedgerFlow/LedgerFlow.Infrastructure/"]
RUN dotnet restore "LedgerFlow/LedgerFlow.Migrations/LedgerFlow.Migrations.csproj"

# copy everything and publish
COPY . .
WORKDIR "/src/LedgerFlow/LedgerFlow.Migrations"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LedgerFlow.Migrations.dll"]