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
    public partial class UICategoryList : ACPageBase
    {
        protected string SearchName { get; set; }
        protected Int32 SearchStat { get; set; }

        protected List<CategoryLib> ListData = new List<CategoryLib>();
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
                var sql = LinqWhereHelper.True<CategoryLib>();
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

                var tmp = db.CategoryLib.Where(sql);
                Int32 rsCount = tmp.Count();
                UIPagePager pp = new UIPagePager(rsCount, pSize) { AttachUrlParameter = attachUrl };
                this.CurPageCount = pp.PageCount;
                if (this.CurPageIndex < 1 || this.CurPageIndex > this.CurPageCount) this.CurPageIndex = 1;

                this.ListData = tmp.OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.CreateTS).Skip((this.CurPageIndex - 1) * pSize).Take(pSize).ToList();
                this.PagerStr = pp.Show(this.CurPageIndex);
            }

        }

        /// <summary>
        /// @UK 待移动主键
        /// @MK 比较的主键
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        string SortSqlGet(string opt)
        {
            return @"   DECLARE @id nvarchar(50)
                        DECLARE @vo bigint
                        SELECT TOP 1 @id=UrlKey, @vo=ViewOrder FROM CategoryLib WITH(NOLOCK) WHERE UrlKey=@UK
                        IF(@id IS NOT NULL) BEGIN
	                        DECLARE @mid nvarchar(50)
	                        DECLARE @mvo bigint
	                        SELECT TOP 1 @mid=UrlKey, @mvo=ViewOrder FROM CategoryLib WITH(NOLOCK) WHERE UrlKey=@MK
	                        IF(@mid IS NOT NULL) BEGIN		
		                        IF(@vo = @mvo) BEGIN
			                        UPDATE CategoryLib SET UpdateTS=@uts, ViewOrder = @mvo " + opt + @" 1 WHERE UrlKey=@id
		                        END ELSE BEGIN
			                        UPDATE CategoryLib SET UpdateTS=@uts, ViewOrder = @mvo WHERE UrlKey=@id
			                        UPDATE CategoryLib SET UpdateTS=@uts, ViewOrder = @vo WHERE UrlKey=@mid
		                        END
	                        END
                        END ";
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
                case "sortup":
                    {
                        string mid = this.GetQueryStr("mid");
                        if (mid.IsNullOrEmpty()) return;
                        sql = this.SortSqlGet("+");
                        sp.Add(new SqlParameter("@MK", mid));
                        break;
                    }
                case "sortdown":
                    {
                        string mid = this.GetQueryStr("mid");
                        if (mid.IsNullOrEmpty()) return;
                        sql = this.SortSqlGet("-");
                        sp.Add(new SqlParameter("@MK", mid));
                        break;
                    }
                case "statehide":
                    {
                        sql = "UPDATE CategoryLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Hiden.GetInt32Str() + " WHERE UrlKey=@UK ";
                        break;
                    }
                case "stateshow":
                    {
                        sql = "UPDATE CategoryLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Normal.GetInt32Str() + " WHERE UrlKey=@UK ";
                        break;
                    }
                case "del":
                    {
                        sql = "UPDATE CategoryLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Delete.GetInt32Str() + " WHERE UrlKey=@UK ";
                        break;
                    }
                case "restore":
                    {
                        sql = "UPDATE CategoryLib SET UpdateTS=@uts, [Stat]=" + EGenStat.Normal.GetInt32Str() + " WHERE UrlKey=@UK ";
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