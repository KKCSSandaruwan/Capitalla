namespace QuickAccounting.Utilities
{
    /// <summary>
    /// Provides utility methods for formatting strings into various formats.
    /// </summary>
    public class StringFormatter
    {
        #region Public Methods
        /// <summary>
        /// Converts a string to uppercase.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string converted to uppercase.</returns>
        public static string ToUpperCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return input.ToUpper();
        }

        /// <summary>
        /// Converts a string to lowercase.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string converted to lowercase.</returns>
        public static string ToLowerCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return input.ToLower();
        }

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
        /// Converts a string to kebab-case (lowercase words joined by hyphens).
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string converted to kebab-case.</returns>
        public static string ToKebabCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            string result = input.Trim().Replace(" ", "-").Replace("_", "-").Replace("-", "-");

            result = System.Text.RegularExpressions.Regex.Replace(result, @"(?<=[a-z0-9])([A-Z])", "-$1");

            return result.ToLower();
        }

        /// <summary>
        /// Converts a string to sentence case (capitalizes the first letter of the sentence).
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string converted to sentence case.</returns>
        public static string ToSentenceCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            input = input.Trim();
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        /// <summary>
        /// Reverses the characters in a string.
        /// </summary>
        /// <param name="input">The string to reverse.</param>
        /// <returns>A string with characters reversed.</returns>
        public static string Reverse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion
    }
}
