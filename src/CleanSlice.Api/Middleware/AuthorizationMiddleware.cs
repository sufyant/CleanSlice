using System.Net;
using System.Text.Json;

namespace CleanSlice.Api.Middleware;

public sealed class AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during request processing");
            throw;
        }

        // Authorization başarısız olduğunda daha detaylı response döndür
        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            await HandleForbiddenAsync(context);
        }
        else if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedAsync(context);
        }
    }

    private async Task HandleForbiddenAsync(HttpContext context)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.ContentType = "application/json";
        
        var response = new
        {
            error = "Forbidden",
            message = "You don't have permission to access this resource",
            statusCode = 403,
            timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private async Task HandleUnauthorizedAsync(HttpContext context)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.ContentType = "application/json";
        
        var response = new
        {
            error = "Unauthorized",
            message = "Authentication is required to access this resource",
            statusCode = 401,
            timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
