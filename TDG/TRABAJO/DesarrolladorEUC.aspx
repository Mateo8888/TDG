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
    <!-- Modal nuevo con validaciones -->
    <div class="modal fade" id="mdlEucNuevo" tabindex="-1" aria-labelledby="mdlEucNuevoLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="mdlEucNuevoLabel">Agregar EUC</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <div class="modal-body">
                    <!-- Nombre -->
    <asp:TextBox ID="txtNombre" runat="server"
             CssClass="form-control mb-1"
             placeholder="Nombre EUC"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
    ControlToValidate="txtNombre"
    ErrorMessage="El nombre es obligatorio."
    CssClass="text-danger" Display="Dynamic"
    ValidationGroup="EUC" />
                    

                    <!-- Descripción -->
                    <asp:TextBox ID="txtDescripcion" runat="server"
                                 CssClass="form-control mb-1"
                                 placeholder="Descripción EUC"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
                        ControlToValidate="txtDescripcion"
                        ErrorMessage="La descripción es obligatoria."
                        CssClass="text-danger" Display="Dynamic"
                        ValidationGroup="EUC" />

                    <!-- Criticidad -->
                    <asp:DropDownList ID="ddlCriticidad" runat="server" CssClass="form-select mb-1">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="ALTA" Value="ALTA" />
                        <asp:ListItem Text="MEDIA" Value="MEDIA" />
                        <asp:ListItem Text="BAJA" Value="BAJA" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCriticidad" runat="server"
                        ControlToValidate="ddlCriticidad" InitialValue=""
                        ErrorMessage="Selecciona una criticidad."
                        CssClass="text-danger" Display="Dynamic"
                        ValidationGroup="EUC" />

                    <!-- Estado -->
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select mb-1">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="Activa" Value="Activa" />
                        <asp:ListItem Text="En construcción" Value="En construcción" />
                        <asp:ListItem Text="Jubilada" Value="Jubilada" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server"
                        ControlToValidate="ddlEstado" InitialValue=""
                        ErrorMessage="Selecciona un estado."
                        CssClass="text-danger" Display="Dynamic"
                        ValidationGroup="EUC" />

                    <asp:ValidationSummary ID="vsEUC" runat="server"
                        ValidationGroup="EUC" CssClass="text-danger mt-2" />
                </div>

                <div class="modal-footer">
                    <asp:Button ID="Button1" runat="server"
                        CssClass="btn btn-primary" Text="Guardar"
                        ValidationGroup="EUC" CausesValidation="true"
                        OnClick="btnGuardarEUC_Click" />
                </div>
            </div>
        </div>
    </div>
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

    <!-- ===================== MODAL EUC ===================== 
    <div class="modal fade" id="mdlEuc" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header modal-header-warning">
                    <h5 class="modal-title">EUC</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="hdEucId" />
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label required">Nombre</label>
                            <input id="txtNombre" class="form-control" maxlength="100" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label required">Descripcion</label>
                            <input id="txtDescripcion" class="form-control" maxlength="255" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label required">Criticidad</label>
                            <select id="ddlCriticidad" class="form-select">
                                <option value="">-- Seleccione --</option>
                                <option value="ALTA">ALTA</option>
                                <option value="MEDIA">MEDIA</option>
                                <option value="BAJA">BAJA</option>
                            </select>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label required">Estado</label>
                            <select id="ddlEstado" class="form-select">
                                <option value="">-- Seleccione --</option>
                                <option value="Activa">Activa</option>
                                <option value="En construcción">En construcción</option>
                                <option value="Jubilada">Jubilada</option>
                            </select>
                            <div class="form-text">Se persiste si el WebMethod <code>CreateEUC</code>/<code>UpdateEUC</code> acepta <b>estado</b>. Si no, el UI lo mostrará pero no quedará guardado.</div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer bg-light">
                    <button id="btnGuardarEuc" type="button" class="btn btn-primary">Guardar</button>
                    <button id="btnCancelarEuc" type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div> -->

    <!-- ===================== MODAL PLAN (ÚNICO) ===================== -->
    <div class="modal fade" id="mdlPlan" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header modal-header-warning">
                    <h5 class="modal-title">Plan de automatización</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="hdPlanId" />
                    <input type="hidden" id="hdPlanEucId" />
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label required">Responsable</label>
                            <input id="txtPlanResponsable" class="form-control" maxlength="100" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label required">Plan</label>
                            <input id="txtPlanTexto" class="form-control" maxlength="255" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer bg-light">
                    <button id="btnGuardarPlan" type="button" class="btn btn-primary">Guardar</button>
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
                    <input type="hidden" id="hdDocId" />
                    <input type="hidden" id="hdDocEucId" />
                    <div class="row g-3">
                        <!-- Requeridos -->
                        <div class="col-md-6">
                            <label class="form-label required">Nombre de la EUC</label>
                            <input id="txtDocNombreEUC" class="form-control" maxlength="100" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label required">Propósito</label>
                            <input id="txtDocProposito" class="form-control" maxlength="255" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label required">Proceso</label>
                            <input id="txtDocProceso" class="form-control" maxlength="255" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label required">Uso</label>
                            <input id="txtDocUso" class="form-control" maxlength="255" />
                        </div>
                        <!-- Opcionales -->
                        <div class="col-md-6">
                            <label class="form-label">Insumos</label>
                            <input id="txtDocInsumos" class="form-control" maxlength="300" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label">Responsable</label>
                            <input id="txtDocResponsable" class="form-control" maxlength="100" />
                        </div>
                        <div class="col-md-12">
                            <label class="form-label">Documentación técnica</label>
                            <textarea id="txtDocTecnica" class="form-control" rows="3" maxlength="500"></textarea>
                        </div>
                        <div class="col-md-12">
                            <label class="form-label">Evidencia de control</label>
                            <input id="txtDocEvControl" class="form-control" maxlength="255" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer bg-light">
                    <button id="btnGuardarDoc" type="button" class="btn btn-primary">Guardar</button>
                    <button id="btnCancelarDoc" type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="cScript" ContentPlaceHolderID="ScriptContent" runat="server">
