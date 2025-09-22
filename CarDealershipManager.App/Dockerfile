# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CarDealershipManager.App/CarDealershipManager.App.csproj", "CarDealershipManager.App/"]
COPY ["CarDealershipManager.Infrastructure/CarDealershipManager.Infrastructure.csproj", "CarDealershipManager.Infrastructure/"]
COPY ["CarDealershipManager.Core/CarDealershipManager.Core.csproj", "CarDealershipManager.Core/"]

RUN dotnet restore "./CarDealershipManager.App/CarDealershipManager.App.csproj"

COPY . .

WORKDIR "/src/CarDealershipManager.App"

RUN dotnet build "./CarDealershipManager.App.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "./CarDealershipManager.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "CarDealershipManager.App.dll"]