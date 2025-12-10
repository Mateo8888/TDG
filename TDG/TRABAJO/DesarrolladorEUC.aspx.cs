

using Negocio.PoliticasEUC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TRABAJO
{
    public partial class DesarrolladorEUC : System.Web.UI.Page
    {
        // Obtener cadena de conexión desde Web.config
        private static string connString = ConfigurationManager.ConnectionStrings["PoliticasEUC"].ConnectionString;
        private DataTable ObtenerEUCDesdeBD()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=PoliticasEUC;Integrated Security=True";
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                @"SELECT EUCID, Nombre, Descripcion, Criticidad, Estado, UsuariosActivos, Creador, VersionEUC 
          FROM dbo.EUC 
          ORDER BY EUCID DESC;", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            CargarEUC();
        }

        protected global::System.Web.UI.WebControls.PlaceHolder contenedorTarjetas;
        private void CargarEUC()
        {
            var dt = ObtenerEUCDesdeBD();
            contenedorTarjetas.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string id = row["EUCID"].ToString();
                string nombre = row["Nombre"].ToString();
                string descripcion = row["Descripcion"].ToString();
                string criticidad = row["Criticidad"].ToString();
                string estado = row["Estado"].ToString();
                string usuariosActivos = row["UsuariosActivos"].ToString();
                string creador = row["Creador"].ToString();
                string versionEuc = row["VersionEUC"].ToString();

                Panel card = new Panel { CssClass = "col-md-4 mb-3" };
                Panel innerCard = new Panel { CssClass = "card shadow-sm" };
                Panel body = new Panel { CssClass = "card-body" };

                body.Controls.Add(new Literal { Text = $"<h5 class='card-title'>{nombre}</h5><p>{descripcion}</p>" });
                body.Controls.Add(new Literal { Text = $"<span class='badge bg-primary'>{criticidad}</span> <span class='badge bg-secondary'>{estado}</span><hr>" });


                Button btnEditar = new Button
                {
                    Text = "Editar",
                    CssClass = "btn btn-sm btn-warning me-2",
                    CommandArgument = id
                };
                btnEditar.Click += BtnEditar_Click;


                Button btnEliminar = new Button
                {
                    Text = "Eliminar",
                    CssClass = "btn btn-sm btn-danger me-2",
                    CommandArgument = id
                };
                btnEliminar.Click += BtnEliminar_Click;


                Button btnPlan = new Button
                {
                    Text = "Plan",
                    CssClass = "btn btn-sm btn-info me-2",
                    CommandArgument = id
                };
                btnPlan.Click += BtnPlan_Click;


                Button btnDoc = new Button
                {
                    Text = "Documentación",
                    CssClass = "btn btn-sm btn-success",
                    CommandArgument = id
                };
                btnDoc.Click += BtnDoc_Click;


                body.Controls.Add(btnEditar);
                body.Controls.Add(btnEliminar);
                body.Controls.Add(new Literal { Text = "<br/><br/>" });
                body.Controls.Add(btnPlan);
                body.Controls.Add(btnDoc);

                innerCard.Controls.Add(body);
                card.Controls.Add(innerCard);
                contenedorTarjetas.Controls.Add(card);
            }
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string id = btn.CommandArgument;

            var dt = ObtenerEUCDesdeBD();
            DataRow[] rows = dt.Select($"EUCID = {id}");
            if (rows.Length > 0)
            {
                hfEUCID.Value = id; // Guardamos el ID
                txtNombre.Text = rows[0]["Nombre"].ToString();
                txtDescripcion.Text = rows[0]["Descripcion"].ToString();
                ddlCriticidad.SelectedValue = rows[0]["Criticidad"].ToString();
                ddlEstado.SelectedValue = rows[0]["Estado"].ToString();
                txtUsuariosActivos.Text = rows[0]["UsuariosActivos"].ToString();
                txtCreador.Text = rows[0]["Creador"].ToString();
                txtVersionEUC.Text = rows[0]["VersionEUC"].ToString();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#mdlEucNuevo').modal('show');", true);
        }

        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;

            if (!int.TryParse(btn.CommandArgument, out int eucid))
            {

                return;
            }

            string connectionString = "Data Source=localhost;Initial Catalog=PoliticasEUC;Integrated Security=True";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {

                        using (var cmdHijos = new SqlCommand(@"
                    DELETE FROM dbo.Certificacion       WHERE EUCID = @EUCID;
                    DELETE FROM dbo.Documentacion       WHERE EUCID = @EUCID;
                    DELETE FROM dbo.PlanAutomatizacion  WHERE EUCID = @EUCID;
                ", conn, tx))
                        {
                            cmdHijos.Parameters.Add("@EUCID", SqlDbType.Int).Value = eucid;
                            cmdHijos.ExecuteNonQuery();
                        }


                        using (var cmdPadre = new SqlCommand(@"DELETE FROM dbo.EUC WHERE EUCID = @EUCID;", conn, tx))
                        {
                            cmdPadre.Parameters.Add("@EUCID", SqlDbType.Int).Value = eucid;
                            int rows = cmdPadre.ExecuteNonQuery();

                            if (rows == 0)
                            {

                                tx.Rollback();

                                return;
                            }
                        }


                        tx.Commit();
                    }
                    catch (SqlException)
                    {
                        tx.Rollback();

                        throw;
                    }
                }
            }


            Response.Redirect("DesarrolladorEUC.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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

        private bool ExistePlan(string eucId)
        {
            string query = "SELECT COUNT(*) FROM PlanAutomatizacion WHERE EUCID = @EUCID";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EUCID", eucId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
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

        private bool ExisteDocumentacion(string eucId)
        {
            string query = "SELECT COUNT(*) FROM Documentacion WHERE EUCID = @EUCID";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EUCID", eucId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        protected global::System.Web.UI.WebControls.HiddenField hfPlanEUCID;
        protected global::System.Web.UI.WebControls.TextBox txtPlanResponsable;
        protected global::System.Web.UI.WebControls.TextBox txtPlan;
        protected global::System.Web.UI.WebControls.Button btnGuardarPlan;
        protected void BtnPlan_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string eucId = btn.CommandArgument;

            hfPlanEUCID.Value = eucId;


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

        protected global::System.Web.UI.WebControls.HiddenField hfDocEUCID;
        protected global::System.Web.UI.WebControls.TextBox txtDocNombreEUC;
        protected global::System.Web.UI.WebControls.TextBox txtDocProposito;
        protected global::System.Web.UI.WebControls.TextBox txtDocProceso;
        protected global::System.Web.UI.WebControls.TextBox txtDocUso;
        protected global::System.Web.UI.WebControls.TextBox txtDocInsumos;
        protected global::System.Web.UI.WebControls.TextBox txtDocResponsable;
        protected global::System.Web.UI.WebControls.TextBox txtDocTecnica;
        protected global::System.Web.UI.WebControls.TextBox txtDocEvControl;
        protected global::System.Web.UI.WebControls.Button btnGuardarDoc;
        protected void BtnDoc_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string eucId = btn.CommandArgument;

            hfDocEUCID.Value = eucId;

            // Limpia siempre
            txtDocNombreEUC.Text = "";
            txtDocProposito.Text = "";
            txtDocProceso.Text = "";
            txtDocUso.Text = "";
            txtDocInsumos.Text = "";
            txtDocResponsable.Text = "";
            txtDocTecnica.Text = "";
            txtDocEvControl.Text = "";

            // Carga si existe
            var doc = ObtenerDocumentacionPorEUC(eucId);
            if (doc != null)
            {
                txtDocNombreEUC.Text = doc["NombreEUC"]?.ToString();
                txtDocProposito.Text = doc["Proposito"]?.ToString();
                txtDocProceso.Text = doc["Proceso"]?.ToString();
                txtDocUso.Text = doc["Uso"]?.ToString();
                txtDocInsumos.Text = doc["Insumos"]?.ToString();
                txtDocResponsable.Text = doc["Responsable"]?.ToString();
                txtDocTecnica.Text = doc["DocTecnica"]?.ToString();
                txtDocEvControl.Text = doc["EvControl"]?.ToString();
            }

            ScriptManager.RegisterStartupScript(
     this,
     GetType(),
     Guid.NewGuid().ToString(),
     @"
    (function() {
        var el = document.getElementById('mdlDoc');
        if (!el) return;
        if (window.bootstrap && bootstrap.Modal) {
            var m = bootstrap.Modal.getOrCreateInstance(el);
            m.show();
        } else if (window.jQuery && $('#mdlDoc').modal) {
            $('#mdlDoc').modal('show');
        } else {
            console.warn('No se encontró Bootstrap para abrir el modal.');
        }
    })();",
     true
 );
        }

        protected void btnGuardarPlan_Click(object sender, EventArgs e)
        {
            string eucId = hfPlanEUCID.Value;
            string responsable = txtPlanResponsable.Text.Trim();
            string plan = txtPlan.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd;
                if (ExistePlan(eucId))
                {
                    cmd = new SqlCommand("UPDATE PlanAutomatizacion SET Responsable=@Responsable, [Plan]=@Plan WHERE EUCID=@EUCID", conn);
                }
                else
                {
                    cmd = new SqlCommand("INSERT INTO PlanAutomatizacion (EUCID, Responsable, [Plan]) VALUES (@EUCID, @Responsable, @Plan)", conn);
                }

                cmd.Parameters.AddWithValue("@EUCID", eucId);
                cmd.Parameters.AddWithValue("@Responsable", responsable);
                cmd.Parameters.AddWithValue("@Plan", plan);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("DesarrolladorEUC.aspx");
        }

        protected void btnGuardarDoc_Click(object sender, EventArgs e)
        {
            string eucId = hfDocEUCID.Value;
            string nombreEUC = txtDocNombreEUC.Text.Trim();
            string proposito = txtDocProposito.Text.Trim();
            string proceso = txtDocProceso.Text.Trim();
            string uso = txtDocUso.Text.Trim();
            string insumos = txtDocInsumos.Text.Trim();
            string responsable = txtDocResponsable.Text.Trim();
            string tecnica = txtDocTecnica.Text.Trim();
            string evidencia = txtDocEvControl.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd;
                if (ExisteDocumentacion(eucId))
                {
                    cmd = new SqlCommand(@"
                UPDATE Documentacion 
                SET NombreEUC=@NombreEUC, Proposito=@Proposito, Proceso=@Proceso, Uso=@Uso, 
                    Insumos=@Insumos, Responsable=@Responsable, DocTecnica=@Tecnica, EvControl=@Evidencia 
                WHERE EUCID=@EUCID", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"
                INSERT INTO Documentacion (EUCID, NombreEUC, Proposito, Proceso, Uso, Insumos, Responsable, DocTecnica, EvControl) 
                VALUES (@EUCID, @NombreEUC, @Proposito, @Proceso, @Uso, @Insumos, @Responsable, @Tecnica, @Evidencia)", conn);
                }

                cmd.Parameters.AddWithValue("@EUCID", eucId);
                cmd.Parameters.AddWithValue("@NombreEUC", nombreEUC);
                cmd.Parameters.AddWithValue("@Proposito", proposito);
                cmd.Parameters.AddWithValue("@Proceso", proceso);
                cmd.Parameters.AddWithValue("@Uso", uso);
                cmd.Parameters.AddWithValue("@Insumos", insumos);
                cmd.Parameters.AddWithValue("@Responsable", responsable);
                cmd.Parameters.AddWithValue("@Tecnica", tecnica);
                cmd.Parameters.AddWithValue("@Evidencia", evidencia);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("DesarrolladorEUC.aspx");
        }

        protected global::System.Web.UI.WebControls.GridView gvEUC;

        private void BindEUCs()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=PoliticasEUC;Integrated Security=True";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                @"SELECT EUCID, Nombre, Descripcion, Criticidad, Estado, UsuariosActivos, Creador, VersionEUC
          FROM dbo.EUC
          ORDER BY EUCID DESC;", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                gvEUC.DataSource = dt;
                gvEUC.DataBind();
            }
        }


        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        protected global::System.Web.UI.WebControls.TextBox txtDescripcion;
        protected global::System.Web.UI.WebControls.DropDownList ddlCriticidad;
        protected global::System.Web.UI.WebControls.DropDownList ddlEstado;
        protected global::System.Web.UI.WebControls.Button btnGuardarEUC;
        protected global::System.Web.UI.WebControls.HiddenField hfEUCID;
        protected global::System.Web.UI.WebControls.TextBox txtUsuariosActivos;
        protected global::System.Web.UI.WebControls.ValidationSummary vsEUC;
        protected global::System.Web.UI.WebControls.TextBox txtCreador;
        protected global::System.Web.UI.WebControls.TextBox txtVersionEUC;




        protected void btnGuardarEUC_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();
            string criticidad = ddlCriticidad.SelectedValue;
            string estado = ddlEstado.SelectedValue;
            string creador = txtCreador.Text.Trim();
            string versionEUC = txtVersionEUC.Text.Trim();
            string id = hfEUCID.Value;

            int? usuariosActivos = null;
            if (!string.IsNullOrWhiteSpace(txtUsuariosActivos.Text))
            {
                if (int.TryParse(txtUsuariosActivos.Text.Trim(), out int valor))
                {
                    usuariosActivos = valor;
                }
                else
                {
                    vsEUC.HeaderText = "Error: El campo Usuarios Activos debe ser numérico.";
                    return;
                }
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(descripcion) ||
                string.IsNullOrEmpty(creador) || string.IsNullOrEmpty(versionEUC))
            {
                return;
            }

            string connectionString = "Data Source=localhost;Initial Catalog=PoliticasEUC;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                if (!string.IsNullOrEmpty(id))
                {
                    string query = @"UPDATE EUC 
                             SET Nombre=@Nombre, Descripcion=@Descripcion, Criticidad=@Criticidad, Estado=@Estado, 
                                 UsuariosActivos=@UsuariosActivos, Creador=@Creador, VersionEUC=@VersionEUC
                             WHERE EUCID=@EUCID";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EUCID", id);
                }
                else
                {
                    string query = @"INSERT INTO EUC (Nombre, Descripcion, Criticidad, Estado, UsuariosActivos, Creador, VersionEUC)
                             VALUES (@Nombre, @Descripcion, @Criticidad, @Estado, @UsuariosActivos, @Creador, @VersionEUC)";
                    cmd = new SqlCommand(query, conn);
                }

                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@Creador", creador);
                cmd.Parameters.AddWithValue("@VersionEUC", versionEUC);

                if (usuariosActivos.HasValue)
                    cmd.Parameters.AddWithValue("@UsuariosActivos", usuariosActivos.Value);
                else
                    cmd.Parameters.AddWithValue("@UsuariosActivos", DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            hfEUCID.Value = "";
            Response.Redirect("DesarrolladorEUC.aspx");
        }


    }
}