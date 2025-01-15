namespace QuickAccounting.Utilities
{
    /// <summary>
    /// Provides utility methods for formatting decimal numbers as currency,
    /// converting numbers to words for currency, and other related operations.
    /// </summary>
    public class CurrencyFormatter
    {
        #region Fields
        /// <summary>
        /// Represents the symbol used for the currency. In this case, it is the symbol for Rupees (Rs).
        /// </summary>
        public static readonly string CurrencySymbol = "Rs";

        #endregion

        #region Public Methods
        /// <summary>
        /// Formats a decimal number as a currency with the currency symbol and specified decimal places.
        /// </summary>
        /// <param name="number">The decimal number to format.</param>
        /// <param name="decimalPlaces">The number of decimal places to include (optional, default is 2).</param>
        /// <returns>A string representing the number formatted as currency with the symbol.</returns>
        public static string FormatCurrency(decimal number, int? decimalPlaces = 2)
        {
            return $"{CurrencySymbol} {string.Format("{0:N" + decimalPlaces + "}", number)}";
        }

        #endregion
    }
}
