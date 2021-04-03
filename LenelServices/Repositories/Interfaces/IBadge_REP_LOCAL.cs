using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.DTO;

namespace LenelServices.Repositories.Interfaces
{
    public interface IBadge_REP_LOCAL
    {
        /// <summary>
        /// Obtiene el ultimo evento de una tarjeta
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        Task<object> LastEvent(string documento);

        /// <summary>
        /// Crea una nueva tarjeta a una persona creada en el sistema por medio de DataCoduIT
        /// </summary>
        /// <param name="newBadge"></param>
        /// <returns></returns>
        Task<object> CrearBadge(AddBadge_DTO newBadge);

        /// <summary>
        /// Obtiene la informacion relacionada a un badge por medio de DataConduIT
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<List<GetBadge_DTO>> ConsultarBadge(string personId);

        /// <summary>
        /// Obtiene la informacion relacionada a un badge por medio de DataConduIT
        /// </summary>
        /// <param name="badgeId"></param>
        /// <returns></returns>
        Task<string> ConsultarPersonaBadge(string badgeId);

        /// <summary>
        /// Cambia el estado de una tarjeta
        /// </summary>
        /// <param name="badgeId"></param>
        /// <param name="nuevoEstado"></param>
        /// <returns></returns>
        Task<bool> ActualizarEstadoBadge(string badgeId, SetBadgeStatus_DTO nuevoEstado);
    }
}
