using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BettingDemo.DAL;

namespace BettingDemo
{
    public partial class BettingDemo : System.Web.UI.MasterPage
    {
        public string GetStyleSheet()
        {
            return Branding.StyleSheetUrl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {                        
            imgLogo.AlternateText = Branding.LogoAlternateText();
            imgLogo.ImageUrl = Branding.LogoUrl();
        }

        protected void Logout_Init(Object o, EventArgs e)
        {
            if ( Request.IsAuthenticated )
            {
                hprlnkLogout.Text = "Logout";
                hprlnkLogout.Enabled = true;
            }
            else
            {
                hprlnkLogout.Text = "";
                hprlnkLogout.Enabled = false;
            }
        }

        protected void WalletAmount_Init (Object sender, EventArgs eventArgs)
        {
            if ( Request.IsAuthenticated == false )
            {
                return;
            }

            // take username
            string l_strUsername = this.Context.User.Identity.Name;
            Account l_accountFound = Account.FindByUsername(l_strUsername);
            if ( l_accountFound == null )
            {
                return;
            }

            Wallet l_wallet = Wallet.FindByAccountId(l_accountFound.Id);
            DisplayWallet(l_wallet);

        }

        private void DisplayWallet(Wallet i_wallet)
        {
            lblWalletAmount.Visible = true;
            txtbxWalletAmount.Visible = true;
            txtbxWalletAmount.Text = i_wallet.Amount.ToString();

        } // DisplayWallet ()
        //--------------------
    }
}
