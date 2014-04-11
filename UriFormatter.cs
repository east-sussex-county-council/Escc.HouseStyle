using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace eastsussexgovuk.webservices.TextXhtml.HouseStyle
{
    /// <summary>
    /// Static methods to build or reformat URIs
    /// </summary>
    public static class UriFormatter
    {
        #region Reformat a URI for presentation, not linking
        /// <summary>
        /// Abridges a URI longer than 60 characters, for XHTML display purposes
        /// </summary>
        /// <param name="uri">The URI to shorten</param>
        /// <returns>A Uri with an XHTML ellipsis (...) entity reference in place of the folder structure portion of the URI</returns>
        [Obsolete("Use EsccWebTeam.Data.Web.Iri.ShortenForDisplay")]
        public static Uri ShortenUri(Uri uri)
        {
            string uriString = uri.ToString();

            if (uriString.Length > 60)
            {
                string protocol = uriString.Substring(0, uriString.IndexOf(":") + 3);
                string domain = uriString.Substring(protocol.Length, uriString.IndexOf("/", protocol.Length) - protocol.Length + 1);
                string basename = uriString.Substring(uriString.LastIndexOf("/"));
                uri = new Uri(protocol + domain + "&#8230;" + HttpUtility.HtmlEncode(basename)); // encode basename because it can include & querystring
            }

            return uri;
        }

        /// <summary>
        /// Gets a version of a URL formatted for display in XHTML, not for use as a link
        /// </summary>
        /// <param name="urlToDisplay"></param>
        /// <returns></returns>
        [Obsolete("Use EsccWebTeam.Data.Web.Iri.ShortenForDisplay")]
        public static string GetDisplayUrl(Uri urlToDisplay)
        {
            Uri displayUri = UriFormatter.ShortenUri(urlToDisplay);
            string displayText = displayUri.ToString();
            int afterProtocol = displayText.IndexOf("://");
            if (afterProtocol > 0) displayText = displayText.Substring(afterProtocol + 3);
            if (displayText.EndsWith("/")) displayText = displayText.Substring(0, displayText.Length - 1);
            return displayText;
        }
        #endregion // Reformat a URI for presentation, not linking

        #region Get standard URLs used on ESCC website

        /// <summary>
        /// Gets a URI for sending an email without exposing an email address to spambots
        /// </summary>
        /// <param name="emailAddress">The email address</param>
        /// <param name="recipientName">The name of the email recipient</param>
        public static Uri GetWebsiteEmailFormUri(string emailAddress, string recipientName)
        {
            if (emailAddress == null || emailAddress.Length == 0) return null;

            int atPos = emailAddress.IndexOf("@");
            if (atPos <= 0) return null;

            string emailAccount = emailAddress.Substring(0, atPos);
            string emailDomain = emailAddress.Substring(atPos + 1);

            return UriFormatter.GetWebsiteEmailFormUri(emailAccount, emailDomain, recipientName);
        }

        /// <summary>
        /// Gets a URI for sending an email without exposing an email address to spambots
        /// </summary>
        /// <param name="emailAccount">The part of the email address before the @</param>
        /// <param name="emailDomain">The part of the email address after the @</param>
        /// <param name="recipientName">The name of the email recipient</param>
        /// <returns>URL of an ESCC website page, or null</returns>
        public static Uri GetWebsiteEmailFormUri(string emailAccount, string emailDomain, string recipientName)
        {
            // if no dots, assume it's an internal dev box and re-use the current host
            // otherwise use the live site
            HttpContext ctx = HttpContext.Current;
            string host = (ctx != null && ctx.Request.Url.Host.IndexOf(".") == -1) ? ctx.Request.Url.Host : "www.eastsussex.gov.uk";

            // Build up URL
            StringBuilder url = new StringBuilder(Uri.UriSchemeHttp)
                .Append("://")
                .Append(host)
                .Append("/contactus/emailus/email.aspx?n=")
                .Append(ctx.Server.UrlEncode(recipientName.Replace(" & ", " and ")))
                .Append("&e=")
                .Append(emailAccount)
                .Append("&d=")
                .Append(emailDomain);

            return new Uri(url.ToString());
        }


        #endregion // Get standard URLs used on ESCC website

        #region Obfuscate email addresses
        /// <summary>
        /// Obfuscate an email address to hide it from at least the dumber spam-bots
        /// </summary>
        /// <param name="emailAddress">Email address to encode</param>
        /// <returns>Series of XHTML entity references representing the email address</returns>
        public static string ConvertEmailToEntities(string emailAddress)
        {
            return UriFormatter.ConvertEmailToEntities(emailAddress, false);
        }

        /// <summary>
        /// Obfuscate an email address to hide it from at least the dumber spam-bots
        /// </summary>
        /// <param name="emailAddress">Email address to encode</param>
        /// <param name="addMailto">if true, adds an encoded mailto: prefix to the email address</param>
        /// <returns>Series of XHTML entity references representing the email address</returns>
        public static string ConvertEmailToEntities(string emailAddress, bool addMailto)
        {
            string text = emailAddress.ToLower(CultureInfo.CurrentCulture);
            text = text.Replace(".", "&#0046;");
            text = text.Replace(":", "&#0058;");
            text = text.Replace("@", "&#0064;");
            text = text.Replace("a", "&#0097;");
            text = text.Replace("b", "&#0098;");
            text = text.Replace("c", "&#0099;");
            text = text.Replace("d", "&#0100;");
            text = text.Replace("e", "&#0101;");
            text = text.Replace("f", "&#0102;");
            text = text.Replace("g", "&#0103;");
            text = text.Replace("h", "&#0104;");
            text = text.Replace("i", "&#0105;");
            text = text.Replace("j", "&#0106;");
            text = text.Replace("k", "&#0107;");
            text = text.Replace("l", "&#0108;");
            text = text.Replace("m", "&#0109;");
            text = text.Replace("n", "&#0110;");
            text = text.Replace("o", "&#0111;");
            text = text.Replace("p", "&#0112;");
            text = text.Replace("q", "&#0113;");
            text = text.Replace("r", "&#0114;");
            text = text.Replace("s", "&#0115;");
            text = text.Replace("t", "&#0116;");
            text = text.Replace("u", "&#0117;");
            text = text.Replace("v", "&#0118;");
            text = text.Replace("w", "&#0119;");
            text = text.Replace("x", "&#0120;");
            text = text.Replace("y", "&#0121;");
            text = text.Replace("z", "&#0122;");

            if (addMailto)
            {
                string mailTo = "&#0109;&#0097;&#0105;&#0108;&#0116;&#0111;&#0058;";
                if (!text.StartsWith(mailTo)) text = mailTo + text;
            }

            return text;
        }
        #endregion // Obfuscate email addresses

        #region Parameter handling
        /// <summary>
        /// Remove any existing parameter with a specified key from a given query string
        /// </summary>
        /// <param name="qs">Exisiting query string</param>
        /// <param name="param">Parameter key</param>
        /// <returns>Modified query string, beginning with "?"</returns>
        [Obsolete("Use EsccWebTeam.Data.Web.Iri.RemoveParameterFromQueryString")]
        public static string RemoveParameter(string qs, string param)
        {
            // if supplied querystring starts with ?, remove it
            if (qs.StartsWith("?")) qs = qs.Substring(1);

            // split supplied querystring into sections
            qs = qs.Replace("&amp;", "&");
            string[] qsBits = qs.Split('&');

            //rebuild query string without its parameter= value
            StringBuilder newQS = new StringBuilder();

            for (int i = 0; i < qsBits.Length; i++)
            {
                string[] paramBits = qsBits[i].Split('=');

                if ((paramBits[0] != param) && (paramBits.Length > 1))
                {
                    if (newQS.Length > 0) newQS.Append("&amp;");
                    newQS.Append(paramBits[0]).Append("=").Append(paramBits[1]);
                }
            }

            // get querystring ready for new parameter
            if (newQS.Length > 0) newQS.Append("&amp;");
            else newQS.Append("?");

            string completeQS = newQS.ToString();
            if (!completeQS.StartsWith("?")) completeQS = "?" + completeQS;

            return completeQS;
        }
        #endregion

        #region Routing public URIs into real URIs
        /// <summary>
        ///  Convert a path of the form page1.aspx into a path of the form  page.aspx?id=1
        /// </summary>
        /// <param name="lookFor">Unique section of the path to replace, up to but not including the id</param>
        /// <param name="replaceWith">Path to replace with, up to = sign of query string parameter</param>
        /// <example>
        /// To convert /councillors/find/councillor10.aspx into /councillors/find/councillor.aspx?councillor=10
        /// <code>
        /// ConvertToHiddenQuerystring("/councillors/find/councillor", "/councillors/find/councillor.aspx?councillor=")
        /// </code>
        /// </example>
        public static void ConvertToHiddenQueryString(string lookFor, string replaceWith)
        {
            HttpContext context = HttpContext.Current;
            string oldpath = context.Request.Path.ToLower(CultureInfo.CurrentCulture);

            int i = oldpath.IndexOf(lookFor);
            int len = lookFor.Length;

            if (i != -1)
            {
                int j = oldpath.IndexOf(".aspx");
                if (j != -1 && j > (i + len))
                {
                    string id = oldpath.Substring(i + len, j - (i + len));

                    string newpath = oldpath.Replace(lookFor + id + ".aspx", replaceWith + id);
                    string qs = context.Request.QueryString.ToString();
                    if (qs.Length > 0) newpath = newpath + "&" + qs;
                    context.RewritePath(newpath);
                }
            }
        }

        /// <summary>
        /// Change the URL requested without the user seeing any change
        /// </summary>
        /// <param name="lookFor">Pattern to look for in the URL</param>
        /// <param name="replaceWith">Pattern to replace with in the URL</param>
        /// <param name="useRegularExpression">Use regular expression replacement or literal string replacement</param>
        public static void RewritePath(string lookFor, string replaceWith, bool useRegularExpression)
        {
            HttpContext context = HttpContext.Current;
            string oldPath = context.Request.Path.ToLower(CultureInfo.CurrentCulture);
            string newPath;

            if (useRegularExpression)
            {
                newPath = Regex.Replace(oldPath, lookFor, replaceWith, RegexOptions.IgnoreCase);
            }
            else
            {
                newPath = oldPath.Replace(lookFor, replaceWith);
            }

            context.RewritePath(newPath);
        }
        #endregion
    }
}
