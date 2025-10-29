using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsDocenteCD
    {
        private readonly clsConexion conexion = new clsConexion();

        public DataTable ListarDocentes()
        {
            const string SQL = @"
SELECT d.DOCENTE_ID, u.USUARIO_ID, u.NOMBRE, u.CORREO, d.TITULO, d.ESTADO
FROM TB_DOCENTES d
JOIN TB_USUARIOS u ON u.USUARIO_ID = d.USUARIO_ID
ORDER BY u.NOMBRE;";

            using (var cn = conexion.mtdAbrirConexion())
            {
        
                if (string.IsNullOrWhiteSpace(cn.ConnectionString))
                {
                    cn.ConnectionString = "Data Source=TU_SERVIDOR;Initial Catalog=SIRAF;User ID=sa;Password=1234;TrustServerCertificate=True;";
                }

                using (var cmd = new SqlCommand(SQL, cn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable ListarUsuariosDisponiblesParaDocente(int? incluirUsuarioId = null, string rolNombre = "DOCENTE")
        {
            const string ENSURE_ROL_SQL = @"
IF NOT EXISTS (SELECT 1 FROM TB_ROL WHERE ROL_NOMBRE = @rol)
    INSERT INTO TB_ROL(ROL_NOMBRE) VALUES (@rol);";

            const string SQL = @"
SELECT u.USUARIO_ID, u.NOMBRE, u.CORREO
FROM TB_USUARIOS u
JOIN TB_ROL r ON r.ROL_ID = u.ROL_ID
LEFT JOIN TB_DOCENTES d ON d.USUARIO_ID = u.USUARIO_ID
WHERE r.ROL_NOMBRE = @rol
  AND u.ESTADO = 1
  AND (d.DOCENTE_ID IS NULL OR u.USUARIO_ID = @incluir)
ORDER BY u.NOMBRE;";

            var cn = conexion.mtdAbrirConexion();
        
            if (string.IsNullOrWhiteSpace(cn.ConnectionString))
            {
                cn.ConnectionString = "Data Source=TU_SERVIDOR;Initial Catalog=SIRAF;User ID=sa;Password=1234;TrustServerCertificate=True;";
            }

            using (var cmdEnsure = new SqlCommand(ENSURE_ROL_SQL, cn))
            {
                cmdEnsure.Parameters.Add("@rol", SqlDbType.NVarChar, 30).Value = rolNombre;
                cmdEnsure.ExecuteNonQuery();
            }

          
            using (var cmd = new SqlCommand(SQL, cn))
            {
                cmd.Parameters.Add("@rol", SqlDbType.NVarChar, 30).Value = rolNombre;
                var p = cmd.Parameters.Add("@incluir", SqlDbType.Int);
                p.Value = (object)incluirUsuarioId ?? DBNull.Value;

                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public int InsertarDocente(int usuarioId, string titulo, bool estado)
        {
            const string SQL = @"
INSERT INTO TB_DOCENTES(USUARIO_ID, TITULO, ESTADO)
VALUES(@u, @t, @e);
SELECT SCOPE_IDENTITY();";

            using (var cn = conexion.mtdAbrirConexion())
            {
                if (string.IsNullOrWhiteSpace(cn.ConnectionString))
                {
                    cn.ConnectionString = "Data Source=TU_SERVIDOR;Initial Catalog=SIRAF;User ID=sa;Password=1234;TrustServerCertificate=True;";
                }

                using (var cmd = new SqlCommand(SQL, cn))
                {
                    cmd.Parameters.Add("@u", SqlDbType.Int).Value = usuarioId;
                    cmd.Parameters.Add("@t", SqlDbType.NVarChar, 80).Value = (object)(titulo ?? "").Trim();
                    cmd.Parameters.Add("@e", SqlDbType.Bit).Value = estado;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int ActualizarDocente(int docenteId, int usuarioId, string titulo, bool estado)
        {
            const string SQL = @"
UPDATE TB_DOCENTES
SET USUARIO_ID = @u, TITULO = @t, ESTADO = @e
WHERE DOCENTE_ID = @id;";

            using (var cn = conexion.mtdAbrirConexion())
            {
                if (string.IsNullOrWhiteSpace(cn.ConnectionString))
                {
                    cn.ConnectionString = "Data Source=TU_SERVIDOR;Initial Catalog=SIRAF;User ID=sa;Password=1234;TrustServerCertificate=True;";
                }

                using (var cmd = new SqlCommand(SQL, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = docenteId;
                    cmd.Parameters.Add("@u", SqlDbType.Int).Value = usuarioId;
                    cmd.Parameters.Add("@t", SqlDbType.NVarChar, 80).Value = (object)(titulo ?? "").Trim();
                    cmd.Parameters.Add("@e", SqlDbType.Bit).Value = estado;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int EliminarDocente(int docenteId)
        {
            const string SQL = "DELETE FROM TB_DOCENTES WHERE DOCENTE_ID = @id;";
            using (var cn = conexion.mtdAbrirConexion())
            {
                if (string.IsNullOrWhiteSpace(cn.ConnectionString))
                {
                    cn.ConnectionString = "Data Source=TU_SERVIDOR;Initial Catalog=SIRAF;User ID=sa;Password=1234;TrustServerCertificate=True;";
                }

                using (var cmd = new SqlCommand(SQL, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = docenteId;
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
