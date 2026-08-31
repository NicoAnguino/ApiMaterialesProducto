using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMaterialesProducto.Models;
//using ApiMaterialesProducto.ModelsView;
using Microsoft.AspNetCore.Authorization;

namespace ApiMaterialesProducto.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RubrosController : ControllerBase
    {
        private readonly IRubroService _academicoService;

        public RubrosController(IRubroService academicoService)
        {
            _academicoService = academicoService;
        }

        // GET: api/Rubros
        [HttpGet]
        public async Task<IActionResult> GetRubro()
        {
            var resultado = await _academicoService.ObtenerRubrosAsync();

            return Ok(resultado);
        }

        //GET: api/Rubros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rubro>> GetRubro(int id)
        {
            var resultado = await _academicoService.ObtenerRubroPorIdAsync(id);

            return Ok(resultado);
        }

        // // PUT: api/Rubros/5
        [HttpPut("{id}")]
        public async Task<ActionResult<RespuestaConsultaDto<RubroDto>>> PutRubro(int id, RubroDto rubroDto)
        {
            if (id != rubroDto.RubroID)
            {
                return BadRequest();
            }

            var resultado = await _academicoService.EditarRubroAsync(id, rubroDto);

            // Si hubo un error de validación o duplicado en el servicio
            if (!resultado.EsExitoso) 
            {
                return BadRequest(resultado); // o Conflict(resultado)
            }

             return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaConsultaDto<RubroDto>>> PostRubro(RubroDto rubroDto)
        {
            var resultado = await _academicoService.CrearRubroAsync(rubroDto);

            // Si hubo un error de validación o duplicado en el servicio
            if (!resultado.EsExitoso) 
            {
                return BadRequest(resultado); // o Conflict(resultado)
            }

            return CreatedAtAction("GetRubro", new { id = resultado.Datos.RubroID }, resultado);
        }

        // DELETE: api/Rubros/5 esta seccion del aplicativo no se usa el delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRubro(int id)
        {
            var resultado = await _academicoService.EliminarRubroAsync(id);

            return Ok(resultado);
        }
    }
}
