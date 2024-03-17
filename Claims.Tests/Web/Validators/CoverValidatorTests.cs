using Claims.Web.Models;
using Claims.Web.Validators;
using Xunit;

namespace Claims.Tests.Web.Validators;
public class CoverValidatorTests
{
    private readonly CoverValidator _sut;
    private readonly DateOnly _today;
    private readonly DateOnly _yesterday;

    public CoverValidatorTests()
    {
        _sut = new CoverValidator();
        _today = DateOnly.FromDateTime(DateTime.Today);
        _yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
    }

    [Fact]
    public void StartDate_ShouldBeLessThanCurrentDate()
    {
        // Arrange
        var cover = new Cover { StartDate = _today };

        // Act
        var result = _sut.Validate(cover);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void StartDate_ShouldNotExceedOneYear()
    {
        // Arrange
        var cover = new Cover { StartDate = _yesterday, EndDate = _yesterday.AddYears(1).AddDays(1) };

        // Act
        var result = _sut.Validate(cover);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Total insurance period cannot exceed 1 year", result.Errors[0].ErrorMessage);
    }
}
