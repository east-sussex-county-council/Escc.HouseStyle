using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using eastsussexgovuk.webservices.TextXhtml.HouseStyle;
using EsccWebTeam.Gdsc;
using EsccWebTeam.HouseStyle.Properties;

namespace EsccWebTeam.HouseStyle
{
    /// <summary>
    /// Standard display of contact information
    /// </summary>
    /// <remarks>This contains an incomplete version of the hCard microformat (http://microformats.org/wiki/hcard). 
    /// Work to do includes applying hCard data to the BS7666 address when it's converted to a simple address, and confirming that the work telephone number is picked up.</remarks>
    public class ContactInfoControl : WebControl, INamingContainer
    {
        #region Private member vars
        private BS7666Address bs7666Address;
        private string fax;
        private string emailAddress;
        private Uri website;
        private Uri addressUrl;
        private bool showMap;
        private string name;
        private string description;
        private string emailText;
        private bool useEmailForm;
        private string addressUrlText;
        private string websiteText;
        private string pageDisplayName;
        private Uri pageUrl;
        private Uri mapUrl;
        private UKContactNumberCollection telephones = new UKContactNumberCollection();

        #endregion // Private member vars

        #region Properties for who or what to contact
        /// <summary>
        /// Gets or sets a description of the contactable person or entity
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the contactable person or entity
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        #endregion // Properties for who or what to contact

        #region Properties for street address
        /// <summary>
        /// Gets or sets a street address
        /// </summary>
        public BS7666Address BS7666Address
        {
            get
            {
                return this.bs7666Address;
            }
            set
            {
                this.bs7666Address = value;
            }
        }

        /// <summary>
        /// Gets or sets the clickable text of the URL specified in <seealso cref="AddressUrl"/>
        /// </summary>
        public string AddressUrlText
        {
            get
            {
                return this.addressUrlText;
            }
            set
            {
                this.addressUrlText = value;
            }
        }

        /// <summary>
        /// Gets or sets an alternative URL where contact information is available
        /// </summary>
        /// <remarks>Addresses supplied by Hastings BC are one-liners linking to full address info stored elsewhere</remarks>
        public Uri AddressUrl
        {
            get
            {
                return this.addressUrl;
            }
            set
            {
                this.addressUrl = value;
            }
        }

        #endregion // Properties for street address

