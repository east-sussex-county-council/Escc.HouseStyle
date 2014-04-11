using System;
using System.Text.RegularExpressions;

namespace eastsussexgovuk.webservices.TextXhtml.TextAreaMarkup
{
    /// <summary>
    /// Converts custom codes, used to mark up formatting in textareas, back into their XHTML equivalents. 
    /// Can be used either by passing a string into each formatting method and receiving a modified string; 
    /// or by passing in a string via the constructor or Text property, running methods without parameters, 
    /// and getting back the modified string via the Text property or the ToString() method.
    /// </summary>
    [Obsolete("Use TinyMCE")]
    public class DisplayMarkup
    {
        /// <summary>
        /// Stores string containing markup, when class is used with internal string
        /// </summary>
        private string text = "";

        /// <summary>
        /// Empty constructor allows class to be used by passing a string to each method
        /// </summary>
        public DisplayMarkup()
        {
        }

        /// <summary>
        /// Paramaterised constructor allows class to be used on an internal string using parameterless methods
        /// </summary>
        /// <param name="text"></param>
        public DisplayMarkup(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Converts simple formatting codes - bold and italic - into XHTML equivalents
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Modified string</returns>
        public string FindSimpleFormatting(string text)
        {
            if (text.Length > 0)
            {
                text = Regex.Replace(text, @"\[(/?)b]", "<$1strong>");
                text = Regex.Replace(text, @"\[(/?)i]", "<$1em>");
            }

            return text;
        }

        /// <summary>
        /// Converts simple formatting codes - bold and italic - into XHTML equivalents, acting on the internal string
        /// </summary>
        public void FindSimpleFormatting()
        {
            this.text = this.FindSimpleFormatting(this.text);
        }

        /// <summary>
        /// Converts unrodered list formatting codes into XHTML equivalents
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Modified string</returns>
        public string FindLists(string text)
        {
            if (text.Length > 0)
            {
                // spacing before unordered list
                text = Regex.Replace(text, @"([^\n])\n\[list", "$1\n\n[list");

                // unordered list
                text = Regex.Replace(text, @"\[(/?)list]\n?", "<$1ul>");

                // list items
                text = Regex.Replace(text, @"\n?\[\*] ?([^\n]+)\n?", "<li>$1</li>");
            }

            return text;
        }

        /// <summary>
        /// Converts unrodered list formatting codes into XHTML equivalents, acting on the internal string
        /// </summary>
        public void FindLists()
        {
            this.text = this.FindLists(this.text);
        }


        /// <summary>
        /// Converts hyperlink formatting codes into XHTML equivalents
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Modified string</returns>
        public string FindLinks(string text)
        {
            if (text.Length > 0)
            {
                // convert links
                text = Regex.Replace(text, @"\[link=([A-Za-z0-9():/ _.?&;=%-]+)]", "<a href=\"$1\">");
                text = Regex.Replace(text, @"\[/link]", "</a>");
            }

            return text;
        }

        /// <summary>
        /// Converts hyperlink formatting codes into XHTML equivalents, but removes those links which start with the specified string
        /// </summary>
        /// <param name="text">String to format</param>
        /// <param name="stripLinksWithPrefix">String which forms start of links to remove</param>
        /// <returns>Modified string</returns>
        /// <example>
        /// To remove links to the intranet:
        /// 
        /// <code>
        /// DisplayMarkup markup = new DisplayMarkup();
        /// markup.FindLinks(myText, "http://esccintranet");
        /// </code>
        /// </example>
        public string FindLinks(string text, string stripLinksWithPrefix)
        {
            if (text.Length > 0)
            {
                text = Regex.Replace(text, @"\[link=" + stripLinksWithPrefix + @"[A-Za-z0-9:/ _.?&;=]+]([^[]+)\[/link]", "$1");
                text = this.FindLinks(text);
            }

            return text;
        }

        /// <summary>
        /// Converts hyperlink formatting codes into XHTML equivalents, acting on the internal string
        /// </summary>
        public void FindLinks()
        {
            this.text = this.FindLinks(this.text);
        }


        /// <summary>
        /// Converts newlines into XHTML paragraphs
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Modified string</returns>
        public string FindParas(string text)
        {
            if (text.Length > 0)
            {
                // find paragraphs
                text = text.Replace("\r", "");
                text = text.Replace("\n\n", "</p><p>");
                text = text.Replace("\n", "<br />");
                text = text.Replace("</p><p>", "</p>\n\n<p>");
                text = "<p>" + text + "</p>\n";

                // text after unordered list
                text = Regex.Replace(text, "</ul>([^\n])", "</ul>\n\n<p>$1");

                // tidy up after previous replaces
                text = text.Replace("<p><br /><ul>", "<ul>\n");
                text = text.Replace("<p><ul>", "<ul>\n");
                text = text.Replace("</ul></p>", "</ul>");
                text = text.Replace("</li>", "</li>\n");
                text = text.Replace("<p><br />", "<p>"); // tidy extra lines
                text = text.Replace("<p></p>\n", "");
            }

            return text;
        }

        /// <summary>
        /// Converts newlines into XHTML paragraphs, acting on the internal string
        /// </summary>
        public void FindParas()
        {
            this.text = this.FindParas(this.text);
        }

        /// <summary>
        /// Converts email addresses into XHTML mailto: hyperlinks
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Modified string</returns>
        public string FindEmails(string text)
        {
            if (text.Length > 0)
            {
                // turn email addresses into links
                // regex is something@something.top-level-domain
                text = Regex.Replace(text, "([^\n\t<> ]+@[^\n\t<> ]+[.][a-zA-Z0-9]{1,8})", "<a href=\"mailto:$1\">$1</a>");
            }

            return text;
        }

        /// <summary>
        /// Converts email addresses into XHTML mailto: hyperlinks, acting on the internal string
        /// </summary>
        public void FindEmails()
        {
            this.text = this.FindEmails(this.text);
        }

        /// <summary>
        /// Converts plain text urls into XHTML hyperlinks
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Modified string</returns>
        public string FindWebURLs(string text)
        {
            if (text.Length > 0)
            {
                // turn web addresses into links
                text = Regex.Replace(text, "(https?://[^\n\t<>[] ]+)", "<a href=\"$1\" title=\"Go to $1\">$1</a>");
            }

            return text;
        }

        /// <summary>
        /// Converts plain text urls into XHTML hyperlinks, acting on the internal string
        /// </summary>
        public void FindWebURLs()
        {
            this.text = this.FindWebURLs(this.text);
        }

        /// <summary>
        /// Gets and sets the internal string to be worked upon. Alternative to parameterised constructor and ToString() method.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        /// <summary>
        /// Gets the internal string which is worked upon by parameterless methods.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.text;
        }
    }
}
