FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

COPY . app/
WORKDIR /app

RUN dotnet restore
RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime-env

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "ConsiliumTempus.Api.dll"]

EXPOSE 80