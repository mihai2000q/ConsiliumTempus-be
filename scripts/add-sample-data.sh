DIR_SCRIPT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Text Colors
GREEN='\033[0;32m'
NC='\033[0m'

source ${DIR_SCRIPT}/../.env

# the sqlcmd location depends on the image version (mssql-tools18 on newer images)
sqlcmd=$( docker compose -f "${DIR_SCRIPT}/../docker-compose.yml" exec -u $UID database bash -c "ls /opt/mssql-tools18/bin/sqlcmd /opt/mssql-tools/bin/sqlcmd 2>/dev/null | head -n 1" | tr -d '\r' )

sqlFiles=$( docker compose -f "${DIR_SCRIPT}/../docker-compose.yml" exec -u $UID database bash -c "ls /mnt/sample-data/*.sql" | tr -d '\r' )

for file in $sqlFiles 
do
    echo -e "${GREEN} \nImporting ${file}...\n ${NC}"
    docker compose -f "${DIR_SCRIPT}/../docker-compose.yml" exec -u $UID database bash -c "${sqlcmd} -C -S localhost -U ${DATABASE_USER} -P ${DATABASE_PASSWORD} -d ${DATABASE_NAME} -i '${file}'"
done