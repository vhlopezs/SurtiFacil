using System;
using System.Collections.Generic;

namespace CapaEntidadesSF
{
    /// <summary>
    /// Orden de compra con su lista de productos.
    /// </summary>
    public class clsOrdenCompraEN
    {
        /// <summary>Número de la orden, lo asigna la base de datos.</summary>
        public int gIntIdOrden { get; set; }

        /// <summary>Empresa que hace el pedido.</summary>
        public string gStrNombreEmpresa { get; set; }

        /// <summary>Fecha usada para el análisis.</summary>
        public DateTime gDteFechaAnalisis { get; set; }

        /// <summary>Suma de todos los subtotales.</summary>
        public decimal gDecTotalAproximado { get; set; }

        /// <summary>Productos que se van a pedir.</summary>
        public List<clsPedidoSugeridoEN> gLstDetalles { get; set; }

        /// <summary>
        /// Constructor vacío, deja los valores iniciales.
        /// </summary>
        public clsOrdenCompraEN()
        {
            this.gIntIdOrden = 0;
            this.gStrNombreEmpresa = "";
            this.gDteFechaAnalisis = DateTime.Today;
            this.gDecTotalAproximado = 0;
            this.gLstDetalles = new List<clsPedidoSugeridoEN>();
        }
    }
}