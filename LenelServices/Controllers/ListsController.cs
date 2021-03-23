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

        [HttpGet("/api/Lists/ListarInstalaciones/{instalacionId?}")]
        public async Task<object> ListarInstalaciones(int? instalacionId)
        {
            try
            {
                return await _lists_REP_LOCAL.ListarInstalaciones(instalacionId);
            }
            catch (Exception ex)
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return result;
            }
        }

        [HttpGet("/api/Lists/ListarAreas/{areaId?}")]
        public async Task<object> ListarDivision(int? areaId)
        {
            try
            {
                return await _lists_REP_LOCAL.ListarDivisiones(areaId);
            }
            catch (Exception ex)
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return result;
            }
        }

        [HttpGet("/api/Lists/ListarCiudades/{ciudadId?}")]
        public async Task<object> ListarCiudad(int? ciudadId)
        {
            try
            {
                return await _lists_REP_LOCAL.ListarCiudades(ciudadId);
            }
            catch (Exception ex)
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return result;
            }
        }

        [HttpGet("/api/Lists/ListarEmpresas/{empresaId?}")]
        public async Task<object> ListarEmpresa(int? empresaId)
        {
            try
            {
                return await _lists_REP_LOCAL.ListarEmpresas(empresaId);
            }
            catch (Exception ex)
            {
                object result = new
                {
                    sucess = false,
                    status = 400,
                    data = ex.Message
                };

                return result;
            }
        }
    }
}
