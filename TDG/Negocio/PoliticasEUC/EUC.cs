using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* using Negocio.Generales */
/* using Transversal */

namespace Negocio.PoliticasEUC
{
     class EUC
    {
        // Atributos

        public string EUCId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Criticidad { get; set; }
        public string Estado { get; set; }
        // Constructor
        public EUC(string nombre, string descripcion, string criticidad, string estado)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Criticidad = criticidad;
            Estado = estado;
        }

        public class EUCService
        {
            private List<EUC> _eucs = new List<EUC>();
            private int _nextId = 1;
            // CREATE
            public EUC CrearEUC(string EUCId, string nombre, string descripcion , string criticidad, string estado)
            {
                var nuevaEUC = new EUC(_nextId++,EUCId, nombre, descripcion, criticidad);
                _eucs.Add(nuevaEUC);
                return nuevaEUC;
            }
            // READ - obtener todas
            public List<EUC> ObtenerTodas()
            {
                return _eucs;
            }
            // READ - obtener por ID
            public EUC ObtenerPorId(int id)
            {
                return _eucs.Find(e => e.Id == id);
            }
            // UPDATE - actualizar datos de la EUC
            public bool ActualizarEUC(int id, string nuevoNombre = null, string nuevoCriticidad = null, string nuevoEstado = null, string nuevoDescripcion = null)
            {
                var euc = _eucs.Find(e => e.Id == id);
                if (euc == null) return false;
                if (!string.IsNullOrEmpty(nuevoNombre)) euc.Nombre = nuevoNombre;
                if (!string.IsNullOrEmpty(nuevoCriticidad)) euc.Criticidad = nuevoCriticidad;
                if (!string.IsNullOrEmpty(nuevoDescripcion)) euc.Descripcion = nuevoDescripcion;
                if (!string.IsNullOrEmpty(nuevoEstado)) euc.Estado = nuevoEstado;
                return true;
            }
            // DELETE - eliminar EUC
            public bool EliminarEUC(int id)
            {
                var euc = _eucs.Find(e => e.Id == id);
                if (euc == null) return false;
                return _eucs.Remove(euc);
            }
        }
    }
}

