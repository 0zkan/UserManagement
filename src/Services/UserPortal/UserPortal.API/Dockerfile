FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src

COPY "UserManagement.sln" "UserManagement.sln"

COPY ["Services/UserPortal/UserPortal.API/UserPortal.API.csproj", "Services/UserPortal/UserPortal.API/"]
COPY ["Services/Management/Management.API/Management.API.csproj", "Services/Management/Management.API/"]
COPY ["Framework/Framework.csproj", "Framework/Framework.csproj"]

COPY "NuGet.config" "NuGet.config"

RUN dotnet pack Framework/Framework.csproj -o ./packages /p:Version=1.0.3
RUN dotnet restore "UserManagement.sln"

COPY . .

#WORKDIR /src
#RUN dotnet pack Framework/Framework.csproj -o ./packages /p:Version=1.0.3
#RUN dotnet nuget add source ./packages -n FW
#RUN dotnet restore "UserManagement.sln"

WORKDIR /src/Services/UserPortal/UserPortal.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish "UserPortal.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UserPortal.API.dll"]
