using System.Threading.Tasks;
using LenelServices.Repositories.DTO;
using DataConduitManager.Repositories.DTO;

namespace LenelServices.Repositories.Interfaces
{
    public interface IReader_REP_LOCAL
    {
        /// <summary>
        /// obtiene parametros de la lectora y su readerMode actual por medio de DataConduIT
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="readerID"></param>
        /// <returns></returns>
        Task<object> ConfiguracionLectora(string panelID, string readerID);

        /// <summary>
        /// Establece la controla en modo UNLOCKED por medio de DataConduIT
        /// </summary>
        /// <param name="readerPath"></param>
        /// <returns></returns>
        Task<bool> AbrirPuerta(ReaderPath_DTO readerPath);

        /// <summary>
        /// Establece la lectora en modo LOCKED por medio de DataConduIT
        /// </summary>
        /// <param name="readerPath"></param>
        /// <returns></returns>
        Task<object> BloquearPuerta(ReaderPath_DTO readerPath);

        /// <summary>
        /// Establece la lectora en el modo elegido por medio de DataConduIT
        /// </summary>
        /// <param name="readerPath"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        Task<object> CambioEstadoPuerta(ReaderPath_DTO readerPath, int estado);

        /// <summary>
        /// Genera un evento generico por medio de DataConduIT
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        Task<bool> EnviarEventoGenerico(SendEvent_DTO evento);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="readerID"></param>
        /// <param name="gap"></param>
        /// <param name="intentos"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<object> LastEventDoor(int panelID, int readerID, int gap, int intentos, int timeout);
    }
}
