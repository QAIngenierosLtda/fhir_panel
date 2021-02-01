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
        /// 
        /// </summary>
        /// <param name="documento">Documento de identidad del tarjetaHabiente</param>
        /// <param name="ssno">codigo Ecopetrol</param>
        /// <returns></returns>
        [HttpGet("/api/CardHolder/ObtenerPersona/{documento}/{ssno}")]
        public async Task<object> ObtenerPersona(string documento, string ssno)
        {
            try 
            {
                return await _cardHolder_REP_LOCAL.ObtenerPersona(documento, ssno);
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
        /// Crea una persona nueva en Lenel
        /// </summary>
        /// <param name="newCardHolder"></param>
        /// <returns></returns>
        [HttpPost("/api/CardHolder/CrearPersona")]
        public async Task<object> CrearPersona([FromBody] AddCardHolder_DTO newCardHolder)
        {
            try { return await _cardHolder_REP_LOCAL.CrearPersona(newCardHolder); } 
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
