using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using MSS.Data.Entities;
using System.ComponentModel;
using MSSSystem.DAL;
using System.IO;
#endregion

/// <summary>
/// Holds any business logic classes as well as classes that handle website calls to our entities.
/// </summary>
namespace MSSSystem.BLL
{
    /// <summary>
    /// Allows the webpage to access the Site entity.
    /// </summary>
    [DataObject]
    public class SiteController
    {
        #region AddDefaultSite
        /// <summary>
        /// Adds a default site to the database on initial project deployment, the default SuperUser account will be attached to this site.
        /// </summary>
        public void Site_AddDefault()
        {
            using (MSSContext context = new MSSContext())
            {
                if (!context.Sites.Any(u => u.SiteName.Equals("Administrator Site")))
                {
                    // Making the test site object and setting all of the site's fields to their respective values
                    Site site = new Site()
                    {
                        Description = "Default Administrator site",
                        SiteName = "Administrator Site",
                        Passcode = "Sunshine",
                        Disabled = false
                    };
                    // Adding the site to the database
                    context.Sites.Add(site);
                    // Committing the transaction
                    context.SaveChanges();
                }
            }
        }
        #endregion

        #region SiteListMethods
        /// <summary>
        /// Searches for all the Sites based on the deactivated parameter.
        /// </summary>
        /// <param name="deactivated">Contains the status of the desired sites; true for deactivated sites, false for active sites.</param>
        /// <returns>A list of Sites.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Site> Site_List(bool deactivated)
        {
            using (MSSContext context = new MSSContext())
            {
                var results =
                   (from item in context.Sites
                    where item.Disabled == deactivated
                    orderby item.SiteName ascending
                    select item);

                return results.ToList();
            }
        }

        /// <summary>
        /// Looks for the site whoose ID matches the ID provided.
        /// </summary>
        /// <param name="siteId">Contains the site ID of the site that the user is looking for</param>
        /// <returns>A Site object matching the siteId</returns>
        public Site Site_FindById(int siteId)
        {
            using (MSSContext context = new MSSContext())
            {
                return context.Sites.Find(siteId);
            }
        }

        /// <summary>
        /// Searches for all the Sites based on the deactivated parameter as well as a user defined search parameter and search fields.
        /// </summary>
        /// <param name="deactivated">Contains the status of the desired sites; true for deactivated sites, false for active sites.</param>
        /// <param name="searchArg">Contains the term the user is searching for.</param>
        /// <param name="searchBy">Contains the value of the fields the user wishes to search against.</param>
        /// <returns>A list of Sites.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Site> Site_List(bool deactivated, string searchArg, string searchBy)
        {
            using (MSSContext context = new MSSContext())
            {
                IQueryable<Site> results = null;
                if (searchBy == "Site Name")
                {
                    results =
                      (from item in context.Sites
                       where item.Disabled == deactivated && item.SiteName.Contains(searchArg)
                       orderby item.SiteName ascending
                       select item);
                }
                else if (searchBy == "Description")
                {
                    results =
                      (from item in context.Sites
                       where item.Disabled == deactivated && item.Description.Contains(searchArg)
                       orderby item.SiteName ascending
                       select item);
                }
                else if (searchBy == "All")
                {
                    results =
                      (from item in context.Sites
                       where item.Disabled == deactivated && (item.Description.Contains(searchArg) || item.SiteName.Contains(searchArg))
                       orderby item.SiteName ascending
                       select item);
                }
                return results.ToList();
            }
        }

        /// <summary>
        /// Searches for all sites, regardless of if they are deactivated.
        /// </summary>
        /// <returns>A list of all Sites.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Site> Site_List_All()
        {
            using (MSSContext context = new MSSContext())
            {
                var results = (from x in context.Sites
                               orderby x.SiteName ascending
                               select x);

                return results.ToList();
            }
        }
        #endregion

        #region CRUDMethods
        /// <summary>
        /// Adds a new site to the system.
        /// </summary>
        /// <param name="siteName">Contains the user entered name for the new site.</param>
        /// <param name="description">Contains the user entered description of the new site</param>
        /// <param name="passcode">Contains the user entered passcode of the new site.</param>
        /// <returns>A SiteId of the newly added Site.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int Site_Add(string siteName, string description, string passcode)
        {
            using (MSSContext context = new MSSContext())
            {
                // Grabs objects from the database matching the user defined site name and password for uniqueness validation.
                var nameExists = (from x in context.Sites
                                  where x.SiteName.Equals(siteName) && x.Disabled == false
                                  select x).FirstOrDefault();
                var codeExists = (from x in context.Sites
                                  where x.Passcode.Equals(passcode) && x.Disabled == false
                                  select x).FirstOrDefault();

                // If the above objects exist throws an error for violating the uniqueness business rule.
                if (nameExists != null && codeExists != null)
                {
                    throw new Exception("SiteName and Passcode must be unique across all sites.");
                }
                if (nameExists != null)
                {
                    throw new Exception("SiteName must be unique across all sites.");
                }
                if (codeExists != null)
                {
                    throw new Exception("Passcode must be unique across all sites.");
                }

                // Creates a new site and gives it the values entered by the user, then saves it to the database.
                Site site = new Site();
                site.SiteName = siteName;
                site.Description = description;
                site.Passcode = passcode;
                site = context.Sites.Add(site);
                context.SaveChanges();
                return site.SiteId;
            }
        }

