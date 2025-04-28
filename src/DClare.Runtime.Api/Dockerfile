FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DClare.Runtime.Api/DClare.Runtime.Api.csproj", "src/DClare.Runtime.Api/"]
COPY ["src/DClare.Runtime.Application/DClare.Runtime.Application.csproj", "src/DClare.Runtime.Application/"]
COPY ["src/DClare.Runtime.Integration/DClare.Runtime.Integration.csproj", "src/DClare.Runtime.Integration/"]
RUN dotnet restore "./src/DClare.Runtime.Api/DClare.Runtime.Api.csproj"
COPY . .
WORKDIR "/src/src/DClare.Runtime.Api"
RUN dotnet build "./DClare.Runtime.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DClare.Runtime.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DClare.Runtime.Api.dll"]