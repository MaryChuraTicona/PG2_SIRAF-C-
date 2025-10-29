using CAPA_DE_DATOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CAPA_DE_NEGOCIOS
{
    public class clsResultadoLogin
    {
        public bool EsValido { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public List<string> Permisos { get; set; } = new List<string>();

        // Datos mínimos que la UI necesita mostrar
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = "";
        public string Correo { get; set; } = "";
    }

}
