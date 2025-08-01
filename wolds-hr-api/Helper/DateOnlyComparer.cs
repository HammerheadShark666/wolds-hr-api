﻿using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace wolds_hr_api.Helper;
public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (d1, d2) => d1 == d2,
        d => d.GetHashCode(),
        d => d)
    { }
}