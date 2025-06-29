FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY TMS.API/*.csproj ./TMS.API/
COPY TMS.Abstractions/*.csproj ./TMS.Abstractions/
COPY TMS.Application/*.csproj ./TMS.Application/
COPY TMS.Infrastructure/*.csproj ./TMS.Infrastructure/
COPY TMS.sln ./
RUN dotnet restore

COPY . ./

# Публикуем только API
WORKDIR /app/TMS.API
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/TMS.API/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "TMS.API.dll"]