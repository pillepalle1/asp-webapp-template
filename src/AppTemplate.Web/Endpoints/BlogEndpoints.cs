using AppTemplate.Application.Cqrs.Blogs;
using AppTemplate.Application.Cqrs.Posts;

namespace AppTemplate.Web.Endpoints;

internal static class CustomerEndpoints
{
    private const string EndpointTags = "Blog";
    internal static void RegisterBlogEndpoints(this WebApplication app)
    {
        app.MapPut("/api/blogs", CreateBlog)
            .Accepts<CreateBlogRequest>("application/json")
            .Produces<BlogResponse>()
            .WithTags(EndpointTags);
        
        app.MapGet("/api/blogs", GetBlogHeaders)
            .Produces<IEnumerable<BlogHeaderResponse>>()
            .WithTags(EndpointTags);

        app.MapGet("/api/blogs/{blogId:guid}", GetBlog)
            .Produces<IEnumerable<BlogResponse>>()
            .WithTags(EndpointTags);

        app.MapPost("/api/blogs/{blogId:guid}/posts", AddPost)
            .Accepts<CreatePostRequest>("application/json")
            .Produces<BlogResponse>()
            .WithTags(EndpointTags);
    }
    
    // Action handlers
    private static async Task<IResult> CreateBlog(
        CreateBlogRequest request,
        IMediator mediator)
    {
        var create = await mediator.Send(new CreateBlogCmd
        {
            Title = request.Title
        });

        return create.Succeeded()
            ? Results.Ok(create.Unwrap().ToResponse())
            : create.Problem().ToProblemDetailsResult();
    }
    
    private static async Task<IResult> GetBlogHeaders(
        IMediator mediator)
    {
        var fetch = await mediator.Send(new AllBlogHeadersQuery());

        return fetch.Succeeded()
            ? Results.Ok(fetch.Unwrap().Select(x => x.ToResponse()))
            : fetch.Problem().ToProblemDetailsResult();
    }

    private static async Task<IResult> GetBlog(
        Guid blogId,
        IMediator mediator)
    {
        var fetch = await mediator.Send(new BlogQuery
        {
            BlogId = blogId
        });

        return fetch.Succeeded()
            ? Results.Ok(fetch.Unwrap().ToResponse())
            : fetch.Problem().ToProblemDetailsResult();
    }
    
    private static async Task<IResult> AddPost(
        CreatePostRequest request,
        Guid blogId,
        IMediator mediator)
    {
        var add = await mediator.Send(new CreatePostCmd
        {
            BlogId = blogId,
            Timestamp = DateTimeOffset.UtcNow,
            Content = request.Content
        });

        return add.Succeeded()
            ? Results.Ok(add.Unwrap().ToResponse())
            : add.Problem().ToProblemDetailsResult();
    }
}