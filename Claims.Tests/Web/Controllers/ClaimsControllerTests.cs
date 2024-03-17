using AutoMapper;
using Claims.Core.Entities;
using Claims.Core.Services;
using Claims.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Claims.Tests.Web.Controllers
{
    public class ClaimsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ClaimsController _sut;
        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimService _claimService;

        public ClaimsControllerTests()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _logger = Substitute.For<ILogger<ClaimsController>>();
            _claimService = Substitute.For<IClaimService>();
            _mapper = mapperConfig.CreateMapper();

            _sut = new ClaimsController(_logger, _claimService, _mapper);
        }

        [Fact]
        public async Task GetClaims_WhenClaimsExist_ReturnsValidContent()
        {
            // Arrange
            var claims = new List<Claim> {
                new Claim
                { Id = "1", CoverId = "1", Created = DateTime.Now, DamageCost = 1, Name = "Test Claim", Type = Core.Enums.ClaimType.BadWeather },
                new Claim
                { Id = "2", CoverId = "2", Created = DateTime.Now.AddDays(-1), DamageCost = 2, Name = "Test Claim 2", Type = Core.Enums.ClaimType.Fire }
            };
            _claimService.GetAsync().Returns(claims);

            // Act
            var results = (await _sut.GetAsync()).ToList();

            // Assert
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(claims[i].Name, results[i].Name);
                Assert.Equal(claims[i].CoverId, results[i].CoverId);
                Assert.Equal(claims[i].Created, results[i].Created);
                Assert.Equal(claims[i].DamageCost, results[i].DamageCost);
                Assert.Equal(claims[i].Type, results[i].Type);
            }
        }

        [Fact]
        public async Task GetClaim_WhenClaimExists_ReturnsValidContentAndStatusCode()
        {
            // Arrange
            var claim = new Claim
            {
                Id = "1",
                CoverId = "1",
                Created = DateTime.Now.AddDays(-1),
                DamageCost = 1,
                Name = "Test Claim 1",
                Type = Core.Enums.ClaimType.Fire
            };

            _claimService.GetAsync(claim.Id).Returns(claim);

            // Act
            var result = (OkObjectResult)(await _sut.GetAsync("1")).Result!;
            var resultClaim = (Claims.Web.Models.Claim)result.Value!;

            // Assert

            Assert.Equal(claim.Name, resultClaim.Name);
            Assert.Equal(claim.CoverId, resultClaim.CoverId);
            Assert.Equal(claim.Created, resultClaim.Created);
            Assert.Equal(claim.DamageCost, resultClaim.DamageCost);
            Assert.Equal(claim.Type, resultClaim.Type);
            Assert.Equal(200, result!.StatusCode!.Value);
        }

        [Fact]
        public async Task GetClaim_WhenClaimNotExists_ReturnsNotFoundStatusCode()
        {
            // Arrange
            _claimService.GetAsync().ReturnsNull();

            // Act
            var result = (NotFoundResult)(await _sut.GetAsync("1")).Result!;

            // Assert
            Assert.Equal(404, result!.StatusCode);
        }
    }
}