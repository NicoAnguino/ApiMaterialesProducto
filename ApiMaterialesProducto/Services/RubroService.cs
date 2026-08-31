using ApiMaterialesProducto.Models;
using Microsoft.EntityFrameworkCore;

public interface IRubroService
{
    Task<RespuestaConsultaDto<List<RubroDto>>> ObtenerRubrosAsync();
    Task<RespuestaConsultaDto<RubroDto>> ObtenerRubroPorIdAsync(int id);
    Task<RespuestaConsultaDto<RubroDto>> CrearRubroAsync(RubroDto rubroDto);
    Task<RespuestaConsultaDto<RubroDto>> EditarRubroAsync(int id, RubroDto rubroDto);
    Task<RespuestaConsultaDto<bool>> EliminarRubroAsync(int id);
}

public class RubroService : BaseService, IRubroService
{
    private readonly ApplicationDbContext _context;

    public RubroService(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _context = context;
    }

    public async Task<RespuestaConsultaDto<List<RubroDto>>> ObtenerRubrosAsync()
    {
        var alumnos = await _context.Rubros
        .Where(r => !r.Eliminado)
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

        // 1. Buscar la entidad existente de forma asíncrona
        var rubro = await _context.Rubros.FirstOrDefaultAsync(r => r.RubroID == id);

        if (rubro == null)
        {
            return ResponderError<RubroDto>("El rubro especificado no existe.");
        }

        // 2. Validar que la nueva descripción no pertenezca a OTRO rubro
        var existe = await _context.Rubros.AnyAsync(t => t.Descripcion == rubroDto.Descripcion && t.RubroID != rubroDto.RubroID);

        if (existe)
        {
            // Retorna tu DTO de respuesta indicando el error sin usar sintaxis HTTP
            return ResponderError<RubroDto>("Ya existe un rubro con esa descripción.");
        }

        rubro.Descripcion = rubroDto.Descripcion;
        await _context.SaveChangesAsync();

        return Responder(rubroDto);
    }

    public async Task<RespuestaConsultaDto<bool>> EliminarRubroAsync(int id)
    {
        var rubro = await _context.Rubros.FirstOrDefaultAsync(r => r.RubroID == id && !r.Eliminado);

        if (rubro == null)
        {
            return ResponderError<bool>("El rubro especificado no existe o ya fue eliminado.");
        }

        // Opcional: Validar si el rubro se está usando en otras tablas activas antes de desactivarlo
        var estaEnUso = await _context.Material.AnyAsync(p => p.RubroID == id && !p.Eliminado);
        if (estaEnUso)
        {
            return ResponderError<bool>("No se puede desactivar el rubro porque tiene productos asociados.");
        }

        // Inactivar el registro en lugar de removerlo
        rubro.Eliminado = true;
        await _context.SaveChangesAsync();

        return Responder(true);
    }
}