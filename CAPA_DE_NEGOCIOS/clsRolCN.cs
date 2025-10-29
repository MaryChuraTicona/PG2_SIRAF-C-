using CAPA_DE_DATOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CAPA_DE_NEGOCIOS
{
    public class clsRolCN
    {
        private readonly clsRolCD cd = new clsRolCD();
        public DataTable Listar() => cd.ListarRoles();
    }
}
