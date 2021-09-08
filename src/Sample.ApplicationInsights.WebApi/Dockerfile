FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Sample.ApplicationInsights.WebApi/Sample.ApplicationInsights.WebApi.csproj", "src/Sample.ApplicationInsights.WebApi/"]
COPY ["src/Sample.ApplicationInsights.WebApi.Core/Sample.ApplicationInsights.WebApi.Core.csproj", "src/Sample.ApplicationInsights.WebApi.Core/"]
RUN dotnet restore "src/Sample.ApplicationInsights.WebApi/Sample.ApplicationInsights.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Sample.ApplicationInsights.WebApi"
RUN dotnet build "Sample.ApplicationInsights.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.ApplicationInsights.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.ApplicationInsights.WebApi.dll"]