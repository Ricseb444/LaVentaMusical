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
    public class CarritoMostrarVM
    {

        public CarritoCompras CarritoCompras { get; set; }
        public IEnumerable<DetalleCarrito> DetalleCarrito { get; set; }
        public IEnumerable<Canciones> ListaCanciones { get; set; }

    }
}
