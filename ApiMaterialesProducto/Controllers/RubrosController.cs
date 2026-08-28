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
        private readonly IProductoService _academicoService;

        public RubrosController(IProductoService academicoService)
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
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutRubro(int id, Rubro rubro)
        // {
        //     if (id != rubro.RubroID)
        //     {
        //         return BadRequest();
        //     }

        //     if (!string.IsNullOrEmpty(rubro.Descripcion))
        //     {
        //         rubro.Descripcion = rubro.Descripcion?.ToUpper();
        //     }

        //     var rubroExiste = await _context.Rubros.Where(t => t.Descripcion == rubro.Descripcion && t.RubroID != rubro.RubroID).FirstOrDefaultAsync();

        //     if (rubroExiste != null)
        //     {
        //         return Conflict(new { mensaje = "Ya existe un rubro con esa descripción." });
        //     }

        //     _context.Entry(rubro).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!RubroExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

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

        // // DELETE: api/Rubros/5 esta seccion del aplicativo no se usa el delete
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteRubro(int id)
        // {
        //     var rubro = await _context.Rubros.FindAsync(id);
        //     if (rubro == null)
        //     {
        //         return NotFound();
        //     }
        //     rubro.Eliminado = true;
        //     await _context.SaveChangesAsync();

        //     return Ok();
        // }

        // private bool RubroExists(int id)
        // {
        //     return _context.Rubros.Any(e => e.RubroID == id);
        // }
    }
}
