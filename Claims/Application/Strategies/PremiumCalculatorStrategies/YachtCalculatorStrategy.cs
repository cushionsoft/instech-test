namespace Claims.Application.Strategies.PremiumCalculatorStrategies
{
    public class YachtCalculatorStrategy : DefaultCalculatorStrategy
    {
        public YachtCalculatorStrategy(decimal dailyBase) : base(dailyBase)
        {
        }

        public override decimal DailyMultiplier => 1.1m;

        public override decimal GetDailyCost(int day)
        {
            var dailyCost = DailyPremium;

            if (day >= 30)
            {
                dailyCost -= dailyCost * 0.05m;
                if (day >= 180) dailyCost -= dailyCost * 0.03m;
            }

            return dailyCost;
        }
    }
}
