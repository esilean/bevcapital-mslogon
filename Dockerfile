#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BevCapital.Logon.API/BevCapital.Logon.API.csproj", "BevCapital.Logon.API/"]
COPY ["BevCapital.Logon.Application/BevCapital.Logon.Application.csproj", "BevCapital.Logon.Application/"]
COPY ["BevCapital.Logon.Domain/BevCapital.Logon.Domain.csproj", "BevCapital.Logon.Domain/"]
COPY ["BevCapital.Logon.Domain.Core/BevCapital.Logon.Domain.Core.csproj", "BevCapital.Logon.Domain.Core/"]
COPY ["BevCapital.Logon.Data/BevCapital.Logon.Data.csproj", "BevCapital.Logon.Data/"]
COPY ["BevCapital.Logon.Infra/BevCapital.Logon.Infra.csproj", "BevCapital.Logon.Infra/"]
COPY ["BevCapital.Logon.Background/BevCapital.Logon.Background.csproj", "BevCapital.Logon.Background/"]


RUN dotnet restore "BevCapital.Logon.API/BevCapital.Logon.API.csproj"
COPY . .
WORKDIR "/src/BevCapital.Logon.API"
RUN dotnet build "BevCapital.Logon.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BevCapital.Logon.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BevCapital.Logon.API.dll"]