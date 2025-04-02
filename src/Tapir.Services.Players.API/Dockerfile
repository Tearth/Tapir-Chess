FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/src/Tapir.Core/Tapir.Core.csproj", "./Tapir.Core/"]
COPY ["/src/Tapir.Services.Players.API/Tapir.Services.Players.API.csproj", "./Tapir.Services.Players.API/"]
COPY ["/src/Tapir.Services.Players.Application/Tapir.Services.Players.Application.csproj", "./Tapir.Services.Players.Application/"]
COPY ["/src/Tapir.Services.Players.Domain/Tapir.Services.Players.Domain.csproj", "./Tapir.Services.Players.Domain/"]
COPY ["/src/Tapir.Services.Players.Infrastructure/Tapir.Services.Players.Infrastructure.csproj", "./Tapir.Services.Players.Infrastructure/"]
COPY ["/src/Tapir.Providers.Database.PostgreSQL/Tapir.Providers.Database.PostgreSQL.csproj", "./Tapir.Providers.Database.PostgreSQL/"]
COPY ["/src/Tapir.Providers.EventStore.MongoDB/Tapir.Providers.EventStore.MongoDB.csproj", "./Tapir.Providers.EventStore.MongoDB/"]
COPY ["/src/Tapir.Providers.Scheduler.Quartz/Tapir.Providers.Scheduler.Quartz.csproj", "./Tapir.Providers.Scheduler.Quartz/"]
RUN dotnet restore "./Tapir.Services.Players.API/Tapir.Services.Players.API.csproj"
WORKDIR /
COPY . .
WORKDIR "./src/Tapir.Services.Players.API"
RUN dotnet build "./Tapir.Services.Players.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tapir.Services.Players.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tapir.Services.Players.API.dll"]