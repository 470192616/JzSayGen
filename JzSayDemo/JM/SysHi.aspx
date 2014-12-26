<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="SysHi.aspx.cs" Inherits="JzSayDemo.JM.SysHi" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerJsAndCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Holder1" runat="server">
    做有价值的事。
    <input type="button" onclick="parent.JSBox.Alert('abc','s')" value="alert" />
    <input type="button" onclick="parent.JSTipBox.Show('你好')" value="tip" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
    
</asp:Content>
