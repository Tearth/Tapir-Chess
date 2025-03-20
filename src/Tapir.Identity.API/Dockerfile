FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/src/Tapir.Core/Tapir.Core.csproj", "./Tapir.Core/"]
COPY ["/src/Tapir.Identity.API/Tapir.Identity.API.csproj", "./Tapir.Identity.API/"]
COPY ["/src/Tapir.Identity.Application/Tapir.Identity.Application.csproj", "./Tapir.Identity.Application/"]
COPY ["/src/Tapir.Identity.Infrastructure/Tapir.Identity.Infrastructure.csproj", "./Tapir.Identity.Infrastructure/"]
RUN dotnet restore "./Tapir.Identity.API/Tapir.Identity.API.csproj"
WORKDIR /
COPY . .
WORKDIR "./src/Tapir.Identity.API/"
RUN dotnet build "./Tapir.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tapir.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tapir.Identity.API.dll"]