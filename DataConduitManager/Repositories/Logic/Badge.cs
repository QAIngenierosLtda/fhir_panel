using System;
using System.Management;
using System.Globalization;
using System.Threading.Tasks;
using DataConduitManager.Repositories.Interfaces;
using DataConduitManager.Repositories.DTO;

namespace DataConduitManager.Repositories.Logic
{
    public class Badge : IBadge
    {
        private readonly IDataConduITMgr _dataConduITMgr;

        #region CONSTRUCTOR
        public Badge(IDataConduITMgr dataConduITMgr)
        {
            _dataConduITMgr = dataConduITMgr;
        }
        #endregion

        #region METODOS
        public async Task<object> AddBadge(AddBadge_DTO newBadge, string path, string user, string pass)
        {
            try
            {
                ManagementScope badgeScope = _dataConduITMgr.GetManagementScope(path, user, pass);

                ManagementClass badgeClass = new ManagementClass(badgeScope, new ManagementPath("Lnl_Badge"), null);

                ManagementObject newBadgeInstance = badgeClass.CreateInstance();

                string fechaDesactivacion = newBadge.fechaActivacion.ToString("yyyyMMdd") + "000000.000000-300";

                newBadgeInstance["ID"] = newBadge.id;
                newBadgeInstance["PERSONID"] = newBadge.personId;
                newBadgeInstance["STATUS"] = newBadge.status; //Active 
                newBadgeInstance["TYPE"] = newBadge.type; // Employee
                newBadgeInstance["DEST_EXEMPT"] = newBadge.dest_exemp; // Sometimes teh value is required
                newBadgeInstance["ACTIVATE"] = newBadge.fechaActivacion.ToString("yyyyMMdd") + "000000.000000-300";
                newBadgeInstance["DEACTIVATE"] = newBadge.fechaDesactivacion.ToString("yyyyMMdd") + "000000.000000-300";

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

        public async Task<ManagementObjectSearcher> GetBadge(string personId, string path, string user, string pass)
        {
            try
            {
                ManagementScope badgeScope = _dataConduITMgr.GetManagementScope(path, user, pass);
                ObjectQuery badgeSearcher = new ObjectQuery(@"SELECT * FROM Lnl_Badge WHERE PERSONID = '" + personId + "'");
                ManagementObjectSearcher getBadge = new ManagementObjectSearcher(badgeScope, badgeSearcher);

                try { return getBadge; }
                catch (Exception ex) { throw new Exception("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ManagementObjectSearcher> GetPersonBadge(string badgeId, string path, string user, string pass)
        {
            try
            {
                ManagementScope badgeScope = _dataConduITMgr.GetManagementScope(path, user, pass);
                ObjectQuery badgeSearcher = new ObjectQuery(@"SELECT * FROM Lnl_Badge WHERE ID = '" + badgeId + "'");
                ManagementObjectSearcher getBadge = new ManagementObjectSearcher(badgeScope, badgeSearcher);

                try { return getBadge; }
                catch (Exception ex) { throw new Exception("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateStatusBadge(string badgeId, badgeStatus status, DateTime deactivationDate, string path, string user, string pass) 
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                ManagementScope badgeScope = _dataConduITMgr.GetManagementScope(path, user, pass);
                ObjectQuery badgeSearcher = new ObjectQuery("SELECT * FROM Lnl_Badge WHERE ID = " + badgeId);
                ManagementObjectSearcher getBadge = new ManagementObjectSearcher(badgeScope, badgeSearcher);

                //redefine properties value  
                foreach (ManagementObject queryObj in getBadge.Get())
                {
                    queryObj["STATUS"] = (int)status;
                    string fechaDesactivacion = deactivationDate.ToString("yyyyMMdd") + "000000.000000-300";
                    if (status == badgeStatus.INACTIVA || status == badgeStatus.ACTIVO)
                        queryObj["DEACTIVATE"] = fechaDesactivacion;

                    PutOptions options = new PutOptions();
                    options.Type = PutType.UpdateOnly;
                    queryObj.Put(options);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
