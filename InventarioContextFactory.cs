using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemasInventarios.Data
{
    public class InventarioContextFactory : IDesignTimeDbContextFactory<InventarioContext>
    {
        public InventarioContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InventarioContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SistemasInventariosDB;Trusted_Connection=True;");

            return new InventarioContext(optionsBuilder.Options);
        }
    }
}
