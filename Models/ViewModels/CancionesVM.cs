//using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models.Data;


namespace Models.ViewModels
{
    public class CancionesVM
    {
        public IEnumerable<GenerosMusicales> ListaGeneros { get; set; } 

        public IEnumerable<Albumes> ListaAlbumes { get; set; } 

        public Canciones Cancion { get; set; }

    }
}
