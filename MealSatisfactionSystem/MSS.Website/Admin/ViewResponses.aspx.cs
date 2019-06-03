using Microsoft.AspNet.Identity;
using MSS.Data.Entities.Security;
using MSS.Data.POCOs;
using MSSSystem.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSSSystem.BLL.Security;
using System.Reflection;
using System.Collections;
/// <summary>
/// ViewResponses class contains the code that drives all functions built into the ViewResponses.aspx page, which includes displaying survey responses, filtering all responses based on user input and displaying a single selected response.
/// </summary>
public partial class Admin_ViewResponses : System.Web.UI.Page
{
    //Declare controllers being used.
    SurveyResponseController sysmgr = new SurveyResponseController();
    UnitController unitmgr = new UnitController();

    //Declare variables being used throughout the methods.
    string filterList;
    private const int UNSELECTED_SITE = 0;
    int siteId;

    //Declare variables for ViewState. 
    //ViewState is needed to have a functional DataPager.
    List<string> SiteFilterList;
    List<string> UnitFilterList;
    List<string> GenderFilterList;
    List<string> AgeFilterList;
    DateTime? StartDateFilter;
    DateTime? EndDateFilter;


    #region PageEvents
    /// <summary>
    /// Checks to make sure that the user has appropriate access to the page and sets up the ViewState for the filters, used for the DataPager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        MessageUserControl.Visible = false;

