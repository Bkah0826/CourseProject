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

/// <summary>
///Contains all functionality related to this page.
/// </summary>
public partial class SurveyAccess : System.Web.UI.Page
{
    /// <summary>
    ///Loads the page and checks for querystrings and display a  message if needed.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //checks for error query string
        
     
        string error = Request.QueryString["error"];

        if (error == "nodataquestions")
        {
            MessageUserControl.ShowInfoError("No data", "Sorry for the inconvenience, currently there are no questions in the database. Please contact an administrator at 555-555-5555 to resolve the issue.");
        }
       if (error == "nounitsinsite")
        {
            MessageUserControl.ShowInfoError("No data", "Sorry for the inconvenience, currently there are no active units at your site. Please contact an administrator at 555-555-5555 to resolve the issue.");
        }
              
    }
    /// <summary>
    ///Checks if the correct passcode was entered. If passcode is correct user will be re-directed to the survey page, if not an error message will be thrown.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void SubmitButton_Click(object sender, EventArgs e)
    {   
        //Calls utility controllter
        Utility utility = new Utility();
        //Creates a empty list to store Active Site's Passcodes
        List<string> PasscodeList = new List<string>();
        SiteController sysmgr = new SiteController();
        PasscodeList = sysmgr.Site_PasscodeList();
        //Passcode entered from textbox
        string passcode = SurveyPasscode.Text;
        //Check if passcode entered on page matches any in list
        MessageUserControl.TryRun(() =>
        {
            utility.checkValidString(SurveyPasscode.Text);
            for (int SitePasscode = 0; SitePasscode < PasscodeList.Count; SitePasscode++)
            {
                //Passcode matches 
                if (passcode.ToUpper() == PasscodeList[SitePasscode].ToUpper())
                {
                    //Redirects the user to survey page correct passcode is passed to survey page
                    Response.Redirect("Survey.aspx?passcode=" + passcode);
                }
                else if (passcode == "")
                {
                    // No Passcode is entered show message
                    MessageUserControl.ShowInfoError("No Passcode Entered", "Please enter the passcode to access the survey.");
                }
                else
                {
                    // Passcode is incorrect show message
                    MessageUserControl.ShowInfoError("Incorrect Passcode", "Passcode entered was incorrect. Please try again.");
                    SurveyPasscode.Text = "";
                }
            }

        });
        
      
    }
}