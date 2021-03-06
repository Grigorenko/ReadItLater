using FluentValidation;
using ReadItLater.Infrastructure.Commands.Refs;
using System.Text.RegularExpressions;

namespace ReadItLater.Infrastructure.DataValidators.Commands.Refs
{
    public class CreateRefCommandValidator : AbstractValidator<CreateRefCommand>
    {
        public CreateRefCommandValidator()
        {
            RuleFor(p => p.FolderId)
                .NotEmpty();
            RuleFor(p => p.Title)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(500);
            RuleFor(p => p.Url)
                .NotEmpty()
                .Matches(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", RegexOptions.IgnoreCase)
                .MinimumLength(3)
                .MaximumLength(500);
            RuleFor(p => p.Image)
                .MaximumLength(500);
            RuleFor(p => p.Note)
                .MaximumLength(2000);
        }
    }
}
