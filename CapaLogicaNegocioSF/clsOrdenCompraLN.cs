using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Arma y guarda la orden de compra.
    /// </summary>
    public class clsOrdenCompraLN
    {
        private clsConexionSQLSF gConexionSF = new clsConexionSQLSF();

        /// <summary>
        /// Arma la orden solo con los productos que tienen cantidad mayor a cero.
        /// </summary>
        public clsOrdenCompraEN CrearOrden(string pStrNombreEmpresaSF, DateTime pDteFechaAnalisisSF,
                                           List<clsPedidoSugeridoEN> pLstPedidoSF)
        {
            clsOrdenCompraEN lEntOrdenSF = new clsOrdenCompraEN();
            lEntOrdenSF.gStrNombreEmpresa = pStrNombreEmpresaSF;
            lEntOrdenSF.gDteFechaAnalisis = pDteFechaAnalisisSF.Date;

            foreach (clsPedidoSugeridoEN lEntPedidoSF in pLstPedidoSF)
            {
                if (lEntPedidoSF.gIntCantidadPedidoSugerido > 0)
                {
                    lEntOrdenSF.gLstDetalles.Add(lEntPedidoSF);
                    lEntOrdenSF.gDecTotalAproximado =
                        lEntOrdenSF.gDecTotalAproximado + lEntPedidoSF.gDecSubtotalSugerido;
                }
            }

            return lEntOrdenSF;
        }

        /// <summary>
        /// Guarda el encabezado y los productos en una sola transacción.
        /// Devuelve el número de la orden.
        /// </summary>
        public int GuardarOrden(clsOrdenCompraEN pEntOrdenSF)
        {
            if (pEntOrdenSF.gLstDetalles.Count == 0)
            {
                throw new InvalidOperationException("La orden no tiene productos para guardar.");
            }

            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            {
                lSqlConexionSF.Open();

                // La transacción guarda todo o no guarda nada
                SqlTransaction lSqlTransaccionSF = lSqlConexionSF.BeginTransaction();

                try
                {
                    int lIntIdOrdenSF = 0;

                    // Primero el encabezado
                    using (SqlCommand lSqlComandoSF = new SqlCommand(
                        "spInsertarOrdenCompra", lSqlConexionSF, lSqlTransaccionSF))
                    {
                        lSqlComandoSF.CommandType = CommandType.StoredProcedure;
                        lSqlComandoSF.Parameters.AddWithValue("@NombreEmpresa", pEntOrdenSF.gStrNombreEmpresa);
                        lSqlComandoSF.Parameters.AddWithValue("@FechaAnalisis", pEntOrdenSF.gDteFechaAnalisis);
                        lSqlComandoSF.Parameters.AddWithValue("@TotalAproximado", pEntOrdenSF.gDecTotalAproximado);
                        lIntIdOrdenSF = Convert.ToInt32(lSqlComandoSF.ExecuteScalar());
                    }

                    // Después cada producto de la orden
                    foreach (clsPedidoSugeridoEN lEntDetalleSF in pEntOrdenSF.gLstDetalles)
                    {
                        using (SqlCommand lSqlComandoDetalleSF = new SqlCommand(
                            "spInsertarDetalleOrdenCompra", lSqlConexionSF, lSqlTransaccionSF))
                        {
                            lSqlComandoDetalleSF.CommandType = CommandType.StoredProcedure;
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@IdOrden", lIntIdOrdenSF);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@IdProducto", lEntDetalleSF.gIntIdProducto);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@CantidadSugerida", lEntDetalleSF.gIntCantidadPedidoSugerido);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@CostoUnitario", lEntDetalleSF.gDecCostoUnitario);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@Subtotal", lEntDetalleSF.gDecSubtotalSugerido);
                            lSqlComandoDetalleSF.ExecuteNonQuery();
                        }
                    }

                    lSqlTransaccionSF.Commit();
                    return lIntIdOrdenSF;
                }
                catch
                {
                    // Si algo falla se deshace todo
                    lSqlTransaccionSF.Rollback();
                    throw;
                }
            }
        }
    }
}