
using System;
using System.Globalization;

namespace Artaxias.Core.Extensions
{
    public static class DatetimeExtensions
    {
        public static string ToDomainStringDate(this DateTime date)
        {
            return date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
        }
    }
}
