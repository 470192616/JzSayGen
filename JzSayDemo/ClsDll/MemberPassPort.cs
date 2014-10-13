using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public Int64 UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; set; }

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
            string clientJson = General.JsSerialize.Serialize(client);
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
            return General.JsSerialize.Deserialize<MemberPassPort>(clientJson);
        }



    }
}