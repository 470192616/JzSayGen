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
    public partial class UIIntroList : PageBase
    {
        protected string SearchName { get; set; }
        protected Int32 SearchStat { get; set; }

        protected List<IntroLib> ListData = new List<IntroLib>();
        protected string PagerStr { get; set; }
        protected Int32 CurPageIndex { get; set; }
        protected Int32 CurPageCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {   

            this.CmdProcess();

            this.SearchStat = this.GetQueryInt32("SearchStat");
            this.SearchName = this.GetQueryStr("SearchName");

            string attachUrl = "";
            this.CurPageIndex = this.GetQueryInt32("page", 1);
            Int32 pSize = 10;
            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var sql = LinqWhereHelper.True<IntroLib>();
                //sql = sql.And(x => x.UnionId == this.Member.UnionId);

                if (this.SearchStat > 0)
                {
                    sql = sql.And(x => x.Stat == this.SearchStat);
                    attachUrl = attachUrl + "&SearchStat=" + this.SearchStat.ToString();
                }

                if (this.SearchName.IsNullOrEmpty() == false)
                {
                    sql = sql.And(x => x.Title.Contains(this.SearchName));
                    attachUrl = attachUrl + "&SearchName=" + this.SearchName;
                }

                var tmp = db.IntroLib.Where(sql);
                Int32 rsCount = tmp.Count();
                UIPagePager pp = new UIPagePager(rsCount, pSize) { AttachUrlParameter = attachUrl };
                this.CurPageCount = pp.PageCount;
                if (this.CurPageIndex < 1 || this.CurPageIndex > this.CurPageCount) this.CurPageIndex = 1;

                this.ListData = tmp.OrderByDescending(x => x.CreateTS).Skip((this.CurPageIndex - 1) * pSize).Take(pSize).ToList();
                this.PagerStr = pp.Show(this.CurPageIndex);
            }

        }

        void CmdProcess()
        {
            string id = this.GetQueryStr("id");
            if (id.IsNullOrEmpty()) return;

            string sql = "";
            string cmd = this.GetQueryStr("cmd");
            List<SqlParameter> sp = new List<SqlParameter>();
            switch (cmd)
            {
                case "statehide":
                    {
                        sql = "UPDATE IntroLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Hiden.GetInt32Str() + " WHERE UrlKey=@UK ";
                        break;
                    }
                case "stateshow":
                    {
                        sql = "UPDATE IntroLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Normal.GetInt32Str() + " WHERE UrlKey=@UK ";
                        break;
                    }
                case "del":
                    {
                        sql = "UPDATE IntroLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Delete.GetInt32Str() + " WHERE UrlKey=@UK ";
                        break;
                    }
                case "restore":
                    {
                        sql = "UPDATE IntroLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Normal.GetInt32Str() + " WHERE UrlKey=@UK ";
                        break;
                    }
                default: return;
            }

            sp.Add(new SqlParameter("@UK", id));
            sp.Add(new SqlParameter("@uts", MACPrimaryKey.GetNowTS));

            SqlHelper.ADOExecuteNone(sql, sp.ToArray());

            Response.Clear();
            Response.Write("<htm><body><script>parent.CMDResult('" + cmd + "','" + id + "');</script></body></htm>");
            Response.End();
        }
    }
}