namespace AppTemplate.Application.Models;

public class Post
{
    public required Guid Id { init; get; }
    public required DateTimeOffset Timestamp { init; get; }
    public required string Content { init; get; }
}

internal static class PostMappingExtensions
{
    public static Post ToModel(this PostEntity entity) => new()
    {
        Id = entity.Id,
        Timestamp = entity.Timestamp,
        Content = entity.Content
    };
}