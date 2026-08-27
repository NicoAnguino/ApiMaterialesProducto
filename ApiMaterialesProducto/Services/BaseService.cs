using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public abstract class BaseService
{
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Encapsula cualquier dato dentro del DTO con la bandera EsAdmin
    protected RespuestaConsultaDto<T> Responder<T>(T datos)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        // Opción recomendada usando ClaimTypes
        string roleName = user?.FindFirst(ClaimTypes.Role)?.Value;

        return new RespuestaConsultaDto<T>
        {
            //Rol = user?.Identity?.Name ?? "Anónimo",
            Rol = roleName,
            Datos = datos
        };
    }
}