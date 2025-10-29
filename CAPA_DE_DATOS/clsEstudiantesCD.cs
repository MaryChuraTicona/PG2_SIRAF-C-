using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsEstudianteCD
    {
        private readonly clsConexion _cn = new clsConexion();

        
        public DataTable Listar(string filtro)
        {
            using (SqlConnection con = _cn.mtdAbrirConexion())
            {
                string sql = @"
                    SELECT 
                        ALUMNO_ID,
                        CODIGO,
                        NOMBRES,
                        APELLIDOS,
                        DOCUMENTO,
                        CICLO,
                        TURNO_ID,
                        ESTADO
                    FROM TB_ESTUDIANTES
                    WHERE 
                        (CODIGO LIKE @filtro OR NOMBRES LIKE @filtro OR APELLIDOS LIKE @filtro)
                    ORDER BY ALUMNO_ID DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataRow ObtenerPorId(int alumnoId)
        {
            using (SqlConnection con = _cn.mtdAbrirConexion())
            {
                string sql = @"SELECT * FROM TB_ESTUDIANTES WHERE ALUMNO_ID = @id";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@id", alumnoId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
        }

      
        public DataTable ListarTurnos()
        {
            using (SqlConnection con = _cn.mtdAbrirConexion())
            {
                string sql = "SELECT TURNO_ID, TURNO_NOMBRE FROM TB_TURNOS ORDER BY TURNO_ID";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public byte[] ObtenerFoto(int alumnoId)
        {
            using (SqlConnection con = _cn.mtdAbrirConexion())
            {
                string sql = "SELECT FOTO FROM TB_ESTUDIANTES WHERE ALUMNO_ID = @id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", alumnoId);

                object result = cmd.ExecuteScalar();
                return result == DBNull.Value || result == null ? null : (byte[])result;
            }
        }

        public int Insertar(string codigo, string nombres, string apellidos, string documento,
                            string ciclo, int turnoId, bool estado, byte[] foto)
        {
            using (SqlConnection con = _cn.mtdAbrirConexion())
            {
                string sql = @"
                    INSERT INTO TB_ESTUDIANTES
                    (CODIGO, NOMBRES, APELLIDOS, DOCUMENTO, CICLO, TURNO_ID, ESTADO, FOTO)
                    VALUES
                    (@codigo, @nombres, @apellidos, @documento, @ciclo, @turnoId, @estado, @foto);

                    SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@nombres", nombres);
                cmd.Parameters.AddWithValue("@apellidos", apellidos);
                cmd.Parameters.AddWithValue("@documento", (object)documento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ciclo", ciclo);
                cmd.Parameters.AddWithValue("@turnoId", turnoId);
                cmd.Parameters.AddWithValue("@estado", estado);

                if (foto == null)
                    cmd.Parameters.AddWithValue("@foto", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@foto", foto);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public bool Actualizar(int alumnoId, string codigo, string nombres, string apellidos, string documento,
                               string ciclo, int turnoId, bool estado, byte[] foto)
        {
            using (SqlConnection con = _cn.mtdAbrirConexion())
            {
                string sql = @"
                    UPDATE TB_ESTUDIANTES SET
                        CODIGO     = @codigo,
                        NOMBRES    = @nombres,
                        APELLIDOS  = @apellidos,
                        DOCUMENTO  = @documento,
                        CICLO      = @ciclo,
                        TURNO_ID   = @turnoId,
                        ESTADO     = @estado,
                        FOTO       = @foto
                    WHERE ALUMNO_ID = @id";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", alumnoId);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@nombres", nombres);
                cmd.Parameters.AddWithValue("@apellidos", apellidos);
                cmd.Parameters.AddWithValue("@documento", (object)documento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ciclo", ciclo);
                cmd.Parameters.AddWithValue("@turnoId", turnoId);
                cmd.Parameters.AddWithValue("@estado", estado);

                if (foto == null)
                    cmd.Parameters.AddWithValue("@foto", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@foto", foto);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Desactivar(int alumnoId)
        {
            using (SqlConnection con = _cn.mtdAbrirConexion())
            {
                string sql = @"UPDATE TB_ESTUDIANTES SET ESTADO = 0 WHERE ALUMNO_ID = @id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", alumnoId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}

