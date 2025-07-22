using CleanSlice.Domain.Tenants;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Contexts;

public sealed class TenantCatalogDbContext(DbContextOptions<TenantCatalogDbContext> options)
    : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantSetting> TenantSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.TenantCatalog);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantCatalogDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }

}
