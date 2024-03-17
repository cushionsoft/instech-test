using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;
using NSubstitute;
using Xunit;

namespace Claims.Application.Services.Tests
{
    public class ClaimServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_CreateClaimAndAddAudit()
        {
            // Arrange
            var claim = new Claim { Id = "1", CoverId = "1", Created = DateTime.Now, DamageCost = 1, Name = "Test Claim", Type = Core.Enums.ClaimType.BadWeather };
            var auditService = Substitute.For<IAuditService>();
            var claimRepository = Substitute.For<IClaimRepository>();
            var claimService = new ClaimService(auditService, claimRepository);

            // Act
            var result = await claimService.CreateAsync(claim);

            // Assert
            await claimRepository.Received().AddItemAsync(claim);
            auditService.Received().AddClaimAudit(Arg.Is<ClaimAudit>(audit => audit.ClaimId == claim.Id && audit.HttpRequestType == "POST"));
            Assert.Equal(claim, result);
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteClaimAndAddAudit()
        {
            // Arrange
            var claimId = "1";
            var auditService = Substitute.For<IAuditService>();
            var claimRepository = Substitute.For<IClaimRepository>();
            var claimService = new ClaimService(auditService, claimRepository);

            // Act
            await claimService.DeleteAsync(claimId);

            // Assert
            await claimRepository.Received().DeleteItemAsync(claimId);
            auditService.Received(1).AddClaimAudit(Arg.Is<ClaimAudit>(audit => audit.ClaimId == claimId && audit.HttpRequestType == "DELETE"));
        }
    }
}
