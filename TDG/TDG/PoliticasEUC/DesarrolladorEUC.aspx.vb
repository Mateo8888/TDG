
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace TDG
    Partial Public Class DesarrolladorEUC
        Inherits System.Web.UI.Page

        ' =====================================================================
        '  Page_Load: fuerza validación clásica y configura el rango de fechas
        ' =====================================================================
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' Habilita los validadores clásicos (si no usas unobtrusive scripts)
            Me.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None

            If Not IsPostBack Then
                ' Configurar rango mínimo/máximo para la fecha del plan (si el validador existe en el .aspx)
                If rvFechaMin IsNot Nothing Then
                    rvFechaMin.MinimumValue = DateTime.Today.ToString("yyyy-MM-dd")        ' hoy
                    rvFechaMin.MaximumValue = DateTime.Today.AddYears(10).ToString("yyyy-MM-dd") ' +10 años
                End If
            End If
        End Sub

        ' =====================================================================
        '  GRID: Acciones por fila (abre modales)
        '   - "Ver": por ahora lo usamos para abrir Documentación (ajústalo a gusto)
        '   - "Doc": abre modal de Documentación
        '   - "Plan": abre modal de Plan
        ' =====================================================================
        Protected Sub grdEUCs_RowCommand(sender As Object, e As GridViewCommandEventArgs)
            If String.IsNullOrWhiteSpace(Convert.ToString(e.CommandArgument)) Then Exit Sub

            Dim id As Integer
            If Not Integer.TryParse(Convert.ToString(e.CommandArgument), id) Then Exit Sub

            Select Case e.CommandName
                Case "Ver", "Doc"
                    hfEUCID_Doc.Value = id.ToString()
                    ShowModal("#modalDocumentacion")

                Case "Plan"
                    hfEUCID_Plan.Value = id.ToString()
                    ShowModal("#modalPlan")
            End Select
        End Sub

        ' =====================================================================
        '  Botón: Agregar nueva EUC -> abre Documentación
        ' =====================================================================
        Protected Sub btnAgregarEUC_Click(sender As Object, e As EventArgs)
            hfEUCID_Doc.Value = "" ' nueva
            ShowModal("#modalDocumentacion")
        End Sub

        ' =====================================================================
        '  DOCUMENTACIÓN: Guardar (solo validación + mensaje)
        ' =====================================================================
        Protected Sub btnGuardarDoc_Click(sender As Object, e As EventArgs)
            Page.Validate("DocForm")
            If Not Page.IsValid Then
                ShowDocAlert("Hay errores en el formulario. Por favor revísalos.", "danger")
                ShowModal("#modalDocumentacion")
                Exit Sub
            End If

            ' Aquí iría tu lógica real de guardado (no incluida hoy)
            ShowDocAlert("Documentación guardada correctamente (simulado).", "success")
            ShowModal("#modalDocumentacion")
        End Sub

        Private Sub ShowDocAlert(message As String, type As String)
            ' type: success | info | warning | danger
            phMsgDoc.Controls.Clear()
            Dim html = $"<div class='alert alert-{type} alert-dismissible fade show' role='alert'>" &
                       $"{Server.HtmlEncode(message)}" &
                       "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>&times;</button>" &
                       "</div>"
            phMsgDoc.Controls.Add(New LiteralControl(html))
        End Sub

        ' =====================================================================
        '  PLAN DE AUTOMATIZACIÓN: Guardar (solo validación + mensaje)
        ' =====================================================================
        Protected Sub btnGuardarPlan_Click(sender As Object, e As EventArgs)
            Page.Validate("PlanForm")
            If Not Page.IsValid Then
                ShowPlanAlert("Corrige los errores del formulario del Plan.", "danger")
                ShowModal("#modalPlan")
                Exit Sub
            End If

            ' Aquí iría tu lógica real de guardado (no incluida hoy)
            ShowPlanAlert("Plan de Automatización guardado (simulado).", "success")
            ShowModal("#modalPlan")
        End Sub

        Private Sub ShowPlanAlert(message As String, type As String)
            phMsgPlan.Controls.Clear()
            Dim html = $"<div class='alert alert-{type} alert-dismissible fade show' role='alert'>" &
                       $"{Server.HtmlEncode(message)}" &
                       "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>&times;</button>" &
                       "</div>"
            phMsgPlan.Controls.Add(New LiteralControl(html))
        End Sub

        ' =====================================================================
        '  UTIL: Reabrir modal de Bootstrap después del PostBack
        '  Requiere jQuery + Bootstrap JS cargados en la página.
        ' =====================================================================
        Private Sub ShowModal(modalSelector As String)
            Dim script As String = $"$(function() {{ $('{modalSelector}').modal('show'); }});"
            ' Usamos ClientScript para no depender de <asp:ScriptManager>
            ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), script, True)
        End Sub

    End Class
End Namespace
