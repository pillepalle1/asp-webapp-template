namespace AppTemplate.Application.Repositories;

public interface IBlogRepository
{
    Task<BlogEntity?> CreateAsync(BlogEntity blog, IDbConnection dbConnection, IDbTransaction? dbTransaction = null);

    Task<IEnumerable<BlogEntity>> RetrieveAllAsync(IDbConnection dbConnection, IDbTransaction? dbTransaction = null);
    
    Task<BlogEntity?> RetrieveAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction = null);
}

internal class DefaultBlogRepository: IBlogRepository
{
    public async Task<BlogEntity?> CreateAsync(BlogEntity blog, IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
    {
        var rowsAffected = await dbConnection.ExecuteAsync(SqlCreate, blog, dbTransaction);

        return rowsAffected > 0
            ? blog
            : null;
    }

    public Task<IEnumerable<BlogEntity>> RetrieveAllAsync(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
    {
        const string Sql =
            """
            SELECT * FROM Blogs;
            """;

        return dbConnection.QueryAsync<BlogEntity>(Sql, null, dbTransaction);
    }
    
    public Task<BlogEntity?> RetrieveAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
    {
        const string Sql = 
            """
            SELECT * FROM Blogs WHERE Id = @BlogId;
            """;

        return dbConnection.QuerySingleOrDefaultAsync<BlogEntity>(Sql, new { BlogId = blogId }, dbTransaction);
    }

    private const string SqlCreate =
        """
        INSERT INTO Blogs (Id, Title)
        VALUES (@Id, @Title);
        """;
}
