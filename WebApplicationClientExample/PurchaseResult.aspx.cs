using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationClientExample
{
    public partial class PurchaseResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string l_messageToUser = Request.Params["message"];
            string l_goBackUrl = Request.Params["go_back_url"];
            if ( l_messageToUser != null )
            {
                lblMessageToUser.Text = l_messageToUser;
            }
            if ( l_goBackUrl != null )
            {
                hplnkGoBackUrl.NavigateUrl = l_goBackUrl;
                hplnkGoBackUrl.Text = "[Go BACK]";
            }
        } // Page_Load ()
    }
}
