using FluentValidation;
using ReadItLater.Infrastructure.Queries.Tags;

namespace ReadItLater.Infrastructure.DataValidators.Queries.Tags
{
    public class GetTagsListQueryValidator : AbstractValidator<GetTagsListQuery>
    {
        public GetTagsListQueryValidator()
        {
            RuleFor(p => p.Key)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);
        }
    }
}
