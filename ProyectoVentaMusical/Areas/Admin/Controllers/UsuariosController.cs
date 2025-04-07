using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Models;

namespace ProyectoVentaMusical.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToPage("/Account/Register", new { area = "Identity" });

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ApplicationUser usuario, string password)
        //{
        //    //Nuevo usuario
        //    var result = await _userManager.CreateAsync(usuario, password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        // 🔹 Si hay errores, los mostramos
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(usuario);
        //}


        [HttpGet]
        public IActionResult Edit(string id)
        {
            var usuario = _context.Users.FirstOrDefault(a => a.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser usuario, string newPassword)
        {
            var usuarioDesdeBd = await _userManager.FindByIdAsync(usuario.Id);
            if (usuarioDesdeBd == null)
            {
                return NotFound();
            }

            // 🔹 Actualizamos los datos del usuario
            usuarioDesdeBd.NumeroIdentificacion = usuario.NumeroIdentificacion;
            usuarioDesdeBd.NombreCompleto = usuario.NombreCompleto;
            usuarioDesdeBd.Genero = usuario.Genero;
            usuarioDesdeBd.CorreoElectronico = usuario.CorreoElectronico;
            usuarioDesdeBd.TipoTarjeta = usuario.TipoTarjeta;
            usuarioDesdeBd.DineroDisponible = usuario.DineroDisponible;
            usuarioDesdeBd.NumeroTarjeta = usuario.NumeroTarjeta;

            if (!string.IsNullOrEmpty(newPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(usuarioDesdeBd);
                var result = await _userManager.ResetPasswordAsync(usuarioDesdeBd, token, newPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(usuario);
                }
            }

            await _userManager.UpdateAsync(usuarioDesdeBd);
            return RedirectToAction(nameof(Index));
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios = _context.Users.ToList();
            return Json(new { data = usuarios });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var usuario = _context.Users.Find(id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error borrando Usuario" });
            }
            _context.Users.Remove(usuario);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            //return Json(new { success = true, message = "Usuario Borrado Correctamente" });
        }
        #endregion
    }
}
