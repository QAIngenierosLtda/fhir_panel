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

        public async Task<object> LastEvent(string documento) 
        {
            int personaId = 0;
            string readerName = string.Empty;
            LastLocation_DTO lastLocation = new LastLocation_DTO();
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
                    ManagementObjectSearcher LastLocation = await _badge_REP.GetLastLocation(personaId, _path, _user, _pass);

                    foreach (ManagementObject queryObj in LastLocation.Get())
                    {
                        try { lastLocation.badgeId = (int)queryObj["BADGEID"]; } catch { lastLocation.badgeId = 0; }
                        try { lastLocation.eventTime = queryObj["EVENTTIME"].ToString(); } catch { lastLocation.eventTime = null; }
                        try { lastLocation.panelId = (int)queryObj["PANELID"]; } catch { lastLocation.panelId = 0; }
                        try { lastLocation.readerId = (int)queryObj["READERID"]; } catch { lastLocation.readerId = 0; }
                    }

                    if (string.IsNullOrEmpty(lastLocation.eventTime))
                        throw new Exception("no se encontró un evento asociado");
                    else
                    {
                        ManagementObjectSearcher readerData = await _reader_REP.GetReaderData
                            (lastLocation.panelId.ToString(), lastLocation.readerId.ToString(), _path, _user, _pass);

                        foreach (ManagementObject queryObj in readerData.Get())
                        {
                            readerName = queryObj["Name"].ToString();
                        }
                    }
                }

                object result = new
                {
                    badgeId = lastLocation.badgeId,
                    eventTime = lastLocation.eventTime,
                    panelId = lastLocation.panelId,
                    readerId = lastLocation.readerId,
                    name = readerName
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
