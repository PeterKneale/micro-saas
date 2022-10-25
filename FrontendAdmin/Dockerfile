FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FrontendAdmin/FrontendAdmin.csproj", "FrontendAdmin/"]
RUN dotnet restore "FrontendAdmin/FrontendAdmin.csproj"
COPY . .
WORKDIR "/src/FrontendAdmin"
RUN dotnet build "FrontendAdmin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrontendAdmin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontendAdmin.dll"]
