using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using JzSayGen;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 
    /// </summary>
    public class Bridge : AjaxBinHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPass"></param>
        /// <param name="safeCode"></param>
        /// <returns></returns>
        public string LoginSys(string userName, string userPass, string safeCode)
        {
            this.Code = 1;
            if (userName.IsNullOrEmpty()) return "请输入登录账户";
            if (userPass.IsNullOrEmpty()) return "请输入登录密码";

            if (this.CurrentContext.Session["ReLogin"] != null)
            {
                if (safeCode.IsNullOrEmpty()) return "请输入验证码";

                var usc = this.CurrentContext.Session[RandImageCode.CheckSessionKey] as string;
                if (string.IsNullOrEmpty(usc)) return "验证码获取失败请刷新验证码";

                if (usc.ToString().Equals(safeCode, StringComparison.OrdinalIgnoreCase) == false) return "验证码输入错误";
            }
            this.CurrentContext.Session["ReLogin"] = "yes";

            /*
            
            var su = XInnerShopUnion.GetByLoginName(userName);
            if (su == null) return "登录账户不存在";
            string cp = (userPass + su.LoginSalt).SHA256();
            if (su.LoginPass != cp) return "登录密码错误";

            MemberPassPort.SigIn(new MemberPassPort()
            {
                Lev = MemberLev.UnionMaster,
                ShopBaseId = 0,
                UnionId = su.Id
            }, this.CurrentContext);
            */

            this.Code = 0;
            return "";
        }
    }
}