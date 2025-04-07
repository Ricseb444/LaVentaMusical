using Microsoft.AspNetCore.Mvc;
using Models.Data;
using ProyectoVentaMusical.Data;
using ProyectoVentaMusical.Models;
using System.Diagnostics;

namespace ProyectoVentaMusical.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(/*ILogger<HomeController> logger*/ ApplicationDbContext context)
        {
            //_logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var canciones = _context.Canciones.ToList();
            return View(canciones);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
