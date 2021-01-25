using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.Interfaces;
using LenelServices.Repositories.Logic;

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardHolderController : ControllerBase
    {
        private readonly ICardHolder_REP_LOCAL _cardHolder_REP_LOCAL;

        public CardHolderController(ICardHolder_REP_LOCAL cardHolder_REP_LOCAL)
        {
            _cardHolder_REP_LOCAL = cardHolder_REP_LOCAL;
        }

        //// GET: api/CardHolder
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/CardHolder/5
        [HttpGet("/api/CardHolder/ObtenerEmpleado/{idLenel}")]
        public async Task<object> ObtenerEmpleado(string idLenel)
        {
            try { return await _cardHolder_REP_LOCAL.ObtenerEmpleado(idLenel); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("/api/CardHolder/ObtenerVisitante/{idLenel}")]
        public async Task<object> ObtenerVisitante(string idLenel)
        {
            return await _cardHolder_REP_LOCAL.ObtenerVisitante(idLenel);
        }

        // POST: api/CardHolder
        [HttpPost("/api/CardHolder/CrearPersona")]
        public async Task<object> CrearPersona([FromBody] AddCardHolder_DTO newCardHolder)
        {
            return await _cardHolder_REP_LOCAL.CrearPersona(newCardHolder);
        }

        // PUT: api/CardHolder/5
        [HttpPut("/api/CardHolder/ActualizarEmpleado/{idLenel}")]
        public async Task<object> ActualizarEmpleado(string idLenel, [FromBody] UpdateCardHolder_DTO cardHolder)
        {
            try { return await _cardHolder_REP_LOCAL.ActualizarEmpleado(cardHolder, idLenel); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
