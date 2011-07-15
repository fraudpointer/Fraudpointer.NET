using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BettingDemo.DAL;

namespace BettingDemo
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_Click (Object o, EventArgs e)
        {
            // we need to authenticate user
            string l_strUsername = txtbxUsername.Text;
            string l_strPassword = txtbxPassword.Text;

            Account l_accountFound = Account.FindByUsername(l_strUsername);
            if ( l_accountFound == null || l_accountFound.Password != l_strPassword )
            {
                lblMessageToUser.Text = "Wrong Credentials";                
                return;
            } 

            // correct credentials given
            FormsAuthentication.RedirectFromLoginPage(l_strUsername,false);
            
        }
    }
}
