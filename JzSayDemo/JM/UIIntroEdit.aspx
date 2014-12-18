<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="UIIntroEdit.aspx.cs" Inherits="JzSayDemo.JM.UIIntroEdit" %>
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
            <td><input type="text" name="urlKey" id="urlKey" maxlength="50" class="input" value="<%=HttpUtility.HtmlEncode(this.EditIntro.UrlKey)%>" /></td>
            <td><div id="urlKeyTip"></div></td>
        </tr>
        <tr>
            <th><span class="red fwb">*</span>标题：</th>
            <td><input type="text" name="title" maxlength="50" class="input" value="<%=HttpUtility.HtmlEncode(this.EditIntro.Title)%>" /></td>
            <td> </td>
        </tr>        
        <tr>
            <th>内容：</th>
            <td colspan="2"><textarea name="intro" class="editor" style="width:100%; height:80px;"><%=HttpUtility.HtmlEncode(this.EditIntro.Intro)%></textarea></td>            
        </tr>
        <tr>
            <th> </th>
            <td colspan="2">                
                <input type="submit" value="确定" class="btn" />
                <a href="<%=this.GetQueryStr("backUrl", "UIIntroList.aspx")%>" class="btn">回到列表</a>
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        var IsEdit = '<%=this.IsEdit %>';
        var BackUrl = '<%=this.GetQueryStr("backUrl", "UIIntroList.aspx")%>';

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
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
</asp:Content>
