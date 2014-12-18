using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using JzSayDemo.ClsDll;
using JzSayGen;
using System.IO;

namespace JzSayDemo.JM
{
    public partial class UICategoryEdit : PageBase
    {
        protected string IsEdit { get; set; }
        protected CategoryLib EditCate { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.IsEdit = "0";
            this.CheckUrlKey();

            string urlKey = this.GetQueryStr("urlKey");
            if (urlKey.IsNullOrEmpty() == false)
            {
                using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
                {
                    this.EditCate = db.CategoryLib.FirstOrDefault(x => x.UrlKey == urlKey);
                    if (this.EditCate != null) this.IsEdit = "1";
                }
            }
            if (this.EditCate == null)
            {
                Int64 nk = MACPrimaryKey.NewKey1;
                this.EditCate = new CategoryLib() { UrlKey = nk.ToString() };
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
                var intro = db.CategoryLib.FirstOrDefault(x => x.UrlKey == urlKey);
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

            string url = this.GetPostStr("url");

            string icon = this.GetPostStr("cImgNew");
            if (icon.IsNullOrEmpty())
            {
                icon = this.GetPostStr("cImgOld");
            }
            else
            {
                if (icon.StartsWith("/src/Temp/", StringComparison.OrdinalIgnoreCase) == false) return "没有操作权限";

                Int32 pos = icon.LastIndexOf('.');
                if (-1 == pos) return "不是图片文件";

                string eName = icon.Substring(pos).ToLower(); //扩展名
                if (".jpg.png.gif".Contains(eName) == false) return "图片文件必须是jpg、png或者gif格式";

                string tRoot = Server.MapPath("~").Replace("\\", "/").TrimEnd('/');
                if (File.Exists(tRoot + icon) == false) return "图片不存在";

                string wRoot = "/src/" + "" + DateTime.Now.ToString("yyyyMM") + "/";
                if (Directory.Exists(tRoot + wRoot) == false) Directory.CreateDirectory(tRoot + wRoot);

                Int32 v = icon.LastIndexOf('/');
                string fk = icon.Substring(v + 1, 11);

                string iconStr = wRoot + fk + eName;
                File.Copy(tRoot + icon, tRoot + iconStr);
                icon = iconStr;
            }

            string actTip = "";
            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var editInfo = db.CategoryLib.FirstOrDefault(x => x.UrlKey == urlKey);
                if (editInfo == null)
                {
                    actTip = "添加";
                    Int64? mx = db.CategoryLib.Max(x => (Int64?)x.ViewOrder);
                    db.CategoryLib.InsertOnSubmit(new CategoryLib()
                    {
                        UrlKey = urlKey,
                        Title = title,
                        Icon = icon,
                        AttJson = url,
                        ViewOrder = (mx.HasValue ? mx.Value : 0) + 1,
                        Stat = EGenStat.Normal.GetInt32(),
                        CreateTS = MACPrimaryKey.GetNowTS,
                        UpdateTS = MACPrimaryKey.GetNowTS
                    });
                }
                else
                {
                    actTip = "修改";
                    editInfo.Title = title;
                    editInfo.AttJson = url;
                    editInfo.Icon = icon;
                    editInfo.UpdateTS = MACPrimaryKey.GetNowTS;
                }
                db.SubmitChanges();
            }

            return actTip + "成功";
        }

    }
}