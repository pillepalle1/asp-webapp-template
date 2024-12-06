using AppTemplate.Application.Cqrs.Blogs;

namespace AppTemplate.Web.BackgroundServices;

public class BlogWatcher(
    ILogger<BlogWatcher> logger,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(30 * 1000, stoppingToken);

            using var scope = serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            var fetch = await mediator.Send(new AllBlogHeadersQuery(), stoppingToken);
            if (fetch.Succeeded())
            {
                var blogs = fetch.Unwrap();
                logger.LogInformation("Currently {BlogCount} blogs in database", blogs.Count);    
            }
            else
            {
                var problem = fetch.Problem();
                logger.LogError("{ProblemDescription}", problem.Description);
            }
        }
    }
}
