namespace AppTemplate.Application.Models;

public static class ApplicationClaimTypes
{
    public static string OwnsBlog => "owns_blog".NormalizeClaimType();
    
    public static string IsBlogManager => "is_blog_manager".NormalizeClaimType();
    
    public static string IsSeniorBlogManager => "is_blog_manager".NormalizeClaimType();

    
    public static ImmutableList<string> AvailableClaims { get; } = new List<string>
    {
        IsBlogManager,
        IsSeniorBlogManager
    }.ToImmutableList();
}