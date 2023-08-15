using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Helper
{
    public class UserHelper
    {
        public static string GetUserName(string emailAddress)
        {
            if (!string.IsNullOrEmpty(emailAddress))
            {
                var gmailFormat = "@ncc.asia";
                var userName = emailAddress.Contains(gmailFormat) ? emailAddress.Replace(gmailFormat, "") : null;
                return userName;
            }
            return null;

        }
    }
}
