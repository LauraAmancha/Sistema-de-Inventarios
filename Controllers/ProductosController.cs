using Microsoft.AspNetCore.Mvc;
using SistemasInventarios.Data;
using SistemasInventarios.Models;

namespace SistemasInventarios.Controllers
{
    public class ProductosController : Controller
    {
        private readonly InventarioContext _context;
        public ProductosController(InventarioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }
    }
}
