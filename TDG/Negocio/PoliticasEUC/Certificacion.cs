using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Negocio.Generales; 
// using Transversal;

namespace Negocio.PoliticasEUC
{
    public class Certificacion
    {
        // Clave primaria (la setea el servicio)
        public int IdCert { get; set; }

        // Referencia al padre EUC (FK)
        public int EUCID { get; set; }
        public EUC EUC { get; set; }
        public string EstadoCert { get; set; } = "Pendiente";
        public DateTime FechaControl { get; set; }
        public string Observacion { get; set; }     
        public Certificacion() { }

        // Constructor principal
        public Certificacion(int eucid, string estadoCert = "Pendiente")
        {
            EUCID = eucid;
            EstadoCert = estadoCert;
        }

        public class CertificacionService
        {
            private static readonly List<Certificacion> certificaciones = new List<Certificacion>();
            private static int contadorId = 1;

            // CREATE
            public Certificacion Crear(Certificacion nueva)
            {
                if (nueva == null) throw new ArgumentNullException(nameof(nueva));

                nueva.IdCert = contadorId++;

                // Asegurar estado por defecto
                if (string.IsNullOrWhiteSpace(nueva.EstadoCert))
                    nueva.EstadoCert = "Pendiente";

                // Si no viene fecha informada, setear ahora (ajústalo a tu regla si solo quieres fecha para Aprobada/Rechazada)
                if (nueva.FechaControl == default(DateTime))
                    nueva.FechaControl = DateTime.Now;

                // Observación opcional: normalizamos espacios
                if (nueva.Observacion != null)
                    nueva.Observacion = nueva.Observacion.Trim();

                certificaciones.Add(nueva);
                return nueva;
            }

            // READ
            public Certificacion ObtenerPorId(int idCert)
            {
                return certificaciones.FirstOrDefault(c => c.IdCert == idCert);
            }

            public List<Certificacion> Listar()
            {
                return new List<Certificacion>(certificaciones);
            }

            public List<Certificacion> ListarPorEUC(int eucid)
            {
                return certificaciones.Where(c => c.EUCID == eucid).ToList();
            }

            public Certificacion CertificarEUC(int eucId, bool aprobado, string observacion)
            {
                var nuevaCert = new Certificacion
                {
                    EUCID = eucId,
                    EstadoCert = aprobado ? "Aprobada" : "Rechazada",
                    FechaControl = DateTime.Now,
                    Observacion = observacion?.Trim()
                };

                return Crear(nuevaCert);
            }
            // UPDATE
            public bool Actualizar(Certificacion actualizada)
            {
                if (actualizada == null) return false;

                var existente = ObtenerPorId(actualizada.IdCert);
                if (existente == null) return false;

                // Estado (si viene)
                if (!string.IsNullOrWhiteSpace(actualizada.EstadoCert))
                    existente.EstadoCert = actualizada.EstadoCert.Trim();

                // Observación (si viene)
                if (actualizada.Observacion != null)
                    existente.Observacion = actualizada.Observacion.Trim();

                // Fecha de control (si viene)
                if (actualizada.FechaControl != default(DateTime))
                    existente.FechaControl = actualizada.FechaControl;

                // Si permites cambiar la asociación:
                // existente.EUCID = actualizada.EUCID;

                return true;
            }

            // DELETE
            public bool Eliminar(int idCert)
            {
                var existente = ObtenerPorId(idCert);
                if (existente == null) return false;
                return certificaciones.Remove(existente);
            }

            // --- OPCIONAL: helpers directos para aprobar/rechazar con fecha/observación ---
            public Certificacion Aprobar(int idCert, string observacion = null, DateTime? fecha = null)
            {
                var c = ObtenerPorId(idCert);
                if (c == null) throw new InvalidOperationException("Certificación no encontrada.");

                c.EstadoCert = "Aprobada";
                c.Observacion = observacion?.Trim();
                c.FechaControl = fecha ?? DateTime.Now;
                return c;
            }

            public Certificacion Rechazar(int idCert, string observacion = null, DateTime? fecha = null)
            {
                var c = ObtenerPorId(idCert);
                if (c == null) throw new InvalidOperationException("Certificación no encontrada.");

                c.EstadoCert = "Rechazada";
                c.Observacion = observacion?.Trim();
                c.FechaControl = fecha ?? DateTime.Now;
                return c;
            }
            // --- FIN OPCIONAL ---
        }
    }

}


