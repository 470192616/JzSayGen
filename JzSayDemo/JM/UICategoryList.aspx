<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="UICategoryList.aspx.cs" Inherits="JzSayDemo.JM.UICategoryList" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerJsAndCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Holder1" runat="server">
    <div>
        <form method="get">            
            名称：<input type="text" name="SearchName" class="input" value="<%=this.SearchName%>" />
            状态：<div class="rule-single-select single-select">
                    <select name="SearchStat">
                        <option value="0">不限制</option>
                        <%=this.ShowOptions<Int32>(EnumExten.GetDictionary(typeof(EGenStat), 0, EGenStat.Unknow.GetInt32()), this.SearchStat)%>
                    </select>         
                  </div>
            <input type="submit" value="查询" class="btn" />
        </form>
    </div>

    <iframe src="/assets/blank.html" name="cmdTag" style="display:none;"></iframe>
    <table>
        <tr>            
            <th width="120">Url</th>
            <th>标题</th>
            <th width="120">发布时间</th>
            <th width="60">状态</th>
            <th width="180">操作</th>
        </tr>
    <%        
        StringBuilder moveCmd = new StringBuilder();
        for (int inx = 0, ldc = this.ListData.Count; inx < ldc; inx++)
        {
            var d = this.ListData[inx];
            var curKey = d.UrlKey;
            moveCmd.Length = 0;            
            if (d.Stat == EGenStat.Normal.GetInt32() || d.Stat == EGenStat.Hiden.GetInt32())
            {
                bool ds = d.Stat == EGenStat.Normal.GetInt32() ? true : false;
                moveCmd.AppendFormat("<a href=\"?cmd=state{1}&id={0}\" title=\"{2}\" target=\"cmdTag\"><i class=\"icon-{3}-cloud\"></i></a> ", curKey, ds ? "hide" : "show", ds ? "隐藏" : "显示", ds ? "download" : "upload");
            }
            
            bool dd = d.Stat == EGenStat.Delete.GetInt32() ? true : false;
            moveCmd.AppendFormat("<a href=\"?cmd={1}&id={0}\" target=\"cmdTag\" onclick=\"return {2}('{0}')\" title=\"{3}\"><i class=\"{4}\"></i></a> ", curKey, dd ? "restore" : "del", dd ? "SureRestore" : "SureDelete", dd ? "复活" : "删除", dd ? "icon-emo-saint" : "icon-trash");

            moveCmd.AppendFormat("<a href=\"UICategoryEdit.aspx?urlKey={0}&backUrl={1}\" title=\"编辑\"><i class=\"icon-edit\"></i></a> ", curKey, this.GoUrl);

            if (ldc > 1 && d.Stat != EGenStat.Delete.GetInt32())
            {
                if (inx == 0 && this.CurPageIndex == 1)
                {
                    //首页第一条
                    moveCmd.AppendFormat("<a href=\"?cmd=sortdown&id={0}&mid={1}\" title=\"下移\" target=\"cmdTag\"><i class=\"icon-down-big\"></i></a> ", curKey, this.ListData[inx + 1].UrlKey);
                }
                else if (inx == ldc - 1 && this.CurPageIndex == this.CurPageCount)
                {
                    //末页最后一条
                    moveCmd.AppendFormat("<a href=\"?cmd=sortup&id={0}&mid={1}\" title=\"上移\" target=\"cmdTag\"><i class=\"icon-up-big\"></i></a> ", curKey, this.ListData[inx - 1].UrlKey);
                }
                else
                {
                    //中间数据
                    moveCmd.AppendFormat("<a href=\"?cmd=sortup&id={0}&mid={1}\" title=\"上移\" target=\"cmdTag\"><i class=\"icon-up-big\"></i></a> ", curKey, this.ListData[inx - 1].UrlKey);
                    moveCmd.AppendFormat("<a href=\"?cmd=sortdown&id={0}&mid={1}\" title=\"下移\" target=\"cmdTag\"><i class=\"icon-down-big\"></i></a> ", curKey, this.ListData[inx + 1].UrlKey);
                }
            }
    %>
        <tr>                        
            <td><%=curKey%></td>            
            <td><%=d.Title%></td>
            <td><%=d.CreateTS.ParseDateTimeTS().ToString("yyyy-MM-dd")%></td>
            <td class="EGenStat<%=d.Stat %>"><%=EnumExten.GetTip(EnumExten.ParseInt32<EGenStat>(d.Stat))%></td>
            <td align="center">
                <%=moveCmd.ToString()%>                           
            </td>
        </tr>
    <%    
        }
    %>
    </table>
    <div class="pager"><%=this.ListData.Count == 0 ? "暂无数据" : PagerStr%></div>

    <script type="text/javascript">
        function SureRestore() {
            //
            return true;
        }
        function SureDelete() {
            //
            return true;
        }
        function CMDResult(cmd, id) {
            //
            location = '<%=this.GoUrl.UrlDecode() %>';
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
</asp:Content>
