using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using MSS.Website;
using MSSSystem.BLL;
using MSS.Data.Entities.Security;

public partial class Account_Manage : System.Web.UI.Page
{
    protected bool CanRemoveExternalLogins
    {
        get;
        private set;
    }

    private bool HasPassword(UserManager manager)
    {
        var user = manager.FindById(User.Identity.GetUserId());
        return (user != null && user.PasswordHash != null);
    }

    protected void Page_Load()
    {
        if (!IsPostBack)
        {
            // Determine the sections to render
            UserManager manager = new UserManager();
            if (HasPassword(manager))
            {
                changePasswordHolder.Visible = true;
            }
            MessageUserControl.Visible = false;
            CanRemoveExternalLogins = manager.GetLogins(User.Identity.GetUserId()).Count() > 1;
        }
        if (!Request.IsAuthenticated)
        {
            // takes user ro login page if the user is signed in
            Response.Redirect("~/Account/Login.aspx");
        }
    }

    protected void ChangePassword_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            MessageUserControl.Visible = true;

            Utility utility = new Utility();
            utility.checkValidString(NewPassword.Text);
            utility.checkValidString(ConfirmNewPassword.Text);
            utility.checkValidString(CurrentPassword.Text);

            if (string.IsNullOrWhiteSpace(CurrentPassword.Text))
            {
                throw new Exception("Please enter your current password");
            }
            else if (string.IsNullOrWhiteSpace(NewPassword.Text))
            {
                throw new Exception("Please enter your new password");
            }
            else if (string.IsNullOrWhiteSpace(ConfirmNewPassword.Text))
            {
                throw new Exception("Please confirm your new password");
            }
            else if (ConfirmNewPassword.Text != NewPassword.Text)
            {
                throw new Exception("Your new password does not match your confirm password");
            }
            // Create the local login info and link the local account to the user
            UserManager manager = new UserManager();
            IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), CurrentPassword.Text, NewPassword.Text);
      
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault());
            }
        },"Success","Password has been changed");
    }  
}