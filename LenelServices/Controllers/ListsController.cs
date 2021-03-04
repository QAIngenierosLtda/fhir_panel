using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataConduitManager.Repositories.DTO;
using LenelServices.Repositories.Interfaces;

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly ILists_REP_LOCAL _lists_REP_LOCAL;

        public ListsController(ILists_REP_LOCAL lists_REP_LOCAL)
        {
            _lists_REP_LOCAL = lists_REP_LOCAL;
        }

        // GET: api/Lists
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
    }
}
