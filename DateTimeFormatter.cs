using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace eastsussexgovuk.webservices.TextXhtml.HouseStyle
{
    /// <summary>
    /// Provides date and time formatting methods not built in to C#
    /// </summary>
    public class DateTimeFormatter
    {
        /// <summary>
        /// Gets the current UK time regardless of the current thread culture
        /// </summary>
        /// <returns></returns>
        /// <remarks>Important for applications hosted on Microsoft Azure where the time is in UTC and the culture is en-US.</remarks>
        public static DateTime UkNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        }

        /// <summary>
        /// Parses the date, in way which is a little more forgiving than the default .NET implementation.
        /// </summary>
        /// <param name="dateText">The date text.</param>
        /// <returns>Parsed date, or <c>null</c> if not recognised</returns>
        public static DateTime? ParseDate(string dateText)
        {
            // Remove ordinals
            dateText = dateText.Trim();
            dateText = Regex.Replace(dateText, "\\b([0-9]+)(st|nd|rd|th)\\b", "$1");

            // Otherwise it must be a date recognised by .NET. But if there's only one space in the string,
            // assume user has put a date without a year, eg "29 may", and add the year on the end
            string space = " ";
            if (dateText.IndexOf(space) > -1 && dateText.IndexOf(space) == dateText.LastIndexOf(space))
            {
                dateText += (" " + DateTime.Now.Year);
            }

            DateTime parsedDate;
            if (DateTime.TryParse(dateText, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a integer month into a full English month name
        /// </summary>
        /// <param name="month">int month</param>
        /// <returns>January, February, March etc.</returns>
        public static string MonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Converts a integer month into an abbreviated English month name
        /// </summary>
        /// <param name="month">int month</param>
        /// <returns>Jan, Feb, Mar etc.</returns>
        public static string ShortMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Get a string in the format 1 January 2004 from a DateTime object. This method should be used only when the preferred style (including the day) is too long.
        /// </summary>
        public static string FullBritishDate(DateTime date)
        {
            return (new StringBuilder(date.Date.Day.ToString(CultureInfo.CurrentCulture)).Append(" ").Append(MonthName(date.Date.Month)).Append(" ").Append(date.Date.Year.ToString()).ToString());
        }

        /// <summary>
        /// Get a string in the format 1 Jan 2004 from a DateTime object. This method should be used only when the preferred full style (including the day) is too long.
        /// </summary>
        public static string ShortBritishDate(DateTime date)
        {
            return (new StringBuilder(date.Date.Day.ToString(CultureInfo.CurrentCulture)).Append(" ").Append(ShortMonthName(date.Date.Month)).Append(" ").Append(date.Date.Year.ToString()).ToString());
        }

        /// <summary>
        /// Get a string in the format 1 Jan 2004 from a DateTime object. Use only for short-term data about the current year, never for anything which will be seen later on, and then only when the preferred full style (including the day) is too long.
        /// </summary>
        public static string ShortBritishDateNoYear(DateTime date)
        {
            return (new StringBuilder(date.Date.Day.ToString(CultureInfo.CurrentCulture)).Append(" ").Append(ShortMonthName(date.Date.Month)).ToString());
        }

        /// <summary>
        /// Get a string in the format Monday 1 January 2004 from a DateTime object
        /// </summary>
        public static string FullBritishDateWithDay(DateTime date)
        {
            return (new StringBuilder(date.Date.DayOfWeek.ToString()).Append(" ").Append(date.Date.Day.ToString()).Append(" ").Append(MonthName(date.Date.Month)).Append(" ").Append(date.Date.Year.ToString()).ToString());
        }

        /// <summary>
        /// Takes a string as a date and returns a string in full british format with date and time
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FullBritishDateWithDayAndTimeString(string date)
        {
            return DateTimeFormatter.FullBritishDateWithDayAndTime(DateTime.Parse(date, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Get a string in the format 10am, Monday 1 January 2004 from a DateTime object
        /// </summary>
        public static string FullBritishDateWithDayAndTime(DateTime date)
        {
            return (new StringBuilder(DateTimeFormatter.FullBritishDateWithDay(date)).Append(", ").Append(DateTimeFormatter.Time(date)).ToString());
        }

        /// <summary>
        /// Get a string in the format 10am, 1 Jan 2004 from a DateTime object
        /// </summary>
        public static string ShortBritishDateWithTime(DateTime date)
        {
            return (new StringBuilder(DateTimeFormatter.ShortBritishDate(date)).Append(", ").Append(DateTimeFormatter.Time(date)).ToString());
        }

        /// <summary>
        /// Get a string in the format 1 Jan, 10am. Use only for short-term data about the current year, never for anything which will be seen later on.
        /// </summary>
        public static string ShortBritishDateNoYearWithTime(DateTime date)
        {
            return (new StringBuilder(DateTimeFormatter.ShortBritishDateNoYear(date)).Append(", ").Append(DateTimeFormatter.Time(date)).ToString());
        }

        /// <summary>
        /// Get a string in the format January 2004 from a DateTime object
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string MonthAndYear(DateTime date)
        {
            return date.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Get a string in the format 10am or 10.15am from a DateTime object
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string Time(DateTime time)
        {
            StringBuilder sb = new StringBuilder();

            // Add the hour
            if (time.Hour == 0) sb.Append("12");
            else
            {
                sb.Append((time.Hour <= 12) ? time.Hour.ToString() : (time.Hour - 12).ToString());
            }

            // Add the minutes only if there are some
            if (time.Minute > 0)
            {
                sb.Append(".");
                sb.Append(time.ToString("mm"));
            }

            // Add am/pm unless it's midnight or midday
            if (time.Hour == 0 && time.Minute == 0)
            {
                sb.Append(" midnight");
            }
            else if (time.Hour == 12 && time.Minute == 0)
            {
                sb.Append(" noon");
            }
            else
            {
                sb.Append(time.ToString("tt", CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            }
            return sb.ToString();
        }



        /// <summary>
        /// Get a string in the format YYYY-MM-DD from a DateTime object
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ISODate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a date and time as an ISO 8601 UTC date and time string
        /// </summary>
        /// <param name="date">Date and time to convert</param>
        /// <returns>ISO 8601 UTC date and time string. <example>2006-04-01T15:30:00Z</example></returns>
        /// <remarks>Suitable for hCalendar microformat.</remarks>
        public static string Iso8601DateTime(DateTime date)
        {
            return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a date and time in RFC 822 format, as used by RSS feeds.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>UTC date and time in RFC 822 format</returns>
        /// <remarks>
        /// <para>Syntax specified at <a href="http://asg.web.cmu.edu/rfc/rfc822.html#sec-5.1">http://asg.web.cmu.edu/rfc/rfc822.html#sec-5.1</a></para>
        /// </remarks>
        /// <example>
        /// date-time   =  [ day "," ] date time        ; dd mm yy
        ///                                             ;  hh:mm:ss zzz
        ///
        /// day         =  "Mon"  / "Tue" /  "Wed"  / "Thu"
        ///             /  "Fri"  / "Sat" /  "Sun"
        ///
        /// date        =  1*2DIGIT month 2DIGIT        ; day month year
        ///                                             ;  e.g. 20 Jun 82
        ///
        /// month       =  "Jan"  /  "Feb" /  "Mar"  /  "Apr"
        ///             /  "May"  /  "Jun" /  "Jul"  /  "Aug"
        ///             /  "Sep"  /  "Oct" /  "Nov"  /  "Dec"
        ///
        /// time        =  hour zone                    ; ANSI and Military
        ///
        /// hour        =  2DIGIT ":" 2DIGIT [":" 2DIGIT]
        ///                                             ; 00:00:00 - 23:59:59
        ///
        /// zone        =  "UT"  / "GMT"                ; Universal Time
        ///                                             ; North American : UT
        ///             /  "EST" / "EDT"                ;  Eastern:  - 5/ - 4
        ///             /  "CST" / "CDT"                ;  Central:  - 6/ - 5
        ///             /  "MST" / "MDT"                ;  Mountain: - 7/ - 6
        ///             /  "PST" / "PDT"                ;  Pacific:  - 8/ - 7
        ///             /  1ALPHA                       ; Military: Z = UT;
        ///                                             ;  A:-1; (J not used)
        ///                                             ;  M:-12; N:+1; Y:+12
        ///             / ( ("+" / "-") 4DIGIT )        ; Local differential
        ///                                             ;  hours+min. (HHMM)
        /// </example>
        public static string Rfc822DateTime(DateTime date)
        {
            // Create a culture object which definitely has the correct day and month names
            CultureInfo ukCulture = CultureInfo.CreateSpecificCulture("en-GB");

            // Using four-digit year even though spec above says use two-digit. 
            // Four-digit appears to be in common use so should be OK. 
            return date.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss UT");
        }

        /// <summary>
        /// Gets and date and time as a UNIX timestamp
        /// </summary>
        /// <param name="date">Date and time to convert</param>
        /// <returns>UNIX timestamp, eg 1115337662 </returns>
        /// <remarks>See <a href="http://www.unixtimestamp.com/">UNIXtimestamp.com</a> for a testing tool</remarks>
        public static int UnixTimestamp(DateTime date)
        {
            const long ticks1970 = 621355968000000000; // .NET ticks for 1970
            return (int)((date.Ticks - ticks1970) / 10000000L);
        }

        /// <summary>
        /// Designed with the Events system in mind but follows house style rules for date and time formatting
        /// 
        /// Get a string in one of the following formats based on start and end date and whether you wish to include time
        /// 
        /// Formats:
        /// 
        ///	Thursday 9 August 2004 (start and end date are on the same day)
        ///	9 - 23 August 2004
        ///	9 August - 8 September 2004
        ///			
        ///	including time:
        ///	
        ///	Thursday 9 August 2004 from 9am (or e.g. 9.30am if minutes are greater than 0)
        ///	9 - 23 August 2004 from 9am
        ///					
        /// </summary>
        /// <param name="start">DateTime</param>
        /// <param name="end">DateTime</param>
        /// <param name="includeTime">Boolean default is false</param>
        /// <returns>String</returns>
        public static string HouseStyleDates(string start, string end, bool includeTime)//DateTime startDate, DateTime endDate, Boolean includeTime)
        {


            DateTime startDate, endDate;

            startDate = Convert.ToDateTime(start, CultureInfo.CurrentCulture);
            endDate = Convert.ToDateTime(end, CultureInfo.CurrentCulture);

            //		DateTime startDate = Convert.ToDateTime(_startDate);
            //	DateTime endDate = Convert.ToDateTime(_endDate);

            //	bool includeTime = true;
            StringBuilder sb = new StringBuilder();

            if (startDate.Month == endDate.Month)
            {



                if (startDate.Day != endDate.Day)
                {
                    sb.Append(startDate.Day.ToString(CultureInfo.CurrentCulture));
                    sb.Append(" - ");
                }
                else
                {
                    sb.Append(startDate.DayOfWeek.ToString());
                    sb.Append(" ");
                }

                sb.Append(endDate.Day.ToString());
                sb.Append(" ");
                sb.Append(startDate.ToString("MMMM", CultureInfo.CurrentCulture));
                sb.Append(" ");
                sb.Append(startDate.ToString("yyyy", CultureInfo.CurrentCulture));

                if (includeTime)
                {

                    sb.Append(" from ");

                    if (startDate.Minute > 0)
                    {
                        sb.Append(startDate.ToString("h ", CultureInfo.CurrentCulture).Trim() + "." +
                            startDate.Minute.ToString(CultureInfo.CurrentCulture) + startDate.ToString("tt", CultureInfo.CurrentCulture).ToLower());
                    }
                    else
                    {
                        sb.Append(startDate.ToString("h ", CultureInfo.CurrentCulture).Trim() + startDate.ToString("tt", CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
                    }



                }

                return sb.ToString();
            }
            else
            {

                sb.Append(startDate.Day.ToString());
                sb.Append("  ");
                sb.Append(startDate.ToString("MMMM"));
                sb.Append(" - ");
                sb.Append(endDate.Day.ToString());
                sb.Append(" ");
                sb.Append(endDate.ToString("MMMM"));
                sb.Append(" ");
                sb.Append(endDate.ToString("yyyy"));
                if (includeTime)
                {

                    sb.Append(" from ");

                    if (startDate.Minute > 0)
                    {
                        sb.Append(startDate.ToString("h ").Trim() + "." +
                            startDate.Minute.ToString() + startDate.ToString("tt").ToLower());
                    }
                    else
                    {
                        sb.Append(startDate.ToString("h ").Trim() + startDate.ToString("tt").ToLower());
                    }



                }



                return sb.ToString();

            }

        }

        /// <summary>
        /// Gets the house-style description of a period from one date and time to another
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A text string which includes HTML entities</returns>
        public static string DateRange(DateTime startDate, DateTime endDate)
        {
            return DateTimeFormatter.DateRange(startDate, endDate, true, true);
        }

        /// <summary>
        /// Gets the house-style description of a period from one date/time to another
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="showStartTime">if set to <c>true</c> show start time.</param>
        /// <param name="showEndTime">if set to <c>true</c> show end time.</param>
        /// <returns>A text string which includes HTML entities</returns>
        public static string DateRange(DateTime startDate, DateTime endDate, bool showStartTime, bool showEndTime)
        {
            return DateRange(startDate, endDate, showStartTime, showEndTime, false);
        }

        /// <summary>
        /// Gets the house-style description of a period from one date/time to another
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="showStartTime">if set to <c>true</c> show start time.</param>
        /// <param name="showEndTime">if set to <c>true</c> show end time.</param>
        /// <param name="useShortDateText">if set to <c>true</c> omit day name and use short month names.</param>
        /// <returns>
        /// A text string which includes HTML entities
        /// </returns>
        public static string DateRange(DateTime startDate, DateTime endDate, bool showStartTime, bool showEndTime, bool useShortDateText)
        {
            bool multiDay = (startDate.DayOfYear != endDate.DayOfYear || startDate.Year != endDate.Year);
            bool showTime = (showStartTime || showEndTime);
            bool sameMonth = (startDate.Month == endDate.Month && startDate.Year == endDate.Year);

            if (!multiDay && !showTime)
            {
                /*
                    One day, no time
                    ---------------------------------------
                    Friday 26 May 2006
                    */

                if (useShortDateText) return ShortBritishDate(startDate);
                return FullBritishDateWithDay(startDate);

            }
            else if (!multiDay && showStartTime && !showEndTime)
            {
                /*
                    One day, with start time
                    ---------------------------------------
                    9am, Friday 26 May 2006
                    */
                if (useShortDateText) return ShortBritishDateWithTime(startDate);
                return DateTimeFormatter.FullBritishDateWithDayAndTime(startDate);

            }
            else if (!multiDay && showStartTime && showEndTime)
            {
                /*
                    One day, with start and finish times
                    ---------------------------------------
                    9am-2pm, Friday 26 May 2006
                    */
                if (useShortDateText) return (new StringBuilder(DateTimeFormatter.Time(startDate)).Append(" to ").Append(DateTimeFormatter.Time(endDate)).Append(", ").Append(DateTimeFormatter.ShortBritishDate(startDate)).ToString());
                return (new StringBuilder(DateTimeFormatter.Time(startDate)).Append(" to ").Append(DateTimeFormatter.Time(endDate)).Append(", ").Append(DateTimeFormatter.FullBritishDateWithDay(startDate)).ToString());

            }
            else if (multiDay && !showTime && sameMonth)
            {
                /*
                    Different days, no times, same month
                    ---------------------------------------
                    26-27 May 2006
                    */
                if (useShortDateText) return (new StringBuilder(startDate.Day.ToString(CultureInfo.CurrentCulture)).Append(" to ").Append(DateTimeFormatter.ShortBritishDate(endDate)).ToString());
                return (new StringBuilder(startDate.Day.ToString(CultureInfo.CurrentCulture)).Append(" to ").Append(DateTimeFormatter.FullBritishDate(endDate)).ToString());

            }
            else if (multiDay && !showTime && !sameMonth)
            {
                /*
                    Different days, no times, different month
                    ---------------------------------------
                    Friday 26 May 2006 - Thursday 1 June 2006 
                    */
                if (useShortDateText) return (new StringBuilder(DateTimeFormatter.ShortBritishDate(startDate)).Append(" to ").Append(DateTimeFormatter.ShortBritishDate(endDate)).ToString());
                return (new StringBuilder(DateTimeFormatter.FullBritishDateWithDay(startDate)).Append(" to ").Append(DateTimeFormatter.FullBritishDateWithDay(endDate)).ToString());

            }
            else if (multiDay && showStartTime && !showEndTime)
            {
                /*
                    Different days, with start time
                    ---------------------------------------
                    9am, Friday 26 May 2006 to Saturday 27 May 2006
                    */
                if (useShortDateText) return (new StringBuilder(DateTimeFormatter.ShortBritishDateWithTime(startDate)).Append(" to ").Append(DateTimeFormatter.ShortBritishDate(endDate)).ToString());
                return (new StringBuilder(DateTimeFormatter.FullBritishDateWithDayAndTime(startDate)).Append(" to ").Append(DateTimeFormatter.FullBritishDateWithDay(endDate)).ToString());

            }
            else if (multiDay && showStartTime && showEndTime)
            {
                /*
                    Different days, with start and end time
                    ---------------------------------------
                    9am, Friday 26 May 2006 to 2pm, Saturday 27 May 2006
                    */
                if (useShortDateText) return (new StringBuilder(DateTimeFormatter.ShortBritishDateWithTime(startDate)).Append(" to ").Append(DateTimeFormatter.ShortBritishDateWithTime(endDate)).ToString());
                return (new StringBuilder(DateTimeFormatter.FullBritishDateWithDayAndTime(startDate)).Append(" to ").Append(DateTimeFormatter.FullBritishDateWithDayAndTime(endDate)).ToString());

            }

            // Shouldn't get here
            return String.Empty;
        }

        /// <summary>
        /// Gets the house-style description of a period from one date and time to another, with HTML controls which implement the hCalendar microformat
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>If the date can be presented as a single string, a single control is returned. If the start and finish dates should be presented separately, two controls will be returned.</returns>
        /// <remarks>These controls form only one part of the hCalendar microformat, and will not work unless other parts of the format are also implemented.</remarks>
        public static Control[] DateRangeHCalendar(DateTime startDate, DateTime endDate)
        {
            return DateTimeFormatter.DateRangeHCalendar(startDate, endDate, true, true, true, true);
        }

        /// <summary>
        /// Gets the house-style description of a period from one date and time to another, with HTML controls which implement the hCalendar microformat
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="startTimeKnown">if set to <c>true</c> the supplied start time is correct. If <c>false</c>, 00.00 is assumed.</param>
        /// <param name="endTimeKnown">if set to <c>true</c> the supplied end time  is correct. If <c>false</c> 00.00 the next morning is assumed.</param>
        /// <param name="showStartTime">if set to <c>true</c> display the start time.</param>
        /// <param name="showEndTime">if set to <c>true</c> display the end time.</param>
        /// <returns>HTML controls with the date embedded</returns>
        public static Control[] DateRangeHCalendar(DateTime startDate, DateTime endDate, bool startTimeKnown, bool endTimeKnown, bool showStartTime, bool showEndTime)
        {
            bool multiDay = (startDate.DayOfYear != endDate.DayOfYear || startDate.Year != endDate.Year);
            bool showTime = (showStartTime || showEndTime);
            bool sameMonth = (startDate.Month == endDate.Month && startDate.Year == endDate.Year);

            // The display date may need to be different to the metadata (actual) date, so work with two copies
            DateTime displayStartDate = startDate;
            DateTime displayEndDate = endDate;

            if (!startTimeKnown) startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            if (!startTimeKnown || !showStartTime) displayStartDate = new DateTime(displayStartDate.Year, displayStartDate.Month, displayStartDate.Day);

            // all-day event goes up to midnight at the start of the NEXT day
            if (!endTimeKnown) endDate = new DateTime(endDate.AddDays(1).Year, endDate.AddDays(1).Month, endDate.AddDays(1).Day);
            if (!endTimeKnown || !showEndTime) displayEndDate = new DateTime(displayEndDate.Year, displayEndDate.Month, displayEndDate.Day);

            HtmlGenericControl tagStart = new HtmlGenericControl("time");
            tagStart.Attributes["class"] = "dtstart"; // hCalendar
            tagStart.Attributes["datetime"] = DateTimeFormatter.Iso8601DateTime(startDate);  // hCalendar

            HtmlGenericControl tagEnd = new HtmlGenericControl("time");
            tagEnd.Attributes["class"] = "dtend"; // hCalendar
            tagEnd.Attributes["datetime"] = DateTimeFormatter.Iso8601DateTime(endDate);  // hCalendar

            if (!multiDay && !showTime)
            {
                /*
                    One day, no time
                    ---------------------------------------
                    Friday 26 May 2006
                    */

                Control[] hCal = new Control[1];
                PlaceHolder ph = new PlaceHolder();

                hCal[0] = ph;
                ph.Controls.Add(tagStart);
                ph.Controls.Add(tagEnd);

                tagEnd.InnerText = DateTimeFormatter.FullBritishDateWithDay(displayStartDate);

                return hCal;

            }
            else if (!multiDay && ((showStartTime && !showEndTime) || (showStartTime && showEndTime && displayStartDate == displayEndDate)))
            {
                /*
                    One day, with start time (or matching start and end times)
                    ---------------------------------------
                    9am, Friday 26 May 2006
                    */

                Control[] hCal = new Control[1];
                PlaceHolder ph = new PlaceHolder();

                hCal[0] = ph;
                ph.Controls.Add(tagStart);
                ph.Controls.Add(tagEnd);

                tagEnd.InnerText = DateTimeFormatter.FullBritishDateWithDayAndTime(displayStartDate);

                return hCal;

            }
            else if (!multiDay && showStartTime && showEndTime)
            {
                /*
                    One day, with start and finish times
                    ---------------------------------------
                    9am to 2pm, Friday 26 May 2006
                    */

                Control[] hCal = new Control[1];
                PlaceHolder ph = new PlaceHolder();

                hCal[0] = ph;

                tagStart.InnerText = DateTimeFormatter.Time(displayStartDate);
                ph.Controls.Add(tagStart);

                ph.Controls.Add(new LiteralControl(" to "));

                tagEnd.InnerText = (new StringBuilder(DateTimeFormatter.Time(displayEndDate)).Append(", ").Append(DateTimeFormatter.FullBritishDateWithDay(displayStartDate)).ToString());
                ph.Controls.Add(tagEnd);

                return hCal;

            }
            else if (multiDay && !showTime && sameMonth)
            {
                /*
                    Different days, no times, same month
                    ---------------------------------------
                    26 to 27 May 2006
                    */

                Control[] hCal = new Control[1];
                PlaceHolder ph = new PlaceHolder();

                hCal[0] = ph;

                tagStart.InnerText = displayStartDate.Day.ToString(CultureInfo.CurrentCulture);
                ph.Controls.Add(tagStart);

                ph.Controls.Add(new LiteralControl(" to "));

                tagEnd.InnerText = DateTimeFormatter.FullBritishDate(displayEndDate);
                ph.Controls.Add(tagEnd);

                return hCal;

            }
            else if (multiDay && !showTime && !sameMonth)
            {
                /*
                    Different days, no times, different month
                    ---------------------------------------
                    Friday 26 May 2006 to Thursday 1 June 2006 
                    */

                Control[] hCal = new Control[1];
                PlaceHolder ph = new PlaceHolder();

                hCal[0] = ph;

                tagStart.InnerText = DateTimeFormatter.FullBritishDateWithDay(displayStartDate);
                ph.Controls.Add(tagStart);

                // Previous version using hyphen (for reference): ph.Controls.Add(new LiteralControl(" &#8211; "));
                ph.Controls.Add(new LiteralControl(" to "));

                tagEnd.InnerText = DateTimeFormatter.FullBritishDateWithDay(displayEndDate);
                ph.Controls.Add(tagEnd);

                return hCal;

            }
            else if (multiDay && showStartTime && !showEndTime)
            {
                /*
                    Different days, with start time
                    ---------------------------------------
                    Start: 9am, Friday 26 May 2006
                    Finish: Saturday 27 May 2006
                    */

                Control[] hCal = new Control[2];

                hCal[0] = tagStart;
                hCal[1] = tagEnd;

                tagStart.InnerText = DateTimeFormatter.FullBritishDateWithDayAndTime(displayStartDate);
                tagEnd.InnerText = DateTimeFormatter.FullBritishDateWithDay(displayEndDate);

                return hCal;

            }
            else if (multiDay && showStartTime && showEndTime)
            {
                /*
                    Different days, with start and end time
                    ---------------------------------------
                    Start: 9am, Friday 26 May 2006
                    Finish: 2pm, Saturday 27 May 2006
                    */

                Control[] hCal = new Control[2];

                hCal[0] = tagStart;
                hCal[1] = tagEnd;

                tagStart.InnerText = DateTimeFormatter.FullBritishDateWithDayAndTime(displayStartDate);
                tagEnd.InnerText = DateTimeFormatter.FullBritishDateWithDayAndTime(displayEndDate);

                return hCal;

            }

            // Shouldn't get here
            return new Control[0];
        }
    }
}
