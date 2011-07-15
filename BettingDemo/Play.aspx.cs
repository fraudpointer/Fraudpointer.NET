using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BettingDemo.DAL;

namespace BettingDemo
{
    public partial class Play : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        protected void Play_Click(object sender, EventArgs e)
        {
            if ( txtbxAmount.Text == "" )
            {
                string l_strMessageToUser = "You have to give an amount";
                string l_strGoBackUrl = "~/Play.aspx";
                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_strMessageToUser) + "&go_back_url=" + l_strGoBackUrl, true);
                return;
            }
            // take the amount
            decimal l_amountRequested = decimal.Parse(txtbxAmount.Text);
            // find the current wallet amount
            string l_strUsername = Context.User.Identity.Name;
            Account l_accountFound = Account.FindByUsername(l_strUsername);
            if ( l_accountFound == null )
            {
                string l_strMessageToUser = "Cannot locate account";
                string l_strGoBackUrl = "~/Play.aspx";
                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_strMessageToUser) + "&go_back_url=" + l_strGoBackUrl, true);
                return;
            }

            Wallet l_wallet = Wallet.FindByAccountId(l_accountFound.Id);
            if ( l_wallet.Amount<l_amountRequested)
            {
                string l_strMessageToUser = "Not enough money";
                string l_strGoBackUrl = "~/Play.aspx";
                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_strMessageToUser) + "&go_back_url=" + l_strGoBackUrl, true);
                return;                
            }

            // ok. Enough Money.

            // Let us win or loose now
            Random l_random = new Random(DateTime.Now.Second);
            bool l_boolWin = l_random.Next() % 2 == 1;

            Transaction l_transaction = new Transaction();
            l_transaction.AccountId = l_accountFound.Id;
            l_transaction.Amount = l_amountRequested;
            if ( !l_boolWin )
            {
                l_transaction.Amount = -l_transaction.Amount;
            }
            l_transaction.DateOfTransaction = DateTime.Now;
            l_transaction.TypeOfTransaction = "play";
            l_transaction = Transaction.Create(l_transaction);
            
            if ( l_transaction != null && l_transaction.Id >= 1 )
            {
                string l_strLooseOrWinMessage = "You won!";
                if (!l_boolWin)
                {
                    l_strLooseOrWinMessage = "Sorry. You missed that one. Why don't you try again?";
                }                    
                string l_strMessageToUser = "Transaction was recorded successfully. Transaction ID: " + l_transaction.Id + ", " + l_strLooseOrWinMessage;
                string l_strGoBackUrl = "~/Play.aspx";
                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_strMessageToUser) + "&go_back_url=" + l_strGoBackUrl, true);
                return;                                
            }
            else
            {
                string l_strMessageToUser = "Problem recording transaction.";
                string l_strGoBackUrl = "~/Play.aspx";
                Response.Redirect("~/MessageToUser.aspx?message_to_user=" + HttpUtility.UrlEncode(l_strMessageToUser) + "&go_back_url=" + l_strGoBackUrl, true);
                return;                                                
            }

        } // Play_Click
        //--------------
    }
}