<script>
    // ===== Utilidades UI =====
    const $msg = document.getElementById('lblMsg');
    const $err = document.getElementById('lblError');
    const $cards = document.getElementById('cardsContainer');
    const toBadgeCrit = (c) => {
        const v = (c || '').toUpperCase();
        if (v === 'ALTA') return 'badge badge-crit-alta';
        if (v === 'MEDIA') return 'badge badge-crit-media';
        if (v === 'BAJA') return 'badge badge-crit-baja';
        return 'badge bg-secondary';
    };
    const toBadgeEstado = (e) => {
        const v = (e || '').toLowerCase();
        if (v === 'activa') return 'badge badge-estado-activa';
        if (v.startsWith('en constru')) return 'badge badge-estado-enconstruccion';
        if (v === 'jubilada') return 'badge badge-estado-jubilada';
        return 'badge bg-secondary';
    };
    const clearMsg = () => { $msg.textContent = ''; $err.textContent = ''; };
    const showOk = (t) => { $err.textContent = ''; $msg.textContent = t; };
    const showErr = (e) => { $msg.textContent = ''; $err.textContent = (e && e.message) ? e.message : (e || 'Ocurrió un error'); };

    function cardTemplate(euc) {
        const crit = euc.Criticidad || '';
        const estado = euc.Estado || '';
        const critBadge = toBadgeCrit(crit);
        const estBadge = toBadgeEstado(estado);
        const showPlanBtn = (crit.toUpperCase() === 'ALTA' && !euc.Plan); // único
        const hasPlan = !!euc.Plan;
        const hasDoc = !!euc.Doc;

        return `
    <div class="col-12 col-md-6 col-lg-4" id="card-euc-${euc.EUCID}">
      <div class="card-soft p-3 h-100 d-flex flex-column">
        <div class="d-flex align-items-start gap-2">
            <div class="flex-grow-1">
                <h5 class="mb-1">${escapeHtml(euc.Nombre || '')}</h5>
                <div class="muted small mb-2">${escapeHtml(euc.Descripcion || '')}</div>
                <div class="d-flex flex-wrap gap-2">
                    <span class="${critBadge}">${crit || '-'}</span>
                    <span class="${estBadge}">${estado || '-'}</span>
                </div>
            </div>
        </div>

        <div class="mt-3 card-actions">
            <button class="btn btn-sm btn-outline-primary" onclick="openEditEuc(${euc.EUCID})">Editar</button>
            <button class="btn btn-sm btn-outline-danger" onclick="confirmDeleteEuc(${euc.EUCID})">Eliminar</button>
            ${showPlanBtn ? `<button class="btn btn-sm btn-success" onclick="openAddPlan(${euc.EUCID})">Agregar plan</button>` : ''}
            ${!hasDoc ? `<button class="btn btn-sm btn-warning" onclick="openAddDoc(${euc.EUCID})">Agregar documentación</button>` : ''}
        </div>

        <!-- Plan -->
        <div class="mt-3">
            <h6 class="mb-1">Plan de automatización</h6>
            ${hasPlan ? `
                <div class="border rounded p-2">
                    <div><b>Responsable:</b> ${escapeHtml(euc.Plan.Responsable || '')}</div>
                    <div class="text-wrap"><b>Plan:</b> ${escapeHtml(euc.Plan.Plan || '')}</div>
                    <div class="mt-2">
                        <button class="btn btn-xs btn-outline-secondary" onclick="openEditPlan(${euc.EUCID}, ${euc.Plan.IdPlan})">Editar</button>
                        <button class="btn btn-xs btn-outline-danger" onclick="confirmDeletePlan(${euc.EUCID}, ${euc.Plan.IdPlan})">Eliminar</button>
                    </div>
                </div>
            ` : `<div class="muted small">No registrado.</div>`}
        </div>

        <!-- Documentación -->
        <div class="mt-3">
            <h6 class="mb-1">Documentación</h6>
            ${hasDoc ? `
                <div class="border rounded p-2">
                    <div><b>Nombre EUC:</b> ${escapeHtml(euc.Doc.NombreEUC || '')}</div>
                    <div><b>Propósito:</b> ${escapeHtml(euc.Doc.Proposito || '')}</div>
                    <div><b>Proceso:</b> ${escapeHtml(euc.Doc.Proceso || '')}</div>
                    <div><b>Uso:</b> ${escapeHtml(euc.Doc.Uso || '')}</div>
                    ${euc.Doc.Insumos ? `<div><b>Insumos:</b> ${escapeHtml(euc.Doc.Insumos)}</div>` : ``}
                    ${euc.Doc.Responsable ? `<div><b>Responsable:</b> ${escapeHtml(euc.Doc.Responsable)}</div>` : ``}
                    ${euc.Doc.DocTecnica ? `<div class="text-wrap"><b>Doc. técnica:</b> ${escapeHtml(euc.Doc.DocTecnica)}</div>` : ``}
                    ${euc.Doc.EvControl ? `<div><b>Evidencia control:</b> ${escapeHtml(euc.Doc.EvControl)}</div>` : ``}
                    <div class="mt-2">
                        <button class="btn btn-xs btn-outline-secondary" onclick="openEditDoc(${euc.EUCID}, ${euc.Doc.IDoc})">Editar</button>
                        <button class="btn btn-xs btn-outline-danger" onclick="confirmDeleteDoc(${euc.EUCID}, ${euc.Doc.IDoc})">Eliminar</button>
                    </div>
                </div>
            ` : `<div class="muted small">No registrada.</div>`}
        </div>
      </div>
    </div>`;
    }

    // ===== API (llamadas a [WebMethod]) =====
    const api = {
        async call(method, payload) {
            // Llama a /desarrollador.aspx/Method con JSON { ... }
            const res = await fetch('desarrollador.aspx/' + method, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify(payload || {})
            });
            if (!res.ok) throw new Error('HTTP ' + res.status + ' al llamar ' + method);
            // ASP.NET WebMethod envuelve en { d: ... }
            const data = await res.json().catch(() => ({}));
            return (data && data.d !== undefined) ? data.d : data;
        },
        // Helpers concretos (con fallback de firmas)
        async createEUC(euc) {
            // Preferido: CreateEUC(nombre, descripcion, criticidad, estado)
            try {
                const r = await this.call('CreateEUC', {
                    nombre: euc.Nombre, descripcion: euc.Descripcion, criticidad: euc.Criticidad, estado: euc.Estado
                });
                return r;
            } catch {
                // Fallback a 3 parámetros (tu firma actual)
                const r = await this.call('CreateEUC', {
                    nombre: euc.Nombre, descripcion: euc.Descripcion, criticidad: euc.Criticidad
                });
                return r;
            }
        },
        async updateEUC(euc) {
            try {
                // Preferido: UpdateEUC(id, nombre, descripcion, criticidad, estado)
                const r = await this.call('UpdateEUC', {
                    id: euc.EUCID, nombre: euc.Nombre, descripcion: euc.Descripcion,
                    criticidad: euc.Criticidad, estado: euc.Estado
                });
                return r;
            } catch {
                // Fallback a 4 parámetros (tu firma actual)
                const r = await this.call('UpdateEUC', {
                    id: euc.EUCID, nombre: euc.Nombre, descripcion: euc.Descripcion,
                    criticidad: euc.Criticidad
                });
                return r;
            }
        },
        async deleteEUC(id) {
            return await this.call('DeleteEUC', { id });
        },
        async addPlan(plan) {
            // AddPlan(idEUC, responsable, plan)
            return await this.call('AddPlan', {
                idEUC: plan.EUCID, responsable: plan.Responsable, plan: plan.Plan
            });
        },
        async updatePlan(plan) {
            return await this.call('UpdatePlan', {
                idPlan: plan.IdPlan, responsable: plan.Responsable, plan: plan.Plan
            });
        },
        async deletePlan(idPlan) {
            return await this.call('DeletePlan', { idPlan });
        },
        async addDoc(doc) {
            return await this.call('AddDocumentacion', {
                idEUC: doc.EUCID, nombreEUC: doc.NombreEUC, proposito: doc.Proposito, proceso: doc.Proceso, uso: doc.Uso,
                insumos: doc.Insumos || '', responsable: doc.Responsable || '', docTecnica: doc.DocTecnica || '', evControl: doc.EvControl || ''
            });
        },
        async updateDoc(doc) {
            return await this.call('UpdateDocumentacion', {
                idDoc: doc.IDoc, nombreEUC: doc.NombreEUC, proposito: doc.Proposito, proceso: doc.Proceso, uso: doc.Uso,
                insumos: doc.Insumos || '', responsable: doc.Responsable || '', docTecnica: doc.DocTecnica || '', evControl: doc.EvControl || ''
            });
        },
        async deleteDoc(idDoc) {
            return await this.call('DeleteDocumentacion', { idDoc });
        },
        // Lecturas opcionales (si las agregas en tu code-behind)
        async getEUCs() { return await this.call('GetEUCs', {}); },
        async getPlanByEUC(eucid) { return await this.call('GetPlanByEUC', { idEUC: eucid }); },
        async getDocByEUC(eucid) { return await this.call('GetDocByEUC', { idEUC: eucid }); }
    };

    // ===== Render inicial =====
    async function loadInitialEUCs() {
        clearMsg();
        // 1) Intentar por WebMethod GetEUCs
        try {
            if (typeof api.getEUCs === 'function') {
                const list = await api.getEUCs(); // Espera JSON [{EUCID,Nombre,...,Plan?,Doc?}]
                renderCards(list || []);
                if (!list || list.length === 0) showOk('No hay EUCs registradas todavía.');
                return;
            }
        } catch (_) { /* sigue */ }

        // 2) Intentar inyección de JSON desde servidor en litEucsJson (si tú lo llenas)
        try {
            const lit = document.getElementById('<%= litEucsJson.ClientID %>');
            if (lit && lit.textContent && lit.textContent.trim().length > 0) {
                const list = JSON.parse(lit.textContent);
                renderCards(list || []);
                return;
            }
        } catch (_) { /* sigue */ }

        // 3) Fallback vacío
        renderCards([]);
        showOk('Usa "Agregar EUC" para registrar la primera.');
    }

    function renderCards(list) {
        $cards.innerHTML = '';
        (list || []).forEach(e => $cards.insertAdjacentHTML('beforeend', cardTemplate(e)));
    }

    // ===== EUC Handlers =====
    document.getElementById('btnGuardarEuc').addEventListener('click', async () => {
        try {
            clearMsg();
            const euc = {
                EUCID: parseInt(document.getElementById('hdEucId').value || '0', 10),
                Nombre: document.getElementById('txtNombre').value.trim(),
                Descripcion: document.getElementById('txtDescripcion').value.trim(),
                Criticidad: document.getElementById('ddlCriticidad').value,
                Estado: document.getElementById('ddlEstado').value
            };
            // Validaciones
            if (!euc.Nombre || !euc.Descripcion || !euc.Criticidad || !euc.Estado) {
                showErr('Por favor completa todos los campos obligatorios de la EUC.');
                return;
            }
            // Create vs Update
            if (!euc.EUCID) {
                await api.createEUC(euc);
                showOk('EUC creada correctamente.');
            } else {
                await api.updateEUC(euc);
                showOk('EUC actualizada correctamente.');
            }
            // Intenta recargar tarjetas si hay método de lectura
            try {
                const list = await api.getEUCs();
                renderCards(list || []);
            } catch {
                // Sin GetEUCs: avisa al usuario
                showOk('Operación realizada. Si no ves los cambios, recarga la página.');
            }
            closeModal('#mdlEuc');
        } catch (ex) { showErr(ex); }
    });

    function openEditEuc(id) {
        clearMsg();
        // Si tenemos listado en memoria, podríamos traer los datos; aquí pedimos recompletar
        document.getElementById('hdEucId').value = id;
        document.getElementById('txtNombre').value = '';
        document.getElementById('txtDescripcion').value = '';
        document.getElementById('ddlCriticidad').value = '';
        document.getElementById('ddlEstado').value = '';
        openModal('#mdlEuc');
    }

    async function confirmDeleteEuc(id) {
        clearMsg();
        if (!confirm('¿Eliminar esta EUC? Esto debería eliminar también su Plan y Documentación.')) return;
        try {
            await api.deleteEUC(id);
            showOk('EUC eliminada.');
            try {
                const list = await api.getEUCs();
                renderCards(list || []);
            } catch {
                const card = document.getElementById('card-euc-' + id);
                if (card) card.remove();
            }
        } catch (ex) {
            showErr('No fue posible eliminar. Asegúrate de eliminar Plan y Documentación primero, o implementa la eliminación en cascada en el backend.');
        }
    }

    // ===== PLAN (único) =====
    function openAddPlan(eucid) {
        clearMsg();
        document.getElementById('hdPlanId').value = '';
        document.getElementById('hdPlanEucId').value = eucid;
        document.getElementById('txtPlanResponsable').value = '';
        document.getElementById('txtPlanTexto').value = '';
        openModal('#mdlPlan');
    }
    function openEditPlan(eucid, idPlan) {
        clearMsg();
        document.getElementById('hdPlanId').value = idPlan;
        document.getElementById('hdPlanEucId').value = eucid;
        openModal('#mdlPlan');
    }
    document.getElementById('btnGuardarPlan').addEventListener('click', async () => {
        try {
            clearMsg();
            const p = {
                IdPlan: parseInt(document.getElementById('hdPlanId').value || '0', 10),
                EUCID: parseInt(document.getElementById('hdPlanEucId').value || '0', 10),
                Responsable: document.getElementById('txtPlanResponsable').value.trim(),
                Plan: document.getElementById('txtPlanTexto').value.trim()
            };
            if (!p.EUCID || !p.Responsable || !p.Plan) { showErr('Responsable y Plan son obligatorios.'); return; }
            if (!p.IdPlan) {
                await api.addPlan(p);
                showOk('Plan agregado.');
            } else {
                await api.updatePlan(p);
                showOk('Plan actualizado.');
            }
            // Recargar tarjetas si es posible
            try {
                const list = await api.getEUCs(); renderCards(list || []);
            } catch { showOk('Operación realizada. Si no ves el cambio, recarga la página.'); }
            closeModal('#mdlPlan');
        } catch (ex) { showErr(ex); }
    });
    async function confirmDeletePlan(eucid, idPlan) {
        clearMsg();
        if (!confirm('¿Eliminar el plan de automatización?')) return;
        try {
            await api.deletePlan(idPlan);
            showOk('Plan eliminado.');
            try { const list = await api.getEUCs(); renderCards(list || []); } catch { }
        } catch (ex) { showErr(ex); }
    }

    // ===== DOCUMENTACIÓN (única) =====
    function openAddDoc(eucid) {
        clearMsg();
        document.getElementById('hdDocId').value = '';
        document.getElementById('hdDocEucId').value = eucid;
        ['NombreEUC', 'Proposito', 'Proceso', 'Uso', 'Insumos', 'Responsable', 'DocTecnica', 'EvControl']
            .forEach(k => document.getElementById('txtDoc' + k).value = '');
        openModal('#mdlDoc');
    }
    function openEditDoc(eucid, idDoc) {
        clearMsg();
        document.getElementById('hdDocId').value = idDoc;
        document.getElementById('hdDocEucId').value = eucid;
        openModal('#mdlDoc');
    }
    document.getElementById('btnGuardarDoc').addEventListener('click', async () => {
        try {
            clearMsg();
            const d = {
                IDoc: parseInt(document.getElementById('hdDocId').value || '0', 10),
                EUCID: parseInt(document.getElementById('hdDocEucId').value || '0', 10),
                NombreEUC: document.getElementById('txtDocNombreEUC').value.trim(),
                Proposito: document.getElementById('txtDocProposito').value.trim(),
                Proceso: document.getElementById('txtDocProceso').value.trim(),
                Uso: document.getElementById('txtDocUso').value.trim(),
                Insumos: document.getElementById('txtDocInsumos').value.trim(),
                Responsable: document.getElementById('txtDocResponsable').value.trim(),
                DocTecnica: document.getElementById('txtDocTecnica').value.trim(),
                EvControl: document.getElementById('txtDocEvControl').value.trim()
            };
            // Requeridos
            if (!d.EUCID || !d.NombreEUC || !d.Proposito || !d.Proceso || !d.Uso) {
                showErr('Completa los campos obligatorios de Documentación.');
                return;
            }
            if (!d.IDoc) {
                await api.addDoc(d);
                showOk('Documentación agregada.');
            } else {
                await api.updateDoc(d);
                showOk('Documentación actualizada.');
            }
            try { const list = await api.getEUCs(); renderCards(list || []); } catch { }
            closeModal('#mdlDoc');
        } catch (ex) { showErr(ex); }
    });
    async function confirmDeleteDoc(eucid, idDoc) {
        clearMsg();
        if (!confirm('¿Eliminar la documentación?')) return;
        try {
            await api.deleteDoc(idDoc);
            showOk('Documentación eliminada.');
            try { const list = await api.getEUCs(); renderCards(list || []); } catch { }
        } catch (ex) { showErr(ex); }
    }

    // ===== Modal helpers =====
    function openModal(sel) {
        const m = new bootstrap.Modal(document.querySelector(sel));
        m.show();
    }
    function closeModal(sel) {
        const el = document.querySelector(sel);
        const m = bootstrap.Modal.getInstance(el) || new bootstrap.Modal(el);
        m.hide();
    }

    // ===== Seguridad básica XSS =====
    function escapeHtml(s) {
        return (s || '').replace(/[&<>"']/g, m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m]));
    }

    // ===== Init =====
    document.addEventListener('DOMContentLoaded', () => loadInitialEUCs());
</script>
</asp:Content>