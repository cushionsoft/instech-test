using Claims.Core.Strategies;

namespace Claims.Application.Strategies.PremiumCalculatorStrategies
{
    public class DefaultCalculatorStrategy : IPremiumCalculatorStrategy
    {
        protected readonly decimal _dailyBase;
        public virtual decimal DailyMultiplier { get; } = 1.3m;
        public decimal DailyPremium => _dailyBase * DailyMultiplier;

        public DefaultCalculatorStrategy(decimal dailyBase)
        {
            _dailyBase = dailyBase;
        }

        public virtual decimal GetDailyCost(int day)
        {
            decimal dailyCost = 0;

            if (day < 30) dailyCost += DailyPremium;
            if (day < 180) dailyCost += (DailyPremium - DailyPremium * 0.02m);
            if (day < 365) dailyCost += (DailyPremium - DailyPremium * 0.03m);

            return dailyCost;
        }
    }
}
