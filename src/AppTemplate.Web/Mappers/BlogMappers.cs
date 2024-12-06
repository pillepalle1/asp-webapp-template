namespace AppTemplate.Web.Mappers;

internal static class BlogMappers
{
    public static BlogResponse ToResponse(this Blog blog) => new()
    {
        Id = blog.Id,
        Title = blog.Title,
        Posts = blog.Posts.Select(x => x.ToPostResponse()).ToImmutableList()
    };

    public static BlogHeaderResponse ToResponse(this BlogHeader blogHeader) => new()
    {
        Id = blogHeader.Id,
        Title = blogHeader.Title
    };
    
    public static PostResponse ToPostResponse(this Post post) => new()
    {
        Id = post.Id,
        Timestamp = post.Timestamp,
        Content = post.Content
    };
}