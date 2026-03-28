FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/1. Presentation/Nucleos.API/Nucleos.API.csproj", "src/1. Presentation/Nucleos.API/"]
COPY ["src/2. Application/Nucleos.Application/Nucleos.Application.csproj", "src/2. Application/Nucleos.Application/"]
COPY ["src/3. Domain/Nucleos.Domain/Nucleos.Domain.csproj", "src/3. Domain/Nucleos.Domain/"]
COPY ["src/4. Infrastructure/Nucleos.Infrastructure/Nucleos.Infrastructure.csproj", "src/4. Infrastructure/Nucleos.Infrastructure/"]

RUN dotnet restore "src/1. Presentation/Nucleos.API/Nucleos.API.csproj"

COPY . .

RUN dotnet build "src/1. Presentation/Nucleos.API/Nucleos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/1. Presentation/Nucleos.API/Nucleos.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nucleos.API.dll"]
