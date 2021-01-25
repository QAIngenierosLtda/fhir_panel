using System;
using System.Management;
using DataConduitManager.Repositories.Interfaces;

namespace DataConduitManager.Repositories.Logic
{
    public class DataConduITMgr : IDataConduITMgr
    {
        public ManagementPath path { get; set; }
        public ManagementPath clave { get; set; }

        public DataConduITMgr()
        {
            /*PATH PARA CONECTAR A DATACONDUIT */
            path = new ManagementPath("\\\\10.11.34.96\\root\\OnGuard"); 
            //VERIFICAR SI ESTO SERA UN PARAMETRO O SE FIJARA EN UN ARCHIVO DE CONFIGURACION EN CADA LUGAR DONDE SE EJECUTE 
        }

        public ManagementScope GetManagementScope() {
            return CreateDataConduitScope();
        }

        private ManagementScope CreateDataConduitScope()
        {
            /*ESTABLECE UN SCOPE DE DATACONDUIT PARA REALIZAR UNA ACCION*/
            ConnectionOptions conexion = new ConnectionOptions();
            conexion.Username = "Administrator";//SE PARAMETRIZAR EN UN ARCHIVO DE CONFIGURACION
            conexion.Password = "Ut1ndr40nc0r1732";//SE PARAMETRIZAR EN UN ARCHIVO DE CONFIGURACION
            conexion.Authentication = AuthenticationLevel.Default;
            conexion.Impersonation = ImpersonationLevel.Impersonate;
            conexion.EnablePrivileges = true;
            return new ManagementScope(path, conexion);
        }
    }
}
