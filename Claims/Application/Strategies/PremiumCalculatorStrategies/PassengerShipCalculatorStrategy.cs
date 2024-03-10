namespace Claims.Application.Strategies.PremiumCalculatorStrategies
{
    public class PassengerShipCalculatorStrategy : DefaultCalculatorStrategy
    {
        public PassengerShipCalculatorStrategy(decimal dailyBase) : base(dailyBase)
        {
        }

        public override decimal DailyMultiplier => 1.2m;
    }
}
