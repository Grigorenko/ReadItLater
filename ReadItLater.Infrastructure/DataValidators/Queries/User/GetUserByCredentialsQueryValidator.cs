using FluentValidation;
using ReadItLater.Infrastructure.Queries.User;

namespace ReadItLater.Infrastructure.DataValidators.Queries.User
{
    public class GetUserByCredentialsQueryValidator : AbstractValidator<GetUserByCredentialsQuery>
    {
        public GetUserByCredentialsQueryValidator()
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
