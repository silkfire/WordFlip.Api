# Make sure the server has started before running the setup step
sleep 25s

echo 'Setting up database...'
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -d master -i ./setup-database.sql

echo 'Database ready.'
