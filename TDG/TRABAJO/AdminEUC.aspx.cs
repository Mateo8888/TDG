using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TRABAJO
{
    public partial class AdminEUC : System.Web.UI.Page
    {
        private string connString = "Data Source=localhost;Initial Catalog=PoliticasEUC;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEUCs();
            }
        }

        private void CargarEUCs()
        {
            DataTable dt = ObtenerEUCDesdeBD();
            rptEUCs.DataSource = dt;
            rptEUCs.DataBind();
        }


        private DataTable ObtenerEUCDesdeBD()
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(@"
        SELECT 
            e.EUCID,
            e.Nombre,
            e.Descripcion,
            e.Criticidad,
            e.Estado,
            e.Creador,
            e.VersionEUC,
            ISNULL(c.EstadoCert, 'Pendiente') AS EstadoCert,
            c.FechaControl
        FROM dbo.EUC e
        LEFT JOIN Certificacion c ON e.EUCID = c.EUCID
        ORDER BY e.EUCID DESC;", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }


        private DataRow ObtenerPlanPorEUC(string eucId)
        {
            string query = "SELECT * FROM PlanAutomatizacion WHERE EUCID = @EUCID";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EUCID", eucId);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    conn.Open();
                    da.Fill(dt);
                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
        }

        private DataRow ObtenerDocumentacionPorEUC(string eucId)
        {
            string query = "SELECT * FROM Documentacion WHERE EUCID = @EUCID";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EUCID", eucId);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    conn.Open();
                    da.Fill(dt);
                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
        }

        protected void rptEUCs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string eucId = e.CommandArgument.ToString();

            if (e.CommandName == "VerPlan")
            {
                MostrarModalPlan(eucId);
            }
            else if (e.CommandName == "VerDoc")
            {
                MostrarModalDoc(eucId);
            }
            else if (e.CommandName == "Aprobar" || e.CommandName == "Rechazar")
            {
                TextBox txtComentario = (TextBox)e.Item.FindControl("txtComentario");
                if (string.IsNullOrWhiteSpace(txtComentario.Text))
                {
                    lblError.Text = "Debe ingresar un comentario.";
                    return;
                }

                bool aprobado = e.CommandName == "Aprobar";
                CertificarEUC(eucId, aprobado, txtComentario.Text);
                lblMsg.Text = aprobado ? "EUC aprobada correctamente." : "EUC rechazada correctamente.";
                lblError.Text = "";
                CargarEUCs();
            }
        }

        private void MostrarModalPlan(string eucId)
        {
            var plan = ObtenerPlanPorEUC(eucId);
            if (plan != null)
            {
                txtPlanResponsable.Text = plan["Responsable"].ToString();
                txtPlan.Text = plan["Plan"].ToString();
            }
            else
            {
                txtPlanResponsable.Text = "";
                txtPlan.Text = "";
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPlanModal", "$('#mdlPlan').modal('show');", true);
        }
        private void MostrarModalDoc(string eucId)
        {
            var doc = ObtenerDocumentacionPorEUC(eucId);
            if (doc != null)
            {
                txtDocNombreEUC.Text = doc["NombreEUC"].ToString();
                txtDocProposito.Text = doc["Proposito"].ToString();
                txtDocProceso.Text = doc["Proceso"].ToString();
                txtDocUso.Text = doc["Uso"].ToString();
                txtDocInsumos.Text = doc["Insumos"].ToString();
                txtDocResponsable.Text = doc["Responsable"].ToString();
                txtDocTecnica.Text = doc["DocTecnica"].ToString();
                txtDocEvControl.Text = doc["EvControl"].ToString();
            }
            else
            {
                txtDocNombreEUC.Text = "";
                txtDocProposito.Text = "";
                txtDocProceso.Text = "";
                txtDocUso.Text = "";
                txtDocInsumos.Text = "";
                txtDocResponsable.Text = "";
                txtDocTecnica.Text = "";
                txtDocEvControl.Text = "";
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowDocModal", "$('#mdlDoc').modal('show');", true);
        }
        private void CertificarEUC(string eucId, bool aprobado, string comentario)
        {
            const string updateSql = @"
        UPDATE Certificacion
        SET EstadoCert = @EstadoCert,
            Observacion = @Observacion,
            FechaControl = GETDATE()
        WHERE EUCID = @EUCID";

            const string insertSql = @"
        INSERT INTO Certificacion (EUCID, EstadoCert, FechaControl, Observacion)
        VALUES (@EUCID, @EstadoCert, GETDATE(), @Observacion)";

            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(updateSql, conn))
            {
                cmd.Parameters.AddWithValue("@EstadoCert", aprobado ? "Aprobada" : "Rechazada");
                cmd.Parameters.AddWithValue("@Observacion", comentario);
                cmd.Parameters.AddWithValue("@EUCID", eucId);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                // Si no había registro, inserta
                if (rows == 0)
                {
                    cmd.CommandText = insertSql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetCertificacionClass(string estadoCert)
        {
            switch (estadoCert.ToUpper())
            {
                case "APROBADA": return "bg-success text-white"; // verde
                case "RECHAZADA": return "bg-danger text-white"; // rojo
                default: return "bg-warning"; // amarillo para pendiente
            }
        }
        public string GetCriticidadClass(string criticidad)
        {
            switch (criticidad.ToUpper())
            {
                case "ALTA": return "badge badge-crit-alta";
                case "MEDIA": return "badge badge-crit-media";
                case "BAJA": return "badge badge-crit-baja";
                default: return "badge bg-secondary";
            }
        }

        public string GetEstadoClass(string estado)
        {
            switch (estado.ToUpper())
            {
                case "ACTIVA": return "badge badge-estado-activa";
                case "EN CONSTRUCCIÓN": return "badge badge-estado-enconstruccion";
                case "JUBILADA": return "badge badge-estado-jubilada";
                default: return "badge bg-secondary";
            }
        }
    }
}