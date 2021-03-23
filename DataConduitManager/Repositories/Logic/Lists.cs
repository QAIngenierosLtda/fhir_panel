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

        public async Task<ManagementObjectSearcher> GetBuildings(string path, string user, string password, int? id)
        {
            ManagementScope buildingScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery buildingSearcher = new ObjectQuery();

            if (id != null)
                buildingSearcher.QueryString = @"SELECT * FROM Lnl_Edificio WHERE ID = " + id;
            else
                buildingSearcher.QueryString = @"SELECT * FROM Lnl_Edificio";

            ManagementObjectSearcher getBuildings = new ManagementObjectSearcher(buildingScope, buildingSearcher);
            
            try { return getBuildings; }
            catch (Exception ex) { throw new Exception("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }

        }

        public async Task<ManagementObjectSearcher> GetDivision(string path, string user, string password, int? id)
        {
            ManagementScope divisionScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery divisionSearcher = new ObjectQuery();

            if (id != null)
                divisionSearcher.QueryString = @"SELECT * FROM Lnl_División WHERE ID = " + id;
            else
                divisionSearcher.QueryString = @"SELECT * FROM Lnl_División";
            
            ManagementObjectSearcher getDivisiones = new ManagementObjectSearcher(divisionScope, divisionSearcher);

            try { return getDivisiones; }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public async Task<ManagementObjectSearcher> GetCiudades(string path, string user, string password, int? id)
        {
            ManagementScope ciudadScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery ciudadSearcher = new ObjectQuery();

            if (id != null)
                ciudadSearcher.QueryString = @"SELECT * FROM Lnl_Departam WHERE ID = " + id;
            else
                ciudadSearcher.QueryString = @"SELECT * FROM Lnl_Departam";
            
            ManagementObjectSearcher getCiudades = new ManagementObjectSearcher(ciudadScope, ciudadSearcher);

            try { return getCiudades; }
            catch (Exception ex) 
            { 
                throw new Exception(ex.Message); 
            }

        }

        public async Task<ManagementObjectSearcher> GetEmpresas(string path, string user, string password, int? id)
        {
            ManagementScope empresaScope = _dataConduITMgr.GetManagementScope(path, user, password);
            ObjectQuery empresaSearcher = new ObjectQuery();

            if (id != null)
                empresaSearcher.QueryString = @"SELECT * FROM Lnl_Titulo WHERE ID = " + id;
            else
                empresaSearcher.QueryString = @"SELECT * FROM Lnl_Titulo";

            ManagementObjectSearcher getEmpresas = new ManagementObjectSearcher(empresaScope, empresaSearcher);

            try { return getEmpresas; }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
    }
}
