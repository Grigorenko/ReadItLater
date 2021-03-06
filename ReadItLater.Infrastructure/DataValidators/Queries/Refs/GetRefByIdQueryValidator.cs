using FluentValidation;
using ReadItLater.Infrastructure.Queries.Refs;

namespace ReadItLater.Infrastructure.DataValidators.Queries.Refs
{
    public class GetRefByIdQueryValidator : AbstractValidator<GetRefByIdQuery>
    {
        public GetRefByIdQueryValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();
        }
    }
}
