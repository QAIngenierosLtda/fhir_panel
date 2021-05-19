using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Management;
using LenelServices.Repositories.Interfaces;
using LenelServices.Repositories.DTO;
using DataConduitManager.Repositories.DTO;
using DataConduitManager.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace LenelServices.Repositories.Logic
{
    public class Badge_REP_LOCAL : IBadge_REP_LOCAL
    {
        #region PROPIEDADES
        private readonly IBadge _badge_REP;
        private readonly ICardHolder _cardHolder_REP;
        private readonly IReader _reader_REP;
        private readonly IConfiguration _config;
        private string _path;
        private string _user;
        private string _pass;
        #endregion

        #region CONSTRUCTOR
        public Badge_REP_LOCAL(IBadge badge_REP, ICardHolder cardHolder_REP, IReader reader_REP, IConfiguration config)
        {
            _badge_REP = badge_REP;
            _cardHolder_REP = cardHolder_REP;
            _reader_REP = reader_REP;
            _config = config;
            _path = _config.GetSection("SERVER_PATH").Value.ToString();
            _user = _config.GetSection("SERVER_USER").Value.ToString();
            _pass = _config.GetSection("SERVER_PASSWORD").Value.ToString();
        }
        #endregion

        #region METODOS

        public async Task<object> LastEvent(string documento,int gap, int intentos, int timeout) 
        {
            bool salir = false;
            DateTime tiempoEvento = new DateTime();
            int personaId = 0;
            string resultado = "OK";
            
            List<LastLocation_DTO> listLocations = new List<LastLocation_DTO>();
            ManagementObjectSearcher cardHolder = await _cardHolder_REP.GetCardHolder(documento, "", _path, _user, _pass);

            try
            {
                foreach (ManagementObject queryObj in cardHolder.Get())
                {
                    personaId = int.Parse(queryObj["ID"].ToString());
                }

                if (personaId == 0)
                    throw new Exception("no se encontró una persona registrada con esos datos");
                else {
                    int intento = 1;
                    do {
                        ManagementObjectSearcher LastLocation = await _badge_REP.GetLastLocation(personaId, _path, _user, _pass);
                        listLocations.Clear();

                        foreach (ManagementObject queryObj in LastLocation.Get())
                        {
                            LastLocation_DTO lastLocation = new LastLocation_DTO();
                            try { lastLocation.badgeId = queryObj["BADGEID"].ToString(); } catch { lastLocation.badgeId = "0"; }
                            try { lastLocation.eventTime = queryObj["EVENTTIME"].ToString(); } catch { lastLocation.eventTime = null; }
                            try { lastLocation.panelId = (int)queryObj["PANELID"]; } catch { lastLocation.panelId = 0; }
                            try { lastLocation.readerId = (int)queryObj["READERID"]; } catch { lastLocation.readerId = 0; }
                            listLocations.Add(lastLocation);
                        }
                        
                        foreach (LastLocation_DTO location in listLocations) {
                            if (string.IsNullOrEmpty(location.eventTime))
                                throw new Exception("no se encontró un evento asociado");
                            else
                            {
                                tiempoEvento = DateTime.ParseExact(location.eventTime.Substring(0, 14), "yyyyMMddHHmmss", null);
                                TimeSpan difTime = tiempoEvento - DateTime.Now;

                                if (difTime.Duration() < new TimeSpan(0, 0, gap)) 
                                {
                                    salir = true;
                                    break;
                                }
                            }
                        }

                        if (intento == intentos && salir != true)
                        {
                            resultado = "No se presento un evento en la ventana de tiempo requerida";
                            salir = true;
                        }

                        else
                        {
                            System.Threading.Thread.Sleep(timeout);
                            intento++;
                        }

                    } while (salir == false);
                }

                object result = new
                {
                    locations = listLocations,
                    result = resultado
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> CrearBadge(AddBadge_DTO newBadge)
        {
            return await _badge_REP.AddBadge(newBadge, _path, _user, _pass);
        }

        public async Task<List<GetBadge_DTO>> ConsultarBadge(string personId) 
        {
            List<GetBadge_DTO> tarjetas = new List<GetBadge_DTO>();
            ManagementObjectSearcher badge = await _badge_REP.GetBadge(personId, _path, _user, _pass);

            try
            {
                foreach (ManagementObject queryObj in badge.Get())
                {
                    GetBadge_DTO item = new GetBadge_DTO();
                    item.badgeID = queryObj["ID"].ToString();
                    try
                    {
                        item.activacion = DateTime.ParseExact(queryObj["ACTIVATE"].ToString().Substring(0, 14),
                            "yyyyMMddHHmmss", null);
                    }
                    catch
                    {
                        item.activacion = null;
                    }
                    try
                    {
                        item.desactivacion = DateTime.ParseExact(queryObj["DEACTIVATE"].ToString().Substring(0, 14),
                            "yyyyMMddHHmmss", null);
                    }
                    catch
                    {
                        item.desactivacion = null;
                    }
                    try
                    {
                        item.estado = queryObj["STATUS"].ToString();
                    }
                    catch
                    {
                        item.desactivacion = null;
                    }
                    try { item.type = int.Parse(queryObj["TYPE"].ToString()); }
                    catch { item.type = null; }
                    try { item.badgekey = int.Parse(queryObj["BADGEKEY"].ToString()); }
                    catch { item.badgekey = 0; }

                    tarjetas.Add(item);
                }
                return tarjetas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> ConsultarPersonaBadge(string badgeId)
        {
            ManagementObjectSearcher badge = await _badge_REP.GetPersonBadge(badgeId, _path, _user, _pass);

            try
            {
                foreach (ManagementObject queryObj in badge.Get())
                {
                    return queryObj["PERSONID"].ToString();
                }
                return "NA";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ActualizarEstadoBadge(string badgeId, SetBadgeStatus_DTO nuevoEstado) 
        {
            badgeStatus status = (badgeStatus)Enum.Parse(typeof(badgeStatus), nuevoEstado.estadoBadge.ToString());

            if (nuevoEstado.fechaDesactivacion == null)
                nuevoEstado.fechaDesactivacion = DateTime.Now;

            return await _badge_REP.UpdateStatusBadge(badgeId, status, (DateTime)nuevoEstado.fechaDesactivacion,
                _path, _user, _pass);
        }
        #endregion
    }
}
