using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataConduitManager.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using DataConduitManager.Repositories.DTO;
using Microsoft.AspNetCore.Http;
using LenelServices.Attributes;

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class HearthBeatController : ControllerBase
    {
        #region PROPIEDADES
        #endregion

        #region CONSTRUCTOR
        public HearthBeatController()
        {

        }
        #endregion

        // GET: api/HearthBeat
        [HttpGet("/api/HearthBeat/GetStatus")]
        public async Task<object> GetStatusAsync()
        {
            object result = new { 
                sucess = true, 
                status = 200,
                data = "LenelServices se encuentra en linea" 
            };

            return result;
        }
    }
}
