using System.ComponentModel.DataAnnotations;

namespace ApiMaterialesProducto.Models
{
    public class Producto
    {
        [Key]
        public int ProductoID { get; set; }
        public string? Descripcion { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool Eliminado {get; set; }

        public virtual ICollection<MaterialProducto>? MaterialesProducto {get; set; }
    }
}