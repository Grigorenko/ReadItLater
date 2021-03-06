using FluentValidation;
using ReadItLater.Infrastructure.Commands.Folders;

namespace ReadItLater.Infrastructure.DataValidators.Commands.Folders
{
    public class ChangeFolderNameCommandValidator : AbstractValidator<ChangeFolderNameCommand>
    {
        public ChangeFolderNameCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();
            RuleFor(p => p.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(200);
        }
    }
}
