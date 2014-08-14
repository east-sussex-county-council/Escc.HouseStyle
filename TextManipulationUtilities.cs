using System;
using System.Text;
using System.Text.RegularExpressions;

namespace eastsussexgovuk.webservices.TextXhtml.TextManipulation
{
    /// <summary>
    /// Collection of static methods for manipulating strings
    /// </summary>
    public static class TextManipulationUtilities
    {
        /// <summary>
        /// Wraps legend text before a maximum number of characters per line.
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <returns>Wrapped text</returns>
        public static string WrapLines(string text)
        {
            return TextManipulationUtilities.WrapLines(text, 70);
        }

        /// <summary>
        /// Wrap text before a maximum number of characters per line
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="wrapAt">Character position to wrap at</param>
        /// <returns>Wrapped text</returns>
        public static string WrapLines(string text, int wrapAt)
        {
            StringBuilder sb = new StringBuilder();

            // remove \r to make new lines less complicated to deal with
            text = text.Replace("\r", "");

            // split text into lines to work with a line at a time
            string[] lines = text.Split('\n');
            string[] words;
            int iLine;
            int lineLength;

            for (iLine = 0; iLine < lines.Length; iLine++)
            {
                // split into words so that we don't split lines mid-word
                words = lines[iLine].Trim().Split(' ');

                // monitor how long the line we're building has become
                lineLength = 0;
                int i = 0;

                do
                {
                    // an "empty" word was a paragraph break
                    if (words[i].Length == 0)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        i++;
                        continue;
                    }

                    // If adding this word would make the line too long, start a new line
                    if ((lineLength + words[i].Length) >= wrapAt)
                    {
                        sb.Append(Environment.NewLine);
                        lineLength = 0;
                    }

                    // add the word
                    sb.Append(words[i] + " ");
                    lineLength += (words[i].Length + 1);
                    i++;
                }
                while (i < words.Length);
            }


            return sb.ToString();
        }

        /// <summary>
        /// Replace unicode entity references with their matching characters
        /// </summary>
        /// <param name="text">String containing entity references</param>
        /// <returns>String with entity references converted</returns>
        /// <remarks>
        /// This method was written to decode email addresses, hence the limited character set. It's fine to add extra characters if you need to. RM 25/07/05.
        /// </remarks>
        public static string ConvertEntitiesToCharacters(string text)
        {
            text = text.Replace("&#0046;", ".");
            text = text.Replace("&#0058;", ":");
            text = text.Replace("&#0064;", "@");
            text = text.Replace("&#0097;", "a");
            text = text.Replace("&#0098;", "b");
            text = text.Replace("&#0099;", "c");
            text = text.Replace("&#0100;", "d");
            text = text.Replace("&#0101;", "e");
            text = text.Replace("&#0102;", "f");
            text = text.Replace("&#0103;", "g");
            text = text.Replace("&#0104;", "h");
            text = text.Replace("&#0105;", "i");
            text = text.Replace("&#0106;", "j");
            text = text.Replace("&#0107;", "k");
            text = text.Replace("&#0108;", "l");
            text = text.Replace("&#0109;", "m");
            text = text.Replace("&#0110;", "n");
            text = text.Replace("&#0111;", "o");
            text = text.Replace("&#0112;", "p");
            text = text.Replace("&#0113;", "q");
            text = text.Replace("&#0114;", "r");
            text = text.Replace("&#0115;", "s");
            text = text.Replace("&#0116;", "t");
            text = text.Replace("&#0117;", "u");
            text = text.Replace("&#0118;", "v");
            text = text.Replace("&#0119;", "w");
            text = text.Replace("&#0120;", "x");
            text = text.Replace("&#0121;", "y");
            text = text.Replace("&#0122;", "z");
            return text;
        }

        /// <summary>
        /// Adds whitespace into postcode
        /// </summary>
        /// <param name="originalPostcode"></param>
        /// <returns></returns>
        public static string AddWhitespaceToPostcode(string originalPostcode)
        {


            string postcodeWithNoWhitespace = Regex.Replace(originalPostcode.ToUpper(), "\\W", "");
            string firstPartOfPostcode = "";
            string lastPartOfPostcode = "";
            string alteredPostcode = "";


            if (postcodeWithNoWhitespace.Length == 6)
            {
                firstPartOfPostcode = postcodeWithNoWhitespace.Substring(0, 3);
                lastPartOfPostcode = postcodeWithNoWhitespace.Substring(3);
                alteredPostcode = firstPartOfPostcode + " " + lastPartOfPostcode;

            }
            else if (postcodeWithNoWhitespace.Length == 7)
            {
                firstPartOfPostcode = postcodeWithNoWhitespace.Substring(0, 4);
                lastPartOfPostcode = postcodeWithNoWhitespace.Substring(4);
                alteredPostcode = firstPartOfPostcode + " " + lastPartOfPostcode;
            }
            else
            {
                return originalPostcode;
            }
            return alteredPostcode;
        }

        /// <summary>
        /// Highlights a term within an XHTML string.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string HighlightTerm(string term, string text)
        {
            // if string supplied, convert to string array
            string[] terms = new string[1];
            terms[0] = term;
            return HighlightTerms(terms, text);
        }

        /// <summary>
        /// Highlights a set of terms within an XHTML string.
        /// </summary>
        /// <param name="terms">The terms.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string HighlightTerms(string[] terms, string text)
        {
            string finalText = " " + text; // space added because term not highlighted at start of string
            string formattedText = "";
            string stringToHere = "";
            int pos;

            // loop through all search terms
            foreach (string term in terms)
            {
                // look for an instance of the term in the string
                while ((term != null) && (finalText.ToLower().IndexOf(term.ToLower()) > 0))
                {
                    // note the position of the instance
                    pos = finalText.ToLower().IndexOf(term.ToLower());

                    // ensure we're not within an HTML element
                    stringToHere = finalText.Substring(0, pos);

                    if (stringToHere.LastIndexOf(">") >= stringToHere.LastIndexOf("<"))
                    {
                        // surround instance with span tags (note: this way preserves case, unlike search and replace)
                        formattedText += (finalText.Substring(0, pos) + "<em class=\"highlight\">" + finalText.Substring(pos, term.Length) + "</em>");

                        // trim string so that we don't find the same instance again
                        finalText = finalText.Substring(pos + term.Length);
                    }
                    else
                    {
                        // if we're in a tag, go to the end of the tag
                        pos = finalText.IndexOf(">") + 1;
                        formattedText += finalText.Substring(0, pos);
                        finalText = finalText.Substring(pos);
                    }
                }

                // re-add remainder of string
                formattedText += finalText;

                // reset for next loop
                finalText = formattedText;
                formattedText = "";
            }

            return finalText.Trim(); // remove extra space
        }

    }
}
