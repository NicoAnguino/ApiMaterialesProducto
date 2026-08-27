// Dtos/RespuestaConsultaDto.cs (Estructura genérica de respuesta)
public class RespuestaConsultaDto<T>
{
    public string Rol { get; set; } = ""; // Le dice al JS si puede mostrar botones de edición
    public T Datos { get; set; } = default!;
}