        /// <summary>
        /// Updates a site in the database.
        /// </summary>
        /// <param name="siteId">Contains the siteId of the unit that was selected for the update.</param>
        /// <param name="siteName">Contains the user updated name for the site.</param>
        /// <param name="description">Contains the user updated description for the site.</param>
        /// <param name="passcode">Contains the user updated passcode for the site.</param>     
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Site_Update(int siteId, string siteName, string description, string passcode)
        {
            using (MSSContext context = new MSSContext())
            {
                // Grabs the current site object the user wishes to update.
                var site = (from x in context.Sites
                            where x.SiteId == siteId
                            select x).FirstOrDefault();

                // Grabs objects from the database, other than the above site object, 
                // that match the user's newly submitted site name and passcode for uniqueness validation.
                var nameExists = (from x in context.Sites
                                  where x.SiteName.Equals(siteName) && x.SiteId != siteId && x.Disabled == false
                                  select x).FirstOrDefault();
                var codeExists = (from x in context.Sites
                                  where x.Passcode.Equals(passcode) && x.SiteId != siteId && x.Disabled == false
                                  select x).FirstOrDefault();

                //Administrator Site name must not be changed or the system will begin adding new admin sites.
                if(site.SiteName == "Administrator Site" && siteName != "Administrator Site")
                {
                    throw new Exception("Administrator Site name cannot be changed.");
                }
                // If the above "Exists" objects exist throws an error for violating the uniqueness business rule.
                if (nameExists != null && codeExists != null)
                {
                    throw new Exception("SiteName and Passcode must be unique across all active sites.");
                }
                if (nameExists != null)
                {
                    throw new Exception("SiteName must be unique across all active sites.");
                }
                if (codeExists != null)
                {
                    throw new Exception("Passcode must be unique across all active sites.");
                }
                // If the site has been disabled since the last databind throws an error.
                if (site.Disabled)
                {
                    throw new Exception("Site has been deactivated, it can no longer be changed.");
                }
                // If the user makes no changes throws and error.
                if (site.SiteName == siteName && site.Description == description && site.Passcode == passcode)
                {
                    throw new Exception("Update failed, no properties were changed.");
                }

                // Modifies the existing sites to the user's new changes and saves them to the database.
                site.SiteName = siteName;
                site.Description = description;
                site.Passcode = passcode;
                context.Entry(site).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Sets the desired site's Disabled field to 1(True).
        /// </summary>
        /// <param name="siteId">Contains the siteId of the site being deactivated.</param>  
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Site_Deactivate(int siteId)
        {
            using (MSSContext context = new MSSContext())
            {
                Site site = context.Sites.Find(siteId);
                //Prevent the admin site from being deactivated.
                if (site.SiteName == "Administrator Site")
                {
                    throw new Exception("Administrator Site name cannot be deactivated.");
                }
                // Grabs a list of units attached to the site being decativated.
                var units = (from x in context.Units
                             where x.SiteId == siteId
                             select x).ToList();

                // Instantiates the UnitController and calls its deactivate method for each unit attached to the deactivated site.
                UnitController sysmgr = new UnitController();
                foreach (var item in units)
                {
                    sysmgr.Unit_Deactivate(item.UnitId);
                }
                 // Grabs the site to be deactivated and changes its Disabled field value to true then saves it to the database.
                
                site.Disabled = true;
                context.Entry(site).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        #endregion 

        #region PasscodeGenerationMethods
        /// <summary>
        ///  Gets a randomly generated word from the file nounlist.txt in the website root folder for use as a passcode.
        /// </summary>
        /// <returns>A randomly generated passcode.</returns>
        public string getPasscode()
        {
            string result = "";
            // Sets the file path for the file holding our passcodes.
            // This file path is (YourDirectoryHere)MealSatisfactionSystem\MealSatisfactionSystem\MSS.Website\nounlist.txt if you need to find the list file
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\nounlist.txt";
            // Grabbing a list of active sites for password file length validation.
            List<Site> sites = Site_List(false);
            StreamReader fileIn = null;
            try
            {
                // Goes through the nounlist line by line and adds each passcode to a list.
                fileIn = new StreamReader(fileName);
                string input;
                List<string> nouns = new List<string>();
                while (null != (input = fileIn.ReadLine()))
                {
                    nouns.Add(input);
                }

                // Throws an error if the nounlist is shorter than the list of active sites.
                if (sites.Count + 1 >= nouns.Count)
                {
                    throw new Exception("You have more sites than passcodes, consider adding new passcodes.");
                }

                // Grabs a random item from the list and assigns it to the result.
                Random random = new Random();
                int index = random.Next(nouns.Count);
                result = nouns.ElementAt(index);
            }
            catch (IOException ioe)
            {
                throw new Exception("Failed to read file at " + fileName + " error " + ioe);
            }
            // Closes the StreamReader when it gets to end of file.
            if (fileIn != null)
            {
                fileIn.Close();
                fileIn.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Gets a single passcode.
        /// </summary>
        /// <returns>A single passcode string.</returns>
        public string getSinglePasscode(int SiteID)
        {
            string result = "";
            // Sets the file path for the file holding our passcodes.
            // This file path is (Your DirectoryHere)MealSatisfactionSystem\MealSatisfactionSystem\MSS.Website\nounlist.txt if you need to find the list file
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\nounlist.txt";

            StreamReader fin = null;
            try
            {
                // Goes through the nounlist line by line and adds each passcode to a list.
                fin = new StreamReader(fileName);
                string input;
                List<string> nouns = new List<string>();
                while (null != (input = fin.ReadLine()))
                {
                    nouns.Add(input);
                }
               
                // Ensures the nounlist never goes below 2500 words
                if (nouns.Count < 2500)
                {
                    throw new Exception("Lower limit of passcodes reached. No more passcodes can be removed. Contact your SiteMaster if the passcode needs to be removed.");
                }

                Random random = new Random();
                int index = random.Next(nouns.Count);

                result = nouns.ElementAt(index);
            }
            catch (IOException ioe)
            {
                throw new Exception("Failed to read file at " + fileName + " error " + ioe);
            }

            if (fin != null)
            {
                fin.Close();
                fin.Dispose();
            }

            // Check if unique
            bool unique = false;

            while (!unique)
            {
                using (var context = new MSSContext())
                {
                    var isUnique = (from s in context.Sites
                                    where s.Passcode == result && s.Disabled == false
                                    select s).ToList();

                    unique = isUnique.Count() >= 1 ? false : true;
                }
            }
            return result;
        }

        /// <summary>
        /// Changes the passcode at every enabled site.
        /// This method is run automatically at midngiht by HangFire.
        /// (See startup.cs in MSS.Website/App_code)
        /// </summary>        
        public void Site_ChangePasscode()
        {
            string passcode;
            bool unique = false;
            using (MSSContext context = new MSSContext())
            {
                List<Site> siteList = Site_List(false);

                // Increments through the list of active sites and gives them a new, unique passcode.
                foreach (Site site in siteList)
                {
                    passcode = getPasscode();
                    // Checks each site to ensure the new passcode does not match any other site's passcode.
                    while (!unique)
                    {
                        var exists = (from x in context.Sites
                                      where x.Passcode == passcode && x.Disabled == false
                                      select x).FirstOrDefault();

                        if (exists != null)
                        {
                            passcode = getPasscode();
                        }
                        else
                        {
                            unique = true;
                            site.Passcode = passcode;
                            context.Entry(site).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    unique = false;
                }
            }
        }

        /// <summary>
        /// Updates a single site in the DB with a new passcode, checks for unique
        /// </summary>
        public void Site_ChangeSinglePasscode(int siteID, string passcode)
        {
            using (MSSContext context = new MSSContext())
            {
                // Grabs the site where the passcode is being changed.
                Site site = (from x in context.Sites
                             where x.SiteId == siteID
                             select x).FirstOrDefault();

                // Changes that sites passcode to the new passcode.
                site.Passcode = passcode;
                context.Entry(site).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        #endregion

        #region SurveyPasscodeMethods
        /// <summary>
        /// Obtains a list of current passcodes for existing sites.
        /// </summary>
        /// <returns>A list of passcodes for existing sites.</returns>
        public List<string> Site_PasscodeList()
        {
            using (MSSContext context = new MSSContext())
            {
                var results = from x in context.Sites
                              where x.Disabled == false
                              select x.Passcode;
                return results.ToList();
            }
        }

        /// <summary>
        /// Obtains the Siteid that matches a user entered passcode.
        /// </summary>
        /// <param name="passcode">Contains the site passcode</param>
        /// <returns>A SiteId from the site matching the passcode.</returns>
        public int GetIdFromPasscode(string passcode)
        {
            using (MSSContext context = new MSSContext())
            {
                var result = (from x in context.Sites
                              where x.Passcode == passcode &&
                              x.Disabled == false
                              select x.SiteId).FirstOrDefault();
                return result;
            }
        }
        #endregion
    }
}
