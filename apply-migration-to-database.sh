source ./.env

dotnet ef database update -p ./src/ConsiliumTempus.Infrastructure -s ./src/ConsiliumTempus.Api/ --connection "Server=Localhost,$DATABASE_PORT;Database=$DATABASE_NAME;User Id=$DATABASE_USER;Password=$DATABASE_PASSWORD;Encrypt=false"
