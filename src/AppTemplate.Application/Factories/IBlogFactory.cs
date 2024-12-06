namespace AppTemplate.Application.Factories;

public interface IBlogFactory
{
    Task<OneOf<Blog, Problem>> FromIdAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction);
}

internal class DefaultBlogFactory(
    IBlogRepository blogRepository, 
    IPostRepository postRepository): IBlogFactory
{
    public async Task<OneOf<Blog, Problem>> FromIdAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction)
    {
        // Fetch the Blog Details
        var blogEntity = await blogRepository.RetrieveAsync(blogId, dbConnection, dbTransaction);
        if (blogEntity is null)
        {
            return Problem.EntityNotFound<BlogEntity>(blogId.ToString());
        }

        // Fetch the Posts belonging to the Blog
        var postEntities = await postRepository.RetrieveOfBlogAsync(blogId, dbConnection, dbTransaction);
        var posts = postEntities
            .Select(x => x.ToModel())
            .ToImmutableList();
        
        // Return Object
        return CreateBlog(blogEntity, posts);
    }

    private static Blog CreateBlog(BlogEntity blogEntity, ImmutableList<Post> posts) => new()
    {
        Id = blogEntity.Id,
        Title = blogEntity.Title,
        Posts = posts        
    };
}