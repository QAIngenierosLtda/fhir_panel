using System.Management;
using System.Threading.Tasks;
using DataConduitManager.Repositories.DTO;

namespace DataConduitManager.Repositories.Interfaces
{
    public interface ICardHolder
    {
        /// <summary>
        /// Crea un nuevo CardHolder en Lenel
        /// </summary>
        /// <param name="newCardHolder"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<object> AddCardHolder(AddCardHolder_DTO newCardHolder, string path, string user, string pass);

        /// <summary>
        /// Actualiza un CardHolder en Lenel
        /// </summary>
        /// <param name="cardHolder"></param>
        /// <param name="idPersona"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<bool> UpdateCardHolder(UpdateCardHolder_DTO cardHolder, string idPersona, string path, string user, string pass);

        /// <summary>
        /// Obtiene un CardHolder de Lenel
        /// </summary>
        /// <param name="documento">documento de identidad de la persona CARDHOLDER.OPHONE</param>
        /// <param name="ssno">codigo Ecopetrol</param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ManagementObjectSearcher> GetCardHolder(string documento, string ssno, string path, string user, string password);

        /// <summary>
        /// Obtiene un CardHolder en Lenel 
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ManagementObjectSearcher> GetCardHolderByID(string idPersona, string path, string user, string password);

        /// <summary>
        /// Obtiene un Visitor en Lenel
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<ManagementObjectSearcher> GetVisitor(string idPersona, string path, string user, string pass);

        /// <summary>
        /// Crea un nuevo Visitor en Lenel
        /// </summary>
        /// <param name="newCardHolder"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<object> AddVisitor(AddCardHolder_DTO newCardHolder, string path, string user, string pass);

        /// <summary>
        /// Actualiza un Visitor en Lenel
        /// </summary>
        /// <param name="cardHolder"></param>
        /// <param name="idPersona"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> UpdateVisitor(UpdateCardHolder_DTO cardHolder, string idPersona, string path, string user, string password);
    }
}
