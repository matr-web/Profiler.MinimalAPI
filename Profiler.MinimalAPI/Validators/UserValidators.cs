using FluentValidation;
using Profiler.MinimalAPI.Entities;

namespace Profiler.MinimalAPI.Validators
{
    public class UserValidators : AbstractValidator<User>
    {
        public UserValidators()
        {
            RuleFor(u => u.FirstName)
                .NotNull()
                .WithMessage("User's first name is required")
                .Length(1, 32)
                .WithMessage("The first name length must be in range of 1 to 32 characters");

            RuleFor(u => u.LastName)
                .NotNull()
                .WithMessage("User's last name is required")
                .Length(1, 32)
                .WithMessage("The last name length must be in range of 1 to 32 characters");
        }
    }
}
