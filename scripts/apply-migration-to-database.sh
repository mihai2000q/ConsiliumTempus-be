DIR_SCRIPT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

EF_VERSION="10.0.13"
installed=$( dotnet tool list --global | awk '$1 == "dotnet-ef" { print $2 }' )

if [[ -z $installed ]]
then
    echo "Installing Dotnet Entity Framework Tool..."
    dotnet tool install --global dotnet-ef --version ${EF_VERSION}
elif [[ ${installed%%.*} != ${EF_VERSION%%.*} ]]
then
    echo "Updating Dotnet Entity Framework Tool from ${installed} to ${EF_VERSION}..."
    dotnet tool update --global dotnet-ef --version ${EF_VERSION}
else
    echo "Dotnet Entity Framework Tool already installed!"
fi

dotnet ef database update -p ${DIR_SCRIPT}/../src/ConsiliumTempus.Infrastructure -s ${DIR_SCRIPT}/../src/ConsiliumTempus.Api/
