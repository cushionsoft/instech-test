using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;
using NSubstitute;
using Xunit;

namespace Claims.Application.Services.Tests
{
    public class ClaimServiceTests
    {
        private readonly IAuditService _auditService;
        private readonly IClaimRepository _claimRepository;
        private readonly ClaimService _claimService;

        public ClaimServiceTests()
        {
            _auditService = Substitute.For<IAuditService>();
            _claimRepository = Substitute.For<IClaimRepository>();
            _claimService = new ClaimService(_auditService, _claimRepository);
        }

        [Fact]
        public async Task CreateAsync_Should_CreateClaimAndAddAudit()
        {
            // Arrange
            var claim = new Claim { Id = "1", CoverId = "1", Created = DateTime.Now, DamageCost = 1, Name = "Test Claim", Type = Core.Enums.ClaimType.BadWeather };

            // Act
            var result = await _claimService.CreateAsync(claim);

            // Assert
            await _claimRepository.Received().AddItemAsync(claim);
            _auditService.Received().AddClaimAudit(Arg.Is<ClaimAudit>(audit => audit.ClaimId == claim.Id && audit.HttpRequestType == "POST"));
            Assert.Equal(claim, result);
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteClaimAndAddAudit()
        {
            // Arrange
            var claimId = "1";

            // Act
            await _claimService.DeleteAsync(claimId);

            // Assert
            await _claimRepository.Received().DeleteItemAsync(claimId);
            _auditService.Received(1).AddClaimAudit(Arg.Is<ClaimAudit>(audit => audit.ClaimId == claimId && audit.HttpRequestType == "DELETE"));
        }
    }
}
