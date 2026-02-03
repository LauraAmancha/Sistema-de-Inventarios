using Microsoft.EntityFrameworkCore;
using SistemasInventarios.Models;

namespace SistemasInventarios.Data
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(DbContextOptions<InventarioContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<OrdenCompra> OrdenesCompras { get; set; }
    }
}
