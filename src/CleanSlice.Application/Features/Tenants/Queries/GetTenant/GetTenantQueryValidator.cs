using FluentValidation;

namespace CleanSlice.Application.Features.Tenants.Queries.GetTenant;

internal class GetTenantQueryValidator : AbstractValidator<GetTenantQuery>
{
    public GetTenantQueryValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty()
            .WithMessage("Tenant ID is required.");
    }
}
