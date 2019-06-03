using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace MSSSystem.BLL
{
    /// <summary>
    /// Handles the actions related to creating the database from the script file.
    /// </summary>
    public class DatabaseCreation
    {
        Security.UserManager userManager = new Security.UserManager();

        private string logFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Scripts/DB_log.txt");
        private string connString = ConfigurationManager.ConnectionStrings["Master"].ToString();
        
        /// <summary>
        /// Fills the DB with the script data by splitting the script on GO commands.
        /// Prints out to db_log.txt with error messages
        /// </summary>
        /// <param name="pathStoreProceduresFile">Path to db script file</param>
        public void runSqlScriptFile(string pathStoreProceduresFile)
        {
            try
            {
                string script = File.ReadAllText(pathStoreProceduresFile);

                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                          RegexOptions.Multiline | RegexOptions.IgnoreCase);
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    foreach (string commandString in commandStrings)
                    {
                        if (commandString.Trim() != "")
                        {
                            using (var command = new SqlCommand(commandString, connection))
                            {
                                try
                                {
                                    command.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    string spError = commandString.Length > 100 ? commandString.Substring(0, 100) + " ...\n..." : commandString;
                                    File.AppendAllText(logFilePath, string.Format("Please check the SqlServer script.\nFile: {0} \nLine: {1} \nError: {2} \nSQL Command: \n{3}", pathStoreProceduresFile, ex.LineNumber, ex.Message, spError));                                    
                                }
                            }
                        }
                    }
                    File.AppendAllText(logFilePath, "SQL Script import concluded.");
                }
            }
            catch (Exception e)
            {
                File.AppendAllText(logFilePath, e.Message);
            }


            //userManager.AddTestUsers();
        }


        /// <summary>
        /// Using the questions table to query if the database has been loaded with script data yet.
        /// </summary>
        /// <returns>bool based on if the db is filled or not</returns>
        public bool isDBFilled()
        {
            using (DAL.MSSContext context = new DAL.MSSContext())
            {
                return context.Questions.Count() > 0 ? true : false;
                
            }
        }
    }
}
