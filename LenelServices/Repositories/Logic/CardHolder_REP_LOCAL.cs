using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using LenelServices.Repositories.Interfaces;
using LenelServices.Repositories.DTO;
using DataConduitManager.Repositories.Interfaces;
using DataConduitManager.Repositories.DTO;

namespace LenelServices.Repositories.Logic
{
    public class CardHolder_REP_LOCAL:ICardHolder_REP_LOCAL
    {
        private readonly ICardHolder _cardHolder_REP;

        #region Constructor
        public CardHolder_REP_LOCAL(ICardHolder cardHolder_REP)
        {
            _cardHolder_REP = cardHolder_REP;
        }
        #endregion

        #region Métodos
        public async Task<object> CrearPersona(AddCardHolder_DTO newCardHolder)
        {
            return await _cardHolder_REP.AddCardHolder(newCardHolder);
        }

        public async Task<GetCardHolder_DTO> ObtenerEmpleado(string idLenel) {

            GetCardHolder_DTO persona = new GetCardHolder_DTO();
            ManagementObjectSearcher cardHolder =  await _cardHolder_REP.GetCardHolder(idLenel);

            foreach (ManagementObject queryObj in cardHolder.Get())
            {
                persona.apellidos = queryObj["LASTNAME"].ToString();
                persona.nombres = queryObj["FIRSTNAME"].ToString();
                persona.ssno = queryObj["SSNO"].ToString();
                try { persona.status = queryObj["STATE"].ToString(); } catch { persona.status = null; }
                try { persona.documento = queryObj["OPHONE"].ToString(); } catch { persona.documento = null; }
                try { persona.empresa = queryObj["DIVISION"].ToString(); } catch { persona.empresa = null; }
                try { persona.ciudad = queryObj["CITY"].ToString(); } catch { persona.ciudad = null; }
                try { persona.email = queryObj["EMAIL"].ToString(); } catch { persona.email = null; }
            }

            return persona;
        }

        public async Task<string> ActualizarEmpleado(UpdateCardHolder_DTO cardHolder, string idLenel) 
        {
            bool actualizado = await _cardHolder_REP.UpdateCardHolder(cardHolder, idLenel);

            if (actualizado)
                return "El empleado fue actualizado satisfactoriamente"; 
            else throw new Exception("No fue posible realizar la actualización de datos");
        }

        public async Task<object> ObtenerVisitante(string idLenel) {
            return await _cardHolder_REP.GetVisitor(idLenel);
        }
        #endregion
    }
}