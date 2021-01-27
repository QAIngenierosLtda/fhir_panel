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

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BadgeController : ControllerBase
    {
        private readonly IBadge_REP_LOCAL _badge_REP_LOCAL;

        public BadgeController(IBadge_REP_LOCAL badge_REP_LOCAL)
        {
            _badge_REP_LOCAL = badge_REP_LOCAL;
        }

        // GET: api/Badge
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Badge/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Badge
        [HttpPost("/api/Badge/CrearBadge")]
        public async Task<object> CrearBadge([FromBody] AddBadge_DTO newBadge)
        {
            return await _badge_REP_LOCAL.CrearBadge(newBadge);
        }

        // PUT: api/Badge/5
        [HttpPut("/api/Badge/CambiarEstadoBadge/{idStatus}")]
        public async Task<object> CambiarEstadoBadge(string idStatus, [FromBody] SetBadgeStatus_DTO nuevoStatus)
        {
            try
            {
                return await _badge_REP_LOCAL.ActualizarEstadoBadge(idStatus, nuevoStatus);
            }
            catch (Exception ex)
            {
                return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
