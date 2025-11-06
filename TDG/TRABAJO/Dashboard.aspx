<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TRABAJO.App_Code.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

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
                <button id="btnRefrescar" type="button" class="btn btn-dark">Refrescar</button>
            </div>
        </div>
    </div>

    <!-- Mensajes -->
    <div class="mb-3">
        <span id="lblMsg" class="text-success fw-semibold me-3"></span>
        <span id="lblError" class="text-danger fw-semibold"></span>
    </div>

    <!-- KPIs -->
    <div class="row g-3 mb-3">
        <div class="col-6 col-md-2">
            <div class="kpi kpi-total">
                <div class="muted text-white-50">Total EUC</div>
                <div class="h4 m-0"><span id="kpiTotal">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-plan">
                <div class="muted text-white-50">Con Plan</div>
                <div class="h4 m-0"><span id="kpiPlan">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-doc">
                <div class="muted text-white-50">Con Doc</div>
                <div class="h4 m-0"><span id="kpiDoc">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-aprob">
                <div class="muted text-white-50">Aprobadas</div>
                <div class="h4 m-0"><span id="kpiAprob">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-rech">
                <div class="muted text-white-50">Rechazadas</div>
                <div class="h4 m-0"><span id="kpiRech">0</span></div>
            </div>
        </div>
        <div class="col-6 col-md-2">
            <div class="kpi kpi-pend">
                <div class="muted text-dark-50">Pendientes</div>
                <div class="h4 m-0"><span id="kpiPend">0</span></div>
            </div>
        </div>
    </div>

    <!-- Filtros -->
    <div class="card-soft p-3 mb-3">
        <div class="row g-3 align-items-end">
            <div class="col-md-3">
                <label class="form-label">Buscar</label>
                <input id="txtBuscar" class="form-control" placeholder="Nombre..." />
            </div>
            <div class="col-md-3">
                <label class="form-label">Criticidad</label>
                <select id="ddlCriticidad" class="form-select">
                    <option value="">Todas</option>
                    <option value="ALTA">ALTA</option>
                    <option value="MEDIA">MEDIA</option>
                    <option value="BAJA">BAJA</option>
                </select>
            </div>
            <div class="col-md-3">
                <label class="form-label">Estado (EUC)</label>
                <select id="ddlEstado" class="form-select">
                    <option value="">Todos</option>
                    <option value="Activa">Activa</option>
                    <option value="En construcción">En construcción</option>
                    <option value="Jubilada">Jubilada</option>
                </select>
            </div>
            <div class="col-md-3">
                <label class="form-label">Certificación</label>
                <select id="ddlCert" class="form-select">
                    <option value="">Todas</option>
                    <option value="Aprobado">Aprobado</option>
                    <option value="Rechazado">Rechazado</option>
                    <option value="Pendiente">Pendiente</option>
                </select>
            </div>
            <div class="col-12 d-flex gap-2">
                <button id="btnFiltrar" class="btn btn-primary">Filtrar</button>
                <button id="btnLimpiar" class="btn btn-outline-secondary">Limpiar</button>
            </div>
        </div>
    </div>

    <!-- Listado de tarjetas -->
    <div class="page-bg p-2">
        <div id="cardsDash" class="row g-3">
            <!-- JS inyecta tarjetas -->
        </div>
    </div>

    <!-- Modal detalle (lectura) -->
    <div class="modal fade" id="mdlDetalle" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header" style="background:#ffc107;color:#212529;">
                    <h5 class="modal-title">Detalle de EUC</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3"><b>Nombre:</b> <span id="detNombre"></span></div>
                    <div class="mb-3"><b>Criticidad:</b> <span id="detCrit"></span> &nbsp; | &nbsp; <b>Estado:</b> <span id="detEstado"></span></div>
                    <div class="mb-3">
                        <h6>Plan de automatización</h6>
                        <div id="detPlan" class="text-wrap muted">N/D</div>
                    </div>
                    <div class="mb-3">
                        <h6>Documentación</h6>
                        <div id="detDoc" class="text-wrap muted">N/D</div>
                    </div>
                    <div class="mb-3">
                        <h6>Certificación</h6>
                        <div id="detCert" class="text-wrap muted">Pendiente</div>
                    </div>
                </div>
                <div class="modal-footer bg-light">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="cScript" ContentPlaceHolderID="ScriptContent" runat="server">
