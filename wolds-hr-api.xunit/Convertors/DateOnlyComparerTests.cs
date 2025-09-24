using FluentAssertions;
using wolds_hr_api.Helper.Converters;

namespace wolds_hr_api.xunit.Convertors;

public class DateOnlyComparerTests
{
    private readonly DateOnlyComparer _comparer = new();

    [Fact]
    public void Equals_ShouldReturnTrue_WhenDatesAreEqual()
    {
        var date1 = new DateOnly(2025, 9, 24);
        var date2 = new DateOnly(2025, 9, 24);

        var result = _comparer.Equals(date1, date2);

        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDatesAreDifferent()
    {
        var date1 = new DateOnly(2025, 9, 24);
        var date2 = new DateOnly(2025, 9, 25);

        var result = _comparer.Equals(date1, date2);

        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForEqualDates()
    {
        var date1 = new DateOnly(2025, 9, 24);
        var date2 = new DateOnly(2025, 9, 24);

        var hash1 = _comparer.GetHashCode(date1);
        var hash2 = _comparer.GetHashCode(date2);

        hash1.Should().Be(hash2);
    }

    [Fact]
    public void GetHashCode_ShouldBeDifferent_ForDifferentDates()
    {
        var date1 = new DateOnly(2025, 9, 24);
        var date2 = new DateOnly(2025, 9, 25);

        var hash1 = _comparer.GetHashCode(date1);
        var hash2 = _comparer.GetHashCode(date2);

        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void Snapshot_ShouldReturnSameDate()
    {
        var date = new DateOnly(2025, 9, 24);

        var snapshot = _comparer.Snapshot(date);

        snapshot.Should().Be(date);
    }
}