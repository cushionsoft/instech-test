using Claims.Core.Enums;
using Claims.Core.Services;
using Claims.Web.Models;
using Claims.Web.Validators;
using FluentValidation.TestHelper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Claims.Tests.Web.Validators
{
    public class ClaimValidatorTests
    {
        private readonly ICoverService _coverService;
        private readonly ClaimValidator _sut;

        public ClaimValidatorTests()
        {
            _coverService = Substitute.For<ICoverService>();
            _sut = new ClaimValidator(_coverService);
        }

        [Fact]
        public async Task DamageCost_ShouldBeLessThanOrEqualTo100000()
        {
            // Arrange
            var claim = new Claim
            {
                DamageCost = 50000,
                CoverId = "1",
                Created = DateTime.Now,
                Name = "Test Claim",
                Type = ClaimType.Fire
            };

            // Act
            var result = await _sut.TestValidateAsync(claim);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.DamageCost);
        }

        [Fact]
        public async Task DamageCost_ShouldFail_WhenGreaterThan100000()
        {
            // Arrange
            var claim = new Claim
            {
                DamageCost = 150000,
                CoverId = "1",
                Created = DateTime.Now,
                Name = "Test Claim",
                Type = ClaimType.Fire
            };

            // Act
            var result = await _sut.TestValidateAsync(claim);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DamageCost);
        }

        [Fact]
        public async Task CoverId_ShouldBeValid_WhenCoverExists()
        {
            // Arrange
            var claim = new Claim
            {
                DamageCost = 50000,
                CoverId = "1",
                Created = DateTime.Now,
                Name = "Test Claim",
                Type = ClaimType.Fire
            };
            var cover = new Core.Entities.Cover { StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), Type = CoverType.Tanker, Premium = 100 };
            _coverService.GetAsync(claim.CoverId).Returns(cover);

            // Act
            var result = await _sut.TestValidateAsync(claim);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.CoverId);
        }

        [Fact]
        public async Task CoverId_ShouldFail_WhenCoverDoesNotExist()
        {
            // Arrange
            var claim = new Claim
            {
                DamageCost = 50000,
                CoverId = "1",
                Created = DateTime.Now,
                Name = "Test Claim",
                Type = ClaimType.Fire
            };
            _coverService.GetAsync(claim.CoverId).ReturnsNull();

            // Act
            var result = await _sut.TestValidateAsync(claim);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CoverId).WithErrorMessage("No active Cover found for the claim");
        }

        [Fact]
        public async Task CoverId_ShouldFail_WhenCoverIsNotActive()
        {
            // Arrange
            var claim = new Claim
            {
                DamageCost = 50000,
                CoverId = "1",
                Created = DateTime.Now,
                Name = "Test Claim",
                Type = ClaimType.Fire
            };
            var cover = new Core.Entities.Cover { StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), Type = CoverType.Tanker, Premium = 100 };
            _coverService.GetAsync(claim.CoverId).Returns(cover);

            // Act
            var result = await _sut.TestValidateAsync(claim);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CoverId).WithErrorMessage("No active Cover found for the claim");
        }
    }
}
