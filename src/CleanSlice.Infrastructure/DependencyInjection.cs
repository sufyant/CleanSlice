using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Authorization;
using CleanSlice.Application.Abstractions.Caching;
using CleanSlice.Application.Abstractions.Keycloak;
using CleanSlice.Infrastructure.Authentication;
using CleanSlice.Infrastructure.Authorization;
using CleanSlice.Infrastructure.Caching;
using CleanSlice.Infrastructure.Keycloak;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSlice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddAuthentication(services, configuration);
        AddAuthorization(services, configuration);
        AddCaching(services, configuration);
        AddKeycloak(services, configuration);

        return services;
    }
    
    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }
    
    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Cache") ??
                                  throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddAuthorization(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
    }

    private static void AddKeycloak(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection(KeycloakOptions.SectionName));

        services.AddHttpClient<IKeycloakService, KeycloakService>();
    }
}
