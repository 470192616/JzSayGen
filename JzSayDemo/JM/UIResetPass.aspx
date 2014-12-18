<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="UIResetPass.aspx.cs" Inherits="JzSayDemo.JM.UIResetPass" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerJsAndCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Holder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
    <iframe src="/assets/blank.html" name="gfTag" style="display:none;"></iframe>
    <form method="post" class="form-inline" target="gfTag" action="?v=3" id="form1" enctype="multipart/form-data">							
        <table class="table table-bordered table-striped">                            
            <tr>
                <th width="120"><span class="fcr fwb">*</span>旧密码：</th>
                <td><input type="password" name="cPass1" maxlength="15" class="input" value="" /></td>                                
            </tr>
            <tr>
                <th><span class="fcr fwb">*</span>新密码：</th>
                <td><input type="password" name="cPass2" maxlength="15" class="input" value="" /></td>                                
            </tr>
            <tr>
                <th><span class="fcr fwb">*</span>确认密码：</th>
                <td><input type="password" name="cPass3" maxlength="15" class="input" value="" /></td>                                
            </tr>
            <tr>
                <th> </th>
                <td>
                    <input type="submit" class="btn" value=" 保存 " />
                </td>
            </tr>
        </table>                                                        
    </form>

    <script type="text/javascript">
        var BackUrl = '<%=this.GetQueryStr("backUrl", "SysHi.aspx")%>';
        function PostResult(msg) {
            //提交回调
            if (msg != '') {
                alert(msg);
                return;
            }
            alert('修改密码成功');
            location = BackUrl;
        }
    </script>
</asp:Content>
