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
    public class Documentacion
    {
        // Clave primaria de Documentación
        public int IDoc { get; set; }

        // FK hacia el padre EUC
        public int EUCID { get; set; }

        // Atributos
        public string NombreEUC { get; set; }
        public string Proposito { get; set; }
        public string Proceso { get; set; }
        public string Uso { get; set; }
        public string Insumos { get; set; }
        public string Responsable { get; set; }
        public string DocTecnica { get; set; }
        public string EvControl { get; set; }
        public EUC EUC { get; set; }       

        // Constructor por defecto 
        public Documentacion() { }

        // Constructor principal: 
        public Documentacion(
            int eucid,
            string nombreEUC,
            string proposito,
            string proceso,
            string uso,
            string insumos,
            string responsable,
            string docTecnica,
            string evControl)
        {
            EUCID = eucid;
            NombreEUC = nombreEUC;
            Proposito = proposito;
            Proceso = proceso;
            Uso = uso;
            Insumos = insumos;
            Responsable = responsable;
            DocTecnica = docTecnica;
            EvControl = evControl;
        }

        public class DocumentacionService
        {
            private string connectionString = "Server=localhost;Database=PoliticasEUC;Trusted_Connection=True;";

            private SqlConnection ObtenerConexion()
            {
                return new SqlConnection(connectionString);
            }

            // CREATE
            public Documentacion CrearDocumentacion(Documentacion nueva)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Documentacion 
                      (EUCId, NombreEUC, Proposito, Proceso, Uso, Insumos, Responsable, DocTecnica, EvControl)
                      VALUES (@EUCId, @NombreEUC, @Proposito, @Proceso, @Uso, @Insumos, @Responsable, @DocTecnica, @EvControl);
                      SELECT SCOPE_IDENTITY();", conn);

                    cmd.Parameters.AddWithValue("@EUCId", nueva.EUCID);
                    cmd.Parameters.AddWithValue("@NombreEUC", nueva.NombreEUC);
                    cmd.Parameters.AddWithValue("@Proposito", nueva.Proposito);
                    cmd.Parameters.AddWithValue("@Proceso", nueva.Proceso);
                    cmd.Parameters.AddWithValue("@Uso", nueva.Uso);
                    cmd.Parameters.AddWithValue("@Insumos", nueva.Insumos);
                    cmd.Parameters.AddWithValue("@Responsable", nueva.Responsable);
                    cmd.Parameters.AddWithValue("@DocTecnica", nueva.DocTecnica);
                    cmd.Parameters.AddWithValue("@EvControl", nueva.EvControl);

                    int idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                    nueva.IDoc = idGenerado;
                }
                return nueva;
            }

            // READ - Todas
            public List<Documentacion> ObtenerTodas()
            {
                List<Documentacion> lista = new List<Documentacion>();
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Id, EUCId, NombreEUC, Proposito, Proceso, Uso, Insumos, Responsable, DocTecnica, EvControl FROM Documentacion", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Documentacion
                        {
                            IDoc = reader.GetInt32(0),
                            EUCID = reader.GetInt32(1),
                            NombreEUC = reader.GetString(2),
                            Proposito = reader.GetString(3),
                            Proceso = reader.GetString(4),
                            Uso = reader.GetString(5),
                            Insumos = reader.GetString(6),
                            Responsable = reader.GetString(7),
                            DocTecnica = reader.GetString(8),
                            EvControl = reader.GetString(9)
                        });
                    }
                }
                return lista;
            }

            // READ - Por EUC
            public List<Documentacion> ObtenerPorEUC(int eucId)
            {
                List<Documentacion> lista = new List<Documentacion>();
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Id, EUCId, NombreEUC, Proposito, Proceso, Uso, Insumos, Responsable, DocTecnica, EvControl FROM Documentacion WHERE EUCId = @EUCId", conn);
                    cmd.Parameters.AddWithValue("@EUCId", eucId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Documentacion
                        {
                            IDoc = reader.GetInt32(0),
                            EUCID = reader.GetInt32(1),
                            NombreEUC = reader.GetString(2),
                            Proposito = reader.GetString(3),
                            Proceso = reader.GetString(4),
                            Uso = reader.GetString(5),
                            Insumos = reader.GetString(6),
                            Responsable = reader.GetString(7),
                            DocTecnica = reader.GetString(8),
                            EvControl = reader.GetString(9)
                        });
                    }
                }
                return lista;
            }

            // UPDATE
            public bool ActualizarDocumentacion(Documentacion actualizada)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"UPDATE Documentacion SET 
                      NombreEUC = @NombreEUC, Proposito = @Proposito, Proceso = @Proceso, Uso = @Uso, 
                      Insumos = @Insumos, Responsable = @Responsable, DocTecnica = @DocTecnica, EvControl = @EvControl
                      WHERE Id = @Id", conn);

                    cmd.Parameters.AddWithValue("@NombreEUC", actualizada.NombreEUC);
                    cmd.Parameters.AddWithValue("@Proposito", actualizada.Proposito);
                    cmd.Parameters.AddWithValue("@Proceso", actualizada.Proceso);
                    cmd.Parameters.AddWithValue("@Uso", actualizada.Uso);
                    cmd.Parameters.AddWithValue("@Insumos", actualizada.Insumos);
                    cmd.Parameters.AddWithValue("@Responsable", actualizada.Responsable);
                    cmd.Parameters.AddWithValue("@DocTecnica", actualizada.DocTecnica);
                    cmd.Parameters.AddWithValue("@EvControl", actualizada.EvControl);
                    cmd.Parameters.AddWithValue("@Id", actualizada.IDoc);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }

            // DELETE
            public bool EliminarDocumentacion(int idDoc)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Documentacion WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", idDoc);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}


