using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BettingDemo.DAL;

namespace BettingDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            lstbxPaymentMethod.Items.Add(new ListItem("Credit Card", "1"));            
            lstbxPaymentMethod.Items.Add(new ListItem("Bank Deposit", "2"));
        }

        protected bool WalletHasMoney ()
        {
            string l_strUsername = Context.User.Identity.Name;
            Account l_accountFound = Account.FindByUsername(l_strUsername);

            if ( l_accountFound == null )
            {
                return false;
            }

            Wallet l_wallet = Wallet.FindByAccountId(l_accountFound.Id);
            return l_wallet.Amount <= 0;

        } // WalletHasMoney()
        //-------------------

        protected void Deposit_Click(Object o, EventArgs e)
        {
            string l_strPaymentMethod = "cc";
            if ( lstbxPaymentMethod.SelectedValue == "2" )
            {
                l_strPaymentMethod = "bank";
            }
            Response.Redirect("~/Checkout.aspx?payment_method=" + l_strPaymentMethod, true);
        }
    }
}
