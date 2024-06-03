DIR_SCRIPT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Text Colors
GREEN='\033[0;32m'
NC='\033[0m'

source ${DIR_SCRIPT}/../.env

sqlFiles=$( docker compose -f "${DIR_SCRIPT}/../docker-compose.yml" exec -u $UID database bash -c "ls /mnt/sample-data/*.sql" )

for file in $sqlFiles 
do
    echo -e "${GREEN} \nImporting ${file}...\n ${NC}"
    docker compose -f "${DIR_SCRIPT}/../docker-compose.yml" exec -u $UID database bash -c "/opt/mssql-tools/bin/sqlcmd -S localhost -U ${DATABASE_USER} -P ${DATABASE_PASSWORD} -d ${DATABASE_NAME} -i '${file}'"
done