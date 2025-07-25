using Asp.Versioning;
using CleanSlice.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CleanSlice.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.Audience = "account";
                o.MetadataAddress = "http://localhost:8080/realms/sample-realm/.well-known/openid-configuration";
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "http://localhost:8080/realms/sample-realm"
                };
            });

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
