using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.SessionState;

namespace BettingDemo
{    
    public class Logout : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {            
            FormsAuthentication.SignOut();
            context.Response.Redirect("~/Login.aspx");            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
