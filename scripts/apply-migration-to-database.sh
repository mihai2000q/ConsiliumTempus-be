DIR_SCRIPT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

source ${DIR_SCRIPT}/../.env

tools=$( dotnet tool list --global )
 
if [[ $tools != *"dotnet-ef"* ]] 
then
    echo "Installing Dotnet Entity Framework Tool..."
    dotnet tool install --global dotnet-ef --version 8.0.3
else
    echo "Dotnet Entity Framework Tool already installed!"
fi

dotnet ef database update -p ${DIR_SCRIPT}/../src/ConsiliumTempus.Infrastructure -s ${DIR_SCRIPT}/../src/ConsiliumTempus.Api/ --connection "Server=Localhost,$DATABASE_PORT;Database=$DATABASE_NAME;User Id=$DATABASE_USER;Password=$DATABASE_PASSWORD;Encrypt=false"
