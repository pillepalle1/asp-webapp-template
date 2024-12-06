namespace AppTemplate.Application.Repositories;

public interface IPostRepository
{
    Task<PostEntity?> CreateAsync(PostEntity post, IDbConnection dbConnection, IDbTransaction? dbTransaction);

    Task<IEnumerable<PostEntity>?> CreateAsync(ImmutableList<PostEntity> posts, IDbConnection dbConnection, IDbTransaction? dbTransaction);
    
    Task<IEnumerable<PostEntity>> RetrieveOfBlogAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction);
}

internal class DefaultPostRepository : IPostRepository
{
    public async Task<PostEntity?> CreateAsync(PostEntity post, IDbConnection dbConnection, IDbTransaction? dbTransaction)
    {
        var rowsAffected = await dbConnection.ExecuteAsync(SqlCreate, post, dbTransaction);

        return rowsAffected > 0
            ? post
            : null;
    }

    public async Task<IEnumerable<PostEntity>?> CreateAsync(ImmutableList<PostEntity> posts, IDbConnection dbConnection, IDbTransaction? dbTransaction)
    {
        var rowsAffected = await dbConnection.ExecuteAsync(SqlCreate, posts, dbTransaction);

        return rowsAffected == posts.Count
            ? posts
            : null;
    }

    public Task<IEnumerable<PostEntity>> RetrieveOfBlogAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction)
    {
        const string Sql =
            """
            SELECT * FROM Posts WHERE BlogId = @BlogId;
            """;

        return dbConnection.QueryAsync<PostEntity>(Sql, new { BlogId = blogId }, dbTransaction);
    }

    private const string SqlCreate =
        """
        INSERT INTO Posts (Id, BlogId, Timestamp, Content)
        VALUES (@Id, @BlogId, @Timestamp, @Content);
        """;
}