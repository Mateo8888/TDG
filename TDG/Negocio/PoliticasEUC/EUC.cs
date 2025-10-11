using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* using Negocio.Generales */
/* using Transversal */

namespace Negocio.PoliticasEUC
{
    public class EUC
    {
        public int EUCID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Criticidad { get; set; }
        public string Estado { get; set; }

        // Navegación (opción B)
        public List<Certificacion> Certificaciones { get; set; } = new List<Certificacion>();
        public List<Documentacion> Documentaciones { get; set; } = new List<Documentacion>();
        public List<PlanAutomatizacion> Planes { get; set; } = new List<PlanAutomatizacion>();

        public class EUCService
        {
            private readonly List<EUC> _eucs = new List<EUC>();
            private int _nextId = 1;

            // Dependencias a servicios hijos
            private readonly Certificacion.CertificacionService _certSvc;
            private readonly Documentacion.DocumentacionService _docSvc;
            private readonly PlanAutomatizacion.PlanAutomatizacionService _planSvc;

            // Inyecta (pásame las mismas instancias que uses en el resto de la app)
            public EUCService(
                Certificacion.CertificacionService certSvc,
                Documentacion.DocumentacionService docSvc,
                PlanAutomatizacion.PlanAutomatizacionService planSvc)
            {
                _certSvc = certSvc;
                _docSvc = docSvc;
                _planSvc = planSvc;
            }

            // Si prefieres, puedes agregar un ctor sin parámetros que cree implementaciones por defecto
            // (solo si sus listas internas son estáticas o compartidas)
            // public EUCService() : this(new Certificacion.CertificacionService(),
            //                            new Documentacion.DocumentacionService(),
            //                            new PlanAutomatizacion.PlanAutomatizacionService()) {}

            // CREATE
            public EUC CrearEUC(string nombre, string descripcion, string criticidad, string estado)
            {
                var nuevaEUC = new EUC
                {
                    EUCID = _nextId++,
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Criticidad = criticidad,
                    Estado = estado
                };

                _eucs.Add(nuevaEUC);
                return nuevaEUC;
            }

            // READ
            public List<EUC> ObtenerTodas() => _eucs;

            public EUC ObtenerPorId(int id)
            {
                return _eucs.FirstOrDefault(e => e.EUCID == id);
            }

            // UPDATE
            public bool ActualizarEUC(
                int id,
                string nuevoNombre = null,
                string nuevaCriticidad = null,
                string nuevoEstado = null,
                string nuevaDescripcion = null)
            {
                var euc = ObtenerPorId(id);
                if (euc == null) return false;

                if (!string.IsNullOrWhiteSpace(nuevoNombre)) euc.Nombre = nuevoNombre.Trim();
                if (!string.IsNullOrWhiteSpace(nuevaCriticidad)) euc.Criticidad = nuevaCriticidad.Trim();
                if (!string.IsNullOrWhiteSpace(nuevaDescripcion)) euc.Descripcion = nuevaDescripcion.Trim();
                if (!string.IsNullOrWhiteSpace(nuevoEstado)) euc.Estado = nuevoEstado.Trim();
                return true;
            }

            // DELETE con cascade manual
            public bool EliminarEUC(int id)
            {
                var euc = ObtenerPorId(id);
                if (euc == null) return false;

                // 1) Borrar hijos (snapshot con ToList())
                var certs = _certSvc.ListarPorEUC(id).ToList();   // Certificacion: ListarPorEUC ✅
                foreach (var c in certs)
                    _certSvc.Eliminar(c.IdCert);

                var docs = _docSvc.ObtenerPorEUC(id).ToList();   // Documentacion: **ObtenerPorEUC** ❗
                foreach (var d in docs)
                    _docSvc.EliminarDocumentacion(d.IDoc);

                var planes = _planSvc.ListarPorEUC(id).ToList();   // Plan: ListarPorEUC ✅
                foreach (var p in planes)
                    _planSvc.Eliminar(p.IdPlan);

                // 2) Limpiar navegación (opcional)
                euc.Certificaciones.Clear();
                euc.Documentaciones.Clear();
                euc.Planes.Clear();

                // 3) Eliminar la EUC
                return _eucs.Remove(euc);
            }
        }
    }
}
