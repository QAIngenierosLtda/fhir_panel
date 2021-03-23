using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace DataConduitManager.Repositories.Interfaces
{
    public interface IDataConduITMgr
    {

        /// <summary>
        /// Obtiene un scope de DataConduIT en servidor local para ejecutar una accion sobre Lenel
        /// </summary>
        /// <returns></returns>
        ManagementScope GetManagementScope();

        /// <summary>
        /// Obtiene un scope de DataConduIT en servidor remoto para ejecutar una accion sobre Lenel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        ManagementScope GetManagementScope(string path, string user, string pass);

        string ReceiveEvent(ManagementScope scope);

        string GetManager(ManagementScope scope);
    }
}
