using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardHolderController : ControllerBase
    {
        private readonly ICardHolder_REP_LOCAL _cardHolder_REP_LOCAL;

        #region CONSTRUCTOR
        public CardHolderController(ICardHolder_REP_LOCAL cardHolder_REP_LOCAL)
        {
            _cardHolder_REP_LOCAL = cardHolder_REP_LOCAL;
        }
        #endregion

        /// <summary>
        /// Obtiene una persona visitante o cardHolder
        /// </summary>
        /// <param name="idTipo">F = FUNCIONARIO -- V = VISITANTE</param>
        /// <param name="idPersona">Numero de identificacion</param>
        /// <returns></returns>
        [HttpGet("/api/CardHolder/ObtenerPersona/{idTipo}/{idPersona}")]
        public async Task<object> ObtenerPersona(string idTipo, string idPersona)
        {
            try 
            {
                return await _cardHolder_REP_LOCAL.ObtenerPersona(idPersona, idTipo);
            }
            catch (Exception ex) 
            {
                return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); 
            }
        }

        /// <summary>
        /// Obtiene una persona visitante o cardHolder
        /// </summary>
        /// <param name="idBadge"></param>
        /// <returns></returns>
        [HttpGet("/api/CardHolder/ObtenerPersona/{idBadge}")]
        public async Task<object> ObtenerPersona(string idBadge)
        {
            try
            {
                return await _cardHolder_REP_LOCAL.ObtenerPersona(idBadge);
            }
            catch (Exception ex)
            {
                return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }

        /// <summary>
        /// Crea un Funcionario o visitante en Lenel
        /// </summary>
        /// <param name="newCardHolder"></param>
        /// <param name="idTipo">F = FUNCIONARIO -- V = VISITANTE</param>
        /// <returns></returns>
        [HttpPost("/api/CardHolder/CrearPersona/{idTipo}")]
        public async Task<object> CrearPersona([FromBody] AddCardHolder_DTO newCardHolder, string idTipo)
        {
            try { return await _cardHolder_REP_LOCAL.CrearPersona(newCardHolder, idTipo); } 
            catch(Exception ex) 
            { return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException); }
        }

        /// <summary>
        /// Actualiza una persona en Lenel
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="cardHolder"></param>
        /// <returns></returns>
        [HttpPut("/api/CardHolder/ActualizarPersona/{idPersona}")]
        public async Task<object> ActualizarPersona(string idPersona, [FromBody] UpdateCardHolder_DTO cardHolder)
        {
            try { return await _cardHolder_REP_LOCAL.ActualizarPersona(cardHolder, idPersona); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
