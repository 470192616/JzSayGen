using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JzSayGen;
using System.Text.RegularExpressions;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 
    /// </summary>
    public class General
    {
        /// <summary>
        /// ip地址匹配
        /// </summary>
        const string RegIPAds = @"^d{1,3}[.]d{1,3}[.]d{1,3}[.]d{1,3}$";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipstr"></param>
        /// <returns></returns>
        static bool IsIPAddress(string ipstr)
        {
            if (string.IsNullOrEmpty(ipstr) || ipstr.Length < 7 || ipstr.Length > 15) return false;
            return new Regex(RegIPAds, RegexOptions.IgnoreCase).IsMatch(ipstr);
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        public static string GetClientIP
        {
            get
            {
                var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理  
                    if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式  
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。  
                            result = result.Replace("  ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                        && temparyip[i].Substring(0, 3) != "10."
                                        && temparyip[i].Substring(0, 7) != "192.168"
                                        && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];        //找到不是内网的地址  
                                }
                            }
                        }
                        else if (IsIPAddress(result))  //代理即是IP格式  
                            return result;
                        else
                            result = null;        //代理中的内容  非IP，取IP  
                    }

                }

                string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

                if (string.IsNullOrEmpty(result)) result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

                if (string.IsNullOrEmpty(result)) result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }
        }

        /// <summary>
        /// 获取客户端数据
        /// </summary>
        public static string GetClientOS
        {
            get { 
                return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            }
        }
    }
    
    /// <summary>
    /// 公共状态
    /// </summary>
    public enum EGenStat
    {
        /// <summary>
        /// 未知1
        /// </summary>
        [EnumTip("未知")]
        Unknow = 1,

        /// <summary>
        /// 显示2
        /// </summary>
        [EnumTip("显示")]
        Normal = 2,

        /// <summary>
        /// 隐藏4
        /// </summary>
        [EnumTip("隐藏")]
        Hiden = 4,

        /// <summary>
        /// 已删除
        /// </summary>
        [EnumTip("已删除")]
        Delete = 8

    }
}