using Claims.Application.Strategies.PremiumCalculatorStrategies;
using Claims.Core.Services;
using Claims.Core.Strategies;

namespace Claims.Application.Services
{
    public class PremiumCalculatorService : IPremiumCalculatorService
    {
        private readonly decimal _premiumPerDayBase = 1250;

        public decimal ComputePremium(DateOnly startDate, DateOnly endDate, Core.Enums.CoverType coverType)
        {
            IPremiumCalculatorStrategy premiumStrategy = coverType switch
            {
                Core.Enums.CoverType.Yacht => new YachtCalculatorStrategy(_premiumPerDayBase),
                Core.Enums.CoverType.PassengerShip => new PassengerShipCalculatorStrategy(_premiumPerDayBase),
                Core.Enums.CoverType.Tanker => new TankerCalculatorStrategy(_premiumPerDayBase),
                _ => new DefaultCalculatorStrategy(_premiumPerDayBase)
            };

            var insuranceLength = endDate.DayNumber - startDate.DayNumber;
            var totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                totalPremium += premiumStrategy.GetDailyCost(i);
            }

            return totalPremium;
        }
    }
}
