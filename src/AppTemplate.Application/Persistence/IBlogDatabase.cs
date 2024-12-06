namespace AppTemplate.Application.Persistence;

internal interface IDatabasesProvider
{
    Task<IDbConnection> ProvideBlogSingletonAsync();
    
    Task<IDbConnection> ProvideBlogNewAsync();
    
}

public interface IDatabasesInitializer
{
    Task InitializeAllAsync();
}
