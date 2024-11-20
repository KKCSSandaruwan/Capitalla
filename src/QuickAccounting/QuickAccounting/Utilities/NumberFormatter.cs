namespace QuickAccounting.Utilities
{
    /// <summary>
    /// Provides utility methods for formatting decimal numbers into various formats.
    /// The formats include percentage, thousands separator and optional decimal places, and scientific notation.
    /// </summary>
    public static class NumberFormatter
    {
        #region Public Methods
        /// <summary>
        /// Formats a decimal number as a percentage.
        /// </summary>
        /// <param name="number">The decimal number to format.</param>
        /// <param name="decimalPlaces">The number of decimal places to include (optional, default is 2).</param>
        /// <returns>A string representing the number as a percentage.</returns>
        public static string FormatPercentage(decimal number, int? decimalPlaces = 2)
        {
            return string.Format("{0:P" + decimalPlaces + "}", number);
        }

        /// <summary>
        /// Formats a decimal number with a thousands separator and an optional number of decimal places.
        /// </summary>
        /// <param name="number">The decimal number to format.</param>
        /// <param name="decimalPlaces">The number of decimal places to include (optional, default is 2).</param>
        /// <returns>A string representing the number with thousands separators and the specified number of decimal places.</returns>
        public static string FormatThousandsSeparator(decimal number, int? decimalPlaces = 2)
        {
            return string.Format("{0:N" + decimalPlaces + "}", number);
        }

        /// <summary>
        /// Formats a decimal number in scientific notation.
        /// </summary>
        /// <param name="number">The decimal number to format.</param>
        /// <returns>A string representing the number in scientific notation.</returns>
        public static string FormatScientific(decimal number)
        {
            return number.ToString("E");
        }

        #endregion
    }
}
