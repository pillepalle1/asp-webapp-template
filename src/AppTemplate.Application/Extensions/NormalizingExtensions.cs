namespace AppTemplate.Application.Extensions;

public static class NormalizingExtensions
{
    public static string NormalizeBlogTitle(this string blogTitle)
        => UseDefaultNormalization(blogTitle);
    
    public static string NormalizeClaimType(this string claimType) 
        => claimType.ToLower().Replace('-', '_').Replace(' ', '_');
    
    private static string UseDefaultNormalization(this string item)
        => item.Trim().ToUpper();
}