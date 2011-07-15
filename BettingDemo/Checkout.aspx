<%@ Page Language="C#" MasterPageFile="~/BettingDemo.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="BettingDemo.Checkout" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head" >
 
    <script language="javascript" type="text/javascript" src='<%= FraudPointerScriptSource() %>'></script>
    <script language="javascript" type="text/javascript">
        window.onload = function() {
            fraudpointer.fp('<%= hdnfldSessionId.ClientID %>');
        }
    </script>

      
</asp:Content>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <h1>Deposit</h1>
    <p>Here you will enter you data in order to deposit money to your Super Sporting Bet Account. After finishing, press "Deposit" button.</p>
    <div>
        <img src="images/credit_card_logo_erny-smaller.jpg" alt="credit card logos" />
    </div>
    
    <hr/>
    Order Number: <asp:Label ID="lblOrderNumber" runat="server" />
    <br/>
    
        <asp:HiddenField ID="hdnfldSessionId" runat="server" />    
        <asp:HiddenField ID="hdnfldPaymentMethod" runat="server" />
                   
        <span class='failed_payment_attempts'><%=GetNumberOfFailedPaymentAttempts() >= 1
                                     ? "Failed Payment Attempts: " + GetNumberOfFailedPaymentAttempts()
                                     : ""%>
                                              <%="<br/>"%></span>
                
        <div id="div_with_purchase_details">
        <table id="table_with_purchase_details">
                <% if (PaymentMethod() == "cc") { %>

            <tr><th><asp:Label ID="lblNameOnCard" runat="server" Text="Name on card:" /></th>
                <td><asp:TextBox ID="txtbxNameOnCard" runat="server" /></td></tr>
            <tr><th><asp:Label ID="lblCardNumber" runat="server" Text="Card number:" /></th>
                <td><asp:TextBox ID="txtbxCardNumber" runat="server" /></td></tr>
            <tr><th><asp:Label ID="lblBankNameOfCard" runat="server" Text="Bank Name of Card:" /></th>
                <td><asp:TextBox ID="txtbxBankNameOfCard" runat="server" /></td></tr>
            <tr><th><asp:Label ID="lblExpires" runat="server" Text="Expires:" /></th>
                <td><asp:Label ID="lblExpiresMonth" runat="server" Text="Month:" />
                    <asp:ListBox ID="lstBoxExpiresMonth" runat="server" SelectionMode="Single" Rows="1" OnInit="BoxExpiresMonth_Init" />        
                    <asp:Label ID="lblExpiresYear" runat="server" Text="Year: " AssociatedControlID="lstBoxExpiresYear" Font-Bold="true"/>
                    <asp:ListBox ID="lstBoxExpiresYear" runat="server" SelectionMode="Single" Rows="1" OnInit="BoxExpiresYear_Init" /></td></tr>
            <tr><th><asp:Label ID="lblCcv" runat="server" Text="CCV:" AssociatedControlID="txtbxCcv" Font-Bold="true"/></th>
                <td><asp:TextBox ID="txtbxCcv" runat="server" /></td></tr>
                
                    <% } %>    
            <tr><th><asp:Label ID="lblAmount" runat="server" Text="Amount:" Font-Bold="true" AssociatedControlID="txtbxAmount" /></th>
                <td><asp:TextBox ID="txtbxAmount" runat="server" /></td></tr>
        </table>
                                  
    <asp:CheckBox ID="chckbxSuccessCharging" runat="server" Checked="true" Text="Success On Charging" />&nbsp;
    <asp:Button ID="btnPurchase" runat="server" OnClick="Purchase_Click" Text="Deposit" /> 
    
    <% if (ConfigurationManager.AppSettings["PlayFlag"] == "true") { %>
    
    Or
    <asp:HyperLink ID="hplnkPlay" runat="server" NavigateUrl="~/Play.aspx" Text="Play" />
    
    <% } %>
         </div>
    <hr />    
    <%= "Assessment Session Id: " + GetOrCreateAssessmentSession().Id %>
    <asp:Button ID="btnStartOver" runat="server" Text="Clear Assessment Session and Start Over" OnClick="StartOver_Click"/>
    
    <br /><small>If you want to override date of deposit, do it here:</small><br />
    <asp:Label ID="lblDateOfDeposit" runat="server" Text="Date of Deposit: (*)" />
    <asp:Label ID="lblDateOfDepositDayOfMonth" runat="server" Text="Day:" />
    <asp:ListBox ID="lstbxDateOfDepositDayOfMonth" runat="server" OnInit="ListBoxDateOfDepositDayOfMonth_Init" Rows="1"/>
    <asp:Label ID="lblDateOfDepositMonth" runat="server" Text="Month:" />
    <asp:ListBox ID="lstbxDateOfDepositMonth" runat="server" OnInit="ListBoxDateOfDepositMonth_Init" Rows="1"/>
    <asp:Label ID="lblDateOfDepositYear" runat="server" Text="Year:" />
    <asp:ListBox ID="lstbxDateOfDepositYear" runat="server" OnInit="ListBoxDateOfDepositYear_Init" Rows="1"/>
    <br />
    <br />
        
</asp:Content>