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
    public partial class UIIntroEdit : PageBase
    {
        protected string IsEdit { get; set; }
        protected IntroLib EditIntro { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.IsEdit = "0";
            this.CheckUrlKey();

            string urlKey = this.GetQueryStr("urlKey");
            if (urlKey.IsNullOrEmpty() == false)
            {
                using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
                {
                    this.EditIntro = db.IntroLib.FirstOrDefault(x => x.UrlKey == urlKey);
                    if (this.EditIntro != null) this.IsEdit = "1";
                }
            }
            if (this.EditIntro == null)
            {
                Int64 nk = MACPrimaryKey.NewKey1;
                this.EditIntro = new IntroLib() { UrlKey = nk.ToString() };
            }

            if (!this.IsFormPost()) return;

            Response.Clear();
            Response.Write("<htm><body><script>parent.PostResult('" + this.PostProcess() + "');</script></body></htm>");
            Response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CheckUrlKey()
        {
            string urlKey = this.GetQueryStr("chkUrlKey");
            if (urlKey.IsNullOrEmpty()) return;
            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var intro = db.IntroLib.FirstOrDefault(x => x.UrlKey == urlKey);
                Response.Clear();
                Response.Write(intro == null ? "0" : "1");
                Response.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string PostProcess()
        {
            string urlKey = this.GetPostStr("urlKey");
            if (urlKey.IsNullOrEmpty()) return "url必填";
            if (urlKey.IsSubDomain() == false) return "url只能由字母、数字组成";

            string title = this.GetPostStr("title");
            if (title.IsNullOrEmpty()) return "标题必填";

            string intro = this.GetPostStr("intro");
            if (intro.IsNullOrEmpty()) return "内容必填";

            string actTip = "";
            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var editIntro = db.IntroLib.FirstOrDefault(x => x.UrlKey == urlKey);
                if (editIntro == null)
                {
                    actTip = "添加";
                    db.IntroLib.InsertOnSubmit(new IntroLib()
                    {
                        UrlKey = urlKey,
                        Title = title,
                        Intro = intro,
                        Stat = EGenStat.Normal.GetInt32(),
                        CreateTS = MACPrimaryKey.GetNowTS,
                        UpdateTS = MACPrimaryKey.GetNowTS
                    });
                }
                else
                {
                    actTip = "修改";
                    editIntro.Title = title;
                    editIntro.Intro = intro;
                    editIntro.UpdateTS = MACPrimaryKey.GetNowTS;
                }
                db.SubmitChanges();
            }

            return actTip + "成功";
        }

    }
}