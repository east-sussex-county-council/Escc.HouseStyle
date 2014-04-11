using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace EsccWebTeam.HouseStyle
{
    /// <summary>
    /// Rounded corners forming the top or bottom edge of a rounded box
    /// </summary>
    [DefaultProperty(""),
        ToolboxData("<{0}:RoundedBoxEdge runat=server></{0}:RoundedBoxEdge>")]
    [Obsolete("Use the CSS border-radius property")]
    public class RoundedBoxEdge : System.Web.UI.WebControls.WebControl
    {
        private BoxEdge edge = BoxEdge.Top;

        /// <summary>
        /// Set whether to render top or bottom corners
        /// </summary>
        public BoxEdge Edge
        {
            get
            {
                return this.edge;
            }
            set
            {
                this.edge = value;
            }
        }

        /// <summary>
        /// Rounded corners forming the top or bottom edge of a rounded box
        /// </summary>
        public RoundedBoxEdge()
            : base(HtmlTextWriterTag.Div)
        {
        }

        /// <summary>
        /// Rounded corners forming the top or bottom edge of a rounded box
        /// </summary>
        public RoundedBoxEdge(BoxEdge edge)
            : base(HtmlTextWriterTag.Div)
        {
            this.edge = edge;
        }

        /// <summary>
        /// Build the box edge
        /// </summary>
        protected override void CreateChildControls()
        {
            this.EnsureChildControls();

            // child control is the left-side corner
            HtmlGenericControl leftCorner = new HtmlGenericControl("div");
            leftCorner.Attributes.Add("class", (this.edge == BoxEdge.Top) ? "cornerTL" : "cornerBL");
            this.Controls.Add(leftCorner);

            // this control is the right-side corner
            this.Attributes.Add("class", (this.edge == BoxEdge.Top) ? "cornerTR" : "cornerBR");
        }

    }

    /// <summary>
    /// Edge types for RoundedBoxEdge control
    /// </summary>
    public enum BoxEdge
    {
        /// <summary>
        /// Top edge of box
        /// </summary>
        Top,

        /// <summary>
        /// Bottom edge of box
        /// </summary>
        Bottom
    }
}
