<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminEUC.aspx.cs" Inherits="TRABAJO.AdminEUC" %>

<asp:Content ID="cHead" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .header-admin { background:#ffc107; color:#212529; border-radius:.5rem; }
        .card-soft { background:#fff; border:1px solid #e9ecef; border-radius:.5rem; padding:1rem; }
        .badge-crit-alta { background:#dc3545; }
        .badge-crit-media { background:#fd7e14; }
        .badge-crit-baja { background:#198754; }
        .badge-estado-activa { background:#198754; }
        .badge-estado-enconstruccion { background:#0d6efd; }
        .badge-estado-jubilada { background:#6c757d; }
    </style>
</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Encabezado -->
    <div class="header-admin p-4 mb-4">
        <h3 class="mb-1">Administración de EUC</h3>
        <div class="muted">En esta seccion puedes revisa la información, Plan y Documentación de cada EUC; y certificar cada EUC como <b>Aprobada</b> o <b>Rechazada</b> con su respectiva información.</div>
    </div>

    <!-- Mensajes -->
    <div class="mb-3">
        <asp:Label ID="lblMsg" runat="server" CssClass="text-success fw-semibold me-3"></asp:Label>
        <asp:Label ID="lblError" runat="server" CssClass="text-danger fw-semibold"></asp:Label>
    </div>

    <!-- Tarjetas dinámicas -->
    <asp:Repeater ID="rptEUCs" runat="server" OnItemCommand="rptEUCs_ItemCommand">
    <ItemTemplate>
        <div class="card-soft mb-3 <%# GetCertificacionClass(Eval("EstadoCert").ToString()) %>">
            <h5><%# Eval("Nombre") %></h5>
            <p><%# Eval("Descripcion") %></p>
            <span class="badge <%# GetCriticidadClass(Eval("Criticidad").ToString()) %>"><%# Eval("Criticidad") %></span>
            <span class="badge <%# GetEstadoClass(Eval("Estado").ToString()) %>"><%# Eval("Estado") %></span>
            <hr />
            <asp:Button ID="btnPlan" runat="server" CssClass="btn btn-info btn-sm" Text="Plan"
                CommandName="VerPlan" CommandArgument='<%# Eval("EUCID") %>'
                Enabled='<%# Eval("EstadoCert").ToString() == "Pendiente" %>' />
            <asp:Button ID="btnDoc" runat="server" CssClass="btn btn-success btn-sm" Text="Documentación"
                CommandName="VerDoc" CommandArgument='<%# Eval("EUCID") %>'
                Enabled='<%# Eval("EstadoCert").ToString() == "Pendiente" %>' />
            <hr />
            <asp:TextBox ID="txtComentario" runat="server" CssClass="form-control mb-2" Placeholder="Comentario"
                Enabled='<%# Eval("EstadoCert").ToString() == "Pendiente" %>'></asp:TextBox>
            <asp:Button ID="btnAprobar" runat="server" CssClass="btn btn-outline-success btn-sm" Text="✔ Aprobar"
                CommandName="Aprobar" CommandArgument='<%# Eval("EUCID") %>'
                Enabled='<%# Eval("EstadoCert").ToString() == "Pendiente" %>' />
            <asp:Button ID="btnRechazar" runat="server" CssClass="btn btn-outline-danger btn-sm" Text="✖ Rechazar"
                CommandName="Rechazar" CommandArgument='<%# Eval("EUCID") %>'
                Enabled='<%# Eval("EstadoCert").ToString() == "Pendiente" %>' />
            <hr />
            <small class="text-muted">Certificación: <%# Eval("EstadoCert") %> 
                <%# Eval("FechaControl") != DBNull.Value ? " | Fecha: " + Convert.ToDateTime(Eval("FechaControl")).ToString("dd/MM/yyyy") : "" %>
            </small>
        </div>
    </ItemTemplate>
</asp:Repeater>

    <!-- Modal Plan -->
    <div class="modal fade" id="mdlPlan" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header modal-header-warning">
                    <h5 class="modal-title">Plan de automatización</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtPlanResponsable" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                    <asp:TextBox ID="txtPlan" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Documentación -->
    <div class="modal fade" id="mdlDoc" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header modal-header-warning">
                    <h5 class="modal-title">Documentación de la EUC</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtDocNombreEUC" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                    <asp:TextBox ID="txtDocProposito" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                    <asp:TextBox ID="txtDocProceso" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                    <asp:TextBox ID="txtDocUso" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                    <asp:TextBox ID="txtDocInsumos" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                    <asp:TextBox ID="txtDocResponsable" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                    <asp:TextBox ID="txtDocTecnica" runat="server" CssClass="form-control mb-2" TextMode="MultiLine" ReadOnly="true" />
                    <asp:TextBox ID="txtDocEvControl" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>