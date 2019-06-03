using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using MSSSystem.BLL;
using MSSSystem.BLL.Security;
using MSS.Data.Entities;
using MSS.Data.Entities.Security;
#endregion

public partial class Admin_Security_UserRoleAdmin : System.Web.UI.Page
{
    // stores the username, full name or partial name the signed in user last searched for
    static string username = "";
    // stores the sites ids the signed in user last searched for
    static List<int> site = new List<int>();
    // stores the role names the signed in user last searched for
    static List<string> role = new List<string>();
    //  stores the status the signed in user last searched for
    static int status = 3;

    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.HandleDataBoundException(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                if (!User.IsInRole(SecurityRoles.SuperUser) && !User.IsInRole(SecurityRoles.AdminEdits))
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                if (User.IsInRole(SecurityRoles.SuperUser))
                {
                    // if the user is a SuperUser then let the user see all users and all sites in dropdown lists
                    SiteAddDropDownList.DataSourceID = "SiteList";
                    SiteAddDropDownList.DataValueField = "SiteId";
                    SiteAddDropDownList.DataTextField = "SiteName";
                    SiteCheckBoxList.Enabled = true;
                    SiteCheckBoxList.Visible = true;
                    SiteScrollDiv.Visible = true;
                    UserListView.DataSourceID = "UserListViewODS";
                    UserListView.DataSource = null;
                }
                else if (User.IsInRole(SecurityRoles.AdminEdits))
                {
                    // if the user is an AdminEdit do not show him users with the superuser role and do not show him the admin site
                    SiteAddDropDownList.DataSourceID = null;
                    SiteController sitemgr = new SiteController();
                    List<Site> info = new List<Site>();
                    UserManager usermgr = new UserManager();
                    info.Add(sitemgr.Site_FindById(usermgr.GetUserSiteId(User.Identity.Name)));
                    SiteAddDropDownList.DataSource = info;
                    SiteAddDropDownList.DataTextField = "SiteName";
                    SiteAddDropDownList.DataValueField = "SiteId";
                    UserListView.DataSourceID = "";
                    UserListView.DataSource = usermgr.ListUser_BySearchParams("", new List<int>(new int[] { sitemgr.Site_FindById(usermgr.GetUserSiteId(User.Identity.Name)).SiteId }), new List<string>(), 3);
                    SiteCheckBoxList.Enabled = false;
                    SiteCheckBoxList.Visible = false;
                    SiteScrollDiv.Visible = false;
                }
                if (!IsPostBack)
                {
                    // change the site dropdown lists when the user first loads the page
                    SiteAddDropDownList.DataBind();
                    LookupUsers();
                }
                MessageUserControl.Visible = false;
            }
        });
    }

    protected void InsertUser_Click(object sender, CommandEventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            SiteController sitemgr = new SiteController();
            // find the site that the user is being inserted in
            Site site = sitemgr.Site_FindById(int.Parse(SiteAddDropDownList.SelectedValue));
            MessageUserControl.Visible = true;
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                throw new Exception("First name is a required field, please enter a valid first name value");
            }
            else if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                throw new Exception("Last name is a required field, please enter a valid last name value");
            }
            else if (string.IsNullOrWhiteSpace(RequestedPasswordLabel.Text))
            {
                throw new Exception("Password is a required field, please enter a valid password value");
            }
            else if (string.IsNullOrWhiteSpace(RoleMemberships.SelectedValue))
            {
                throw new Exception("Role is required for every employee please enter a valid role for before inserting the employee");
            }
            else if (string.IsNullOrWhiteSpace(UserNameLabel.Text))
            {
                throw new Exception("Please assign the user a username");
            }
            else if (site.Disabled)
            {
                // refresh the site dropdown list without disabled sites if the user attempted to insert a user into a disabled site
                SiteAddDropDownList.DataBind();
                throw new Exception("Please select a site which is not deactivated");
            }
            else
            {
                UserProfile user = new UserProfile();
                Utility utility = new Utility();
                user.UserName = UserNameLabel.Text;
                user.FirstName = FirstNameTextBox.Text;
                user.LastName = LastNameTextBox.Text;
                user.SiteId = int.Parse(SiteAddDropDownList.SelectedValue);
                user.RequestedPassword = RequestedPasswordLabel.Text;
                var roleList = new List<string>();
                roleList.Add(RoleMemberships.SelectedValue);
                user.RoleMemberships = roleList;
                user.Active = (disabledCheckBox.Checked);
                utility.checkValidString(user.UserName);
                utility.checkValidString(user.FirstName);
                utility.checkValidString(user.LastName);
                utility.checkValidString(user.RequestedPassword);
                UserManager sysmgr = new UserManager();
                sysmgr.AddUser(user);
                if (User.IsInRole(SecurityRoles.SuperUser))
                {
                    // if the user is a SuperUser then let the user see all users and all sites in dropdown lists
                    SiteAddDropDownList.DataSourceID = "SiteList";
                    SiteAddDropDownList.DataValueField = "SiteId";
                    SiteAddDropDownList.DataTextField = "SiteName";
                    SiteCheckBoxList.Enabled = true;
                    SiteCheckBoxList.Visible = true;
                    SiteScrollDiv.Visible = true;
                    UserListView.DataSourceID = "UserListViewODS";
                    UserListView.DataSource = null;
                }
                else if (User.IsInRole(SecurityRoles.AdminEdits))
                {
                    // if the user is an AdminEdit do not show him users with the superuser role and do not show him the admin site
                    SiteAddDropDownList.DataSourceID = null;
                    List<Site> info = new List<Site>();
                    info.Add(sitemgr.Site_FindById(sysmgr.GetUserSiteId(User.Identity.Name)));
                    SiteAddDropDownList.DataSource = info;
                    SiteAddDropDownList.DataTextField = "SiteName";
                    SiteAddDropDownList.DataValueField = "SiteId";
                    UserListView.DataSourceID = "";
                    UserListView.DataSource = sysmgr.ListUser_BySearchParams("", new List<int>(new int[] { sitemgr.Site_FindById(sysmgr.GetUserSiteId(User.Identity.Name)).SiteId }), new List<string>(), 3);
                    SiteCheckBoxList.Enabled = false;
                    SiteCheckBoxList.Visible = false;
                    SiteScrollDiv.Visible = false;
                }
                UserListView.DataBind();
                LookupUsers();
                CancelButton_Command(sender, e);
                MessageUserControl.Visible = true;
            }
        }, "Success", "New user has been added.");
    }

    protected void Update_Command(object sender, ListViewUpdateEventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            MessageUserControl.Visible = true;
            UserProfile user = new UserProfile();
            Utility utility = new Utility();
            user.FirstName = (UserListViewEdit.EditItem.FindControl("FirstNameLabel") as TextBox).Text;
            user.LastName = (UserListViewEdit.EditItem.FindControl("LastNameLabel") as TextBox).Text;
            user.SiteId = int.Parse((UserListViewEdit.EditItem.FindControl("SiteDropDownList") as DropDownList).SelectedValue);
            user.RequestedPassword = (UserListViewEdit.EditItem.FindControl("RequestedPasswordLabel") as TextBox).Text;
            user.UserName = (UserListViewEdit.EditItem.FindControl("UserNameLabel") as Label).Text;
            user.Active = (disabledCheckBox.Checked);
            utility.checkValidString(user.RequestedPassword);
            utility.checkValidString(user.FirstName);
            utility.checkValidString(user.LastName);
            // check to see if the user being edited is the webmaster
            if ((UserListViewEdit.EditItem.FindControl("UserNameLabel") as Label).Text != "Webmaster")
            {
                // change the role of non webmasters
                var roleList = new List<string>();
                roleList.Add((UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).SelectedValue);
                user.RoleMemberships = roleList;
            }
            user.Active = (UserListViewEdit.EditItem.FindControl("disabledCheckBox") as CheckBox).Checked;
            UserManager sysmgr = new UserManager();
            sysmgr.UpdateUser(user);
            UserListViewEdit.EditIndex = -1;
            List<UserProfile> info = sysmgr.ListUser_BySearchParams(username, site, role, status);
            UserListViewEdit.DataSource = info;
            UserListViewEdit.DataBind();
            UserListView.DataBind();
        }, "Success", "Successfully updated users");
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            MessageUserControl.Visible = true;
            Utility utility = new Utility();
            utility.checkValidString(txtSearchMaster.Text);
            LookupUsers();
            SiteController sitemgr = new SiteController();
            UserManager usermgr = new UserManager();
            if (User.IsInRole(SecurityRoles.AdminEdits))
            {
                // if the user is an AdminEdit do not show him users with the superuser role and do not show him the admin site
                SiteAddDropDownList.DataSourceID = null;
                List<Site> info = new List<Site>();
                info.Add(sitemgr.Site_FindById(usermgr.GetUserSiteId(User.Identity.Name)));
                SiteAddDropDownList.DataSource = info;
                UserListView.DataSourceID = "";
                UserListView.DataSource = usermgr.ListUser_BySearchParams("", new List<int>(new int[] { sitemgr.Site_FindById(usermgr.GetUserSiteId(User.Identity.Name)).SiteId }), new List<string>(), 3);
                SiteCheckBoxList.Enabled = false;
                SiteCheckBoxList.Visible = false;
                SiteScrollDiv.Visible = false;
            }
            UserListView.DataBind();
            MessageUserControl.Visible = false;
        });
    }

    private void LookupUsers()
    {
        username = txtSearchMaster.Text;
        site = new List<int>();
        role = new List<string>();
        if (SiteCheckBoxList.SelectedItem != null)
        {
            for (int i = 0; i < SiteCheckBoxList.Items.Count; i++)
            {
                if (SiteCheckBoxList.Items[i].Selected)
                {
                    site.Add(int.Parse(SiteCheckBoxList.Items[i].Value));
                }
            }
        }
        if (RoleCheckBoxList.SelectedItem != null)
        {
            for (int i = 0; i < RoleCheckBoxList.Items.Count; i++)
            {
                if (RoleCheckBoxList.Items[i].Selected)
                {
                    role.Add(RoleCheckBoxList.Items[i].Text);
                }
            }
        }

        //if only active users status is 1 if only inactive status is 2 if both active and inactive status is 3 if neither status is 0
        status = 3;
        if (ActiveCheckbox.Checked)
        {
            status = 1;
        }
        if (InactiveCheckbox.Checked)
        {
            status = 2;
        }
        if (ActiveCheckbox.Checked && InactiveCheckbox.Checked)
        {
            status = 3;
        }
        UserManager sysmgr = new UserManager();
        if (User.IsInRole(SecurityRoles.AdminEdits))
        {
            site.Add(sysmgr.GetUserSiteId(User.Identity.Name));
        }
        List<UserProfile> info = sysmgr.ListUser_BySearchParams(username, site, role, status);
        UserListViewEdit.DataSource = info;
        UserListViewEdit.DataBind();
    }

    protected void Item_Edit(object sender, ListViewEditEventArgs e)
    {
        MessageUserControl.Visible = false;
        UserListViewEdit.EditIndex = e.NewEditIndex;
        UserManager sysmgr = new UserManager();
        List<UserProfile> info = sysmgr.ListUser_BySearchParams(username, site, role, status);
        UserListViewEdit.DataSource = info;
        UserListViewEdit.DataBind();
        string userRole = sysmgr.getUserRole((UserListViewEdit.EditItem.FindControl("UserNameLabel") as Label).Text);
        if (userRole == SecurityRoles.SuperUser)
        {
            // do not allow anyone to change the role or site of the webmaster and make sure that they default to their proper values
            (UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).Enabled = false;
            (UserListViewEdit.EditItem.FindControl("SiteDropDownList") as DropDownList).Enabled = false;
            (UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).DataSourceID = "AllRoleNames";
            (UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).SelectedIndex = 2;
            (UserListViewEdit.EditItem.FindControl("SiteDropDownList") as DropDownList).SelectedValue = "1";
            (UserListViewEdit.EditItem.FindControl("disabledCheckBox") as CheckBox).Enabled = false;
        }
        else if (userRole == SecurityRoles.AdminEdits)
        {
            // make the default role of the AdminEdits AdminEdit and make their default site to what their site was before edit was clicked
            (UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).SelectedIndex = 0;
            (UserListViewEdit.EditItem.FindControl("SiteDropDownList") as DropDownList).SelectedValue =
                sysmgr.GetUserSiteId((UserListViewEdit.EditItem.FindControl("UserNameLabel") as Label).Text).ToString();
        }
        else if (userRole == SecurityRoles.AdminViews)
        {
            // make the default role of the AdminViews AdminView and make their default site to what their site was before edit was clicked
            (UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).SelectedIndex = 1;
            (UserListViewEdit.EditItem.FindControl("SiteDropDownList") as DropDownList).SelectedValue = 
                sysmgr.GetUserSiteId((UserListViewEdit.EditItem.FindControl("UserNameLabel") as Label).Text).ToString();
        }
        if (User.IsInRole(SecurityRoles.AdminEdits))
        {
            // Does not allow AdminEdits to edit any users' roles
            (UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).Enabled = false;
            (UserListViewEdit.EditItem.FindControl("SiteDropDownList") as DropDownList).Enabled = false;
            (UserListViewEdit.EditItem.FindControl("RoleMembershipsDropDown") as DropDownList).DataSourceID = "AllRoleNames";            
            (UserListViewEdit.EditItem.FindControl("SiteDropDownList") as DropDownList).SelectedValue = "1";
        }
        for (int i = 0; i < UserListViewEdit.Items.Count; i++)
        {
            if (i != UserListViewEdit.EditIndex)
            {
                // Hide all other rows except the row being edited
                UserListViewEdit.Items[i].Visible = false;
            }
        }
        (UserListViewEdit.FindControl("DataPager2") as DataPager).Visible = false;
    }

    protected void Item_Cancel(object sender, ListViewCancelEventArgs e)
    {
        MessageUserControl.Visible = false;
        UserManager sysmgr = new UserManager();
        List<UserProfile> info = sysmgr.ListUser_BySearchParams(username, site, role, status);
        UserListViewEdit.DataSource = info;
        UserListViewEdit.EditIndex = -1;
        UserListViewEdit.DataBind();
        (UserListViewEdit.FindControl("DataPager2") as DataPager).Visible = true;
    }

    protected void BtnClear_Click(object sender, EventArgs e)
    {
        MessageUserControl.Visible = false;
        txtSearchMaster.Text = "";
        SiteCheckBoxList.SelectedIndex = -1;
        RoleCheckBoxList.SelectedIndex = -1;
        ActiveCheckbox.Checked = false;
        InactiveCheckbox.Checked = false;
    }

    protected void UserListViewEdit_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        MessageUserControl.Visible = false;
        (UserListViewEdit.FindControl("DataPager2") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        UserManager sysmgr = new UserManager();
        List<UserProfile> info = sysmgr.ListUser_BySearchParams(username, site, role, status);
        UserListViewEdit.DataSource = info;
        UserListViewEdit.DataBind();
    }

    protected void UserListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        MessageUserControl.Visible = false;
        (UserListView.FindControl("DataPager2") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        UserListView.DataBind();
    }

    protected void CancelButton_Command(object sender, CommandEventArgs e)
    {
        MessageUserControl.Visible = false;
        FirstNameTextBox.Text = "";
        LastNameTextBox.Text = "";
        SiteAddDropDownList.SelectedIndex = -1;
        UserNameLabel.Text = "";
        RoleMemberships.SelectedIndex = -1;
        disabledCheckBox.Checked = true;
        RequestedPasswordLabel.Text = "";
    }
}