using System;
using System.Threading;
using System.Web.UI.WebControls;
using System.Configuration;
using Fraudpointer.API;
using Fraudpointer.API.Models;
using System.Web;

namespace WebApplicationClientExample
{
    public partial class Checkout : System.Web.UI.Page
    {
        #region private fields

        private const string ASSESSMENT_SESSION = "ASSESSMENT_SESSION";

        private readonly string _fraudPointerUrl = ConfigurationManager.AppSettings["FraudPointerUrl"];
        private readonly string _fraudPointerApiBaseUrl = ConfigurationManager.AppSettings["FraudPointerApiBaseUrl"];
        private readonly string _fraudPointerScriptUrl = ConfigurationManager.AppSettings["FraudPointerScriptUrl"];
        private readonly string _fraudPointerApiKey = GetFraudpointerApiKey();

        private IClient _client;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _client = GetFraudpointerClient(_fraudPointerUrl, _fraudPointerApiBaseUrl, _fraudPointerApiKey);

            hdnfldSessionId.Value = GetOrCreateAssessmentSession().Id.ToString();

            lblAcmeOrderNumber.Text = GetAcmeOrderNumber();

        } // Page_load ()
        //---------------

        protected void BoxCityFrom_Init(Object o, EventArgs e)
        {
            // Cities From
            lstBoxCityFrom.Items.Add(new ListItem("London", "1"));
            lstBoxCityFrom.Items.Add(new ListItem("Madrid", "2"));
            lstBoxCityFrom.Items.Add(new ListItem("Paris", "3"));
            lstBoxCityFrom.Items.Add(new ListItem("Rome", "4"));

        } // BoxCityFrom_Init ()
        //------------------------

        protected void BoxCityTo_Init(Object o, EventArgs e)
        {
            // Cities To
            lstBoxCityTo.Items.Add(new ListItem("Atlanta", "1"));
            lstBoxCityTo.Items.Add(new ListItem("Chicago", "2"));
            lstBoxCityTo.Items.Add(new ListItem("New York", "3"));
            lstBoxCityTo.Items.Add(new ListItem("Los Angeles", "4"));

        } // BoxCityTo_Init ()
        //------------------------


        protected void ButtonCalculatePrice_Init(Object o, EventArgs e)
        {
            btnCalculatePrice.Attributes.Add("onclick", GetCalculatePriceJavascript(false));

        } // ButtonCalculatePrice_Init ()
        //--------------------------------

        protected void BoxExpiresMonth_Init(Object o, EventArgs e)
        {
            for (int i = 1; i <= 12; i++)
            {
                lstBoxExpiresMonth.Items.Add(new ListItem(String.Format("{0:00}", i), i.ToString()));
            }

        } // BoxExpiresMonth_Init()
        //-------------------------

        protected void BoxExpiresYear_Init(Object o, EventArgs e)
        {

            for (int i = DateTime.Now.Year; i <= DateTime.Now.AddYears(20).Year; i++)
            {
                lstBoxExpiresYear.Items.Add(new ListItem(String.Format("{0:0000}", i), i.ToString()));
            }

        } // BoxExpiresYear_Init ()
        //-------------------------

        protected string FraudPointerScriptSource()
        {
            return _fraudPointerUrl + "/" + _fraudPointerScriptUrl;

        } // FraudPointerScriptSource ()
        //-------------------------------

        protected void Purchase_Click(Object o, EventArgs e)
        {
            // Create Checkout Event and send it over to FraudPointer Server
            Event l_eventCreated = CreateAndSendCheckoutEvent();

            // ask for Interim Fraud Assessment

            FraudAssessment l_fa = _client.CreateFraudAssessment(GetOrCreateAssessmentSession(), true);

            if (l_fa.Result == "Accept")
            {
                // We will try to charge customers credit card

                bool l_chargeResult = SendDataToBankForCharging();
                if (l_chargeResult)
                {
                    // success charging credit card
                    CreateAndSendSuccessfulPaymentEvent();

                    l_fa = _client.CreateFraudAssessment(GetOrCreateAssessmentSession(), false);

                    ResetSessionVars();

                    String l_messageToUser = "You have been successfully charged.";
                    String l_strGoBackUrl = "Default.aspx";

                    Response.Redirect("~/PurchaseResult.aspx?message=" + HttpUtility.UrlEncode(l_messageToUser) + "&go_back_url=" + HttpUtility.UrlEncode(l_strGoBackUrl), true);

                    return;

                }
                else
                {
                    // charging the card failed.

                    CreateAndSendFailedPaymentEvent();

                    // increase the number of failed payments
                    int l_numberOfFailedPaymentAttempts = GetNumberOfFailedPaymentAttempts();
                    l_numberOfFailedPaymentAttempts = l_numberOfFailedPaymentAttempts + 1;
                    Context.Session["number_of_failed_payment_attempts"] = l_numberOfFailedPaymentAttempts;

                    if (l_numberOfFailedPaymentAttempts > 3)
                    {
                        ResetSessionVars();
                        Response.Redirect("~/Default.aspx", true);
                        return;
                    }
                    else
                    {
                        Response.Redirect("~/Checkout.aspx", true);
                        return;
                    }

                }

            }
            else if (l_fa.Result == "Review")
            {
                ResetSessionVars();
                string l_strMessageToUser =
                    "We will hold your purchase request data and come back to you soon. Sorry for the delay.";
                string l_strGoBackUrl = "Default.aspx";
                Response.Redirect("~/PurchaseResult.aspx?message=" + HttpUtility.UrlEncode(l_strMessageToUser) + "&go_back_url=" + HttpUtility.UrlEncode(l_strGoBackUrl), true);
                return;
            }
            else
            {
                // this is a Reject case
                _client.CreateFraudAssessment(GetOrCreateAssessmentSession(), false);
                ResetSessionVars();
                string l_strMessageToUser =
                    "Sorry, but we cannot process your request. Your data has been declined.";
                Response.Redirect("~/PurchaseResult.aspx?message=" + HttpUtility.UrlEncode(l_strMessageToUser), true);
                return;

            }

        } // Purchase_Click ()
        //---------------------

