using Microsoft.EntityFrameworkCore;

public interface IProductoService
{
    Task<RespuestaConsultaDto<List<RubroDto>>> ObtenerRubrosAsync();
    Task<RespuestaConsultaDto<RubroDto>> ObtenerRubroPorIdAsync(int id);
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
            .Select(a => new RubroDto {
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
        .Select(a => new RubroDto {
            RubroID = a.RubroID,
            Descripcion = a.Descripcion,
            Eliminado = a.Eliminado
        })
        .FirstOrDefaultAsync();

    return Responder(rubro);
}
}