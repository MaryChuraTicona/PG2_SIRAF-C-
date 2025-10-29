using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsUsuarioRegistro
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Rol { get; set; } = "";
        // Nada de lógica aquí
    }
}
