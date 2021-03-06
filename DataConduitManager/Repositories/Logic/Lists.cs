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

        public async Task<ManagementObjectSearcher> GetDivision(string path, string user, string password)
        {
            ManagementScope divisionScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery divisionSearcher =
                new ObjectQuery(@"SELECT * FROM Lnl_División");
            ManagementObjectSearcher getDivisiones = new ManagementObjectSearcher(divisionScope, divisionSearcher);

            try { return getDivisiones; }
            catch (Exception ex) { throw new Exception("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }

        }

        public async Task<ManagementObjectSearcher> GetCiudades(string path, string user, string password)
        {
            ManagementScope ciudadScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery ciudadSearcher =
                new ObjectQuery(@"SELECT * FROM Lnl_Departam");
            ManagementObjectSearcher getCiudades = new ManagementObjectSearcher(ciudadScope, ciudadSearcher);

            try { return getCiudades; }
            catch (Exception ex) { throw new Exception("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }

        }

        public async Task<ManagementObjectSearcher> GetEmpresas(string path, string user, string password)
        {
            ManagementScope empresaScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery empresaSearcher =
                new ObjectQuery(@"SELECT * FROM Lnl_Titulo");
            ManagementObjectSearcher getEmpresas = new ManagementObjectSearcher(empresaScope, empresaSearcher);

            try { return getEmpresas; }
            catch (Exception ex) { throw new Exception("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }

        }
    }
}
