namespace Claims.Core.Strategies
{
    public interface IPremiumCalculatorStrategy
    {
        public abstract decimal GetDailyCost(int day);
    }
}
