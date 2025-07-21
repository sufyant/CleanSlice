using CleanSlice.Domain.Tenants;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Contexts;

public sealed class TenantCatalogDbContext(DbContextOptions<TenantCatalogDbContext> options)
    : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantCatalogDbContext).Assembly);
        
        modelBuilder.HasDefaultSchema(Schemas.TenantCatalog);
        
        base.OnModelCreating(modelBuilder);
    }

}
