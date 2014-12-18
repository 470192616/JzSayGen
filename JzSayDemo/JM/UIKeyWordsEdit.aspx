<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="UIKeyWordsEdit.aspx.cs" Inherits="JzSayDemo.JM.UIKeyWordsEdit" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerJsAndCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Holder1" runat="server">
    <iframe src="/assets/blank.html" name="fTag" style="display:none;"></iframe>
    <form method="post" target="fTag" action="" enctype="multipart/form-data">    
    <table>
        <tr>
            <th width="100"><span class="red fwb">*</span>Url：</th>
            <td><input type="text" name="urlKey" id="urlKey" maxlength="50" class="input" value="<%=HttpUtility.HtmlEncode(this.EditWord.UrlKey)%>" /></td>
            <td><div id="urlKeyTip"></div></td>
        </tr>
        <tr>
            <th><span class="red fwb">*</span>关键词：</th>
            <td><input type="text" name="KeyWord" id="KeyWord" maxlength="20" class="input" value="<%=HttpUtility.HtmlEncode(this.EditWord.KeyWord)%>" /></td>
            <td><div id="KeyWordTip"></div></td>
        </tr>        
        <tr>
            <th>权重值：</th>
            <td><input type="text" name="KeyWeight" maxlength="50" class="input" value="<%=this.EditWord.KeyWeight%>" /></td>
            <td> </td>
        </tr>
        <tr>
            <th> </th>
            <td colspan="2">                
                <input type="submit" value="确定" class="btn" />
                <a href="<%=this.GetQueryStr("backUrl", "UIKeyWordsList.aspx")%>" class="btn">回到列表</a>
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        var IsEdit = '<%=this.IsEdit %>';
        var BackUrl = '<%=this.GetQueryStr("backUrl", "UIKeyWordsList.aspx")%>';

        function UrlKeyExists() {
            //
            var urlk = $('#urlKey').val();
            if (urlk == '') {
                $('#urlKeyTip').html('<i class="w">必填</i>');
                return;
            }
            var curl = '?chkUrlKey=' + urlk;
            $.get(curl, function (r) {
                $('#urlKeyTip').html(r == '0' ? '' : '<i class="e">已存在</i>');
            });
        }
        function KeyWordExists() {
            //
            var kw = $('#KeyWord').val();
            if (kw == '') {
                $('#KeyWordTip').html('<i class="w">必填</i>');
                return;
            }
            var curl = '?chkKeyWord=' + kw;
            $.get(curl, function (r) {
                $('#KeyWordTip').html(r == '0' ? '' : '<i class="e">已存在</i>');
            });
        }
        function PostResult(msg) {
            //提交回调            
            alert(msg);
            if (-1 == msg.indexOf('成功')) return;
            location = BackUrl;
        }
        $(function () {
            //
            $('#urlKey').blur(function () {
                //
                UrlKeyExists();
            });

            $('#KeyWord').blur(function () {
                KeyWordExists();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
</asp:Content>
