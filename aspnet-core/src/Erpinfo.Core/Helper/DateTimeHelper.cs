using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime? ConvertDate(string date)
        {
            if (date == null) return null;
            DateTime dateTime = DateTime.ParseExact(date, "MM/dd/yyyy",
                                       System.Globalization.CultureInfo.CurrentCulture);
            return dateTime;
        }
    }
}
