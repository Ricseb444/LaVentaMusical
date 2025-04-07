using Microsoft.AspNetCore.Mvc;
//using AutoMapper;
using Models.ViewModels;
using Models.Data;
using Microsoft.AspNetCore.Identity;
using Models;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.EntityFrameworkCore;


namespace ProyectoVentaMusical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlantillaController : Controller
    {
        //private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PlantillaController(/*IMapper mapper,*/ ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            //_mapper = mapper;
        }

        public IActionResult PDFVenta(int IdVenta)
        {
            var viewModel = new PDFVentaVM();

            var ventas =  _context.Ventas                
                .FirstOrDefault(v => v.IdVenta == IdVenta);

            if (ventas == null)
            {
                ViewBag.Message = "Error al generar la factura: Venta no encontrada.";
                return View(viewModel);
            }

            var detalles =  _context.DetalleVentas
                .Include(dv => dv.CodigoCancionNavigation)
                .Where(dv => dv.IdVenta == ventas.IdVenta)
                .ToList();

            var canciones = detalles.Select(dv => dv.CodigoCancionNavigation).Distinct().ToList();

            viewModel.Venta = ventas;
            viewModel.DetalleVentas = detalles;
            viewModel.Canciones = detalles.Select(dv => dv.CodigoCancionNavigation).Distinct().ToList();

            return View(viewModel);
        }

    }
}
