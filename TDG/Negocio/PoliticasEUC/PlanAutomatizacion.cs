using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* using Negocio.Generales */
/* using Transversal */
using System.Data.SqlClient;

namespace Negocio.PoliticasEUC
{
    public class PlanAutomatizacion
    {
        // Atributos
        public int IdPlan { get; set; }
        public string Responsable { get; set; }
        public string Plan { get; set; }
        public int EUCID { get; set; }
        public EUC EUC { get; set; }
        // Constructor
        public PlanAutomatizacion(int eucid, string responsable, string plan)
        {
            EUCID = eucid;
            Responsable = responsable;
            Plan = plan;
        }

        public class PlanAutomatizacionService
        {
            private string connectionString = "Server=localhost;Database=PoliticasEUC;Trusted_Connection=True;";

            private SqlConnection ObtenerConexion()
            {
                return new SqlConnection(connectionString);
            }

            // Listar todos los planes
            public List<PlanAutomatizacion> Listar()
            {
                List<PlanAutomatizacion> lista = new List<PlanAutomatizacion>();
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdPlan, Responsable, Plan, EUCID FROM PlanAutomatizacion", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new PlanAutomatizacion(
                            reader.GetInt32(3), // EUCID
                            reader.GetString(1), // Responsable
                            reader.GetString(2)  // Plan
                        )
                        {
                            IdPlan = reader.GetInt32(0)
                        });
                    }
                }
                return lista;
            }

            // Listar por EUC
            public List<PlanAutomatizacion> ListarPorEUC(int eucid)
            {
                List<PlanAutomatizacion> lista = new List<PlanAutomatizacion>();
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdPlan, Responsable, Plan, EUCID FROM PlanAutomatizacion WHERE EUCID = @EUCID", conn);
                    cmd.Parameters.AddWithValue("@EUCID", eucid);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new PlanAutomatizacion(
                            reader.GetInt32(3),
                            reader.GetString(1),
                            reader.GetString(2)
                        )
                        {
                            IdPlan = reader.GetInt32(0)
                        });
                    }
                }
                return lista;
            }

            // Crear
            public void Crear(PlanAutomatizacion nuevo)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO PlanAutomatizacion (Responsable, Plan, EUCID) VALUES (@Responsable, @Plan, @EUCID)", conn);
                    cmd.Parameters.AddWithValue("@Responsable", nuevo.Responsable);
                    cmd.Parameters.AddWithValue("@Plan", nuevo.Plan);
                    cmd.Parameters.AddWithValue("@EUCID", nuevo.EUCID);
                    cmd.ExecuteNonQuery();
                }
            }

            // Obtener por ID
            public PlanAutomatizacion ObtenerPorId(int id)
            {
                PlanAutomatizacion plan = null;
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdPlan, Responsable, Plan, EUCID FROM PlanAutomatizacion WHERE IdPlan = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        plan = new PlanAutomatizacion(
                            reader.GetInt32(3),
                            reader.GetString(1),
                            reader.GetString(2)
                        )
                        {
                            IdPlan = reader.GetInt32(0)
                        };
                    }
                }
                return plan;
            }

            // Actualizar
            public bool Actualizar(PlanAutomatizacion actualizado)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE PlanAutomatizacion SET Responsable = @Responsable, Plan = @Plan WHERE IdPlan = @IdPlan", conn);
                    cmd.Parameters.AddWithValue("@Responsable", actualizado.Responsable);
                    cmd.Parameters.AddWithValue("@Plan", actualizado.Plan);
                    cmd.Parameters.AddWithValue("@IdPlan", actualizado.IdPlan);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }

            // Eliminar
            public bool Eliminar(int id)
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM PlanAutomatizacion WHERE IdPlan = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
