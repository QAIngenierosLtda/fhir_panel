using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LenelServices.Repositories.DTO;

namespace LenelServices.Repositories.Interfaces
{
    public interface ILists_REP_LOCAL
    {
        /// <summary>
        /// Trae el listado de insatalaciones en Lenel por medio de DataConduIT
        /// </summary>
        /// <param name="newCardHolder"></param>
        /// <returns></returns>
        Task<List<Instalaciones_DTO>> ListarInstalaciones();
    }
}
