using FluentValidation;

namespace CleanSlice.Application.Features.Tenants.Commands.CreateTenant;

internal class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tenant name is required.")
            .MaximumLength(100)
            .WithMessage("Tenant name must not exceed 100 characters.");

        RuleFor(x => x.Domain)
            .NotEmpty()
            .WithMessage("Domain is required.")
            .MaximumLength(255)
            .WithMessage("Domain must not exceed 255 characters.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug is required.")
            .MaximumLength(50)
            .WithMessage("Slug must not exceed 50 characters.");

        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithMessage("Connection string is required.");
    }
}
