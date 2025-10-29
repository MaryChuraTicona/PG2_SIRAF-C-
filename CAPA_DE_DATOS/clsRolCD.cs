using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsRolCD
    {
        private readonly clsConexion conexion = new clsConexion();

        public DataTable ListarRoles()
        {
            const string SQL = "SELECT ROL_ID, ROL_NOMBRE FROM TB_ROL ORDER BY ROL_NOMBRE;";
            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(SQL, cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
