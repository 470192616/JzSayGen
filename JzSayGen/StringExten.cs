using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JzSayGen
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExten
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// 替换 \r \n \t ' " 为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToJsLine(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\\'", "").Replace("\\\"", "").Replace("'", "").Replace("\"", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this string s, Int32 defaultVal = 0)
        {
            Int32 v;
            if (Int32.TryParse(s, out v) == false) return defaultVal;
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this string s, Int64 defaultVal = 0)
        {
            Int64 v;
            if (Int64.TryParse(s, out v) == false) return defaultVal;
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string s, decimal defaultVal = 0M)
        {
            decimal v;
            if (decimal.TryParse(s, out v) == false) return defaultVal;
            return v;
        }
    }
}
