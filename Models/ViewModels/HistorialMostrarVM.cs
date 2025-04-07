using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Data;

namespace Models.ViewModels
{
    public class HistorialMostrarVM
    {
        public IEnumerable<Ventas> Ventas { get; set;}
        public IEnumerable<DetalleVentas> DetalleVentas { get; set;}
        public IEnumerable<Canciones> Canciones { get; set;}
    }
}
