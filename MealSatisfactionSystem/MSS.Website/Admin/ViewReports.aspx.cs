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
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.AspNet.Identity;
using MSSSystem.BLL.Security;

public partial class Admin_ViewReports : Page
{
    QuestionController questionController = new QuestionController();
    UnitController unitController = new UnitController();
    SiteController siteController = new SiteController();
    UserManager userManager = new UserManager();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Account/Login.aspx");
        } else
        {
            GenerateQuestionParameterFields();
            if(Page.IsPostBack)
                GenerateUnitsFields();
        }
    }

    /// <summary>
    /// Adds checkboxes of Question parameters to the webpage
    /// </summary>
    private void GenerateQuestionParameterFields()
    {
        List<Question> QuestionList = questionController.GetQuestionParameterList();
        
        foreach(Question item in QuestionList)
        {
            if (item.QuestionParameter != "Comments")
            {
                questionParameterPlaceHolder.Controls.Add(new CheckBox { ID = item.QuestionParameter, Text = item.QuestionParameter });
            }
        }
    }


    /// <summary>
    /// Adds checkboxes of units to the webpage
    /// </summary>
    private void GenerateUnitsFields()
    {
        int Site = userManager.GetUserSiteId(User.Identity.Name);
        List<MSS.Data.Entities.Unit> UnitList = new List< MSS.Data.Entities.Unit > ();
        if (Site != 1)
        {
            UnitList = unitController.Unit_Search(false, Site, "", "");
        }else
        {
            int siteFromDD = int.Parse(SiteDD.SelectedValue);
            UnitList = unitController.Unit_Search(false, siteFromDD, "", "");
        }
        for (int i = 0; i < UnitList.Count(); i++)
        {
            unitPlaceholder.Controls.Add(new CheckBox { ID = UnitList[i].UnitId.ToString(), Text = UnitList[i].UnitName });
            if (i == 1)
            {
                unitPlaceholder.Controls.Add(new LiteralControl("<br />"));
            }
            else if ((i + 2) % 3 == 0 && i > 1)
            {
                unitPlaceholder.Controls.Add(new LiteralControl("<br />"));

            }
        }
    }

    // Need siteDD to be set before the units are generated
    protected void SiteDD_DataBound(object sender, EventArgs e)
    {
        SiteDD.Items[0].Selected = true;
        GenerateUnitsFields();
    }

    /// <summary>
    /// Webservice for the JS charts. Recieves JSON string matching the params.    
    /// </summary>
    /// <param name="Parameters" type="string">Contains a single parameter being queried</param>
    /// <param name="FromDate" type="datetime">Contains the start date to filter</param>
    /// <param name="ToDate" type="datetime">Contains the end date to filter</param>
    /// <param name="Units" type="List<string>">Contains a list of units to filter by</param>
    /// <param name="Genders" type="List<string>"> Contains a list of genders to filter by</param>
    /// <param name="Ages" type="List<string>">Contains a list of ages to filter by</param>
    /// <returns>JSON string of required data</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static ChartingResponse ChartDataRequest(string Parameters, DateTime FromDate, DateTime ToDate, List<string> Units, List<string> Genders, List<string> Ages, int Site)
    {        
        ChartingResponseController chartingResponseController = new ChartingResponseController();
        UserManager userManager = new UserManager();

        if(Site == -1)
        {
            Page page = new Page();
            Site = userManager.GetUserSiteId(page.User.Identity.Name);
        }

        ChartingResponse response = chartingResponseController.GetChartingResponseData( Parameters, FromDate, ToDate, Units, Genders, Ages, Site);
        return response;
    }
    
    /// <summary>
    /// Facilitates the dynamic creation of lookup tables for charting colours and site names.    
    /// </summary>
    /// <param name="type" type="string">Contains the type of lookup table being queried</param>
    /// <returns>JSON of returned data</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<LookupValues> LookupRequest(string type)
    {
        LookupController lookupController = new LookupController();

        List<LookupValues> resilts = lookupController.getValues(type);
        if (resilts.Count < 1)
        {
            //throw new Exception("Database error retrieving the lookup table for " + type);
            resilts.Add(new LookupValues()
            {
                Id = -1,
                Description = "Database error retrieving the lookup table for " + type
            });
            return resilts;
        }
        else
        {
            return resilts;

        }        
    }

    /// <summary>
    /// Queries for response data matching the filters passed in.  
    /// </summary>
    /// <returns> ChartingHomeResponse with information associated to questions, answers, values, units, and colours.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<ChartingHomeResponse> PercentileDataRequest(int siteId, DateTime fromDate, DateTime toDate)
    {
        HomePageController homePageControl = new HomePageController();
        UserManager userManager = new UserManager();
    
        if (siteId == -1)
        {
            Page page = new Page();
            siteId = userManager.GetUserSiteId(page.User.Identity.Name);
        }
        var results = homePageControl.grabData(siteId, fromDate, toDate);
        return results;
    }
}