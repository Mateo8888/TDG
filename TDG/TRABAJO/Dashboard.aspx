<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TRABAJO.Dashboard" %>


<asp:Content ID="cHead" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Paleta y layout */
        .header-dash { background:#ffc107; color:#212529; border-radius:.5rem; }
        body, .page-bg { background:#f5f6f8; }
        .card-soft { background:#fff; border:1px solid #e9ecef; border-radius:.5rem; }
        .muted { color:#6c757d; }
        .kpi { border-radius:.5rem; padding:1rem 1.25rem; color:#fff; }
        .kpi-total { background:#0d6efd; }
        .kpi-plan { background:#198754; }
        .kpi-doc { background:#6f42c1; }
        .kpi-aprob { background:#198754; }
        .kpi-rech { background:#dc3545; }
        .kpi-pend { background:#ffc107; color:#111; }

        /* Badges */
        .badge-crit-alta { background:#dc3545; }
        .badge-crit-media { background:#fd7e14; }
        .badge-crit-baja { background:#198754; }
        .badge-estado-activa { background:#198754; }
        .badge-estado-enconstruccion { background:#0d6efd; }
        .badge-estado-jubilada { background:#6c757d; }
        .chip { display:inline-block; padding:.25rem .5rem; border-radius:1rem; font-size:.75rem; margin-right:.35rem; }
        .chip-ok { background:#e6f4ea; color:#0f5132; border:1px solid #badbcc; }
        .chip-bad{ background:#fde8e8; color:#842029; border:1px solid #f5c2c7; }
        .chip-warn{ background:#fff3cd; color:#664d03; border:1px solid #ffecb5; }

        /* Estado general (color mapeado) */
        .state-verde { border-left:4px solid #198754; }
        .state-azul  { border-left:4px solid #0d6efd; }
        .state-rojo  { border-left:4px solid #dc3545; }

        .cursor-pointer { cursor:pointer; }
        .text-wrap { white-space:pre-wrap; }
    </style>
</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Encabezado amarillo -->
    <div class="header-dash p-4 mb-4">
        <div class="d-flex align-items-center flex-wrap gap-3">
            <div>
                <h3 class="mb-1">Dashboard EUC</h3>
                <div class="muted">Resumen global: Plan, Documentación y Certificación por cada EUC.</div>
            </div>
            <div class="ms-auto d-flex gap-2">
                <asp:Button ID="btnRefrescar" runat="server" CssClass="btn btn-dark" Text="Refrescar" />
            </div>
        </div>
    </div>

    <!-- Mensajes -->
    <div class="mb-3">
        <span id="lblMsg" runat="server" class="text-success fw-semibold me-3"></span>
        <span id="lblError" runat="server" class="text-danger fw-semibold"></span>
    </div>

    <!-- KPIs -->
    <div class="row g-3 mb-3">
        <div class="col-6 col-md-2">
            <div class="kpi kpi-total">
                <div class="muted text-white-50">Total EUC</div>
                <div class="h4 m-0"><span id="kpiTotal" runat="server">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-plan">
                <div class="muted text-white-50">Con Plan</div>
                <div class="h4 m-0"><span id="kpiPlan" runat="server">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-doc">
                <div class="muted text-white-50">Con Doc</div>
                <div class="h4 m-0"><span id="kpiDoc" runat="server">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-aprob">
                <div class="muted text-white-50">Aprobadas</div>
                <div class="h4 m-0"><span id="kpiAprob" runat="server">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-rech">
                <div class="muted text-white-50">Rechazadas</div>
                <div class="h4 m-0"><span id="kpiRech" runat="server">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-pend">
                <div class="muted text-dark-50">Pendientes</div>
                <div class="h4 m-0"><span id="kpiPend" runat="server">0</span></div>
            </div>
        </div>
    </div>

    <!-- Filtros -->
    <div class="card-soft p-3 mb-3">
        <div class="row g-3 align-items-end">
            <div class="col-md-3">
                <label class="form-label">Buscar</label>
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Placeholder="Nombre..." />
            </div>
            <div class="col-md-3">
                <label class="form-label">Criticidad</label>
                <asp:DropDownList ID="ddlCriticidad" runat="server" CssClass="form-select">
                    <asp:ListItem Value="">Todas</asp:ListItem>
                    <asp:ListItem Value="ALTA">ALTA</asp:ListItem>
                    <asp:ListItem Value="MEDIA">MEDIA</asp:ListItem>
                    <asp:ListItem Value="BAJA">BAJA</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label class="form-label">Estado (EUC)</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem Value="">Todos</asp:ListItem>
                    <asp:ListItem Value="Activa">Activa</asp:ListItem>
                    <asp:ListItem Value="En construcción">En construcción</asp:ListItem>
                    <asp:ListItem Value="Jubilada">Jubilada</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label class="form-label">Certificación</label>
                <asp:DropDownList ID="ddlCert" runat="server" CssClass="form-select">
                    <asp:ListItem Value="">Todas</asp:ListItem>
                    <asp:ListItem Value="Aprobada">Aprobada</asp:ListItem>
                    <asp:ListItem Value="Rechazada">Rechazada</asp:ListItem>
                    <asp:ListItem Value="Pendiente">Pendiente</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-12 d-flex gap-2">
                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="Filtrar" />
                <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-outline-secondary" Text="Limpiar" />
            </div>
        </div>
    </div>

    <!-- GridView -->
    <asp:GridView ID="gvDashboard" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
        OnRowCommand="gvDashboard_RowCommand">
        <Columns>
            <asp:BoundField DataField="Nombre" HeaderText="EUC" />
            <asp:TemplateField HeaderText="Documentación">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDoc" runat="server" CommandName="VerDoc" CommandArgument='<%# Eval("EUCID") %>'
                        CssClass='<%# GetColorClass(Eval("TieneDoc"), "doc") %>'>
                        <%# Convert.ToBoolean(Eval("TieneDoc")) ? "Ver" : "N/D" %>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Plan">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPlan" runat="server" CommandName="VerPlan" CommandArgument='<%# Eval("EUCID") %>'
                        CssClass='<%# GetColorClass(Eval("TienePlan"), "plan") %>'>
                        <%# Convert.ToBoolean(Eval("TienePlan")) ? "Ver" : "N/D" %>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Certificación">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCert" runat="server" CommandName="VerCert" CommandArgument='<%# Eval("EUCID") %>'
                        CssClass='<%# GetColorClass(Eval("Certificacion"), "cert") %>'>
                        <%# Eval("Certificacion") %>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado">
                <ItemTemplate>
                    <span class='<%# GetColorClass(Eval("Estado"), "estado") %>'><%# Eval("Estado") %></span>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <!-- Modal -->
    <div class="modal fade" id="mdlDetalle" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header" style="background:#ffc107;color:#212529;">
                    <h5 class="modal-title">Detalle de EUC</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3"><b>Nombre:</b> <span id="detNombre" runat="server"></span></div>
                    <div class="mb-3"><b>Criticidad:</b> <span id="detCrit" runat="server"></span> &nbsp; | &nbsp; <b>Estado:</b> <span id="detEstado" runat="server"></span></div>
                    <div class="mb-3">
                        <h6>Plan de automatización</h6>
                        <div id="detPlan" runat="server" class="text-wrap muted">N/D</div>
                    </div>
                    <div class="mb-3">
                        <h6>Documentación</h6>
                        <div id="detDoc" runat="server" class="text-wrap muted">N/D</div>
                    </div>
                    <div class="mb-3">
                        <h6>Certificación</h6>
                        <div id="detCert" runat="server" class="text-wrap muted">Pendiente</div>
                    </div>
                </div>
                <div class="modal-footer bg-light">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

