using Microsoft.AspNetCore.Authorization;

namespace AppTemplate.Web.Policies;

internal class BlogOwnerRequirement : IAuthorizationRequirement
{
    
}

internal class BlogOwnerAuthorizationHandler : AuthorizationHandler<BlogOwnerRequirement>
{
    private const string RouteParameterName = "blogId";
    
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        BlogOwnerRequirement requirement)
    {
        // Get the HttpContext and fail if it does not exist
        var httpContext = context.Resource as HttpContext;
        if (httpContext is null)
        {
            return Task.CompletedTask;
        }
        
        // If route parameter userId is provided, make sure it matches the Claim id
        var idRouteValue = httpContext.Request.RouteValues[RouteParameterName] as string;
        if (String.IsNullOrWhiteSpace(idRouteValue))
        {
            return Task.CompletedTask;
        }
        
        // We are not logged in so we cannot access the user
        var id = Guid.Parse(idRouteValue).ToString();
        if (httpContext.User.HasClaim(claim => claim.Type == ApplicationClaimTypes.OwnsBlog && claim.Value == id))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}