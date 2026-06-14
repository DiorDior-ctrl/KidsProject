FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ProgressService.API/ProgressService.API.csproj", "ProgressService.API/"]
COPY ["ProgressService.Application/ProgressService.Application.csproj", "ProgressService.Application/"]
COPY ["ProgressService.Domain/ProgressService.Domain.csproj", "ProgressService.Domain/"]
COPY ["ProgressService.Infrastructure/ProgressService.Infrastructure.csproj", "ProgressService.Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "ProgressService.API/ProgressService.API.csproj"

COPY ProgressService.API/ ProgressService.API/
COPY ProgressService.Application/ ProgressService.Application/
COPY ProgressService.Domain/ ProgressService.Domain/
COPY ProgressService.Infrastructure/ ProgressService.Infrastructure/
COPY SharedKernel/ SharedKernel/

WORKDIR "/src/ProgressService.API"
RUN dotnet build "ProgressService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProgressService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProgressService.API.dll"]