using System;
using System.Globalization;

namespace EOSCRM.Util
{
    public enum Weekday
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
    public class DateTimeUtil
    {
        /// <summary>
        /// Get Date String To Display
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetDateStringToDisplay(DateTime? datetime)
        {
            string dateStr = string.Empty;

            try
            {
                if (datetime != null)
                {
                    dateStr = DateTime.Parse(Convert.ToString(datetime)).ToString(Constants.DateformatView);
                }
            }
            catch
            {
                dateStr = "N/A";
            }
            return dateStr;
        }

        /// <summary>
        /// Get Date String To Display
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetDateStringToDisplay(string datetime)
        {
            try
            {
                return (DateTime.Parse(datetime)).ToString(Constants.DateformatView);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Get Date String To Display
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetDateTimeStringToDisplay(string datetime)
        {
            try
            {
                return (DateTime.Parse(datetime)).ToString(Constants.DateTimeformatView);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// GetDateFromWeekOfYear
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static DateTime GetDateFromWeekOfYear(int Year, int week)
        {
            DateTime date = new DateTime(Year, 1, 1);
            date = date.AddDays(week * 7 - 1);
            return date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentWeekOfYear()
        {
            GregorianCalendar calendar = new GregorianCalendar();
            return calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime date)
        {
            GregorianCalendar calendar = new GregorianCalendar();
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Get First Date Of Month
        /// </summary>
        /// <param name="weekday"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetFirstDateOfMonth(Weekday weekday, int month, int year)
        {
            int date = -1;
            string dayCmp = weekday.ToString().Substring(0, 3);

            for (int i = 1; i <= 7; i++)
            {
                if (dayCmp.Equals(new DateTime(year, month, i).ToString("ddd")))
                {
                    date = i;
                    break;
                }
            }

            return date;
        }

        /// <summary>
        /// Get First Date Of Month
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetFirstDateOfMonth(int day, int month, int year)
        {
            var week = 1;

            if (day > 7 && day <= 14)
                week = 2;
            else if (day > 14 && day <= 21)
                week = 3;
            else if (day > 21 && day <= 28)
                week = 4;
            else if (day > 28 && day <= 31)
                week = 5;

            var date = GetFirstDateOfMonth(Weekday.Monday, month, year);

            if ((week > 1) && (week <= 5))
            {
                date += ((week - 1) * 7);
            }

            return date;
        }
        /// <summary>
        /// True is ok
        /// StartDate + year >= EndDate (true)
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool CompareYear(DateTime startdate, DateTime enddate, int year)
        {
            return startdate.AddYears(year).CompareTo(enddate) < 0;
        }

        public static string GetDateString(DateTime date)
        {
            return date.ToString(Constants.DateformatView);
        }

        /// <summary>
        /// GetFirstDayOfWeek
        /// by Tan Tran
        /// </summary>        
        /// <param name="year"></param>
        /// <param name="weekNumber"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(int year, int weekNumber, DayOfWeek dayOfWeek)
        {
            return GetLastOccurenceOfDay(new DateTime(year, 1, 1).AddDays(7 * weekNumber), dayOfWeek);
        }

        /// <summary>
        /// GetFirstDayOfWeek
        /// by Tan Tran
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            return GetFirstDayOfWeek(date.Year, GetWeekOfYear(date) + 1, DayOfWeek.Monday);
        }

        /// <summary>
        /// GetLastOccurenceOfDay
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetLastOccurenceOfDay(DateTime value, DayOfWeek dayOfWeek)
        {
            int daysToAdd = dayOfWeek - value.DayOfWeek;
            if (daysToAdd < 1)
            {
                daysToAdd -= 7;
            }
            return value.AddDays(daysToAdd);
        }

        public static DateTime GetVietNamDate(string value)
        {
            string[] slist1 = value.Split('/');
            return new DateTime(int.Parse(slist1[2]), int.Parse(slist1[1]), int.Parse(slist1[0]), 0, 0, 0);
        }

        public static DateTime GetVietNamDate(string value, string gio, string phut)
        {
            string[] slist1 = value.Split('/');
            DateTime tmp = new DateTime(int.Parse(slist1[2]), int.Parse(slist1[1]), int.Parse(slist1[0]), int.Parse(gio),
                                        int.Parse(phut), 0);
            return tmp;
        }
    }
}
