using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio.PoliticasEUC;

namespace PoliticasEUC.TRABAJO
{
    public partial class AdminEUC : Page
    {
        private EUC.EUCService _eucService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InicializarServicios();
                CargarEUCs();
            }
        }

        private void InicializarServicios()
        {
            var certSvc = new Certificacion.CertificacionService();
            var docSvc = new Documentacion.DocumentacionService();
            var planSvc = new PlanAutomatizacion.PlanAutomatizacionService();

            _eucService = new EUC.EUCService(certSvc, docSvc, planSvc);
        }

        private void CargarEUCs()
        {
            var listaEUCs = _eucService.ObtenerTodas();
            rptEUCs.DataSource = listaEUCs;
            rptEUCs.DataBind();
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int eucId = Convert.ToInt32(btn.CommandArgument);
            string observacion = Request.Form["txtObservacion" + eucId];

            var certSvc = new Certificacion.CertificacionService();
            certSvc.CertificarEUC(eucId, true, observacion);

            InicializarServicios();
            CargarEUCs();
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int eucId = Convert.ToInt32(btn.CommandArgument);
            string observacion = Request.Form["txtObservacion" + eucId];

            var certSvc = new Certificacion.CertificacionService();
            certSvc.CertificarEUC(eucId, false, observacion);

            InicializarServicios();
            CargarEUCs();
        }
    }
}