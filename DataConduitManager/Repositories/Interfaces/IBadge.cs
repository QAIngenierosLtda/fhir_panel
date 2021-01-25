using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using DataConduitManager.Repositories.DTO;

namespace DataConduitManager.Repositories.Interfaces
{
    public interface IBadge
    {
        /// <summary>
        /// CREA UNA TARJETA NUEVA A UN USUARIO DEL SISTEMA EN LENEL
        /// </summary>
        /// <param name="newBadge"></param>
        /// <returns></returns>
        Task<object> AddBadge(AddBadge_DTO newBadge);
    }
}
