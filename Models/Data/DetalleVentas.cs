using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class DetalleVentas
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public int CodigoCancion { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Total { get; set; }

    public virtual Canciones CodigoCancionNavigation { get; set; } = null!;

    public virtual Ventas IdVentaNavigation { get; set; } = null!;
}
