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
        /// <returns></returns>
        Task<List<Instalaciones_DTO>> ListarInstalaciones(int? instalacionId);

        /// <summary>
        /// Trae el listado de Divisiones (areas de la compañia= en Lenel por medio de DataConduIT
        /// </summary>
        /// <returns></returns>
        Task<List<Instalaciones_DTO>> ListarDivisiones(int? areaId);

        /// <summary>
        /// Trae el listado de Ciudades en Lenel por medio de DataConduIT
        /// </summary>
        /// <returns></returns>
        Task<List<Instalaciones_DTO>> ListarCiudades(int? ciudadId);

        /// <summary>
        /// Trae el listado de insatalaciones en Lenel por medio de DataConduIT
        /// </summary>
        /// <returns></returns>
        Task<List<Instalaciones_DTO>> ListarEmpresas(int? empresaId);
    }
}
