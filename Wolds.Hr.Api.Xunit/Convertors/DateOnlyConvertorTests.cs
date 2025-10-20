using FluentAssertions;
using wolds_hr_api.Library.Converters;

namespace wolds_hr_api.xunit.Convertors;

public class DateOnlyConvertorTests
{
    private readonly DateOnlyConverter _converter = new();

    [Fact]
    public void ConvertToProvider_ShouldReturnDateTimeAtMidnight()
    {
        var dateOnly = new DateOnly(2025, 9, 24);

        var result = _converter.ConvertToProvider(dateOnly);

        result.Should().BeOfType<DateTime>();
        ((DateTime)result).Should().Be(new DateTime(2025, 9, 24, 0, 0, 0));
    }

    [Fact]
    public void ConvertFromProvider_ShouldReturnDateOnly()
    {
        var dateTime = new DateTime(2025, 9, 24, 15, 45, 30); // includes time

        var result = _converter.ConvertFromProvider(dateTime);

        result.Should().BeOfType<DateOnly>();
        ((DateOnly)result).Should().Be(new DateOnly(2025, 9, 24));
    }

    [Fact]
    public void RoundTrip_Conversion_ShouldPreserveDate()
    {
        var original = new DateOnly(2025, 9, 24);

        var providerValue = _converter.ConvertToProvider(original);
        var roundTripped = _converter.ConvertFromProvider(providerValue);

        roundTripped.Should().Be(original);
    }
}