using Microsoft.Extensions.Options;

namespace AppTemplate.Cli;

public static class _DependencyInjection
{
    public static CoconaAppBuilder AddConfiguration(this CoconaAppBuilder builder)
    {
        // Fetch parameters from configuration
        builder.Services.Configure<BlogConfig>(builder.Configuration.GetSection(BlogConfig.SectionName));
        builder.Services.AddTransient(provider => provider.GetRequiredService<IOptions<BlogConfig>>().Value);
        
        return builder;
    }
}