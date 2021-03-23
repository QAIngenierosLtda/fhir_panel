using System;
using System.Collections.Generic;
using System.Management;
using System.Threading.Tasks;
using LenelServices.Repositories.Interfaces;
using LenelServices.Repositories.DTO;
using DataConduitManager.Repositories.Interfaces;
using DataConduitManager.Repositories.DTO;
using Microsoft.Extensions.Configuration;

namespace LenelServices.Repositories.Logic
{
    public class CardHolder_REP_LOCAL:ICardHolder_REP_LOCAL
    {
        #region PROPIEDADES
        private readonly ICardHolder _cardHolder_REP;
        private readonly IDataConduITMgr _dataConduITMgr;
        private readonly IConfiguration _config;
        private readonly IBadge_REP_LOCAL _badge_REP_LOCAL;
        private string _path;
        private string _user;
        private string _pass;
        #endregion

        #region CONSTRUCTOR
        public CardHolder_REP_LOCAL(ICardHolder cardHolder_REP, IBadge_REP_LOCAL badge_REP_LOCAL,
            IDataConduITMgr dataConduITMgr, IConfiguration config)
        {
            _cardHolder_REP = cardHolder_REP;
            _badge_REP_LOCAL = badge_REP_LOCAL;
            _dataConduITMgr = dataConduITMgr;
            _config = config;
            _path = _config.GetSection("SERVER_PATH").Value.ToString();
            _user = _config.GetSection("SERVER_USER").Value.ToString();
            _pass = _config.GetSection("SERVER_PASSWORD").Value.ToString();
        }
        #endregion

        #region METODOS
        public async Task<object> CrearPersona(AddCardHolder_DTO newCardHolder)
        {
            try {
                await _cardHolder_REP.AddCardHolder(newCardHolder, _path, _user, _pass);
                return await ObtenerPersona(newCardHolder.nroDocumento, newCardHolder.ssno);
            }
            catch (Exception ex) 
            { 
                throw new Exception("No fue posible completar la operación " + ex.Message); 
            } 
        }

        public async Task<GetCardHolder_DTO> ObtenerPersona(string documento, string ssno) 
        { 
            GetCardHolder_DTO persona = new GetCardHolder_DTO();

            ManagementObjectSearcher cardHolder = await _cardHolder_REP.GetCardHolder(documento, ssno, _path, _user, _pass);

            try
            {
                foreach (ManagementObject queryObj in cardHolder.Get())
                {
                    persona.id = int.Parse(queryObj["ID"].ToString());
                    try { persona.apellidos = queryObj["LASTNAME"].ToString(); } catch { persona.apellidos = null; }
                    try { persona.nombres = queryObj["FIRSTNAME"].ToString(); } catch { persona.nombres = null; }
                    try { persona.ssno = queryObj["SSNO"].ToString();} catch { persona.ssno = null; }
                    try { persona.status = queryObj["STATE"].ToString(); } catch { persona.status = null; }
                    try { persona.documento = queryObj["OPHONE"].ToString(); } catch { persona.documento = null; }
                    try { persona.empresa = queryObj["TITLE"].ToString(); } catch { persona.empresa = null; }
                    try { persona.ciudad = queryObj["DEPT"].ToString(); } catch { persona.ciudad = null; }
                    try { persona.instalacion = queryObj["BUILDING"].ToString(); } catch { persona.instalacion = null; }
                    try { persona.piso = queryObj["FLOOR"].ToString(); } catch { persona.piso = null; }
                    try { persona.area = queryObj["DIVISION"].ToString(); } catch { persona.area = null; }
                    try { persona.email = queryObj["EMAIL"].ToString(); } catch { persona.email = null; }
                    persona.permiteVisitantes = (bool)queryObj["ALLOWEDVISITORS"];
                    List<GetBadge_DTO> badges = await _badge_REP_LOCAL.ConsultarBadge(queryObj["ID"].ToString());
                    persona.Badges = badges;
                }

                if (persona.id == 0)
                    throw new Exception("no se encontró una persona registrada con esos datos");
                return persona;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            } 
        }

