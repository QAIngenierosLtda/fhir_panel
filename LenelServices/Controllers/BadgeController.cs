using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.Interfaces;
using LenelServices.Repositories.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using LenelServices.Attributes;

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BadgeController : ControllerBase
    {
        private readonly IBadge_REP_LOCAL _badge_REP_LOCAL;

        public BadgeController(IBadge_REP_LOCAL badge_REP_LOCAL)
        {
            _badge_REP_LOCAL = badge_REP_LOCAL;
        }


        // POST: api/Badge
        [HttpPost("/api/Badge/CrearBadge")]
        public async Task<object> CrearBadge([FromBody] AddBadge_DTO newBadge)
        {
            try 
            { 
                object result = new
                {
                    success = true,
                    status = 200,
                    data = await _badge_REP_LOCAL.CrearBadge(newBadge)
                };
                return result;
            }
            catch (Exception ex) 
            {
                object result = new
                {
                    success = false,
                    status = 400,
                    data = ex.Message
                };

                return BadRequest(result);
            }
        }

        // PUT: api/Badge/5
        [HttpPut("/api/Badge/CambiarEstadoBadge/{idStatus}")]
        public async Task<object> CambiarEstadoBadge(string idStatus, [FromBody] SetBadgeStatus_DTO nuevoStatus)
        {
            try
            {
                object result = new
                {
                    success = await _badge_REP_LOCAL.ActualizarEstadoBadge(idStatus, nuevoStatus),
                    status = 200,
                    data = "Done"
                };
                return result;
            }
            catch (Exception ex)
            {
                object result = new
                {
                    success = false,
                    status = 400,
                    data = ex.Message
                };

                return BadRequest(result);
            }
        }
    }
}
