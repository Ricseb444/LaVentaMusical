using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NumeroIdentificacion { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string Genero { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string TipoTarjeta { get; set; } = null!;

    public decimal DineroDisponible { get; set; }

    public string NumeroTarjeta { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int IdPerfil { get; set; }

    public virtual ICollection<CarritoCompras> CarritoCompras { get; set; } = new List<CarritoCompras>();

    public virtual Perfiles IdPerfilNavigation { get; set; } = null!;

    public virtual ICollection<Ventas> Venta { get; set; } = new List<Ventas>();
}
