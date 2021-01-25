using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataConduitManager.Repositories.DTO;

namespace LenelServices.Repositories.Interfaces
{
    public interface IBadge_REP_LOCAL
    {
        /// <summary>
        /// Crea una nueva tarjeta a una persona creada en el sistema por medio de DataCoduIT
        /// </summary>
        /// <param name="newBadge"></param>
        /// <returns></returns>
        Task<object> CrearBadge(AddBadge_DTO newBadge);
    }
}
