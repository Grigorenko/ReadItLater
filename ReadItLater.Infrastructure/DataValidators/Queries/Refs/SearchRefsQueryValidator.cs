using FluentValidation;
using ReadItLater.Infrastructure.Queries.Refs;

namespace ReadItLater.Infrastructure.DataValidators.Queries.Refs
{
    public class SearchRefsQueryValidator : AbstractValidator<SearchRefsQuery>
    {
        public SearchRefsQueryValidator()
        {
            RuleFor(p => p.SearchTerm)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(200);
        }
    }
}
