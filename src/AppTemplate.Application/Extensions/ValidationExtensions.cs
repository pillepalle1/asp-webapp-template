namespace AppTemplate.Application.Extensions;

internal static class ValidationExtensions
{
    // General Id Validation logic
    
    public static IRuleBuilderOptions<T, Guid> IsValidId<T>(this IRuleBuilder<T, Guid> ruleBuilder) =>
        ruleBuilder.Must(x => !x.Equals(Guid.Empty)).WithMessage("Empty Guid is not a valid Id");
    
    public static IRuleBuilderOptions<T, long> IsValidId<T>(this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThanOrEqualTo(0).WithMessage("Ids cannot be negative");
    
    public static IRuleBuilderOptions<T, int> IsValidId<T>(this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder.GreaterThanOrEqualTo(0).WithMessage("Ids cannot be negative");
    
    public static IRuleBuilderOptions<T, string> IsValidNormalizedBlogTitle<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotEmpty().WithMessage("Please provide a valid Title");
    
    public static IRuleBuilderOptions<T, string> IsValidNormalizedPostContent<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotEmpty().WithMessage("Please provide content for the post");
}