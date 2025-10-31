using System.Data.SqlClient;

public class ConexionBD
{
    private string connectionString = "Server=localhost;Database=PoliticasEUC;Trusted_Connection=True;";

    public SqlConnection ObtenerConexion()
    {
        return new SqlConnection(connectionString);
    }
}