using ApiMaterialesProducto.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)

        : base(options)

    {

    }

    // Agrega tus DbSet aquí
    public DbSet<Rubro> Rubros { get; set; }
    public DbSet<Material> Material { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<MaterialProducto> MaterialesProductos { get; set; }
}