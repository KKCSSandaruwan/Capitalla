using System.Globalization;

namespace QuickAccounting.Utilities
{
    /// <summary>
    /// Provides utility methods for format dates and date-time values into predefined string representations. 
    /// It supports Default, Short, and Long formats and allows for culture-based localization.
    /// </summary>
    public static class DateFormatter
    {
        #region Fields
        /// <summary>
        /// Enum to specify the format types for date.
        /// </summary>
        public enum DateFormatType
        {
            Default,
            Short,
            Long
        }

        /// <summary>
        /// Represents the default date format (day/month/year).
        /// </summary>
        public const string DefaultDateFormat = "dd/MM/yyyy";

        /// <summary>
        /// Represents the short date format (day/month/two-digit year).
        /// </summary>
        public const string ShortDateFormat = "dd/MM/yy";

        /// <summary>
        /// Represents the long date format including the day of the week (day, full month name, year).
        /// </summary>
        public const string LongDateFormat = "dddd, dd/MMMM/yyyy";

        /// <summary>
        /// Represents the default date-time format (day/month/year hour:minute AM/PM).
        /// </summary>
        public const string DefaultDateTimeFormat = "dd/MM/yyyy hh:mm tt";

        /// <summary>
        /// Represents the short date-time format (day/month/two-digit year hour:minute AM/PM).
        /// </summary>
        public const string ShortDateTimeFormat = "dd/MM/yy hh:mm tt";

        /// <summary>
        /// Represents the long date-time format including the day of the week (day, full month name, year hour:minute AM/PM).
        /// </summary>
        public const string LongDateTimeFormat = "dddd, dd/MMMM/yyyy hh:mm tt";

        #endregion

        #region Public Methods
        /// <summary>
        /// Formats the specified date using one of the predefined formats (Default, Short, or Long).
        /// </summary>
        /// <param name="date">The date to format.</param>
        /// <param name="format">The format type: Default, Short, or Long. Defaults to Default.</param>
        /// <param name="culture">Culture for localization; defaults to current culture.</param>
        /// <returns>A string representing the formatted date.</returns>
        public static string FormatDate(DateTime date, DateFormatType format = DateFormatType.Default, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;

            // Determine the date format based on the provided format type
            string dateFormat = format switch
            {
                DateFormatType.Short => ShortDateFormat,
                DateFormatType.Long => LongDateFormat,
                _ => DefaultDateFormat
            };

            return date.ToString(dateFormat, culture);
        }

        /// <summary>
        /// Formats the specified date and time using one of the predefined formats (Default, Short, or Long).
        /// </summary>
        /// <param name="dateTime">The date and time to format.</param>
        /// <param name="format">The format type: Default, Short, or Long. Defaults to Default.</param>
        /// <param name="culture">Culture for localization; defaults to current culture.</param>
        /// <returns>A string representing the formatted date and time.</returns>
        public static string FormatDateTime(DateTime dateTime, DateFormatType format = DateFormatType.Default, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;

            // Determine the date-time format based on the provided format type
            string dateTimeFormat = format switch
            {
                DateFormatType.Short => ShortDateTimeFormat,
                DateFormatType.Long => LongDateTimeFormat,
                _ => DefaultDateTimeFormat
            };

            return dateTime.ToString(dateTimeFormat, culture);
        }

        #endregion
    }
}
