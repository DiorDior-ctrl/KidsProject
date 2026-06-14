FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["LessonService/LessonService.API.csproj", "LessonService/"]
COPY ["LessonService.Application/LessonService.Application.csproj", "LessonService.Application/"]
COPY ["LessonService.Domain/LessonService.Domain.csproj", "LessonService.Domain/"]
COPY ["LessonService.Infrastructure/LessonService.Infrastructure.csproj", "LessonService.Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "LessonService/LessonService.API.csproj"

COPY LessonService/ LessonService/
COPY LessonService.Application/ LessonService.Application/
COPY LessonService.Domain/ LessonService.Domain/
COPY LessonService.Infrastructure/ LessonService.Infrastructure/
COPY SharedKernel/ SharedKernel/

WORKDIR "/src/LessonService"
RUN dotnet build "LessonService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LessonService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENTRYPOINT ["dotnet", "LessonService.API.dll"]