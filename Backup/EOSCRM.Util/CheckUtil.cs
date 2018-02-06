#region Imports

using System;
using System.Text.RegularExpressions;

#endregion

namespace EOSCRM.Util
{
	/// <summary>
	/// Summary description for CheckUtil.
	/// </summary>
    public class CheckUtil
    {
        /// <summary>
        /// IsNaturalNumber
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNaturalNumber(String strNumber)
        {
            var objNotNaturalPattern = new Regex("[^0-9]");
            var objNaturalPattern = new Regex("0*[1-9][0-9]*");

            return !objNotNaturalPattern.IsMatch(strNumber) && objNaturalPattern.IsMatch(strNumber);
        }

        // Function to test for Positive Integers with zero inclusive

        /// <summary>
        /// IsWholeNumber
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsWholeNumber(string strNumber)
        {
            var objNotWholePattern = new Regex("[^0-9]");

            return !objNotWholePattern.IsMatch(strNumber);
        }

        // Function to Test for Integers both Positive & Negative

        /// <summary>
        /// IsInteger
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsInteger(string strNumber)
        {
            var objNotIntPattern = new Regex("[^0-9-]");
            var objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");

            return !objNotIntPattern.IsMatch(strNumber) && objIntPattern.IsMatch(strNumber);
        }

        // Function to Test for Positive Number both Integer & Real

        /// <summary>
        /// IsPositiveNumber
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsPositiveNumber(string strNumber)
        {
            var objNotPositivePattern = new Regex("[^0-9.]");
            var objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            var objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");

            return !objNotPositivePattern.IsMatch(strNumber) &&
            objPositivePattern.IsMatch(strNumber) && !objTwoDotPattern.IsMatch(strNumber);
        }

        // Function to test whether the string is valid number or not

        /// <summary>
        /// IsNumber
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
            !objTwoDotPattern.IsMatch(strNumber) &&
            !objTwoMinusPattern.IsMatch(strNumber) &&
            objNumberPattern.IsMatch(strNumber);
        }

        // Function To test for Alphabets.

        /// <summary>
        /// IsAlpha
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static bool IsAlpha(string strToCheck)
        {
            Regex objAlphaPattern = new Regex("[^a-zA-Z]");

            return !objAlphaPattern.IsMatch(strToCheck);
        }

        // Function to Check for AlphaNumeric.

        /// <summary>
        /// IsAlphaNumeric
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");

            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }

        /// <summary>
        /// IsEmail
        /// </summary>
        /// <param name="emailToCheck"></param>
        /// <returns></returns>
        public static bool IsEmail(string emailToCheck)
        {
            Regex objEmailPattern = new Regex(@"^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$");

            return objEmailPattern.IsMatch(emailToCheck);
        }

        /// <summary>
        /// IsDate
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static bool IsDate(string dateStr)
        {
            if (dateStr.Length < 8)
            {
                return false;
            }
            DateTime dt;
            bool isDate = true;

            try
            {
                dt = DateTime.Parse(dateStr);
                if (dt.CompareTo(new DateTime(1753, 1, 1)) < 1)
                {
                    isDate = false;
                }
            }
            catch
            {
                isDate = false;
            }
            return isDate;
        }

        /// <summary>
        /// IsDate
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsDate(string dateStr, string format)
        {
            if (dateStr.Length < 8)
            {
                return false;
            }
            DateTime dt;
            bool isDate = true;

            try
            {
                dt = DateTime.ParseExact(dateStr, format, null);
                if (dt.CompareTo(new DateTime(1753, 1, 1)) < 1)
                {
                    isDate = false;
                }
            }
            catch
            {
                isDate = false;
            }
            return isDate;
        }

        public static bool HasSpecialCharacter(string value)
        {
            bool flag = true;

            return flag;
        }

        public static bool IsFirstDateOfWeek(string value)
        {
            bool isFirst;
            try
            {
                var date = DateTime.Parse(value);
                return date.DayOfWeek.Equals(DayOfWeek.Monday);
            }
            catch
            {
                isFirst = false;
            }
            return isFirst;
        }

        public static bool IsLessThanCurrentWeek(string value)
        {
            bool isLess = false;
            try
            {
                DateTime date = DateTime.Parse(value);
                DateTime firstDateCurrentWeek = DateTimeUtil.GetDateFromWeekOfYear(DateTime.Now.Year, DateTimeUtil.GetWeekOfYear(DateTime.Now));
                if (date < firstDateCurrentWeek)
                {
                    isLess = true;
                }
            }
            catch
            {
                isLess = false;
            }
            return isLess;
        }
    }
}

