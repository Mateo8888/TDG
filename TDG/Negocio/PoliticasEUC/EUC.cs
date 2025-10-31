using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
/* using Negocio.Generales */
/* using Transversal */

namespace Negocio.PoliticasEUC
{
    public class EUC
    {
        public int EUCID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Criticidad { get; set; }
        public string Estado { get; set; }

        // Navegación (opción B)
        public List<Certificacion> Certificaciones { get; set; } = new List<Certificacion>();
        public List<Documentacion> Documentaciones { get; set; } = new List<Documentacion>();
        public List<PlanAutomatizacion> Planes { get; set; } = new List<PlanAutomatizacion>();

        public class EUCService
        {
            private readonly string _connectionString = "Server=localhost;Database=PoliticasEUC;Trusted_Connection=True;";

            private SqlConnection ObtenerConexion()
            {
                return new SqlConnection(_connectionString);
            }

            // CREATE
            public void CrearEUC(string nombre, string descripcion, string criticidad, string estado)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    string query = "INSERT INTO EUC (Nombre, Descripcion, Criticidad, Estado) VALUES (@Nombre, @Descripcion, @Criticidad, @Estado)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.ExecuteNonQuery();
                }
            }

            // READ - Todas
            public List<EUC> ObtenerTodas()
            {
                List<EUC> lista = new List<EUC>();
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT EUCID, Nombre, Descripcion, Criticidad, Estado FROM EUC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new EUC
                        {
                            EUCID = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            Criticidad = reader.GetString(3),
                            Estado = reader.GetString(4)
                        });
                    }
                }
                return lista;
            }

            // READ - Por ID
            public EUC ObtenerPorId(int id)
            {
                EUC euc = null;
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT EUCID, Nombre, Descripcion, Criticidad, Estado FROM EUC WHERE EUCID = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        euc = new EUC
                        {
                            EUCID = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            Criticidad = reader.GetString(3),
                            Estado = reader.GetString(4)
                        };
                    }
                }
                return euc;
            }

            // UPDATE
            public bool ActualizarEUC(int id, string nombre, string descripcion, string criticidad, string estado)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    string query = "UPDATE EUC SET Nombre=@Nombre, Descripcion=@Descripcion, Criticidad=@Criticidad, Estado=@Estado WHERE EUCID=@Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }

            // DELETE
            public bool EliminarEUC(int id)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    string query = "DELETE FROM EUC WHERE EUCID=@Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