        public async Task<GetCardHolder_DTO> ObtenerPersona(string idBadge)
        {
            GetCardHolder_DTO persona = new GetCardHolder_DTO();
            ManagementObjectSearcher cardHolder = new ManagementObjectSearcher();

            string idPersona = await _badge_REP_LOCAL.ConsultarPersonaBadge(idBadge);

            if (idPersona == "NA")
                throw new Exception("No hay una persona relacionada a este codigo de tarjeta");
            
            cardHolder = await _cardHolder_REP.GetCardHolderByID(idPersona, _path, _user, _pass);

            try
            {
                foreach (ManagementObject queryObj in cardHolder.Get())
                {
                    persona.id = int.Parse(queryObj["ID"].ToString());
                    try { persona.apellidos = queryObj["LASTNAME"].ToString(); } catch { persona.apellidos = null; }
                    try { persona.nombres = queryObj["FIRSTNAME"].ToString(); } catch { persona.nombres = null; }
                    try { persona.ssno = queryObj["SSNO"].ToString(); } catch { persona.ssno = null; }
                    try { persona.status = queryObj["STATE"].ToString(); } catch { persona.status = null; }
                    try { persona.documento = queryObj["OPHONE"].ToString(); } catch { persona.documento = null; }
                    try { persona.empresa = queryObj["TITLE"].ToString(); } catch { persona.empresa = null; }
                    try { persona.ciudad = queryObj["DEPT"].ToString(); } catch { persona.ciudad = null; }
                    try { persona.instalacion = queryObj["BUILDING"].ToString(); } catch { persona.instalacion = null; }
                    try { persona.piso = queryObj["FLOOR"].ToString(); } catch { persona.piso = null; }
                    try { persona.area = queryObj["DIVISION"].ToString(); } catch { persona.area = null; }
                    try { persona.email = queryObj["EMAIL"].ToString(); } catch { persona.email = null; }
                    List<GetBadge_DTO> badges = await _badge_REP_LOCAL.ConsultarBadge(queryObj["ID"].ToString());
                    persona.Badges = badges;
                }

                if (persona.id == 0)
                    throw new Exception("no se encontró una persona registrada con esos datos");
                return persona;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetCardHolder_DTO> ObtenerPersonaByName(string nombre, string apellido) 
        {
            GetCardHolder_DTO persona = new GetCardHolder_DTO();

            ManagementObjectSearcher cardHolder = await _cardHolder_REP.GetCardHolderByName(_path, _user, _pass, nombre, apellido);

            try
            {
                foreach (ManagementObject queryObj in cardHolder.Get())
                {
                    persona.id = int.Parse(queryObj["ID"].ToString());
                    try { persona.apellidos = queryObj["LASTNAME"].ToString(); } catch { persona.apellidos = null; }
                    try { persona.nombres = queryObj["FIRSTNAME"].ToString(); } catch { persona.nombres = null; }
                    try { persona.ssno = queryObj["SSNO"].ToString(); } catch { persona.ssno = null; }
                    try { persona.status = queryObj["STATE"].ToString(); } catch { persona.status = null; }
                    try { persona.documento = queryObj["OPHONE"].ToString(); } catch { persona.documento = null; }
                    try { persona.empresa = queryObj["TITLE"].ToString(); } catch { persona.empresa = null; }
                    try { persona.ciudad = queryObj["DEPT"].ToString(); } catch { persona.ciudad = null; }
                    try { persona.instalacion = queryObj["BUILDING"].ToString(); } catch { persona.instalacion = null; }
                    try { persona.piso = queryObj["FLOOR"].ToString(); } catch { persona.piso = null; }
                    try { persona.area = queryObj["DIVISION"].ToString(); } catch { persona.area = null; }
                    try { persona.email = queryObj["EMAIL"].ToString(); } catch { persona.email = null; }
                    persona.permiteVisitantes = (bool)queryObj["ALLOWEDVISITORS"];
                    List<GetBadge_DTO> badges = await _badge_REP_LOCAL.ConsultarBadge(queryObj["ID"].ToString());
                    persona.Badges = badges;
                }

                if (persona.id == 0)
                    throw new Exception("no se encontró una persona registrada con esos datos");
                return persona;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> ActualizarPersona(UpdateCardHolder_DTO cardHolder, string idPersona) 
        {
            bool actualizado = false;
                actualizado = await _cardHolder_REP.UpdateCardHolder(cardHolder, idPersona, _path, _user, _pass);

            if (actualizado)
                return await ObtenerPersona(cardHolder.nrodocumento, cardHolder.ssno); 
            else throw new Exception("No fue posible realizar la actualización de datos");
        }

        public async Task<object> ObtenerVisitante(string idLenel) {
            return await _cardHolder_REP.GetVisitor(idLenel, _path, _user, _pass);
        }
        #endregion
    }
}