FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5225

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

#COPY "UserManagement.sln" "UserManagement.sln"

#COPY "Services/UserPortal/UserPortal.API/UserPortal.API.csproj" "Services/UserPortal/UserPortal.API/"
COPY "Services/Management/Management.API/Management.API.csproj" "Services/Management/Management.API/"
COPY "Framework/Framework.csproj" "Framework/Framework.csproj"

COPY "NuGet.config" "NuGet.config"

RUN mkdir -p packages

WORKDIR /src/Framework
RUN dotnet restore "Framework.csproj"
RUN dotnet build --no-restore "Framework.csproj"
RUN dotnet pack Framework.csproj -o ../packages /p:Version=1.0.3

#WORKDIR /src
#RUN dotnet restore "UserManagement.sln" --configfile NuGet.config

WORKDIR /src
COPY . .

WORKDIR /src/Services/Management/Management.API
RUN dotnet restore "Management.API.csproj" --configfile ../../../NuGet.config
RUN dotnet publish "Management.API.csproj" --no-restore -c Release -o /app /p:UseAppHost=false

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=build /src/Services/Management/Management.API/Protos /app/Proto
ENTRYPOINT ["dotnet", "Management.API.dll"]
