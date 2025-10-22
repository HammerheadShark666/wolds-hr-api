using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Wolds.Hr.Api.Library.Converters;

internal sealed class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (d1, d2) => d1 == d2,
        d => d.GetHashCode(),
        d => d)
    { }
}