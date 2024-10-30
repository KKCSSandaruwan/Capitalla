using System.Globalization;

namespace QuickAccounting.Utilities
{
    public static class NumberFormatter
    {
        /// <summary>
        /// Formats a decimal number as currency with thousands separators and an optional currency symbol.
        /// </summary>
        /// <param name="amount">The decimal amount to format.</param>
        /// <param name="currencySymbol">Optional currency symbol to prepend.</param>
        /// <param name="decimalPlaces">Number of decimal places to display, defaulting to 2.</param>
        /// <param name="culture">Culture for localization; defaults to current culture.</param>
        /// <returns>A formatted string representing the amount in currency format.</returns>
        public static string FormatCurrency(decimal amount, string? currencySymbol = "Rs", int decimalPlaces = 2, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            var formattedAmount = amount.ToString($"N{decimalPlaces}", culture);
            return currencySymbol != null ? $"{currencySymbol} {formattedAmount}" : formattedAmount;
        }
    }
}
