using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.DTO;

namespace LenelServices.Repositories.Interfaces
{
    public interface ICardHolder_REP_LOCAL
    {
        /// <summary>
        /// Crea un nuevo tarjetaHabiente por medio de DataConduIT
        /// </summary>
        /// <param name="newBadge"></param>
        /// <returns></returns>
        Task<object> CrearPersona(AddCardHolder_DTO newCardHolder);

        /// <summary>
        /// Obtiene un Empleado por medio de DataConduIT
        /// </summary>
        /// <param name="idLenel"></param>
        /// <returns></returns>
        Task<GetCardHolder_DTO> ObtenerEmpleado(string idLenel);

        /// <summary>
        /// Obtiene un Visitante por medio de DataConduIT
        /// </summary>
        /// <param name="idLenel"></param>
        /// <returns></returns>
        Task<object> ObtenerVisitante(string idLenel);

        /// <summary>
        /// Actualiza la informacion de un empleado por medio de DataConduIT
        /// </summary>
        /// <param name="cardHolder"></param>
        /// <param name="idLenel"></param>
        /// <returns></returns>
        Task<string> ActualizarEmpleado(UpdateCardHolder_DTO cardHolder, string idLenel);
    }
}
