echo 'Setting up database...'
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -d master -i ./setup-database.sql -C

echo 'Database ready.'
