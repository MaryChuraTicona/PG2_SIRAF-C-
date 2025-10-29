using CAPA_DE_DATOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_DE_NEGOCIOS
{
    public class clsCursoCN
    {
        private readonly clsCursoCD cd = new clsCursoCD();

        public DataTable Listar() => cd.ListarCursos();

        public int Guardar(int cursoId, string codigo, string nombre, int creditos, bool estado)
        {
            if (cursoId == 0)
                return cd.InsertarCurso(codigo, nombre, creditos, estado);
            else
                return cd.ActualizarCurso(cursoId, codigo, nombre, creditos, estado);
        }

        public DataTable ListarPrereq(int cursoId) => cd.ListarPrerequisitos(cursoId);

        public void AgregarPrereq(int cursoId, int preId) => cd.InsertarPrerequisito(cursoId, preId);

        public void QuitarPrereq(int cursoId, int preId) => cd.EliminarPrerequisito(cursoId, preId);

        public int GuardarSeccion(int id, int cursoId, string nombre, string periodo, int docenteId, int aulaId)
        {
            if (id == 0)
                return cd.InsertarSeccion(cursoId, nombre, periodo, docenteId, aulaId);
            else
                return cd.ActualizarSeccion(id, nombre, periodo, docenteId, aulaId);
        }

        public void GuardarHorario(int seccionId, int dia, TimeSpan inicio, TimeSpan fin)
            => cd.InsertarHorario(seccionId, dia, inicio, fin);

        public void EliminarHorario(int horarioId)
            => cd.EliminarHorario(horarioId);

        public void Eliminar(int id)
        {
            
            cd.EliminarCurso(id);
        }

    }

}
