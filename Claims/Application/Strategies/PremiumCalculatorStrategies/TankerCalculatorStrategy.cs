namespace Claims.Application.Strategies.PremiumCalculatorStrategies
{
    public class TankerCalculatorStrategy : DefaultCalculatorStrategy
    {
        public TankerCalculatorStrategy(decimal dailyBase) : base(dailyBase)
        {
        }

        public override decimal DailyMultiplier => 1.5m;
    }
}
