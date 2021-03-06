using FluentValidation;
using ReadItLater.Infrastructure.Commands.Refs;

namespace ReadItLater.Infrastructure.DataValidators.Commands.Refs
{
    public class UpdateRefCountOfViewCommandValidator : AbstractValidator<UpdateRefCountOfViewCommand>
    {
        public UpdateRefCountOfViewCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();
        }
    }
}
