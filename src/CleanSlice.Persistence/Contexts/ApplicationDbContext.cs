using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Data;
using CleanSlice.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Contexts;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options, 
    IUserContext userContext)
    : DbContext(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Set default schema for the database
        modelBuilder.HasDefaultSchema(Schemas.TenantData);
        
        // Apply tenant filter
        modelBuilder.ApplyTenantFilter(userContext.TenantId);
        
        // Apply soft delete filter
        modelBuilder.ApplySoftDeleteFilter();
        
        base.OnModelCreating(modelBuilder);
    }
}
