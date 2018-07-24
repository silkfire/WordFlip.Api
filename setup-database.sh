# Make sure the server has started before running the setup step
sleep 10s

echo 'Setting up database...'
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P January2018 -d master -i ./setup-database.sql

echo 'Database ready.'