namespace AppTemplate.Application.Entities;

public class PostEntity
{
    public required Guid Id { init; get; }
    public required Guid BlogId { init; get; }
    public required DateTimeOffset Timestamp { init; get; }
    public required string Content { init; get; }
}