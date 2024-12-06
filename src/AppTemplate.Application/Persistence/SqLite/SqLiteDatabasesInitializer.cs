namespace AppTemplate.Application.Persistence.SqLite;

internal class SqLiteDatabasesInitializer(IDatabasesProvider databaseConnectionProvider) : IDatabasesInitializer
{
    public async Task InitializeAllAsync()
    {
        await InitializeBlogDatabase();
    }

    private async Task InitializeBlogDatabase()
    {
        using (var dbConnection = await databaseConnectionProvider.ProvideBlogNewAsync())
        using (var transaction = dbConnection.BeginTransaction())
        {
            _ = await dbConnection.ExecuteAsync(
                """
                CREATE TABLE IF NOT EXISTS Blogs (
                    Id UUID PRIMARY KEY,
                    Title TEXT NOT NULL);
                """);
            
            _ = await dbConnection.ExecuteAsync(
                """
                CREATE TABLE IF NOT EXISTS Posts (
                    Id UUID PRIMARY KEY,
                    BlogId UUID NOT NULL,
                    Timestamp TIMESTAMPTZ NOT NULL,
                    Content TEXT NOT NULL);
                """);
            
            transaction.Commit();
        }
    }
}
