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
    public partial class UIKeyWordsEdit : ACPageBase
    {
        protected string IsEdit { get; set; }
        protected KeyWordsLib EditWord { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.IsEdit = "0";
            this.CheckUrlKey();
            this.CheckKeyWord();

            string urlKey = this.GetQueryStr("urlKey");
            if (urlKey.IsNullOrEmpty() == false)
            {
                using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
                {
                    this.EditWord = db.KeyWordsLib.FirstOrDefault(x => x.UrlKey == urlKey);
                    if (this.EditWord != null) this.IsEdit = "1";
                }
            }
            if (this.EditWord == null)
            {
                Int64 nk = MACPrimaryKey.NewKey1;
                this.EditWord = new KeyWordsLib() { UrlKey = nk.ToString() };
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
                var intro = db.KeyWordsLib.FirstOrDefault(x => x.UrlKey == urlKey);
                Response.Clear();
                Response.Write(intro == null ? "0" : "1");
                Response.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CheckKeyWord()
        {
            string keyWord = this.GetQueryStr("chkKeyWord");
            if (keyWord.IsNullOrEmpty()) return;
            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var intro = db.KeyWordsLib.FirstOrDefault(x => x.KeyWord == keyWord);
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

            string keyWord = this.GetPostStr("KeyWord");
            if (keyWord.IsNullOrEmpty()) return "关键词必填";

            Int32 keyWeight = this.GetPostInt32("KeyWeight");


            string actTip = "";
            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var editInfo = db.KeyWordsLib.FirstOrDefault(x => x.UrlKey == urlKey);
                if (editInfo == null)
                {
                    actTip = "添加";
                    db.KeyWordsLib.InsertOnSubmit(new KeyWordsLib()
                    {
                        UrlKey = urlKey,
                        KeyWord = keyWord,
                        KeyLength = keyWord.Length,
                        KeyWeight = keyWeight,
                        Stat = EGenStat.Normal.GetInt32(),
                        CreateTS = MACPrimaryKey.GetNowTS,
                        UpdateTS = MACPrimaryKey.GetNowTS
                    });
                }
                else
                {
                    actTip = "修改";
                    editInfo.KeyWeight = keyWeight;
                    editInfo.UpdateTS = MACPrimaryKey.GetNowTS;
                }
                db.SubmitChanges();
            }

            return actTip + "成功";
        }

    }
}