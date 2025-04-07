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
    public class CarritoVM
    {
        public CarritoCompras CarritoCompras { get; set; }
        public Canciones Cancion { get; set; }
        public IEnumerable<Canciones> ListaCanciones { get; set; }

    }
}
