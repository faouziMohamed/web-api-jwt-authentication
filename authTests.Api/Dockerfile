FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["authTests.Api/authTests.Api.csproj", "authTests.Api/"]
RUN dotnet restore "authTests.Api/authTests.Api.csproj"
COPY . .
WORKDIR "/src/authTests.Api"
RUN dotnet build "authTests.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "authTests.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "authTests.Api.dll"]
