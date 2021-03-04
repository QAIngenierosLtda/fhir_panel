using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management;
using LenelServices.Repositories.Interfaces;
using LenelServices.Repositories.DTO;
using DataConduitManager.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LenelServices.Repositories.Logic
{
    public class Lists_REP_LOCAL:ILists_REP_LOCAL
    {
        private readonly ILists _lists_REP;
        private readonly IConfiguration _config;
        private string _path;
        private string _user;
        private string _pass;
        public Lists_REP_LOCAL(ILists lists_REP, IConfiguration config)
        {
            _lists_REP = lists_REP;
            _config = config;
            _path = _config.GetSection("SERVER_PATH").Value.ToString();
            _user = _config.GetSection("SERVER_USER").Value.ToString();
            _pass = _config.GetSection("SERVER_PASSWORD").Value.ToString();
        }
        public async Task<List<Instalaciones_DTO>> ListarInstalaciones() {

            List<Instalaciones_DTO> listaInstalaciones = new List<Instalaciones_DTO>();
            ManagementObjectSearcher instalaciones = await _lists_REP.GetBuildings(_path,_user,_pass);

            try
            {
                foreach (ManagementObject queryObj in instalaciones.Get())
                {
                    Instalaciones_DTO item = new Instalaciones_DTO();
                    item.ID = int.Parse(queryObj["ID"].ToString());
                    try { item.name = queryObj["NAME"].ToString(); } catch { item.name = null; }
                    listaInstalaciones.Add(item);                    
                }

                if (listaInstalaciones.Count == 0)
                    throw new Exception("no se encontraron instalaciones");
                return listaInstalaciones;
            }
            catch (Exception ex)
            {
                throw new Exception("message: " + ex.Message + "|||query: " + instalaciones.Query.QueryString +
                "|||path: " + instalaciones.Scope.Path + "|||st: " + ex.StackTrace + "|||inne: " + ex.InnerException + "|||data: " +
                ex.Data + "|||helplink: " + ex.HelpLink + "|||Hresult: " + ex.HResult);
            }
        }
    }
}
