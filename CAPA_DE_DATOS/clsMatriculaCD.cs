using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsMatriculaCD
    {
        private readonly clsConexion conexion = new clsConexion();

        public DataTable ListarPeriodos()
        {
            const string SQL = @"SELECT DISTINCT PERIODO FROM TB_SECCIONES ORDER BY PERIODO DESC;";
            using (var cn = conexion.mtdAbrirConexion())
            using (var da = new SqlDataAdapter(SQL, cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarCursosActivos()
        {
            const string SQL = @"SELECT CURSO_ID, CODIGO, NOMBRE FROM TB_CURSOS WHERE ESTADO=1 ORDER BY NOMBRE;";
            using (var cn = conexion.mtdAbrirConexion())
            using (var da = new SqlDataAdapter(SQL, cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarSecciones(int cursoId, string periodo)
        {
            const string SQL = @"
SELECT  s.SECCION_ID,
        s.NOMBRE AS SECCION,
        s.PERIODO,
        a.CODIGO AS AULA,
        d.DOCENTE_ID,
        u.NOMBRE AS DOCENTE
FROM TB_SECCIONES s
JOIN TB_AULAS a     ON a.AULA_ID = s.AULA_ID
JOIN TB_DOCENTES d  ON d.DOCENTE_ID = s.DOCENTE_ID
JOIN TB_USUARIOS u  ON u.USUARIO_ID = d.USUARIO_ID
WHERE s.ESTADO = 1
  AND s.CURSO_ID = @curso
  AND (@per IS NULL OR @per='' OR s.PERIODO = @per)
ORDER BY s.NOMBRE;";
            using (var cn = conexion.mtdAbrirConexion())
            using (var da = new SqlDataAdapter(SQL, cn))
            {
                da.SelectCommand.Parameters.AddWithValue("@curso", cursoId);
                da.SelectCommand.Parameters.AddWithValue("@per", (object)periodo ?? DBNull.Value);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarMatriculasPorAlumno(int alumnoId)
        {
            const string SQL = @"
SELECT  m.MATRICULA_ID,
        s.PERIODO,
        c.CODIGO AS COD_CURSO,
        c.NOMBRE AS CURSO,
        s.NOMBRE AS SECCION,
        u.NOMBRE AS DOCENTE,
        a.CODIGO AS AULA
FROM TB_MATRICULAS m
JOIN TB_SECCIONES s ON s.SECCION_ID = m.SECCION_ID
JOIN TB_CURSOS c    ON c.CURSO_ID   = s.CURSO_ID
JOIN TB_DOCENTES d  ON d.DOCENTE_ID = s.DOCENTE_ID
JOIN TB_USUARIOS u  ON u.USUARIO_ID = d.USUARIO_ID
JOIN TB_AULAS a     ON a.AULA_ID    = s.AULA_ID
WHERE m.ALUMNO_ID = @alumno
ORDER BY s.PERIODO DESC, c.NOMBRE, s.NOMBRE;";
            using (var cn = conexion.mtdAbrirConexion())
            using (var da = new SqlDataAdapter(SQL, cn))
            {
                da.SelectCommand.Parameters.AddWithValue("@alumno", alumnoId);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public int InsertarMatricula(int alumnoId, int seccionId)
        {
            const string SQL = @"
INSERT INTO TB_MATRICULAS (ALUMNO_ID, SECCION_ID)
VALUES (@alumno, @seccion);
SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@alumno", alumnoId);
                cmd.Parameters.AddWithValue("@seccion", seccionId);
                try
                {
                    return (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    // UQ_MATRICULAS (ALUMNO_ID, SECCION_ID)
                    if (ex.Number == 2627 || ex.Number == 2601)
                        throw new Exception("Este alumno ya está matriculado en esa sección.", ex);
                    // FK o cualquier otra
                    throw;
                }
            }
        }

        public bool EliminarMatricula(int matriculaId)
        {
            const string SQL = "DELETE FROM TB_MATRICULAS WHERE MATRICULA_ID = @id;";
            using (var cn = conexion.mtdAbrirConexion())
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.AddWithValue("@id", matriculaId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
