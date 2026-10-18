using Microsoft.Data.SqlClient;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Crea las conexiones a SQL Express.
    /// </summary>
    public class clsConexionSQLSF
    {
        // Si cambia el servidor, solo se edita esta línea
        private const string cStrCadenaConexionSF =
            "Server=localhost;Database=SurtiFacilDB;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Devuelve una conexión nueva, todavía cerrada.
        /// </summary>
        public SqlConnection CrearConexion()
        {
            return new SqlConnection(cStrCadenaConexionSF);
        }
    }
}