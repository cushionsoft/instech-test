namespace Claims.Core.Services
{
    public interface IPremiumCalculatorService
    {
        public decimal ComputePremium(DateOnly startDate, DateOnly endDate, Core.Enums.CoverType coverType);
    }
}
