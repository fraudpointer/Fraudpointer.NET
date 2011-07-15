<%@ Page Language="C#" MasterPageFile="~/BettingDemo.Master" AutoEventWireup="true" CodeBehind="Play.aspx.cs" Inherits="BettingDemo.Play" %>

<asp:Content ID="PlayContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <br />
    <asp:Label ID="lblAmount" runat="server" Text="Amount:"/>
    <asp:TextBox ID="txtbxAmount" runat="server" /><br />
    <br />
    <asp:Button ID="btnPlay" runat="server" OnClick="Play_Click" Text="Play" /> Or
    <asp:HyperLink ID="hprlnkDeposit" runat="server" NavigateUrl="~/Default.aspx" Text="Deposit"/>
</asp:Content>