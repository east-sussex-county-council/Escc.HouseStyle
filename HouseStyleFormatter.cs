using System;
using EsccWebTeam.Gdsc;

namespace EsccWebTeam.HouseStyle
{
	/// <summary>
	/// Methods used to format data according to the East Sussex County Council house style
	/// </summary>
	public static class HouseStyleFormatter
	{
		/// <summary>
		/// Tidies up a UK telephone number according to house style
		/// </summary>
		/// <param name="tel"></param>
		/// <returns></returns>
		public static string FormatTelephone(string tel)
		{
			UKContactNumber num = new UKContactNumber();
			num.NationalNumber = tel;
			return num.ToUKString();
		}
	}
}
