using FluentValidation;
using Profiler.MinimalAPI.Entities;

namespace Profiler.MinimalAPI.Validators;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(r => r.RoleName)
            .NotEmpty()
            .WithMessage("The role name is required")
            .Length(1, 32)
            .WithMessage("The role name length must be in range of 1 to 32 characters");
    }
}
