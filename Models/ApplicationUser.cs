using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        // Número de identificación del usuario
        public string? NumeroIdentificacion { get; set; }

        // Nombre completo del usuario
        public string? NombreCompleto { get; set; }

        // Género del usuario (Masculino/Femenino)
        public string? Genero { get; set; }

        // Correo electrónico adicional (no es el UserName)
        public string? CorreoElectronico { get; set; }

        // Tipo de tarjeta (Visa, MasterCard, etc.)
        public string? TipoTarjeta { get; set; }

        // Cantidad de dinero disponible en la cuenta
        public decimal DineroDisponible { get; set; }

        // Número de tarjeta con validación de formato 0000-0000-0000-0000
        [RegularExpression(@"^\d{4}-\d{4}-\d{4}-\d{4}$",
            ErrorMessage = "Formato de la tarjeta inválido. Debe ser 0000-0000-0000-0000")]
        public string? NumeroTarjeta { get; set; }
    }
}
