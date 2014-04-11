using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace EsccWebTeam.HouseStyle
{
    /// <summary>
    /// A box with rounded corners
    /// </summary>
    /// <remarks>This control expects to find a CSS class called <code><b>roundedBox</b></code> 
    /// which uses 4 divs and four corner graphics to create a rounded box.</remarks>
    [Obsolete("Use the CSS border-radius property")]
    public class RoundedBox : PlaceHolder, INamingContainer
    {
        private string cssClass;

        /// <summary>
        /// Gets or sets the CSS class to apply to the control
        /// </summary>
        public string CssClass
        {
            get
            {
                return this.cssClass;
            }
            set
            {
                this.cssClass = value;
            }
        }

        /// <summary>
        /// Render container divs around child elements
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            // if there are no child controls, or there are only invisible controls, 
            // there's no point rendering the box
            bool childrenFound = false;
            foreach (Control control in Controls)
            {
                if (control.Visible == true)
                {
                    childrenFound = true;
                    break;
                }
            }
            if (!childrenFound) return;

            // do we need to add a fourth div, or is there one which can be used?

            // first check is for controls added in code-behind, or .NET 2
            bool divNeeded = (this.Controls.Count == 0 || this.Controls[0].GetType() != typeof(HtmlGenericControl) || (this.Controls[0] as HtmlGenericControl).TagName.ToLower() != "div");

            // check again for controls added declaratively
            if (divNeeded)
            {
                LiteralControl contents = this.Controls[0] as LiteralControl;
                if (contents != null && (contents.Text.StartsWith("<div>") || contents.Text.StartsWith("<div "))) divNeeded = false;
            }

            // write the outer div with the roundedBox CSS class
            writer.Write("<div class=\"roundedBox");
            if (this.cssClass != null && this.cssClass.Length > 0) writer.Write(" " + this.cssClass);
            writer.Write("\">");

            // write 3 inner divs
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (divNeeded) writer.RenderBeginTag(HtmlTextWriterTag.Div);

            // write any child controls
            base.RenderChildren(writer);

            // write 3 or 4 end tags, depending on how many start tags written
            if (divNeeded) writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.Write("</div>");
        }

    }
}
