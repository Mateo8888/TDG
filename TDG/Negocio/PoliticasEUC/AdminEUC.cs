using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* using Negocio.Generales */
/* using Transversal */

namespace Negocio.PoliticasEUC
{
     public class AdminEUC
    {
        // Atributos
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Rol { get; set; } = "admin";
        // Constructor
        public AdminEUC(string nombre, string usuario)
        {
            Nombre = nombre;
            Usuario = usuario;
        }
    }
}
