using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using JzSayDemo.ClsDll;
using JzSayGen;
using JzSayDemo.JM.WUCLib;


namespace JzSayDemo.JM
{


    public partial class WaterFall : UIPageBase
    {
        class J
        {
            public int K { get; set; }
            public int H { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetQueryStr("createtest").IsNullOrEmpty() == false)
            {
                using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
                {
                    var dts = db.IntroLib.OrderByDescending(x => x.CreateTS).ToList();
                    string html = UIAscx.RenderView<UCTest>("~/JM/WUCLib/UCTest.ascx", (c) =>
                    {
                        c.ListData = dts;
                    });
                    Response.Clear();
                    Response.Write(html);
                    Response.End();
                }
            }

            if (this.GetQueryStr("ajax") == "1")
            {

                Int32 pSize = 10;
                Int32 p = this.GetQueryInt32("page", 1);
                List<J> ldat = new List<J>();
                if (p < 10)
                {
                    System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
                    for (int i = 0; i < pSize; i++)
                    {
                        ldat.Add(new J() { K = (p - 1) * pSize + i, H = random.Next(100, 555) });
                    }
                }
                Response.Clear();
                Response.Write(new JavaScriptSerializer().Serialize(ldat));
                Response.End();
                return;
            }
        }
    }
}