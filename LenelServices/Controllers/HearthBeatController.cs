using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DataConduitManager.Repositories.DTO;
using Microsoft.AspNetCore.Http;
using LenelServices.Repositories.Interfaces;
using LenelServices.Attributes;

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class HearthBeatController : ControllerBase
    {
        #region PROPIEDADES
        private readonly ICardHolder_REP_LOCAL _cardHolder_REP_LOCAL;
        #endregion

        #region CONSTRUCTOR
        public HearthBeatController(ICardHolder_REP_LOCAL cardHolder_REP_LOCAL)
        {
            _cardHolder_REP_LOCAL = cardHolder_REP_LOCAL;
        }
        #endregion

        // GET: api/HearthBeat
        [HttpGet("/api/HearthBeat/GetStatus")]
        public async Task<object> GetStatusAsync()
        {
            try
            {
                var data = await _cardHolder_REP_LOCAL.ObtenerPersona("1", "");

                object result = new
                {
                    sucess = true,
                    status = 200,
                    data = "LenelServices se encuentra en linea"
                };

                return result;
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
