using CleanSlice.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Contexts;

public sealed class TenantDbContext(DbContextOptions<TenantDbContext> options)
    : DbContext(options), ITenantDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.TenantData);
        
        // Uncomment the line below if you have a multitenancy setup and want to filter entities by TenantId
        //_ = modelBuilder.AppendGlobalQueryFilter<IMustHaveTenant>(b => b.TenantId == CurrentTenantId); 
        
        // Filter entities that implement ISoftDelete interface to only include those that are not deleted
        //_ = modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedAt == null); 
        
        base.OnModelCreating(modelBuilder);
    }
    
}
