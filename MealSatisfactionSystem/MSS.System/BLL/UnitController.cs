using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using MSS.Data.Entities;
using System.ComponentModel;
using MSSSystem.DAL;
#endregion

namespace MSSSystem.BLL
{
    /// <summary>
    /// This class will control all the methods that lookup or modify units
    /// </summary>
    [DataObject]
    public class UnitController
    {
        #region UnitListMethods
        /// <summary>
        /// Unit_List will list all the units fitting the parameters entered.
        /// </summary>
        /// <param name="deactivated">True to show deactivated sites, False to show Active sites</param>
        /// <returns>Returns a list of units</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Unit> Unit_List(bool deactivated)
        {
            using (MSSContext context = new MSSContext())
            {
                var results =
                   (from item in context.Units
                    where item.Disabled == deactivated
                    orderby item.Site.SiteName
                    select item);
                return results.ToList();
            }
        }

        /// <summary>
        /// Unit_Search will search for any unit fitting the parameters entered.
        /// </summary>
        /// <param name="deactivated">True to show deactivated sites, False to show Active sites</param>
        /// <param name="searchArg">The search arguments being passed in to look for</param>
        /// <param name="searchBy">What to search for, IE: Description, SiteName, UnitName</param>
        /// <returns>Returns a list of units</returns>
        public List<Unit> Unit_Search(bool deactivated, string searchArg, string searchBy)
        {
            using (MSSContext context = new MSSContext())
            {
                if (searchBy == "SiteName")
                {
                    var results =
                       (from item in context.Units
                        where item.Disabled == deactivated &&
                            item.Site.SiteName.Contains(searchArg)
                        orderby item.Site.SiteName
                        select item);
                    return results.ToList();
                }
                else if (searchBy == "UnitName")
                {
                    var results =
                       (from item in context.Units
                        where item.Disabled == deactivated &&
                            item.UnitName.Contains(searchArg)
                        orderby item.Site.SiteName
                        select item);
                    return results.ToList();
                }
                else if (searchBy == "Description")
                {
                    var results =
                       (from item in context.Units
                        where item.Disabled == deactivated &&
                            item.Description.Contains(searchArg)
                        orderby item.Site.SiteName
                        select item);
                    return results.ToList();
                }
                else // searchBy=="All"
                {
                    var results =
                        (from item in context.Units
                         where item.Disabled == deactivated &&
                         (item.UnitName.Contains(searchArg) ||
                          item.Site.SiteName.Contains(searchArg) ||
                          item.Description.Contains(searchArg))
                         orderby item.UnitName
                         select item);
                    return results.ToList();
                }
            }
        }

