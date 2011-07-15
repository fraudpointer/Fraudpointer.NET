using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BettingDemo.DAL;
using Fraudpointer.API;
using Fraudpointer.API.Models;

namespace BettingDemo
{
    public partial class Checkout : System.Web.UI.Page
    {
        #region private fields

        private const string ASSESSMENT_SESSION = "ASSESSMENT_SESSION";

        private readonly string _fraudPointerUrl = ConfigurationManager.AppSettings["FraudPointerUrl"];
        private readonly string _fraudPointerApiBaseUrl = ConfigurationManager.AppSettings["FraudPointerApiBaseUrl"];
        private readonly string _fraudPointerScriptUrl = ConfigurationManager.AppSettings["FraudPointerScriptUrl"];
        private readonly string _fraudPointerApiKey = GetFraudpointerApiKey();
        private readonly string _reviewMapping = ConfigurationManager.AppSettings["ReviewMapping"];

        private IClient _client;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnfldPaymentMethod.Value = Request.Params["payment_method"];
            _client = GetFraudpointerClient(_fraudPointerUrl, _fraudPointerApiBaseUrl, _fraudPointerApiKey);

            hdnfldSessionId.Value = GetOrCreateAssessmentSession().Id.ToString();

            lblOrderNumber.Text = GetOrderNumber();

            if ( IsPostBack == false )
            {
                FillInDataFromSession();
            }

        } // Page_Load

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

        protected void StartOver_Click(Object o, EventArgs e)
        {
            Context.Session["fraudpointer_assessment_session"] = null;
            Response.Redirect("Checkout.aspx", true);
        } // StartOver_Click()
        //---------------------

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

        protected string GetOrderNumber()
        {
            String l_orderNumber = (String)Context.Session["order_number"];
            if (l_orderNumber == null)
            {
                Random l_random = new Random(DateTime.Now.Hour * 60 + DateTime.Now.Second);
                l_orderNumber = l_random.Next(1000, 100000).ToString();
                Context.Session["order_number"] = l_orderNumber;
            }
            return l_orderNumber;

        } // GetOrderNumber ()
        //-------------------------

        protected void Purchase_Click(Object o, EventArgs e)
        {
            StoreDataInSession();

            // Find Account
            string l_strUsername = Context.User.Identity.Name;
            Account l_accountFound = Account.FindByUsername(l_strUsername);
            if (l_accountFound == null)
            {
                String l_messageToUser = "Cannot locate account";
                String l_strGoBackUrl = "Default.aspx";

                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_messageToUser) + "&go_back_url=" + HttpUtility.UrlEncode(l_strGoBackUrl), true);

