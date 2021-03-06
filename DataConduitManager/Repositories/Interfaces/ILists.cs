using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace DataConduitManager.Repositories.Interfaces
{
    public interface ILists
    {
        Task<ManagementObjectSearcher> GetBuildings(string path, string user, string password);
        Task<ManagementObjectSearcher> GetDivision(string path, string user, string password);
        Task<ManagementObjectSearcher> GetCiudades(string path, string user, string password);
        Task<ManagementObjectSearcher> GetEmpresas(string path, string user, string password);

    }
}
