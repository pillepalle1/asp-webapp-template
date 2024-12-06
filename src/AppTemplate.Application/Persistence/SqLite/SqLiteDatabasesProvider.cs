namespace AppTemplate.Application.Persistence.SqLite;

internal class SqLiteDatabasesProvider(IConfiguration config) : IDatabasesProvider
{
    private SqliteConnection? _blogDbSingleton;

    public async Task<IDbConnection> ProvideBlogSingletonAsync()
    {
        if (_blogDbSingleton is null)
        {
            _blogDbSingleton = new SqliteConnection(config.GetConnectionString("SQLite.Blog")); 
            await _blogDbSingleton.OpenAsync();
        }

        return _blogDbSingleton;
    }

    public async Task<IDbConnection> ProvideBlogNewAsync()
    { 
        var dbConnection = new SqliteConnection(config.GetConnectionString("SQLite.Blog"));
        await dbConnection.OpenAsync();

        return dbConnection;
    }
}
