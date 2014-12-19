using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using JzSayDemo.ClsDll;
using JzSayGen;

namespace JzSayDemo.JM
{
    public partial class UIResetPass : ACPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsFormPost() == false) return;

            string htmTemplate = "<htm><body><script>parent.PostResult('{0}');</script></body></htm>";
            string echoString = this.PostProcess();

            Response.Clear();
            Response.Write(string.Format(htmTemplate, echoString));
            Response.End();


        }

        protected string PostProcess()
        {
            string cPass1 = this.GetPostStr("cPass1");
            string cPass2 = this.GetPostStr("cPass2");
            string cPass3 = this.GetPostStr("cPass3");
            if (cPass1.IsNullOrEmpty()) return "请输入旧密码";
            if (cPass2.IsNullOrEmpty()) return "请输入新密码";
            if (cPass2.Length < 6) return "新密码长度必须大于5位";
            if (cPass3.IsNullOrEmpty()) return "请输入确认密码";
            if (cPass2 != cPass3) return "新密码和确认密码不同";

            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var fsu = db.WebSafe.FirstOrDefault(x => x.LoginName == this.Member.UserKey);
                if (fsu == null) return "登录错误，请重新登录";                
                if ((cPass1 + fsu.LoginSalt).SHA256().Equals(fsu.LoginPass) == false) return "旧密码输入错误";
                fsu.LoginPass = (cPass2 + fsu.LoginSalt).SHA256();

                db.SubmitChanges();
            }
            return "";
        }
    }
}