FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env

COPY . app/
WORKDIR /app

RUN dotnet restore src/ConsiliumTempus.Api
RUN dotnet publish src/ConsiliumTempus.Api -c Release -o /out --no-restore

# Self-contained executable that applies the EF migrations (no SDK or dotnet-ef needed to run it)
FROM build-env AS migrations-build-env

RUN dotnet tool install --global dotnet-ef --version 10.0.13
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef migrations bundle \
    -p src/ConsiliumTempus.Infrastructure \
    -s src/ConsiliumTempus.Api \
    -o /migrations/efbundle

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS migrator

WORKDIR /migrations
COPY --from=migrations-build-env /migrations/efbundle .

ENTRYPOINT ["./efbundle"]

# Inserts the sample data into an already migrated database, independent from the migrations and the Api
FROM build-env AS sample-data-build-env

RUN dotnet publish sample-data -c Release -o /sample-data

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS sample-data

WORKDIR /sample-data
COPY --from=sample-data-build-env /sample-data .

ENTRYPOINT ["dotnet", "ConsiliumTempus.SampleData.dll"]

# Keep the api as the last stage, so it is the default build target
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime-env

WORKDIR /app
COPY --from=build-env /out .

ENTRYPOINT ["dotnet", "ConsiliumTempus.Api.dll"]
