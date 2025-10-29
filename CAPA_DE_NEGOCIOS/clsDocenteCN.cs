using CAPA_DE_DATOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_DE_NEGOCIOS
{
    public class clsDocenteCN
    {
        private readonly clsDocenteCD cd = new clsDocenteCD();

        public DataTable Listar() => cd.ListarDocentes();

        // Ahora acepta el nombre de rol, por defecto "DOCENTE"
        public DataTable ListarUsuariosElegibles(int? incluirUsuarioId = null, string rolNombre = "DOCENTE")
            => cd.ListarUsuariosDisponiblesParaDocente(incluirUsuarioId, rolNombre);

        public int Guardar(int docenteId, int usuarioId, string titulo, bool estado)
        {
            if (usuarioId <= 0) throw new Exception("Seleccione un usuario válido.");
            if ((titulo ?? "").Length > 80) throw new Exception("El título no puede exceder 80 caracteres.");

            if (docenteId == 0)
                return cd.InsertarDocente(usuarioId, titulo, estado);

            cd.ActualizarDocente(docenteId, usuarioId, titulo, estado);
            return docenteId;
        }

        public void Eliminar(int docenteId)
        {
            if (docenteId <= 0) return;
            cd.EliminarDocente(docenteId);
        }
    }
}
