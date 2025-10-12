<%@ Page Language="VB" AutoEventWireup="false"
    CodeBehind="DesarrolladorEUC.aspx.vb"
    Inherits="TDG.DesarrolladorEUC" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Panel Desarrollador - EUCs</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- Estilos de la plantilla (OJO: estas rutas están rotas, las dejamos igual para no salirnos de alcance)
    ../../Recursos/Bootstrap_ColorAdmin/assetsNew/css/css.css
    ../../Recursos/Bootstrap_ColorAdmin/assetsNew/css/facebook/app.min.css
    ../../Recursos/Bootstrap_ColorAdmin/assetsellow.min.css
    <script src=</script>
    -->
</head>
<body>
    <form id="form1" runat="server">
        <div id="page-loader" class="fade show">
            <span class="spinner"></span>
        </div>

        <div id="page-container" class="page-container fade page-sidebar-fixed page-header-fixed">
            <!-- Sidebar izquierdo -->
            <div id="sidebar" class="sidebar">
                <div class="sidebar-content">
                    <ul class="nav">
                        <li class="nav-header">Menú</li>
                        <li>#<i class="fa fa-database"></i> <span>EUCs</span></a></li>
                        <li>#<i class="fa fa-file-text"></i> <span>Documentación</span></a></li>
                        <li>#<i class="fa fa-cogs"></i> <span>Planes</span></a></li>
                    </ul>
                </div>
            </div>

            <!-- Contenido principal -->
            <div id="content" class="content">
                <h1 class="page-header">Panel del Desarrollador</h1>

                <!-- Lista de EUCs -->
                <div class="card">
                    <div class="card-header bg-yellow text-white">
                        <h4 class="card-title">Mis EUCs</h4>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="grdEUCs" runat="server" CssClass="table table-bordered table-hover"
                            AutoGenerateColumns="False" OnRowCommand="grdEUCs_RowCommand" DataKeyNames="EUCID">
                            <Columns>
                                <asp:BoundField DataField="EUCID" HeaderText="ID" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción corta" />
                                <asp:BoundField DataField="Criticidad" HeaderText="Criticidad" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:Button ID="btnVer" runat="server" Text="Ver"
                                            CssClass="btn btn-sm btn-info"
                                            CommandName="Ver"
                                            CommandArgument='<%# Eval("EUCID") %>' />
                                        <%-- Ejemplo botones para abrir modales directos:
                                        <asp:Button ID="btnDoc" runat="server" Text="Documentación" CssClass="btn btn-sm btn-secondary ms-1"
                                            CommandName="Doc" CommandArgument='<%# Eval("EUCID") %>' />
                                        <asp:Button ID="btnPlan" runat="server" Text="Plan" CssClass="btn btn-sm btn-warning ms-1"
                                            CommandName="Plan" CommandArgument='<%# Eval("EUCID") %>' />
                                        --%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <asp:Button ID="btnAgregarEUC" runat="server" Text="Agregar nueva EUC"
                            CssClass="btn btn-primary mt-2" OnClick="btnAgregarEUC_Click" />
                    </div>
                </div>

                <!-- ===================== Modal Documentación ===================== -->
                <div class="modal fade" id="modalDocumentacion" tabindex="-1" role="dialog" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header bg-secondary text-white">
                                <h5 class="modal-title">Agregar Documentación</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Cerrar">&times;</button>
                            </div>

                            <div class="modal-body">
                                <!-- Mensajes -->
                                <asp:PlaceHolder ID="phMsgDoc" runat="server" />

                                <!-- Resumen de validación -->
                                <asp:ValidationSummary ID="vsDoc" runat="server"
                                    ValidationGroup="DocForm"
                                    HeaderText="Por favor corrige los siguientes campos:"
                                    DisplayMode="BulletList" ShowSummary="true"
                                    CssClass="text-danger mb-3" />

                                <asp:HiddenField ID="hfEUCID_Doc" runat="server" />

                                <div class="form-group">
                                    <label>Nombre EUC *</label>
                                    <asp:TextBox ID="txtNombreEUC" runat="server" CssClass="form-control" MaxLength="150" />
                                    <asp:RequiredFieldValidator ID="rfvNombreEUC" runat="server"
                                        ControlToValidate="txtNombreEUC"
                                        ErrorMessage="El nombre de la EUC es obligatorio."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="DocForm" />
                                </div>

                                <div class="form-group">
                                    <label>Propósito *</label>
                                    <asp:TextBox ID="txtProposito" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="2" MaxLength="400" placeholder="Propósito" />
                                    <asp:RequiredFieldValidator ID="rfvProposito" runat="server"
                                        ControlToValidate="txtProposito"
                                        ErrorMessage="El propósito es obligatorio."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="DocForm" />
                                </div>

                                <div class="form-group">
                                    <label>Proceso *</label>
                                    <asp:TextBox ID="txtProceso" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="2" MaxLength="400" placeholder="Proceso" />
                                    <asp:RequiredFieldValidator ID="rfvProceso" runat="server"
                                        ControlToValidate="txtProceso"
                                        ErrorMessage="El proceso es obligatorio."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="DocForm" />
                                </div>

                                <div class="form-group">
                                    <label>Uso *</label>
                                    <asp:TextBox ID="txtUso" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="2" MaxLength="400" placeholder="Uso" />
                                    <asp:RequiredFieldValidator ID="rfvUso" runat="server"
                                        ControlToValidate="txtUso"
                                        ErrorMessage="El uso es obligatorio."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="DocForm" />
                                </div>

                                <div class="form-group">
                                    <label>Insumos *</label>
                                    <asp:TextBox ID="txtInsumos" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="2" MaxLength="400" placeholder="Insumos" />
                                    <asp:RequiredFieldValidator ID="rfvInsumos" runat="server"
                                        ControlToValidate="txtInsumos"
                                        ErrorMessage="Los insumos son obligatorios."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="DocForm" />
                                </div>

                                <div class="form-group">
                                    <label>Responsable *</label>
                                    <asp:TextBox ID="txtResponsableDoc" runat="server" CssClass="form-control"
                                        MaxLength="120" placeholder="Responsable" />
                                    <asp:RequiredFieldValidator ID="rfvResponsableDoc" runat="server"
                                        ControlToValidate="txtResponsableDoc"
                                        ErrorMessage="El responsable es obligatorio."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="DocForm" />
                                </div>

                                <div class="form-group">
                                    <label>Documentación Técnica (opcional)</label>
                                    <asp:TextBox ID="txtDocTecnica" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="2" MaxLength="800" placeholder="Documentación Técnica" />
                                </div>

                                <div class="form-group">
                                    <label>Evidencia de Control (opcional)</label>
                                    <asp:TextBox ID="txtEvControl" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="2" MaxLength="800" placeholder="Evidencia de Control" />
                                </div>

                            </div>

                            <div class="modal-footer">
                                <asp:Button ID="btnGuardarDoc" runat="server" Text="Guardar Documentación"
                                    CssClass="btn btn-success" OnClick="btnGuardarDoc_Click"
                                    ValidationGroup="DocForm" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===================== Modal Plan de Automatización ===================== -->
                <div class="modal fade" id="modalPlan" tabindex="-1" role="dialog" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header bg-secondary text-white">
                                <h5 class="modal-title">Agregar Plan de Automatización</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Cerrar">&times;</button>
                            </div>

                            <div class="modal-body">
                                <!-- Mensajes -->
                                <asp:PlaceHolder ID="phMsgPlan" runat="server" />

                                <!-- Resumen de validación -->
                                <asp:ValidationSummary ID="vsPlan" runat="server"
                                    ValidationGroup="PlanForm"
                                    HeaderText="Corrige los siguientes campos del plan:"
                                    DisplayMode="BulletList" ShowSummary="true"
                                    CssClass="text-danger mb-3" />

                                <asp:HiddenField ID="hfEUCID_Plan" runat="server" />

                                <div class="form-group">
                                    <label>Herramientas *</label>
                                    <asp:TextBox ID="txtHerramientasPlan" runat="server" CssClass="form-control"
                                        MaxLength="200" placeholder="Ej.: Power BI, Python, UiPath" />
                                    <asp:RequiredFieldValidator ID="rfvHerr" runat="server"
                                        ControlToValidate="txtHerramientasPlan"
                                        ErrorMessage="Indica las herramientas."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="PlanForm" />
                                </div>

                                <div class="form-group">
                                    <label>Fecha estimada *</label>
                                    <asp:TextBox ID="txtFechaEstimadaPlan" runat="server"
                                        CssClass="form-control" TextMode="Date" />
                                    <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                        ControlToValidate="txtFechaEstimadaPlan"
                                        ErrorMessage="La fecha estimada es obligatoria."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="PlanForm" />
                                    <asp:CompareValidator ID="cvFecha" runat="server"
                                        ControlToValidate="txtFechaEstimadaPlan"
                                        Operator="DataTypeCheck" Type="Date"
                                        ErrorMessage="La fecha no es válida."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="PlanForm" />
                                    <asp:RangeValidator ID="rvFechaMin" runat="server"
                                        ControlToValidate="txtFechaEstimadaPlan"
                                        Type="Date"
                                        ErrorMessage="La fecha debe ser hoy o futura."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="PlanForm" />
                                </div>

                                <div class="form-group">
                                    <label>Responsable *</label>
                                    <asp:TextBox ID="txtResponsablePlan" runat="server" CssClass="form-control"
                                        MaxLength="120" />
                                    <asp:RequiredFieldValidator ID="rfvResp" runat="server"
                                        ControlToValidate="txtResponsablePlan"
                                        ErrorMessage="El responsable es obligatorio."
                                        Display="Dynamic" CssClass="text-danger"
                                        ValidationGroup="PlanForm" />
                                </div>

                                <div class="form-group">
                                    <label>Plan / Notas (opcional)</label>
                                    <asp:TextBox ID="txtPlan" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="3" MaxLength="800" />
                                </div>
                            </div>

                            <div class="modal-footer">
                                <asp:Button ID="btnGuardarPlan" runat="server" Text="Guardar Plan"
                                    CssClass="btn btn-success" OnClick="btnGuardarPlan_Click"
                                    ValidationGroup="PlanForm" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- ===================== /Modales ===================== -->

            </div>
        </div>
    </form>
</body>
