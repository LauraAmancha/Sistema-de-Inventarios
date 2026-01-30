using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

// ===================== MODELOS =====================
class Usuario { public int UsuarioId; public string Nombre; public string Password; }
class Producto { public int ProductoId; public string Nombre; public string Descripcion; public decimal Precio; public int Stock; public int UsuarioId; }
class Proveedor { public int ProveedorId; public string Nombre; public string Direccion; public string Telefono; public string Email; public int UsuarioId; }
class OrdenCompra { public int OrdenId; public int ProductoId; public int Cantidad; public DateTime Fecha; public int UsuarioId; }

class Program
{
    static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=InventariosDB;Trusted_Connection=True;";
    static int usuarioId = 0;

    static void Main()
    {
        usuarioId = Login();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== SISTEMA DE INVENTARIOS =====");
            Console.WriteLine("1. Gestionar Productos");
            Console.WriteLine("2. Gestionar Proveedores");
            Console.WriteLine("3. Registrar Órdenes de Compra");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
            string opcion = Console.ReadLine();

            if (opcion == "0") break;

            if (opcion == "1")
            {
                Console.Clear();
                Console.WriteLine("1. Listar");
                Console.WriteLine("2. Agregar");
                Console.WriteLine("3. Editar");
                Console.WriteLine("4. Eliminar");
                Console.WriteLine("0. Volver");
                string op = Console.ReadLine();
                if (op == "1") ListarProductos();
                else if (op == "2") AgregarProducto();
                else if (op == "3") EditarProducto();
                else if (op == "4") EliminarProducto();
            }
            else if (opcion == "2")
            {
                Console.Clear();
                Console.WriteLine("1. Listar");
                Console.WriteLine("2. Agregar");
                Console.WriteLine("0. Volver");
                string op = Console.ReadLine();
                if (op == "1") ListarProveedores();
                else if (op == "2") AgregarProveedor();
            }
            else if (opcion == "3")
            {
                RegistrarOrden();
            }
        }
    }

    // ===================== LOGIN =====================
    static int Login()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== LOGIN =====");
            Console.Write("Usuario: "); string nombre = Console.ReadLine();
            Console.Write("Contraseña: "); string password = Console.ReadLine();
            using var con = new SqlConnection(connectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT usuario_id FROM dbo.Usuarios WHERE nombre=@nombre AND password=@password", con);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@password", password);
            var result = cmd.ExecuteScalar();
            if (result != null) return (int)result;
            Console.WriteLine("Usuario o contraseña incorrectos. Presiona Enter...");
            Console.ReadLine();
        }
    }

    // ===================== PRODUCTOS =====================
    static List<Producto> ObtenerProductos()
    {
        var lista = new List<Producto>();
        using var con = new SqlConnection(connectionString);
        con.Open();
        var cmd = new SqlCommand("SELECT * FROM dbo.Productos WHERE usuario_id=@uid", con);
        cmd.Parameters.AddWithValue("@uid", usuarioId);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(new Producto
            {
                ProductoId = (int)dr["producto_id"],
                Nombre = dr["nombre"].ToString(),
                Descripcion = dr["descripcion"].ToString(),
                Precio = (decimal)dr["precio"],
                Stock = (int)dr["stock"],
                UsuarioId = (int)dr["usuario_id"]
            });
        }
        return lista;
    }

    static Producto ObtenerProducto(int id) => ObtenerProductos().Find(p => p.ProductoId == id);

    static void ListarProductos()
    {
        Console.Clear();
        Console.WriteLine("=== LISTA PRODUCTOS ===");
        foreach (var p in ObtenerProductos())
            Console.WriteLine($"{p.ProductoId} - {p.Nombre} - Stock:{p.Stock} - Precio:{p.Precio}");
        Console.WriteLine("Presiona Enter...");
        Console.ReadLine();
    }

    static void AgregarProducto()
    {
        Console.Clear();
        Producto p = new Producto();
        Console.Write("Nombre: "); p.Nombre = Console.ReadLine();
        Console.Write("Descripcion: "); p.Descripcion = Console.ReadLine();
        Console.Write("Precio: "); p.Precio = decimal.Parse(Console.ReadLine());
        Console.Write("Stock: "); p.Stock = int.Parse(Console.ReadLine());
        p.UsuarioId = usuarioId;

        using var con = new SqlConnection(connectionString);
        con.Open();
        var cmd = new SqlCommand("INSERT INTO dbo.Productos (nombre,descripcion,precio,stock,usuario_id) VALUES (@nombre,@desc,@precio,@stock,@uid)", con);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@stock", p.Stock);
        cmd.Parameters.AddWithValue("@uid", p.UsuarioId);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Producto agregado! Presiona Enter...");
        Console.ReadLine();
    }

    static void EditarProducto()
    {
        Console.Clear();
        Console.Write("ID del producto a editar: "); int id = int.Parse(Console.ReadLine());
        var p = ObtenerProducto(id);
        if (p == null) { Console.WriteLine("Producto no encontrado"); Console.ReadLine(); return; }

        Console.Write("Nombre: "); p.Nombre = Console.ReadLine();
        Console.Write("Descripcion: "); p.Descripcion = Console.ReadLine();
        Console.Write("Precio: "); p.Precio = decimal.Parse(Console.ReadLine());
        Console.Write("Stock: "); p.Stock = int.Parse(Console.ReadLine());

        using var con = new SqlConnection(connectionString);
        con.Open();
        var cmd = new SqlCommand("UPDATE dbo.Productos SET nombre=@nombre, descripcion=@desc, precio=@precio, stock=@stock WHERE producto_id=@id AND usuario_id=@uid", con);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@stock", p.Stock);
        cmd.Parameters.AddWithValue("@id", p.ProductoId);
        cmd.Parameters.AddWithValue("@uid", usuarioId);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Producto editado! Presiona Enter...");
        Console.ReadLine();
    }

    static void EliminarProducto()
    {
        Console.Clear();
        Console.Write("ID del producto a eliminar: "); int id = int.Parse(Console.ReadLine());
        using var con = new SqlConnection(connectionString);
        con.Open();
        var cmd = new SqlCommand("DELETE FROM dbo.Productos WHERE producto_id=@id AND usuario_id=@uid", con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@uid", usuarioId);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Producto eliminado! Presiona Enter...");
        Console.ReadLine();
    }

    // ===================== PROVEEDORES =====================
    static List<Proveedor> ObtenerProveedores()
    {
        var lista = new List<Proveedor>();
        using var con = new SqlConnection(connectionString);
        con.Open();
        var cmd = new SqlCommand("SELECT * FROM dbo.Proveedores WHERE usuario_id=@uid", con);
        cmd.Parameters.AddWithValue("@uid", usuarioId);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(new Proveedor
            {
                ProveedorId = (int)dr["proveedor_id"],
                Nombre = dr["nombre"].ToString(),
                Direccion = dr["direccion"].ToString(),
                Telefono = dr["telefono"].ToString(),
                Email = dr["email"].ToString(),
                UsuarioId = (int)dr["usuario_id"]
            });
        }
        return lista;
    }

    static Proveedor ObtenerProveedor(int id) => ObtenerProveedores().Find(p => p.ProveedorId == id);

    static void ListarProveedores()
    {
        Console.Clear();
        Console.WriteLine("=== LISTA PROVEEDORES ===");
        foreach (var p in ObtenerProveedores())
            Console.WriteLine($"{p.ProveedorId} - {p.Nombre} - {p.Direccion} - {p.Telefono} - {p.Email}");
        Console.WriteLine("Presiona Enter...");
        Console.ReadLine();
    }

    static void AgregarProveedor()
    {
        Console.Clear();
        Proveedor p = new Proveedor();
        Console.Write("Nombre: "); p.Nombre = Console.ReadLine();
        Console.Write("Direccion: "); p.Direccion = Console.ReadLine();
        Console.Write("Telefono: "); p.Telefono = Console.ReadLine();
        Console.Write("Email: "); p.Email = Console.ReadLine();
        p.UsuarioId = usuarioId;

        using var con = new SqlConnection(connectionString);
        con.Open();
        var cmd = new SqlCommand("INSERT INTO dbo.Proveedores (nombre,direccion,telefono,email,usuario_id) VALUES (@nombre,@direccion,@telefono,@email,@uid)", con);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@direccion", p.Direccion);
        cmd.Parameters.AddWithValue("@telefono", p.Telefono);
        cmd.Parameters.AddWithValue("@email", p.Email);
        cmd.Parameters.AddWithValue("@uid", usuarioId);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Proveedor agregado! Presiona Enter...");
        Console.ReadLine();
    }

    // ===================== ORDENES DE COMPRA =====================
    static void RegistrarOrden()
    {
        Console.Clear();
        OrdenCompra o = new OrdenCompra();
        Console.Write("ProductoId: "); o.ProductoId = int.Parse(Console.ReadLine());
        Console.Write("Cantidad: "); o.Cantidad = int.Parse(Console.ReadLine());
        o.Fecha = DateTime.Now;
        o.UsuarioId = usuarioId;

        using var con = new SqlConnection(connectionString);
        con.Open();

        var cmd = new SqlCommand("INSERT INTO dbo.OrdenesCompra (producto_id,cantidad,fecha,usuario_id) VALUES (@prod,@cant,@fecha,@uid)", con);
        cmd.Parameters.AddWithValue("@prod", o.ProductoId);
        cmd.Parameters.AddWithValue("@cant", o.Cantidad);
        cmd.Parameters.AddWithValue("@fecha", o.Fecha);
        cmd.Parameters.AddWithValue("@uid", usuarioId);
        cmd.ExecuteNonQuery();

        var p = ObtenerProducto(o.ProductoId);
        if (p != null)
        {
            p.Stock -= o.Cantidad;
            var cmd2 = new SqlCommand("UPDATE dbo.Productos SET stock=@stock WHERE producto_id=@id AND usuario_id=@uid", con);
            cmd2.Parameters.AddWithValue("@stock", p.Stock);
            cmd2.Parameters.AddWithValue("@id", p.ProductoId);
            cmd2.Parameters.AddWithValue("@uid", usuarioId);
            cmd2.ExecuteNonQuery();
            Console.WriteLine("Orden registrada y stock actualizado!");
        }
        else Console.WriteLine("Producto no encontrado!");

        Console.WriteLine("Presiona Enter...");
        Console.ReadLine();
    }
}
