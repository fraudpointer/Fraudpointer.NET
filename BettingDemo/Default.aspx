<%@ Page Language="C#" MasterPageFile="~/BettingDemo.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BettingDemo.Default" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
<h1>Welcome</h1>
<%
    if (WalletHasMoney())
    {%>
<p>
Before you can place any bets, you will need to put money into your account.
Please, choose one of the payment methods below, and press the "Deposit" button
</p>
<%
    }%>
<asp:ListBox ID="lstbxPaymentMethod" runat="server" Rows="1" /><br />
<br />
<asp:Button ID="btnDepositToYourAccount" runat="server" Text="Deposit" OnClick="Deposit_Click"/>
<% if (ConfigurationManager.AppSettings["PlayFlag"] == "true") { %>
    
Or
<asp:HyperLink ID="hprlnkPlay" runat="server" NavigateUrl="~/Play.aspx" Text="Play" />

<% } %>
</asp:Content>