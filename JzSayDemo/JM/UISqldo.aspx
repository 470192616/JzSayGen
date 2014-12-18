<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="UISqldo.aspx.cs" Inherits="JzSayDemo.JM.UISqldo" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerJsAndCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Holder1" runat="server">
    <form name="" method="post">
        <textarea name="SqlStatement" style="width:80%; height:160px;"><%=this.SqlStatement%></textarea>
        <br /><br />
        <input type="submit" value="执行输入Sql" class="btn" />
        <a href="?" class="btn">重新输入语句</a>
        <br /><br />
    </form>
    <%
        if (this.Errors != null)
        {
            Response.Write(this.Errors.ToString());
        }

        Response.Write("<table class=\"table table-striped table-bordered table-hover\">");
        Response.Write("<tr>");
        for (int i = 0; i < Result.Columns.Count; i++)
        {
            Response.Write("<th>" + Result.Columns[i].ColumnName + "</th>");
        }
        Response.Write("</tr>");
        Int32 rc = Result.Rows.Count;
        if (rc > 0)
        {            
            for (int i = 0; i < rc; i++)
            {
                Response.Write("<tr>");
                for (int k = 0; k < Result.Columns.Count; k++)
                {
                    Response.Write("<td>");
                    Response.Write(Result.Rows[i][Result.Columns[k].ColumnName]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }            
        }
        Response.Write("</table>");
    %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
</asp:Content>
