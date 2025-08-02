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

        var tenantCatalogDbConnectionString = configuration.GetConnectionString("TenantCatalog");
        if (string.IsNullOrWhiteSpace(tenantCatalogDbConnectionString))
            throw new ArgumentException("TenantCatalog connection string is not configured", nameof(configuration));

        services.AddDbContext<TenantCatalogDbContext>(options =>
            options.UseNpgsql(tenantCatalogDbConnectionString)
                   .UseSnakeCaseNamingConvention());

        #endregion

        #region Application DbContext

        var applicationDbConnectionString = configuration.GetConnectionString("Tenant");
        if (string.IsNullOrWhiteSpace(applicationDbConnectionString))
            throw new ArgumentException("Tenant connection string is not configured", nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(applicationDbConnectionString)
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        #endregion

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Register Tenant Management Repository
        services.AddScoped<ITenantManagementRepository, TenantManagementRepository>();

        // Register User Management Repositories
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