                return;
            }

            if ( hdnfldPaymentMethod.Value == "bank")
            {
                // create the transaction
                Transaction l_transaction = new Transaction();
                l_transaction.Amount = decimal.Parse(txtbxAmount.Text);
                l_transaction.DateOfTransaction = new DateTime(int.Parse(lstbxDateOfDepositYear.SelectedValue),
                                                               int.Parse(lstbxDateOfDepositMonth.SelectedValue),
                                                               int.Parse(lstbxDateOfDepositDayOfMonth.SelectedValue),
                                                               DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                                                               DateTimeKind.Local);
                l_transaction.AccountId = l_accountFound.Id;
                l_transaction.TypeOfTransaction = "deposit";
                l_transaction.BankDeposit = 1;
                l_transaction = Transaction.Create(l_transaction);
                //--------------------------------------------------

                ResetSessionVars();

                String l_messageToUser = "Thank you for your bank deposit.";
                String l_strGoBackUrl = "Default.aspx";

                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_messageToUser) + "&go_back_url=" + HttpUtility.UrlEncode(l_strGoBackUrl), true);

                return;
            }

            // Create Checkout Event and send it over to FraudPointer Server
            Event l_eventCreated = CreateAndSendCheckoutEvent(l_accountFound);

            // ask for Interim Fraud Assessment

            FraudAssessment l_fa = _client.CreateFraudAssessment(GetOrCreateAssessmentSession(), true);

            if (l_fa.Result == "Accept" || l_fa.Result == "Review" && _reviewMapping == "Accept")
            {
                // We will try to charge customers credit card

                bool l_chargeResult = chckbxSuccessCharging.Checked; 
                
                if (l_chargeResult)
                {
                    // create the transaction
                    Transaction l_transaction = new Transaction();
                    l_transaction.Amount = decimal.Parse(txtbxAmount.Text);
                    l_transaction.DateOfTransaction = new DateTime(int.Parse(lstbxDateOfDepositYear.SelectedValue),
                                                                   int.Parse(lstbxDateOfDepositMonth.SelectedValue),
                                                                   int.Parse(lstbxDateOfDepositDayOfMonth.SelectedValue),
                                                                   DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                                                                   DateTimeKind.Local);                    
                    l_transaction.AccountId = l_accountFound.Id;
                    l_transaction.TypeOfTransaction = "deposit";
                    l_transaction = Transaction.Create(l_transaction);
                    //--------------------------------------------------

                    // success charging credit card
                    CreateAndSendSuccessfulPaymentEvent();

                    l_fa = _client.CreateFraudAssessment(GetOrCreateAssessmentSession(), false);

                    ResetSessionVars();

                    String l_messageToUser = "You have been successfully charged.";
                    String l_strGoBackUrl = "Default.aspx";

                    Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_messageToUser) + "&go_back_url=" + HttpUtility.UrlEncode(l_strGoBackUrl), true);

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
                        Response.Redirect("~/Checkout.aspx?payment_method=" + hdnfldPaymentMethod.Value, true);
                        return;
                    }

                }

            }
            else if (l_fa.Result == "Review" && _reviewMapping == "Review")
            {
                ResetSessionVars();
                string l_strMessageToUser =
                    "We will hold your purchase request data and come back to you soon. Sorry for the delay.";
                string l_strGoBackUrl = "Default.aspx";
                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_strMessageToUser) + "&go_back_url=" + HttpUtility.UrlEncode(l_strGoBackUrl), true);
                return;
            }
            else
            {
                // this is a Reject case
                _client.CreateFraudAssessment(GetOrCreateAssessmentSession(), false);
                ResetSessionVars();
                string l_strMessageToUser =
                    "Sorry, but we cannot process your request. Your data has been declined.";
                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_strMessageToUser), true);
                return;

            }

        } // Purchase_Click ()
        //---------------------

        protected void ListBoxDateOfDepositDayOfMonth_Init(object sender, EventArgs e)
        {
            for (int i = 1; i <= 31; i++)
            {
                lstbxDateOfDepositDayOfMonth.Items.Add(new ListItem(String.Format("{0:00}", i), i.ToString()));
                if (DateTime.Now.Day == i)
                {
                    lstbxDateOfDepositDayOfMonth.SelectedIndex = i - 1;
                }
            }
        }

        protected void ListBoxDateOfDepositMonth_Init(object sender, EventArgs e)
        {
            for (int i = 1; i <= 12; i++)
            {
                lstbxDateOfDepositMonth.Items.Add(new ListItem(String.Format("{0:00}", i), i.ToString()));
                if (DateTime.Now.Month == i)
                {
                    lstbxDateOfDepositMonth.SelectedIndex = i - 1;
                }
            }
        }

        protected void ListBoxDateOfDepositYear_Init(object sender, EventArgs e)
        {
            for (int i = DateTime.Now.AddYears(-5).Year; i <= DateTime.Now.AddYears(30).Year; i++)
            {
                lstbxDateOfDepositYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (DateTime.Now.Year == i)
                {
                    lstbxDateOfDepositYear.SelectedIndex = i - 1 - DateTime.Now.AddYears(-5).Year + 1;
                }
            }            
        }

        private void StoreDataInSession ()
        {
            if ( hdnfldPaymentMethod.Value == "cc" )
            {
                string l_strNameOnCard = txtbxNameOnCard.Text;
                if ( String.IsNullOrEmpty(l_strNameOnCard) == false )
                {
                    Context.Session.Add("NAME_ON_CARD", l_strNameOnCard);                       
                }

                string l_strCardNumber = txtbxCardNumber.Text; 
                if (String.IsNullOrEmpty(l_strCardNumber) == false)
                {
                     Context.Session.Add("CARD_NUMBER", l_strCardNumber);
                }

                string l_strBankNameOfCard = txtbxBankNameOfCard.Text; 
                if (String.IsNullOrEmpty(l_strBankNameOfCard) == false)
                {
                     Context.Session.Add("BANK_NAME_OF_CARD", l_strBankNameOfCard);
                }

                string l_strExpiresMonth = lstBoxExpiresMonth.SelectedValue; 
                if (String.IsNullOrEmpty(l_strExpiresMonth) == false)
                {
                     Context.Session.Add("EXPIRES_MONTH", l_strExpiresMonth);
                }

                string l_strExpiresYear = lstBoxExpiresYear.SelectedValue; 
                if (String.IsNullOrEmpty(l_strExpiresYear) == false)
                {
                     Context.Session.Add("EXPIRES_YEAR", l_strExpiresYear);
                }

                string l_strCcv = txtbxCcv.Text; 
                if (String.IsNullOrEmpty(l_strCcv) == false)
                {
                     Context.Session.Add("CCV", l_strCcv);
                }

                decimal l_dAmount = 0;
                try
                {
                    l_dAmount = decimal.Parse(txtbxAmount.Text); 
                    Context.Session.Add("AMOUNT", l_dAmount);
                }
                catch (Exception ex)
                {
                }
                
            }    
        } // StoreDataInSession ()
        //-------------------------

        private void FillInDataFromSession()
        {
            if ( hdnfldPaymentMethod.Value == "cc" )
            {
                string l_strNameOnCard = (string) Context.Session["NAME_ON_CARD"];
                if (String.IsNullOrEmpty(l_strNameOnCard) == false)
                {
                    txtbxNameOnCard.Text = l_strNameOnCard;
                }

                string l_strCardNumber = (string) Context.Session["CARD_NUMBER"];
                if (String.IsNullOrEmpty(l_strCardNumber) == false )
                {
                    txtbxCardNumber.Text = l_strCardNumber;
                }

                string l_strBankNameOfCard = (string) Context.Session["BANK_NAME_OF_CARD"];
                if (String.IsNullOrEmpty(l_strBankNameOfCard) == false)
                {
                    txtbxBankNameOfCard.Text = l_strBankNameOfCard;
                }

                string l_strExpiresMonth = (string) Context.Session["EXPIRES_MONTH"];
                if ( String.IsNullOrEmpty(l_strExpiresMonth) == false)
                {
                    lstBoxExpiresMonth.SelectedValue = l_strExpiresMonth;
                }

                string l_strExpiresYear = (string) Context.Session["EXPIRES_YEAR"];
                if ( String.IsNullOrEmpty(l_strExpiresYear) == false)
                {
                    lstBoxExpiresYear.SelectedValue = l_strExpiresYear;
                }

                string l_strCcv = (string) Context.Session["CCV"];
                if ( String.IsNullOrEmpty(l_strCcv) == false)
                {
                    txtbxCcv.Text = l_strCcv;
                }

                decimal l_dAmount = 0;
                try
                {
                    l_dAmount = (decimal)Context.Session["AMOUNT"];
                    txtbxAmount.Text = l_dAmount.ToString();
                }
                catch (Exception ex)
                {                    
                }

            }    
        } // FillInDataFromSession ()
        //---------------------------

        private IClient GetFraudpointerClient(string i_fraudPointerUrl, string i_fraudPointerApiBaseUrl, string i_fraudPointerApiKey)
        {
            IClient l_client = ClientFactory.Construct(i_fraudPointerUrl + "/" + i_fraudPointerApiBaseUrl, i_fraudPointerApiKey);
            return l_client;

        } // GetFraudpointerClient();
        //----------------------------

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

        private Event CreateAndSendCheckoutEvent(Account i_account)
        {
            Event l_eventToCreate = new Event(Event.CheckoutEvent);

            // PURCHASE_AMOUNT is a System Session Attribute
            l_eventToCreate.AddData("PURCHASE_AMOUNT", txtbxAmount.Text);

            // CC_HASH is a System Attribute. You should always use the CreditCardHash to hash your 
            // Credit Card Number and send it to Fraud Pointer Server.
            l_eventToCreate.AddData("CC_HASH", _client.CreditCardHash(txtbxCardNumber.Text));

            // CC_CARD_HOLDER_NAME is a System Attribute.
            l_eventToCreate.AddData("CC_CARD_HOLDER_NAME", txtbxNameOnCard.Text);

            // CC_BANK_NAME is a System Attribute
            l_eventToCreate.AddData("CC_BANK_NAME", txtbxBankNameOfCard.Text);

            // CREDIT_CARD_FIRST_6_DIGITS
            if ( txtbxCardNumber.Text.Length>=6 )
            {
                l_eventToCreate.AddData("CREDIT_CARD_FIRST_6_DIGITS", txtbxCardNumber.Text.Substring(0, 6));    
            }            
            else
            {
                l_eventToCreate.AddData("CREDIT_CARD_FIRST_6_DIGITS", txtbxCardNumber.Text.Substring(0, txtbxCardNumber.Text.Length));    
            }

            // LET US SEND THE USER_E_MAIL 
            l_eventToCreate.AddData("USER_E_MAIL", i_account.Email);

            // LET US SEND THE USERNAME
            l_eventToCreate.AddData("USERNAME", i_account.Username);


            // first name
            l_eventToCreate.AddData("USER_FIRSTNAME", i_account.Firstname);

            // last name
            l_eventToCreate.AddData("USER_LASTNAME", i_account.Surname);

            // country of customer
            Country l_billingCountry = Country.Find(i_account.CountryId);
            if ( l_billingCountry != null )
            {
                l_eventToCreate.AddData("BILLING_COUNTRY", Country.Find(i_account.CountryId).Iso2);    
            }
            
            // address street
            if ( String.IsNullOrEmpty(i_account.AddressStreet) == false )
            {
                l_eventToCreate.AddData("BILLING_ADDRESS_STREET_NAME", i_account.AddressStreet);    
            }

            //BILLING_ADDRESS_STREET_NUMBER
            if (String.IsNullOrEmpty(i_account.AddressNumber) == false)
            {
                l_eventToCreate.AddData("BILLING_ADDRESS_STREET_NUMBER", i_account.AddressNumber);
            }

            //BILLING_ADDRESS_CITY
            if (String.IsNullOrEmpty(i_account.AddressCity) == false)
            {
                l_eventToCreate.AddData("BILLING_ADDRESS_CITY", i_account.AddressCity);
            }

            // BILLING_ADDRESS_POST_CODE
            if (String.IsNullOrEmpty(i_account.AddressPostCode) == false)
            {
                l_eventToCreate.AddData("BILLING_ADDRESS_POST_CODE", i_account.AddressPostCode);
            }
            
            // BILLING_TELEPHONE_NUMBER
            if (String.IsNullOrEmpty(i_account.Telephone) == false)
            {
                l_eventToCreate.AddData("BILLING_TELEPHONE_NUMBER", i_account.Telephone);
            }                   

            int l_iNumberOfDeposits = 0;
            l_iNumberOfDeposits = Transaction.NumberOfDeposits(i_account.Id, DateTime.MinValue);
            l_eventToCreate.AddData("SUPER_SPORTING_BET_NUMBER_OF_USER_S_DEPOSITS_SINCE_INITIAL_REGISTRATION_DATE", l_iNumberOfDeposits);

            int l_iNumberOfDaysSinceInitialRegistrationDate = 0;
            l_iNumberOfDaysSinceInitialRegistrationDate = (DateTime.Now - i_account.DateOfRegistration).Days;
            l_eventToCreate.AddData("SUPER_SPORTING_BET_DAYS_SINCE_INITIAL_REGISTRATION_DATE", l_iNumberOfDaysSinceInitialRegistrationDate);

            // SUPER_SPORTING_BET_LAST_30_DAYS__ACCUMULATED_VALUE_OF_USER_S_DEPOSITS
            decimal l_iLast30DaysAccumulatedValueOfUsersDeposits = 0;
            l_iLast30DaysAccumulatedValueOfUsersDeposits = Transaction.SumDepositsLaterThan(i_account.Id, DateTime.Now.AddDays(-30));
            l_eventToCreate.AddData("SUPER_SPORTING_BET_LAST_30_DAYS__ACCUMULATED_VALUE_OF_USER_S_DEPOSITS", l_iLast30DaysAccumulatedValueOfUsersDeposits);

            // SUPER_SPORTING_BET_LAST_30_DAYS__NUMBER_OF_USER_S_DEPOSITS
            int l_iNumberOfDepositsDuringLast30Days = 0;
            l_iNumberOfDepositsDuringLast30Days = Transaction.NumberOfDeposits(i_account.Id, DateTime.Now.AddDays(-30));
            l_eventToCreate.AddData("SUPER_SPORTING_BET_LAST_30_DAYS__NUMBER_OF_USER_S_DEPOSITS", l_iNumberOfDepositsDuringLast30Days);
            
            // "SUPER_SPORTING_BET_USER_S_IP_ADDRESS_ON_REGISTRATION"
            if ( String.IsNullOrEmpty(i_account.IpAddressOnRegistration) == false )
            {
                l_eventToCreate.AddData("SUPER_SPORTING_BET_USER_S_IP_ADDRESS_ON_REGISTRATION", i_account.IpAddressOnRegistration);                
            }

            // SUPER_SPORTING_BET_USER_HAS_AT_LEAST_ONE_BANK_DEPOSIT
            int l_iNumberOfBankDeposits = Transaction.NumberOfBankDeposits(i_account.Id, DateTime.MinValue);
            l_eventToCreate.AddData("SUPER_SPORTING_BET_USER_HAS_AT_LEAST_ONE_BANK_DEPOSIT", l_iNumberOfBankDeposits >= 1);

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

        protected string PaymentMethod ()
        {
            return Request.Params["payment_method"];
        }
      
        private Event CreateAndSendSuccessfulPaymentEvent()
        {
            Event l_eventToCreate = new Event(Event.PurchaseEvent);
            l_eventToCreate.AddData("MERCHANT_REFERENCE", lblOrderNumber.Text);            

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
            l_eventToCreate.AddData("MERCHANT_REFERENCE", lblOrderNumber.Text);

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
       
        private void ResetSessionVars()
        {
            Context.Session["number_of_failed_payment_attempts"] = 0;
            Context.Session["order_number"] = "0";
            Context.Session["fraudpointer_assessment_session"] = null;
        } // ResetSessionVars()
        //----------------------




    }
}
