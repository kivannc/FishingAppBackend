CONTAINER_NAME="sqlserver"
SQL_IMAGE="mcr.microsoft.com/mssql/server"
SA_PASSWORD="123qwe123S!"

# Check if the container is already running
if [ "$(docker ps -q -f name=$CONTAINER_NAME)" ]; then
    echo "SQL Server container is already running."
else
    # Check if the container exists but is not running
    if [ "$(docker ps -aq -f status=exited -f name=$CONTAINER_NAME)" ]; then
        echo "Starting existing SQL Server container..."
        docker start $CONTAINER_NAME
    else
        # Run a new SQL Server container
        echo "Running a new SQL Server container..."
        docker run -e 'ACCEPT_EULA=Y' -e "SA_PASSWORD=$SA_PASSWORD" -p 1433:1433 --name $CONTAINER_NAME -d $SQL_IMAGE
    fi
fi