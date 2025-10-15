using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio.PoliticasEUC;

namespace TuProyecto
{
    public partial class Desarrollador : Page
    {
        private EUC.EUCService eucSvc = ServiceRegistry.EUCs;
        private Documentacion.DocumentacionService docSvc = ServiceRegistry.Docs;
        private PlanAutomatizacion.PlanAutomatizacionService planSvc = ServiceRegistry.Planes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEUCs();
            }
        }

        private void CargarEUCs()
        {
            grdEUCs.DataSource = eucSvc.ObtenerTodas();
            grdEUCs.DataBind();
        }

        protected void btnAgregarEUC_Click(object sender, EventArgs e)
        {
            hfEUCID.Value = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            ddlCriticidad.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            txtAdminUsuario.Text = "";
        }

        protected void btnGuardarEUC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfEUCID.Value))
            {
                var nueva = new EUC
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    Criticidad = ddlCriticidad.SelectedValue,
                    Estado = ddlEstado.SelectedValue,
                    AdminUsuario = txtAdminUsuario.Text.Trim()
                };
                eucSvc.CrearEUC(nueva.Nombre, nueva.Descripcion, nueva.Criticidad, nueva.Estado);
            }
            else
            {
                int id = int.Parse(hfEUCID.Value);
                eucSvc.ActualizarEUC(id, txtNombre.Text.Trim(), ddlCriticidad.SelectedValue, ddlEstado.SelectedValue, txtDescripcion.Text.Trim());
                eucSvc.AsignarAdminAEUC(id, txtAdminUsuario.Text.Trim());
            }
            CargarEUCs();
        }

        protected void grdEUCs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int eucid = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                var euc = eucSvc.ObtenerPorId(eucid);
                if (euc != null)
                {
                    hfEUCID.Value = euc.EUCID.ToString();
                    txtNombre.Text = euc.Nombre;
                    txtDescripcion.Text = euc.Descripcion;
                    ddlCriticidad.SelectedValue = euc.Criticidad;
                    ddlEstado.SelectedValue = euc.Estado;
                    txtAdminUsuario.Text = euc.AdminUsuario;
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                eucSvc.EliminarEUC(eucid);
                CargarEUCs();
            }
            else if (e.CommandName == "Ver")
            {
                hfEUCID_Doc.Value = eucid.ToString();
                hfEUCID_Plan.Value = eucid.ToString();
                var euc = eucSvc.ObtenerPorId(eucid);
                if (euc != null && euc.Criticidad == "Alta")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "abrirPlan", "$('#modalPlan').modal('show');", true);
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "abrirDoc", "$('#modalDocumentacion').modal('show');", true);
            }
        }

        protected void btnGuardarDoc_Click(object sender, EventArgs e)
        {
            int eucid = int.Parse(hfEUCID_Doc.Value);
            docSvc.CrearDocumentacion(
                eucid,
                txtNombreEUC.Text.Trim(),
                txtProposito.Text.Trim(),
                txtProceso.Text.Trim(),
                txtUso.Text.Trim(),
                txtInsumos.Text.Trim(),
                txtResponsableDoc.Text.Trim(),
                txtDocTecnica.Text.Trim(),
                txtEvControl.Text.Trim()
            );
        }

        protected void btnGuardarPlan_Click(object sender, EventArgs e)
        {
            int eucid = int.Parse(hfEUCID_Plan.Value);
            var euc = eucSvc.ObtenerPorId(eucid);
            if (euc != null && euc.Criticidad == "Alta")
            {
                var nuevoPlan = new PlanAutomatizacion(eucid, txtResponsablePlan.Text.Trim(), txtPlan.Text.Trim());
                planSvc.Crear(nuevoPlan, "Alta");
            }
        }

        protected void ddlCriticidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Aquí podrías mostrar u ocultar el panel de Plan según criticidad
        }
    }
}
