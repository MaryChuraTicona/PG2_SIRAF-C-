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

        public DataTable Listar() => cd.Listar();

        public void Crear(string codigo, string nombre, int creditos, bool estado)
        {
            ValidarDatos(codigo, nombre, creditos);

            // Código único
            if (cd.ExisteCodigo(codigo))
                throw new Exception("El código ya existe. Ingrese otro.");

            cd.Insertar(codigo.Trim(), nombre.Trim(), creditos, estado);
        }

        public void Actualizar(int id, string codigo, string nombre, int creditos, bool estado)
        {
            if (id <= 0) throw new Exception("Id inválido");
            ValidarDatos(codigo, nombre, creditos);

            // Código único excluyendo el mismo Id
            if (cd.ExisteCodigo(codigo, id))
                throw new Exception("El código ya existe en otro curso.");

            var filas = cd.Actualizar(id, codigo.Trim(), nombre.Trim(), creditos, estado);
            if (filas == 0) throw new Exception("No se actualizó el registro (puede que ya no exista).");
        }

        public void Eliminar(int id)
        {
            if (id <= 0) throw new Exception("Seleccione un curso.");
            var filas = cd.Eliminar(id);
            if (filas == 0) throw new Exception("No se eliminó el registro (puede que ya no exista).");
        }

        private static void ValidarDatos(string codigo, string nombre, int creditos)
        {
            if (string.IsNullOrWhiteSpace(codigo)) throw new Exception("Ingrese el código.");
            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese el nombre.");
            if (creditos < 0) throw new Exception("Los créditos no pueden ser negativos.");
        }
    }
}
