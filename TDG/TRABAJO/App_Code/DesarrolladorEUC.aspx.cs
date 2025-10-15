
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TRABAJO
{
    public partial class DesarrolladorEUC : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                if (rvFechaMin != null)
                {
                    rvFechaMin.MinimumValue = DateTime.Today.ToString("yyyy-MM-dd");
                    rvFechaMin.MaximumValue = DateTime.Today.AddYears(10).ToString("yyyy-MM-dd");
                }
            }
        }

        protected void grdEUCs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(e.CommandArgument))) return;

            if (!int.TryParse(Convert.ToString(e.CommandArgument), out int id)) return;

            switch (e.CommandName)
            {
                case "Ver":
                case "Doc":
                    hfEUCID_Doc.Value = id.ToString();
                    ShowModal("#modalDocumentacion");
                    break;
                case "Plan":
                    hfEUCID_Plan.Value = id.ToString();
                    ShowModal("#modalPlan");
                    break;
            }
        }

        protected void btnAgregarEUC_Click(object sender, EventArgs e)
        {
            hfEUCID_Doc.Value = "";
            ShowModal("#modalDocumentacion");
        }

        protected void btnGuardarDoc_Click(object sender, EventArgs e)
        {
            Page.Validate("DocForm");
            if (!Page.IsValid)
            {
                ShowDocAlert("Hay errores en el formulario. Por favor revísalos.", "danger");
                ShowModal("#modalDocumentacion");
                return;
            }

            ShowDocAlert("Documentación guardada correctamente (simulado).", "success");
            ShowModal("#modalDocumentacion");
        }

        private void ShowDocAlert(string message, string type)
        {
            phMsgDoc.Controls.Clear();
            string html = $"<div class='alert alert-{type} alert-dismissible fade show' role='alert'>" +
                          Server.HtmlEncode(message) +
                          "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>&times;</button>" +
                          "</div>";
            phMsgDoc.Controls.Add(new LiteralControl(html));
        }

        protected void btnGuardarPlan_Click(object sender, EventArgs e)
        {
            Page.Validate("PlanForm");
            if (!Page.IsValid)
            {
                ShowPlanAlert("Corrige los errores del formulario del Plan.", "danger");
                ShowModal("#modalPlan");
                return;
            }

            ShowPlanAlert("Plan de Automatización guardado (simulado).", "success");
            ShowModal("#modalPlan");
        }

        private void ShowPlanAlert(string message, string type)
        {
            phMsgPlan.Controls.Clear();
            string html = $"<div class='alert alert-{type} alert-dismissible fade show' role='alert'>" +
                          Server.HtmlEncode(message) +
                          "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>&times;</button>" +
                          "</div>";
            phMsgPlan.Controls.Add(new LiteralControl(html));
        }

        private void ShowModal(string modalSelector)
        {
            string script = $"$(function() {{ $('{modalSelector}').modal('show'); }});";
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }

}