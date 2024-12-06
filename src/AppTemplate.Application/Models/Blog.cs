namespace AppTemplate.Application.Models;

public class BlogHeader
{
    public required Guid Id { init; get; }
    public required string Title { init; get; }
}

public class Blog
{
    public required Guid Id { init; get; }
    public required string Title { init; get; }
    public required ImmutableList<Post> Posts { init; get; }
}
