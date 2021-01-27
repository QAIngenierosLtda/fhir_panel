using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LenelServices.Repositories.DTO;
using LenelServices.Repositories.Interfaces;
using DataConduitManager.Repositories.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace LenelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly IReader_REP_LOCAL _reader_REP_LOCAL;
        enum tipoEvento : ushort
        {
            IB, //INGRESO BIOMETRICO
            IBNI,  //INGRESO BIOMETRICO PERSONA NO IDENTIFICADA
            SB,  //SALIDA BIOMETRICO
            SBNI  //SALIDA BIOMETRICO PERSONA NO IDENTIFICADA
        }

        public ReaderController(IReader_REP_LOCAL reader_REP_LOCAL)
        {
            _reader_REP_LOCAL = reader_REP_LOCAL;
        }

        //GET: api/Reader/5
        [HttpGet("/api/Reader/configLectora/{panelID}/{readerID}")]
        public async Task<object> ConfiguracionLectora(string panelID, string readerID)
        {
            try
            {
                return await _reader_REP_LOCAL.ConfiguracionLectora(panelID, readerID);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // POST: /api/Reader/AbrirPuerta
        [HttpPost("/api/Reader/AbrirPuerta")]
        public async Task<object> AbrirPuerta([FromBody] ReaderPath_DTO pathReader)
        {
            try {
                return await _reader_REP_LOCAL.AbrirPuerta(pathReader);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("/api/Reader/BloquearPuerta")]
        public async Task<object> BloquearPuerta([FromBody] ReaderPath_DTO pathReader)
        {
            try
            {
                return await _reader_REP_LOCAL.BloquearPuerta(pathReader);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Cambio de modo de acceso de una lectora
        ///    LOCKED = 0,
        ///    CARDONLY = 1,
        ///    PIN_OR_CARD = 2,
        ///    PIN_AND_CARD = 3, 
        ///    UNLOCKED = 4,
        ///    FACCODE_ONLY = 5,
        ///    CYPHERLOCK = 6,
        ///    AUTOMATIC = 7,
        ///    DEFAULT = 100
        /// </summary>
        /// <param name="pathReader"></param>
        /// <param name="estadoId"></param>
        /// <returns></returns>
        [HttpPut("/api/Reader/CambiarModoLectora/{estadoId}")]
        public async Task<object> CambiarEstadoPuerta([FromBody] ReaderPath_DTO pathReader, int estadoId)
        {
            try
            {
                return await _reader_REP_LOCAL.CambioEstadoPuerta(pathReader, estadoId);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("/api/Reader/EnviarEvento")]
        public async Task<object> EnviarEvento([FromBody] SendEvent_DTO evento)
        {
            try
            {
                return await _reader_REP_LOCAL.EnviarEventoGenerico(evento);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("/api/Reader/AutorizacionIngreso")]
        public async Task<object> AutorizarIngreso([FromBody] SendEvent_DTO evento)
        {
            try
            {
                EvaluacionEvento_DTO eval = new EvaluacionEvento_DTO();

                if (evento.badgeID != null)
                    eval = GetDescripcion(tipoEvento.IB, evento);
                else
                    eval = GetDescripcion(tipoEvento.IBNI, evento);

                evento.description = eval.descripcionEvento;
                bool enviado = await _reader_REP_LOCAL.EnviarEventoGenerico(evento);

                if (enviado)
                    return eval;
                else
                    throw new Exception("No se pudo enviar el evento");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("/api/Reader/AutorizacionSalida")]
        public async Task<object> AutorizarSalida([FromBody] SendEvent_DTO evento)
        {
            try
            {
                EvaluacionEvento_DTO eval = new EvaluacionEvento_DTO();
                if (evento.badgeID != null)
                    eval = GetDescripcion(tipoEvento.SB, evento);
                else
                    eval = GetDescripcion(tipoEvento.SBNI, evento);

                evento.description = eval.descripcionEvento;
                bool enviado = await _reader_REP_LOCAL.EnviarEventoGenerico(evento);

                if (enviado)
                    return eval;
                else
                    throw new Exception("No se pudo enviar el evento");

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Devuelve resultado del analisis de temperatura y tapabocas
        /// </summary>
        /// <param name="tipo">
        /// 0-Ingreso Biometrico, 1-Ingreso Biometrico No Identificado,
        /// 2-Salida Biometrico, 3-Salida Biometrico No identificado
        /// </param>
        /// <param name="evento"></param>
        /// <returns></returns>
        private EvaluacionEvento_DTO GetDescripcion(tipoEvento tipo, SendEvent_DTO evento)
        {
            List<string> descripcion = new List<string>
            {
                tipo.ToString(), //NOMBRE DEL EVENTO
                (bool)evento.tapabocas ? "0" : "1", //ALERTA DE TAPABOCAS
                (evento.temperatura <= evento.tempRef) ? "0" : "1", //ALERTA DE TEMPERATURA
            };

            return new EvaluacionEvento_DTO
            {
                descripcionEvento = descripcion[0] + "|" + descripcion[1] + "|" + descripcion[2] +
                    "|" + evento.badgeID.ToString() + "|" + evento.temperatura.ToString() +
                    "|" + evento.tempRef.ToString(),
                alarmaEvento = (descripcion[1] == "1" || descripcion[2] == "1")
            };
        }
    }
}
