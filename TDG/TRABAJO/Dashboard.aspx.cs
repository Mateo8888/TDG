using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace TRABAJO
{
    public partial class Dashboard : System.Web.UI.Page
    {

        private static string connString = ConfigurationManager.ConnectionStrings["PoliticasEUC"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDashboard();
            }
        }

        protected System.Web.UI.HtmlControls.HtmlGenericControl kpiTotal;
        protected System.Web.UI.HtmlControls.HtmlGenericControl kpiPlan;
        protected System.Web.UI.HtmlControls.HtmlGenericControl kpiDoc;
        protected System.Web.UI.HtmlControls.HtmlGenericControl kpiAprob;
        protected System.Web.UI.HtmlControls.HtmlGenericControl kpiRech;
        protected System.Web.UI.HtmlControls.HtmlGenericControl kpiPend;
        protected System.Web.UI.HtmlControls.HtmlGenericControl detNombre;
        protected System.Web.UI.HtmlControls.HtmlGenericControl detCrit;
        protected System.Web.UI.HtmlControls.HtmlGenericControl detEstado;
        protected System.Web.UI.HtmlControls.HtmlGenericControl detPlan;
        protected System.Web.UI.HtmlControls.HtmlGenericControl detDoc;
        protected System.Web.UI.HtmlControls.HtmlGenericControl detCert;
        private void CargarDashboard()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT e.EUCID, e.Nombre, e.Criticidad, e.Estado,
                                        CASE WHEN p.IdPlan IS NOT NULL THEN 1 ELSE 0 END AS TienePlan,
                                        CASE WHEN d.IDoc IS NOT NULL THEN 1 ELSE 0 END AS TieneDoc,
                                        ISNULL(c.EstadoCert, 'Pendiente') AS Certificacion
                                 FROM EUC e
                                 LEFT JOIN PlanAutomatizacion p ON e.EUCID = p.EUCID
                                 LEFT JOIN Documentacion d ON e.EUCID = d.EUCID
                                 LEFT JOIN Certificacion c ON e.EUCID = c.EUCID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDashboard.DataSource = dt;
                gvDashboard.DataBind();

                // KPIs
                kpiTotal.InnerText = dt.Rows.Count.ToString();
                kpiPlan.InnerText = dt.Select("TienePlan = 1").Length.ToString();
                kpiDoc.InnerText = dt.Select("TieneDoc = 1").Length.ToString();
                kpiAprob.InnerText = dt.Select("Certificacion = 'Aprobada'").Length.ToString();
                kpiRech.InnerText = dt.Select("Certificacion = 'Rechazada'").Length.ToString();
                kpiPend.InnerText = dt.Select("Certificacion = 'Pendiente'").Length.ToString();
            }
        }

        protected string GetColorClass(object valor, string tipo)
        {
            if (tipo == "estado")
            {
                switch (valor.ToString())
                {
                    case "Activa": return "badge-estado-activa";
                    case "En construcción": return "badge-estado-enconstruccion";
                    case "Jubilada": return "badge-estado-jubilada";
                }
            }
            else if (tipo == "doc" || tipo == "plan")
            {
                return Convert.ToBoolean(valor) ? "chip chip-ok" : "chip chip-bad";
            }
            else if (tipo == "cert")
            {
                switch (valor.ToString())
                {
                    case "Aprobada": return "chip chip-ok";
                    case "Rechazada": return "chip chip-bad";
                    default: return "chip chip-warn";
                }
            }
            return "";
        }

        protected void gvDashboard_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int eucid = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "VerDoc" || e.CommandName == "VerPlan" || e.CommandName == "VerCert")
            {
                MostrarDetalle(eucid);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#mdlDetalle').modal('show');", true);
            }
        }

        protected System.Web.UI.HtmlControls.HtmlGenericControl Span1; // Nombre
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span2; // Criticidad
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span3; // Estado
        protected System.Web.UI.HtmlControls.HtmlGenericControl Div1;  // Plan
        protected System.Web.UI.HtmlControls.HtmlGenericControl Div2;  // Documentación
        protected System.Web.UI.HtmlControls.HtmlGenericControl Div3;  // Certificación
        private void MostrarDetalle(int eucid)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT e.Nombre, e.Criticidad, e.Estado,
               p.[Plan],
               d.Proposito, d.Proceso, d.Uso, d.Insumos, d.DocTecnica,
               ISNULL(c.EstadoCert, 'Pendiente') AS EstadoCert
        FROM EUC e
        LEFT JOIN PlanAutomatizacion p ON e.EUCID = p.EUCID
        LEFT JOIN Documentacion d ON e.EUCID = d.EUCID
        LEFT JOIN Certificacion c ON e.EUCID = c.EUCID
        WHERE e.EUCID = @id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", eucid);
                conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        detNombre.InnerText = dr["Nombre"]?.ToString();

                        detCrit.InnerText = dr["Criticidad"]?.ToString();
                        detEstado.InnerText = dr["Estado"]?.ToString();

                        // Plan
                        string plan = dr["Plan"]?.ToString();
                        detPlan.InnerText = string.IsNullOrWhiteSpace(plan) ? "N/D" : plan;

                        // Documentación (con saltos de línea)
                        string proposito = dr["Proposito"]?.ToString();
                        if (string.IsNullOrWhiteSpace(proposito))
                        {
                            detDoc.InnerText = "N/D";
                        }
                        else
                        {
                            detDoc.InnerText =
                                $"Propósito: {dr["Proposito"]}\n" +
                                $"Proceso: {dr["Proceso"]}\n" +
                                $"Uso: {dr["Uso"]}\n" +
                                $"Insumos: {dr["Insumos"]}\n" +
                                $"Doc Técnica: {dr["DocTecnica"]}";
                        }

                        // Certificación
                        detCert.InnerText = dr["EstadoCert"]?.ToString();
                    }
                }
            }
        }
    }
    
 }
