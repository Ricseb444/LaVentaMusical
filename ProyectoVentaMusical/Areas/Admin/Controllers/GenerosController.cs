using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ProyectoVentaMusical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenerosController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _context;

        public GenerosController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GenerosMusicales genero)
        {
            //if (ModelState.IsValid)
            //{
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;
            if (genero.CodigoGenero == 0 && archivos.Count() > 0)
            {
                //Nuevo articulo
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\generos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                genero.FotoGenero = @"\imagenes\generos\" + nombreArchivo + extension;

                _context.GenerosMusicales.Add(genero);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Imagen", "Debes seleccionar una imagen");
            }

            //}
            return View(genero);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            GenerosMusicales genero = new GenerosMusicales();
            genero = _context.GenerosMusicales.FirstOrDefault(a => a.CodigoGenero == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GenerosMusicales genero)
        {
            //if (ModelState.IsValid)
            //{
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            var articuloDesdeBd = _context.GenerosMusicales.FirstOrDefault(a => a.CodigoGenero == genero.CodigoGenero);


            if (archivos.Count() > 0)
            {
                //Nuevo imagen para el artículo
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\generos");
                var extension = Path.GetExtension(archivos[0].FileName);
                var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeBd.FotoGenero.TrimStart('\\'));

                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }

                //Nuevamente subimos el archivo
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                genero.FotoGenero = @"\imagenes\generos\" + nombreArchivo + extension;

                articuloDesdeBd.Descripcion = genero.Descripcion;
                articuloDesdeBd.FotoGenero = genero.FotoGenero;
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                //Aquí sería cuando la imagen ya existe y se conserva
                genero.FotoGenero = articuloDesdeBd.FotoGenero;
            }

            articuloDesdeBd.Descripcion = genero.Descripcion;
            articuloDesdeBd.FotoGenero = genero.FotoGenero;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

            //}
            return View();
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            var generos = _context.GenerosMusicales.ToList();
            return Json(new { data = generos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var genero = _context.GenerosMusicales
                .Include(a => a.Canciones)
                .FirstOrDefault(a => a.CodigoGenero == id);
            if (genero == null)
            {
                return Json(new { success = false, message = "Error borrando Genero" });
            }

            if (genero.Canciones.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "No se puede eliminar el genero porque tiene canciones asociadas."
                });
            }
            _context.GenerosMusicales.Remove(genero);
            _context.SaveChanges();
            return Json(new { success = true, message = "Genero Borrado Correctamente" });
        }
        #endregion
    }
}
