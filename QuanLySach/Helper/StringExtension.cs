using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLySach.Helper
{
    public static class StringExtension
    {
        public static string RemoveVietnameseSign(this string str)
        {
            string[] VietnameseSigns = new string[]
            {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
            };
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        public static string GetTwentyWords(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "Chưa cập nhật";
            str.Trim();
            int wordCount = str.Split(' ').Length;
            if (wordCount <= 20) return str;

            int spaceCount = 0;
            string result = "";
            var a = str.ToArray();
            for (int i = 0; i < a.Length; i++)
                if (a[i] == ' ')
                {
                    spaceCount++;
                    if (spaceCount == 20)
                        result = str.Substring(0, i);
                }
            return result;
        }
        public static bool Contains(this String str,
                                String substr,
                                StringComparison cmp)
        {
            if (substr == null)
                throw new ArgumentNullException("substring substring",
                                                " cannot be null.");

            else if (!Enum.IsDefined(typeof(StringComparison), cmp))
                throw new ArgumentException("comp is not a member of",
                                            "StringComparison, comp");

            return str.IndexOf(substr, cmp) >= 0;
        }
    }
}