<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesarrolladorEUC.aspx.cs" Inherits="TRABAJO.DesarrolladorEUC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestión de EUC - Desarrollador</title>
    assets/css/bootstrap.min.css
    assets/css/coloradmin.css
    assets/js/jquery.min.js</script>
    assets/js/bootstrap.bundle.min.js</script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2 class="mb-4">Crear EUC</h2>
            <asp:Button ID="btnAgregarEUC" runat="server" CssClass="btn btn-primary" Text="Agregar EUC" OnClick="btnAgregarEUC_Click" />

            <asp:GridView ID="grdEUCs" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered mt-4"
                OnRowCommand="grdEUCs_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre EUC" />

                    <asp:TemplateField HeaderText="Documentación">
                        <ItemTemplate>
                            <asp:Button ID="btnDoc" runat="server" Text="Documentación"
                                CommandName="Doc"
                                CommandArgument='<%# Eval("EUCID") %>' CssClass="btn btn-info btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Plan de Automatización">
                        <ItemTemplate>
                            <asp:Button ID="btnPlan" runat="server" Text="Plan de Automatización"
                                CommandName="Plan"
                                CommandArgument='<%# Eval("EUCID") %>' CssClass="btn btn-warning btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Campo de fecha y RangeValidator -->
        <div class="container mt-4">
            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
            <asp:RangeValidator ID="rvFechaMin" runat="server"
                ControlToValidate="txtFecha"
                Type="Date"
                MinimumValue=""
                MaximumValue=""
                ErrorMessage="La fecha debe estar entre hoy y 10 años adelante."
                CssClass="text-danger" />
        </div>

        <!-- Modal Documentación -->
        <div class="modal fade" id="modalDocumentacion" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Documentación de la EUC</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <div class="modal-body">
                            <asp:HiddenField ID="hfEUCID_Doc" runat="server" />
                            <asp:ValidationSummary ID="vsDoc" runat="server" ValidationGroup="DocForm" CssClass="text-danger" />
                            <asp:TextBox ID="txtDescripcionDoc" runat="server" CssClass="form-control" placeholder="Descripción" />
                            <asp:RequiredFieldValidator ID="rfvDescripcionDoc" runat="server" ControlToValidate="txtDescripcionDoc"
                                ErrorMessage="La descripción es obligatoria." ValidationGroup="DocForm" CssClass="text-danger" />
                            <asp:PlaceHolder ID="phMsgDoc" runat="server" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnGuardarDoc" runat="server" CssClass="btn btn-success" Text="Guardar"
                                OnClick="btnGuardarDoc_Click" ValidationGroup="DocForm" />
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Plan -->
        <div class="modal fade" id="modalPlan" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Plan de Automatización</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                   div class="modal-body">
                        <asp:HiddenField ID="hfEUCID_Plan" runat="server" />
                        <asp:ValidationSummary ID="vsPlan" runat="server" ValidationGroup="PlanForm" CssClass="text-danger" />
                        <asp:TextBox ID="txtDescripcionPlan" runat="server" CssClass="form-control" placeholder="Descripción del Plan" />
                        <asp:RequiredFieldValidator ID="rfvDescripcionPlan" runat="server" ControlToValidate="txtDescripcionPlan"
                            ErrorMessage="La descripción del plan es obligatoria." ValidationGroup="PlanForm" CssClass="text-danger" />
                        <asp:PlaceHolder ID="phMsgPlan" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGuardarPlan" runat="server" CssClass="btn btn-success" Text="Guardar"
                            OnClick="btnGuardarPlan_Click" ValidationGroup="PlanForm" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>