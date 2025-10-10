using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* using Negocio.Generales */
/* using Transversal */

namespace Negocio.PoliticasEUC
{
    public class Documentacion
    {
        // Atributos
        public string NombreEUC { get; set; }
        public string Proposito { get; set; }
        public string Proceso { get; set; }
        public string Uso { get; set; }
        public string Insumos { get; set; }
        public string Responsable { get; set; }
        public string DocTecnica { get; set; }
        public string EvControl { get; set; }
        // Constructor
        public Documentacion(string nombreEUC, string proposito, string proceso, string uso,
                             string insumos, string responsable, string docTecnica, string evControl)
        {
            NombreEUC = nombreEUC;
            Proposito = proposito;
            Proceso = proceso;
            Uso = uso;
            Insumos = insumos;
            Responsable = responsable;
            DocTecnica = docTecnica;
            EvControl = evControl;
        }

        public class DocumentacionService
        {
            private List<Documentacion> _docs = new List<Documentacion>();
            private int _nextId = 1;
            // CREATE
            public Documentacion CrearDocumentacion(string EUCId, string NombreEUC, string Proposito, string Proceso, string Uso, string Insumos, string Responsable, string DocTecnica, string EvControl )
            {
                var nuevaDoc = new Documentacion(_nextId++, EUCId, NombreEUC, Proposito, Uso, Insumos, Responsable, DocTecnica, EvControl);
                _docs.Add(nuevaDoc);
                return nuevaDoc;
            }
            // READ - todas las documentaciones
            public List<Documentacion> ObtenerTodas()
            {
                return _docs;
            }
            // READ - por EUC
            public List<Documentacion> ObtenerPorEUC(int eucId)
            {
                return _docs.FindAll(d => d.EUCId == eucId);
            }
            // READ - por ID de Documentación
            public Documentacion ObtenerPorId(int id)
            {
                return _docs.Find(d => d.Id == id);
            }
            // UPDATE
            public bool ActualizarDocumentacion(
        string nuevoEUCId = null,
        string nuevoNombreEUC = null,
        string nuevoProposito = null,
        string nuevoProceso = null,
        string nuevoUso = null,
        string nuevosInsumos = null,
        string nuevoResponsable = null,
        string nuevaDocTecnica = null,
        string nuevaEvControl = null)
            {

                var doc = _docs.FirstOrDefault(d => d.Id == id);
                if (doc == null) return false;

                if (!string.IsNullOrWhiteSpace(nuevoNombreEUC)) doc.NombreEUC = nuevoNombreEUC.Trim();
                if (!string.IsNullOrWhiteSpace(nuevoProposito)) doc.Proposito = nuevoProposito.Trim();
                if (!string.IsNullOrWhiteSpace(nuevoProceso)) doc.Proceso = nuevoProceso.Trim();
                if (!string.IsNullOrWhiteSpace(nuevoUso)) doc.Uso = nuevoUso.Trim();
                if (!string.IsNullOrWhiteSpace(nuevosInsumos)) doc.Insumos = nuevosInsumos.Trim();
                if (!string.IsNullOrWhiteSpace(nuevoResponsable)) doc.Responsable = nuevoResponsable.Trim();

                if (!string.IsNullOrWhiteSpace(nuevaDocTecnica)) doc.DocTecnica = nuevaDocTecnica.Trim();
                if (!string.IsNullOrWhiteSpace(nuevaEvControl)) doc.EvControl = nuevaEvControl.Trim();

                return true;

            }
            // DELETE
            public bool EliminarDocumentacion(int id)
            {
                var doc = _docs.Find(d => d.Id == id);
                if (doc == null) return false;
                return _docs.Remove(doc);
            }
        }
}
