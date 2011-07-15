<%@ Page Language="C#" MasterPageFile="~/AcmeBuyingTickets.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="WebApplicationClientExample.Checkout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    
    <!-- =================================== -->
    <!-- This is necessary for FRAUD POINTER -->
    <!--                                     -->
    <script language="javascript" type="text/javascript" src='<%= FraudPointerScriptSource() %>'></script>
    <script language="javascript" type="text/javascript">
        window.onload = function() {
            fraudpointer.fp('<%= hdnfldSessionId.ClientID %>');
        }
    </script>
    <!-- end of FRAUD POINTER related javascript stuff -->
    <!-- ============================================= -->
    
    <script language="javascript" type="text/javascript" src="scripts/checkout.js"></script>
   
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <hr/>
    Order Number: <asp:Label ID="lblAcmeOrderNumber" runat="server" />
    <br/>
    
    <!-- Print the Number of Failed Payment Attempts -->
    <span class='failed_payment_attempts'><%= GetNumberOfFailedPaymentAttempts()>=1 ? "Failed Payment Attempts: " + GetNumberOfFailedPaymentAttempts() : "" %></span>
    <!-- =========================================== -->
            
    <fieldset>
        <legend>Tickets from Europe to USA - Pick up your tickets</legend>
        <asp:HiddenField ID="hdnfldSessionId" runat="server" />
               
        <asp:Label ID="lblBoxCityFrom" runat="server" AssociatedControlID="lstBoxCityFrom" Text="Travel From:" />
        <asp:ListBox ID="lstBoxCityFrom" runat="server" SelectionMode="Single" Rows="1" OnInit="BoxCityFrom_Init" /><br />
        
        <asp:Label ID="lblBoxCityTo" runat="server" AssociatedControlID="lstBoxCityTo" Text="Travel To:" />
        <asp:ListBox ID="lstBoxCityTo" runat="server" SelectionMode="Single" Rows="1" OnInit="BoxCityTo_Init" /><br />
        
        <asp:Button ID="btnCalculatePrice" runat="server" CausesValidation="false" UseSubmitBehavior="false" Text="Calculate Price" OnInit="ButtonCalculatePrice_Init"/>
        
        <fieldset>
        
            <legend>Charging Details:</legend>
            
            <asp:Label ID="lblPrice" runat="server" Text="Price:" />
            <asp:Label ID="lblPriceValue" runat="server" /><br />
            
            <asp:Label ID="lblCreditCardNumber" runat="server" AssociatedControlID="txtbxCreditCardNumber" Text="Credit Card:" />
            <asp:TextBox ID="txtbxCreditCardNumber" runat="server" /><br />
            
            <asp:Label ID="lblExpires" runat="server" Text="Expires: Month: " AssociatedControlId="lstBoxExpiresMonth"/>
            <asp:ListBox ID="lstBoxExpiresMonth" runat="server" SelectionMode="Single" Rows="1" OnInit="BoxExpiresMonth_Init" />        
            <asp:Label ID="lblExpiresYear" runat="server" Text="Year: " AssociatedControlID="lstBoxExpiresYear"/>
            <asp:ListBox ID="lstBoxExpiresYear" runat="server" SelectionMode="Single" Rows="1" OnInit="BoxExpiresYear_Init" /><br />
            
            <asp:Label ID="lblCardHolderName" runat="server" Text="Card Holder Name: " AssociatedControlID="txtbxCardHolderName" />
            <asp:TextBox ID="txtbxCardHolderName" runat="server" /><br />
            
            <asp:Label ID="lblBankNameOfCard" runat="server" Text="Bank Name of Card: " AssociatedControlID="txtbxBankNameOfCard" />
            <asp:TextBox ID="txtbxBankNameOfCard" runat="server" /><br />
            
            <asp:Label ID="lblCcv" runat="server" Text="CCV:" AssociatedControlID="txtbxCcv" />
            <asp:TextBox ID="txtbxCcv" runat="server" /><br />
        
            <asp:CheckBox ID="chckbxSuccessBankTransaction" runat="server" Checked="true" Text="Succeed with Bank?"/><br />
            <asp:Button ID="btnPurchase" runat="server" OnClick="Purchase_Click" Text="Purchase" UseSubmitBehavior="true" />
        </fieldset>        
        
    </fieldset>
    
    <%= "Assessment Session Id: " + GetOrCreateAssessmentSession().Id %>
    <asp:Button ID="btnStartOver" runat="server" Text="Clear Assessment Session and Start Over" OnClick="StartOver_Click"/>
    
</asp:Content>

