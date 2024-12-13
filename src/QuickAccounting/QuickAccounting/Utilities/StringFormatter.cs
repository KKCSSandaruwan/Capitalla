namespace QuickAccounting.Utilities
{
    /// <summary>
    /// Provides utility methods for formatting strings into various formats.
    /// </summary>
    public class StringFormatter
    {
        #region Public Methods
        /// <summary>
        /// Converts a string to title case (capitalizes the first letter of each word).
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string converted to title case.</returns>
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(input.ToLower());
        }

        /// <summary>
        /// Converts a string to PascalCase (capitalize the first letter of each word without spaces).
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string converted to PascalCase.</returns>
        public static string ToPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var words = input.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return string.Empty;

            string pascalCaseString = string.Empty;
            foreach (var word in words)
            {
                pascalCaseString += char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }

            return pascalCaseString;
        }

        /// <summary>
        /// Converts a string to camel case (first letter lowercase, capitalize subsequent words).
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string in camel case. Returns an empty string if the input is null or empty.</returns>
        public static string ToCamelCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return string.Empty;

            string camelCaseString = char.ToLower(words[0][0]) + words[0].Substring(1);
            for (int i = 1; i < words.Length; i++)
            {
                camelCaseString += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }

            return camelCaseString;
        }

        /// <summary>
        /// Converts a string to snake_case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string converted to snake_case.</returns>
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            string result = input.Trim().Replace(" ", "_").Replace("-", "_");

            result = System.Text.RegularExpressions.Regex.Replace(result, @"(?<=[a-z0-9])([A-Z])", "_$1");

            return result.ToLower();
        }

        /// <summary>
        /// Converts a string to Sentence case (capitalizes the first letter of the sentence).
        /// </summary>
        public static string ToSentenceCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            input = input.Trim();
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        #endregion
    }
}
