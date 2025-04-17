using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Data;
using Models.ViewModels;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoVentaMusical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ComprarController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComprarController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }


        // Este ActionResult se utiliza para buscar canciones con la barra de busqueda del index
        [HttpGet]
        public async Task<IActionResult> Index(string objBuscar)
        {
            var userID = _userManager.GetUserId(User);

            var cantidadEnCarrito = _context.DetalleCarrito
                .Include(dc => dc.IdCarritoNavigation)
                .Where(dc => dc.IdCarritoNavigation.IdUsuario == userID)
                .Sum(dc => dc.Cantidad);

            ViewBag.CantidadEnCarrito = cantidadEnCarrito;

            var canciones = from c in _context.Canciones select c;
            if (!string.IsNullOrEmpty(objBuscar))
            {
                canciones = canciones.Where(c => c.NombreCancion.Contains(objBuscar));
            }

            var lstFiltrada = await canciones.ToListAsync();
            CarritoVM viewModel = new CarritoVM
            {
                ListaCanciones = lstFiltrada,
                CarritoCompras = new CarritoCompras()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCantidadEnCarrito()
        {
            var userID = _userManager.GetUserId(User);

            var cantidadEnCarrito = _context.DetalleCarrito
                .Include(dc => dc.IdCarritoNavigation)
                .Where(dc => dc.IdCarritoNavigation.IdUsuario == userID)
                .Sum(dc => dc.Cantidad);

            return Json(new { cantidad = cantidadEnCarrito });
        }

        [HttpPost]
        public async Task<IActionResult> anadirACarrito(CarritoVM carritoVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuario = await _userManager.FindByIdAsync(userId);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Verifica si ya existe un carrito para este usuario
            var carritoExistente = _context.CarritoCompras
                                           .FirstOrDefault(c => c.IdUsuario == usuario.Id);

            if (carritoExistente == null)
            {
                // Crea un nuevo carrito si no existe
                CarritoCompras nuevoCarrito = new CarritoCompras
                {
                    IdUsuario = usuario.Id,
                    NombreUsuario = usuario.UserName,
                    FechaCompra = DateTime.Now,
                    TipoPago = "Tarjeta de Crédito", //carritoVM.CarritoCompras.TipoPago,
                    Subtotal = 0,
                    Total = 0
                };

                _context.CarritoCompras.Add(nuevoCarrito);
                await _context.SaveChangesAsync();
                carritoExistente = nuevoCarrito;
            }

            // Agrega un nuevo detalle de carrito
            DetalleCarrito nuevoDetalle = new DetalleCarrito
            {
                IdCarrito = carritoExistente.IdCarrito,
                CodigoCancion = carritoVM.Cancion.CodigoCancion,
                Cantidad = carritoVM.Cancion.CantidadDisponible,
                PrecioUnitario = carritoVM.Cancion.Precio,
                Total = carritoVM.Cancion.CantidadDisponible * carritoVM.Cancion.Precio
            };

            _context.DetalleCarrito.Add(nuevoDetalle);

            // Actualiza los totales del carrito
            carritoExistente.Subtotal += nuevoDetalle.Total;
            carritoExistente.Total += nuevoDetalle.Total;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> CarritoCompras()
        {
            var userId = _userManager.GetUserId(User);

            // Encontrar el carrito de compras para el usuario logueado

            var carrito = _context.CarritoCompras.FirstOrDefault(c => c.IdUsuario == userId);

            if (carrito == null)
            {
                ViewBag.Message = "Tu carrito está vacío. ¡Añade canciones para comenzar a comprar!";
                return View();
            }

            var detalles = _context.DetalleCarrito
                .Include(dc => dc.CodigoCancionNavigation)
                .Where(dc => dc.IdCarrito == carrito.IdCarrito)
                .ToList();

            var canciones = detalles.Select(dc => dc.CodigoCancionNavigation).Distinct().ToList();

            var viewModel = new CarritoMostrarVM
            {
                CarritoCompras = carrito,
                DetalleCarrito = detalles,
                ListaCanciones = canciones
            };
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Pagar()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            //Encontrar la venta para el usuario logueado

            var venta = _context.Ventas.FirstOrDefault(v => v.IdUsuario == userId);

            if (venta == null)
            {
                ViewBag.Message = "Debes ingresar carritos de compra a tu venta";
                return View();
            }

            var detalles = _context.DetalleVentas
                .Include(dv => dv.CodigoCancionNavigation)
                .Where(dv => dv.IdVenta == venta.IdVenta)
                .ToList();

            var canciones = detalles.Select(dv => dv.CodigoCancionNavigation).Distinct().ToList();

            var viewModel = new VentaMostrarVM
            {
                VENTAS = venta,
                DetalleVentas = detalles,
                ListaCanciones = canciones
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarPago(VentaMostrarVM ventaVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuario = await _userManager.FindByIdAsync(userId);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            var carritoCompras = await _context.CarritoCompras
                .FirstOrDefaultAsync(c => c.IdUsuario == usuario.Id);

            if (carritoCompras == null)
            {
                return BadRequest("El carrito de compras está vacío.");
            }

            List<DetalleCarrito> LstDetalleCarritos = await _context.DetalleCarrito
                .Where(d => d.IdCarrito == carritoCompras.IdCarrito)
                .ToListAsync();

            if (LstDetalleCarritos.Count == 0)
            {
                return BadRequest("No hay productos en el carrito.");
            }
            

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    // Crear nueva venta
                    var ventaNueva = new Ventas()
                    {
                        IdUsuario = userId,
                        FechaCompra = DateTime.Now,
                        TipoPago = ventaVM.VENTAS.TipoPago,
                        Subtotal = carritoCompras.Subtotal,
                        Total = carritoCompras.Total
                    };

                    await _context.Ventas.AddAsync(ventaNueva);
                    await _context.SaveChangesAsync(); // Guardar la venta primero para obtener el IdVenta
                    Console.WriteLine($"Venta creada con ID: {ventaNueva.IdVenta}");
                    // Crear lista de detalles de venta
                    var listaDetalleVentas = new List<DetalleVentas>();
                    var codigosCanciones = LstDetalleCarritos.Select(x => x.CodigoCancion).ToList();
                    var canciones = await _context.Canciones
                        .Where(c => codigosCanciones.Contains(c.CodigoCancion))
                        .ToListAsync();

                    foreach (var item in LstDetalleCarritos)
                    {
                        listaDetalleVentas.Add(new DetalleVentas()
                        {
                            IdVenta = ventaNueva.IdVenta,
                            CodigoCancion = item.CodigoCancion,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = item.PrecioUnitario,
                            Total = item.Total
                        });
                        var cancion = canciones.FirstOrDefault(c => c.CodigoCancion == item.CodigoCancion);
                        if (cancion != null)
                        {
                            if (cancion.CantidadDisponible >= item.Cantidad)
                            {
                                cancion.CantidadDisponible -= item.Cantidad;
                            }
                            else
                            {
                                return BadRequest($"Stock insuficiente para la canción {cancion.NombreCancion}");
                            }
                        }

                    }
                    // Guardar todos los cambios de las canciones en una sola operación
                    _context.Canciones.UpdateRange(canciones);
                    await _context.SaveChangesAsync();
                    // Guardar todos de una vez
                    await _context.DetalleVentas.AddRangeAsync(listaDetalleVentas);
                    await _context.SaveChangesAsync();




                    // Si todo fue exitoso, confirmar la transacción y enviar json
                    await transaction.CommitAsync();

                    _context.RemoveRange(LstDetalleCarritos);
                    await _context.SaveChangesAsync();

                    _context.Remove(carritoCompras);
                    await _context.SaveChangesAsync();

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "Pago Procesado con Éxito", idVenta = ventaNueva.IdVenta });
                    }

                    if (ventaNueva.IdVenta <= 0)
                    {
                        return Json(new { success = false, message = "Error al generar el ID de la venta" });
                    }
                    return RedirectToAction("ConfirmacionCompra", "Comprar", new { idVenta = ventaNueva.IdVenta });
                }
                catch (Exception ex)
                {
                    // Si hay un error, revertir la transacción
                    await transaction.RollbackAsync();
                    //return StatusCode(500, "Ocurrió un error al procesar el pago: " + ex.Message);
                    return Json(new { success = false, message = "Error al procesar el pago: " + ex.Message });
                }
            }
            //return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ConfirmacionCompra(int idVenta)
        {
            return RedirectToAction(nameof(Index));
        }

    }
}

