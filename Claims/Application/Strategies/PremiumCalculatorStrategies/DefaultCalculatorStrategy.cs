using Claims.Core.Strategies;

namespace Claims.Application.Strategies.PremiumCalculatorStrategies
{
    public class DefaultCalculatorStrategy : IPremiumCalculatorStrategy
    {
        protected readonly decimal _dailyBase;
        public virtual decimal DailyMultiplier { get; } = 1.3m;
        protected decimal DailyPremium => _dailyBase * DailyMultiplier;

        public DefaultCalculatorStrategy(decimal dailyBase)
        {
            _dailyBase = dailyBase;
        }

        public virtual decimal GetDailyCost(int day)
        {
            var dailyCost = DailyPremium;

            if (day >= 30)
            {
                dailyCost -= dailyCost * 0.02m;
                if (day >= 180) dailyCost -= dailyCost * 0.01m;
            }

            return dailyCost;
        }
    }
}
