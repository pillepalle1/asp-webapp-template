namespace AppTemplate.Application.Cqrs.Posts;

public class CreatePostCmd : ARequest<Blog>
{
    public required Guid BlogId { init; get; }
    public required DateTimeOffset Timestamp { init; get; }
    public required string Content { init; get; }
}

public class CreatePostCmdValidator : AbstractValidator<CreatePostCmd>
{
    public CreatePostCmdValidator()
    {
        RuleFor(x => x.Content).IsValidNormalizedBlogTitle();
    }
}

internal class CreatePostCmdHandler(
    ILogger<CreatePostCmdHandler> logger,
    IEnumerable<IValidator<CreatePostCmd>> validators,
    IDatabasesProvider databasesProvider, 
    IBlogManager blogManager)
    : ARequestHandler<CreatePostCmd, Blog>(logger, validators)
{
    public override async Task<OneOf<Blog, Problem>> HandleImpl(CreatePostCmd request, CancellationToken cancellationToken)
    {
        var dbConnection = await databasesProvider.ProvideBlogNewAsync();
        return await blogManager.AddToBlogAsync(request.BlogId, request, dbConnection, null);
    }
}