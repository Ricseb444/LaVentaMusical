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

            // Obtener el carrito activo del usuario
            var carrito = await _context.CarritoCompras
                .Include(c => c.DetalleCarritos)
                .FirstOrDefaultAsync(c => c.IdUsuario == userId);

            if (carrito == null)
            {
                ViewBag.Message = "Tu carrito está vacío. ¡Añade canciones para comenzar a comprar!";
                return View();
            }

            // Obtener detalles con navegación de canciones
            var detalles = await _context.DetalleCarrito
                .Include(dc => dc.CodigoCancionNavigation)
                .Where(dc => dc.IdCarrito == carrito.IdCarrito)
                .ToListAsync();

            var canciones = detalles.Select(dc => dc.CodigoCancionNavigation).Distinct().ToList();

            // Seteo del saldo actual del usuario
            ViewBag.SaldoDisponible = user.DineroDisponible;


            string tarjetaCensurada = string.IsNullOrEmpty(user.NumeroTarjeta) || user.NumeroTarjeta.Length < 4
        ? ""
        : "**** **** **** " + user.NumeroTarjeta[^4..];
            ViewBag.TarjetaCensurada = tarjetaCensurada;

            // Recuperar el número de tarjeta desde el modelo de usuario
            var numeroTarjeta = user.NumeroTarjeta;

            // Pasa el número de tarjeta a la vista
            ViewBag.NumeroTarjeta = numeroTarjeta;








            var viewModel = new VentaMostrarVM
            {
                VENTAS = new Ventas
                {
                    Subtotal = carrito.Subtotal,
                    Total = carrito.Total
                },
                DetalleVentas = new List<DetalleVentas>(), // vacía por ahora
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

            // Obtener el carrito de compras del usuario
            var carritoCompras = await _context.CarritoCompras
                .FirstOrDefaultAsync(c => c.IdUsuario == usuario.Id);

            if (carritoCompras == null)
            {
                return BadRequest("El carrito de compras está vacío.");
            }

            // Obtener los detalles del carrito
            List<DetalleCarrito> LstDetalleCarritos = await _context.DetalleCarrito
                .Where(d => d.IdCarrito == carritoCompras.IdCarrito)  
                .ToListAsync();

            if (LstDetalleCarritos.Count == 0)
            {
                return BadRequest("No hay productos en el carrito.");
            }

            decimal totalCompra = carritoCompras.Total;

            // Procesar el pago según el tipo seleccionado
            if (ventaVM.VENTAS.TipoPago == "Dinero Disponible")
            {
                if (usuario.DineroDisponible < totalCompra)
                {
                    return BadRequest("Saldo insuficiente.");
                }
                usuario.DineroDisponible -= totalCompra;
            }
            //else if (ventaVM.VENTAS.TipoPago == "Tarjeta de Crédito" || ventaVM.VENTAS.TipoPago == "Paypal")
            //{

            //}
            //else
            //{
            //    return BadRequest("Método de pago no válido.");
            //}

          
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Crear la nueva venta
                    var ventaNueva = new Ventas()
                    {
                        IdUsuario = userId,
                        FechaCompra = DateTime.Now,
                        TipoPago = ventaVM.VENTAS.TipoPago,
                        Subtotal = carritoCompras.Subtotal,
                        Total  = Math.Round(carritoCompras.Subtotal + Math.Round(carritoCompras.Subtotal * 0.13m, 2), 2)
                    };

                    // Guardamos la nueva venta
                    await _context.Ventas.AddAsync(ventaNueva);
                    await _context.SaveChangesAsync(); // Guardamos para obtener el ID de la venta
                    Console.WriteLine($"Venta creada con ID: {ventaNueva.IdVenta}");

                    // Crear la lista de detalles de la venta
                    var listaDetalleVentas = new List<DetalleVentas>();
                    var codigosCanciones = LstDetalleCarritos.Select(x => x.CodigoCancion).ToList();
                    var canciones = await _context.Canciones
                        .Where(c => codigosCanciones.Contains(c.CodigoCancion))
                        .ToListAsync();

                    // Procesar los detalles de la venta y las existencias
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

                    // Actualizamos las canciones con los nuevos valores de cantidad disponible
                    _context.Canciones.UpdateRange(canciones);
                    await _context.SaveChangesAsync();

                    // Guardamos los detalles de la venta
                    await _context.DetalleVentas.AddRangeAsync(listaDetalleVentas);
                    await _context.SaveChangesAsync();

                    // Confirmamos la transacción
                    await transaction.CommitAsync();

                    // Limpiar el carrito de compras
                    _context.DetalleCarrito.RemoveRange(LstDetalleCarritos);
                    await _context.SaveChangesAsync();

                    // Eliminar el carrito de compras
                    _context.CarritoCompras.Remove(carritoCompras);
                    await _context.SaveChangesAsync();

                    // Responder correctamente si todo salió bien
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "Pago Procesado con Éxito", idVenta = ventaNueva.IdVenta });
                    }

                    return RedirectToAction("ConfirmacionCompra", "Comprar", new { idVenta = ventaNueva.IdVenta });
                }
                catch (Exception ex)
                {
                    // En caso de error, revertir los cambios
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Ocurrió un error al procesar el pago: " + ex.Message);
                }
            }
        }


        [HttpGet]
		public async Task<IActionResult> EliminarDelCarrito(int id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var carrito = await _context.CarritoCompras
				.FirstOrDefaultAsync(c => c.IdUsuario == userId);

			if (carrito == null)
			{
				return Json(new { success = false, message = "Carrito no encontrado." });
			}

			var detalle = await _context.DetalleCarrito
				.FirstOrDefaultAsync(d => d.IdCarrito == carrito.IdCarrito && d.IdDetalleCarrito == id);

			if (detalle == null)
			{
				return Json(new { success = false, message = "Producto no encontrado en el carrito." });
			}

			_context.DetalleCarrito.Remove(detalle);
			await _context.SaveChangesAsync();

			// Recalcular totales
			var nuevosDetalles = await _context.DetalleCarrito
				.Where(d => d.IdCarrito == carrito.IdCarrito)
				.ToListAsync();

			carrito.Subtotal = nuevosDetalles.Sum(d => d.Total);
			carrito.Total = carrito.Subtotal; // Si tenés impuestos, agregalos aquí

			_context.CarritoCompras.Update(carrito);
			await _context.SaveChangesAsync();

			return Json(new { success = true, total = carrito.Total });
		}






		public async Task<IActionResult> ConfirmacionCompra(int idVenta)
        {
            return RedirectToAction(nameof(Index));
        }

    }
}

