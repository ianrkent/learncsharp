using System;

namespace ConsoleApp.ExtentionMethods
{
    public static class DateExtentions
    {
        public static string ReverseFormat(this DateTime date)
        {
            return "the calculated format of date" + date.ToLongDateString();
        }
    }
}
