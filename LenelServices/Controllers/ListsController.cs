using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class ListsController : ControllerBase
    {
        private readonly ILists_REP_LOCAL _lists_REP_LOCAL;

        public ListsController(ILists_REP_LOCAL lists_REP_LOCAL)
        {
            _lists_REP_LOCAL = lists_REP_LOCAL;
        }

        // GET: api/Lists

        [HttpGet("/api/Lists/ListarInstalaciones")]
        public async Task<object> ListarInstalaciones()
        {
            try
            {
                return await _lists_REP_LOCAL.ListarInstalaciones();
            }
            catch (Exception ex)
            {
                return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }

        [HttpGet("/api/Lists/ListarDivisiones")]
        public async Task<object> ListarDivision()
        {
            try
            {
                return await _lists_REP_LOCAL.ListarDivisiones();
            }
            catch (Exception ex)
            {
                return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }

        [HttpGet("/api/Lists/ListarCiudades")]
        public async Task<object> ListarCiudad()
        {
            try
            {
                return await _lists_REP_LOCAL.ListarCiudades();
            }
            catch (Exception ex)
            {
                return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }

        [HttpGet("/api/Lists/ListarEmpresas")]
        public async Task<object> ListarEmpresa()
        {
            try
            {
                return await _lists_REP_LOCAL.ListarEmpresas();
            }
            catch (Exception ex)
            {
                return BadRequest("error: " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }
    }
}
