using AppTemplate.Web.Config;
using AppTemplate.Web.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AppTemplate.Web;

public static class _DependencyInjection
{
    public static WebApplicationBuilder ConfigureWeb(this WebApplicationBuilder builder)
    {
        builder.ConfigurePolicies();
        builder.ConfigureJwt();

        return builder;
    }

    private static WebApplicationBuilder ConfigurePolicies(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            
            .AddPolicy("IsBlogManagerPolicy", policy => policy.RequireAssertion(context 
                => context.User.HasClaim(claim => 
                    claim.Type == ApplicationClaimTypes.IsBlogManager || 
                    claim.Type == ApplicationClaimTypes.IsSeniorBlogManager)))
            
            .AddPolicy("IsSeniorBlogManagerPolicy", policy => policy.RequireClaim(ApplicationClaimTypes.IsSeniorBlogManager));
        
        builder.Services.AddSingleton<IAuthorizationHandler, BlogOwnerAuthorizationHandler>();

        return builder;
    }

    private static WebApplicationBuilder ConfigureJwt(this WebApplicationBuilder builder)
    {
        var env = builder.Environment;
        var services = builder.Services;
        var config = builder.Configuration;
        
        // Fetch parameters from configuration
        var jwt = new JwtParameters();
        config.GetSection(JwtParameters.SectionName).Bind(jwt);
        
        services.AddSingleton<JwtParameters>(jwt);
        
        // Configure token validation
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = jwt.IssuerSigningKey()
        };

        if (env.IsDevelopment())
        {
            tokenValidationParameters.ValidateIssuer = false;
            tokenValidationParameters.ValidateAudience = false;
            tokenValidationParameters.ValidateLifetime = false;
            tokenValidationParameters.ValidateIssuerSigningKey = false;
        }

        services.AddSingleton<TokenValidationParameters>(tokenValidationParameters);
        
        // Add Authentication
        builder.Services.AddAuthentication(o =>
        {
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = tokenValidationParameters;
        });

        return builder;
    }
}