using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using AppTemplate.Application.Persistence.SqLite;

namespace AppTemplate.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Automagically add services via assembly scanning
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(executingAssembly);
        services.AddMediatR(executingAssembly);

        // Database services
        services.ConfigureSqlite();

        // Repositories
        services.AddRepositories();
        
        // Factories
        services.AddFactories();
        
        // Managers
        services.AddManagers();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBlogRepository, DefaultBlogRepository>();
        services.AddScoped<IPostRepository, DefaultPostRepository>();

        return services;
    }
    
    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddScoped<IBlogFactory, DefaultBlogFactory>();

        return services;
    }
    
    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<IBlogManager, DefaultBlogManager>();

        return services;
    }
}