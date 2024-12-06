namespace AppTemplate.Application.Cqrs.Blogs;

public class CreateBlogCmd : ARequest<Blog>
{
    public required string Title { init; get; }
}

public class CreateBlogCmdValidator : AbstractValidator<CreateBlogCmd>
{
    public CreateBlogCmdValidator()
    {
        RuleFor(x => x.Title).IsValidNormalizedBlogTitle();
    }
}

internal class CreateBlogCmdHandler(
    ILogger<CreateBlogCmdHandler> logger,
    IEnumerable<IValidator<CreateBlogCmd>> validators,
    IDatabasesProvider databasesProvider, 
    IBlogManager blogManager)
    : ARequestHandler<CreateBlogCmd, Blog>(logger, validators)
{
    public override async Task<OneOf<Blog, Problem>> HandleImpl(CreateBlogCmd request, CancellationToken cancellationToken)
    {
        var dbConnection = await databasesProvider.ProvideBlogNewAsync();
        return await blogManager.CreateBlogAsync(request, dbConnection, null);
    }
}