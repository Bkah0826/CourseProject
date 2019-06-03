using MSS.Data.Entities.Security;
using MSSSystem.BLL;
using MSSSystem.BLL.Security;
using MSSSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ManageUnits : System.Web.UI.Page
{
    /// <summary>
    /// Standard pre-generated Page Load with added function to check the siteId of the user.
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // If the page is not being refreshed, check the siteId of the user, or redirect if they do not have the proper permissions.
        if (!IsPostBack)
        {
            // Check and make sure the user is logged in to an appropriate account, if not redirect them to log in.
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                // Check and make sure the user is a SuperUser or an Administrator allowed to edit things
                if (!User.IsInRole(SecurityRoles.SuperUser) && !User.IsInRole(SecurityRoles.AdminEdits))
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    // Collect the SiteId of the user. This is important because Non-SuperUsers will only see their site's units
                    if (!User.IsInRole(SecurityRoles.SuperUser))
                    {
                        UserManager manager = new UserManager();
                        string userName = User.Identity.Name;
                        int siteId = manager.GetUserSiteId(userName);

                        UserSiteId.Text = siteId.ToString();
                    }
                    else
                    {
                        // Only a SuperUser will return 0, meaning they will have access to all units from all sites
                        UserSiteId.Text = "0";
                    }
                }
            }
        }
        MessageUserControl.Visible = false;
    }

    /// <summary>
    /// Check for any exceptions that might have been raised by the system
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.Visible = true;
        MessageUserControl.HandleDataBoundException(e);
    }

    /// <summary>
    /// Button commands for inside the ListView. Allows the user to do things like Update, Add, Clear, Cancel etc.
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void UnitListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        MessageUserControl.Visible = false;

        int unitId = 1;

        MessageUserControl.TryRun(() =>
        {
            // If the user is attempting to do anything but Add or Cancel, collect the unitId from the ListView
            if (!e.CommandName.Equals("Cancel") && !e.CommandName.Equals("Add"))
            {
                unitId = int.Parse(e.CommandArgument.ToString());
            }

            // Update the Unit on the selected row. Only one item can be updated at a time.
            if (e.CommandName.Equals("Change"))
            {
                MessageUserControl.TryRun(() =>
                {
                    int i = e.Item.DisplayIndex;

                    DropDownList siteNameDDL = ActiveUnitListView.Items[i].FindControl("SiteNameDDL") as DropDownList;
                    int siteId = int.Parse(siteNameDDL.SelectedValue);
                    TextBox unitNameTextBox = ActiveUnitListView.Items[i].FindControl("UnitNameTextBox") as TextBox;
                    string unitName = unitNameTextBox.Text;
                    TextBox descriptionTextBox = ActiveUnitListView.Items[i].FindControl("DescriptionTextBox") as TextBox;
                    string description = descriptionTextBox.Text;

                    MessageUserControl.Visible = true;

                    Utility utility = new Utility();
                    utility.checkValidString(unitName);
                    utility.checkValidString(description);

                    UnitController sysmgr = new UnitController();
                    sysmgr.Unit_Update(unitId, siteId, unitName, description);
                    ActiveUnitListView.DataSourceID = ActiveUnitListView_ODS.ID;
                    ActiveUnitListView.EditIndex = -1;
                    ActiveUnitListView.DataBind();
                    AddUnitListView.DataBind();
                    DeactivatedUnitListView.DataBind();
                    ActiveSearchBox.Text = "";
                    MessageUserControl.Visible = true;
                    ActiveSearchButton.Enabled = true;
                    ActiveClearButton.Enabled = true;
                    DataPager pager = ActiveUnitListView.FindControl("ActiveDataPager") as DataPager;
                    pager.Visible = true;

                }, "Success", "Unit has been updated");
            }

            // Deactivate the Unit on the selected row.
            else if (e.CommandName.Equals("Deactivate"))
            {
                MessageUserControl.TryRun(() =>
                {
                    MessageUserControl.Visible = true;
                    UnitController sysmgr = new UnitController();
                    sysmgr.Unit_Deactivate(unitId);
                }, "Success", "Unit has been deactivated");
            }
        });
        
        ActiveUnitListView.DataSourceID = ActiveUnitListView_ODS.ID;
        DeactivatedUnitListView.DataSourceID = DeactivatedUnitListView_ODS.ID;
        AddSiteNameDDL.DataSourceID = ActiveSiteNameDDL_ODS.ID;
        ActiveUnitListView.DataBind();
        AddUnitListView.DataBind();
        DeactivatedUnitListView.DataBind();
        AddSiteNameDDL.DataBind();
    }

    /// <summary>
    /// Disables the search buttons and hides the datapager when the edit button is pushed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ActiveSiteListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            ActiveSearchButton.Enabled = false;
            ActiveClearButton.Enabled = false;
            DataPager pager = ActiveUnitListView.FindControl("ActiveDataPager") as DataPager;
            pager.Visible = false;
        });
    }

    /// <summary>
    /// Re-enables the search buttons and datapager when the edit item cancel button is pushed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ActiveSiteListView_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            ActiveSearchButton.Enabled = true;
            ActiveClearButton.Enabled = true;
            DataPager pager = ActiveUnitListView.FindControl("ActiveDataPager") as DataPager;
            pager.Visible = true;
        });
    }

    /// <summary>
    /// Hides all other rows when the user enters edit mode to make editing sites easier.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ActiveSiteListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (ActiveUnitListView.EditIndex != -1)
        {
            if (e.Item.DisplayIndex != ActiveUnitListView.EditIndex)
            {
                e.Item.Visible = false;
            }
        }
    }

    /// <summary>
    /// Search button for the Active tab, to search for active sites
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void ActiveSearchButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            if (ActiveSearchBox.Text != "")
            {
                MessageUserControl.Visible = true;
                searchArg.Value = ActiveSearchBox.Text;
                Utility utility = new Utility();
                utility.checkValidString(searchArg.Value);
                deactivatedSearch.Value = "false";
                bool siteNameChecked = ActiveSiteNameCheckbox.Selected;
                bool descriptionChecked = ActiveDescriptionCheckbox.Selected;
                bool unitNameChecked = ActiveUnitNameCheckbox.Selected;
                searchBy.Value = "All";
                if (siteNameChecked == true)
                {
                    searchBy.Value = "SiteName";
                }
                else if (unitNameChecked == true)
                {
                    searchBy.Value = "UnitName";
                }
                else if (descriptionChecked == true)
                {
                    searchBy.Value = "Description";
                }
                ActiveUnitListView.DataSourceID = SearchListODS.ID;
                ActiveUnitListView.DataBind();
                MessageUserControl.Visible = false;
            }
        });
    }

    /// <summary>
    /// Search button for the Deactivated tab, to search for Deactivated sites
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void DeactivatedSearchButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            if (DeactivatedSearchBox.Text != "")
            {
                MessageUserControl.Visible = true;
                UnitController sysmgr = new UnitController();
                searchArg.Value = DeactivatedSearchBox.Text;
                Utility utility = new Utility();
                utility.checkValidString(searchArg.Value);
                deactivatedSearch.Value = "true";
                bool siteNameChecked = DeactivatedSiteNameCheckbox.Selected;
                bool descriptionChecked = DeactivatedDescriptionCheckbox.Selected;
                bool unitNameChecked = DeactivatedUnitNameCheckbox.Selected;
                searchBy.Value = "All";
                if (siteNameChecked == true)
                {
                    searchBy.Value = "SiteName";
                }
                else if (unitNameChecked == true)
                {
                    searchBy.Value = "UnitName";
                }
                else if (descriptionChecked == true)
                {
                    searchBy.Value = "Description";
                }
                DeactivatedUnitListView.DataSourceID = SearchListODS.ID;
                DeactivatedUnitListView.DataBind();
                MessageUserControl.Visible = false;
            }
        });
    }

    /// <summary>
    /// Clear button for the
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void ActiveClearButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            ActiveSearchBox.Text = "";
            ActiveUnitListView.DataSourceID = ActiveUnitListView_ODS.ID;
            ActiveUnitListView.DataBind();
            MessageUserControl.Visible = false;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void DeactivatedClearButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            DeactivatedSearchBox.Text = "";
            DeactivatedUnitListView.DataSourceID = DeactivatedUnitListView_ODS.ID;
            DeactivatedUnitListView.DataBind();
            MessageUserControl.Visible = false;
        });
    }

    /// <summary>
    /// This method runs when the Drop Down List(DDL) for the site is DataBound. It creates sets the tooltip when you hover over with a mouse so you can see the full site name.
    /// </summary>
    /// <param name="sender">Sender of the PageLoad IE: Button</param>
    /// <param name="e">Arguments submitted by the sender. IE: "Cancel"</param>
    protected void SiteNameDDL_DataBound(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            using (MSSContext context = new MSSContext())
            {
                // Grab the DropDownList
                DropDownList ddl = sender as DropDownList;

                // If the DDL isnt null, reorder it alphabetically.
                if (ddl != null)
                {
                    ReorderAlphabetized(ddl);

                    int siteId = int.Parse(ddl.SelectedValue);
                    string toolTipText = (from x in context.Units
                                          where x.SiteId == siteId
                                          select x.Site.SiteName).FirstOrDefault();
                    ddl.ToolTip = toolTipText;
                }
            }
        });
    }

    protected void AddUnitButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            MessageUserControl.Visible = true;

            DropDownList siteNameDDL = AddSiteNameDDL;
            int siteId = int.Parse(siteNameDDL.SelectedValue);
            string unitName = AddUnitName.Text;
            string description = AddUnitDescription.Text;

            Utility utility = new Utility();
            utility.checkValidString(unitName);
            utility.checkValidString(description);

            if (string.IsNullOrEmpty(unitName))
            {
                MessageUserControl.ShowInfo("Warning", "Unit Name is required.");
            }
            else if (string.IsNullOrEmpty(description))
            {
                MessageUserControl.ShowInfo("Warning", "Description is required.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    UnitController sysmgr = new UnitController();
                    sysmgr.Unit_Add(siteId, unitName, description, int.Parse(UserSiteId.Text));
                    ActiveUnitListView.DataSourceID = ActiveUnitListView_ODS.ID;
                    AddUnitListView.DataBind();
                    ActiveUnitListView.DataBind();
                    
                    // Clear fields after add
                    AddClearButton_Click(sender, e);
                }, "Success", "New unit has been added.");
            }
        });

    }

    protected void AddClearButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            AddSiteNameDDL.SelectedIndex = 0;
            AddUnitName.Text = "";
            AddUnitDescription.Text = "";
        });
    }

    protected static void ReorderAlphabetized(DropDownList ddl)
    {
        List<ListItem> listCopy = new List<ListItem>();
        foreach (ListItem item in ddl.Items)
            listCopy.Add(item);
        ddl.Items.Clear();
        foreach (ListItem item in listCopy.OrderBy(item => item.Text))
            ddl.Items.Add(item);
    }
}