using System;
using System.Management;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using DataConduitManager.Repositories.Interfaces;
using DataConduitManager.Repositories.DTO;

namespace DataConduitManager.Repositories.Logic
{
    public class Badge : IBadge
    {
        private readonly IDataConduITMgr _dataConduITMgr;

        #region Constructor
        public Badge(IDataConduITMgr dataConduITMgr)
        {
            _dataConduITMgr = dataConduITMgr;
        }
        #endregion

        #region Métodos
        public async Task<object> AddBadge(AddBadge_DTO newBadge)
        {
            try
            {
                ManagementScope badgeScope = _dataConduITMgr.GetManagementScope();

                ManagementClass badgeClass = new ManagementClass(badgeScope, new ManagementPath("Lnl_Badge"), null);

                ManagementObject newBadgeInstance = badgeClass.CreateInstance();

                newBadgeInstance["ID"] = newBadge.id;
                newBadgeInstance["PERSONID"] = newBadge.personId;
                newBadgeInstance["STATUS"] = newBadge.status; //Active 
                newBadgeInstance["TYPE"] = newBadge.type; // Employee
                newBadgeInstance["DEST_EXEMPT"] = newBadge.dest_exemp; // Sometimes teh value is required

                ManagementBaseObject inParams = badgeClass.GetMethodParameters("AddBadge");
                inParams.Properties["BadgeIn"].Value = newBadgeInstance;

                //Execute the method
                ManagementBaseObject outParamObject = badgeClass.InvokeMethod("AddBadge", inParams, null);

                if (outParamObject != null)
                {
                    //Display results
                    object outObj = outParamObject["BadgeOut"];
                    ManagementBaseObject addedBadge = (ManagementBaseObject)outObj;
                    return addedBadge["BADGEKEY"];
                }
                else
                {
                    throw new Exception("No se pudo crear el nuevo Badge");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }    
        #endregion

    }
}
