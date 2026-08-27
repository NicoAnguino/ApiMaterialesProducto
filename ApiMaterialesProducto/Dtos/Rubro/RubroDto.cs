// Dtos/RubroDto.cs (Datos específicos que enviamos a la pantalla)
public class RubroDto
{
    public int RubroID { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public bool Eliminado { get; set; }
}