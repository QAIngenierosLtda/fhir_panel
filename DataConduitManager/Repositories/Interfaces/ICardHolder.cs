using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Threading.Tasks;
using DataConduitManager.Repositories.DTO;

namespace DataConduitManager.Repositories.Interfaces
{
    public interface ICardHolder
    {
        /// <summary>
        /// CREA UN EMPLEADO NUEVA EN LENEL
        /// </summary>
        /// <returns></returns>
        Task<object> AddCardHolder(AddCardHolder_DTO newCardHolder);

        /// <summary>
        /// ACTUALIZA UN EMPLEADO EN LENEL
        /// </summary>
        /// <param name="cardHolder"></param>
        /// <param name="idLenel"></param>
        /// <returns></returns>
        Task<bool> UpdateCardHolder(UpdateCardHolder_DTO cardHolder, string idLenel);

        /// <summary>
        /// Obtiene un empleado Tarjeta Habiente en LENEL
        /// </summary>
        /// <param name="idLenel"></param>
        /// <returns></returns>
        Task<ManagementObjectSearcher> GetCardHolder(string idLenel);

        /// <summary>
        /// Obtiene un visitante en Lenel
        /// </summary>
        /// <param name="idLenel"></param>
        /// <returns></returns>
        Task<object> GetVisitor(string idLenel);
    }
}
