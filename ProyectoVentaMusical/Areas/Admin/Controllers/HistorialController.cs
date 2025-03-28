using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Data;
using Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoVentaMusical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HistorialController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HistorialController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> HistorialCompras()
        {
            var userID = _userManager.GetUserId(User);

            var ventas = await _context.Ventas
                .Where(v => v.IdUsuario == userID)
                .ToListAsync();

            if (!ventas.Any())
            {
                ViewBag.Message = "No has realizado ninguna compra. 😥";
                return View();
            }

            var detalles = await _context.DetalleVentas
                .Include(dv => dv.CodigoCancionNavigation)
                .Where(dv => ventas.Select(v => v.IdVenta).Contains(dv.IdVenta))
                .ToListAsync();

            var canciones = detalles.Select(dv => dv.CodigoCancionNavigation).Distinct().ToList();

            var viewModel = new HistorialMostrarVM
            { 
                Ventas = ventas,
                DetalleVentas = detalles,
                Canciones = canciones
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDetalleVenta(int idVenta)
        {
            var detalles = await _context.DetalleVentas
                .Include(dv => dv.CodigoCancionNavigation)
                .Where(dv => dv.IdVenta == idVenta)
                .Select(dv => new
                {
                    dv.IdVenta,
                    Cancion = dv.CodigoCancionNavigation.NombreCancion,
                    dv.Cantidad,
                    dv.PrecioUnitario,
                    Total = dv.Cantidad * dv.PrecioUnitario
                }).ToListAsync();

            return Json(detalles);
        }

        [HttpGet]
        public IActionResult HistorialVentas()
        {
            var ventas = _context.Ventas.ToList();

            var detalles = _context.DetalleVentas
                .Include(dv => dv.CodigoCancionNavigation)
                .Where(dv => ventas.Select(v => v.IdVenta).Contains(dv.IdVenta))
                .ToList();

            var canciones = detalles.Select(dv => dv.CodigoCancionNavigation).Distinct().ToList();

            var ViewModel = new HistorialMostrarVM
            {
                Ventas = ventas,
                DetalleVentas = detalles,
                Canciones = canciones
            };
            return View(ViewModel);
        }        
    }
}
