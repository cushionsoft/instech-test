using Claims.Core.Services;
using Claims.Web.Models;
using FluentValidation;

namespace Claims.Web.Validators
{
    public class ClaimValidator : AbstractValidator<Claim>
    {
        private readonly ICoverService _coverService;
        public ClaimValidator(ICoverService coverService)
        {
            _coverService = coverService;

            RuleFor(c => c.DamageCost).LessThanOrEqualTo(100000);
            RuleFor(c => c.CoverId).MustAsync(async (claim, coverId, cancellationToken) =>
            {
                var cover = await _coverService.GetAsync(coverId);
                if (cover == null)
                    return false;
                else
                    return cover.StartDate <= DateOnly.FromDateTime(claim.Created) && cover.EndDate >= DateOnly.FromDateTime(claim.Created);
            }).WithMessage("No active Cover found for the claim");
        }
    }
}
