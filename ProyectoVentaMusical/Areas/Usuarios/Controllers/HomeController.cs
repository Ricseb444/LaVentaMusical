// CONTROLADOR HOMECONTROLLER
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IActionResult> ObtenerCancion(int id)
        {
            var cancion = await _context.Canciones
                .Include(c => c.CodigoAlbumNavigation)
                    .ThenInclude(a => a.CodigoArtistaNavigation)
                .Where(c => c.CodigoCancion == id)
                .Select(c => new
                {
                    nombreCancion = c.NombreCancion,
                    nombreAlbum = c.CodigoAlbumNavigation.NombreAlbum,
                    nombreArtista = c.CodigoAlbumNavigation.CodigoArtistaNavigation.NombreArtistico,
                    urlVideo = c.LinkVideo
                })
                .FirstOrDefaultAsync();

            if (cancion == null)
            {
                return NotFound();
            }

            return Json(cancion);
        }




    }
}