        protected void StartOver_Click(Object o, EventArgs e)
        {
            Context.Session["fraudpointer_assessment_session"] = null;
            Response.Redirect("Checkout.aspx", true);
        } // StartOver_Click()
        //---------------------

        protected string GetAcmeOrderNumber()
        {
            String l_acmeOrderNumber = (String)Context.Session["acme_order_number"];
            if (l_acmeOrderNumber == null)
            {
                Random l_random = new Random(DateTime.Now.Hour * 60 + DateTime.Now.Second);
                l_acmeOrderNumber = l_random.Next(1000, 100000).ToString();
                Context.Session["acme_order_number"] = l_acmeOrderNumber;
            }
            return l_acmeOrderNumber;

        } // GetAcmeOrderNumber ()
        //-------------------------

        protected int GetNumberOfFailedPaymentAttempts()
        {
            int l_numberOfFailedPaymentAttempts = 0;
            if (Context.Session["number_of_failed_payment_attempts"] == null)
            {
                Context.Session["number_of_failed_payment_attempts"] = l_numberOfFailedPaymentAttempts;
            }
            else
            {
                l_numberOfFailedPaymentAttempts = (int)Context.Session["number_of_failed_payment_attempts"];
            }

            return l_numberOfFailedPaymentAttempts;
        } // GetNumberOfFailedPaymentAttempts()
        //--------------------------------------

        protected AssessmentSession GetOrCreateAssessmentSession()
        {
            // check whether you have session in SESSION
            AssessmentSession l_assessmentSession = (AssessmentSession)Context.Session["fraudpointer_assessment_session"];

            if (l_assessmentSession == null)
            {
                try
                {
                    l_assessmentSession = _client.CreateAssessmentSession();
                    Context.Session["fraudpointer_assessment_session"] = l_assessmentSession;
                    return l_assessmentSession;
                }
                catch (ClientException ex)
                {
                    return null;
                }

            }
            return l_assessmentSession;


        } // GetOrCreateAssessmentSession ()
        //-----------------------------------


        #region private methods

        private void MessageToUser(string i_messageToUser)
        {
            Label lb = (Label)Master.FindControl("lblMessageToUser");
            lb.Text = i_messageToUser;

        } // MessageToUser()
        //------------------

        private static string GetFraudpointerApiKey()
        {
            //throw new NotImplementedException("Uncomment the following and put in web.config the API KEY that you have been provided by FraudPointer Support. Contact: support@fraudpointer.com if you need more help.");
            return ConfigurationManager.AppSettings["FraudPointerApiKey"];

        } // GetFraudpointerApiKey ()
        //----------------------------

        private void SaveToSession(AssessmentSession i_assessmentSession)
        {
            Session.Add(ASSESSMENT_SESSION, i_assessmentSession);

        } // SaveToSession ()
        //--------------------

        private string GetCalculatePriceJavascript(bool i_returnValue)
        {
            string calculate_price = "javascript: calculate_price('" + lstBoxCityFrom.ClientID + "', '" +
                lstBoxCityTo.ClientID + "', '" + lblPriceValue.ClientID + "' ); return " + (i_returnValue ? "true" : "false") + ";";
            return calculate_price;
        } // GetCalculatePriceJavascript ()
        //--------------------------------

