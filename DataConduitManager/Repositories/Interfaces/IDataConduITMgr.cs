using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace DataConduitManager.Repositories.Interfaces
{
    public interface IDataConduITMgr
    {
        /// <summary>
        /// Obtiene un scope de DataConduIT para ejecutar una accion sobre Lenel
        /// </summary>
        /// <returns></returns>
        ManagementScope GetManagementScope();
    }
}
