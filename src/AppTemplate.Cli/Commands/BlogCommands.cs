using AppTemplate.Application.Cqrs.Blogs;

namespace AppTemplate.Cli.Commands;

public static class BlogCommandsExtensions
{
    public static CoconaApp AddBlogCommands(this CoconaApp app)
    {
        app.AddCommand("create-blog", BlogCommands.CreateBlog);
        
        return app;
    }
}

internal class BlogCommands
{
    public static async Task CreateBlog(
        string? blogTitle,
        BlogConfig blogConfig,
        ILogger<BlogCommands> logger,
        IMediator mediator)
    {
        if (blogTitle is null)
        {
            blogTitle = blogConfig.DefaultTitle;
        }
        
        var add = await mediator.Send(new CreateBlogCmd
        {
            Title = blogTitle
        });

        if (add.Succeeded())
        {
            var blog = add.Unwrap();
            
            logger.LogInformation($"Id .......... {blog.Id.ToString()}");
            logger.LogInformation($"Title........ {blog.Title}");
            logger.LogInformation($"# Posts ..... {blog.Posts.Count}");
        }
        else
        {
            logger.LogCritical(add.Problem().Description);
        }
    }    
}