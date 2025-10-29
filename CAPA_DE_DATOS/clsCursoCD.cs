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

        
        public DataTable ListarCursos()
        {
            string sql = "SELECT * FROM TB_CURSOS ORDER BY NOMBRE";
            using (var cn = conexion.mtdAbrirConexion())
            using (var da = new SqlDataAdapter(sql, cn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public int InsertarCurso(string codigo, string nombre, int creditos, bool estado)
        {
            string sql = @"INSERT INTO TB_CURSOS(CODIGO, NOMBRE, CREDITOS, ESTADO)
                       VALUES(@c,@n,@cr,@e);
                       SELECT SCOPE_IDENTITY();";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@c", codigo);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@cr", creditos);
                cmd.Parameters.AddWithValue("@e", estado);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int ActualizarCurso(int id, string codigo, string nombre, int creditos, bool estado)
        {
            string sql = @"UPDATE TB_CURSOS
                       SET CODIGO=@c, NOMBRE=@n, CREDITOS=@cr, ESTADO=@e
                       WHERE CURSO_ID=@id";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@c", codigo);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@cr", creditos);
                cmd.Parameters.AddWithValue("@e", estado);

                return cmd.ExecuteNonQuery();
            }
        }

        public int EliminarCurso(int id)
        {
            string sql = "DELETE FROM TB_CURSOS WHERE CURSO_ID=@id";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
        }

       
        public DataTable ListarPrerequisitos(int cursoId)
        {
            string sql = @"SELECT c.CURSO_ID, c.NOMBRE,
                       CASE WHEN p.PREREQ_CURSO_ID IS NULL THEN 0 ELSE 1 END AS TIENE
                       FROM TB_CURSOS c
                       LEFT JOIN TB_CURSOS_PREREQ p 
                       ON p.PREREQ_CURSO_ID = c.CURSO_ID AND p.CURSO_ID = @id
                       WHERE c.CURSO_ID <> @id";

            using (var cn = conexion.mtdAbrirConexion())
            using (var da = new SqlDataAdapter(sql, cn))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", cursoId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertarPrerequisito(int cursoId, int prereqId)
        {
            string sql = @"INSERT INTO TB_CURSOS_PREREQ(CURSO_ID, PREREQ_CURSO_ID)
                       VALUES(@c,@p)";
            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@c", cursoId);
                cmd.Parameters.AddWithValue("@p", prereqId);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarPrerequisito(int cursoId, int prereqId)
        {
            string sql = @"DELETE FROM TB_CURSOS_PREREQ
                       WHERE CURSO_ID=@c AND PREREQ_CURSO_ID=@p";
            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@c", cursoId);
                cmd.Parameters.AddWithValue("@p", prereqId);
                cmd.ExecuteNonQuery();
            }
        }

  
        public int InsertarSeccion(int cursoId, string nombre, string periodo, int docenteId, int aulaId)
        {
            string sql = @"INSERT INTO TB_SECCIONES(CURSO_ID,NOMBRE,PERIODO,DOCENTE_ID,AULA_ID)
                       VALUES(@c,@n,@p,@d,@a);
                       SELECT SCOPE_IDENTITY();";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@c", cursoId);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@p", periodo);
                cmd.Parameters.AddWithValue("@d", docenteId);
                cmd.Parameters.AddWithValue("@a", aulaId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int ActualizarSeccion(int id, string nombre, string periodo, int docenteId, int aulaId)
        {
            string sql = @"UPDATE TB_SECCIONES
                       SET NOMBRE=@n, PERIODO=@p, DOCENTE_ID=@d, AULA_ID=@a
                       WHERE SECCION_ID=@id";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@p", periodo);
                cmd.Parameters.AddWithValue("@d", docenteId);
                cmd.Parameters.AddWithValue("@a", aulaId);

                return cmd.ExecuteNonQuery();
            }
        }

        public void InsertarHorario(int seccionId, int dia, TimeSpan inicio, TimeSpan fin)
        {
            string sql = @"INSERT INTO TB_HORARIOS(SECCION_ID,DIA_SEMANA,HORA_INICIO,HORA_FIN)
                       VALUES(@s,@d,@i,@f)";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@s", seccionId);
                cmd.Parameters.AddWithValue("@d", dia);
                cmd.Parameters.AddWithValue("@i", inicio);
                cmd.Parameters.AddWithValue("@f", fin);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarHorario(int horarioId)
        {
            string sql = @"DELETE FROM TB_HORARIOS WHERE HORARIO_ID=@id";

            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", horarioId);
                cmd.ExecuteNonQuery();
            }
        }
    }

}
