using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class ApplicationUser : IdentityUser
	{
        public string? NumeroIdentificacion { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Genero { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? TipoTarjeta { get; set; }
        public decimal DineroDisponible { get; set; }
        public string? NumeroTarjeta { get; set; }


    }
}
