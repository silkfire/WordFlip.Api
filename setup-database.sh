echo 'Setting up database...'

# Wait for SQL Server to be ready
for i in {1..50};
do
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -Q "SELECT 1" -C > /dev/null 2>&1
    if [ $? -eq 0 ]
    then
        echo "SQL Server is ready."
        break
    else
        # echo "Not ready yet..."
        sleep 1
    fi
done

/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -d master -i ./setup-database.sql -C

echo 'Database ready.'
