using Claims.Application.Services;
using Claims.Core.Entities;
using Claims.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Claims.Tests.Services
{
    public class AuditServiceTests
    {
        private readonly AuditService _sut;
        private readonly IAuditRepository _auditRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuditService> _logger;

        public AuditServiceTests()
        {
            _auditRepository = Substitute.For<IAuditRepository>();
            _serviceProvider = Substitute.For<IServiceProvider>();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(_auditRepository);
            _serviceProvider = services.BuildServiceProvider();

            _logger = Substitute.For<ILogger<AuditService>>();

            _sut = new AuditService(_serviceProvider, _logger);
        }

        [Fact]
        public async Task AddCoverAudit_WhenAuditRepositoryAvailable_ShouldSaveCoverAudit()
        {
            // Arrange
            var coverAudit = new CoverAudit { CoverId = "1", HttpRequestType = "POST" };

            // Act
            _sut.AddCoverAudit(coverAudit);

            // Assert
            await Task.Delay(100); // Wait for the audit to be processed
            await _auditRepository.Received().AuditCover(coverAudit.CoverId, coverAudit.HttpRequestType);
        }

        [Fact]
        public void AddCoverAudit_WhenAuditRepositoryNotAvailable_ShouldHandlesExceptions()
        {
            // Arrange
            var coverAudit = new CoverAudit { CoverId = "1", HttpRequestType = "POST" };
            _auditRepository.AuditCover(coverAudit.CoverId, coverAudit.HttpRequestType).Throws(new Exception());

            // Act
            Exception exception = default!;
            try
            {
                _sut.AddCoverAudit(coverAudit);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Null(exception);
            _logger.Received().LogError(Arg.Any<Exception>(), "Error saving Cover audit");
        }

        [Fact]
        public async Task AddClaimAudit_WhenAuditRepositoryAvailable_ShouldSaveClaimAudit()
        {
            // Arrange
            var claimAudit = new ClaimAudit { ClaimId = "1", HttpRequestType = "POST" };

            // Act
            _sut.AddClaimAudit(claimAudit);

            // Assert
            await Task.Delay(100); // Wait for the audit to be processed
            await _auditRepository.Received().AuditClaim(claimAudit.ClaimId, claimAudit.HttpRequestType);
        }

        [Fact]
        public void AddClaimAudit_WhenAuditRepositoryNotAvailable_ShouldHandlesExceptions()
        {
            // Arrange
            var claimAudit = new ClaimAudit { ClaimId = "1", HttpRequestType = "POST" };
            _auditRepository.AuditClaim(claimAudit.ClaimId, claimAudit.HttpRequestType).Throws(new Exception());

            // Act
            Exception exception = default!;
            try
            {
                _sut.AddClaimAudit(claimAudit);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Null(exception);
            _logger.Received().LogError(Arg.Any<Exception>(), "Error saving Claim audit");
        }
    }
}
