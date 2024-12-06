namespace AppTemplate.Application.Cqrs.Blogs;

public class AllBlogHeadersQuery : ARequest<ImmutableList<BlogHeader>>
{
    
}

internal class AllBlogHeadersQueryHandler(
    ILogger<AllBlogHeadersQueryHandler> logger,
    IEnumerable<IValidator<AllBlogHeadersQuery>> validators,
    IDatabasesProvider databasesProvider, 
    IBlogManager blogManager)
    : ARequestHandler<AllBlogHeadersQuery, ImmutableList<BlogHeader>>(logger, validators)
{
    public override async Task<OneOf<ImmutableList<BlogHeader>, Problem>> HandleImpl(AllBlogHeadersQuery request, CancellationToken cancellationToken)
    {
        var dbConnection = await databasesProvider.ProvideBlogNewAsync();
        return await blogManager.RetrieveAllBlogHeadersAsyc(dbConnection, null);
    }
}