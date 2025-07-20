using CleanSlice.Shared.Entities;

namespace CleanSlice.Shared.Models;

public class Tenant : BaseEntity
{
    public string Name { get; set; }
    public string ConnectionString { get; set; } // any tenant with a null value for ConnectionString will use the main shared database
}
