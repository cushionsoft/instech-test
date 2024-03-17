using Claims.Web.Models;
using FluentValidation;

namespace Claims.Web.Validators
{
    public class CoverValidator : AbstractValidator<Cover>
    {
        public CoverValidator()
        {
            RuleFor(c => c.StartDate).LessThan(DateOnly.FromDateTime(DateTime.Now)); //TODO: inject custom DateTime provider for tests, should be in UTC?
            RuleFor(c => c.StartDate).Must((c, startDate) => c.EndDate <= c.StartDate.AddYears(1)).WithMessage("Total insurance period cannot exceed 1 year");
        }
    }
}
