<%@ Page Language="VB" AutoEventWireup="false"
    CodeBehind="DesarrolladorEUC.aspx.vb"
    Inherits="TDG.DesarrolladorEUC" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Panel Desarrollador - EUCs</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- Estilos de la plantilla -->
    ../../Recursos/Bootstrap_ColorAdmin/assetsNew/css/css.css
    ../../Recursos/Bootstrap_ColorAdmin/assetsNew/css/facebook/app.min.css
    <link href="../../Recursos/Bootstrap_ColorAdmin/assetsellow.min.css
    https://code.jquery.com/jquery-3.6.0.min.js</script>
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
                        <asp:GridView ID="grdEUCs" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="grdEUCs_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="EUCID" HeaderText="ID" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción corta" />
                                <asp:BoundField DataField="Criticidad" HeaderText="Criticidad" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:Button ID="btnVer" runat="server" Text="Ver" CssClass="btn btn-sm btn-info" CommandName="Ver" CommandArgument='<%# Eval("EUCID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnAgregarEUC" runat="server" Text="Agregar nueva EUC" CssClass="btn btn-primary mt-2" OnClick="btnAgregarEUC_Click" />
                    </div>
                </div>

                <!-- Modal Documentación -->
                <div class="modal fade" id="modalDocumentacion" tabindex="-1" role="dialog">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header bg-secondary text-white">
                                <h5 class="modal-title">Agregar Documentación</h5>
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                            </div>
                            <div class="modal-body">
                                <asp:HiddenField ID="hfEUCID_Doc" runat="server" />
                                <div class="form-group">
                                    <label>Nombre EUC</label>
                                    <asp:TextBox ID="txtNombreEUC" runat="server" CssClass="form-control" />
                               asp:TextBox ID="txtProposito" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Propósito" />
                                <asp:TextBox ID="txtProceso" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Proceso" />
                                <asp:TextBox ID="txtUso" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Uso" />
                                <asp:TextBox ID="txtInsumos" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Insumos" />
                                <asp:TextBox ID="txtResponsableDoc" runat="server" CssClass="form-control" placeholder="Responsable" />
                                <asp:TextBox ID="txtDocTecnica" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Documentación Técnica" />
                                <asp:TextBox ID="txtEvControl" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Evidencia de Control" />
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnGuardarDoc" runat="server" Text="Guardar Documentación" CssClass="btn btn-success" OnClick="btnGuardarDoc_Click" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal Plan de Automatización -->
                <div class="modal fade" id="modalPlan" tabindex="-1" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header bg-secondary text-white">
                                <h5 class="modal-title">Agregar Plan de Automatización</h5>
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                            </div>
                            <div class="modal-body">
                                <asp:HiddenField ID="hfEUCID_Plan" runat="server" />
                                <div class="form-group">
                                    <label>Responsable</label>
                                    <asp:TextBox ID="txtResponsablePlan" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-group">
                                    <label>Plan</label>
                                    <asp:TextBox ID="txtPlan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnGuardarPlan" runat="server" Text="Guardar Plan" CssClass="btn btn-success" OnClick="btnGuardarPlan_Click" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>