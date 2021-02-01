using System.Threading.Tasks;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.DTO;

namespace LenelServices.Repositories.Interfaces
{
    public interface ICardHolder_REP_LOCAL
    {
        /// <summary>
        /// Crea un nuevo TarjetaHabiente en Lenel por medio de DataConduIT
        /// </summary>
        /// <param name="newCardHolder"></param>
        /// <returns></returns>
        Task<object> CrearPersona(AddCardHolder_DTO newCardHolder);

        /// <summary>
        /// Obtiene un tarjeta habiente por medio de DataConduIT
        /// </summary>
        /// <param name="documento">documento de identidad LENEL OPHONE </param>
        /// <param name="ssno">codigo de ecopetrol</param>
        /// <returns></returns>
        Task<GetCardHolder_DTO> ObtenerPersona(string documento, string ssno);

        /// <summary>
        /// Obtiene una persona de lenel por medio de DataConduIT
        /// </summary>
        /// <param name="idBadge"></param>
        /// <returns></returns>
        Task<GetCardHolder_DTO> ObtenerPersona(string idBadge);

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
        Task<object> ActualizarPersona(UpdateCardHolder_DTO cardHolder, string idPersona);
    }
}
