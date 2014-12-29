using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using JzSayDemo.ClsDll;
using JzSayGen;
using System.Web.Script.Serialization;

namespace JzSayDemo.JM
{
    public partial class UILogs : ACPageBase
    {
        class _Log
        {
            public string Type { get; set; }
            public string Json { get; set; }
            public string Date { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetQueryStr("ajax").IsNullOrEmpty()) return;

            List<_Log> ldat = new List<_Log>();
            var curPageIndex = this.GetQueryInt32("page", 1);
            Int32 pSize = 30;
            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var sql = LinqWhereHelper.True<logsLib>();
                var tmp = db.logsLib.Where(sql);

                var dts = tmp.OrderByDescending(x => x.WaterId).Skip((curPageIndex < 1 ? 0 : curPageIndex - 1) * pSize).Take(pSize).ToList();

                var dit = EnumExten.GetDictionary(typeof(ELogs));                
                foreach (var d in dts)
                {
                    ldat.Add(new _Log()
                    {
                        Type = dit.ContainsKey(d.EventKey) ? dit[d.EventKey] : "-",
                        Json = d.EventData,
                        Date = d.EventDate.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
            }
            Response.Clear();
            Response.Write(new JavaScriptSerializer().Serialize(ldat));
            Response.End();

        }
    }
}