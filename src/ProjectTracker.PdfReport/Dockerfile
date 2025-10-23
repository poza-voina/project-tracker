# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ProjectTracker.PdfReport/ProjectTracker.PdfReport.csproj", "src/ProjectTracker.PdfReport/"]
COPY ["src/ProjectTracker.Abstractions/ProjectTracker.Abstractions.csproj", "src/ProjectTracker.Abstractions/"]
COPY ["src/ProjectTracker.Contracts/ProjectTracker.Contracts.csproj", "src/ProjectTracker.Contracts/"]
COPY ["src/ProjectTracker.Infrastructure/ProjectTracker.Infrastructure.csproj", "src/ProjectTracker.Infrastructure/"]
RUN dotnet restore "./src/ProjectTracker.PdfReport/ProjectTracker.PdfReport.csproj"
COPY . .
WORKDIR "/src/src/ProjectTracker.PdfReport"
RUN dotnet build "./ProjectTracker.PdfReport.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProjectTracker.PdfReport.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectTracker.PdfReport.dll"]