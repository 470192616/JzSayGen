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
    public partial class SysHi : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(MACPrimaryKey.GetNowTS.ToString().Substring(6));
        }
    }
}