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
        public async Task<List<Instalaciones_DTO>> ListarInstalaciones(int? instalacionId) {

            List<Instalaciones_DTO> listaInstalaciones = new List<Instalaciones_DTO>();
            ManagementObjectSearcher instalaciones = await _lists_REP.GetBuildings(_path,_user,_pass, instalacionId);

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

        public async Task<List<Instalaciones_DTO>> ListarDivisiones(int? areaId)
        {

            List<Instalaciones_DTO> listaDivisiones = new List<Instalaciones_DTO>();
            ManagementObjectSearcher divisiones = await _lists_REP.GetDivision(_path, _user, _pass, areaId);

            try
            {
                foreach (ManagementObject queryObj in divisiones.Get())
                {
                    Instalaciones_DTO item = new Instalaciones_DTO();
                    item.ID = int.Parse(queryObj["ID"].ToString());
                    try { item.name = queryObj["NAME"].ToString(); } catch { item.name = null; }
                    listaDivisiones.Add(item);
                }

                if (listaDivisiones.Count == 0)
                    throw new Exception("no se encontraron divisiones");
                return listaDivisiones;
            }
            catch (Exception ex)
            {
                throw new Exception("message: " + ex.Message + "|||query: " + divisiones.Query.QueryString +
                "|||path: " + divisiones.Scope.Path + "|||st: " + ex.StackTrace + "|||inne: " + ex.InnerException + "|||data: " +
                ex.Data + "|||helplink: " + ex.HelpLink + "|||Hresult: " + ex.HResult);
            }
        }

        public async Task<List<Instalaciones_DTO>> ListarCiudades(int? ciudadId)
        {

            List<Instalaciones_DTO> listaCiudades = new List<Instalaciones_DTO>();
            ManagementObjectSearcher ciudades = await _lists_REP.GetCiudades(_path, _user, _pass, ciudadId);

            try
            {
                foreach (ManagementObject queryObj in ciudades.Get())
                {
                    Instalaciones_DTO item = new Instalaciones_DTO();
                    item.ID = int.Parse(queryObj["ID"].ToString());
                    try { item.name = queryObj["NAME"].ToString(); } catch { item.name = null; }
                    listaCiudades.Add(item);
                }

                if (listaCiudades.Count == 0)
                    throw new Exception("no se encontraron ciudades");
                return listaCiudades;
            }
            catch (Exception ex)
            {
                throw new Exception("message: " + ex.Message + "|||query: " + ciudades.Query.QueryString +
                "|||path: " + ciudades.Scope.Path + "|||st: " + ex.StackTrace + "|||inne: " + ex.InnerException + "|||data: " +
                ex.Data + "|||helplink: " + ex.HelpLink + "|||Hresult: " + ex.HResult);
            }
        }

        public async Task<List<Instalaciones_DTO>> ListarEmpresas(int? empresaId)
        {

            List<Instalaciones_DTO> listaEmpresas = new List<Instalaciones_DTO>();
            ManagementObjectSearcher empresas = await _lists_REP.GetEmpresas(_path, _user, _pass, empresaId);

            try
            {
                foreach (ManagementObject queryObj in empresas.Get())
                {
                    Instalaciones_DTO item = new Instalaciones_DTO();
                    item.ID = int.Parse(queryObj["ID"].ToString());
                    try { item.name = queryObj["NAME"].ToString(); } catch { item.name = null; }
                    listaEmpresas.Add(item);
                }

                if (listaEmpresas.Count == 0)
                    throw new Exception("no se encontraron empresas");
                return listaEmpresas;
            }
            catch (Exception ex)
            {
                throw new Exception("message: " + ex.Message + "|||query: " + empresas.Query.QueryString +
                "|||path: " + empresas.Scope.Path + "|||st: " + ex.StackTrace + "|||inne: " + ex.InnerException + "|||data: " +
                ex.Data + "|||helplink: " + ex.HelpLink + "|||Hresult: " + ex.HResult);
            }
        }
    }
}
