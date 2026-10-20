using Microsoft.Data.SqlClient;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Crea las conexiones a SQL Server.
    /// </summary>
    public class clsConexionSQLSF
    {
        // En caso de estar usando otra instancia (ej: localhost o .\SQLEXPRESS), modificar solo esta constante
        private const string cStrServidorSF = "localhost";
        public const string cStrBaseDatosSF = "SurtiFacilDB";
        private const string cStrAutenticacionSF = "Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection CrearConexion()
        {
            return new SqlConnection("Server=" + cStrServidorSF + ";Database=" + cStrBaseDatosSF + ";" + cStrAutenticacionSF);
        }

        public SqlConnection CrearConexionMaster()
        {
            return new SqlConnection("Server=" + cStrServidorSF + ";Database=master;" + cStrAutenticacionSF);
        }
    }
}