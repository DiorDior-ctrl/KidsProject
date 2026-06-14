FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["GamificationService.API/GamificationService.API.csproj", "GamificationService.API/"]
COPY ["GamificationService.Application/GamificationService.Application.csproj", "GamificationService.Application/"]
COPY ["GamificationService.Domain/GamificationService.Domain.csproj", "GamificationService.Domain/"]
COPY ["GamificationService.Infrastructure/GamificationService.Infrastructure.csproj", "GamificationService.Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "GamificationService.API/GamificationService.API.csproj"

COPY GamificationService.API/ GamificationService.API/
COPY GamificationService.Application/ GamificationService.Application/
COPY GamificationService.Domain/ GamificationService.Domain/
COPY GamificationService.Infrastructure/ GamificationService.Infrastructure/
COPY SharedKernel/ SharedKernel/

WORKDIR "/src/GamificationService.API"
RUN dotnet build "GamificationService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GamificationService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENTRYPOINT ["dotnet", "GamificationService.API.dll"]