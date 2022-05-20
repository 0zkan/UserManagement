FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Management/Management.API/Management.API.csproj", "Services/Management/Management.API/"]
RUN dotnet restore "Services\Management\Management.API\Management.API.csproj"
COPY . .
WORKDIR "/src/Services/Management/Management.API"
RUN dotnet build "Management.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Management.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.API.dll"]
