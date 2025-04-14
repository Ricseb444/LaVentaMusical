using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.ViewModels;

namespace ProyectoVentaMusical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CancionesController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _context;

        public CancionesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            CancionesVM cancionVM = new CancionesVM()
            {
                Cancion = new Canciones(),
                ListaAlbumes = _context.Albumes.ToList(),
                ListaGeneros = _context.GenerosMusicales.ToList(),
            };
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CancionesVM cancionVM = new CancionesVM()
            {
                Cancion = new Canciones(),  // Inicializa la propiedad Cancion
                ListaGeneros = _context.GenerosMusicales.ToList(), // Carga los géneros desde la BD
                ListaAlbumes = _context.Albumes.ToList() // Carga los álbumes desde la BD
            };
            return View(cancionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Canciones cancion)
        {
            //if (ModelState.IsValid)
            //{
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;
            if (cancion.CodigoCancion == 0 && archivos.Count() > 0)
            {
                // Nueva canción
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\canciones");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                cancion.fotoCancion = @"\imagenes\canciones\" + nombreArchivo + extension;

                _context.Canciones.Add(cancion);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
                //}
                //else
                //{
                //	ModelState.AddModelError("Archivo", "Debes seleccionar un archivo de audio");
                //}
            }
            return View(cancion);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cancion = _context.Canciones.FirstOrDefault(a => a.CodigoCancion == id);

            if (cancion == null)
            {
                return NotFound();
            }

            CancionesVM cancionVM = new CancionesVM()
            {
                Cancion = cancion,
                ListaGeneros = _context.GenerosMusicales.ToList(),
                ListaAlbumes = _context.Albumes.ToList()
            };

            return View(cancionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Canciones cancion)
        {
            //if (ModelState.IsValid)
            //{
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            var cancionDesdeBd = _context.Canciones.FirstOrDefault(a => a.CodigoCancion == cancion.CodigoCancion);

            if (archivos.Count() > 0)
            {
                // 
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\canciones");
                var extension = Path.GetExtension(archivos[0].FileName);

                var rutaCancion = Path.Combine(rutaPrincipal, cancionDesdeBd.fotoCancion.TrimStart('\\'));

                if (System.IO.File.Exists(rutaCancion))
                {
                    System.IO.File.Delete(rutaCancion);
                }

                // Nuevamente subimos el archivo
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                cancion.fotoCancion = @"\imagenes\canciones\" + nombreArchivo + extension;

                cancionDesdeBd.CodigoGenero = cancion.CodigoGenero;
                cancionDesdeBd.CodigoAlbum = cancion.CodigoAlbum;
                cancionDesdeBd.NombreCancion = cancion.NombreCancion;
                cancionDesdeBd.LinkVideo = cancion.LinkVideo;
                cancionDesdeBd.Precio = cancion.Precio;
                cancionDesdeBd.CantidadDisponible = cancion.CantidadDisponible;
                cancionDesdeBd.fotoCancion = cancion.fotoCancion;
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Aquí sería cuando el arc ya existe y se conserva
                cancion.fotoCancion = cancionDesdeBd.fotoCancion;
            }

            cancionDesdeBd.CodigoGenero = cancion.CodigoGenero;
            cancionDesdeBd.CodigoAlbum = cancion.CodigoAlbum;
            cancionDesdeBd.NombreCancion = cancion.NombreCancion;
            cancionDesdeBd.LinkVideo = cancion.LinkVideo;
            cancionDesdeBd.Precio = cancion.Precio;
            cancionDesdeBd.CantidadDisponible = cancion.CantidadDisponible;
            cancionDesdeBd.fotoCancion = cancion.fotoCancion;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
            //}
            //return View(cancion);
        }

        #region Llamadas a la API

        [HttpGet]

        public IActionResult GetAll()
        {
            var canciones = _context.Canciones.ToList();
            return Json(new { data = canciones });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var cancion = _context.Canciones
                .Include(a => a.DetalleCarritos)
                .Include(a => a.DetalleVenta)
                .FirstOrDefault(a => a.CodigoCancion == id);
            if (cancion == null)
            {
                return Json(new { success = false, message = "Error borrando cancion" });
            }

            if (cancion.DetalleCarritos.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "No se puede eliminar la canción porque tiene carritos asociados."
                });
            }

            if (cancion.DetalleVenta.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "No se puede eliminar la canción porque tiene facturas asociados."
                });
            }

            _context.Canciones.Remove(cancion);

            _context.SaveChanges();

            return Json(new { success = true, message = "Cancion Borrada Correctamente" });
        }

        [HttpGet]
        public IActionResult ObtenerCanciones()
        {
            var canciones = _context.Canciones
                .Include(c => c.CodigoGeneroNavigation)
                .Include(c => c.CodigoAlbumNavigation)
                    .ThenInclude(a => a.CodigoArtistaNavigation)
                .Select(c => new
                {
                    codigoCancion = c.CodigoCancion,
                    nombreCancion = c.NombreCancion,
                    linkVideo = c.LinkVideo,
                    precio = c.Precio,
                    cantidadDisponible = c.CantidadDisponible,
                    fotoCancion = c.fotoCancion,
                    codigoGeneroNavigation = new
                    {
                        nombreGenero = c.CodigoGeneroNavigation.Descripcion
                    },
                    codigoAlbumNavigation = new
                    {
                        nombreAlbum = c.CodigoAlbumNavigation.NombreAlbum,
                        codigoArtistaNavigation = new
                        {
                            nombreArtistico = c.CodigoAlbumNavigation.CodigoArtistaNavigation.NombreArtistico
                        }
                    }
                }).ToList();

            return Json(canciones);
        }

        #endregion
    }
}