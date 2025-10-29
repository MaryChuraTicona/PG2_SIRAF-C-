using CAPA_DE_DATOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_DE_NEGOCIOS
{
    public class clsUsuarioCN
    {
        private readonly clsUsuarioCD usuarioCD = new clsUsuarioCD();

        public clsResultadoLogin AutenticarUsuario(string usuarioCorreo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(usuarioCorreo) || string.IsNullOrWhiteSpace(contrasena))
            {
                return new clsResultadoLogin
                {
                    EsValido = false,
                    Mensaje = "Debe ingresar el usuario y la contraseña."
                };
            }

            var dto = usuarioCD.mtdObtenerUsuarioPorCredenciales(usuarioCorreo.Trim(), contrasena);
            if (dto == null)
            {
                return new clsResultadoLogin
                {
                    EsValido = false,
                    Mensaje = "Usuario o contraseña incorrectos."
                };
            }

            var res = new clsResultadoLogin
            {
                EsValido = true,
                Mensaje = "Bienvenido " + dto.Nombre,
                Rol = dto.Rol,
                UsuarioId = dto.UsuarioId,
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Permisos = PermisosPorRol(dto.Rol)
            };

            return res;
        }

        private static List<string> PermisosPorRol(string rol)
        {
            var R = (rol ?? "").Trim().ToUpperInvariant();
            var p = new List<string>();
            switch (R)
            {
                case "ADMIN":
                    p.AddRange(new[]{
                Permisos.GEST_USUARIOS_DOCENTES, Permisos.GEST_ESTUDIANTES, Permisos.GEST_CURSOS,
                Permisos.GEST_SECCIONES, Permisos.GEST_HORARIOS, Permisos.GEST_AULAS,
                Permisos.GEST_MATRICULAS, Permisos.GEST_CAMARAS, Permisos.INICIAR_SESION_CLASE,
                Permisos.CONSULTAR_ASISTENCIAS, Permisos.REPORTES
            });
                    break;

                case "SECRETARIA":
                    p.AddRange(new[]{
                Permisos.GEST_USUARIOS_DOCENTES, Permisos.GEST_ESTUDIANTES, Permisos.GEST_CURSOS,
                Permisos.GEST_SECCIONES, Permisos.GEST_HORARIOS, Permisos.GEST_AULAS,
                Permisos.GEST_MATRICULAS, Permisos.CONSULTAR_ASISTENCIAS, Permisos.REPORTES
            });
                    break;

                case "DOCENTE":
                    p.AddRange(new[]{
                Permisos.CONSULTAR_ASISTENCIAS
            });
                    break;
            }
            return p;
        }

    }
}