        /// <summary>
        /// Unit_Search will search for any unit fitting the parameters entered.
        /// </summary>
        /// <param name="deactivated">True to show deactivated sites, False to show Active sites</param>
        /// <param name="siteId">SiteId of the current user looking for a list of units</param>
        /// <param name="searchArg">The search arguments being passed in to look for</param>
        /// <param name="searchBy">What to search for, IE: Description, SiteName, UnitName</param>
        /// <returns>Returns a list of units</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Unit> Unit_Search(bool deactivated, int siteId, string searchArg, string searchBy)
        {
            // SiteId will be 0 if the current user is a SuperUser, then they will get all the units for all the sites.
            if (siteId == 0)
            {
                // No search argument, display all.
                if (searchArg == null || searchArg == "")
                {
                    return Unit_List(deactivated);
                }
                // Search argument here, display search
                else
                {
                    return Unit_Search(deactivated, searchArg, searchBy);
                }
            }
            // If they arent a SuperUser, the current user will get only the units for their site.
            else
            {
                using (MSSContext context = new MSSContext())
                {
                    if (searchBy == "SiteName")
                    {
                        var results =
                           (from item in context.Units
                            where item.Disabled == deactivated &&
                                item.SiteId == siteId &&
                                item.Site.SiteName.Contains(searchArg)
                            orderby item.UnitName
                            select item);
                        return results.ToList();
                    }
                    else if (searchBy == "UnitName")
                    {
                        var results =
                           (from item in context.Units
                            where item.Disabled == deactivated &&
                                item.SiteId == siteId &&
                                item.UnitName.Contains(searchArg)
                            orderby item.UnitName
                            select item);
                        return results.ToList();
                    }
                    else if (searchBy == "Description")
                    {
                        var results =
                           (from item in context.Units
                            where item.Disabled == deactivated &&
                                item.SiteId == siteId &&
                                item.Description.Contains(searchArg)
                            orderby item.UnitName
                            select item);
                        return results.ToList();
                    }
                    else // searchBy=="All"
                    {
                        var results =
                            (from item in context.Units
                             where item.Disabled == deactivated &&
                             item.SiteId == siteId &&
                             (item.UnitName.Contains(searchArg) ||
                              item.Site.SiteName.Contains(searchArg) ||
                              item.Description.Contains(searchArg))
                             orderby item.UnitName
                             select item);
                        return results.ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Method to look for active units with the same siteId
        /// </summary>
        /// <param name="siteId">Unique identifer for specific site</param>
        /// <returns>A list of units matching the siteId</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Unit> SiteUnitList(int siteId)
        {
            using (MSSContext context = new MSSContext())
            {
                var results = from x in context.Units
                              where x.SiteId == siteId && x.Disabled == false
                              select x;
                return results.ToList();
            }
        }
        #endregion

        #region CRUDMethods
        /// <summary>
        /// Unit_Add will add a new unit to the database
        /// </summary>
        /// <param name="item">Item is a Unit Entity. The fields not required are UnitId and deactivated.</param>
        /// <param name="userSiteId">The siteId of the user attempting to add a new unit. This much match the Unit's siteId</param>
        /// <returns>Returns the UnitId of the newly added Unit.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int Unit_Add(Unit item, int userSiteId)
        {
            using (MSSContext context = new MSSContext())
            {
                if (userSiteId == item.SiteId || userSiteId == 0)
                {
                    Site site = (from x in context.Sites
                                 where x.SiteId == item.SiteId
                                 select x).FirstOrDefault();

                    if (site != null)
                    {
                        if (site.Disabled != true)
                        {
                            item = context.Units.Add(item);
                            context.SaveChanges();
                            return item.UnitId;
                        }
                        else
                        {
                            throw new Exception("The Site you are trying to add this Unit to is Disabled. Someone may have disabled the Site since you started adding the unit. Site Name: " + site.SiteName +
                                ".\rPlease exit and re-enter the page to fix your list of Sites.");
                        }
                    }
                    else
                    {
                        throw new Exception("The Unit's Site is coming up null, please contact an Administrator to check the database for siteId:" + item.SiteId + ".");
                    }
                }
                else
                {
                    throw new Exception("Can not add a unit to a site you do not work at. \r Please ask a SuperUser or another Administrator from the correct site.");
                }
            }
        }

        /// <summary>
        /// Unit_Add will add a new unit to the database
        /// </summary>
        /// <param name="siteId">The siteId of the new Unit</param>
        /// <param name="unitName">The name of the new Unit</param>
        /// <param name="description">The description of the new unit</param>
        /// <param name="userSiteId">The siteId of the user attempting to add a new unit. This much match the Unit's siteId</param>
        /// <returns>Returns the UnitId of the newly added Unit.</returns>
        public int Unit_Add(int siteId, string unitName, string description, int userSiteId)
        {
            using (MSSContext context = new MSSContext())
            {
                if (siteId == userSiteId || userSiteId == 0)
                {
                    Site site = (from x in context.Sites
                                 where x.SiteId == siteId
                                 select x).FirstOrDefault();
                    if (site != null)
                    {
                        if (site.Disabled != true)
                        {
                            var exists = (from x in context.Units
                                          where x.SiteId == siteId &&
                                                x.UnitName.ToUpper() == unitName.ToUpper() &&
                                                x.Disabled == false
                                          select x).FirstOrDefault();

                            Unit unit = null;

                            if (exists != null)
                            {
                                throw new Exception("There is already an active Unit at the selected site with this name. Choose a new name or set the old unit to inactive to continue.");
                            }

                            unit = new Unit();
                            unit.SiteId = siteId;
                            unit.UnitName = unitName;
                            unit.Description = description;
                            unit.Disabled = false;

                            unit = context.Units.Add(unit);
                            context.SaveChanges();
                            return unit.UnitId;
                        }
                        else
                        {
                            throw new Exception("The Site you are trying to add this Unit to is Disabled. Someone may have disabled the Site since you started adding the unit. Site Name: " + site.SiteName);
                        }
                    }
                    else
                    {
                        throw new Exception("The Unit's Site is coming up null, please contact an Administrator to check the database for siteId:" + siteId);
                    }
                }
                else
                {
                    throw new Exception("Can not add a unit to a site you do not work at. \r Please ask a SuperUser or another Administrator from the correct site.");
                }
            }
        }

        /// <summary>
        /// Unit_Update will update a unit in the database
        /// </summary>
        /// <param name="item">Item is a completed Unit Entity that will be replacing the one in the database. The UnitId should not be changed to prevent errors.</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Unit_Update(Unit item)
        {
            using (MSSContext context = new MSSContext())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Unit_Update will update a unit in the database
        /// </summary>
        /// <param name="unitId">The UnitId of the unit you wish to update</param>
        /// <param name="siteId">The new or old SiteId</param>
        /// <param name="unitName">The new or old Unit Name</param>
        /// <param name="description">The new or old Description</param>
        public void Unit_Update(int unitId, int siteId, string unitName, string description)
        {
            using (MSSContext context = new MSSContext())
            {
                var unit = (from x in context.Units
                            where x.UnitId == unitId
                            select x).FirstOrDefault();

                // Checks to see if there is a Unit at the same site with the same name they are trying to update to.
                var sameNameAndSite = (from x in context.Units
                                       where x.UnitName == unitName && x.SiteId == siteId && x.UnitId != unitId
                                       select x).FirstOrDefault();

                // Check if the unitName is unique for the site it is located at.
                if (sameNameAndSite != null)
                {
                    throw new Exception("Units with the same Site must have unique names. Please choose a new name for the unit.");
                }

                // Ensure the site hasn't been disabled.
                if (unit.Disabled == true)
                {
                    throw new Exception("Unit has been archived. It can no longer be changed.");
                }
                // If the user makes no changes throws and error.
                if (unit.SiteId == siteId && unit.UnitName == unitName && unit.Description == description)
                {
                    throw new Exception("Update failed, no properties were changed.");
                }

                // Process the update.
                unit.SiteId = siteId;
                unit.UnitName = unitName;
                unit.Description = description;
                context.Entry(unit).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Unit_Inactive will set the desired unit's deactivated field to 1(True)
        /// </summary>
        /// <param name="item">Item is the Unit that you want to set as inactive</param>
        /// <returns>Returns the UnitId of the now inactive unit</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Unit_Deactivate(Unit item)
        {
            using (MSSContext context = new MSSContext())
            {
                item = context.Units.Find(item.UnitId);
                item.Disabled = true;
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return item.UnitId;
            }
        }

        /// <summary>
        /// Unit_Inactive will set the desired unit's deactivated field to 1(True)
        /// </summary>
        /// <param name="unitId">UnitId of the Unit you want to be set as inactive</param>
        /// <returns>Returns the UnitId of the now inactive unit</returns>
        public int Unit_Deactivate(int unitId)
        {
            using (MSSContext context = new MSSContext())
            {
                Unit unit = (from x in context.Units
                             where x.UnitId == unitId
                             select x).FirstOrDefault();

                return Unit_Deactivate(unit);
            }
        }
        #endregion
    }
}
