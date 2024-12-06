namespace AppTemplate.Application.Cqrs.Blogs;

public class BlogQuery : ARequest<Blog>
{
    public required Guid BlogId { init; get; }
}

public class BlogQueryValidator : AbstractValidator<BlogQuery>
{
    public BlogQueryValidator()
    {
        RuleFor(x => x.BlogId).IsValidId();
    }
}

internal class BlogQueryHandler(
    ILogger<BlogQueryHandler> logger,
    IEnumerable<IValidator<BlogQuery>> validators,
    IDatabasesProvider databasesProvider, 
    IBlogManager blogManager)
    : ARequestHandler<BlogQuery, Blog>(logger, validators)
{
    public override async Task<OneOf<Blog, Problem>> HandleImpl(BlogQuery request, CancellationToken cancellationToken)
    {
        var dbConnection = await databasesProvider.ProvideBlogNewAsync();
        return await blogManager.RetrieveBlogByIdAsync(request.BlogId, dbConnection, null);
    }
}