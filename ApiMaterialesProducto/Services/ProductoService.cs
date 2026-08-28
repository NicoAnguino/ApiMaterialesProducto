using ApiMaterialesProducto.Models;
using Microsoft.EntityFrameworkCore;

public interface IProductoService
{
    Task<RespuestaConsultaDto<List<RubroDto>>> ObtenerRubrosAsync();
    Task<RespuestaConsultaDto<RubroDto>> ObtenerRubroPorIdAsync(int id);
    Task<RespuestaConsultaDto<RubroDto>> CrearRubroAsync(RubroDto rubroDto);
    Task<RespuestaConsultaDto<RubroDto>> EditarRubroAsync(int id, RubroDto rubroDto);
}

public class ProductoService : BaseService, IProductoService
{
    private readonly ApplicationDbContext _context;

    public ProductoService(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _context = context;
    }

    public async Task<RespuestaConsultaDto<List<RubroDto>>> ObtenerRubrosAsync()
    {
        var alumnos = await _context.Rubros
            .Select(a => new RubroDto
            {
                RubroID = a.RubroID,
                Descripcion = a.Descripcion,
                Eliminado = a.Eliminado
            })
            .OrderBy(n => n.Descripcion)
            .ToListAsync();

        // Responder() adjunta automáticamente Rol = "valor"
        return Responder(alumnos);
    }

    public async Task<RespuestaConsultaDto<RubroDto>> ObtenerRubroPorIdAsync(int id)
    {
        var rubro = await _context.Rubros
            .Where(a => a.RubroID == id)
            .Select(a => new RubroDto
            {
                RubroID = a.RubroID,
                Descripcion = a.Descripcion,
                Eliminado = a.Eliminado
            })
            .FirstOrDefaultAsync();

        return Responder(rubro);
    }

    public async Task<RespuestaConsultaDto<RubroDto>> CrearRubroAsync(RubroDto rubroDto)
    {
        if (!string.IsNullOrEmpty(rubroDto.Descripcion))
        {
            rubroDto.Descripcion = rubroDto.Descripcion?.ToUpper();
        }

        var existe = await _context.Rubros.AnyAsync(t => t.Descripcion == rubroDto.Descripcion);

        if (existe)
        {
            // Retorna tu DTO de respuesta indicando el error sin usar sintaxis HTTP
            return ResponderError<RubroDto>("Ya existe un rubro con esa descripción.");
        }

        var rubro = new Rubro
        {
            Descripcion = rubroDto.Descripcion
        };
        _context.Rubros.Add(rubro);
        await _context.SaveChangesAsync();

        rubroDto.RubroID = rubro.RubroID;

        return Responder(rubroDto);
    }

    public async Task<RespuestaConsultaDto<RubroDto>> EditarRubroAsync(int id, RubroDto rubroDto)
    {
        if (!string.IsNullOrEmpty(rubroDto.Descripcion))
        {
            rubroDto.Descripcion = rubroDto.Descripcion?.ToUpper();
        }

        var existe = await _context.Rubros.AnyAsync(t => t.Descripcion == rubroDto.Descripcion && t.RubroID != rubroDto.RubroID);

        if (existe)
        {
            // Retorna tu DTO de respuesta indicando el error sin usar sintaxis HTTP
            return ResponderError<RubroDto>("Ya existe un rubro con esa descripción.");
        }

        var rubro = _context.Rubros.Where(r => r.RubroID == id).SingleOrDefault();
        if (rubro != null)
        {
            rubro.Descripcion = rubroDto.Descripcion;
            await _context.SaveChangesAsync();
        }
       
        return Responder(rubroDto);
    }
}