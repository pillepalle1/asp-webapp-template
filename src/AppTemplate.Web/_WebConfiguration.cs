namespace AppTemplate.Web;

internal static class _WebConfiguration
{
    public static void ConfigureApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "AppTemplate",
                Version = "v1"
            });
        });
    }
    
    
    public static void ConfigureAppServices(this IServiceCollection services)
    {
        services.AddRazorPages();
    }

    public static void ConfigureApiMiddleware(this WebApplication application)
    {
        if (!application.Environment.IsProduction())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        application.RegisterBlogEndpoints();
    }

    public static void ConfigureAppMiddleware(this WebApplication application)
    {
        if (!application.Environment.IsDevelopment())
        {
            application.UseExceptionHandler("/Error");
        }

        application.UseRouting();
        application.UseAuthorization();
        application.MapStaticAssets();
        application.MapRazorPages().WithStaticAssets();
    }
}