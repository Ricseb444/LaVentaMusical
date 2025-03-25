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
    public class AlbumesVM
    {
        public Albumes Album { get; set; }
        public IEnumerable<Artistas> ListaArtistas { get; set; }

    }
}
