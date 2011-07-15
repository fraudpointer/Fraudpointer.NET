<%@ Page Language="C#" MasterPageFile="~/BettingDemo.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BettingDemo.Login" %>

<asp:Content ID="LoginContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <asp:Label ID="lblUsername" runat="server" Text="Username: " />
        <asp:TextBox ID="txtbxUsername" runat="server" />
        <br />
        <br />
        
        <asp:Label ID="lblPassword" runat="server" Text="Password: " />
        <asp:TextBox ID="txtbxPassword" runat="server" TextMode="Password" />
        <br />
        <br />
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="Login_Click"/>
        <br />
        <br />
        <asp:Label ID="lblMessageToUser" runat="server" Text="" ForeColor="Red" />
    </div>

    <asp:HyperLink ID="hprlnkSignUp" runat="server" Text="Or Sign Up" NavigateUrl="~/SignUp.aspx" />
</asp:Content>