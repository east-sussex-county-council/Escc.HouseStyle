using System;
using System.Globalization;

namespace eastsussexgovuk.webservices.TextXhtml.HouseStyle
{
    /// <summary>
    /// Methods for formatting numbers according to house style
    /// </summary>
    public static class NumberFormatter
    {
        /// <summary>
        /// Gets text representing the number formatted according to house style
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetNumber(int number)
        {
            switch (number)
            {
                case 0:
                    return "Zero";
                case 1:
                    return "One";
                case 2:
                    return "Two";
                case 3:
                    return "Three";
                case 4:
                    return "Four";
                case 5:
                    return "Five";
                case 6:
                    return "Six";
                case 7:
                    return "Seven";
                case 8:
                    return "Eight";
                case 9:
                    return "Nine";
                default:
                    return number.ToString(CultureInfo.CurrentCulture);
            }
        }
    }
}
