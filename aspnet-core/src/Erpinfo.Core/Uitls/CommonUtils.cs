using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Erpinfo.Uitls
{
    public class CommonUtils
    {
        public static string GenerateRandomPassword(byte length)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        /// <summary>
        /// Convert Discord Text To Html
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertDiscordTextToHtml(string text)
        {
            try
            {
                text = RemoveCommandText(text);

                text = ConvertTextLinkToHtml(text);

                text = RemoveSpecialCharsUseRegularExpression(text);

                string[] special_char = { "***", "~~", "```", "__", "_", "**", "*" };

                foreach (var item in special_char)
                {
                    text = RemoveSpecialChars(text, item);
                }

                return @"<html><body><p style='margin-top: 10px'>" + text + "</p></body></html>";
            }
            catch (RegexMatchTimeoutException)
            {
                return text;
            }
        }

        /// <summary>
        /// Convert Hover Text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertHoverText(string text)
        {
            text = RemoveCommandText(text);

            text = RemoveSpecialCharsUseRegularExpression(text);

            string[] special_char = { "***", "~~", "```", "__", "_", "**", "*" };

            foreach (var item in special_char)
            {
                text = text.Replace(item, "");
            }

            return text;
        }

        /// <summary>
        /// Remove Strange Special Char
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string RemoveSpecialCharsUseRegularExpression(string text)
        {
            string[] arrPattern = { @"[-]+[ ]+[<]+[A-Za-z0-9 :@&^%$#]+[>]+[ ]+[-]", @"[<]+[A-Za-z0-9 :@&^%$#]+[>]" };

            foreach (Match match in Regex.Matches(text, arrPattern[0], RegexOptions.None, TimeSpan.FromSeconds(2)))
            {
                if (text.Contains(match.Value))
                {
                    text = Regex.Replace(text, arrPattern[0], "-");
                }
            }

            foreach (Match match in Regex.Matches(text, arrPattern[1], RegexOptions.None, TimeSpan.FromSeconds(2)))
            {
                if (text.Contains(match.Value))
                {
                    text = Regex.Replace(text, arrPattern[1], "");
                }
            }

            return text;
        }
         
        /// <summary>
        /// Remove Command Text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string RemoveCommandText(string text)
        {
            if (text.StartsWith("*thongbao"))
            {
                text = text.Substring(10);
            }

            return text;
        }

        /// <summary>
        /// Remove Special Chars
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string RemoveSpecialChars(string text, string kt)
        {
            var dicSpecialCharToReplaceText = new Dictionary<string, string>()
                        {
                            {"***", "<b><i>{0}</i></b>" },
                            {"**", "<b>{0}</b>" },
                            {"*", "<i>{0}</i>" },
                            {"__", "<u>{0}</u>" },
                            {"_", "<i>{0}</i>" },
                            {"```", @"<p style='background-color: #606060;color: #fff;margin-top: 1.4px; padding:10px;border-radius: 5px;line-height: 19.5px'>{0}</p>" },
                            {"~~", "<s>{0}</s>" },
                        };

            string[] special_char = { "***", "~~", "```", "__", "_", "**", "*" };

            foreach (var item in special_char)
            {
                List<string> res = text.Split(item).ToList();

                res.ForEach(x =>
                {
                    if (dicSpecialCharToReplaceText.ContainsKey(kt) && !x.StartsWith("\n") && !x.EndsWith("\n"))
                    {
                        text = text.Replace(kt + x + kt, string.Format(dicSpecialCharToReplaceText[kt], x));
                    }
                });
            }

            return text;
        }

        /// <summary>
        /// Check Link Html Text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string ConvertTextLinkToHtml(string text)
        {
            string patternLink = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";

            foreach (Match match in Regex.Matches(text, patternLink, RegexOptions.None, TimeSpan.FromSeconds(2)))
            {
                if (text.Contains(match.Value))
                {
                    text = Regex.Replace(text, patternLink, @"<a href='" + match.Value + "' target='_blank'>" + match.Value + "</a>");
                }
            }

            return text;
        }
    }
}