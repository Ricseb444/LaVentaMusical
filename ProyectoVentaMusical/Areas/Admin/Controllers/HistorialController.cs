using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Data;
using Models.ViewModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Utilidades;
using NuGet.Protocol.Plugins;

namespace ProyectoVentaMusical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HistorialController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConverter _converter;
        private readonly EmailSender _emailSender;

        public HistorialController(ApplicationDbContext context,
            IWebHostEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            IConverter converter, EmailSender emailSender)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _converter = converter;
            _emailSender = emailSender;
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
                    Subtotal = dv.Cantidad * dv.PrecioUnitario,
                    IVA = Math.Round(dv.Cantidad * dv.PrecioUnitario * 0.13m, 2),
                    TotalConIVA = Math.Round(dv.Cantidad * dv.PrecioUnitario * 1.13m, 2)
                }).ToListAsync();

            return Json(detalles);
        }

        [HttpGet]
        public IActionResult HistorialVentas()
        {
            var ventas = _context.Ventas.ToList();

            //var detalles = _context.DetalleVentas
            //    .Include(dv => dv.CodigoCancionNavigation)
            //    .Where(dv => ventas.Select(v => v.IdVenta).Contains(dv.IdVenta))
            //    .ToList();

            //var canciones = detalles.Select(dv => dv.CodigoCancionNavigation).Distinct().ToList();
            

            var ViewModel = new HistorialMostrarVM
            {
                Ventas = ventas
                //DetalleVentas = detalles,
                //Canciones = canciones
            };
            return View(ViewModel);
        }

        public async Task<IActionResult> MostrarPDFVenta(int IdVenta)
        {
            string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Admin/Plantilla/PDFVenta?IdVenta={IdVenta}";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page = urlPlantillaVista
                    }
                }
            };

            var archivoPDF = _converter.Convert(pdf);

            var ventas = _context.Ventas
                .FirstOrDefault(v => v.IdVenta == IdVenta);

            var usuario = await _userManager.FindByIdAsync(ventas.IdUsuario);
            var emailDestino = usuario?.Email;

            if (!string.IsNullOrEmpty(emailDestino))
            {
                await _emailSender.SendEmailWithAttachmentAsync
                     (
                         emailDestino,
                        "Factura de compra",
                        "Hola, adjuntamos tu factura en PDF. Gracias por tu Compra",
                        archivoPDF,
                        "Factura.pdf"
                    );
            }

            return File(archivoPDF, "application/pdf");
        }


        public async Task<IActionResult> ReversarCompra(int id)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var venta = await _context.Ventas.FirstOrDefaultAsync(x => x.IdVenta == id);

                decimal dinero = venta.Subtotal;


                var Idusuario = venta.IdUsuario;

                var usuarioBD = await _context.applicationUsers.FirstOrDefaultAsync(u => u.Id == venta.IdUsuario);

                usuarioBD.DineroDisponible += dinero;
                await _context.SaveChangesAsync();

                List<DetalleVentas> Detalles = await _context.DetalleVentas
                    .Where(d => d.IdVenta == venta.IdVenta)
                    .ToListAsync();

                foreach (var item in Detalles)
                {
                    //colocar canciones
                    _context.DetalleVentas.Remove(item);
                }

                _context.Ventas.Remove(venta);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction(nameof(HistorialVentas));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Ocurrió un error al intentar revertir la compra.");
            }            
        }


    }
}
