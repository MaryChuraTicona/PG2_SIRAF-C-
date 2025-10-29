using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPA_DE_DATOS;

namespace CAPA_DE_NEGOCIOS
{
    public class clsEstudianteCN
    {
        private readonly clsEstudianteCD _cd = new clsEstudianteCD();


        public DataTable Listar(string filtro)
        {
            // Devuelve el listado para el dgvEstudiantes (coincide con el form)
            return _cd.Listar(filtro);
        }

        public DataRow ObtenerPorId(int alumnoId)
        {
            if (alumnoId <= 0) throw new Exception("ALUMNO_ID inválido.");
            return _cd.ObtenerPorId(alumnoId);
        }

        public DataTable ListarTurnos()
        {
            return _cd.ListarTurnos();
        }

        public byte[] ObtenerFoto(int alumnoId)
        {
            if (alumnoId <= 0) throw new Exception("ALUMNO_ID inválido.");
            return _cd.ObtenerFoto(alumnoId);
        }

        public int Insertar(string codigo, string nombres, string apellidos, string documento,
                            string ciclo, int turnoId, bool estado, byte[] foto)
        {
            Validar(codigo, nombres, apellidos, ciclo, turnoId);
            return _cd.Insertar(
                codigo?.Trim(),
                nombres?.Trim(),
                apellidos?.Trim(),
                string.IsNullOrWhiteSpace(documento) ? null : documento.Trim(),
                ciclo?.Trim(),
                turnoId,
                estado,
                foto
            );
        }

        public bool Actualizar(int alumnoId, string codigo, string nombres, string apellidos, string documento,
                               string ciclo, int turnoId, bool estado, byte[] foto)
        {
            if (alumnoId <= 0) throw new Exception("ALUMNO_ID inválido.");
            Validar(codigo, nombres, apellidos, ciclo, turnoId);
            return _cd.Actualizar(
                alumnoId,
                codigo?.Trim(),
                nombres?.Trim(),
                apellidos?.Trim(),
                string.IsNullOrWhiteSpace(documento) ? null : documento.Trim(),
                ciclo?.Trim(),
                turnoId,
                estado,
                foto
            );
        }

        public bool Desactivar(int alumnoId)
        {
            if (alumnoId <= 0) throw new Exception("ALUMNO_ID inválido.");
            return _cd.Desactivar(alumnoId);
        }

        private void Validar(string codigo, string nombres, string apellidos, string ciclo, int turnoId)
        {
            if (string.IsNullOrWhiteSpace(codigo)) throw new Exception("Código requerido.");
            if (string.IsNullOrWhiteSpace(nombres)) throw new Exception("Nombres requeridos.");
            if (string.IsNullOrWhiteSpace(apellidos)) throw new Exception("Apellidos requeridos.");
            if (string.IsNullOrWhiteSpace(ciclo)) throw new Exception("Ciclo requerido.");
            if (turnoId <= 0) throw new Exception("Selecciona un turno válido.");
            if (codigo.Length > 20) throw new Exception("Código: máximo 20 caracteres.");
            if (ciclo.Length > 20) throw new Exception("Ciclo: máximo 20 caracteres.");
        }
    }
}

