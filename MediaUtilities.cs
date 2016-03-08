using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EsccWebTeam.HouseStyle
{
    /// <summary>
    /// Helpers to work with HTML for common media formats
    /// </summary>
    [Obsolete("Use the alternatives recommended on each method")]
    public static class MediaUtilities
    {
        /// <summary>
        /// Recognises YouTube URLs and replaces with code for embedded video.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="embedWidth">Width of the embedded video.</param>
        /// <param name="embedHeight">Height of the embedded video.</param>
        /// <returns></returns>
        [Obsolete("Currently this is done per-site. If a shared implementation is required, build a new one using HtmlAgilityPack to parse the HTML rather than regular expressions.")]
        public static string RecogniseAndEmbedYouTubeUrl(string text, int embedWidth, int embedHeight)
        {
            text = Regex.Replace(text, "<a href=\"http://www.youtube.com/watch" + @"\?" + "v=(?<VideoId>[A-Za-z0-9_-]+)\"[^>]*>.*?</a>", "<object type=\"application/x-shockwave-flash\" width=\"" + embedWidth.ToString(CultureInfo.InvariantCulture) + "\" height=\"" + embedHeight.ToString(CultureInfo.InvariantCulture) + "\" data=\"http://www.youtube-nocookie.com/v/${VideoId}&amp;hl=en_GB&amp;fs=1&amp;rel=0\" class=\"video\"><param name=\"movie\" value=\"http://www.youtube-nocookie.com/v/$1&amp;hl=en_GB&amp;fs=1&amp;rel=0\" /><param name=\"wmode\" value=\"transparent\" /><param name=\"allowFullScreen\" value=\"true\"></param><param name=\"allowscriptaccess\" value=\"always\"></param></object>");

            text = Regex.Replace(text, "<a href=\"http://youtu.be/(?<VideoId>[A-Za-z0-9_-]+)\">.*?</a>", "<iframe width=\"" + embedWidth.ToString(CultureInfo.InvariantCulture) + "\" height=\"" + embedHeight.ToString(CultureInfo.InvariantCulture) + "\" src=\"https://www.youtube.com/embed/${VideoId}\" frameborder=\"0\" allowfullscreen=\"allowfullscreen\" class=\"video\"></iframe>");

            return text;
        }

        /// <summary>
        /// Recognises Flickr URLs and replaces with code for embedded slideshow.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="embedWidth">Width of the embedded video.</param>
        /// <param name="embedHeight">Height of the embedded video.</param>
        /// <returns></returns>
        [Obsolete("Has no effect since Flickr dropped support for iframe embedding")]
        public static string RecogniseAndEmbedFlickrUrl(string text, int embedWidth, int embedHeight)
        {
            // iframe method no longer allowed by Flickr 6 March 2013
            //text = Regex.Replace(text, "<a href=\"http://www.flickr.com/photos/[A-Za-z0-9@-]+/sets/([0-9]+)/\">.*?</a>", "<iframe width=\"" + embedWidth.ToString(CultureInfo.InvariantCulture) + "\" height=\"" + embedHeight.ToString(CultureInfo.InvariantCulture) + "\" src=\"http://www.flickr.com/slideShow/index.gne?set_id=$1\" frameborder=\"0\" allowfullscreen=\"allowfullscreen\" class=\"slideshow\"></iframe>");

            return text;
        }
    }
}
