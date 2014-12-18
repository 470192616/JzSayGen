<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCTest.ascx.cs" Inherits="JzSayDemo.JM.WUCLib.UCTest" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>
<%@ Register src="UCWebHeader.ascx" tagname="UCWebHeader" tagprefix="ucWeb" %>
<%@ Register src="UCWebFooter.ascx" tagname="UCWebFooter" tagprefix="ucWeb" %>


<ucWeb:UCWebHeader ID="UCWebHeader1" runat="server" />
<ul>
<%
    foreach (var d in this.ListData)
    { 
%>
    <li><%=d.Title %></li>
<%
    }
%>
</ul>
<ucWeb:UCWebFooter ID="UCWebFooter1" runat="server" />
