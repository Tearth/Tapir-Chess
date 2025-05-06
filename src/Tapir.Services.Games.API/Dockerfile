FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/src/Tapir.Core/Tapir.Core.csproj", "./Tapir.Core/"]
COPY ["/src/Tapir.Services.Games.API/Tapir.Services.Games.API.csproj", "./Tapir.Services.Games.API/"]
COPY ["/src/Tapir.Services.Games.Application/Tapir.Services.Games.Application.csproj", "./Tapir.Services.Games.Application/"]
COPY ["/src/Tapir.Services.Games.Domain/Tapir.Services.Games.Domain.csproj", "./Tapir.Services.Games.Domain/"]
COPY ["/src/Tapir.Services.Games.Infrastructure/Tapir.Services.Games.Infrastructure.csproj", "./Tapir.Services.Games.Infrastructure/"]
COPY ["/src/Tapir.Providers.Database.PostgreSQL/Tapir.Providers.Database.PostgreSQL.csproj", "./Tapir.Providers.Database.PostgreSQL/"]
COPY ["/src/Tapir.Providers.EventStore.MongoDB/Tapir.Providers.EventStore.MongoDB.csproj", "./Tapir.Providers.EventStore.MongoDB/"]
COPY ["/src/Tapir.Providers.Scheduler.Quartz/Tapir.Providers.Scheduler.Quartz.csproj", "./Tapir.Providers.Scheduler.Quartz/"]
RUN dotnet restore "./Tapir.Services.Games.API/Tapir.Services.Games.API.csproj"
WORKDIR /
COPY . .
WORKDIR "./src/Tapir.Services.Games.API/"
RUN dotnet build "./Tapir.Services.Games.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tapir.Services.Games.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tapir.Services.Games.API.dll"]