using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using JzSayDemo.ClsDll;
using JzSayGen;


namespace JzSayDemo.JM
{
    public partial class UISqldo : ACPageBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected string SqlStatement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected DataTable Result = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        protected Exception Errors = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsFormPost() == false) return;

            this.SqlStatement = this.GetPostStr("SqlStatement");
            if (this.SqlStatement.IsNullOrEmpty()) return;

            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(this.SqlStatement, SqlHelper.DB_CONN_STRING))
                {
                    sda.Fill(this.Result);
                }
            }
            catch (Exception exs)
            {
                this.Errors = exs;
            }
        }
    }
}