        //Check and make sure the user has either a AdministratorView, AdministratorEdit, or SuperUser role, otherwise redirect the user to the 'Login' page.
        if (!User.IsInRole(SecurityRoles.SuperUser) && !User.IsInRole(SecurityRoles.AdminViews) && !User.IsInRole(SecurityRoles.AdminEdits))
        {
            Response.Redirect("~/Account/Login.aspx");
        }
        else
        {
            //If there is a ViewState, set the filters. 
            if (ViewState["siteFilterViewState"] != null)
            {
                SiteFilterList = (List<string>)ViewState["siteFilterViewState"];
                UnitFilterList = (List<string>)ViewState["unitFilterViewState"];
                GenderFilterList = (List<string>)ViewState["genderFilterViewState"];
                AgeFilterList = (List<string>)ViewState["ageFilterViewState"];
                StartDateFilter = (DateTime?)ViewState["fromDateFilterViewState"];
                EndDateFilter = (DateTime?)ViewState["toDateFilterViewState"];
            }
            //Otherwise, create the filters.
            else
            {
                SetupFilters();
            }
            //Display the page based on the user's role.
            SetupPage();
        }
    }

    /// <summary>
    /// Adds the ViewStates to the session, displays filter messages, and disables the filter buttons when required (i.e. a user with a non-SuperUser role has a site with no active units or submitted surveys).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Page_PreRender(object sender, EventArgs e)
    {
        ViewState.Add("siteFilterViewState", SiteFilterList);
        ViewState.Add("unitFilterViewState", UnitFilterList);
        ViewState.Add("genderFilterViewState", GenderFilterList);
        ViewState.Add("ageFilterViewState", AgeFilterList);
        ViewState.Add("fromDateFilterViewState", StartDateFilter);
        ViewState.Add("toDateFilterViewState", EndDateFilter);

        SurveyOverviewListView.DataSourceID = null;
        SurveyOverviewListView.DataSource = sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList);
        SurveyOverviewListView.DataBind();

        //If there are no surveys returned, determine whether it is because there are no active sites, no active units, or no surveys attached to the site and alert the user accordingly.
        if (sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList).Count() <= 0)
        {
            SurveyOverviewListView.Visible = false;
            //If the user is not a SuperUser and there are no units, determine if it's because the site is deactivated or there are no active units and alert the user accordingly.
            if (!ViewAllPrivileges() && UnitCheckboxList.Items.Count <= 0)
            {
                //Wrapped in a try-catch to fix a bug that occured when the user with a disabled site logged off.
                try
                {
                    if (sysmgr.Get_SiteStatus(siteId) == true)
                    {
                        MessageUserControl.ShowInfoError("Alert", "The " + sysmgr.Get_SiteName(siteId) + " site is not active and no surveys can be viewed.");
                        DisableButtons(false, false, false, false);
                    }
                    else
                    {
                        MessageUserControl.ShowInfoError("Alert", "The " + sysmgr.Get_SiteName(siteId) + " site does not have any active units and no surveys can be viewed.");
                        DisableButtons(false, true, false, false);
                    }
                }
                catch
                {
                    //Will log the user off.
                }
            }
            //If the user does not have a SuperUser role and the site has at least one unit, alert the user that the site does not have any surveys that have been submitted. 
            else if (!ViewAllPrivileges() && UnitCheckboxList.Items.Count > 0)
            {
                //Wrapped in a try-catch to fix a bug that occured when the user with a site that has no submitted surveys logged off.
                try
                {
                    List<string> surveySite = new List<string>();
                    surveySite.Add(sysmgr.Get_SiteName(GetUserSiteId()));
                    if (sysmgr.Response_List_Filters(surveySite, GetCheckboxFiltersAll(UnitCheckboxList), GetCheckboxFiltersAll(GenderCheckboxList), null, null, GetCheckboxFiltersAll(AgeCheckboxList)).Count() <= 0)
                    {
                        MessageUserControl.ShowInfoError("Alert", "The " + sysmgr.Get_SiteName(siteId) + " site does not have any surveys submitted.");
                        DisableButtons(false, false, false, true);
                    }
                }
                catch
                {
                    //Will just log the user off.
                }
            }
            //Otherwise, the user has a SuperUser role. 
            else if (ViewAllPrivileges())
            {
                //If no site has been selected (e.g. the SiteDDL is "All Sites"), check to make sure that there is at least one active site.
                if (siteId == 0)
                {
                    //If there is at least one active site, determine if there are any active units to make sure that at least one survey exists.
                    if (sysmgr.Active_Site_List().Count() > 0)
                    {
                        if (sysmgr.Active_Unit_List().Count() > 0)
                        {
                            //If there are no submitted surveys for any sites that have an active unit, alert the user that no surveys have been submitted.
                            if (sysmgr.Response_List_All().Count <= 0)
                            {
                                MessageUserControl.ShowInfoError("Error", "No surveys have been submitted for any sites.");
                                DisableButtons(false, false, false, false);
                            }
                        }
                        //Otherwise, alert the user that there are no active units within the site.
                        else
                        {
                            MessageUserControl.ShowInfoError("Error", "There are no active units for any sites. No surveys can be viewed");
                            DisableButtons(false, false, false, false);
                        }
                    }
                    //Otherwise, alert the user that there are no active sites.
                    else
                    {
                        MessageUserControl.ShowInfoError("Error", "There are no active sites. No surveys can be viewed");
                        DisableButtons(false, false, false, false);
                    }
                }
                else
                //If a site has been selected, check to make sure that unit is active.
                {
                    List<string> surveySite = new List<string>();
                    surveySite.Add(sysmgr.Get_SiteName(siteId));
                    //If there are no active units, alert the user.
                    if (UnitCheckboxList.Items.Count <= 0)
                    {
                        MessageUserControl.ShowInfoError("Error", "The " + sysmgr.Get_SiteName(siteId) + " site has no active units and no surveys can be viewed.  Please select a different site.");
                        DisableButtons(true, true, true, false);
                    }
                    //If there are active units but no surveys submitted, alert the user.
                    else if (sysmgr.Response_List_Filters(surveySite, GetCheckboxFiltersAll(UnitCheckboxList), GetCheckboxFiltersAll(GenderCheckboxList), null, null, GetCheckboxFiltersAll(AgeCheckboxList)).Count <= 0)
                    {
                        MessageUserControl.ShowInfoError("Error", "The " + sysmgr.Get_SiteName(siteId) + " site does not have any surveys submitted. Please select a different site.");
                    }
                }
            }
        }
    }


    /// <summary>
    /// Binds the filters to the SurveyOverviewListView's Datapager. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SurveyOverviewListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        MessageUserControl.Visible = true;

        //Find the Datapager and bind the filters to it.
        (SurveyOverviewListView.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        SurveyOverviewListView.DataSource = sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList);
        SurveyOverviewListView.DataBind();

        //If there is more than one result, make sure SurveyOverviewListView is visible.
        if (sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList).Count() <= 0)
        {
            SurveyOverviewListView.Visible = false;
        }
    }
    #endregion

    #region PageDisplayHelperMethods
    /// <summary>
    /// Changes the title and subtitle that is displayed on the ViewResponses page.
    /// </summary>
    /// <remarks>
    /// The title and the subtitle change based on whether the user has a SuperUser role and whether the user is looking at the SurveyOverview screen or the IndividualResponse screen.
    /// </remarks>
    /// <param name="heading">Contains a string with the title of the page.</param>
    /// <param name="tagline">Contains a string with the tagline of the page.</param>
    private void UpdatePageHeading(string heading, string tagline)
    {
        PageHeading.InnerText = heading;
        PageTagline.InnerText = tagline;
    }
    #endregion

    #region SurveyOverviewScreen_FilterGenerationMethods
    /// <summary>
    /// Ensures that the UnitCheckboxList is visible when a specific SiteId has been selected and is hidden when a SiteId has not been selected. Populates the UnitCheckboxList with all Units that are associated with the selected SiteId.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SiteDDL_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(MessageUserControl.ToString()))
        {
            MessageUserControl.Visible = false;
        }
        else
        {
            MessageUserControl.Visible = true;
        }

        //Change the value of the siteId based on the SiteDDL.
        siteId = int.Parse(SiteDDL.SelectedValue);

        //Render the appropriate UnitCheckbox List based on the new siteId.
        GenerateUnitList(siteId);
    }

    /// <summary>
    /// Updates the UnitCheckboxList based on the selected SiteId, inputted via the SiteDDL control.
    /// </summary>
    /// <param name="siteId">Contains an integer with the SiteId of the selected Site.</param>
    private void GenerateUnitList(int siteId)
    {
        //Clear the existing UnitCheckboxList data source and set a new one, based on the siteId.
        UnitCheckboxList.DataSourceID = null;
        UnitCheckboxList.Items.Clear();
        UnitCheckboxList.DataSource = unitmgr.SiteUnitList(siteId);
        UnitCheckboxList.DataBind();

        //If a site has no active units, hide the UnitCheckboxList and show a label that says "No Active Units".
        if (UnitCheckboxList.Items.Count <= 0 && siteId != 0)
        {
            if (ViewAllPrivileges())
            {
                ViewSiteAndUnitInformation(true, false);
            }
            else
            {
                ViewSiteAndUnitInformation(false, false);
            }
            UnitAlert.Visible = true;
        }
        //Otherwise, change the filter bar displayed based on whether the user has a SuperUser role or not.
        else
        {
            if (ViewAllPrivileges())
            {
                UpdateSuperUserSiteUnitLists();
            }
            else
            {
                ViewSiteAndUnitInformation(false, true);
            }
        }
    }
    #endregion

    #region SurveyOverviewScreen_FilterHelperMethods
    /// <summary>
    /// Initalizes all filters, depending on if the user is a SuperUser or not.
    /// </summary>
    private void SetupFilters()
    {
        //If the user has a SuperUser role, set the site and unit filters to all active sites and units, respectively.
        if (ViewAllPrivileges())
        {
            SiteFilterList = sysmgr.Active_Site_List();
            UnitFilterList = sysmgr.Active_Unit_List();
        }
        //Otherwise, get the user's site and add the site name to the site filter.
        //Generate the corresponding unit list and add those units to the unit filter.
        else
        {
            siteId = GetUserSiteId();

            List<string> siteName = new List<string>();
            siteName.Add(sysmgr.Get_SiteName(GetUserSiteId()));
            SiteFilterList = siteName;

            GenerateUnitList(siteId);
            UnitFilterList = GetCheckboxFilters(UnitCheckboxList);
        }

        //Regardless of the user's role, add the gender and age filters, and the date filters. 
        AgeFilterList = GetCheckboxFilters(AgeCheckboxList);
        GenderFilterList = GetCheckboxFilters(GenderCheckboxList);
        StartDateFilter = CreateFilterDate("From Date", "");
        EndDateFilter = CreateFilterDate("To Date", "");
    }

    /// <summary>
    /// Ensures that the user has entered either a valid DateTime format or a null value. If the format is invalid, this method throws an exception that alerts the user of their mistake.
    /// </summary>
    /// <param name="inputtedDate">Contains a string with the date to be parsed.</param>
    /// <param name="errorMessage">Contains a string with the name of the control, used in the error message.</param>
    /// <returns>A valid or null DateTime.</returns>
    /// <exception cref="Exception">The inputtedDate is not null and not in the correct format.</exception>
    private DateTime? GetDate(string inputtedDate, string errorMessage)
    {
        DateTime? date;

        if (!string.IsNullOrEmpty(inputtedDate))
        {
            try
            {
                date = DateTime.ParseExact(inputtedDate, "MM/dd/yyyy", null);
            }
            catch
            {
                SurveyOverviewListView.Visible = false;
                throw new Exception("Invalid " + errorMessage + ". Please ensure that it is in the MM/DD/YYYY format and try again.");
            }
        }
        else
        {
            date = null;
        }

        return date;
    }

    /// <summary>
    /// Validates a date that is inputted from a Textbox control.If the input is not valid, sets the DateTime to null. 
    /// </summary>
    /// <param name="filterName">Contains a string with the name of the control holding the inputted date.</param>
    /// <param name="dateString">Contains a string with the date to be parsed.</param>
    /// <returns>A valid DateTime or a null value.</returns>
    /// <remarks>This method is different from the GetDate() method because, in the event of an invalid string, it does not throw an exception.</remarks>
    private DateTime? CreateFilterDate(string filterName, string dateString)
    {
        DateTime? date;

        if (String.IsNullOrEmpty(GetFilterLineDate(filterName, dateString)))
        {
            date = null;
        }
        else
        {
            date = DateTime.Parse(dateString);
        }
        return date;
    }

    /// <summary>
    /// Finds the site that has been selected, if one, when the user has a SuperUser role. If the user does not have a SuperUser role, this method finds the user's site based on their profile.
    /// </summary>
    /// <returns>A list of SiteName strings.</returns>
    private List<String> GetSiteFilter()
    {
        //Initialize a new list.
        List<string> sites = new List<string>();

        //If the user does not have a SuperUser role, they can only have one possible site to view. Find this site and add it to the initialized list.
        if (!ViewAllPrivileges())
        {
            siteId = GetUserSiteId();
            sites.Add(sysmgr.Get_SiteName(siteId));
        }
        else
        {
            //If the user has a SuperUser role and no site has been selected from the SiteDDL, add all active sites to the initialized list.
            if (SiteDDL.SelectedValue == "0")
            {
                sites = sysmgr.Active_Site_List();
            }
            //Otherwise, find the selected site from the SiteDDL and add it to the list.
            else
            {
                for (int i = 0; i < SiteDDL.Items.Count; i++)
                {
                    if (SiteDDL.Items[i].Selected)
                    {
                        sites.Add(SiteDDL.Items[i].Text);
                    }
                }
            }
        }

        //Return the list.
        return sites;
    }

    /// <summary>
    /// Finds the units that have been checked, if any, from the UnitCheckboxList. If no units have been checked, finds all units from either one site or all sites, depending on the user's role.
    /// </summary>
    /// <returns>A list of UnitName strings.</returns>
    private List<String> GetUnitFilters()
    {
        //Initialize a new list.
        List<string> units = new List<string>();

        //If the user has a SuperUser role and no site has been selected, add all active units from every active site to the initialized list. 
        if (SiteDDL.SelectedValue == "0" && ViewAllPrivileges())
        {
            units = sysmgr.Active_Unit_List();
        }
        //Otherwise, if the UnitCheckbox list has no items checked, add all UnitCheckbox list items to the initialized list.
        //This brace is applicable to users with non-SuperUser roles who cannot view multiple sites.
        else if (UnitCheckboxList.SelectedIndex == -1)
        {
            for (int i = 0; i < UnitCheckboxList.Items.Count; i++)
            {
                units.Add(UnitCheckboxList.Items[i].Value);

            }
        }
        //Otherwise, the UnitCheckbox list has been selected. Add all checked items to the initialized list.
        else
        {
            for (int i = 0; i < UnitCheckboxList.Items.Count; i++)
            {
                if (UnitCheckboxList.Items[i].Selected)
                {
                    units.Add(UnitCheckboxList.Items[i].Value);
                }
            }
        }

        //Return the list.
        return units;
    }

    /// <summary>
    /// Finds all items in a CheckboxList, regardless if the items have been checked.
    /// </summary>
    /// <param name="checkboxList">Contains a CheckboxList with a collection of Checkbox items.</param>
    /// <returns>A list of all the checkbox items' values in strings.</returns>
    private List<String> GetCheckboxFiltersAll(CheckBoxList checkboxList)
    {
        //Initialize a new list.
        List<string> checkboxFilters = new List<string>();

        //For every checkbox item, add its value to the initialized list.
        for (int i = 0; i < checkboxList.Items.Count; i++)
        {
            checkboxFilters.Add(checkboxList.Items[i].Value);
        }
        //Return the list.
        return checkboxFilters;
    }

    /// <summary>
    /// Determines if any items in a CheckboxList have been checked and, if so, finds all checked items. If no CheckboxList items have been checked, the method finds all items in the CheckboxList.
    /// </summary>
    /// <param name="checkboxList">Contains a CheckboxList with a collection of Checkbox items being filtered.</param>
    /// <returns>A list of the selected checkbox items' values strings.</returns>
    private List<String> GetCheckboxFilters(CheckBoxList checkboxList)
    {
        //Initialize a new list.
        List<string> checkboxFilters = new List<string>();

        //If no item has been checked in the checkbox list, return all checkbox list items to the initialized list.
        if (checkboxList.SelectedIndex == -1)
        {
            for (int i = 0; i < checkboxList.Items.Count; i++)
            {
                checkboxFilters.Add(checkboxList.Items[i].Value);
            }
        }
        //Otherwise, return all checked checkbox list items to the initialized list.
        else
        {
            for (int i = 0; i < checkboxList.Items.Count; i++)
            {
                if (checkboxList.Items[i].Selected)
                {
                    checkboxFilters.Add(checkboxList.Items[i].Value);
                }
            }
        }

        //Return the list.
        return checkboxFilters;
    }
    #endregion

    #region SurveyOverviewScreen_FilterMessageMethods
    /// <summary>
    /// Determines if the user has inputted any filters and, if so, creates a message that indicates what values were filtered.
    /// </summary>
    /// <param name="startDate">Contains a string with the text in the "From Date" textbox control.</param>
    /// <param name="endDate">Contains a string with the text in the "To Date" textbox control.</param>
    /// <param name="siteBool">Contains a boolean with a true/false value that indicates if a Site has been selected (true) or not (false).</param>
    /// <param name="site">Contains a list of SiteName strings.</param>
    /// <param name="unitBool">Contains a boolean with a true/false value that indicates if at least one Unit has been checked (true) or not (false).</param>
    /// <param name="units">Contains a list of UnitName strings.</param>
    /// <param name="genderBool">Contains a boolean with a true/false value that indicates if at least one Gender has been checked (true) or not (false).</param>
    /// <param name="genders">Contains a list of Gender strings.</param>
    /// <param name="ageBool">Contains a boolean with a true/false value that indicates if at least one Age has been checked (true) or not (false).</param>
    /// <param name="ages">Contains a list of Age strings.</param>
    /// <returns>A string of all filters that were selected.</returns>
    private String GetFilterMessage(string startDate, string endDate, bool siteBool, List<string> site, bool unitBool, List<string> units, bool genderBool, List<string> genders, bool ageBool, List<string> ages)
    {
        //Initialize the filter message.
        string filterList = "";

        //If the startDate is not empty and the date is valid (determined via GetFilterLineDate() method), create a line in the filter message for From Date.
        if (!String.IsNullOrEmpty(startDate))
        {
            if (!String.IsNullOrEmpty(GetFilterLineDate("From Date: ", startDate)))
            {
                filterList += GetFilterLineDate("From Date: ", startDate);
            }
        }

        //If the endDate is not empty and the date is valid (determined via GetFilterLineDate() method), create a line in the filter message for To Date.
        if (endDate != null)
        {
            if (!String.IsNullOrEmpty(GetFilterLineDate("To Date: ", endDate)))
            {
                filterList += GetFilterLineDate("To Date: ", endDate);
            }
        }

        //If the SiteDDL has been changed (siteBool = true) and the user has a SuperUser role, create a line in the filter message for Site.
        if (siteBool && ViewAllPrivileges())
        {
            filterList += GetFilterLine("Site: ", site);
        }

        //If the UnitCheckboxList has been selected (unitBool = true), create a line in the filter message for Unit(s).
        if (unitBool)
        {
            filterList += GetFilterLine("Unit(s): ", units);
        }

        //If the GenderCheckboxList has been selected (genderBool = true), create a line in the filter message for Gender(s).
        if (genderBool)
        {
            filterList += GetFilterLine("Gender(s): ", genders);
        }

        //If the AgeCheckboxList has been selected (ageBool = true), create a line in the filter message for Ages(s).
        if (ageBool)
        {
            filterList += GetFilterLine("Age(s): ", ages);
        }

        //Return the list.
        return filterList;
    }

    /// <summary>
    /// Formats one line in the filter message for the date filters.
    /// </summary>
    /// <remarks>
    /// This method is separate from GetFilterLine method because it requires DateTime validation.
    /// </remarks>
    /// <param name="filterName">Contains a string with the name of the control holding the date.</param>
    /// <param name="dateString">Contains a string with the inputted date.</param>
    /// <returns>A string that is one line of the filter message.</returns>
    private String GetFilterLineDate(string filterName, string dateString)
    {
        string line = "";
        DateTime validDateTime;
        DateTime? enteredDateTime;
        //If the dateString entered is not valid, set it to null.
        if (DateTime.TryParse(dateString, out validDateTime))
        {
            enteredDateTime = validDateTime;
        }
        else
        {
            enteredDateTime = null;
        }
        //If the dateString is not null, format the line. Otherwise, return an empty string.
        if (enteredDateTime != null)
        {
            line = "\t\u2022 " + filterName + enteredDateTime.Value.ToString("MMMM dd, yyyy") + "<br />";
        }
        return line;
    }

    /// <summary>
    /// Formats one line in the filter message for the Site, Unit, Gender, and Age filters.
    /// </summary>
    /// <param name="filterName">Contains a string with the name of the filter label.</param>
    /// <param name="filters">Contains a list of the filters selected as strings.</param>
    /// <returns>A string that is one line of the filter message.</returns>
    private String GetFilterLine(string filterName, List<string> filters)
    {
        String line = "\t\u2022 " + filterName + String.Join(", ", filters.ToArray()) + "<br />";
        return line;
    }
    #endregion

    #region SurveyOverviewScreen_FilterButtons
    /// <summary>
    /// Collects the user input in the search bar and refreshes the SurveyOverviewListView to display all survey responses that match the input.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ApplyFilters_Click(object sender, EventArgs e)
    {
        //Update the filters by grabbing all input.
        SiteFilterList = GetSiteFilter();
        UnitFilterList = GetUnitFilters();
        GenderFilterList = GetCheckboxFilters(GenderCheckboxList);
        AgeFilterList = GetCheckboxFilters(AgeCheckboxList);
        StartDateFilter = CreateFilterDate("From Date", FromDate.Text);
        EndDateFilter = CreateFilterDate("To Date", ToDate.Text);

        //Reset the datapage to the first index, if possible. Otherwise, just rebind the Data.
        //This block is done to prevent a bug. For example, without this try-catch, if the user is on DataPager page 3 and clicks apply filters for a table that has only 1 page, it would not show.
        try
        {
            if (sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList).Count() > 0)
            {
                (SurveyOverviewListView.FindControl("DataPager1") as DataPager).SetPageProperties(0, 10, false);
            }
        }
        catch
        {
            SurveyOverviewListView.DataBind();
        }

        //Search for filters.
        SearchFilters();
    }

    /// <summary>
    /// Clears all the applied filters, resets the SurveyOverviewListView to display all survey responses from active sites and units (SuperUser role) or active units (non-SuperUser role), and clears all user input in the search bar.
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    protected void ClearFilters_Click(object Sender, EventArgs e)
    {
        MessageUserControl.TryRun(() =>
        {
            //Reset the Datapager if it exists
            if (sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList).Count() > 0)
            {
                (SurveyOverviewListView.FindControl("DataPager1") as DataPager).SetPageProperties(0, 10, false);
            }

            MessageUserControl.Visible = true;

            //Clear all user input.
            FromDate.Text = "";
            ToDate.Text = "";
            SiteDDL.Enabled = true;
            SiteDDL.SelectedIndex = -1;
            siteId = UNSELECTED_SITE;
            GenerateUnitList(siteId);
            AgeCheckboxList.ClearSelection();
            GenderCheckboxList.ClearSelection();

            SetupFilters();

            //Reset SurveyOverview based on the initalized filters.
            SurveyOverviewListView.Visible = true;
            SurveyOverviewListView.DataSource = sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList);
            SurveyOverviewListView.DataBind();

        }, "Success", "All filters have been cleared. You are viewing all surveys.");
    }
    #endregion

    #region SurveyOverviewScreen_ListViewButtons
    /// <summary>
    /// Hides the SurveyOverview screen and displays the IndividualResponse screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void View_Click(object sender, EventArgs e)
    {
        //Hide the SurveyOverview screen and show the IndividualResponse screen.
        IndividualResponseScreen.Visible = true;
        SurveysListViewScreen.Visible = false;
        UpdatePageHeading("Individual Response", "View a selected Individual Response.");

        //Ensure that the responseId that is passed through the 'View' button is a valid integer. 
        //If it is not valid, display an error message and hide the survey panel.
        int responseId;
        bool validInt = int.TryParse(((Button)sender).CommandArgument, out responseId);
        if (!validInt)
        {
            responseId = 0;
            MessageUserControlIndividual.ShowInfoError("Error", "The ResponseID is not valid. Please try again.");
            SurveyInformation.Visible = false;
            BackBottom.Visible = false;
        }
        //Ensure that the responseId that is passed through exists in the database.
        //If it does not exist, display an error message and hide the survey panel.
        else
        {
            List<SurveyOverview> responses = sysmgr.Response_List_All();
            bool responseIdExists = responses.Exists(x => x.ResponseId == responseId);
            if (!responseIdExists)
            {
                MessageUserControlIndividual.ShowInfoError("Error", "The entered Response ID does not exist. Please try again.");
                SurveyInformation.Visible = false;
                BackBottom.Visible = false;
            }
            //Otherwise, populate the survey panel with the corresponding information.
            //Wrapped in a try-catch to be safe.
            else
            {
                try
                {
                    MessageUserControlIndividual.Visible = false;
                    PopulateSurvey(responseId);
                }
                catch
                {
                    MessageUserControlIndividual.Visible = true;
                    MessageUserControlIndividual.ShowInfoError("Error", "Oops. Something went wrong and the survey could not be retrieved. Please go back to the previous page and try again.");
                    SurveyInformation.Visible = false;
                    BackBottom.Visible = false;
                }
            }
        }
    }
    #endregion

    #region SurveyOverviewScreen_ListViewHelperMethods
    /// <summary>
    /// Takes the filters in the ViewState and determines if the selected filters match any existing surveys.
    /// </summary>
    private void SearchFilters()
    {

        //Declare booleans to be used to determine if a user input a certain filter. 
        //This mechanism helps form the filter message that is displayed to the user.
        Boolean siteBool = false;
        Boolean unitBool = false;
        Boolean genderBool = false;
        Boolean ageBool = false;

        //Note: due to the way the MessageUserControl has been programmed, the date input is not determined until the TryRun() is used. Otherwise, the GetDate() method wil throw an exception that is not caught.

        //Find the site that has been selected or is associated with the user.
        List<string> site = SiteFilterList;

        //If the SiteDDL value has been changed, a site filter has been inputted. Set the boolean to true so that the filter message will add a line for site.
        if (SiteDDL.SelectedValue != "0")
        {
            siteBool = true;
        }

        //Find the units that have been selected.
        List<string> units = UnitFilterList;
        //If the UnitCheckboxList has at least one item checked, a unit filter has been inputted. Set the boolean to true so that the filter message will add a line for units.
        if (UnitCheckboxList.SelectedIndex != -1)
        {
            unitBool = true;
        }

        //Find the genders that have been selected.
        List<string> genders = GenderFilterList;
        //If the GenderCheckboxList has at least one item checked, a gender filter has been inputted. Set the boolean to true so that the filter message will add a line for genders.
        if (GenderCheckboxList.SelectedIndex != -1)
        {
            genderBool = true;
        }

        //Find the ages that have been selected.
        List<string> ages = AgeFilterList;
        //If the AgeCheckboxList has at least one item checked, an age filter has been inputted. Set the boolean to true so that the filter message will add a line for ages.
        if (AgeCheckboxList.SelectedIndex != -1)
        {
            ageBool = true;
        }

        //Using the bools and date values, determine if any filters have been inputted and generate a message.
        filterList = GetFilterMessage(FromDate.Text, ToDate.Text, siteBool, site, unitBool, units, genderBool, genders, ageBool, ages);

        int responseCount = sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList).Count();

        //If no filters have been selected, alert the user that they have not selected any filters.
        if (String.IsNullOrEmpty(FromDate.Text) && String.IsNullOrEmpty(ToDate.Text) && !siteBool && !unitBool && !genderBool && !ageBool)
        {
            MessageUserControl.Visible = true;

            if (ViewAllPrivileges())
            {
                MessageUserControl.ShowInfo("Alert", "You did not select any filters. You are viewing all " + responseCount + " surveys from active sites and units.");

            }
            else
            {
                MessageUserControl.ShowInfo("Alert", "You did not select any filters. You are viewing all " + responseCount + " surveys from active units at the " + sysmgr.Get_SiteName(siteId) + " site.");
            }
        }
        //Otherwise, try to update the SurveyOverview based on the selected filters.
        else
        {

            MessageUserControl.TryRun(() =>
            {
                MessageUserControl.Visible = true;

                //Convert date inputs to DateTime? data type. 
                //If the user entered an invalid date time, GetDate() will throw an exception, handled by the MessageUserControl.
                DateTime? startDate = GetDate(FromDate.Text, "'From Date'");
                DateTime? endDate = GetDate(ToDate.Text, "'To Date'");
                //The jQuery calender inputs all the dates at 12:00AM, which will throw off the filter. 
                //If the endDate has a non-null DateTime field, set the date to 11:59PM.
                if (endDate.HasValue)
                {
                    endDate = endDate.Value.AddHours(23).AddMinutes(59);
                }

                StartDateFilter = startDate;
                EndDateFilter = endDate;

                //If the dates entered aren't logical, alert the user.
                if (StartDateFilter > EndDateFilter)
                {
                    throw new Exception("The From Date cannot be more recent than the To Date.");
                }

                //If the From date is in the future, alert the user.
                if (StartDateFilter > DateTime.Today)
                {
                    throw new Exception("The From Date cannot be in the future.");
                }

                //If the To date is in the future, alert the user.
                if (EndDateFilter > DateTime.Today.AddDays(1))
                {
                    throw new Exception("To Date cannot be in the future.");
                }

                //Make sure that at the site (or sites) have at least one survey without any filters.
                //If the site does not have any surveys, the user will be alerted via the Pre_Render() method.
                List<string> surveySite = new List<string>();
                List<string> surveyUnits = new List<string>();
                if (siteId == 0)
                {
                    surveySite = sysmgr.Active_Site_List();
                    surveyUnits = sysmgr.Active_Unit_List();
                }
                else
                {
                    surveySite.Add(sysmgr.Get_SiteName(siteId));
                    surveyUnits = GetCheckboxFilters(UnitCheckboxList);
                }

                //If the site does not have any surveys that match the filter, but has at least one survey attached to the site, alert the user that their filters returned no responses.
                if ((sysmgr.Response_List_Filters(site, units, genders, startDate, endDate, ages)).Count() == 0)
                {
                    if (sysmgr.Response_List_Filters(surveySite, surveyUnits, GetCheckboxFiltersAll(GenderCheckboxList), null, null, GetCheckboxFiltersAll(AgeCheckboxList)).Count() > 0)
                    {
                        string filterListError = "No surveys match the following filters: <br />" + filterList + "<br />To find a match, select less filters or clear them altogether. <br /> Note: If you are a SuperUser and want to select a different site, please clear the filters first.";
                        SurveyOverviewListView.Visible = false;
                        //Prevents a bug where if the DDL changed, the message would disappear.
                        SiteDDL.Enabled = false;
                        throw new Exception(filterListError);
                    }
                }
                else
                {
                    SiteDDL.Enabled = true;
                    SurveyOverviewListView.Visible = true;
                }

            }, "Success. " + responseCount + " surveys were found when the following filters were applied:", filterList);
        }
    }
    #endregion

    #region SurveyOverviewScreen_PageDisplayHelperMethods
    /// <summary>
    /// Controls how the page renders to the user, based on whether the user has a SuperUser role and whether the SiteDDL has been selected.
    /// </summary>
    private void SetupPage()
    {
        //If the page is being loaded for the first time, determine whether the user has a SuperUser role and set-up the page accordingly.
        if (!IsPostBack)
        {

            int responseCount = sysmgr.Response_List_Filters(SiteFilterList, UnitFilterList, GenderFilterList, StartDateFilter, EndDateFilter, AgeFilterList).Count();
            MessageUserControl.Visible = true;

            //If the user has a SuperUser role, set the siteId to 0, which indicates that the dropdown list has not been selected.
            //Update the page to show the SiteDDL but not the UnitCheckbox List.
            if (ViewAllPrivileges())
            {
                UpdateSuperUserSiteUnitLists();
                MessageUserControl.ShowInfo("Note", "No filters have been applied. You are viewing all " + responseCount + " surveys from active sites and units.");
            }
            //If the user does not have a SuperUser role, get their siteId based on their username, and render the appropriate UnitCheckbox List and ResponsesListView.
            //In addition, hide the SiteDDL, since non-SuperUser roles cannot view surveys from sites other than their own and set an appropriate page heading.
            else
            {
                siteId = GetUserSiteId();

                ViewSiteAndUnitInformation(false, true);
                GenerateUnitList(siteId);

                UpdatePageHeading("Surveys", "View and filter all submitted survey responses from active units.");
                MessageUserControl.ShowInfo("Note", "No filters have been applied. You are viewing all " + +responseCount + " surveys from active units at the " + sysmgr.Get_SiteName(siteId) + " site.");
            }
        }
        //Otherwise, the user has entered a value into the page (e.g. clicked a button or selected a site from the SiteDDL).
        else
        {
            //If the user has a SuperUser role, set the SiteId to the SiteDDL value and update the UnitList visibiity.
            if (ViewAllPrivileges())
            {
                siteId = int.Parse(SiteDDL.SelectedValue);
                UpdateSuperUserSiteUnitLists();
                UpdatePageHeading("Surveys", "View and filter all submitted survey responses from active sites and units.");
            }
            //If the user does not have a SuperUser role, hide the SiteDLL and set an appropriate page heading.
            else
            {
                ViewSiteAndUnitInformation(false, true);
                UpdatePageHeading("Surveys", "View and filter all submitted survey responses from active units.");
            }
        }

        //Hide the IndividualResponse screen.
        IndividualResponseScreen.Visible = false;
    }

    /// <summary>
    /// Hides and enables the filter panel's controls and buttons.
    /// </summary>
    /// <param name="buttonShow">Contains a boolean with a true/false value that determines whether the ApplyFilters and ClearFilters buttons should be enabled (true) or disabled (false).</param>
    /// <param name="unitAlertShow">Contains a boolean with a true/false value that indicates whether the "No Active Units" label should be seen (true) or hidden (false).</param>
    /// <param name="siteShow">Contains a boolean with a true/false value that indicates whether the SiteDDL should be seen (true) or hidden (false).</param>
    /// <param name="unitShow">Contains a boolean with a true/false value that indicates whether the UnitCheckboxList should be seen (true) or hidden (false).</param>
    private void DisableButtons(bool buttonShow, bool unitAlertShow, bool siteShow, bool unitShow)
    {
        ApplyFiltersButton.Enabled = buttonShow;
        ClearFiltersButton.Enabled = buttonShow;
        ViewSiteAndUnitInformation(siteShow, unitShow);
        UnitAlert.Visible = unitAlertShow;
    }

    /// <summary>
    /// Hides the SiteDDL control and/or the UnitCheckboxList control based on the inputted parameters.
    /// </summary>
    /// <param name="siteVisible">Contains a boolean with a true/false value that indicates if the SiteDLL control should be visible (true) or hidden (false).</param>
    /// <param name="unitVisible">Contains a boolean with a true/false value that indicates if the UnitCheckbox List control should be visible (true) or hidden (false).</param>
    private void ViewSiteAndUnitInformation(bool siteVisible, bool unitVisible)
    {
        if (siteVisible)
        {
            SiteHide.Visible = true;
        }
        else
        {
            SiteHide.Visible = false;
        }

        if (unitVisible)
        {
            UnitHide.Visible = true;
            UnitAlert.Visible = false;
        }
        else
        {
            UnitHide.Visible = false;
            UnitAlert.Visible = false;
        }
    }

    /// <summary>
    /// Determines the user's role and produces a true/false values that indicates whether the user is a SuperUser.
    /// </summary>
    /// <returns>A true/false value that indicates whether the user has a SuperUser role (true) or not (false).</returns>
    private bool ViewAllPrivileges()
    {
        bool isSuperUser = false;
        if (User.IsInRole(SecurityRoles.SuperUser))
        {
            isSuperUser = true;
        }
        return isSuperUser;
    }

    /// <summary>
    /// Determines if the user with a SuperUser role has selected a site from the SiteDDL and displays the page accordingly.
    /// </summary>
    /// <remarks>
    /// If a site has been selected, the SiteDLL and UnitCheckboxList are visible. If a site has not been selected, the SiteDDL is visible but the UnitCheckboxList is not.
    /// </remarks>
    private void UpdateSuperUserSiteUnitLists()
    {
        //If a site has been selected, show both the SiteDDL and the UnitCheckboxList.
        if (siteId != UNSELECTED_SITE)
        {
            ViewSiteAndUnitInformation(true, true);
        }
        //Otherwise, show the SiteDDL and hide the UnitCheckboxList.
        else
        {
            ViewSiteAndUnitInformation(true, false);
        }
    }

    /// <summary>
    /// Uses AspNet.Identity to find the user's associated SiteId.
    /// </summary>
    /// <returns>An integer equal to the SiteId attached to UserProfile.</returns>
    private int GetUserSiteId()
    {
        UserManager userManager = new UserManager();
        int userSiteId = userManager.GetUserSiteId(User.Identity.GetUserName());
        return userSiteId;
    }
    #endregion

    #region IndividualResponseScreen_Buttons
    /// <summary>
    /// Hides the IndividualResponse screen and shows the SurveyOverview screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Back_Click(object sender, EventArgs e)
    {
        IndividualResponseScreen.Visible = false;
        SurveysListViewScreen.Visible = true;
        MessageUserControl.Visible = true;

        SetupPage();
    }
    #endregion

    #region IndividualResponseScreen_HelperMethods
    /// <summary>
    /// Retrieves and displays all the information associated with one Response (via the SurveyOverview POCO and LINQ queries), based on the inputted ResponseId.
    /// </summary>
    /// <param name="responseId">Contains an integer with the ResponseId of the selected Response.</param>
    private void PopulateSurvey(int responseId)
    {
        //Populate the survey with questions from the database.
        Question1Label.Text = "1) " + sysmgr.Get_Survey_Question(1);
        Question1ALabel.Text = "A) " + sysmgr.Get_Survey_SubQuestion(1);
        Question1BLabel.Text = "B) " + sysmgr.Get_Survey_SubQuestion(2);
        Question1CLabel.Text = "C) " + sysmgr.Get_Survey_SubQuestion(3);
        Question1DLabel.Text = "D) " + sysmgr.Get_Survey_SubQuestion(4);
        Question1ELabel.Text = "E) " + sysmgr.Get_Survey_SubQuestion(5);
        Question2Label.Text = "2) " + sysmgr.Get_Survey_Question(6);
        Question3Label.Text = "3) " + sysmgr.Get_Survey_Question(7);
        Question4Label.Text = "4) " + sysmgr.Get_Survey_Question(8);
        Question5Label.Text = "5) " + sysmgr.Get_Survey_Question(9);


        ////Retrieve the selected survey based on the inputted responseId.
        SurveyOverview survey = sysmgr.Response_List_Individual(responseId);

        ////Populate fields in the "Survey Information" panel.
        Location.Text = survey.SiteName + " - " + survey.UnitName;
        Date.Text = survey.Date.ToString("MMMM dd, yyyy");

        ////Populate fields in the "Client Profile" panel.
        Gender.Text = survey.Gender;
        Age.Text = survey.Age;

        //Populate survey with answers.
        Question1AResponse.Text = sysmgr.Get_Survey_Answer(responseId, 1);
        Question1BResponse.Text = sysmgr.Get_Survey_Answer(responseId, 2);
        Question1CResponse.Text = sysmgr.Get_Survey_Answer(responseId, 3);
        Question1DResponse.Text = sysmgr.Get_Survey_Answer(responseId, 4);
        Question1EResponse.Text = sysmgr.Get_Survey_Answer(responseId, 5);
        Question2Response.Text = sysmgr.Get_Survey_Answer(responseId, 6);
        Question3Response.Text = sysmgr.Get_Survey_Answer(responseId, 7);
        Question4Response.Text = sysmgr.Get_Survey_Answer(responseId, 8);
        Question5Response.Text = survey.Comment;

        //If there was no submitted answer, italicize the "No Response" so that it is easier to distinguish from entered responses.
        SetItalicsNoResponse(Question1AResponse.Text, Question1AResponse);
        SetItalicsNoResponse(Question1BResponse.Text, Question1BResponse);
        SetItalicsNoResponse(Question1CResponse.Text, Question1CResponse);
        SetItalicsNoResponse(Question1DResponse.Text, Question1DResponse);
        SetItalicsNoResponse(Question1EResponse.Text, Question1EResponse);
        SetItalicsNoResponse(Question2Response.Text, Question2Response);
        SetItalicsNoResponse(Question3Response.Text, Question3Response);
        SetItalicsNoResponse(Question4Response.Text, Question4Response);
        SetItalicsNoResponse(Question5Response.Text, Question5Response);
    }

    /// <summary>
    /// Sets and unsets a label's italic font property.
    /// </summary>
    /// <param name="response">Contains a string with the text to be parsed.</param>
    /// <param name="labelControl">Contains a string with the name of the control to be set/unset.</param>
    private void SetItalicsNoResponse(string response, Label labelControl)
    {
        const string NO_RESPONSE = "No Response";
        //If the inputted string is "No Response," italicize the label.
        if (response.Equals(NO_RESPONSE))
        {
            labelControl.Font.Italic = true;
        }
        //Otherwise, un-italicize the label.
        //This step is important when users are viewing many individual surveys in the same session.
        else
        {
            labelControl.Font.Italic = false;
        }
    }

    #endregion
}