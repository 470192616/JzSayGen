using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JzSayGen;

namespace JzSayDemo.ClsDll
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MemberPassPort
    {
        /// <summary>
        /// 
        /// </summary>
        static readonly string MemberSessionKey = "MemberPassPortSessionKey";

        /// <summary>
        /// 
        /// </summary>
        static readonly string TokenCryptKey = "YouSeeTheYouAreGreateQQ470192616";

        /// <summary>
        /// 
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="hc"></param>
        /// <param name="client"></param>
        public static void SigIn(MemberPassPort client, HttpContext hc = null)
        {
            ReferSession(client, hc);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="hc"></param>
        public static void SigOut(HttpContext hc = null)
        {
            if (hc == null) hc = HttpContext.Current;
            hc.Session[MemberSessionKey] = null;
            hc.Session.Clear();
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="client"></param>
        /// <param name="hc"></param>
        public static void ReferSession(MemberPassPort client, HttpContext hc = null)
        {
            if (hc == null) hc = HttpContext.Current;
            string clientJson = Global.JsSerialize.Serialize(client);
            hc.Session[MemberSessionKey] = clientJson;
        }

        /// <summary>
        /// 获取登录对象
        /// </summary>
        /// <param name="hc"></param>
        /// <returns></returns>
        public static MemberPassPort GetSession(HttpContext hc = null)
        {
            if (hc == null) hc = HttpContext.Current;

            if (hc.Session[MemberSessionKey] == null) return null;

            string clientJson = hc.Session[MemberSessionKey].ToString();
            return Global.JsSerialize.Deserialize<MemberPassPort>(clientJson);
        }


        /// <summary>
        /// 获取当前登录用户的token字符串
        /// </summary>
        /// <param name="hc"></param>
        /// <returns></returns>
        public string TokenStrEncrypt(HttpContext hc = null)
        {
            if (hc == null) hc = HttpContext.Current;

            if (hc.Session[MemberSessionKey] == null) return "";
            string clientJson = hc.Session[MemberSessionKey].ToString();
            clientJson = DateTime.Now.ToString("yyyyMMddHHmmssfff") + clientJson; //随机干扰码

            return clientJson.AESEncryptSwap(TokenCryptKey);
        }

        /// <summary>
        /// 解析token字符串为用户数据
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public MemberPassPort TokenStrDecrypt(string tokenStr)
        {
            if (tokenStr.IsNullOrEmpty()) return null;

            try
            {
                tokenStr = tokenStr.AESDecryptSwap(TokenCryptKey);
                tokenStr = tokenStr.Substring(17); //移除干扰码
                return Global.JsSerialize.Deserialize<MemberPassPort>(tokenStr);
            }
            catch
            {
                return null;
            }
        }

    }
}