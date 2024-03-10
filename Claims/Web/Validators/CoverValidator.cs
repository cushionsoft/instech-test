using Claims.Web.Models;
using FluentValidation;
using System.Globalization;

namespace Claims.Web.Validators
{
    public class CoverValidator : AbstractValidator<Cover>
    {
        public CoverValidator()
        {
            RuleFor(c => c.StartDate).LessThan(DateOnly.FromDateTime(DateTime.Now)); //TODO: inject custom DateTime provider for tests, should be in UTC?
            RuleFor(c => c.StartDate).Must((c, startDate) => c.EndDate.DayNumber - c.StartDate.DayNumber <= new GregorianCalendar().GetDaysInYear(c.StartDate.Year)).WithMessage("Total insurance period cannot exceed 1 year"); //is GregorianCalendar the right one to use?
        }
    }
}
