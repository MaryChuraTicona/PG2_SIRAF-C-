using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CAPA_DE_DATOS
{
    public class clsUsuarioCD
    {
            private readonly clsConexion conexion = new clsConexion();

            // Valida contra TB_USUARIOS.CLAVE_HASH (SHA2_256) + ESTADO=1
            public clsUsuarioRegistro mtdObtenerUsuarioPorCredenciales(string correo, string contrasena)
            {
                if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                    return null;

                const string SQL = @"
SELECT TOP 1
    u.USUARIO_ID,
    u.NOMBRE,
    u.CORREO,
    r.ROL_NOMBRE
FROM TB_USUARIOS u
JOIN TB_ROL r ON r.ROL_ID = u.ROL_ID
WHERE u.CORREO = @correo
  AND u.ESTADO = 1
  AND u.CLAVE_HASH = HASHBYTES('SHA2_256', @pwd);";

                SqlConnection cn = null;
                try
                {
                    cn = conexion.mtdAbrirConexion();
                    using (var cmd = new SqlCommand(SQL, cn))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@pwd", contrasena);

                        using (var rd = cmd.ExecuteReader())
                        {
                            if (!rd.Read()) return null;

                            return new clsUsuarioRegistro
                            {
                                UsuarioId = rd.GetInt32(0),
                                Nombre = rd.GetString(1),
                                Correo = rd.GetString(2),
                                Rol = rd.GetString(3)
                            };
                        }
                    }
                }
                finally
                {
                    if (cn != null) conexion.mtdCerrarConexion();
                }
            }
        }
    }
