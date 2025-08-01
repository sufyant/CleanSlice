using CleanSlice.Application.Abstractions.Data;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Persistence.Repositories;
using CleanSlice.Persistence.TenantManagement.Repositories;
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

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<ITenantManagementRepository, TenantManagementRepository>();

        // User management repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTenantRepository, UserTenantRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();

        return services;
    }
}
