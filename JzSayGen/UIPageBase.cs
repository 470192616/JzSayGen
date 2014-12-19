using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JzSayGen
{
    /// <summary>
    /// 基础页
    /// </summary>
    public class UIPageBase : System.Web.UI.Page
    {        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStringArg"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected Int32 GetQueryInt32(string queryStringArg, Int32 defaultVal = 0)
        {
            return (Request.QueryString.Get(queryStringArg) ?? defaultVal.ToString()).ToInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStringArg"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected Int64 GetQueryInt64(string queryStringArg, Int64 defaultVal = 0)
        {
            return (Request.QueryString.Get(queryStringArg) ?? defaultVal.ToString()).ToInt64();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStringArg"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public Decimal GetQueryDecimal(string queryStringArg, Decimal defaultVal = 0)
        {
            return (Request.QueryString.Get(queryStringArg) ?? defaultVal.ToString()).ToDecimal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStringArg"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected string GetQueryStr(string queryStringArg, string defaultVal = "")
        {
            return (Request.QueryString.Get(queryStringArg) ?? defaultVal).Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected Int32 GetPostInt32(string formName, Int32 defaultVal = 0)
        {
            return (Request.Form.Get(formName) ?? defaultVal.ToString()).ToInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected Int64 GetPostInt64(string formName, Int64 defaultVal = 0)
        {
            return (Request.Form.Get(formName) ?? defaultVal.ToString()).ToInt64();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected Decimal GetPostDecimal(string formName, Decimal defaultVal = 0M)
        {
            return (Request.Form.Get(formName) ?? defaultVal.ToString()).ToDecimal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected string GetPostStr(string formName, string defaultVal = "")
        {
            return (Request.Form.Get(formName) ?? defaultVal).Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool IsFormPost()
        {
            return (Request.HttpMethod.ToUpper() == "POST");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionItem"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        protected string ShowOptions<T>(Dictionary<T, string> optionItem, T selectedValue)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in optionItem)
            {
                sb.AppendFormat("<option value=\"{0}\"{2}>{1}</option>", kv.Key, kv.Value, kv.Key.ToString() == selectedValue.ToString() ? " selected=\"selected\"" : "");
            }
            return sb.ToString();
        }
    }
}
