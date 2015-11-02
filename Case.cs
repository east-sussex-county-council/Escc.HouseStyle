using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EsccWebTeam.HouseStyle
{
    /// <summary>
    /// Collection of static methods to manipulate the case of text strings.
    /// </summary>
    [Obsolete("Use http://humanizr.net/ via NuGet")]
    public static class Case
    {
        /// <summary>
        /// Convert the first character of a string to uppercase.
        /// </summary>
        /// <param name="text">Any string</param>
        /// <returns>Modified string</returns>
        public static string FirstToUpper(string text)
        {
            // check we have a text string to work with
            if (text != null && text.Length > 1)
            {
                // uppercase the first character; tack on the rest, unchanged
                text = text.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + text.Substring(1);
            }

            return text;
        }

        /// <summary>
        /// Convert "pascalCasedText" to "Title Cased Text"
        /// </summary>
        /// <param name="text">Text in Pascal case</param>
        /// <returns>Text in title case</returns>
        public static string PascalToTitle(string text)
        {
            text = Case.FirstToUpper(text);
            text = Regex.Replace(text, "([A-Z])", " $1");
            return Regex.Replace(text, "([A-Za-z])([0-9])", "$1 $2");
        }


        /// <summary>
        /// Convert text to Title Case
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToTitleCase(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower(CultureInfo.CurrentCulture));
        }
    }
}
