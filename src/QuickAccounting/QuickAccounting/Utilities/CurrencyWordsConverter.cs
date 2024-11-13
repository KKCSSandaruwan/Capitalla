namespace QuickAccounting.Utilities
{
    /// <summary>
    /// Provides utility methods for convert decimal numbers into their word representations. 
    /// It supports converting both the integer and fractional parts of the number, making it suitable for monetary values.
    /// The conversion includes the currency representation in "Rupees" and "Cents". The class handles values up to billions
    /// and formats them in a human-readable manner.
    /// </summary>
    public static class CurrencyWordsConverter
    {
        #region Fields
        private static readonly string[] UnitWords =
            { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };

        private static readonly string[] TeenWords =
            { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

        private static readonly string[] TensWords =
            { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private static readonly string[] ThousandsGroups =
            { "", "Thousand", "Million", "Billion" };

        private static readonly string[] CurrencyUnits = 
            { "Rupees", "Cents" };

        #endregion

        #region Public Methods
        /// <summary>
        /// Converts a decimal number to its word representation, suitable for monetary values.
        /// </summary>
        /// <param name="number">The decimal number to be converted.</param>
        /// <returns>The word representation of the number.</returns>
        public static string ConvertNumberToWords(decimal number)
        {
            if (number == 0)
                return "Zero Only";

            int integerPart = (int)Math.Floor(number);
            int fractionalPart = (int)((number - integerPart) * 100);

            string words = $"{ConvertIntegerToWords(integerPart)} {CurrencyUnits[0]}";

            if (fractionalPart > 0)
            {
                words += $" and {ConvertIntegerToWords(fractionalPart)} {CurrencyUnits[1]}";
            }

            words += " Only";

            return words;
        }

        #endregion

        #region Private Helper Methods
        private static string ConvertIntegerToWords(int number)
        {
            if (number == 0)
                return "";

            int thousandGroupIndex = 0;
            string words = "";

            while (number > 0)
            {
                int groupChunk = number % 1000;
                if (groupChunk > 0)
                {
                    string chunkWords = ConvertChunkToWords(groupChunk);
                    words = chunkWords + " " + ThousandsGroups[thousandGroupIndex] + " " + words;
                }

                number /= 1000;
                thousandGroupIndex++;
            }

            return words.Trim();
        }

        private static string ConvertChunkToWords(int number)
        {
            string chunkWords = "";

            if (number >= 100)
            {
                chunkWords += UnitWords[number / 100] + " Hundred ";
                number %= 100;
            }

            if (number >= 20)
            {
                chunkWords += TensWords[number / 10] + " ";
                number %= 10;
            }
            else if (number >= 10)
            {
                chunkWords += TeenWords[number - 10] + " ";
                number = 0;
            }

            if (number > 0)
            {
                chunkWords += UnitWords[number] + " ";
            }

            return chunkWords.Trim();
        }

        #endregion
    }
}
