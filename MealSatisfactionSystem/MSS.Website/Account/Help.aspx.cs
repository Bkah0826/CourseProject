using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using MSS.Website;

public partial class Account_Help : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.IsAuthenticated)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}