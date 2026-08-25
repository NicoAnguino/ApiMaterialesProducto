using System.ComponentModel.DataAnnotations;

namespace ApiMaterialesProducto.Models
{
    public class Rubro
    {
        [Key]
        public int RubroID { get; set; }
        public string? Descripcion { get; set; }
        public bool Eliminado {get; set; }

        public virtual ICollection<Material>? Materiales {get; set; }
    }
}