using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemasInventarios.Data;
using SistemasInventarios.Models;
using System.Threading.Tasks;

namespace SistemasInventarios.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly InventarioContext _context;

        public ProveedoresController(InventarioContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedores.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }
    }
}
