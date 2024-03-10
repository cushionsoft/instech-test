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
            decimal dailyCost = 0;

            if (day < 30) dailyCost += DailyPremium;
            if (day < 180) dailyCost += (DailyPremium - DailyPremium * 0.05m);
            if (day < 365) dailyCost += (DailyPremium - DailyPremium * 0.08m);

            return dailyCost;
        }
    }
}
