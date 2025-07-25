using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Shared.Contracts.Tenants.Requests;

public sealed record GetTenantRequest(
    [Required]
    [Description("Unique identifier of the tenant")]
    [FromRoute(Name = "tenantId")]
    Guid TenantId
    );
