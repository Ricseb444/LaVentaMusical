using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class Artistas
{
    public int CodigoArtista { get; set; }

    public string NombreArtistico { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string NombreReal { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public string? FotoArtista { get; set; }

    public string? LinkBiografia { get; set; }

    public virtual ICollection<Albumes> Albumes { get; set; } = new List<Albumes>();
}
