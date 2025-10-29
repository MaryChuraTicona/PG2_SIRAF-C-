using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsCursoCD
    {
        private readonly clsConexion conexion = new clsConexion();

        public DataTable Listar()
        {
            const string SQL = @"SELECT CURSO_ID, CODIGO, NOMBRE, CREDITOS, ESTADO 
                             FROM TB_CURSOS ORDER BY NOMBRE;";

            using (var cn = conexion.mtdAbrirConexion())
            using (var da = new SqlDataAdapter(SQL, cn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool ExisteCodigo(string codigo, int? excluirId = null)
        {
            const string SQL = @"SELECT 1 
                             FROM TB_CURSOS 
                             WHERE UPPER(CODIGO)=UPPER(@c) 
                               AND (@id IS NULL OR CURSO_ID<>@id)";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@c", codigo ?? "");
                cmd.Parameters.AddWithValue("@id", (object)excluirId ?? DBNull.Value);
                return cmd.ExecuteScalar() != null;
            }
        }

        public int Insertar(string codigo, string nombre, int creditos, bool estado)
        {
            const string SQL = @"
INSERT INTO TB_CURSOS(CODIGO,NOMBRE,CREDITOS,ESTADO)
VALUES(@c,@n,@cr,@e);
SELECT SCOPE_IDENTITY();";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@c", codigo);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@cr", creditos);
                cmd.Parameters.AddWithValue("@e", estado);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int Actualizar(int id, string codigo, string nombre, int creditos, bool estado)
        {
            const string SQL = @"
UPDATE TB_CURSOS
SET CODIGO=@c, NOMBRE=@n, CREDITOS=@cr, ESTADO=@e
WHERE CURSO_ID=@id;";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@c", codigo);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@cr", creditos);
                cmd.Parameters.AddWithValue("@e", estado);

                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(int id)
        {
            const string SQL = @"DELETE FROM TB_CURSOS WHERE CURSO_ID=@id;";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
        }
    }

}
