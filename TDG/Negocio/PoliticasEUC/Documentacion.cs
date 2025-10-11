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
        // Clave primaria de Documentación
        public int IDoc { get; set; }

        // FK hacia el padre EUC
        public int EUCID { get; set; }

        // Atributos
        public string NombreEUC { get; set; }
        public string Proposito { get; set; }
        public string Proceso { get; set; }
        public string Uso { get; set; }
        public string Insumos { get; set; }
        public string Responsable { get; set; }
        public string DocTecnica { get; set; }
        public string EvControl { get; set; }
        public EUC EUC { get; set; }       

        // Constructor por defecto 
        public Documentacion() { }

        // Constructor principal: 
        public Documentacion(
            int eucid,
            string nombreEUC,
            string proposito,
            string proceso,
            string uso,
            string insumos,
            string responsable,
            string docTecnica,
            string evControl)
        {
            EUCID = eucid;
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
            private readonly List<Documentacion> _docs = new List<Documentacion>();
            private int _nextId = 1;

            // CREATE
            public Documentacion CrearDocumentacion(
                int eucid,
                string nombreEUC,
                string proposito,
                string proceso,
                string uso,
                string insumos,
                string responsable,
                string docTecnica,
                string evControl)
            {
                
                var nuevaDoc = new Documentacion(
                    eucid,
                    nombreEUC,
                    proposito,
                    proceso,
                    uso,
                    insumos,
                    responsable,
                    docTecnica,
                    evControl)
                {
                    IDoc = _nextId++
                };

                _docs.Add(nuevaDoc);
                return nuevaDoc;
            }

            // READ - 
            public List<Documentacion> ObtenerTodas()
            {
                return new List<Documentacion>(_docs);
            }

            // READ - por EUC
            public List<Documentacion> ObtenerPorEUC(int eucId)
            {
                return _docs.Where(d => d.EUCID == eucId).ToList();
            }

            // READ - por ID de Documentación
            public Documentacion ObtenerPorId(int idDoc)
            {
                return _docs.FirstOrDefault(d => d.IDoc == idDoc);
            }

            // UPDATE
            public bool ActualizarDocumentacion(
                int idDoc,                    
                int? nuevoEUCID = null,
                string nuevoNombreEUC = null,
                string nuevoProposito = null,
                string nuevoProceso = null,
                string nuevoUso = null,
                string nuevosInsumos = null,
                string nuevoResponsable = null,
                string nuevaDocTecnica = null,
                string nuevaEvControl = null)
            {
                var doc = ObtenerPorId(idDoc);
                if (doc == null) return false;

                if (nuevoEUCID.HasValue) doc.EUCID = nuevoEUCID.Value;
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
            public bool EliminarDocumentacion(int idDoc)
            {
                var doc = ObtenerPorId(idDoc);
                if (doc == null) return false;
                return _docs.Remove(doc);
            }
        }
    }
}
