using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemasInventarios.Data;
using SistemasInventarios.Models;
using System.Threading.Tasks;

namespace SistemasInventarios.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly InventarioContext _context;

        public OrdenesController(InventarioContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _context.OrdenesCompras
                .Include(o => o.Producto)
                .Include(o => o.Proveedor)
                .ToListAsync();
            return View(ordenes);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Productos = await _context.Productos.ToListAsync();
            ViewBag.Proveedores = await _context.Proveedores.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrdenCompra orden)
        {
            if (ModelState.IsValid)
            {
                _context.OrdenesCompras.Add(orden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Productos = await _context.Productos.ToListAsync();
            ViewBag.Proveedores = await _context.Proveedores.ToListAsync();
            return View(orden);
        }
    }
}
