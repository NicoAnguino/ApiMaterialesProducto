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
            EsExitoso = true,
            Mensaje = "",
            Datos = datos
        };
    }

    protected RespuestaConsultaDto<T> ResponderError<T>(string mensaje)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        // Opción recomendada usando ClaimTypes
        string roleName = user?.FindFirst(ClaimTypes.Role)?.Value;

        return new RespuestaConsultaDto<T>
        {
            Rol = roleName,
            EsExitoso = false,
            Mensaje = mensaje,
            Datos = default
        };
    }
}