FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/src/Tapir.Core/Tapir.Core.csproj", "./Tapir.Core/"]
COPY ["/src/Tapir.Services.News.API/Tapir.Services.News.API.csproj", "./Tapir.Services.News.API/"]
COPY ["/src/Tapir.Services.News.Application/Tapir.Services.News.Application.csproj", "./Tapir.Services.News.Application/"]
COPY ["/src/Tapir.Services.News.Domain/Tapir.Services.News.Domain.csproj", "./Tapir.Services.News.Domain/"]
COPY ["/src/Tapir.Services.News.Infrastructure/Tapir.Services.News.Infrastructure.csproj", "./Tapir.Services.News.Infrastructure/"]
COPY ["/src/Tapir.Providers.Database.PostgreSQL/Tapir.Providers.Database.PostgreSQL.csproj", "./Tapir.Providers.Database.PostgreSQL/"]
COPY ["/src/Tapir.Providers.EventStore.MongoDB/Tapir.Providers.EventStore.MongoDB.csproj", "./Tapir.Providers.EventStore.MongoDB/"]
COPY ["/src/Tapir.Providers.Scheduler.Quartz/Tapir.Providers.Scheduler.Quartz.csproj", "./Tapir.Providers.Scheduler.Quartz/"]
RUN dotnet restore "./Tapir.Services.News.API/Tapir.Services.News.API.csproj"
WORKDIR /
COPY . .
WORKDIR "./src/Tapir.Services.News.API/"
RUN dotnet build "./Tapir.Services.News.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tapir.Services.News.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tapir.Services.News.API.dll"]