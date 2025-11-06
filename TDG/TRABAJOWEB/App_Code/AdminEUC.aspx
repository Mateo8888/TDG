 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminEUC.aspx.cs" Inherits="PoliticasEUC.TRABAJOWEB.AdminEUC" %>

<asp:Content ID="cHead" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Paleta y layout */
        .header-admin { background:#ffc107; color:#212529; border-radius:.5rem; }
        body, .page-bg { background:#f5f6f8; }
        .card-soft { background:#fff; border:1px solid #e9ecef; border-radius:.5rem; }
        .muted { color:#6c757d; }

        /* Badges */
        .badge-crit-alta { background:#dc3545; }    /* rojo */
        .badge-crit-media { background:#fd7e14; }   /* naranja */
        .badge-crit-baja { background:#198754; }    /* verde */
        .badge-estado-activa { background:#198754; }
        .badge-estado-enconstruccion { background:#0d6efd; }
        .badge-estado-jubilada { background:#6c757d; }
        .badge-cert-pendiente { background:#ffc107; color:#111; }
        .badge-cert-aprobado { background:#198754; }
        .badge-cert-rechazado { background:#dc3545; }

        /* Detalles colapsables */
        .details { display:none; }
        .details.show { display:block; }
        .details .section { background:#fff; border:1px solid #e9ecef; border-radius:.5rem; padding:1rem; margin-bottom: .75rem; }

        /* Pie certificación */
        .cert-footer textarea { resize:vertical; min-height:70px; }
        .required::after { content:" *"; color:#dc3545; }
        .banner-info { border-left:4px solid #0d6efd; background:#e7f1ff; color:#0b5ed7; padding:.5rem .75rem; border-radius:.25rem; }
        .banner-success { border-left:4px solid #198754; background:#e6f4ea; color:#0f5132; padding:.5rem .75rem; border-radius:.25rem; }
        .banner-danger { border-left:4px solid #dc3545; background:#fde8e8; color:#842029; padding:.5rem .75rem; border-radius:.25rem; }
        .cursor-pointer { cursor:pointer; }
        .text-wrap { white-space:pre-wrap; }
    </style>
</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Encabezado amarillo -->
    <div class="header-admin p-4 mb-4">
        <div class="d-flex align-items-center flex-wrap gap-3">
            <div>
                <h3 class="mb-1">Administración de EUC</h3>
                <div class="muted">Revisa la información, Plan y Documentación; certifica cada EUC como <b>Aprobada</b> o <b>Rechazada</b> con comentario.</div>
            </div>
            <div class="ms-auto">
                <button id="btnRefrescar" type="button" class="btn btn-dark">Refrescar</button>
            </div>
        </div>
    </div>

    <!-- Mensajes globales -->
    <div class="mb-3">
        <span id="lblMsg" class="text-success fw-semibold me-3"></span>
        <span id="lblError" class="text-danger fw-semibold"></span>
    </div>

    <!-- Contenedor de tarjetas -->
    <div class="page-bg p-2">
        <div id="adminCards" class="row g-3">
            <!-- JS inyecta tarjetas -->
        </div>
    </div>
</asp:Content>

<asp:Content ID="cScript" ContentPlaceHolderID="ScriptContent" runat="server">
<script>
/* ===== Utilidades ===== */
const $cards = document.getElementById('adminCards');
const $msg = document.getElementById('lblMsg');
const $err = document.getElementById('lblError');
const clearMsg = () => { $msg.textContent=''; $err.textContent=''; };
const ok = (t) => { $err.textContent=''; $msg.textContent=t; };
const ko = (e) => { $msg.textContent=''; $err.textContent=(e && e.message) ? e.message : (e||'Ocurrió un error'); };

const toBadgeCrit = (c) => {
    const v = (c||'').toUpperCase();
    if (v==='ALTA') return 'badge badge-crit-alta';
    if (v==='MEDIA') return 'badge badge-crit-media';
    if (v==='BAJA') return 'badge badge-crit-baja';
    return 'badge bg-secondary';
};
const toBadgeEstado = (e) => {
    const v = (e||'').toLowerCase();
    if (v==='activa') return 'badge badge-estado-activa';
    if (v.startsWith('en constru')) return 'badge badge-estado-enconstruccion';
    if (v==='jubilada') return 'badge badge-estado-jubilada';
    return 'badge bg-secondary';
};
const toBadgeCert = (s) => {
    const v = (s||'').toLowerCase();
    if (v==='aprobado') return 'badge badge-cert-aprobado';
    if (v==='rechazado') return 'badge badge-cert-rechazado';
    return 'badge badge-cert-pendiente';
};
const esc = (s)=> (s||'').replace(/[&<>"']/g, m=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[m]));

/* ===== API WebMethods ===== */
const api = {
    async call(method, payload) {
        const res = await fetch('administradoreuc.aspx/' + method, {
            method:'POST',
            headers:{'Content-Type':'application/json; charset=utf-8'},
            body: JSON.stringify(payload || {})
        });
        if (!res.ok) throw new Error('HTTP ' + res.status + ' en ' + method);
        const data = await res.json().catch(()=> ({}));
        return (data && data.d !== undefined) ? data.d : data;
    },
    async getList() { return await this.call('GetEUCList', {}); },
    async getDetails(id) { return await this.call('GetEUCDetails', { id }); },
    // Recomendado: un único upsert con comentario obligatorio
    async upsertCert(eucid, estadoCert, observacion) {
        // Intenta método recomendado
        try {
            return await this.call('UpsertCertificacion', { eucid, estadoCert, observacion });
        } catch {
            // Fallback: si no existe, intenta Approve/Reject con observación como 2do parámetro
            if (estadoCert === 'Aprobado') {
                try { return await this.call('ApproveEUC', { id: eucid, observacion }); } catch {}
            } else if (estadoCert === 'Rechazado') {
                try { return await this.call('RejectEUC', { id: eucid, observacion }); } catch {}
            }
            // Último recurso: llama a las versiones sin comentario (NO recomendado por regla de negocio)
            if (estadoCert === 'Aprobado') return await this.call('ApproveEUC', { id: eucid });
            if (estadoCert === 'Rechazado') return await this.call('RejectEUC', { id: eucid });
        }
    }
};

/* ===== Render tarjetas ===== */
function cardTemplate(e) {
    const critBadge = toBadgeCrit(e.Criticidad);
    const estBadge  = toBadgeEstado(e.Estado);
    // Estado de certificación se carga cuando abrimos detalles (por si no lo sabes en el listado)
    const certSpan = `<span id="badge-cert-${e.EUCID}" class="badge badge-cert-pendiente">Pendiente</span>`;

    return `
    <div class="col-12 col-md-6 col-lg-4" id="card-${e.EUCID}">
      <div class="card-soft p-3 h-100 d-flex flex-column">
        <div class="d-flex gap-2 align-items-start cursor-pointer" onclick="toggleDetails(${e.EUCID})">
            <div class="flex-grow-1">
                <h5 class="mb-1">${esc(e.Nombre || '')}</h5>
                <div class="d-flex flex-wrap gap-2">
                    <span class="${critBadge}">${esc(e.Criticidad||'-')}</span>
                    <span class="${estBadge}">${esc(e.Estado||'-')}</span>
                    ${certSpan}
                </div>
                <div class="muted small mt-2">Haz clic para ver Plan y Documentación</div>
            </div>
        </div>

        <div id="details-${e.EUCID}" class="details mt-3">
            <div class="section">
                <h6 class="mb-2">Plan de automatización</h6>
                <div id="plan-${e.EUCID}" class="muted">Cargando...</div>
            </div>
            <div class="section">
                <h6 class="mb-2">Documentación</h6>
                <div id="doc-${e.EUCID}" class="muted">Cargando...</div>
            </div>
            <div class="section">
                <h6 class="mb-2">Certificación</h6>
                <div id="cert-banner-${e.EUCID}" class="banner-info">Estado actual: <span id="cert-estado-${e.EUCID}">Pendiente</span></div>
                <div class="cert-footer mt-2">
                    <label class="form-label required">Comentario</label>
                    <textarea id="txtObs-${e.EUCID}" class="form-control" maxlength="255" placeholder="Escribe el motivo de la decisión (requerido)"></textarea>
                    <div class="mt-2 d-flex flex-wrap gap-2">
                        <button class="btn btn-success" onclick="certificar(${e.EUCID}, 'Aprobado')">Aceptar</button>
                        <button class="btn btn-danger" onclick="certificar(${e.EUCID}, 'Rechazado')">Rechazar</button>
                    </div>
                </div>
            </div>
        </div>
      </div>
    </div>`;
}

function renderList(list) {
    $cards.innerHTML = '';
    (list||[]).forEach(e => $cards.insertAdjacentHTML('beforeend', cardTemplate(e)));
}

/* ===== Interacciones ===== */
async function loadList() {
    clearMsg();
    try {
        const list = await api.getList();
        if (!list || list.length===0) ok('No hay EUC registradas.');
        renderList(list || []);
    } catch (ex) { ko(ex); }
}

async function toggleDetails(id) {
    clearMsg();
    const box = document.getElementById('details-' + id);
    if (!box) return;
    const showing = box.classList.contains('show');
    if (showing) { box.classList.remove('show'); return; }

    // Abrir y cargar
    box.classList.add('show');
    try {
        const d = await api.getDetails(id);
        // Plan
        const planBox = document.getElementById('plan-'+id);
        if (d && d.ResponsablePlan) {
            planBox.innerHTML = `
                <div><b>Responsable:</b> ${esc(d.ResponsablePlan)}</div>
                <div class="text-wrap"><b>Plan:</b> ${esc(d.Plan || '')}</div>`;
        } else {
            planBox.textContent = 'No registrado.';
        }
        // Documentación
        const docBox = document.getElementById('doc-'+id);
        if (d && d.Documentacion) {
            // Tu GetEUCDetails devuelve hoy solo "Proposito". Si agregas más campos, los muestro aquí.
            docBox.innerHTML = `<div><b>Resumen:</b> ${esc(d.Documentacion)}</div>`;
        } else {
            docBox.textContent = 'No registrada.';
        }
        // Certificación
        const est = (d && d.EstadoCertificacion) ? d.EstadoCertificacion : 'Pendiente';
        applyCertUI(id, est, /*obs*/ null);
    } catch (ex) {
        document.getElementById('plan-'+id).textContent = 'Error al cargar.';
        document.getElementById('doc-'+id).textContent = 'Error al cargar.';
        ko(ex);
    }
}

async function certificar(eucid, estado) {
    clearMsg();
    const obs = (document.getElementById('txtObs-'+eucid)?.value || '').trim();
    if (!obs) { ko('El comentario es obligatorio para Aceptar o Rechazar.'); return; }

    try {
        await api.upsertCert(eucid, estado, obs);
        applyCertUI(eucid, estado, obs);
        ok(`Certificación ${estado.toLowerCase()} correctamente.`);
    } catch (ex) {
        ko(ex);
    }
}

function applyCertUI(id, estado, observacion) {
    const badge = document.getElementById('badge-cert-'+id);
    const label = document.getElementById('cert-estado-'+id);
    const banner = document.getElementById('cert-banner-'+id);

    const v = (estado||'').toLowerCase();
    badge.className = 'badge ' + (v==='aprobado' ? 'badge-cert-aprobado' : v==='rechazado' ? 'badge-cert-rechazado' : 'badge-cert-pendiente');
    badge.textContent = (estado || 'Pendiente');

    if (label) label.textContent = (estado || 'Pendiente');

    if (banner) {
        banner.className = (v==='aprobado') ? 'banner-success'
                         : (v==='rechazado') ? 'banner-danger'
                         : 'banner-info';
        const obsHtml = observacion ? `<div class="mt-1"><b>Comentario:</b> ${esc(observacion)}</div>` : '';
        banner.innerHTML = `Estado actual: <b>${esc(estado||'Pendiente')}</b>${obsHtml}`;
    }
}

/* ===== Init ===== */
document.getElementById('btnRefrescar').addEventListener('click', loadList);
document.addEventListener('DOMContentLoaded', loadList);
</script>
</asp:Content>