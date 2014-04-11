using System;
using EsccWebTeam.Data.Web;

namespace eastsussexgovuk.webservices.TextXhtml.TextAreaMarkup
{
    /// <summary>
    /// Functions used when inserting formatted text
    /// </summary>
    [Obsolete("Use TinyMCE")]
    public class InsertMarkup
    {
        /// <summary>
        /// Gets the XHTML and JavaScript for a "bold" button
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public static string RenderBoldButton(string id, string imagePath)
        {
            return "<a href=\"javascript:;\" onclick=\"markup_add_strong('" + id + "');\" onmouseover=\"markup_mouseover('markup-bold', oMarkupBoldOver, false);\" onmouseout=\"markup_mouseover('markup-bold', oMarkupBoldOut, false);\" accesskey=\"b\">" +
              "<img id=\"markup-bold\" src=\"" + imagePath + "bold_off.gif\" width=\"23\" height=\"22\" alt=\"Bold\" title=\"Add some words in bold to the end of the current text. Bold text should be used only to emphasise important points, not simply for the sake of appearance (Alt+B)\" /></a>\n";
        }

        /// <summary>
        /// Gets the XHTML and JavaScript for an "italic" button
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public static string RenderItalicButton(string id, string imagePath)
        {
            return "<a href=\"javascript:;\" onclick=\"markup_add_emphasis('" + id + "');\" onmouseover=\"markup_mouseover('markup-italic', oMarkupItalicOver, false);\" onmouseout=\"markup_mouseover('markup-italic', oMarkupItalicOut, false);\" accesskey=\"i\">" +
                "<img id=\"markup-italic\" src=\"" + imagePath + "italic_off.gif\" width=\"23\" height=\"22\" alt=\"Italic\" title=\"Add some words in italics to the end of the current text. Italics should be used only to emphasise important points, not simply for the sake of appearance (Alt+I)\" /></a>\n";
        }

        /// <summary>
        /// Gets the XHTML and JavaScript for a "bullets" button
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imagePath">URL stub to the location of the button images</param>
        /// <returns></returns>
        public static string RenderBulletsButton(string id, string imagePath)
        {
            return "<a href=\"javascript:;\" onclick=\"markup_add_list('" + id + "');\" onmouseover=\"markup_mouseover('markup-bullets', oMarkupBulletsOver, false);\" onmouseout=\"markup_mouseover('markup-bullets', oMarkupBulletsOut, false);\">" +
                "<img id=\"markup-bullets\" src=\"" + imagePath + "bullets_off.gif\" width=\"23\" height=\"22\" alt=\"Bullets\" title=\"Add a bulleted list of items.\" /></a>\n";
        }

        /// <summary>
        /// Gets the XHTML and JavaScript for a "link" button
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public static string RenderLinkButton(string id, string imagePath)
        {
            return "<a href=\"javascript:;\" onclick=\"markup_add_link('" + id + "');\" onmouseover=\"markup_mouseover('markup-link', oMarkupLinkOver, false);\" onmouseout=\"markup_mouseover('markup-link', oMarkupLinkOut, false);\" accesskey=\"k\">" +
                "<img id=\"markup-link\" src=\"" + imagePath + "link_off.gif\" width=\"23\" height=\"22\" alt=\"Bold\" title=\"Add a link to a relevant web, intranet or Ezone page (Alt+K)\" /></a>\n";
        }

        /// <summary>
        /// Converts allowed XHTML markup to custom markup, and strips any other XHTML
        /// </summary>
        /// <param name="text">Text with XHTML markup</param>
        /// <returns>Text with XHTML replaced or removed</returns>
        public static string ReplaceXhtml(string text)
        {
            text = text.Replace("<b>", "[b]");
            text = text.Replace("</b>", "[/b]");
            text = text.Replace("<strong>", "[b]");
            text = text.Replace("</strong>", "[/b]");
            text = text.Replace("<i>", "[i]");
            text = text.Replace("</i>", "[/i]");
            text = text.Replace("<em>", "[i]");
            text = text.Replace("</em>", "[/i]");
            text = text.Replace("<ul>", "[list]");
            text = text.Replace("</ul>", "[/list]");
            text = text.Replace("<ol>", "[list]");
            text = text.Replace("</ol>", "[/list]");
            text = text.Replace("<li>", "[*]");
            return Html.StripTags(text);
        }
    }
}
