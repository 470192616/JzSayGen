using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace JzSayGen
{
    /// <summary>
    /// 网络操作
    /// </summary>
    public class WebHelper
    {
        /// <summary>
        /// post数据到地址
        /// </summary>
        /// <param name="gatewayUrl">http://baidu.com/action.cgi</param>
        /// <param name="postData">a=1&amp;b=2&amp;c=3</param>
        /// <returns></returns>
        public static string UrlPost(string gatewayUrl, string postData)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                client.Headers.Add("ContentLength", postData.Length.ToString());
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                byte[] responseData = client.UploadData(gatewayUrl, "POST", bytes);
                return Encoding.UTF8.GetString(responseData);
            }
        }

        /// <summary>
        /// get数据到地址
        /// </summary>
        /// <param name="gatewayUrl">http://baidu.com/action.cgi?a=1&amp;b=2&amp;c=3</param>
        /// <returns></returns>
        public static string UrlGet(string gatewayUrl)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return client.DownloadString(gatewayUrl);
            }
        }

    }
}
