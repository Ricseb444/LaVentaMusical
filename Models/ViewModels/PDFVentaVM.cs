using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class PDFVentaVM
    {
        //public VentaVM? venta { get; set; }
        public Ventas? Venta { get; set; }
        public IEnumerable<DetalleVentas>? DetalleVentas { get; set; }
        public IEnumerable<Canciones>? Canciones { get; set; }
    }
}
