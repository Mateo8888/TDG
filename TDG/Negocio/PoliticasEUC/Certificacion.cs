using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* using Negocio.Generales */
/* using Transversal */

namespace Negocio.PoliticasEUC
{
    public class Certificacion
    {
        // Atributos
        public string EstadoCert { get; set; }
        //public string Attribute2 { get; set; }
        // Constructor
        public Certificacion(string estadoCert)
        {
            EstadoCert = estadoCert;
            //Attribute2 = attribute2;
        }

        public class CertificacionService
        {
            private static List<Certificacion> certificaciones = new List<Certificacion>();
            private static int contadorId = 1;
            
            public Certificacion Crear(Certificacion nueva)
            {
                nueva.Id = contadorId++;
                nueva.EstadoCert = "Pendiente";
                certificaciones.Add(nueva);
                return nueva;
            }
            // Leer
            public Certificacion ObtenerPorId(int id)
            {
                return certificaciones.Find(c => c.Id == id);
            }
            // Listar
            public List<Certificacion> Listar()
            {
                return certificaciones;
            }
            // Actualizar (Desarrollador puede actualizar su info, Admin cambia estado)
            public bool Actualizar(Certificacion actualizada)
            {
                var existente = ObtenerPorId(actualizada.Id);
                if (existente == null) return false;
                //existente.Nombre = actualizada.Nombre;
                //existente.Fecha = actualizada.Fecha;
                //existente.DesarrolladorAsignado = actualizada.DesarrolladorAsignado;
                existente.EstadoCert = actualizada.EstadoCert; // admin o flujo de aprobación
                return true;
            }
            // Eliminar
            public bool Eliminar(int id)
            {
                var existente = ObtenerPorId(id);
                if (existente == null) return false;
                certificaciones.Remove(existente);
                return true;
            }
        }
    }
}
