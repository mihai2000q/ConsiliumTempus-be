cd ..

source ./.env

dotnet tool install --global dotnet-ef --version 8.0.1

dotnet ef database update -p .././src/ConsiliumTempus.Infrastructure -s .././src/ConsiliumTempus.Api/ --connection "Server=Localhost,$DATABASE_PORT;Database=$DATABASE_NAME;User Id=$DATABASE_USER;Password=$DATABASE_PASSWORD;Encrypt=false"
