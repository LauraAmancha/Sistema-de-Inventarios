using Microsoft.AspNetCore.Mvc;

namespace SistemasInventarios.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
