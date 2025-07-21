using CleanSlice.Application.Abstractions.Data;
using CleanSlice.Persistence.Extensions;
using CleanSlice.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Contexts;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
        
        // Uncomment the line below if you have a multitenancy setup and want to filter entities by TenantId
        //_ = modelBuilder.AppendGlobalQueryFilter<IMustHaveTenant>(b => b.TenantId == CurrentTenantId); 
        
        // Filter entities that implement ISoftDelete interface to only include those that are not deleted
        _ = modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedAt == null); 
        
        base.OnModelCreating(modelBuilder);
    }
    
}
