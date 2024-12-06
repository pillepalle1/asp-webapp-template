namespace AppTemplate.Web.Extensions;

internal static class BlogExtensions
{
    public static string GetBlogTitle(this Blog blog)
        => blog.Title;

}