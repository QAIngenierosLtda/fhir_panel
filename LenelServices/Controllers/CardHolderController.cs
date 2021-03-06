﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using LenelServices.Attributes;


namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
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
        [HttpGet("/api/CardHolder/ObtenerPersonaByDocumento/{documento}")]
        public async Task<object> ObtenerPersonaByDocumento(string documento)
        {
            try 
            {
                return await _cardHolder_REP_LOCAL.ObtenerPersona(documento, "");
            }
            catch (Exception ex) 
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return BadRequest(result);
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
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return BadRequest(result);
            }
        }

        ///
        [HttpGet("/api/CardHolder/ObtenerPersonaByNombre/{nombre}/{apellido?}")]
        public async Task<object> ObtenerPersona(string nombre, string apellido)
        {
            try
            {
                return await _cardHolder_REP_LOCAL.ObtenerPersonaByName(nombre,apellido);
            }
            catch (Exception ex)
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return BadRequest(result);
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
            try 
            { 
                return await _cardHolder_REP_LOCAL.CrearPersona(newCardHolder); 
            } 
            catch(Exception ex) 
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return BadRequest(result);
            }
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
            try 
            { 
                return await _cardHolder_REP_LOCAL.ActualizarPersona(cardHolder, idPersona); 
            }
            catch (Exception ex) 
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return BadRequest(result);
            }
        }
    }
}
