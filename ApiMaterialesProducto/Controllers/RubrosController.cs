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
        private readonly ApplicationDbContext _context;

        public RubrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rubros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rubro>>> GetRubro()
        {
            //List<VistaRubro> vistaRubros = new List<VistaRubro>();

            var rubros = await _context.Rubros.Where(a => a.Eliminado == false).OrderBy(n => n.Descripcion).ToListAsync();

            // foreach (var Rubro in Rubros)
            // {
            //     var elemento = new VistaRubro
            //     {
            //         RubroID = Rubro.RubroID,
            //         Descripcion = Rubro.Descripcion,
            //         Eliminado = Rubro.Eliminado
            //     };
            //     vistaRubros.Add(elemento);
            // }

            return rubros;
        }

        // GET: api/Rubros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rubro>> GetRubro(int id)
        {
            var rubro = await _context.Rubros.FindAsync(id);

            if (rubro == null)
            {
                return NotFound();
            }

            return rubro;
        }



        // PUT: api/Rubros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRubro(int id, Rubro rubro)
        {
            if (id != rubro.RubroID)
            {
                return BadRequest();
            }

            if (!string.IsNullOrEmpty(rubro.Descripcion))
            {
                rubro.Descripcion = rubro.Descripcion?.ToUpper();
            }

            var rubroExiste = await _context.Rubros.Where(t => t.Descripcion == rubro.Descripcion && t.RubroID != rubro.RubroID).FirstOrDefaultAsync();

            if (rubroExiste != null)
            {
                return Conflict(new { mensaje = "Ya existe un rubro con esa descripción." });
            }

            _context.Entry(rubro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RubroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Rubro>> PostRubro(Rubro rubro)
        {

            if (!string.IsNullOrEmpty(rubro.Descripcion))
            {
                rubro.Descripcion = rubro.Descripcion?.ToUpper();
            }

            var rubroExiste = await _context.Rubros.Where(t => t.Descripcion == rubro.Descripcion).FirstOrDefaultAsync();

            if (rubroExiste != null)
            {
                return Conflict(new { mensaje = "Ya existe un rubro con esa descripción." });
            }

            _context.Rubros.Add(rubro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRubro", new { id = rubro.RubroID }, rubro);
        }

        // DELETE: api/Rubros/5 esta seccion del aplicativo no se usa el delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRubro(int id)
        {
            var rubro = await _context.Rubros.FindAsync(id);
            if (rubro == null)
            {
                return NotFound();
            }
            rubro.Eliminado = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool RubroExists(int id)
        {
            return _context.Rubros.Any(e => e.RubroID == id);
        }
    }
}
