using CleanSlice.Application.Abstractions.Data;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Persistence.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSlice.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var tenantCatalogDbConnectionString = configuration.GetConnectionString("TenantCatalog") ??
                                       throw new ArgumentNullException(nameof(configuration));
        
        var tenantDbConnectionString = configuration.GetConnectionString("Tenant") ??
                                              throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<TenantDbContext>(options => 
            options.UseNpgsql(tenantDbConnectionString).UseSnakeCaseNamingConvention());

        services.AddScoped<ITenantDbContext>(sp => 
            sp.GetRequiredService<TenantDbContext>());
        
        services.AddDbContext<TenantCatalogDbContext>(options =>
            options.UseNpgsql(tenantCatalogDbConnectionString).UseSnakeCaseNamingConvention());

        services.AddSingleton<IDbConnectionFactory>(_ => 
            new DbConnectionFactory(tenantDbConnectionString));
        
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
