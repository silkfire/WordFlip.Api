echo 'Starting up SQL Server 2025...'
/opt/mssql/bin/sqlservr & 
./setup-database.sh
wait
