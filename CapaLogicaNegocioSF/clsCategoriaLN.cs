using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Lógica de las categorías.
    /// </summary>
    public class clsCategoriaLN
    {
        private clsConexionSQLSF gConexionSF = new clsConexionSQLSF();

        /// <summary>
        /// Trae todas las categorías de la base de datos.
        /// </summary>
        public List<clsCategoriaEN> ListarCategorias()
        {
            List<clsCategoriaEN> lLstCategoriasSF = new List<clsCategoriaEN>();

            // Los using cierran la conexión solos, aunque haya error
            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            using (SqlCommand lSqlComandoSF = new SqlCommand("spListarCategorias", lSqlConexionSF))
            {
                lSqlComandoSF.CommandType = CommandType.StoredProcedure;
                lSqlConexionSF.Open();

                using (SqlDataReader lSqlLectorSF = lSqlComandoSF.ExecuteReader())
                {
                    while (lSqlLectorSF.Read())
                    {
                        clsCategoriaEN lEntCategoriaSF = new clsCategoriaEN();
                        lEntCategoriaSF.gIntIdCategoria = Convert.ToInt32(lSqlLectorSF["IdCategoria"]);
                        lEntCategoriaSF.gStrNombreCategoria = lSqlLectorSF["NombreCategoria"].ToString();
                        lLstCategoriasSF.Add(lEntCategoriaSF);
                    }
                }
            }

            return lLstCategoriasSF;
        }
    }
}