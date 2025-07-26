using Asp.Versioning;
using CleanSlice.Api.Authorization;
using CleanSlice.Api.Infrastructure;
using CleanSlice.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CleanSlice.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        var authOptions = configuration.GetSection("Authentication");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = authOptions.GetValue<bool>("RequireHttpsMetadata");
                o.Audience = authOptions.GetValue<string>("Audience");
                o.MetadataAddress = authOptions.GetValue<string>("MetadataUrl");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authOptions.GetValue<string>("ValidIssuer"),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            // Fallback policy - tüm endpoint'ler authentication gerektirir
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Permission-based policy provider
            options.AddPolicy("Permission", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new PermissionRequirement(string.Empty));
            });
        });

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services.AddControllers();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new()
                {
                    Title = "CleanSlice API",
                    Version = "v1",
                    Description = "CleanSlice is a sample application built with ASP.NET Core, demonstrating best practices in software architecture and design patterns.",
                    Contact = new OpenApiContact
                    {
                        Url = new Uri("http://www.sufyant.com"),
                        Email = "sufyantaskin@gmail.com",
                        Name = "Sufyan Taskin"
                    }
                };
                return Task.CompletedTask;
            });
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
