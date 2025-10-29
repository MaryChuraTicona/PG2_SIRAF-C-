
using CAPA_DE_NEGOCIOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG2_SIRAF_C_
{
    public partial class frmPrincipal : Form
    {
        private readonly clsResultadoLogin _sesion;

        public frmPrincipal(clsResultadoLogin sesion)
        {
            InitializeComponent();
            _sesion = sesion;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = _sesion.Nombre + " (" + _sesion.Correo + ")";
            lblRol.Text = "Rol: " + _sesion.Rol;

            // desactiva todo
            btnGestionUsuarios.Enabled = false;
            btnGestionEstudiantes.Enabled = false;
            btnCursos.Enabled = false;           // agrega botón en el panel
            btnSecciones.Enabled = false;
            btnHorarios.Enabled = false;
            btnAulas.Enabled = false;
            btnDocentes.Enabled = false;
            btnCamaras.Enabled = false;
            btnAsistencia.Enabled = false;       // consultas
            btnReportes.Enabled = false;

            Habilitar(Permisos.GEST_USUARIOS_DOCENTES, btnGestionUsuarios);
            Habilitar(Permisos.GEST_ESTUDIANTES, btnGestionEstudiantes);
            Habilitar(Permisos.GEST_CURSOS, btnCursos);
            Habilitar(Permisos.GEST_SECCIONES, btnSecciones);
            Habilitar(Permisos.GEST_HORARIOS, btnHorarios);
            Habilitar(Permisos.GEST_AULAS, btnAulas);
            Habilitar(Permisos.GEST_MATRICULAS, btnDocentes);
            Habilitar(Permisos.GEST_CAMARAS, btnCamaras);
            Habilitar(Permisos.CONSULTAR_ASISTENCIAS, btnAsistencia);
            Habilitar(Permisos.REPORTES, btnReportes);

            MostrarMensaje("Bienvenida/o " + _sesion.Nombre + "\nSelecciona una opción del menú.");
        }


        private void Habilitar(string permiso, Control ctrl)
        {
            if (_sesion.Permisos.Any(p => p.Equals(permiso, StringComparison.OrdinalIgnoreCase)))
                ctrl.Enabled = true;
        }
        private void MostrarMensaje(string texto)
        {
            panelContenido.Controls.Clear();
            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Text = texto
            };
            panelContenido.Controls.Add(lbl);
        }

        private void CargarFormularioEnPanel(Form frm)
        {
            panelContenido.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(frm);
            frm.Show();
        }

        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            MostrarMensaje("Módulo: Gestión de Usuarios (en construcción)");
        }

        private void btnCursos_Click(object sender, EventArgs e)
        {
            CargarFormularioEnPanel(new frmCursos());
        }

        private void btnDocentes_Click(object sender, EventArgs e)
        {
            CargarFormularioEnPanel(new frmDocente());
        }

        private void btnGestionEstudiantes_Click(object sender, EventArgs e)
        {
            CargarFormularioEnPanel(new frmEstudiantes());
        }
    }
    
}
