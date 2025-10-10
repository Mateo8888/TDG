using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* using Negocio.Generales */
/* using Transversal */

namespace Negocio.PoliticasEUC
{
    public class PlanAutomatizacion
    {
        // Atributos
        public string IdPlan { get; set; }
        public string Responsable { get; set; }
        public string Plan { get; set; }
        // Constructor
        public PlanAutomatizacion(string responsable, string plan, string idPlan)
        {
            Responsable = responsable;
            Plan = plan;
            IdPlan = idPlan;
        }

        public class PlanAutomatizacionService
        {
            private static List<PlanAutomatizacion> planes = new List<PlanAutomatizacion>();
            private static int contadorId = 1;
            // Crear (solo si EUC es de criticidad alta)
            public PlanAutomatizacion Crear(PlanAutomatizacion nuevo, string criticidadEUC)
            {
                if (criticidadEUC != "Alta")
                {
                    throw new InvalidOperationException("Solo se permite plan de automatización para EUC de criticidad Alta.");
                }
                nuevo.IdPlan = contadorIdPlan++;
                planes.Add(nuevo);
                return nuevo;
            }
            // Leer
            public PlanAutomatizacion ObtenerPorId(int id)
            {
                return planes.Find(p => p.IdPlan == idPlan);
            }
            // Listar
            public List<PlanAutomatizacion> Listar()
            {
                return planes;
            }
            // Actualizar
            public bool Actualizar(PlanAutomatizacion actualizado)
            {
                var existente = ObtenerPorId(actualizado.IdPlan);
                if (existente == null) return false;
                existente.Responsable = actualizado.Responsable;
                existente.Plan = actualizado.Plan;
                return true;
            }
            // Eliminar
            public bool Eliminar(int id)
            {
                var existente = ObtenerPorId(id);
                if (existente == null) return false;
                planes.Remove(existente);
                return true;
            }
        }
}
