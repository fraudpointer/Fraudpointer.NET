<%@ Page Language="C#" MasterPageFile="~/AcmeBuyingTickets.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplicationClientExample.Default" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript" language='javascript'>function foo() {}</script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
<h1>Welcome</h1>
<asp:HyperLink ID="hprlnkPickupTickets" runat="server" NavigateUrl="~/Checkout.aspx" Text="Pick up your tickets" />
</asp:Content>