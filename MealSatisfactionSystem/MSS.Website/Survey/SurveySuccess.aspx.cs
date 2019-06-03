using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region AdditionalNamespaces
using MSSSystem.BLL;
using MSS.Data.Entities;
#endregion

public partial class Survey_SurveySuccess : System.Web.UI.Page
{/// <summary>
 /// Loads the page and displays a message. Also checks to see if ResponseId exists -+
 /// </summary>
 /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
 /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Checks to see if response id exists if it does not exist redirects user to Survey Access page.
        if (Request.QueryString["rid"] == null)
        {
            Response.Redirect("SurveyAccess.aspx");
        }
        string rid = Request.QueryString["rid"];
        ResponseController sysmgr = new ResponseController();
        int NewestResponseId = sysmgr.Get_NewestResponseID();
        if(int.Parse(rid) != NewestResponseId)
        {
            Response.Redirect("SurveyAccess.aspx");

        }
    }
}