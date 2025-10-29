using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsConexion
    {
        private SqlConnection conexion = new SqlConnection(
           "server=DESKTOP-H2DTJ40;database=PROYECTO_ASISTENCIA; integrated security=true; TrustServerCertificate=True;"
       );


        public SqlConnection mtdAbrirConexion()
        {
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();

            return conexion;
        }


        public SqlConnection mtdCerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
                conexion.Close();

            return conexion;
        }
    }
}
