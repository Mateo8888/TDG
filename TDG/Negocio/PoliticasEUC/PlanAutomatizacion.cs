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
        public int IdPlan { get; set; }       
        public string Responsable { get; set; }
        public string Plan { get; set; }
        public int EUCID { get; set; }
        public EUC EUC { get; set; }       
        // Constructor
        public PlanAutomatizacion(int eucid, string responsable, string plan)
        {
            EUCID = eucid;
            Responsable = responsable;
            Plan = plan;
        }

        public class PlanAutomatizacionService
        {
            private static readonly List<PlanAutomatizacion> planes = new List<PlanAutomatizacion>();
            private static int contadorId = 1;

            //Listar por ID de EUC
            public List<PlanAutomatizacion> ListarPorEUC(int eucid)
            {
                return planes.Where(p => p.EUCID == eucid).ToList();
            }
            // Crear (solo si EUC es de criticidad alta)
            public PlanAutomatizacion Crear(PlanAutomatizacion nuevo, string criticidadEUC)
            {
                if (!string.Equals(criticidadEUC, "Alta", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Solo se permite plan de automatización para EUC de criticidad Alta.");
                }

                nuevo.IdPlan = contadorId++;             
                planes.Add(nuevo);
                return nuevo;
            }

            // Leer
            public PlanAutomatizacion ObtenerPorId(int id)
            {
                return planes.Find(p => p.IdPlan == id);                  
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
                // existente.EUCID = actualizado.EUCID; 
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
}
