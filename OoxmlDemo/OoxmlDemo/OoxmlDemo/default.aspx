<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="OoxmlDemo._default" %>
<%@ Import Namespace="System.IO" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OOXML demo</title>
</head>
<body>
    <h1>Dokumenter</h1>
    <asp:Repeater runat="server" ID="documentList">
        <HeaderTemplate><ul></HeaderTemplate>
        <ItemTemplate><li><a href="/docx/Expand.aspx?file=<%# Server.HtmlEncode( Container.DataItem as string) %>"><%# Path.GetFileName( Container.DataItem as string) %></a></li></ItemTemplate>
        <FooterTemplate></ul></FooterTemplate>
    </asp:Repeater>
</body>
</html>
