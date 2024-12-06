namespace AppTemplate.Cli.Extensions;

public static class BlogExtensions
{
    public static string GetBlogTitle(this Blog blog)
        => blog.Title;
}