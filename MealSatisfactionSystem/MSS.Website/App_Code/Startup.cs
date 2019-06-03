using Hangfire;
using Microsoft.Owin;
using MSSSystem.BLL;
using MSSSystem.BLL.Security;
using Owin;
using System;
using System.Configuration;
using System.Data.SqlClient;

[assembly: OwinStartupAttribute(typeof(MSS.Website.Startup))]
namespace MSS.Website
{
    /// <summary>
    /// Runs on project startup to initialize the application.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Runs on startup and initializes HangFire as well as beginning the task to automatically update the Passcode at midnight.
        /// </summary>
        /// <param name="app">Used by the system to build the webapp.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // This code generates a skeleton database for Hangfire to create its tables in if one does not already exist.
            String str; 
            String connectionString = ConfigurationManager.ConnectionStrings["Master"].ToString();
            SqlConnection myConn = new SqlConnection(connectionString);
            str = "IF NOT EXISTS(select * from sys.databases where name='Hangfire') " + "CREATE DATABASE Hangfire";
            SqlCommand myCommand = new SqlCommand(str, myConn);            
            myConn.Open();
            myCommand.ExecuteNonQuery();       
            myConn.Close();
             
            // This initializes Hangfire and connects the database, dashboard and server. 
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Hangfire");
            // Comment out this line to remove the dashboard
            // app.UseHangfireDashboard();
            app.UseHangfireServer();

            //// This sets the daily task of updating passcodes for sites at midnight.
            SiteController sysmgr = new SiteController();
            RecurringJob.AddOrUpdate(() => sysmgr.Site_ChangePasscode(), Cron.Daily, TimeZoneInfo.Local);            
        }
    }
}
