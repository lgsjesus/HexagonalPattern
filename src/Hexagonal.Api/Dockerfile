FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Hexagonal.Api/Hexagonal.Api.csproj", "./Hexagonal.Api/"]
COPY ["./Hexagonal.Domain/Hexagonal.Domain.csproj", "./Hexagonal.Domain/"]
COPY ["./Hexagonal.Services/Hexagonal.Services.csproj", "./Hexagonal.Services/"]
COPY ["./Hexagonal.EntityFramework/Hexagonal.EntityFramework.csproj", "./Hexagonal.EntityFramework/"]

RUN dotnet restore "./Hexagonal.Api/Hexagonal.Api.csproj"
COPY . .
WORKDIR "./Hexagonal.Api"
RUN dotnet build "Hexagonal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Hexagonal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hexagonal.Api.dll"]
