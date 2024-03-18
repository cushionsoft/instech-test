using Claims.Application.Services;
using Claims.Core.Enums;
using Xunit;

namespace Claims.Tests.Services
{
    public class PremiumCalculatorServiceTests
    {
        private readonly PremiumCalculatorService _sut;
        private readonly decimal _baseRate = 1250;
        public PremiumCalculatorServiceTests()
        {
            _sut = new PremiumCalculatorService();
        }

        [Theory]
        //2 days
        [InlineData("2020-01-01", "2020-01-02", CoverType.PassengerShip)]
        [InlineData("2020-01-01", "2020-01-02", CoverType.Tanker)]
        [InlineData("2020-01-01", "2020-01-02", CoverType.Yacht)]
        [InlineData("2020-01-01", "2020-01-02", CoverType.BulkCarrier)]
        [InlineData("2020-01-01", "2020-01-02", CoverType.ContainerShip)]
        //30 days
        [InlineData("2020-01-01", "2020-01-30", CoverType.PassengerShip)]
        [InlineData("2020-01-01", "2020-01-30", CoverType.Tanker)]
        [InlineData("2020-01-01", "2020-01-30", CoverType.Yacht)]
        [InlineData("2020-01-01", "2020-01-30", CoverType.BulkCarrier)]
        [InlineData("2020-01-01", "2020-01-30", CoverType.ContainerShip)]
        public void CalculatePremium_WhenInsuranceLengthLessThan30Days_ShouldReturnCorrectPremium(string startDate, string endDate, CoverType coverType)
        {
            // Arrange
            var startDateParsed = DateTime.Parse(startDate);
            var endDateParsed = DateTime.Parse(endDate);
            var insuranceLength = (endDateParsed - startDateParsed).Days;
            var expectedPremium = _baseRate * insuranceLength;

            switch (coverType)
            {
                case CoverType.PassengerShip:
                    expectedPremium *= 1.2m;
                    break;
                case CoverType.Tanker:
                    expectedPremium *= 1.5m;
                    break;
                case CoverType.Yacht:
                    expectedPremium *= 1.1m;
                    break;
                default:
                    expectedPremium *= 1.3m;
                    break;
            }

            // Act
            var actualPremium = _sut.ComputePremium(DateOnly.Parse(startDate), DateOnly.Parse(endDate), coverType);

            // Assert
            Assert.Equal(expectedPremium, actualPremium);
        }

        [Theory]
        //31 days
        [InlineData("2020-01-01", "2020-01-31", CoverType.PassengerShip)]
        [InlineData("2020-01-01", "2020-01-31", CoverType.Tanker)]
        [InlineData("2020-01-01", "2020-01-31", CoverType.Yacht)]
        [InlineData("2020-01-01", "2020-01-31", CoverType.BulkCarrier)]
        [InlineData("2020-01-01", "2020-01-31", CoverType.ContainerShip)]
        //180 days
        [InlineData("2020-01-01", "2020-06-29", CoverType.PassengerShip)]
        [InlineData("2020-01-01", "2020-06-29", CoverType.Tanker)]
        [InlineData("2020-01-01", "2020-06-29", CoverType.Yacht)]
        [InlineData("2020-01-01", "2020-06-29", CoverType.BulkCarrier)]
        [InlineData("2020-01-01", "2020-06-29", CoverType.ContainerShip)]
        public void CalculatePremium_WhenInsuranceLengthMoreThan30DaysAndLessThan180_ShouldReturnCorrectPremium(string startDate, string endDate, CoverType coverType)
        {
            // Arrange
            var startDateParsed = DateTime.Parse(startDate);
            var endDateParsed = DateTime.Parse(endDate);
            var insuranceLength = (endDateParsed - startDateParsed).Days;

            decimal premiumMultiplier;

            switch (coverType)
            {
                case CoverType.PassengerShip:
                    premiumMultiplier = 1.2m;
                    break;
                case CoverType.Tanker:
                    premiumMultiplier = 1.5m;
                    break;
                case CoverType.Yacht:
                    premiumMultiplier = 1.1m;
                    break;
                default:
                    premiumMultiplier = 1.3m;
                    break;
            }

            var dailyRate = _baseRate * premiumMultiplier;

            var expectedPremium = dailyRate * insuranceLength;

            var nonDiscountedDays = 30;

            //calculating discount
            switch (coverType)
            {
                case CoverType.Yacht:
                    expectedPremium -= (insuranceLength - nonDiscountedDays) * 0.05m * dailyRate;
                    break;
                default:
                    expectedPremium -= (insuranceLength - nonDiscountedDays) * 0.02m * dailyRate;
                    break;
            }

            // Act
            var actualPremium = _sut.ComputePremium(DateOnly.Parse(startDate), DateOnly.Parse(endDate), coverType);

            // Assert
            Assert.Equal(expectedPremium, actualPremium);
        }

        [Theory]
        //181 days
        [InlineData("2020-01-01", "2020-06-30", CoverType.PassengerShip)]
        [InlineData("2020-01-01", "2020-06-30", CoverType.Tanker)]
        [InlineData("2020-01-01", "2020-06-30", CoverType.Yacht)]
        [InlineData("2020-01-01", "2020-06-30", CoverType.BulkCarrier)]
        [InlineData("2020-01-01", "2020-06-30", CoverType.ContainerShip)]
        //365 days
        [InlineData("2020-01-01", "2020-12-31", CoverType.PassengerShip)]
        [InlineData("2020-01-01", "2020-12-31", CoverType.Tanker)]
        [InlineData("2020-01-01", "2020-12-31", CoverType.Yacht)]
        [InlineData("2020-01-01", "2020-12-31", CoverType.BulkCarrier)]
        [InlineData("2020-01-01", "2020-12-31", CoverType.ContainerShip)]

        public void CalculatePremium_WhenInsuranceLengthMoreThan180_ShouldReturnCorrectPremium(string startDate, string endDate, CoverType coverType)
        {
            // Arrange
            var startDateParsed = DateTime.Parse(startDate);
            var endDateParsed = DateTime.Parse(endDate);
            var insuranceLength = (endDateParsed - startDateParsed).Days;

            decimal premiumMultiplier;

            switch (coverType)
            {
                case CoverType.PassengerShip:
                    premiumMultiplier = 1.2m;
                    break;
                case CoverType.Tanker:
                    premiumMultiplier = 1.5m;
                    break;
                case CoverType.Yacht:
                    premiumMultiplier = 1.1m;
                    break;
                default:
                    premiumMultiplier = 1.3m;
                    break;
            }

            //calculate discount
            var dailyRate = _baseRate * premiumMultiplier;

            var expectedPremium = dailyRate * insuranceLength;

            var first30Days = 30;
            var first180Days = 180;

            decimal after30DaysDiscount;
            decimal after180DaysDiscount;

            switch (coverType)
            {
                case CoverType.Yacht:
                    after30DaysDiscount = 0.05m * dailyRate;
                    after180DaysDiscount = 0.03m * (dailyRate - after30DaysDiscount);
                    break;
                default:
                    after30DaysDiscount = 0.02m * dailyRate;
                    after180DaysDiscount = 0.01m * (dailyRate - after30DaysDiscount);
                    break;
            }

            expectedPremium -= (insuranceLength - first30Days) * after30DaysDiscount +
                (insuranceLength - first180Days) * after180DaysDiscount;

            // Act
            var actualPremium = _sut.ComputePremium(DateOnly.Parse(startDate), DateOnly.Parse(endDate), coverType);

            // Assert
            Assert.Equal(expectedPremium, actualPremium);
        }
    }
}
