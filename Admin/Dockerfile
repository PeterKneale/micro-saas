FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Admin/Admin.csproj", "Admin/"]
COPY ["Client/Client.csproj", "Client/"]
RUN dotnet restore "Admin/Admin.csproj"
COPY . .
WORKDIR "/src/Admin"
RUN dotnet build "Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Admin.dll"]
