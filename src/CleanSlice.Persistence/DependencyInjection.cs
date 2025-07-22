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
        #region Tenant Catalog DbContext

        var tenantCatalogDbConnectionString = configuration.GetConnectionString("TenantCatalog") ??
                                              throw new ArgumentNullException(nameof(configuration));
        
        services.AddDbContext<TenantCatalogDbContext>(options =>
            options.UseNpgsql(tenantCatalogDbConnectionString).UseSnakeCaseNamingConvention());

        #endregion
        
        #region Application DbContext
        
        var applicationDbConnectionString = configuration.GetConnectionString("Tenant") ??
                                            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(applicationDbConnectionString).UseSnakeCaseNamingConvention());

        services.AddScoped<IApplicationDbContext>(sp => 
            sp.GetRequiredService<ApplicationDbContext>());
        
        #endregion
        
        services.AddSingleton<IDbConnectionFactory>(_ => 
            new DbConnectionFactory(applicationDbConnectionString));
        
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
