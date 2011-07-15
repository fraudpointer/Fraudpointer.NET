<%@ Page Language="C#" MasterPageFile="~/BettingDemo.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="BettingDemo.SignUp" %>

<asp:Content ID="SignUpMainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Sign Up</h1>
    <div id="login_div">
    <small><asp:HyperLink ID="hprlnkLogin" runat="server" NavigateUrl="~/Login.aspx" Text="Or Login"></asp:HyperLink></small>
    </div>
    
    <div id="personal_security">
        <div id="personal_details">
        <fieldset>
            <legend>Your personal details</legend>
            
            <table>
                <tr><th><asp:Label ID="lblTitle" runat="server" AssociatedControlID="lstbxTitle" Text="Title: (*)" /></th>
                    <td><asp:ListBox ID="lstbxTitle" runat="server" Rows="1" OnInit="ListBoxTitle_Init"/></td></tr>
                <tr><th><asp:Label ID="lblFirstname" runat="server" AssociatedControlID="txtbxFirstname" Text="Firstname: (*)" /></th>
                    <td><asp:TextBox ID="txtbxFirstname" runat="server"/></td></tr>
                <tr><th><asp:Label ID="lblSurname" runat="server" AssociatedControlID="txtbxSurname" Text="Surname: (*)" /></th>
                    <td><asp:TextBox ID="txtbxSurname" runat="server"/></td></tr>
                <tr><th><asp:Label ID="lblCountry" runat="server" AssociatedControlID="lstbxCountry" Text="Country: (*)" /></th>
                    <td><asp:ListBox ID="lstbxCountry" runat="server" Rows="1" OnInit="ListBoxCountry_Init" /></td></tr>
                <tr><th><asp:Label ID="lblAddressStreet" runat="server" AssociatedControlID="txtbxAddressStreet" Text="Address Street: (*)" /></th>
                    <td><asp:TextBox ID="txtbxAddressStreet" runat="server" /></td></tr>
                <tr><th><asp:Label ID="lblAddressNumber" runat="server" AssociatedControlID="txtbxAddressNumber" Text="Address Number:" /></th>
                    <td><asp:TextBox ID="txtbxAddressNumber" runat="server" /></td></tr>
                <tr><th><asp:Label ID="lblAddressCity" runat="server" AssociatedControlID="txtbxAddressCity" Text="Address City: (*)" /></th>
                    <td><asp:TextBox ID="txtbxAddressCity" runat="server" /></td></tr>
                <tr><th><asp:Label ID="lblAddressPostCode" runat="server" AssociatedControlID="txtbxAddressPostCode" Text="Address PostCode: (*)" /></th>
                    <td><asp:TextBox ID="txtbxAddressPostCode" runat="server" /></td></tr>
                <tr><th><asp:Label ID="lblDateOfBirth" runat="server" Text="Date of Birth: (*)" /></th>
                    <td><asp:Label ID="lblDateOfBirthDayOfMonth" runat="server" Text="Day:" />
                        <asp:ListBox ID="lstbxDateOfBirthDayOfMonth" runat="server" OnInit="ListBoxDateOfBirthDayOfMonth_Init" Rows="1"/>
                        <asp:Label ID="lblDateOfBirthMonth" runat="server" Text="Month:" />
                        <asp:ListBox ID="lstbxDateOfBirthMonth" runat="server" OnInit="ListBoxDateOfBirthMonth_Init" Rows="1"/>
                        <asp:Label ID="lblDateOfBirthYear" runat="server" Text="Year:" />
                        <asp:ListBox ID="lstbxDateOfBirthYear" runat="server" OnInit="ListBoxDateOfBirthYear_Init" Rows="1"/>
                    </td></tr>
                <tr><th><asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtbxEmail" Text="Email: (*)" /></th>
                    <td><asp:TextBox ID="txtbxEmail" runat="server" /></td></tr>
                <tr><th><asp:Label ID="lblTelephone" runat="server" AssociatedControlID="txtbxTelephone" Text="Telephone:" /></th>
                    <td><asp:TextBox ID="txtbxTelephone" runat="server" /></td></tr>
                <tr><th><asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtbxMobile" Text="Mobile:" /></th>
                    <td><asp:TextBox ID="txtbxMobile" runat="server" /></td></tr>
            </table>                        
            
        </fieldset> 
        </div>
    
        <div id="security_information">

        <fieldset>
            <legend>Security Information</legend>    

            <table>
                <tr><th><asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtbxUsername" Text="Username: (*)" /></th>
                    <td><asp:TextBox ID="txtbxUsername" runat="server" /></td></tr>
                <tr><th><asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtbxPassword" Text="Password: (*)"/></th>
                    <td><asp:TextBox ID="txtbxPassword" runat="server" TextMode="password"/></td></tr>
                <tr><th><asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtbxConfirmPassword" Text="Confirm: (*)"/></th>
                    <td><asp:TextBox ID="txtbxConfirmPassword" runat="server" TextMode="password"/></td></tr>
            </table>           

        </fieldset>

        <small>If you want to override date of registration, do it here:</small>
        <br />
        <br />
        <asp:Label ID="lblDateOfRegistration" runat="server" Text="Date of Registration: (*)" />
        <asp:Label ID="lblDateOfRegistrationDayOfMonth" runat="server" Text="Day:" />
        <asp:ListBox ID="lstbxDateOfRegistrationDayOfMonth" runat="server" OnInit="ListBoxDateOfRegistrationDayOfMonth_Init" Rows="1"/>
        <asp:Label ID="lblDateOfRegistrationMonth" runat="server" Text="Month:" />
        <asp:ListBox ID="lstbxDateOfRegistrationMonth" runat="server" OnInit="ListBoxDateOfRegistrationMonth_Init" Rows="1"/>
        <asp:Label ID="lblDateOfRegistrationYear" runat="server" Text="Year:" />
        <asp:ListBox ID="lstbxDateOfRegistrationYear" runat="server" OnInit="ListBoxDateOfRegistrationYear_Init" Rows="1"/>
        <br />
        <br />
        <small>If you want to override the IP on registration, do it here:</small>
        <br />
        <br />
        <asp:Label ID="lblIpOnRegistration" runat="server" Text="IP:" />
        <asp:TextBox ID="txtbxIpOnRegistration" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" OnClick="Submit_Click" Text="Register" Font-Size="Large" Font-Bold="true" />
        <asp:Label ID="lblMessageToUser" runat="server" ForeColor="Red" Text="" />

        </div>
    </div>
</asp:Content>