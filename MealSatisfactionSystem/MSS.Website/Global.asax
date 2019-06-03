<%@ Application Language="C#" %>
<%@ Import Namespace="MSS.Website" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="MSSSystem.BLL.Security" %>
<%@ Import Namespace="MSSSystem.BLL" %>


<script RunAt="server">
    /// <summary>
    /// Handles some start up functionality for the site, including database and user setup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);

        // Creates the database if it does not exist
        MSSSystem.DAL.MSSContext context = new MSSSystem.DAL.MSSContext();
        context.Database.CreateIfNotExists();

        // Adding the test admin site to the database
        SiteController sysmgr = new SiteController();
        sysmgr.Site_AddDefault();

        // Adding all of the different roles to our system
        RoleManager RoleManager = new RoleManager();
        RoleManager.AddDefaultRoles();

        DatabaseCreation dbCreation = new DatabaseCreation();
        if (!dbCreation.isDBFilled())
        {
            //dbCreation.runSqlScriptFile(Server.MapPath("~/Scripts/dbscript_pres.sql"));
            dbCreation.runSqlScriptFile(Server.MapPath("~/Scripts/dbscript.sql"));            
        }       

        // Adding a webmaster (Superuser) to our system
        var UserManager = new MSSSystem.BLL.Security.UserManager();
        UserManager.AddWebMaster();
    }

</script>
