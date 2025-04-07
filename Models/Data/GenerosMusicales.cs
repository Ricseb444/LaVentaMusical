using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class GenerosMusicales
{
    public int CodigoGenero { get; set; }

    public string Descripcion { get; set; } = null!;

    public string FotoGenero { get; set; } = null!;

    public virtual ICollection<Canciones> Canciones { get; set; } = new List<Canciones>();
}
