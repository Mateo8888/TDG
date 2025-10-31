using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Negocio.Generales; 
// using Transversal;
using System.Data.SqlClient;

namespace Negocio.PoliticasEUC
{
    public class Certificacion
    {
        // Clave primaria (la setea el servicio)
        public int IdCert { get; set; }

        // Referencia al padre EUC (FK)
        public int EUCID { get; set; }
        public EUC EUC { get; set; }
        public string EstadoCert { get; set; } = "Pendiente";
        public DateTime FechaControl { get; set; }
        public string Observacion { get; set; }     
        public Certificacion() { }

        // Constructor principal
        public Certificacion(int eucid, string estadoCert = "Pendiente")
        {
            EUCID = eucid;
            EstadoCert = estadoCert;
        }

        public class CertificacionService
        {
            private string connectionString = "Server=localhost;Database=PoliticasEUC;Trusted_Connection=True;";

            private SqlConnection ObtenerConexion()
            {
                return new SqlConnection(connectionString);
            }

            // CREATE
            public Certificacion Crear(Certificacion nueva)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Certificacion (EUCId, EstadoCert, FechaControl, Observacion) " +
                        "VALUES (@EUCId, @EstadoCert, @FechaControl, @Observacion); SELECT SCOPE_IDENTITY();", conn);

                    cmd.Parameters.AddWithValue("@EUCId", nueva.EUCID);
                    cmd.Parameters.AddWithValue("@EstadoCert", nueva.EstadoCert ?? "Pendiente");
                    cmd.Parameters.AddWithValue("@FechaControl", nueva.FechaControl == default ? DateTime.Now : nueva.FechaControl);
                    cmd.Parameters.AddWithValue("@Observacion", nueva.Observacion ?? (object)DBNull.Value);

                    int idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                    nueva.IdCert = idGenerado;
                }
                return nueva;
            }

            // READ: Obtener por ID
            public Certificacion ObtenerPorId(int idCert)
            {
                Certificacion cert = null;
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Id, EUCId, EstadoCert, FechaControl, Observacion FROM Certificacion WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", idCert);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cert = new Certificacion
                        {
                            IdCert = reader.GetInt32(0),
                            EUCID = reader.GetInt32(1),
                            EstadoCert = reader.GetString(2),
                            FechaControl = reader.GetDateTime(3),
                            Observacion = reader.IsDBNull(4) ? null : reader.GetString(4)
                        };
                    }
                }
                return cert;
            }

            // READ: Listar todas
            public List<Certificacion> Listar()
            {
                List<Certificacion> lista = new List<Certificacion>();
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Id, EUCId, EstadoCert, FechaControl, Observacion FROM Certificacion", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Certificacion
                        {
                            IdCert = reader.GetInt32(0),
                            EUCID = reader.GetInt32(1),
                            EstadoCert = reader.GetString(2),
                            FechaControl = reader.GetDateTime(3),
                            Observacion = reader.IsDBNull(4) ? null : reader.GetString(4)
                        });
                    }
                }
                return lista;
            }

            // UPDATE
            public bool Actualizar(Certificacion actualizada)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Certificacion SET EstadoCert = @EstadoCert, FechaControl = @FechaControl, Observacion = @Observacion WHERE Id = @Id", conn);

                    cmd.Parameters.AddWithValue("@EstadoCert", actualizada.EstadoCert);
                    cmd.Parameters.AddWithValue("@FechaControl", actualizada.FechaControl);
                    cmd.Parameters.AddWithValue("@Observacion", actualizada.Observacion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", actualizada.IdCert);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }

            // DELETE
            public bool Eliminar(int idCert)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Certificacion WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", idCert);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }

    
 



