namespace AppTemplate.Application.Entities;

public class BlogEntity
{
    public required Guid Id { init; get; }
    public required string Title { init; get; }
}