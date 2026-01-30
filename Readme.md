# Sistema de Inventario - .NET

Este proyecto es un **Sistema de Gestión de Inventarios** desarrollado en **.NET 10** con C# y SQL Server.  
Permite gestionar **Productos, Proveedores** y registrar **Órdenes de Compra** para usuarios individuales.

---

## 🗂 Estructura del Proyecto

- `Program.cs` → Contiene la lógica principal del sistema y el menú.  
- `Models` → Clases de las tablas: `Producto`, `Proveedor`, `OrdenCompra`, `Usuario`.  
- `*.csproj` → Archivo de proyecto .NET.  

---

## 💾 Base de Datos

Se requiere **SQL Server**. La base de datos se llama `InventariosDB` y contiene las siguientes tablas:

- `Usuarios` (usuario_id, nombre, password)  
- `Productos` (producto_id, nombre, descripcion, precio, stock, usuario_id)  
- `Proveedores` (proveedor_id, nombre, direccion, telefono, email, usuario_id)  
- `OrdenesCompra` (orden_id, producto_id, cantidad, fecha, usuario_id)  

### ⚡ Crear base de datos y tablas

Ejecuta el siguiente script en **SQL Server Management Studio**:

```sql
-- Crear base de datos (si no existe)
CREATE DATABASE InventariosDB;
GO
USE InventariosDB;
GO

-- Usuarios
CREATE TABLE dbo.Usuarios (
    usuario_id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL,
    password NVARCHAR(50) NOT NULL
);
INSERT INTO dbo.Usuarios (nombre, password) VALUES ('admin', '1234');

-- Proveedores
CREATE TABLE dbo.Proveedores (
    proveedor_id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    direccion NVARCHAR(200),
    telefono NVARCHAR(20),
    email NVARCHAR(100),
    usuario_id INT NOT NULL FOREIGN KEY REFERENCES dbo.Usuarios(usuario_id)
);

-- Productos
CREATE TABLE dbo.Productos (
    producto_id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(200),
    precio DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL,
    usuario_id INT NOT NULL FOREIGN KEY REFERENCES dbo.Usuarios(usuario_id)
);

-- Ordenes de Compra
CREATE TABLE dbo.OrdenesCompra (
    orden_id INT IDENTITY(1,1) PRIMARY KEY,
    producto_id INT NOT NULL FOREIGN KEY REFERENCES dbo.Productos(producto_id),
    cantidad INT NOT NULL,
    fecha DATETIME NOT NULL,
    usuario_id INT NOT NULL FOREIGN KEY REFERENCES dbo.Usuarios(usuario_id)
);
