using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

public partial class Dashboard : System.Web.UI.Page
{
    [WebMethod]
    public static List<EUC> GetDashboardData()
    {
        List<EUC> listaEUC = new List<EUC>();

        string connectionString = ConfigurationManager.ConnectionStrings["PoliticasEUC"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string query = @"SELECT Id, NombreEUC, Certificacion, Documentacion, PlanAutomatizacion 
                             FROM EUC"; // Ajusta el nombre de la tabla si es distinto

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EUC euc = new EUC
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Certificacion = reader.GetString(2),
                        Documentacion = reader.GetString(3),
                        PlanAutomatizacion = reader.GetString(4),
                        Estado = CalcularEstado(reader.GetString(2), reader.GetString(3), reader.GetString(4))
                    };

                    listaEUC.Add(euc);
                }
            }
        }

        return listaEUC;
    }

    private static string CalcularEstado(string certificacion, string documentacion, string plan)
    {
        // Lógica para determinar el color/estado
        if (certificacion.Equals("Aprobada", StringComparison.OrdinalIgnoreCase) &&
            documentacion.Equals("Completa", StringComparison.OrdinalIgnoreCase) &&
            plan.Equals("Completo", StringComparison.OrdinalIgnoreCase))
        {
            return "Verde";
        }
        else if (certificacion.Equals("Rechazada", StringComparison.OrdinalIgnoreCase) ||
                 documentacion.Equals("Incompleta", StringComparison.OrdinalIgnoreCase) ||
                 plan.Equals("Incompleto", StringComparison.OrdinalIgnoreCase))
        {
            return "Rojo";
        }
        else
        {
            return "Azul"; // Pendiente o incompleto parcial
        }
    }
}