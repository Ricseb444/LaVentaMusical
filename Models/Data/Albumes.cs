using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class Albumes
{
    public int CodigoAlbum { get; set; }

    public int CodigoArtista { get; set; }

    public string NombreAlbum { get; set; } = null!;

    public int AñoLanzamiento { get; set; }

    public string? ImagenAlbum { get; set; }

    public virtual ICollection<Canciones> Canciones { get; set; } = new List<Canciones>();

    public virtual Artistas CodigoArtistaNavigation { get; set; } = null!;
}
