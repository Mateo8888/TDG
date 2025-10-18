 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminEUC.aspx.cs" Inherits="PoliticasEUC.TRABAJO.AdminEUC" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrador EUC</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5s/bootstrap.min.css
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.jsead>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2 class="mb-4">Panel de Administración de EUCs</h2>

            <asp:Repeater ID="rptEUCs" runat="server">
                <ItemTemplate>
                    <div class="card mb-3 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("Nombre") %></h5>
                            <p class="card-text">Estado: <%# Eval("Estado") %></p>
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalEUC<%# Eval("EUCID") %>">
                                Ver Detalle
                            </button>
                        </div>
                    </div>

                    <!-- Modal -->
                    <div class="modal fade" id="modalEUC<%# Eval("EUCID") %>" tabindex="-1" aria-labelledby="modalLabel<%# Eval("EUCID") %>" aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modalLabel<%# Eval("EUCID") %>"><%# Eval("Nombre") %></h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                </div>
                                <div class="modal-body">
                                    <p><strong>Descripción:</strong> <%# Eval("Descripcion") %></p>
                                    <p><strong>Criticidad:</strong> <%# Eval("Criticidad") %></p>
                                    <p><strong>Estado:</strong> <%# Eval("Estado") %></p>

                                    <!-- Documentación y Plan de Automatización -->
                                    <div class="mb-3">
                                        <label class="form-label">Documentación:</label>
                                        <asp:Literal ID="litDocumentacion" runat="server" Text='<%# Eval("Documentacion") %>' />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Plan de Automatización:</label>
                                        <asp:Literal ID="litPlan" runat="server" Text='<%# Eval("PlanAutomatizacion") %>' />
                                    </div>

                                    <!-- Observación y botones -->
                                    <div class="mb-3">
                                        <label for="txtObservacion<%# Eval("EUCID") %>" class="form-label">Observación:</label>
                                        <textarea class="form-control" id="txtObservacion<%# Eval("EUCID") %>" rows="3"></textarea>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnAprobar" runat="server" CssClass="btn btn-success" Text="Aprobar" CommandArgument='<%# Eval("EUCID") %>' OnClick="btnAprobar_Click" />
                                    <asp:Button ID="btnRechazar" runat="server" CssClass="btn btn-danger" Text="Rechazar" CommandArgument='<%# Eval("EUCID") %>' OnClick="btnRechazar_Click" />
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
``