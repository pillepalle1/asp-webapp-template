using AppTemplate.Application.Cqrs.Blogs;
using AppTemplate.Application.Cqrs.Posts;

namespace AppTemplate.Application.Managers;

public interface IBlogManager
{
    Task<OneOf<ImmutableList<BlogHeader>, Problem>> RetrieveAllBlogHeadersAsyc(IDbConnection dbConnection, IDbTransaction? dbTransaction = null);
    
    Task<OneOf<Blog, Problem>> CreateBlogAsync(CreateBlogCmd cmd, IDbConnection dbConnection, IDbTransaction? dbTransaction = null);
    
    Task<OneOf<Blog, Problem>> RetrieveBlogByIdAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction = null);

    Task<OneOf<Blog, Problem>> AddToBlogAsync(Guid blogId, CreatePostCmd cmd, IDbConnection dbConnection, IDbTransaction? dbTransaction = null);
}

internal class DefaultBlogManager(
    IBlogRepository blogRepository,
    IPostRepository postRepository,
    IBlogFactory blogFactory) : IBlogManager
{
    public async Task<OneOf<ImmutableList<BlogHeader>, Problem>> RetrieveAllBlogHeadersAsyc(IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
    {
        var entities = await blogRepository.RetrieveAllAsync(dbConnection, dbTransaction);
        
        return entities
            .Select(x => new BlogHeader
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToImmutableList();
    }
    
    public Task<OneOf<Blog, Problem>> RetrieveBlogByIdAsync(Guid blogId, IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
        => blogFactory.FromIdAsync(blogId, dbConnection, dbTransaction);
    
    public async Task<OneOf<Blog, Problem>> CreateBlogAsync(CreateBlogCmd cmd, IDbConnection dbConnection, IDbTransaction? dbTransaction)
    {
        // Create the blog in the database
        var blogEntity = await blogRepository.CreateAsync(new BlogEntity
        {
            Id = Guid.NewGuid(),
            Title = cmd.Title
        }, dbConnection, dbTransaction);

        if (blogEntity is null)
        {
            return Problem.FailedToPersist<BlogEntity>(cmd.Title);
        }
        
        // Return the entire blog
        return await RetrieveBlogByIdAsync(blogEntity.Id, dbConnection, dbTransaction);
    }
    
    public async Task<OneOf<Blog, Problem>> AddToBlogAsync(Guid blogId, CreatePostCmd cmd, IDbConnection dbConnection, IDbTransaction? dbTransaction = null)
    {
        var blogEntity = await blogRepository.RetrieveAsync(blogId, dbConnection, dbTransaction);
        if (blogEntity is null)
        {
            return Problem.EntityNotFound<BlogEntity>(blogId.ToString());
        }

        // Store the post in the database
        var postEntity = await postRepository.CreateAsync(new PostEntity
        {
            Id = Guid.NewGuid(),
            BlogId = blogEntity.Id,
            Timestamp = cmd.Timestamp,
            Content = cmd.Content
            
        }, dbConnection, dbTransaction);

        if (postEntity is null)
        {
            return Problem.FailedToPersist<PostEntity>(cmd.Content);
        }
        
        // Return the entire blog
        return await RetrieveBlogByIdAsync(blogEntity.Id, dbConnection, dbTransaction);
    }
}