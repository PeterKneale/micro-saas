FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FrontendSignup/FrontendSignup.csproj", "FrontendSignup/"]
RUN dotnet restore "FrontendSignup/FrontendSignup.csproj"
COPY . .
WORKDIR "/src/FrontendSignup"
RUN dotnet build "FrontendSignup.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrontendSignup.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontendSignup.dll"]
