using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class EditarPerfilVM
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El número de tarjeta es obligatorio.")]
        [StringLength(19, MinimumLength = 13, ErrorMessage = "El número de tarjeta debe tener entre 13 y 19 dígitos.")]
        [RegularExpression(@"^\d{13,19}$", ErrorMessage = "El número de tarjeta debe contener solo dígitos.")]
        [Display(Name = "Número de tarjeta")]
        public string NumeroTarjeta { get; set; }

        [Required(ErrorMessage = "El tipo de tarjeta es obligatorio.")]
        [Display(Name = "Tipo de tarjeta")]
        [RegularExpression("^(Crédito|Débito)$", ErrorMessage = "El tipo de tarjeta debe ser 'Crédito' o 'Débito'.")]
        public string TipoTarjeta { get; set; }
    }
}
