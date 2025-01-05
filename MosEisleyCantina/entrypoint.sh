
echo "Waiting for SQL Server to be ready..."
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "P@ssword123" -Q "SELECT 1" -b

echo "Adding Logs migration..."
dotnet ef migrations add Logs --environment Development

echo "Running migrations..."
dotnet ef database update --environment Development

echo "Starting the application..."
dotnet MosEisleyCantina.dll
