using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationClientExample
{
    public partial class AcmeBuyingTickets : System.Web.UI.MasterPage
    {
        public string MessageToUser
        {
            get
            {
                return lblMessageToUser.Text;
            }
            set
            {
                lblMessageToUser.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void MessageToUser_Init(Object o, EventArgs e)
        {
            if ( IsPostBack == false )
            {
                // the first time, clear message
                lblMessageToUser.Text = "";
            }    
        } // MessageToUser_Init()
        //-------------------------
    }
}
