using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BettingDemo
{
    public partial class MessageToUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string l_strMessageToUser = Request.Params["message_to_user"];
            if ( String.IsNullOrEmpty(l_strMessageToUser) == false)
            {
                lblMessageToUser.Text = l_strMessageToUser;
            }
                
            string l_strGoBackUrl = Request.Params["go_back_url"];
            if ( String.IsNullOrEmpty(l_strGoBackUrl) == false)
            {
                hplnkBackLink.NavigateUrl = l_strGoBackUrl;    
            }
            else
            {
                hplnkBackLink.NavigateUrl = "";
                hplnkBackLink.Text = "";
            }
            
        }
    }
}
