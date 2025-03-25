using System;
using System.Collections.Generic;

namespace Models.Data;

public partial class Perfiles
{
    public int IdPerfil { get; set; }

    public string NombrePerfil { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
