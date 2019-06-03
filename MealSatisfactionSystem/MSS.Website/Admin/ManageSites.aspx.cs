using MSS.Data.Entities.Security;
using MSSSystem.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Contains the code the drives all functions built into the ManageSites.aspx page
/// </summary>
public partial class AdminPortal_ManageSites : System.Web.UI.Page
{
    /// <summary>
    /// Redirects any non-superuser users to login and also hides the UserControl message.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void Page_Load(object sender, EventArgs e)
    {       
        // Check and make sure the user is a SuperUser
        if (!User.IsInRole(SecurityRoles.SuperUser))
        {
            Response.Redirect("~/Account/Login.aspx");
        }
        MessageUserControl.Visible = false;
    }

    #region MessageUserControlValidation
    /// <summary>
    /// Allows the MessageUserControl to handle any exceptions thrown from violating the constraints within the entity classes.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.Any event caused by an ODS in code.</param>
    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.Visible = true;
        MessageUserControl.HandleDataBoundException(e);
    }
    #endregion

    #region ListViewMethods
    /// <summary>
    /// Handles all the events caused by clicking any button in the ActiveSiteListView.  
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void SiteListView_ItemCommand(object sender, ListViewCommandEventArgs e)    {
        
        MessageUserControl.Visible = false;
        int siteId = 1;
        // If the cancel button is pressed the CommandArgument will be null so we do not want to try to convert it to string.
        // Otherwise we want to get the siteId of the row the user clicked the button on.
        if (!e.CommandName.Equals("Cancel"))
        {
            siteId = int.Parse(e.CommandArgument.ToString());
        }
        // If the user presses the "Update" button we grab the values they input into the textboxes and then run Site_Update using them as the parameters.
        // Afterwords we must set EditIndex to -1 to deactivate edit mode on the updated field.
        if (e.CommandName.Equals("Change"))
        {
            // Grabs the index of the current item in the listview.
            int i = e.Item.DisplayIndex;

            // Gets the values the user has input into the textboxs and assigns them to a string variable.
            TextBox siteNameBox = ActiveSiteListView.Items[i].FindControl("SiteNameTextBox") as TextBox;
            string siteName = siteNameBox.Text;
            TextBox descriptionBox = ActiveSiteListView.Items[i].FindControl("DescriptionTextBox") as TextBox;
            string description = descriptionBox.Text;
            TextBox passcodeBox = ActiveSiteListView.Items[i].FindControl("PasscodeTextBox") as TextBox;
            string passcode = passcodeBox.Text;

            MessageUserControl.TryRun(() =>
            {                
                MessageUserControl.Visible = true;

                // Validation for special characters.
                Utility utility = new Utility();
                utility.checkValidString(siteName);
                utility.checkValidString(description);
                utility.checkValidString(passcode);

                // Updates the selected site, turns off the edit mode and rebinds all active site listviews.
                SiteController sysmgr = new SiteController();
                sysmgr.Site_Update(siteId, siteName, description, passcode);
                ActiveSiteListView.DataSourceID = ActiveSiteListODS.ID;
                ActiveSiteListView.EditIndex = -1;
                ActiveSiteListView.DataBind();
                AddSiteListView.DataBind();

                // Clears the search field and re-enables all the controls that are disabled during edit mode.
                ActiveSearchBox.Text = "";                               
                ActiveSearchButton.Enabled = true;
                ActiveClearButton.Enabled = true;
                DataPager pager = ActiveSiteListView.FindControl("ActiveDataPager") as DataPager;
                pager.Visible = true;

            }, "Success", "Site has been updated");
        }
        // If the user presses deactivate we simply take the siteId from above and deactivate the site it is attributed to.
        else if (e.CommandName.Equals("Deactivate"))
        {
            MessageUserControl.TryRun(() =>
            {
                MessageUserControl.Visible = true;

                // Deactivates the current site.
                SiteController sysmgr = new SiteController();
                sysmgr.Site_Deactivate(siteId);
            }, "Success", "Site has been deactivated");

            // Rebinds all listviews and resets the ODS' to their defaults rather than the search ODS.
            ActiveSiteListView.DataSourceID = ActiveSiteListODS.ID;
            DeactivatedSiteListView.DataSourceID = DeactivatedSiteListODS.ID;
            ActiveSiteListView.DataBind();
            AddSiteListView.DataBind();
            DeactivatedSiteListView.DataBind();
        }

    }

    /// <summary>
    /// Disables the search buttons and hides the datapager when the edit button is pushed.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void ActiveSiteListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {        
        ActiveSearchButton.Enabled = false;
        ActiveClearButton.Enabled = false;
        DataPager pager = ActiveSiteListView.FindControl("ActiveDataPager") as DataPager;
        pager.Visible = false;
    }

    /// <summary>
    /// Reenables the search buttons and datapager when the edit item cancel button is pushed.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void ActiveSiteListView_ItemCanceling(object sender, ListViewCancelEventArgs e)    {
        
        ActiveSearchButton.Enabled = true;
        ActiveClearButton.Enabled = true;
        DataPager pager = ActiveSiteListView.FindControl("ActiveDataPager") as DataPager;
        pager.Visible = true;
    }

    /// <summary>
    /// Hides all other rows when the user enters edit mode to make editing sites easier.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void ActiveSiteListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (ActiveSiteListView.EditIndex != -1)
        {
            if (e.Item.DisplayIndex != ActiveSiteListView.EditIndex)
            {
                e.Item.Visible = false;
            }
        }
    }
    #endregion

    #region SearchBarButtons
    /// <summary>
    /// Searches deactivated sites based on the selected radio button and the users input in the search box.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void DeactivatedSearchButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            if (DeactivatedSearchBox.Text != "")
            {
                MessageUserControl.Visible = true;
                // Sets the hidden searchArg field to the user's search field text for use by the SearchListODS.                
                searchArg.Value = DeactivatedSearchBox.Text;

                // Special character validation.
                Utility utility = new Utility();
                utility.checkValidString(searchArg.Value);

                // Sets the deactivated flag and the searchBy hidden field value.
                deactivatedSearch.Value = "true";
                bool siteNameChecked = DeactivatedSiteNameCheckbox.Selected;
                bool descriptionChecked = DeactivatedDescriptionCheckbox.Selected;
                searchBy.Value = "All";
                if (siteNameChecked == true)
                {
                    searchBy.Value = "Site Name";
                }
                else if (descriptionChecked == true)
                {
                    searchBy.Value = "Description";
                }

                // Changes the listview ODS to the search ODS and binds it.
                DeactivatedSiteListView.DataSourceID = SearchListODS.ID;
                DeactivatedSiteListView.DataBind();
                MessageUserControl.Visible = false;
            }
        });

    }



    /// <summary>
    /// Searches active sites based on the selected radio button and the users input in the search box.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void ActiveSearchButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            if (ActiveSearchBox.Text != "")
            {
                MessageUserControl.Visible = true;

                // Sets the hidden searchArg field to the user's search field text for use by the SearchListODS
                searchArg.Value = ActiveSearchBox.Text;

                // Special character validation.
                Utility utility = new Utility();
                utility.checkValidString(searchArg.Value);

                // Sets the deactivated flag and the searchBy hidden field value.
                deactivatedSearch.Value = "false";
                bool siteNameChecked = ActiveSiteNameCheckbox.Selected;
                bool descriptionChecked = ActiveDescriptionCheckbox.Selected;
                searchBy.Value = "All";
                if (siteNameChecked == true)
                {
                    searchBy.Value = "Site Name";
                }
                else if (descriptionChecked == true)
                {
                    searchBy.Value = "Description";
                }

                // Changes the listview ODS to the search ODS and binds it.
                ActiveSiteListView.DataSourceID = SearchListODS.ID;
                ActiveSiteListView.DataBind();
                MessageUserControl.Visible = false;
            }
        });

    }

    /// <summary>
    /// Clears the search textbox and repopulates the sitelist with all active sites.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void ActiveClearButton_Click(object sender, EventArgs e)
    {
        ActiveSearchBox.Text = "";
        ActiveSiteListView.DataSourceID = ActiveSiteListODS.ID;
        ActiveSiteListView.DataBind();
        MessageUserControl.Visible = false;
    }

    /// <summary>
    /// Clears the search textbox and repopulates the sitelist with all deactivated sites.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void DeactivatedClearButton_Click(object sender, EventArgs e)
    {
        DeactivatedSearchBox.Text = "";
        DeactivatedSiteListView.DataSourceID = DeactivatedSiteListODS.ID;
        DeactivatedSiteListView.DataBind();
        MessageUserControl.Visible = false;
    }
    #endregion

    #region AddTabButtons
    /// <summary>
    /// Takes the user's input in the textboxes and adds a new site using those parameters.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void AddButton_Click(object sender, EventArgs e)
    {

        MessageUserControl.TryRun(() =>
        {
            MessageUserControl.Visible = true;

            // Get the user's inputs from the textboxes.
            string siteName = AddSiteNameTextBox.Text;
            string description = AddDescriptionTextBox.Text;
            string passcode = AddPasscodeTextBox.Text;

            // Special character validation.
            Utility utility = new Utility();
            utility.checkValidString(siteName);
            utility.checkValidString(description);
            utility.checkValidString(passcode);

            // Adds the new site to the database.
            SiteController sysmgr = new SiteController();
            sysmgr.Site_Add(siteName, description, passcode);

            // Clears the textboxes when a site is successfully added. 
            AddSiteNameTextBox.Text = "";
            AddDescriptionTextBox.Text = "";
            AddPasscodeTextBox.Text = "";

            // Resets the default datasource and rebinds all active site lists.
            ActiveSiteListView.DataSourceID = ActiveSiteListODS.ID;
            AddSiteListView.DataBind();
            ActiveSiteListView.DataBind();

        }, "Success", "New site has been added.");

    }

    /// <summary>
    /// Clears all the add site textboxes for the user.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        AddSiteNameTextBox.Text = "";
        AddDescriptionTextBox.Text = "";
        AddPasscodeTextBox.Text = "";
    }
    #endregion

    #region PasscodeButton (Currently unused)
    /// <summary>
    /// Changes the passcode of every site to a new, randomly generated passcode this is same process that occurs at midnight every day. 
    /// This method is attached to the Passcode button which is currently commented out but left in for testing purposes.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void PasscodeButton_Click(object sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            MessageUserControl.Visible = true;
            SiteController sysmgr = new SiteController();
            sysmgr.Site_ChangePasscode();
            ActiveSiteListView.DataBind();
        }, "Success", "Site passcodes have been changed.");

    }
    #endregion



}