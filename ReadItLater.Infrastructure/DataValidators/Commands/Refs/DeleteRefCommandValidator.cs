using FluentValidation;
using ReadItLater.Infrastructure.Commands.Refs;

namespace ReadItLater.Infrastructure.DataValidators.Commands.Refs
{
    public class DeleteRefCommandValidator : AbstractValidator<DeleteRefCommand>
    {
        public DeleteRefCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();
        }
    }
}
