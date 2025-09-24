using FluentAssertions;
using System.Text.Json;
using wolds_hr_api.Helpers.Converters;

namespace wolds_hr_api.xunit.Convertors;

public class DateOnlyJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateOnlyJsonConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new DateOnlyJsonConverter());
    }

    [Fact]
    public void Serialize_ShouldReturnFormattedDateString_WhenDateHasValue()
    {
        DateOnly? date = new DateOnly(2025, 9, 24);

        var json = JsonSerializer.Serialize(date, _options);

        json.Should().Be("\"2025-09-24\"");
    }

    [Fact]
    public void Serialize_ShouldReturnNull_WhenDateIsNull()
    {
        DateOnly? date = null;

        var json = JsonSerializer.Serialize(date, _options);

        json.Should().Be("null");
    }

    [Fact]
    public void Deserialize_ShouldReturnDateOnly_WhenValidDateString()
    {
        var json = "\"2025-09-24\"";

        var result = JsonSerializer.Deserialize<DateOnly?>(json, _options);

        result.Should().Be(new DateOnly(2025, 9, 24));
    }

    [Fact]
    public void Deserialize_ShouldReturnNull_WhenJsonIsNull()
    {
        var json = "null";

        var result = JsonSerializer.Deserialize<DateOnly?>(json, _options);

        result.Should().BeNull();
    }

    [Fact]
    public void Deserialize_ShouldThrowJsonException_WhenFormatIsInvalid()
    {
        var json = "\"09-24-2025\""; // invalid format

        Action act = () => JsonSerializer.Deserialize<DateOnly?>(json, _options);

        act.Should().Throw<JsonException>()
            .WithMessage("Invalid DateOnly format: 09-24-2025");
    }
}

