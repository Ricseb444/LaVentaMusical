using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class Ventas
{
    public int IdVenta { get; set; }

    public string IdUsuario { get; set; }

    public DateTime? FechaCompra { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public string TipoPago { get; set; } = null!;

    public virtual ICollection<DetalleVentas> DetalleVenta { get; set; } = new List<DetalleVentas>();

    //public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
