using System;
using System.Globalization;
using System.Text;
using System.Resources;
using System.Web.UI;
using System.Web.UI.WebControls;
using EsccWebTeam.HouseStyle.Properties;

namespace EsccWebTeam.HouseStyle
{
	/// <summary>
	/// Paragraph stating which download formats are used and linking to a help page
	/// </summary>
	public class DownloadFormats : WebControl
	{
		private string prefix;
		private string formats;
		private string helpUrl;
		
		/// <summary>
		/// URL of help page to link to
		/// </summary>
		public string HelpUrl
		{
			get
			{
				return this.helpUrl;
			}
			set
			{
				this.helpUrl = value;
			}
		}
		
		/// <summary>
		/// Formats used, separated by semi-colons
		/// </summary>
		/// <example>PDF;RTF;XLS</example>
		public string Formats
		{
			get
			{
				return this.formats;
			}
			set
			{
				this.formats = value;
			}
		}
		
		/// <summary>
		/// Introductory text to replace "These documents are "
		/// </summary>
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		/// <summary>
		/// Instantiate a paragraph stating which download formats are used and linking to a help page
		/// </summary>
		public DownloadFormats() : base("p")
		{
			// default values
			this.prefix = "These documents are ";
			this.formats = "";
			this.helpUrl = "/help/viewingfiles/";
		}

		/// <summary>
		/// Build the para
		/// </summary>
		protected override void CreateChildControls()
		{
			// Get formats
			string[] formatsToList = this.formats.Split(';');
			int totalFormats = formatsToList.Length;

			if (totalFormats == 1 && formatsToList[0].Length == 0) 
			{
				this.Visible = false;
			}
			else
			{
				this.Visible = true;

				// Add lead text
				StringBuilder sb = new StringBuilder();
				sb.Append(this.prefix);
				sb.Append(" available in ");

				// Add formats
				for (short i = 0; i < totalFormats; i++)
				{
					formatsToList[i] = formatsToList[i].ToUpper(CultureInfo.CurrentCulture);
					if (totalFormats > 1 && i == totalFormats-1) sb.Append(" and ");
					else if (totalFormats > 1 && i > 0) sb.Append(", ");
					sb.Append("<span class=\"downloadFormat");
					sb.Append(formatsToList[i]);
					sb.Append("\">");
					sb.Append(Resources.ResourceManager.GetString("DownloadFormat" + formatsToList[i]));
					sb.Append("</span>");
				}

				// Add following text and link to help
				sb.Append(". If ");
				sb.Append(formatsToList.Length > 1 ? "these don't" : "this doesn't");
				sb.Append(" work for you, we can <a href=\"");
				sb.Append(this.helpUrl);
				sb.Append("\">help you with viewing files</a>.");

				this.Controls.Add(new LiteralControl(sb.ToString()));
			}
		}

	}
}
