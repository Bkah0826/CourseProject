using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSSSystem.BLL;
using MSS.Data.Entities;
using System.Web.Services;
using System.Web.Script.Services;
using MSS.Data.POCOs;
using Microsoft.AspNet.Identity;
using Hangfire;
using System.IO;
using MSSSystem.BLL.Security;

/// <summary>
/// Holds the code that drives all functions built into the home page.
/// </summary>
public partial class Admin_Home : System.Web.UI.Page
{
    QuestionController questionController = new QuestionController();
    UnitController unitController = new UnitController();
    HomePageController homePageControl = new HomePageController();
    SiteController siteController = new SiteController();
    UserManager userManager = new UserManager();
    RoleManager roleManager = new RoleManager();

    /// <summary>
    /// Runs when the page is loaded and checks the UserID. If the UserID does not exist, the method redirects to the login page.
    /// </summary>
    /// <param name="sender">The source of the event load.</param>
    /// <param name="e">The load event fired from the source.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                dailyPass.Text = homePageControl.getPass(User.Identity.GetUserId());
                try
                {
                    if (userManager.IsInRole(User.Identity.GetUserId(), "AdminView"))
                    {
                        discardPass.Visible = false;
                    }
                }
                catch
                {
                    Response.Redirect("~/Account/Login.aspx");
                }

                MessageUserControl.Visible = false;

            }
        }
    }
    /// <summary>
    /// Resets the current password and removes it from the library of possible passwords permanently after presenting a message to the user.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The click event fired from the source.</param>
    protected void DiscardPasscode(Object sender, EventArgs e)
    {

        MessageUserControl.Visible = false;
        string text = dailyPass.Text.ToLower();
        MessageUserControl.TryRun(() =>
        {
            MessageUserControl.Visible = true;
            RemoveWordFromFile(text);
            ResetPasscode();
            //Prevents postback issues.
            Response.Redirect("~/Admin/Home.aspx");
        });
    }

    /// <summary>
    /// Removes the empty lines after removing a passcode.
    /// </summary>
    /// <param name="path"> The path to the list of passwords.</param>
    /// <param name="lines"> The lines in the list of passwords.</param>
    public static void WriteAllLinesBetter(string path, List<string> lines)
    {
        if (path == null)
            throw new ArgumentNullException("path");
        if (lines == null)
            throw new ArgumentNullException("lines");

        using (var stream = File.OpenWrite(path))
        {
            stream.SetLength(0);
            using (var writer = new StreamWriter(stream))
            {
                if (lines.Count > 0)
                {
                    for (var i = 0; i < lines.Count - 1; i++)
                    {
                        writer.WriteLine(lines[i]);
                    }
                    writer.Write(lines[lines.Count - 1]);
                }
            }
        }
    }

    /// <summary>
    /// Maps the path to the list of passwords and applies the WriteAllLinesBetter function to reformat the list.
    /// </summary>
    /// <param name="text">Contains the password to be discarded.</param>
    private void RemoveWordFromFile(string text)
    {
        string fileName = Server.MapPath("~/nounlist.txt");
        WriteAllLinesBetter(fileName,
            File.ReadLines(fileName).Where(l => l != text).ToList());
    }

    /// <summary>
    /// Finds the site which the user is affiliated with and changes the passcode.
    /// </summary>
    private void ResetPasscode()
    {
        int siteID = userManager.GetUserSiteId(Page.User.Identity.Name);

        string newCode = siteController.getSinglePasscode(siteID);
        siteController.Site_ChangeSinglePasscode(siteID, newCode);
    }

    /// <summary>
    /// Looks up tables for charting colours and site names.
    /// </summary>
    /// <returns> ChartingHomeResponse with information associated to questions, answers, values, units, and colours.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<ChartingHomeResponse> ChartDataRequest()
    {
        HomePageController homePageControl = new HomePageController();

        Page page = new Page();
        string site = page.User.Identity.GetUserId();
        var results = homePageControl.grabData(homePageControl.getSite(site));
        return results;
        
    }

}