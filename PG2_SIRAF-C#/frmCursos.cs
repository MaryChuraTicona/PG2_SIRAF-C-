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
    public partial class frmCursos : Form
    {
        private readonly clsCursoCN _cn = new clsCursoCN();
        public frmCursos()
        {
            InitializeComponent();
        }

        private void frmCursos_Load(object sender, EventArgs e)
        {
            CargarTabla();
            ModoNuevo();
        }
        private void CargarTabla()
        {
            dgvCursos.AutoGenerateColumns = false;
            dgvCursos.DataSource = _cn.Listar();
        }

        private void ModoNuevo()
        {
            txtId.Text = "";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            nudCreditos.Value = 0;
            chkActivo.Checked = true;
            txtCodigo.Focus();
        }

        private void dgvCursos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = ((DataRowView)dgvCursos.Rows[e.RowIndex].DataBoundItem).Row;
            txtId.Text = row["CURSO_ID"].ToString();
            txtCodigo.Text = row["CODIGO"].ToString();
            txtNombre.Text = row["NOMBRE"].ToString();
            nudCreditos.Value = Convert.ToDecimal(row["CREDITOS"]);
            chkActivo.Checked = Convert.ToBoolean(row["ESTADO"]);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var codigo = txtCodigo.Text;
                var nombre = txtNombre.Text;
                var creditos = (int)nudCreditos.Value;
                var estado = chkActivo.Checked;

                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    _cn.Crear(codigo, nombre, creditos, estado);
                    MessageBox.Show("Curso creado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int id = int.Parse(txtId.Text);
                    _cn.Actualizar(id, codigo, nombre, creditos, estado);
                    MessageBox.Show("Curso actualizado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                CargarTabla();
                ModoNuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) return;
            if (MessageBox.Show("¿Eliminar curso?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _cn.Eliminar(int.Parse(txtId.Text));
                    CargarTabla();
                    ModoNuevo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
