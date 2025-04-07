using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class DetalleCarrito
{
    public int IdDetalleCarrito { get; set; }

    public int IdCarrito { get; set; }

    public int CodigoCancion { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Total { get; set; }

    public virtual Canciones CodigoCancionNavigation { get; set; } = null!;

    public virtual CarritoCompras IdCarritoNavigation { get; set; } = null!;
}
