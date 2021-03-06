using FluentValidation;
using ReadItLater.Infrastructure.Commands.Folders;

namespace ReadItLater.Infrastructure.DataValidators.Commands.Folders
{
    public class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
    {
        public CreateFolderCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(200);
        }
    }
}
