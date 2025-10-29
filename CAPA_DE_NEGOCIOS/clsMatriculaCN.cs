using CAPA_DE_DATOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_DE_NEGOCIOS
{
    public class clsMatriculaCN
    {
            private readonly clsMatriculaCD _cd = new clsMatriculaCD();

            public DataTable ListarPeriodos() => _cd.ListarPeriodos();

            public DataTable ListarCursosActivos() => _cd.ListarCursosActivos();

            public DataTable ListarSecciones(int cursoId, string periodo)
            {
                if (cursoId <= 0) throw new Exception("Selecciona un curso válido.");
                return _cd.ListarSecciones(cursoId, periodo);
            }

            public DataTable ListarMatriculasPorAlumno(int alumnoId)
            {
                if (alumnoId <= 0) throw new Exception("Selecciona un alumno válido.");
                return _cd.ListarMatriculasPorAlumno(alumnoId);
            }

            public int InsertarMatricula(int alumnoId, int seccionId)
            {
                if (alumnoId <= 0) throw new Exception("Selecciona un alumno válido.");
                if (seccionId <= 0) throw new Exception("Selecciona una sección válida.");
                return _cd.InsertarMatricula(alumnoId, seccionId);
            }

            public bool EliminarMatricula(int matriculaId)
            {
                if (matriculaId <= 0) throw new Exception("Selecciona una matrícula válida.");
                return _cd.EliminarMatricula(matriculaId);
            }

            
        }
    }
