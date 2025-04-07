using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class CarritoCompras
{
    public int IdCarrito { get; set; }

    public string IdUsuario { get; set; }

    public DateTime? FechaCompra { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public string TipoPago { get; set; } = null!;

    public string NombreUsuario { get; set; }

    public virtual ICollection<DetalleCarrito> DetalleCarritos { get; set; } = new List<DetalleCarrito>();

}
