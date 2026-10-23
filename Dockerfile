FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env

COPY . app/
WORKDIR /app

RUN dotnet restore src/ConsiliumTempus.Api
RUN dotnet publish src/ConsiliumTempus.Api -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime-env

WORKDIR /app
COPY --from=build-env /out .

ENTRYPOINT ["dotnet", "ConsiliumTempus.Api.dll"]
