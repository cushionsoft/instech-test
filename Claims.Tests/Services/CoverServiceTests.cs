using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;

namespace Claims.Application.Services.Tests
{
    public class CoverServiceTests
    {
        private readonly IAuditService _auditService;
        private readonly ICoverRepository _coverRepository;
        private readonly IPremiumCalculatorService _premiumCalculatorService;
        private readonly CoverService _coverService;

        public CoverServiceTests()
        {
            _auditService = Substitute.For<IAuditService>();
            _coverRepository = Substitute.For<ICoverRepository>();
            _premiumCalculatorService = Substitute.For<IPremiumCalculatorService>();
            _premiumCalculatorService.ReturnsForAll<decimal>(1);
            _coverService = new CoverService(_auditService, _premiumCalculatorService, _coverRepository);
        }

        [Fact]
        public async Task CreateAsync_Should_CreateCoverAndAddAudit()
        {
            // Arrange
            var cover = new Cover
            {
                Id = "1",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now),
                Type = Core.Enums.CoverType.Yacht,
                Premium = 1
            };

            // Act
            var result = await _coverService.CreateAsync(cover);

            // Assert
            await _coverRepository.Received().AddItemAsync(cover);
            _auditService.Received().AddCoverAudit(Arg.Is<CoverAudit>(audit => audit.CoverId == cover.Id && audit.HttpRequestType == "POST"));
            Assert.Equal(cover, result);
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteCoverAndAddAudit()
        {
            // Arrange
            var coverId = "1";

            // Act
            await _coverService.DeleteAsync(coverId);

            // Assert
            await _coverRepository.Received().DeleteItemAsync(coverId);
            _auditService.Received(1).AddCoverAudit(Arg.Is<CoverAudit>(audit => audit.CoverId == coverId && audit.HttpRequestType == "DELETE"));
        }
    }
}