<script>
/* ===== Helpers ===== */
const $cards = document.getElementById('cardsDash');
const $msg = document.getElementById('lblMsg');
const $err = document.getElementById('lblError');
const ok = (t)=>{ $err.textContent=''; $msg.textContent=t||''; };
const ko = (e)=>{ $msg.textContent=''; $err.textContent=(e&&e.message)?e.message:(e||'Ocurrió un error'); };
const esc = (s)=> (s||'').replace(/[&<>"']/g, m=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[m]));

const critBadge = (c)=> {
  const v=(c||'').toUpperCase();
  if(v==='ALTA') return 'badge badge-crit-alta';
  if(v==='MEDIA') return 'badge badge-crit-media';
  if(v==='BAJA') return 'badge badge-crit-baja';
  return 'badge bg-secondary';
};
const estBadge = (e)=> {
  const v=(e||'').toLowerCase();
  if(v==='activa') return 'badge badge-estado-activa';
  if(v.startsWith('en constru')) return 'badge badge-estado-enconstruccion';
  if(v==='jubilada') return 'badge badge-estado-jubilada';
  return 'badge bg-secondary';
};
function stateClass(color) {
  const v=(color||'').toLowerCase();
  if(v.includes('verde')) return 'state-verde';
  if(v.includes('rojo')) return 'state-rojo';
  return 'state-azul';
}

/* ===== API ===== */
const api = {
  async get() {
    const res = await fetch('dashboard.aspx/GetDashboardData', {
      method:'POST', headers:{'Content-Type':'application/json; charset=utf-8'}, body:'{}'
    });
    if(!res.ok) throw new Error('HTTP ' + res.status);
    const data = await res.json().catch(()=> ({}));
    return (data && data.d !== undefined) ? data.d : data;
  }
};

let RAW = []; // datos crudos del backend

/* ===== KPIs ===== */
function computeKPIs(list){
  const total = list.length;
  let conPlan=0, conDoc=0, aprob=0, rech=0, pend=0;
  list.forEach(x=>{
    const plan = (x.PlanAutomatizacion||'').toLowerCase();
    const doc  = (x.Documentacion||'').toLowerCase();
    const cert = (x.Certificacion||'Pendiente').toLowerCase();

    if(plan==='completo' || plan==='si' || plan==='true') conPlan++;
    if(doc==='completa' || doc==='si' || doc==='true') conDoc++;
    if(cert==='aprobado' || cert==='aprobada') aprob++;
    else if(cert==='rechazado' || cert==='rechazada') rech++;
    else pend++;
  });
  document.getElementById('kpiTotal').textContent = total;
  document.getElementById('kpiPlan').textContent  = conPlan;
  document.getElementById('kpiDoc').textContent   = conDoc;
  document.getElementById('kpiAprob').textContent = aprob;
  document.getElementById('kpiRech').textContent  = rech;
  document.getElementById('kpiPend').textContent  = pend;
}

/* ===== Filtros ===== */
function applyFilters(){
  const txt = (document.getElementById('txtBuscar').value||'').toLowerCase();
  const crit = document.getElementById('ddlCriticidad').value;
  const est  = document.getElementById('ddlEstado').value;
  const cert = document.getElementById('ddlCert').value;

  let list = RAW.slice();
  list = list.filter(x=>{
    const name = (x.Nombre || x.NombreEUC || '').toLowerCase();
    const passTxt = !txt || name.includes(txt);

    const c = (x.Criticidad||'').toUpperCase();
    const passCrit = !crit || c===crit;

    const e = (x.Estado||''); // 'Activa' | 'En construcción' | 'Jubilada'
    const passEst = !est || e===est;

    const ce = (x.Certificacion||'Pendiente'); // 'Aprobado' | 'Rechazado' | 'Pendiente'
    const passCert = !cert || ce===cert;

    return passTxt && passCrit && passEst && passCert;
  });

  renderCards(list);
  computeKPIs(list);
}

/* ===== Render ===== */
function cardTemplate(x){
  const id   = x.EUCID || x.Id;
  const name = x.Nombre || x.NombreEUC || ('EUC #' + id);
  const crit = x.Criticidad || '';
  const est  = x.Estado || '';
  const cert = x.Certificacion || 'Pendiente';
  const doc  = x.Documentacion || 'Incompleta';
  const plan = x.PlanAutomatizacion || 'Incompleto';

  const critB = critBadge(crit);
  const estB  = estBadge(est);

  // Estado general (color) viene en x.EstadoColor o lo calculamos client-side
  const stClass = stateClass(x.EstadoColor || calcEstadoColor(cert, doc, plan));

  const chipPlan = (plan.toLowerCase()==='completo' || plan.toLowerCase()==='si' || plan===true)
    ? `<span class="chip chip-ok">Plan: Completo</span>`
    : `<span class="chip chip-bad">Plan: Incompleto</span>`;

  const chipDoc = (doc.toLowerCase()==='completa' || doc.toLowerCase()==='si' || doc===true)
    ? `<span class="chip chip-ok">Doc: Completa</span>`
    : `<span class="chip chip-bad">Doc: Incompleta</span>`;

  const chipCert = (cert.toLowerCase().startsWith('aprob'))
    ? `<span class="chip chip-ok">Cert: Aprobado</span>`
    : (cert.toLowerCase().startsWith('rech'))
      ? `<span class="chip chip-bad">Cert: Rechazado</span>`
      : `<span class="chip chip-warn">Cert: Pendiente</span>`;

  return `
  <div class="col-12 col-md-6 col-lg-4" id="card-${id}">
    <div class="card-soft p-3 h-100 d-flex flex-column ${stClass}">
      <div class="d-flex align-items-start justify-content-between">
        <div class="flex-grow-1">
          <h5 class="mb-1">${esc(name)}</h5>
          <div class="d-flex flex-wrap gap-2">
            ${crit ? `<span class="${critB}">${esc(crit)}</span>` : ``}
            ${est ? `<span class="${estB}">${esc(est)}</span>` : ``}
          </div>
        </div>
        <button class="btn btn-sm btn-outline-secondary" onclick="openDetalle(${id})">Ver</button>
      </div>
      <div class="mt-3">
        ${chipPlan} ${chipDoc} ${chipCert}
      </div>
    </div>
  </div>`;
}

function renderCards(list){
  $cards.innerHTML = '';
  (list||[]).forEach(x => $cards.insertAdjacentHTML('beforeend', cardTemplate(x)));
}

function calcEstadoColor(cert, doc, plan) {
  const c=(cert||'').toLowerCase(), d=(doc||'').toLowerCase(), p=(plan||'').toLowerCase();
  if ((c==='aprobado' || c==='aprobada') && (d==='completa'||d==='si') && (p==='completo'||p==='si')) return 'Verde';
  if (c==='rechazado' || c==='rechazada' || d==='incompleta' || p==='incompleto') return 'Rojo';
  return 'Azul';
}

/* ===== Detalle (modal simple con lo que tengamos) ===== */
function openDetalle(id){
  const x = RAW.find(z => (z.EUCID||z.Id) === id);
  if(!x) return;
  document.getElementById('detNombre').textContent = x.Nombre || x.NombreEUC || ('EUC #' + id);
  document.getElementById('detCrit').textContent   = x.Criticidad || 'N/D';
  document.getElementById('detEstado').textContent = x.Estado || 'N/D';

  const doc = x.Documentacion || 'N/D';
  const plan= x.PlanAutomatizacion || 'N/D';
  const cert= x.Certificacion || 'Pendiente';

  document.getElementById('detPlan').textContent = (plan==='N/D') ? 'N/D' : plan;
  document.getElementById('detDoc').textContent  = (doc==='N/D') ? 'N/D' : doc;
  document.getElementById('detCert').textContent = cert;

  const m = new bootstrap.Modal(document.getElementById('mdlDetalle'));
  m.show();
}

/* ===== Init / Eventos ===== */
async function load(){
  try{
    $msg.textContent=''; $err.textContent='';
    RAW = await api.get();
    renderCards(RAW);
    computeKPIs(RAW);
  }catch(ex){ ko(ex); }
}

document.getElementById('btnRefrescar').addEventListener('click', load);
document.getElementById('btnFiltrar').addEventListener('click', (e)=>{ e.preventDefault(); applyFilters(); });
document.getElementById('btnLimpiar').addEventListener('click', (e)=>{ 
  e.preventDefault();
  document.getElementById('txtBuscar').value='';
  document.getElementById('ddlCriticidad').value='';
  document.getElementById('ddlEstado').value='';
  document.getElementById('ddlCert').value='';
  renderCards(RAW); computeKPIs(RAW);
});
document.addEventListener('DOMContentLoaded', load);
</script>
</asp:Content>
