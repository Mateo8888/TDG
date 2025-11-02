using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

public partial class Dashboard : System.Web.UI.Page
{
    public class EUCDto
    {
        public int EUCID { get; set; }
        public string Nombre { get; set; }
        public string Criticidad { get; set; }   // ALTA|MEDIA|BAJA
        public string Estado { get; set; }       // Activa|En construcción|Jubilada
        public string Certificacion { get; set; }      // Aprobado|Rechazado|Pendiente
        public string Documentacion { get; set; }      // Completa|Incompleta
        public string PlanAutomatizacion { get; set; } // Completo|Incompleto
        public string EstadoColor { get; set; }        // Verde|Rojo|Azul (para pintar)
    }

    [WebMethod]
    public static List<EUCDto> GetDashboardData()
    {
        var list = new List<EUCDto>();
        var cs = ConfigurationManager.ConnectionStrings["PoliticasEUC"].ConnectionString;

        using (var conn = new SqlConnection(cs))
        {
            conn.Open();

            // Arma estados del dashboard a partir de tus tablas
            var sql = @"
SELECT
    e.EUCID,
    e.Nombre,
    e.Criticidad,
    e.Estado,
    ISNULL(c.EstadoCert, 'Pendiente')       AS Certificacion,
    CASE WHEN d.IDoc IS NULL  THEN 'Incompleta' ELSE 'Completa'   END AS Documentacion,
    CASE WHEN p.IdPlan IS NULL THEN 'Incompleto' ELSE 'Completo'  END AS PlanAutomatizacion
FROM EUC e
LEFT JOIN Certificacion      c ON c.EUCID = e.EUCID
LEFT JOIN Documentacion      d ON d.EUCID = e.EUCID
LEFT JOIN PlanAutomatizacion p ON p.EUCID = e.EUCID
ORDER BY e.EUCID DESC;";

            using (var cmd = new SqlCommand(sql, conn))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    var cert = r["Certificacion"].ToString();
                    var doc = r["Documentacion"].ToString();
                    var plan = r["PlanAutomatizacion"].ToString();

                    list.Add(new EUCDto
                    {
                        EUCID = Convert.ToInt32(r["EUCID"]),
                        Nombre = r["Nombre"].ToString(),
                        Criticidad = r["Criticidad"].ToString(),
                        Estado = r["Estado"].ToString(),
                        Certificacion = cert,
                        Documentacion = doc,
                        PlanAutomatizacion = plan,
                        EstadoColor = CalcularEstado(cert, doc, plan) // 'Verde'|'Rojo'|'Azul'
                    });
                }
            }
        }
        return list;
    }

    private static string CalcularEstado(string certificacion, string documentacion, string plan)
    {
        certificacion = (certificacion ?? "").ToLowerInvariant();
        documentacion = (documentacion ?? "").ToLowerInvariant();
        plan = (plan ?? "").ToLowerInvariant();

        if ((certificacion.StartsWith("aprob")) &&
            (documentacion == "completa") &&
            (plan == "completo"))
            return "Verde";

        if (certificacion.StartsWith("rech") ||
            documentacion == "incompleta" ||
            plan == "incompleto")
            return "Rojo";

        return "Azul"; // pendiente / parcial
    }
}
