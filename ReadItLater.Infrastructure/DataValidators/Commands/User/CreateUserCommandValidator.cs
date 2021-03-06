using FluentValidation;
using ReadItLater.Infrastructure.Commands.User;

namespace ReadItLater.Infrastructure.DataValidators.Commands.User
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.Username)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);
            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);
        }
    }
}
