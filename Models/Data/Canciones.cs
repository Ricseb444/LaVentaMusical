using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class Canciones
{
    public int CodigoCancion { get; set; }

    public int CodigoGenero { get; set; }

    public int CodigoAlbum { get; set; }

    public string NombreCancion { get; set; } = null!;

    public string? LinkVideo { get; set; }

    public decimal Precio { get; set; }

    public int CantidadDisponible { get; set; }
	public string fotoCancion { get; set; }

	public virtual Albumes CodigoAlbumNavigation { get; set; } = null!;

    public virtual GenerosMusicales CodigoGeneroNavigation { get; set; } = null!;

    public virtual ICollection<DetalleCarrito> DetalleCarritos { get; set; } = new List<DetalleCarrito>();

    public virtual ICollection<DetalleVentas> DetalleVenta { get; set; } = new List<DetalleVentas>();
}
