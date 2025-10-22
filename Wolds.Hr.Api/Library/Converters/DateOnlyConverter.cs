using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Wolds.Hr.Api.Library.Converters;

internal sealed class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
        d => d.ToDateTime(TimeOnly.MinValue),
        d => DateOnly.FromDateTime(d))
    { }
}