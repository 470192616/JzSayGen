using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

namespace JzSayGen
{
    /// <summary>
    /// 字符串方法扩展
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
        /// 替换 \r \n \t ' " 为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToJsLine(this string s)
        {
            if (s.IsNullOrEmpty()) return "";
            return s.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\\'", "").Replace("\\\"", "").Replace("'", "").Replace("\"", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultVal"></param>
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
        /// <param name="defaultVal"></param>
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
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string s, decimal defaultVal = 0M)
        {
            decimal v;
            if (decimal.TryParse(s, out v) == false) return defaultVal;
            return v;
        }

        /// <summary>
        /// 判断是否guid类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsGuid(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s.Length < 30) return false;
            Guid g;
            return Guid.TryParse(s, out g);
        }

        /// <summary>
        /// 判断字符串是否是短日期格式 yyyy-mm-dd
        /// </summary>    
        public static bool IsShortDate(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s.Length < 8) return false;

            string RegShortDate = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
            //短日期型 yyyy-mm-dd  
            return Regex.IsMatch(s, RegShortDate, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断字符串是否是长日期格式 yyyy-mm-dd hh:mm:ss
        /// </summary>
        public static bool IsLongDateTime(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s.Length < 14) return false;

            string RegLongDate = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
            //长日期型 yyyy-mm-dd hh:mm:ss
            return Regex.IsMatch(s, RegLongDate, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 清除html标签标记，及&lt;和&gt;包括的字符如 &lt;br /&gt;
        /// </summary>    
        public static string ClearHtmlTag(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            //Regex.Replace(html, "(?is)<.*?>", "")
            //string pattern = @"<(.|\n)*?>";
            return Regex.Replace(s, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 清除换行标记 \t \r \n
        /// </summary>    
        public static string ClearLineTag(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("\t", "");
            sb.Replace("\r", "");
            sb.Replace("\n", "");
            return sb.ToString();
        }

        /// <summary>
        /// 清理HTML标签，删除所有的属性
        /// </summary>
        /// <param name="s"></param>
        /// <param name="element">要清除的标签如：div</param>
        /// <returns></returns>
        public static string ClearElement(this string s, string element)
        {
            if (s.IsNullOrEmpty()) return "";
            string old = @"<" + element + "[^>]+>";
            string rep = "<" + element + ">";
            return Regex.Replace(s, old, rep, RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 清除HTML标签，删除包括标记本身
        /// </summary>        
        /// <param name="s"></param>
        /// <param name="element">要清除的标签如：div</param>
        /// <returns></returns>
        public static string RemoveElement(this string s, string element)
        {
            if (s.IsNullOrEmpty()) return "";
            string regFront = @"<" + element + "[^>]*>";
            string regAfter = "</" + element + ">";
            s = Regex.Replace(s, regFront, "", RegexOptions.IgnoreCase);
            return Regex.Replace(s, regAfter, "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 字符串首字母小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstLeterLower(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s[0].ToString().ToLower() + s.Substring(1);
        }

        /// <summary>
        /// 字符串首字母大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstLeterUpper(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s[0].ToString().ToUpper() + s.Substring(1);
        }

        /// <summary>
        /// 清除左右的字符串
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <param name="forRemoving">移除的字符串</param>
        /// <returns>字符串</returns>
        public static string Trim(this string s, string forRemoving)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.TrimEnd(forRemoving).TrimStart(forRemoving);
        }

        /// <summary>
        /// 清除左边的字符串
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <param name="forRemoving">移除的字符串</param>
        /// <returns>字符串</returns>
        public static string TrimStart(this string s, string forRemoving)
        {
            if (string.IsNullOrEmpty(s)) return s;
            while (s.StartsWith(forRemoving, StringComparison.InvariantCultureIgnoreCase))
            {
                s = s.Substring(forRemoving.Length);
            }
            return s;
        }

        /// <summary>
        /// 清除右边的字符串
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <param name="forRemoving">移除的字符串</param>
        /// <returns>字符串</returns>
        public static string TrimEnd(this string s, string forRemoving)
        {
            if (string.IsNullOrEmpty(s)) return s;
            while (s.EndsWith(forRemoving, StringComparison.InvariantCultureIgnoreCase))
            {
                s = s.Remove(s.LastIndexOf(forRemoving, StringComparison.InvariantCultureIgnoreCase));
            }
            return s;
        }

        /// <summary>
        /// 提取指定开始和结束标记之间的内容
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <param name="start">开始字符串</param>
        /// <param name="end">结束字符串</param>
        /// <returns>字符串</returns>
        public static string GetTagContext(this string s, string start, string end)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var spos = s.IndexOf(start, StringComparison.OrdinalIgnoreCase);
            if (-1 == spos) return "";
            var epos = s.IndexOf(end, spos, StringComparison.OrdinalIgnoreCase);
            if (-1 == epos) return "";
            return s.Substring(spos, epos - spos).Remove(0, start.Length);
        }

        /// <summary>
        /// 返回字符串的长度，一个中文算两个长度
        /// </summary>        
        public static int LengthCN(this string s)
        {
            return Encoding.Default.GetBytes(s).Length;
        }

        /// <summary>
        /// 返回指定个数左边开始的字符串
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <param name="length">获取的长度</param>
        public static string GetLeft(this string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Length < length + 1) return s;
            return s.Substring(0, length);
        }


        /// <summary>
        /// 返回指定个数左边开始的字符串，一个中文算两个字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        public static string GetLeftCN(this string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return "";
            int btlength = System.Text.Encoding.Default.GetBytes(s.Trim()).Length;
            if (btlength < length + 1) return s;

            #region 截取字符串
            StringBuilder sb = new StringBuilder();

            string sr = "";                   //临时变量   
            int num = 0;                      //计数器   
            for (int i = 0; i < length; i++)
            {
                sr = s.Substring(i, 1);
                byte[] bt2 = System.Text.Encoding.Default.GetBytes(sr);
                if (bt2.Length == 1)
                {
                    num = num + 1;
                }
                if (bt2.Length == 2)
                {
                    num = num + 2;
                }
                if (num <= length)
                {
                    sb.Append(sr);
                }
                else
                {
                    break;
                }
            }
            #endregion

            return sb.ToString();
        }

        /// <summary>
        /// 清理指定字符串，大小写不敏感
        /// </summary>        
        /// <param name="s"></param>
        /// <param name="strOld">要替换的字符串，支持正则表达式，大小写不敏感</param>
        /// <param name="strNew">替换成的字符串</param>
        /// <returns></returns>
        public static string RegexReplace(this string s, string strOld, string strNew)
        {
            return Regex.Replace(s, strOld, strNew, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="spliterStr"></param>
        /// <returns></returns>
        public static string[] RegexSplit(this string s, string spliterStr)
        {
            if (string.IsNullOrEmpty(s)) return new string[0] { };
            if (s.IndexOf(spliterStr, StringComparison.CurrentCultureIgnoreCase) < 0) return new string[] { s };
            return Regex.Split(s, Regex.Escape(spliterStr), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 查找子字符串在字符串中的位置，大小写不敏感
        /// </summary>        
        /// <param name="s"></param>
        /// <param name="strFind">查找的子串</param>
        /// <returns></returns>
        public static int RegexIndexOf(this string s, string strFind)
        {
            return System.Globalization.CultureInfo.InvariantCulture.CompareInfo.IndexOf(s, strFind, System.Globalization.CompareOptions.IgnoreCase);
        }


        /// <summary>
        /// 查找子字符串在字符串中的位置，大小写不敏感
        /// </summary>        
        /// <param name="s"></param>
        /// <param name="strFind">查找的子串</param>
        /// <param name="startIndex">开始查找的位置</param>
        /// <returns></returns>
        public static int RegexIndexOf(this string s, string strFind, int startIndex)
        {
            return System.Globalization.CultureInfo.InvariantCulture.CompareInfo.IndexOf(s, strFind, startIndex, System.Globalization.CompareOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断是否是0到9组成的数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string s)
        {
            return Regex.IsMatch(s, @"^[0-9]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证字符串是否是电子邮件格式
        /// </summary>
        public static bool IsValidEmailAddress(this string s)
        {
            return Regex.IsMatch(s, @"^[\w-\.]+@([\w-]+\.)+[\w-]{1,}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断字符串是否是中文
        /// </summary>        
        public static bool IsChineseCharter(this string s)
        {
            return Regex.IsMatch(s, @"^[\u4E00-\u9FA5]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断字符串是否只包含字母数字和下划线
        /// </summary>
        public static bool IsCharDigitAndUnderLine(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return Regex.IsMatch(s, @"^[0-9a-zA-Z_]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检查字符串是否为正确的二级域名头，字母数字和中横线组成，且不能以中横线打头
        /// </summary>        
        public static bool IsSubDomain(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return Regex.IsMatch(s, @"^[A-Za-z0-9][A-Za-z0-9-]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检查字符串是否为3或6位的标准合法颜色值
        /// </summary>    
        public static bool IsColorValue(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s.Length != 3 && s.Length != 6) return false;
            return !Regex.IsMatch(s, @"[^0-9a-fA-F]", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检查是否有效的Url地址
        /// </summary>        
        public static bool IsURL(this string s)
        {
            return Regex.IsMatch(s, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        


    }
}
