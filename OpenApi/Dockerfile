FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OpenApi/OpenApi.csproj", "OpenApi/"]
RUN dotnet restore "OpenApi/OpenApi.csproj"
COPY . .
WORKDIR "/src/OpenApi"
RUN dotnet build "OpenApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenApi.dll"]