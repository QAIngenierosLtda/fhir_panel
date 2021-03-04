using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using DataConduitManager.Repositories.Interfaces;
using System.Threading.Tasks;

namespace DataConduitManager.Repositories.Logic
{
    public class Lists:ILists
    {
        private readonly IDataConduITMgr _dataConduITMgr;
        public Lists(IDataConduITMgr dataConduITMgr)
        {
            _dataConduITMgr = dataConduITMgr;
        }

        public async Task<ManagementObjectSearcher> GetBuildings(string path, string user, string password)
        {
            ManagementScope buildingScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery buildingSearcher =
                new ObjectQuery(@"SELECT * FROM Lnl_Edificio");
            ManagementObjectSearcher getBuildings = new ManagementObjectSearcher(buildingScope, buildingSearcher);

            try { return getBuildings; }
            catch (Exception ex) { throw new Exception("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }

        }
    }
}
