using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;

namespace ProyectoVentaMusical.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PerfilController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Editar()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null) return NotFound();

            var modelo = new EditarPerfilVM
            {
                NombreCompleto = usuario.NombreCompleto,
                NumeroTarjeta = usuario.NumeroTarjeta,
                TipoTarjeta = usuario.TipoTarjeta
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarPerfilVM modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null) return NotFound();

            usuario.NombreCompleto = modelo.NombreCompleto;
            usuario.NumeroTarjeta = modelo.NumeroTarjeta;
            usuario.TipoTarjeta = modelo.TipoTarjeta;

            var resultado = await _userManager.UpdateAsync(usuario);

            if (resultado.Succeeded)
            {
                TempData["Mensaje"] = "Perfil actualizado correctamente.";
                return RedirectToAction("Editar");
            }

            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(modelo);
        }
    }
}
