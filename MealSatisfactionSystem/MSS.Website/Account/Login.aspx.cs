using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI;
using MSS.Website;
using MSSSystem.BLL;

public partial class Account_Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {   
    }

    protected void LogIn(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            if (string.IsNullOrEmpty(UserName.Text) && string.IsNullOrEmpty(Password.Text))
            {
                throw new Exception("Please enter a valid username <br><li> Please enter a valid password</li>");
            }
            else if (string.IsNullOrEmpty(UserName.Text))
            {
                throw new Exception("Please enter a valid username");
            }
            else if (string.IsNullOrEmpty(Password.Text))
            {
                throw new Exception("Please enter a valid password");
            }
            else if (IsValid)
            {
                // Validate the user password
                var manager = new UserManager();
                ApplicationUserWeb user = manager.Find(UserName.Text, Password.Text);
                Utility utility = new Utility();
                utility.checkValidString(UserName.Text);
                utility.checkValidString(Password.Text);
                if (user != null)
                {
                    if (user.Active)
                    {
                        IdentityHelper.SignIn(manager, user, false);                        
                        Response.Redirect("~/Admin/Home.aspx");
                    }
                    else
                    {
                        throw new Exception("User is inactive");
                    }
                }
                else
                {
                    throw new Exception("Invalid username or password");
                }
            }
        });
    }
    protected void CancelBtn_Click(object sender, EventArgs e)
    {
        Password.Text = "";
        UserName.Text = "";
    }
}