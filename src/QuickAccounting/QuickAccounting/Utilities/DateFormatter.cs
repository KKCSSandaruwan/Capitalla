using System.Globalization;

namespace QuickAccounting.Utilities
{
    public static class DateFormatter
    {
        /// <summary>
        /// Formats the specified date using a custom format string.
        /// </summary>
        /// <param name="date">The date to format.</param>
        /// <param name="format">The custom format string; defaults to general date/time pattern.</param>
        /// <param name="culture">Culture for localization; defaults to current culture.</param>
        /// <param name="dateOnly">Indicates whether to format the date as a date-only string (true) or include time (false); defaults to false.</param>
        /// <returns>A string representing the formatted date.</returns>
        public static string FormatDate(DateTime date, string? format = null, CultureInfo? culture = null, bool? dateOnly = false)
        {
            string dateFormat = dateOnly == true ? "d" : string.IsNullOrWhiteSpace(format) ? "g" : format;
            culture ??= CultureInfo.CurrentCulture; 
            return date.ToString(dateFormat, culture);
        }
    }
}
