using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BettingDemo.DAL;

namespace BettingDemo
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ListBoxTitle_Init (Object o, EventArgs e)
        {
            List<DAL.Title> l_titles = DAL.Title.All();

            foreach (var l_title in l_titles)
            {
                lstbxTitle.Items.Add(new ListItem(l_title.TitleName, l_title.Id.ToString()));
            }

        }

        protected void ListBoxCountry_Init(Object o, EventArgs e)
        {
            List<DAL.Country> l_countries = DAL.Country.All();

            foreach (var l_country in l_countries)
            {
                lstbxCountry.Items.Add(new ListItem(l_country.Name, l_country.Id.ToString()));
            }

        }

        protected void ListBoxDateOfBirthDayOfMonth_Init (Object o, EventArgs e)
        {
            for ( int i = 1; i<=31; i++ )
            {
                lstbxDateOfBirthDayOfMonth.Items.Add(new ListItem(String.Format("{0:00}", i), i.ToString()));
            }            
        } 

        protected void ListBoxDateOfBirthMonth_Init (Object o, EventArgs e)
        {
            for ( int i = 1; i<=12; i++)
            {
                lstbxDateOfBirthMonth.Items.Add(new ListItem(String.Format("{0:00}", i), i.ToString()));
            }
        }

        protected void ListBoxDateOfBirthYear_Init (Object o, EventArgs e)
        {
            for (int i = 1910; i <= DateTime.Now.AddYears(-18).Year; i++)
            {
                lstbxDateOfBirthYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            lstbxDateOfBirthYear.SelectedIndex = lstbxDateOfBirthYear.Items.Count - 1;
        }

        protected void ListBoxDateOfRegistrationDayOfMonth_Init(Object o, EventArgs e)
        {
            for (int i = 1; i <= 31; i++)
            {
                lstbxDateOfRegistrationDayOfMonth.Items.Add(new ListItem(String.Format("{0:00}", i), i.ToString()));
                if ( DateTime.Now.Day == i )
                {
                    lstbxDateOfRegistrationDayOfMonth.SelectedIndex = i - 1;
                }
            }
        }

        protected void ListBoxDateOfRegistrationMonth_Init(Object o, EventArgs e)
        {
            for (int i = 1; i <= 12; i++)
            {
                lstbxDateOfRegistrationMonth.Items.Add(new ListItem(String.Format("{0:00}", i), i.ToString()));
                if (DateTime.Now.Month == i)
                {
                    lstbxDateOfRegistrationMonth.SelectedIndex = i - 1;
                }
            }
        }

        protected void ListBoxDateOfRegistrationYear_Init(Object o, EventArgs e)
        {
            for (int i = DateTime.Now.AddYears(-5).Year; i <= DateTime.Now.AddYears(30).Year; i++)
            {
                lstbxDateOfRegistrationYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (DateTime.Now.Year == i)
                {
                    lstbxDateOfRegistrationYear.SelectedIndex = i - 1 - DateTime.Now.AddYears(-5).Year + 1;
                }
            }            
        }

        protected void Submit_Click (Object o, EventArgs e)
        {
            // collect data
            int l_iTitleId = int.Parse(lstbxTitle.SelectedValue);
            
            string l_strFirstname = txtbxFirstname.Text;
            if (l_strFirstname == "")
            {
                lblMessageToUser.Text = "You have to give Firstname";
                return;
            }

            string l_strSurname = txtbxSurname.Text;
            if (l_strSurname == "")
            {
                lblMessageToUser.Text = "You have to give Surname";
                return;
            }

            int l_iCountryId = int.Parse(lstbxCountry.SelectedValue);

            string l_strAddressStreet = txtbxAddressStreet.Text;
            if (l_strAddressStreet == "")
            {
                lblMessageToUser.Text = "You have to give Address Street";
                return;
            }

            string l_strAddressNumber = txtbxAddressNumber.Text;

            string l_strAddressCity = txtbxAddressCity.Text;
            if ( l_strAddressCity == "" )
            {
                lblMessageToUser.Text = "You have to give Address City";
                return;
            }

            string l_strAddressPostCode = txtbxAddressPostCode.Text;
            if ( l_strAddressPostCode == "" )
            {
                lblMessageToUser.Text = "You have to give Address Post Code";
                return;
            }

            DateTime l_dtDateOfBirth;
            try
            {
                l_dtDateOfBirth = new DateTime(int.Parse(lstbxDateOfBirthYear.SelectedValue),
                                                     int.Parse(lstbxDateOfBirthMonth.SelectedValue),
                                                     int.Parse(lstbxDateOfBirthDayOfMonth.SelectedValue));

            }
            catch (Exception ex)
            {
                lblMessageToUser.Text = "Invalid Date of Birth";
                return;
            }

            string l_strEmail = txtbxEmail.Text;
            if ( l_strEmail == "" )
            {
                lblMessageToUser.Text = "You have to give Email";
                return;
            }

            string l_strTelephone = txtbxTelephone.Text;

            string l_strMobile = txtbxMobile.Text;

            string l_strUsername = txtbxUsername.Text;
            if ( l_strUsername == "" )
            {
                lblMessageToUser.Text = "You have to give Username";
                return;
            }

            string l_strPassword = txtbxPassword.Text;
            if ( l_strPassword == "" )
            {
                lblMessageToUser.Text = "You have to give password";
                return;
            }

            string l_strConfirmPassword = txtbxConfirmPassword.Text;
            if ( l_strConfirmPassword != l_strPassword )
            {
                lblMessageToUser.Text = "Password and Confirmation do not match";
                return;
            }

            DateTime l_dtDateOfRegistration;
            try
            {
                l_dtDateOfRegistration = new DateTime(int.Parse(lstbxDateOfRegistrationYear.SelectedValue),
                                                      int.Parse(lstbxDateOfRegistrationMonth.SelectedValue),
                                                      int.Parse(lstbxDateOfRegistrationDayOfMonth.SelectedValue));

            }
            catch (Exception ex)
            {
                lblMessageToUser.Text = "Invalid Date of Registration";
                return;
            }                     

            // data are valid

            // let us check for already taken data
            Account l_accountFound = Account.FindByFirstnameAndSurname(l_strFirstname, l_strSurname);
            if ( l_accountFound != null )
            {
                lblMessageToUser.Text = "Firstname/Surname already taken";
                return;
            }

            l_accountFound = Account.FindByEmail(l_strEmail);
            if ( l_accountFound != null )
            {
                lblMessageToUser.Text = "Email already taken";
                return;                
            }

            l_accountFound = Account.FindByUsername(l_strUsername);
            if ( l_accountFound != null )
            {
                lblMessageToUser.Text = "Username already taken";
                return;
            }

            // now we can do the create
            Account l_accountToCreate = new Account();
            l_accountToCreate.AddressCity = l_strAddressCity;
            l_accountToCreate.AddressNumber = l_strAddressNumber;
            l_accountToCreate.AddressPostCode = l_strAddressPostCode;
            l_accountToCreate.AddressStreet = l_strAddressStreet;
            l_accountToCreate.CountryId = l_iCountryId;
            l_accountToCreate.DateOfBirth = l_dtDateOfBirth;
            l_accountToCreate.DateOfRegistration = l_dtDateOfRegistration;
            l_accountToCreate.Email = l_strEmail;
            l_accountToCreate.Firstname = l_strFirstname;
            l_accountToCreate.Mobile = l_strMobile;
            l_accountToCreate.Password = l_strPassword;
            l_accountToCreate.Surname = l_strSurname;
            l_accountToCreate.Telephone = l_strTelephone;
            l_accountToCreate.TitleId = l_iTitleId;
            l_accountToCreate.Username = l_strUsername;
            string l_strIpOnRegistration = Request.UserHostAddress;
            if ( txtbxIpOnRegistration.Text != "" )
            {
                l_strIpOnRegistration = txtbxIpOnRegistration.Text;
            }
            if ( l_strIpOnRegistration != "" )
            {
                l_accountToCreate.IpAddressOnRegistration = l_strIpOnRegistration;    
            }            
            l_accountToCreate = Account.Create(l_accountToCreate);
            if ( l_accountToCreate == null )
            {
                lblMessageToUser.Text = "Problem creating the account";
                return;
            }

            Response.Redirect("~/Default.aspx");

        } // Submit_Click ()
        //------------------
    }
}
