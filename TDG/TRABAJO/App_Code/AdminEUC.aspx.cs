using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class administradoreuc : System.Web.UI.Page
{
    private static string connString = ConfigurationManager.ConnectionStrings["PoliticasEUC"].ConnectionString;

    public class EUC
    {
        public int EUCID { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Criticidad { get; set; }
    }

    public class EUCDetails
    {
        public EUC Info { get; set; }
        public string Plan { get; set; }
        public string ResponsablePlan { get; set; }
        public string Documentacion { get; set; } // Podrías devolver JSON con todos los campos si lo necesitas
        public string EstadoCertificacion { get; set; }
    }

    // ============================
    // Listar EUCs
    // ============================
    [WebMethod]
    public static List<EUC> GetEUCList()
    {
        List<EUC> lista = new List<EUC>();
        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = "SELECT EUCID, Nombre, Estado, Criticidad FROM EUC";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new EUC
                {
                    EUCID = Convert.ToInt32(reader["EUCID"]),
                    Nombre = reader["Nombre"].ToString(),
                    Estado = reader["Estado"].ToString(),
                    Criticidad = reader["Criticidad"].ToString()
                });
            }
        }
        return lista;
    }

    // ============================
    // Detalles de EUC
    // ============================
    [WebMethod]
    public static EUCDetails GetEUCDetails(int id)
    {
        EUCDetails details = new EUCDetails();
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();

            // Info EUC
            string queryEUC = "SELECT EUCID, Nombre, Estado, Criticidad FROM EUC WHERE EUCID=@Id";
            SqlCommand cmdEUC = new SqlCommand(queryEUC, conn);
            cmdEUC.Parameters.AddWithValue("@Id", id);
            SqlDataReader readerEUC = cmdEUC.ExecuteReader();
            if (readerEUC.Read())
            {
                details.Info = new EUC
                {
                    EUCID = Convert.ToInt32(readerEUC["EUCID"]),
                    Nombre = readerEUC["Nombre"].ToString(),
                    Estado = readerEUC["Estado"].ToString(),
                    Criticidad = readerEUC["Criticidad"].ToString()
                };
            }
            readerEUC.Close();

            // Plan
            string queryPlan = "SELECT Responsable, [Plan] FROM PlanAutomatizacion WHERE EUCID=@Id";
            SqlCommand cmdPlan = new SqlCommand(queryPlan, conn);
            cmdPlan.Parameters.AddWithValue("@Id", id);
            SqlDataReader readerPlan = cmdPlan.ExecuteReader();
            if (readerPlan.Read())
            {
                details.ResponsablePlan = readerPlan["Responsable"].ToString();
                details.Plan = readerPlan["Plan"].ToString();
            }
            readerPlan.Close();

            // Documentación (simplificado)
            string queryDoc = "SELECT Proposito FROM Documentacion WHERE EUCID=@Id";
            SqlCommand cmdDoc = new SqlCommand(queryDoc, conn);
            cmdDoc.Parameters.AddWithValue("@Id", id);
            SqlDataReader readerDoc = cmdDoc.ExecuteReader();
            if (readerDoc.Read())
            {
                details.Documentacion = readerDoc["Proposito"].ToString();
            }
            readerDoc.Close();

            // Certificación
            string queryCert = "SELECT EstadoCert FROM Certificacion WHERE EUCID=@Id";
            SqlCommand cmdCert = new SqlCommand(queryCert, conn);
            cmdCert.Parameters.AddWithValue("@Id", id);
            SqlDataReader readerCert = cmdCert.ExecuteReader();
            if (readerCert.Read())
            {
                details.EstadoCertificacion = readerCert["EstadoCert"].ToString();
            }
            readerCert.Close();
        }
        return details;
    }

    // ============================
    // Aprobar EUC
    // ============================
    [WebMethod]
    public static string ApproveEUC(int id)
    {
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            string query = "UPDATE Certificacion SET EstadoCert='Aprobado', FechaControl=GETDATE() WHERE EUCID=@Id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();

            // Actualizar estado EUC
            string updateEUC = "UPDATE EUC SET Estado='Completo' WHERE EUCID=@Id";
            SqlCommand cmdEUC = new SqlCommand(updateEUC, conn);
            cmdEUC.Parameters.AddWithValue("@Id", id);
            cmdEUC.ExecuteNonQuery();
        }
        return "EUC aprobada correctamente";
    }

    // ============================
    // Rechazar EUC
    // ============================
    [WebMethod]
    public static string RejectEUC(int id)
    {
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            string query = "UPDATE Certificacion SET EstadoCert='Rechazado', FechaControl=GETDATE() WHERE EUCID=@Id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();

            // Actualizar estado EUC
            string updateEUC = "UPDATE EUC SET Estado='Rechazado' WHERE EUCID=@Id";
            SqlCommand cmdEUC = new SqlCommand(updateEUC, conn);
            cmdEUC.Parameters.AddWithValue("@Id", id);
            cmdEUC.ExecuteNonQuery();
        }
        return "EUC rechazada correctamente";
    }
}