        #region Properties used to build map link
        /// <summary>
        /// Gets or sets whether to show a link to a map on the ESCC website
        /// </summary>
        public bool ShowMap
        {
            get
            {
                return this.showMap;
            }
            set
            {
                this.showMap = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the current page
        /// </summary>
        public Uri PageUrl
        {
            get
            {
                return this.pageUrl;
            }
            set
            {
                this.pageUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the display name of the current page
        /// </summary>
        public string PageDisplayName
        {
            get
            {
                return this.pageDisplayName;
            }
            set
            {
                this.pageDisplayName = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the map to link to (only set if you need to override the default)
        /// </summary>
        public Uri MapUrl
        {
            get
            {
                return this.mapUrl;
            }
            set
            {
                this.mapUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the text of the map link.
        /// </summary>
        /// <value>The map link text.</value>
        public string MapLinkText { get; set; }

        #endregion // Properties used to build map link

        #region Properties for phone and fax
        /// <summary>
        /// Gets or sets a fax number
        /// </summary>
        public string Fax
        {
            get
            {
                return this.fax;
            }
            set
            {
                this.fax = value;
            }
        }

        /// <summary>
        /// Gets the telephone numbers.
        /// </summary>
        /// <value>The telephone numbers.</value>
        public UKContactNumberCollection TelephoneNumbers { get { return this.telephones; } }

        /// <summary>
        /// Gets or sets the first telephone number
        /// </summary>
        /// <remarks>This is for backwards compatibility, from before multiple telephone numbers were supported</remarks>
        public string Telephone
        {
            get
            {
                return (this.telephones.Count > 0) ? this.telephones[0].NationalNumber : null;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    // treat as request to remove the first phone number
                    if (this.telephones.Count > 0) this.telephones.Remove(this.telephones[0]);
                }
                else if (this.telephones.Count > 0)
                {
                    // replace the first number
                    this.telephones[0].NationalNumber = value;
                }
                else
                {
                    // add as new number
                    this.telephones.Add(new UKContactNumber(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to use a more compact format for mobiles.
        /// </summary>
        /// <value><c>true</c> to use mobile format; otherwise, <c>false</c>.</value>
        /// <remarks>House style says the display can be varied where space is restricted on the web</remarks>
        public bool Mobile { get; set; }

        #endregion // Properties for phone and fax

        #region Properties for email and website
        /// <summary>
        /// Gets or sets the clickable text of the website link specified in <seealso cref="Website"/>
        /// </summary>
        public string WebsiteText
        {
            get
            {
                return this.websiteText;
            }
            set
            {
                this.websiteText = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to link to an email form, rather than an actual email address
        /// </summary>
        public bool UseEmailForm
        {
            get
            {
                return this.useEmailForm;
            }
            set
            {
                this.useEmailForm = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the email recipient
        /// </summary>
        public string EmailText
        {
            get
            {
                return this.emailText;
            }
            set
            {
                this.emailText = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of a website 
        /// </summary>
        public Uri Website
        {
            get
            {
                return this.website;
            }
            set
            {
                this.website = value;
            }
        }

        /// <summary>
        /// Gets or sets an email address
        /// </summary>
        public string EmailAddress
        {
            get
            {
                return this.emailAddress;
            }
            set
            {
                this.emailAddress = value;
            }
        }

        #endregion // Properties for email and website

        #region Constructor
        /// <summary>
        /// Standard display of contact information
        /// </summary>
        public ContactInfoControl()
            : base(HtmlTextWriterTag.Div)
        {
            this.showMap = true;
            if (HttpContext.Current != null) this.pageUrl = HttpContext.Current.Request.Url;
            this.MapLinkText = Resources.MapLinkText;
        }
        #endregion // Constructor

        #region Helpers for building up child controls collection
        /// <summary>
        /// Adds an XHTML line break to the controls collection
        /// </summary>
        /// <param name="parentControl">Control to add new line to</param>
        private static void StartOnNewLine(Control parentControl)
        {
            if (parentControl.Controls.Count > 0) parentControl.Controls.Add(new LiteralControl("<br />"));
        }

        /// <summary>
        /// Expands expected abbreviations using XHTML &lt;abbr /&gt; element
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string ExpandAbbreviations(string text)
        {
            string lookFor = " " + Resources.AbbrExtension + " ";
            string replaceWith = String.Format(CultureInfo.CurrentCulture, "<abbr title=\"{0}\">{1}</abbr>", Resources.AbbrFullExtension, Resources.AbbrExtension);
            return text.Replace(lookFor, replaceWith);
        }

        /// <summary>
        /// Adds a string of XHTML to the controls collection
        /// </summary>
        /// <param name="xhtml">XHTML string to add</param>
        /// <param name="parentControl">Control to add it to</param>
        private static void AddString(string xhtml, Control parentControl)
        {
            ContactInfoControl.AddString(xhtml, parentControl, String.Empty);
        }

        /// <summary>
        /// Adds a string of XHTML to the controls collection
        /// </summary>
        /// <param name="xhtml">XHTML string to add</param>
        /// <param name="parentControl">Control to add it to</param>
        /// <param name="hCardClass">The relevant CSS class name for the hCard microformat.</param>
        private static void AddString(string xhtml, Control parentControl, string hCardClass)
        {
            if (hCardClass.Length > 0)
            {
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes["class"] = hCardClass;
                span.InnerHtml = xhtml;
                parentControl.Controls.Add(span);
            }
            else
            {
                parentControl.Controls.Add(new LiteralControl(xhtml));
            }
        }

        #endregion // Helpers for building up child controls collection

        #region CreateChildControls

        /// <summary>
        /// Build the contact details into two paras of info
        /// </summary>
        protected override void CreateChildControls()
        {
            // Remember we can't be sure if any property will be supplied, but for hCard
            // we must label something as the "fn"...
            bool fnFound = false;

            HtmlGenericControl part1 = new HtmlGenericControl("p");
            HtmlGenericControl part2 = new HtmlGenericControl("p");

            // Add name
            if (this.name != null && this.name.Length > 0)
            {
                ContactInfoControl.AddString(HttpUtility.HtmlEncode(this.name), part1, "fn");
                fnFound = true;
            }

            // Add description
            if (this.description != null && this.description.Length > 0)
            {
                ContactInfoControl.StartOnNewLine(part1);
                if (fnFound)
                {
                    ContactInfoControl.AddString(HttpUtility.HtmlEncode(this.Description).Replace(Environment.NewLine, "<br />"), part1);
                }
                else
                {
                    ContactInfoControl.AddString(HttpUtility.HtmlEncode(this.Description).Replace(Environment.NewLine, "<br />"), part1, "fn");
                    fnFound = true;
                }
            }

            // street address
            if (this.bs7666Address != null)
            {
                // Get XHTML of correctly-cased address
                if (this.bs7666Address.HasAddress())
                {
                    ContactInfoControl.StartOnNewLine(part1);
                    SimpleAddressControl addr = new SimpleAddressControl(this.bs7666Address); // control implements hCard/adr microformat
                    addr.CssClass = "location"; // hCalendar
                    addr.Separator = SimpleAddressControl.SeparatorHCalendar;
                    part1.Controls.Add(addr);

                    // link to map
                    if (this.showMap)
                    {
                        string mapPageUrl = null;
                        if (this.mapUrl != null) mapPageUrl = this.mapUrl.ToString();
                        else
                        {
                            // handle backToUrl separately to ensure the page is valid. GetWebsiteMapUri correctly UrlEncodes it, 
                            // but it returns it as a Uri object which UrlDecodes it again
                            mapPageUrl = GetWebsiteMapUrl(this.bs7666Address, this.pageDisplayName, this.PageUrl);
                            //if (gisMapUrl != null) gisMapUrl.ToString() + "&backToUrl=" + HttpUtility.UrlEncode(.ToString());
                        }
                        if (mapPageUrl != null)
                        {
                            ContactInfoControl.StartOnNewLine(part1);
                            HtmlAnchor mapLink = new HtmlAnchor();
                            mapLink.InnerText = this.MapLinkText;
                            mapLink.HRef = HttpUtility.HtmlEncode(mapPageUrl);
                            mapLink.Attributes["data-unpublished"] = "false"; // disable website CMS warning about internal links
                            part1.Controls.Add(mapLink);
                        }
                    }
                }
            }

            // phones property (house style doesn't say how to present multiple phone numbers)
            int phonesDone = 0;
            foreach (var phone in this.telephones)
            {
                if (phonesDone == 0) ContactInfoControl.StartOnNewLine(part2);

                HtmlControl telContainer = null;
                try
                {
                    if (Mobile)
                    {
                        telContainer = new HtmlAnchor();
                        ((HtmlAnchor)telContainer).HRef = "tel:" + phone.ToString();
                    }
                    else
                    {
                        telContainer = new HtmlGenericControl("span");
                    }

                    telContainer.Attributes["class"] = "tel"; // hCard

                    // always label phone number if there's also a fax number
                    if (!Mobile || !String.IsNullOrEmpty(this.fax))
                    {
                        if (phonesDone == 0)
                        {
                            ContactInfoControl.AddString(Resources.TelephonePrefix, telContainer);
                            ContactInfoControl.AddString(Resources.ContactPrefixSeparator, telContainer);
                        }
                    }
                    if (phonesDone > 0)
                    {
                        ContactInfoControl.AddString(" or ", part2);
                    }
                    ContactInfoControl.AddString(this.ExpandAbbreviations(HouseStyleFormatter.FormatTelephone(phone.NationalNumber)), telContainer, "value");
                    part2.Controls.Add(telContainer);
                }
                finally
                {
                    telContainer.Dispose();
                }

                phonesDone++;
            }

            // fax property
            if (this.fax != null && this.fax.Length > 0)
            {
                ContactInfoControl.StartOnNewLine(part2);
                HtmlGenericControl faxContainer = new HtmlGenericControl("span");
                faxContainer.Attributes["class"] = "tel"; // hCard
                part2.Controls.Add(faxContainer);
                ContactInfoControl.AddString(Resources.FaxPrefix, faxContainer, "type");
                ContactInfoControl.AddString(Resources.ContactPrefixSeparator, faxContainer);
                ContactInfoControl.AddString(this.ExpandAbbreviations(HouseStyleFormatter.FormatTelephone(this.fax)), faxContainer, "value");
            }

            // email property
            if (this.emailAddress != null && this.emailAddress.Length > 0)
            {
                ContactInfoControl.StartOnNewLine(part2);

                // Clickable text can be email address or custom text
                bool visibleAddress = (this.emailText == null || this.emailText.Length == 0);

                // Don't use HtmlAnchor because in ASP.NET 4 it URLEncodes the hashes used in entities, 
                // making the result invalid XML which in turn breaks parsing of the page 
                string emailText = String.Empty;
                string emailHref = String.Empty;
                string emailClass = String.Empty;
                if (visibleAddress)
                {
                    emailText = UriFormatter.ConvertEmailToEntities(this.emailAddress);
                }
                else
                {
                    emailText = HttpUtility.HtmlEncode(Resources.EmailPrefix + " " + this.emailText);
                }

                // Link can be to email address, or to email form
                if (this.useEmailForm)
                {
                    Uri formUrl;
                    if (visibleAddress)
                    {
                        formUrl = UriFormatter.GetWebsiteEmailFormUri(this.emailAddress, this.emailAddress, Context.Request.Url);
                    }
                    else
                    {
                        formUrl = UriFormatter.GetWebsiteEmailFormUri(this.emailAddress, this.emailText, Context.Request.Url);
                    }
                    if (formUrl != null)
                    {
                        emailHref = HttpUtility.HtmlEncode(formUrl.ToString());
                    }
                    else
                    {
                        emailHref = UriFormatter.ConvertEmailToEntities(this.emailAddress, true);
                    }
                }
                else
                {
                    emailHref = UriFormatter.ConvertEmailToEntities(this.emailAddress, true);
                    emailClass = "email"; // hCard
                }

                if (!fnFound)
                {
                    emailClass = (emailClass + " fn").TrimStart();
                    fnFound = true;
                }

                // Only use prefix if email address is being shown
                if (visibleAddress && !Mobile) ContactInfoControl.AddString(Resources.EmailPrefix + Resources.ContactPrefixSeparator, part2);
                string emailLink = "<a href=\"" + emailHref + "\"";
                if (!String.IsNullOrEmpty(emailClass)) emailLink += " class=\"" + emailClass + "\"";
                emailLink += ">" + emailText + "</a>";
                part2.Controls.Add(new LiteralControl(emailLink));
            }

            if (this.website != null && this.website.ToString().Length > 0)
            {
                ContactInfoControl.StartOnNewLine(part2);

                // Display text can be the URL or custom text
                bool showWebUrl = (this.websiteText == null || this.websiteText.Length == 0);

                // Different prefix depending on use of URL or custom text
                if (!Mobile)
                {
                    if (showWebUrl)
                    {
                        ContactInfoControl.AddString(Resources.WebsitePrefix, part2);
                    }
                    else
                    {
                        ContactInfoControl.AddString(Resources.WebsiteLeadIn, part2);
                    }
                }

                // Build link
                HtmlAnchor webLink = new HtmlAnchor();
                webLink.HRef = HttpUtility.HtmlAttributeEncode(this.website.ToString());
                webLink.Attributes["class"] = "url";
                if (showWebUrl)
                {
                    webLink.InnerHtml = EsccWebTeam.Data.Web.Iri.ShortenForDisplay(this.website);

                }
                else
                {
                    webLink.InnerText = this.websiteText;
                }
                part2.Controls.Add(webLink);
            }

            // address URL
            if (this.addressUrl != null && this.addressUrl.ToString().Length > 0)
            {
                ContactInfoControl.StartOnNewLine(part2);
                HtmlAnchor addrLink = new HtmlAnchor();
                addrLink.HRef = this.addressUrl.ToString();

                if (this.addressUrlText != null && this.addressUrlText.Length > 0)
                {
                    addrLink.InnerText = this.addressUrlText;
                }
                else
                {
                    addrLink.InnerText = this.addressUrl.ToString();
                }
                part2.Controls.Add(addrLink);
            }

            if (part1.Controls.Count > 0) this.Controls.Add(part1);
            if (part2.Controls.Count > 0) this.Controls.Add(part2);

            // hCard microformat requires "fn" property. If we marked up a suitable value, set this control to be an hCard.
            if (fnFound)
            {
                this.Attributes["class"] = "vcard";
            }
        }

        /// <summary>
        /// Gets a URL for displaying a map on the website of the given address
        /// </summary>
        /// <param name="address">The address to show a map of</param>
        /// <param name="displayName">The display name of the page to link from</param>
        /// <param name="returnUrl">The URL to return to, if not the current page</param>
        /// <returns>URI of a page which will display the map, or null if the map is not available</returns>
        /// <remarks>returnUrl property is useful for CMS, where the URL of the current page is translated on-the-fly</remarks>
        private static string GetWebsiteMapUrl(BS7666Address address, string displayName, Uri returnUrl)
        {
            if (address == null || address.Postcode == null || address.Postcode.Length == 0) return null;
            if (String.IsNullOrEmpty(address.AdministrativeArea) || String.Compare(address.AdministrativeArea.ToLowerInvariant(), "east sussex", true, CultureInfo.CurrentCulture) != 0) return null;
            if (address.Town != null && address.Town.Length > 0 && address.Town.ToLowerInvariant().IndexOf("brighton") > -1) return null;

            // if no dots, assume it's an internal dev box and re-use the current host
            // otherwise use the live site
            HttpContext ctx = HttpContext.Current;
            string host = (ctx != null && ctx.Request.Url.Host.IndexOf(".") == -1) ? ctx.Request.Url.Host : "www.eastsussex.gov.uk";

            // Build up URL
            StringBuilder url = new StringBuilder(Uri.UriSchemeHttp).Append("://").Append(host).Append("/contactus/map.aspx");

            url.Append("?postcode=").Append(HttpUtility.UrlEncode(address.Postcode));
            url.Append("&e=").Append(HttpUtility.UrlEncode(address.GridEasting.ToString(CultureInfo.CurrentCulture)));
            url.Append("&n=").Append(HttpUtility.UrlEncode(address.GridNorthing.ToString(CultureInfo.CurrentCulture)));

            if (address.Paon != null && address.Paon.Length > 0) url.Append("&paon=").Append(HttpUtility.UrlEncode(address.Paon));

            url.Append("&mapOf=").Append(HttpUtility.UrlEncode(HttpUtility.HtmlDecode(address.GetSimpleAddress().ToString())));

            if (returnUrl != null) url.Append("&backToUrl=").Append(HttpUtility.UrlEncode(returnUrl.ToString()));

            if (displayName != null && displayName.Length > 0) url.Append("&backTo=").Append(HttpUtility.UrlEncode(displayName));

            return url.ToString();
        }

        /// <summary>
        /// Creates the child controls on demand
        /// </summary>
        /// <remarks>Used when creating the control outside the context of a web page, eg to create an RSS feed</remarks>
        public void CreateControls()
        {
            this.EnsureChildControls();
        }
        #endregion // CreateChildControls

    }
}
