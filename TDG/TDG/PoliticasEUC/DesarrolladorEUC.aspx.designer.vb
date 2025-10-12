Option Strict On
Option Explicit On

Namespace TDG
    Partial Public Class DesarrolladorEUC

        ' ⬇️ Pega AQUÍ dentro TODAS las declaraciones que ya tienes ⬇️

        Protected WithEvents form1 As Global.System.Web.UI.HtmlControls.HtmlForm

        Protected WithEvents grdEUCs As Global.System.Web.UI.WebControls.GridView
        Protected WithEvents btnAgregarEUC As Global.System.Web.UI.WebControls.Button

        Protected WithEvents phMsgDoc As Global.System.Web.UI.WebControls.PlaceHolder
        Protected WithEvents vsDoc As Global.System.Web.UI.WebControls.ValidationSummary
        Protected WithEvents hfEUCID_Doc As Global.System.Web.UI.WebControls.HiddenField
        Protected WithEvents txtNombreEUC As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvNombreEUC As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtProposito As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvProposito As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtProceso As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvProceso As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtUso As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvUso As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtInsumos As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvInsumos As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtResponsableDoc As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvResponsableDoc As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtDocTecnica As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents txtEvControl As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents btnGuardarDoc As Global.System.Web.UI.WebControls.Button

        Protected WithEvents phMsgPlan As Global.System.Web.UI.WebControls.PlaceHolder
        Protected WithEvents vsPlan As Global.System.Web.UI.WebControls.ValidationSummary
        Protected WithEvents hfEUCID_Plan As Global.System.Web.UI.WebControls.HiddenField
        Protected WithEvents txtHerramientasPlan As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvHerr As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtFechaEstimadaPlan As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvFecha As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents cvFecha As Global.System.Web.UI.WebControls.CompareValidator
        Protected WithEvents rvFechaMin As Global.System.Web.UI.WebControls.RangeValidator
        Protected WithEvents txtResponsablePlan As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents rfvResp As Global.System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents txtPlan As Global.System.Web.UI.WebControls.TextBox
        Protected WithEvents btnGuardarPlan As Global.System.Web.UI.WebControls.Button

        ' ⬆️ Pega aquí dentro TODAS tus declaraciones ⬆️

    End Class
End Namespace