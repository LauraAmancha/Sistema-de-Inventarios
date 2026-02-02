namespace SistemasInventarios.Models
{
    public class OrdenCompra
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int ProveedorId { get; set; }
        public int Cantidad { get; set; }

        public Producto Producto { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
