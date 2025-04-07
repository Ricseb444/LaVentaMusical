using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class DetalleVentaVM
    {
        public int IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }

        public int CodigoCancion { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Total { get; set; }
    }
}
