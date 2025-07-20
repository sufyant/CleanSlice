using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanSlice.Shared;

public class Tenant : BaseEntity
{
    public string Name { get; set; }
    public string ConnectionString { get; set; } // any tenant with a null value for ConnectionString will use the main shared database
}