        private Event CreateAndSendCheckoutEvent()
        {
            Event l_eventToCreate = new Event(Event.CheckoutEvent);

            // ACME_DEPARTURE_CITY is an Account Custom Session Attribute defined in Fraud Pointer Application                        
            l_eventToCreate.AddData("ACME_DEPARTURE_CITY", lstBoxCityFrom.Items[lstBoxCityFrom.SelectedIndex].Text);

            // ACME_ARRIVAL_CITY is an Account Custome Session Attribute defined in Fraud Pointer Application
            l_eventToCreate.AddData("ACME_ARRIVAL_CITY", lstBoxCityTo.Items[lstBoxCityTo.SelectedIndex].Text);

            // PURCHASE_AMOUNT is a System Session Attribute
            l_eventToCreate.AddData("PURCHASE_AMOUNT", CalculatePrice(lstBoxCityFrom.SelectedValue, lstBoxCityTo.SelectedValue));

            // CC_HASH is a System Attribute. You should always use the CreditCardHash to hash your 
            // Credit Card Number and send it to Fraud Pointer Server.
            l_eventToCreate.AddData("CC_HASH", _client.CreditCardHash(txtbxCreditCardNumber.Text));

            // CC_CARD_HOLDER_NAME is a System Attribute.
            l_eventToCreate.AddData("CC_CARD_HOLDER_NAME", txtbxCardHolderName.Text);

            // CC_BANK_NAME is a System Attribute
            l_eventToCreate.AddData("CC_BANK_NAME", txtbxBankNameOfCard.Text);

            // CREDIT_CARD_FIRST_6_DIGITS
            if (String.IsNullOrEmpty(txtbxCreditCardNumber.Text) == false)
            {
                if (txtbxCreditCardNumber.Text.Length > 6)
                {
                    l_eventToCreate.AddData("CREDIT_CARD_FIRST_6_DIGITS", txtbxCreditCardNumber.Text.Substring(0, 6));
                }
                else
                {
                    l_eventToCreate.AddData("CREDIT_CARD_FIRST_6_DIGITS", txtbxCreditCardNumber.Text.Substring(0, txtbxCreditCardNumber.Text.Length));
                }
            }


            try
            {
                Event l_eventCreated = _client.AppendEventToAssessmentSession(GetOrCreateAssessmentSession(), l_eventToCreate);
                return l_eventCreated;
            }
            catch (ClientException ex)
            {
                return null;
            }

        } // CreateAndSendCheckoutEvent ()
        //----------------------------------

        private Event CreateAndSendSuccessfulPaymentEvent()
        {
            Event l_eventToCreate = new Event(Event.PurchaseEvent);
            l_eventToCreate.AddData("MERCHANT_REFERENCE", lblAcmeOrderNumber.Text);

            try
            {
                Event l_eventCreated = _client.AppendEventToAssessmentSession(GetOrCreateAssessmentSession(), l_eventToCreate);
                return l_eventCreated;
            }
            catch (ClientException ex)
            {
                return null;
            }
        } // CreateAndSendSuccessfulPaymentEvent ()
        //--------------------------------------------

        private Event CreateAndSendFailedPaymentEvent()
        {
            Event l_eventToCreate = new Event(Event.FailedPayment);
            l_eventToCreate.AddData("MERCHANT_REFERENCE", lblAcmeOrderNumber.Text);

            try
            {
                Event l_eventCreated = _client.AppendEventToAssessmentSession(GetOrCreateAssessmentSession(), l_eventToCreate);
                return l_eventCreated;
            }
            catch (ClientException ex)
            {
                return null;
            }
        } // CreateAndSendFailedPaymentEvent ()
        //--------------------------------------------

        private Decimal CalculatePrice(string i_valueFrom, string i_valueTo)
        {
            return Decimal.Parse(i_valueFrom) * Decimal.Parse(i_valueTo);
        } // CalculatePrice ()
        //--------------------

        private bool SendDataToBankForCharging()
        {
            return chckbxSuccessBankTransaction.Checked;
        } // SendDataToBankForCharging ()
        //--------------------------------

        private void ResetSessionVars()
        {
            Context.Session["number_of_failed_payment_attempts"] = 0;
            Context.Session["acme_order_number"] = 0;
            Context.Session["fraudpointer_assessment_session"] = null;
        } // ResetSessionVars()
        //----------------------

        private IClient GetFraudpointerClient(string i_fraudPointerUrl, string i_fraudPointerApiBaseUrl, string i_fraudPointerApiKey)
        {
            IClient l_client = ClientFactory.Construct(i_fraudPointerUrl + "/" + i_fraudPointerApiBaseUrl, i_fraudPointerApiKey);
            return l_client;

        } // GetFraudpointerClient();
        //----------------------------

        #endregion

    } // class Checkout
    //------------------

} // namespace WebApplicationClientExample
//-----------------------------------------

