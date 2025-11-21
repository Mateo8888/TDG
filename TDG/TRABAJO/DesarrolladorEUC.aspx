<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesarrolladorEUC.aspx.cs" Inherits="TRABAJO.DesarrolladorEUC" %>


<asp:Content ID="cHead" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Paleta y layout */
        .header-euc { background: #ffc107; color: #212529; border-radius: .5rem; }
        body, .page-bg { background: #f5f6f8; }
        .card-soft { background: #fff; border: 1px solid #e9ecef; border-radius: .5rem; }
        .form-section { background: #fff; border: 1px solid #e9ecef; border-radius: .5rem; }
        .muted { color:#6c757d; }
        .badge-crit-alta { background:#dc3545; }    /* rojo */
        .badge-crit-media { background:#fd7e14; }   /* naranja */
        .badge-crit-baja { background:#198754; }    /* verde */
        .badge-estado-activa { background:#198754; }           /* verde */
        .badge-estado-enconstruccion { background:#0d6efd; }   /* azul */
        .badge-estado-jubilada { background:#6c757d; }         /* gris */
        .required::after { content:" *"; color:#dc3545; }
        .card-actions .btn { margin-right:.35rem; margin-bottom:.35rem; }
        .modal-header-warning { background:#ffc107; color:#212529; }
        .text-wrap { white-space:pre-wrap; }
    </style>
</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Encabezado amarillo ... (lo de arriba se queda igual) -->

    <button id="btnAgregarEuc" type="button" class="btn btn-dark"
        data-toggle="modal" data-target="#mdlEucNuevo">
    + Agregar EUC
</button>
   <div class="modal fade" id="mdlEucNuevo" tabindex="-1" aria-labelledby="mdlEucNuevoLabel" aria-hidden="true">
  <div class="modal-dialog modal-lg modal-dialog-scrollable">
    <div class="modal-content">
      <!-- Igual que el modal de Plan -->
      <div class="modal-header modal-header-warning">
        <h5 class="modal-title" id="mdlEucNuevoLabel">Agregar EUC</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
      </div>

      <div class="modal-body">
        <asp:HiddenField ID="hfEUCID" runat="server" />
        <div class="row g-3">
          <div class="col-md-12">
            <label class="form-label required">Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre EUC" />
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
              ControlToValidate="txtNombre"
              ErrorMessage="El nombre es obligatorio."
              CssClass="text-danger d-block small" Display="Dynamic"
              ValidationGroup="EUC" />
          </div>

          <div class="col-md-12">
            <label class="form-label required">Descripción</label>
            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Descripción EUC" />
            <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
              ControlToValidate="txtDescripcion"
              ErrorMessage="La descripción es obligatoria."
              CssClass="text-danger d-block small" Display="Dynamic"
              ValidationGroup="EUC" />
          </div>

          <div class="col-md-6">
            <label class="form-label required">Criticidad</label>
            <asp:DropDownList ID="ddlCriticidad" runat="server" CssClass="form-select" placeholder="Selecciona criticidad">
              <asp:ListItem Text="" Value="" />
              <asp:ListItem Text="ALTA" Value="ALTA" />
              <asp:ListItem Text="MEDIA" Value="MEDIA" />
              <asp:ListItem Text="BAJA" Value="BAJA" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCriticidad" runat="server"
              ControlToValidate="ddlCriticidad" InitialValue=""
              ErrorMessage="Selecciona una criticidad."
              CssClass="text-danger d-block small" Display="Dynamic"
              ValidationGroup="EUC" />
          </div>

          <div class="col-md-6">
            <label class="form-label required">Estado</label>
            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" placeholder="Selecciona estado">
              <asp:ListItem Text="" Value="" />
              <asp:ListItem Text="Activa" Value="Activa" />
              <asp:ListItem Text="En construcción" Value="En construcción" />
              <asp:ListItem Text="Jubilada" Value="Jubilada" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvEstado" runat="server"
              ControlToValidate="ddlEstado" InitialValue=""
              ErrorMessage="Selecciona un estado."
              CssClass="text-danger d-block small" Display="Dynamic"
              ValidationGroup="EUC" />
          </div>
            <div class="col-md-6">
                    <label class="form-label">Usuarios activos</label>
                    <asp:TextBox ID="txtUsuariosActivos" runat="server"
                              CssClass="form-control"
                            placeholder="Cantidad de usuarios que usan la EUC"
                            TextMode="Number" />
  <asp:RegularExpressionValidator ID="revUsuariosActivos" runat="server"
    ControlToValidate="txtUsuariosActivos"
    ValidationExpression="^\d*$"
    ErrorMessage="Ingresa solo números enteros (o deja el campo vacío)."
    CssClass="text-danger d-block small" Display="Dynamic"
    ValidationGroup="EUC" />
          </div>
        </div>

        <asp:ValidationSummary ID="vsEUC" runat="server"
          ValidationGroup="EUC" CssClass="mt-3 p-2 border rounded small" />
      </div>

      <div class="modal-footer bg-light">
        <asp:Button ID="Button1" runat="server"
          CssClass="btn btn-primary" Text="Guardar"
          ValidationGroup="EUC" CausesValidation="true"
          OnClick="btnGuardarEUC_Click" />
      </div>
    </div>
  </div>
</div>

    <div class="row">
<asp:PlaceHolder ID="contenedorTarjetas" runat="server"></asp:PlaceHolder>
    </div>

    <asp:GridView ID="gvEUC" runat="server" AutoGenerateColumns="false"
              CssClass="table table-striped table-bordered">
    <Columns>
        <asp:BoundField DataField="EUCID" HeaderText="ID" />
        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
        <asp:BoundField DataField="Criticidad" HeaderText="Criticidad" />
        <asp:BoundField DataField="Estado" HeaderText="Estado" />
    </Columns>
</asp:GridView>

    <!-- Mensajes -->
    <div class="mb-3">
        <span id="lblMsg" class="text-success fw-semibold me-3"></span>
        <span id="lblError" class="text-danger fw-semibold"></span>
    </div>

    <!-- Contenedor de tarjetas -->
    <div class="page-bg p-2">
        <div id="cardsContainer" class="row g-3">
            <!-- Tarjetas EUC renderizadas por JS -->
        </div>
    </div>

    <!-- Literal opcional para inyectar JSON inicial desde servidor (si lo usas) -->
    <asp:Literal ID="litEucsJson" runat="server" Visible="false"></asp:Literal>

    <!-- ===================== MODAL PLAN (ÚNICO) ===================== -->
   <div class="modal fade" id="mdlPlan" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header modal-header-warning">
                <h5 class="modal-title">Plan de automatización</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfPlanEUCID" runat="server" />
                <input type="hidden" id="hdPlanId" />
                <input type="hidden" id="hdPlanEucId" />
                <div class="row g-3">
                    <div class="col-md-6">
                        <label class="form-label required">Responsable</label>
                        <asp:TextBox ID="txtPlanResponsable" runat="server" CssClass="form-control" MaxLength="100" />
                    </div>
                    <div class="col-md-6">
                        <label class="form-label required">Plan</label>
                        <asp:TextBox ID="txtPlan" runat="server" CssClass="form-control" MaxLength="255" />
                    </div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <asp:Button ID="btnGuardarPlan" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardarPlan_Click" />
                <button id="btnCancelarPlan" type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
</div>


    <!-- ===================== MODAL DOCUMENTACIÓN (ÚNICA) ===================== -->
    <div class="modal fade" id="mdlDoc" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header modal-header-warning">
                <h5 class="modal-title">Documentación de la EUC</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfDocEUCID" runat="server" />
                <input type="hidden" id="hdDocId" />
                <input type="hidden" id="hdDocEucId" />
                <div class="row g-3">
                    <!-- Requeridos -->
                    <div class="col-md-6">
                        <label class="form-label required">Nombre de la EUC</label>
                        <asp:TextBox ID="txtDocNombreEUC" runat="server" CssClass="form-control" MaxLength="100" />
                    </div>
                    <div class="col-md-6">
                        <label class="form-label required">Propósito</label>
                        <asp:TextBox ID="txtDocProposito" runat="server" CssClass="form-control" MaxLength="255" />
                    </div>
                    <div class="col-md-6">
                        <label class="form-label required">Proceso</label>
                        <asp:TextBox ID="txtDocProceso" runat="server" CssClass="form-control" MaxLength="255" />
                    </div>
                    <div class="col-md-6">
                        <label class="form-label required">Uso</label>
                        <asp:TextBox ID="txtDocUso" runat="server" CssClass="form-control" MaxLength="255" />
                    </div>
                    <!-- Opcionales -->
                    <div class="col-md-6">
                        <label class="form-label">Insumos</label>
                        <asp:TextBox ID="txtDocInsumos" runat="server" CssClass="form-control" MaxLength="300" />
                    </div>
                    <div class="col-md-6">
                        <label class="form-label">Responsable</label>
                        <asp:TextBox ID="txtDocResponsable" runat="server" CssClass="form-control" MaxLength="100" />
                    </div>
                    <div class="col-md-12">
                        <label class="form-label">Documentación técnica</label>
                        <asp:TextBox ID="txtDocTecnica" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500" />
                    </div>
                    <div class="col-md-12">
                        <label class="form-label">Evidencia de control</label>
                        <asp:TextBox ID="txtDocEvControl" runat="server" CssClass="form-control" MaxLength="255" />
                    </div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <asp:Button ID="btnGuardarDoc" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardarDoc_Click" />
                <button id="btnCancelarDoc" type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
