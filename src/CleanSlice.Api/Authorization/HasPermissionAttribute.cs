using Microsoft.AspNetCore.Authorization;

namespace CleanSlice.Api.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(policy: $"Permission.{permission}")
    {
        Permission = permission;
    }

    public string Permission { get; }
}
