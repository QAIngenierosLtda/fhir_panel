using System;
using System.Threading.Tasks;
using System.Management;
using DataConduitManager.Repositories.DTO;

namespace DataConduitManager.Repositories.Interfaces
{
    public enum badgeStatus
    {
        ACTIVO = 1, 
        PERDIDO = 2, 
        REGRESADO = 3, 
        ACTIVE = 6, 
        INACTIVA = 7,
    }

    public interface IBadge
    {
        /// <summary>
        /// Crea un nuevo Badge en Lenel
        /// </summary>
        /// <param name="newBadge"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<object> AddBadge(AddBadge_DTO newBadge, string path, string user, string pass);

        /// <summary>
        /// Obtiene la informacion de un Badge en Lenel
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<ManagementObjectSearcher> GetBadge(string personId, string path, string user, string pass);

        /// <summary>
        /// Obtiene la informacion de una tarjeta dado su id
        /// </summary>
        /// <param name="badgeId"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<ManagementObjectSearcher> GetPersonBadge(string badgeId, string path, string user, string pass);

        /// <summary>
        /// Actualiza el estado de una Tarjeta
        /// </summary>
        /// <param name="badgeId"></param>
        /// <param name="status"></param>
        /// <param name="deactivationDate"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<bool> UpdateStatusBadge(string badgeId, badgeStatus status, DateTime deactivationDate, string path, string user, string pass);

        /// <summary>
        /// obtiene la ultima ubicacion del cardholder
        /// </summary>
        /// <param name="personaId"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ManagementObjectSearcher> GetLastLocation(int personaId, string path, string user, string password);
    }
}
