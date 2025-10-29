using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CAPA_DE_NEGOCIOS;
using CAPA_DE_DATOS;
using System.Data.SqlClient;

namespace PG2_SIRAF_C_
{
    public partial class frmDocente : Form
    {
        private readonly clsDocenteCN _cn = new clsDocenteCN();
        private DataTable _dtDocentes;
        private bool _cargandoCombo = false;
        public frmDocente()
        {
            InitializeComponent();
            this.Load += frmDocente_Load;

            if (btnNuevoDoc != null) btnNuevoDoc.Click += btnNuevoDoc_Click;
            if (btnGuardarDoc != null) btnGuardarDoc.Click += btnGuardarDoc_Click;
            if (btnEliminarDoc != null) btnEliminarDoc.Click += btnEliminarDoc_Click;
            if (btnRefrescarUsuarios != null) btnRefrescarUsuarios.Click += btnRefrescarUsuarios_Click;

            if (dgvDocentes != null) dgvDocentes.CellClick += dgvDocentes_CellClick;

            // OJO: usamos cmbRol como "combo de usuarios" (mismo nombre del diseñador)
            if (cmbRol != null) cmbRol.SelectedIndexChanged += cmbUsuario_SelectedIndexChanged;

            if (txtBuscarDoc != null) txtBuscarDoc.TextChanged += txtBuscarDoc_TextChanged;
        }

        private void frmDocente_Load(object sender, EventArgs e)
        {

            ConfigurarGrid();
            CargarDocentes();
            CargarUsuariosElegibles(null);
            ModoNuevo();
        }
        private void ConfigurarGrid()
        {
            dgvDocentes.AutoGenerateColumns = false;
            dgvDocentes.AllowUserToAddRows = false;
            dgvDocentes.ReadOnly = true;
            dgvDocentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocentes.RowHeadersVisible = false;

            if (dgvDocentes.Columns.Count > 0) return;

            dgvDocentes.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    Name = "colDocenteId",
                    HeaderText = "ID",
                    DataPropertyName = "DOCENTE_ID",
                    Width = 60
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colUsuarioId",
                    HeaderText = "USUARIO_ID",
                    DataPropertyName = "USUARIO_ID",
                    Visible = false
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colNombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "NOMBRE",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colCorreo",
                    HeaderText = "Correo",
                    DataPropertyName = "CORREO",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colTitulo",
                    HeaderText = "Título",
                    DataPropertyName = "TITULO",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                },
                new DataGridViewCheckBoxColumn
                {
                    Name = "colEstado",
                    HeaderText = "Activo",
                    DataPropertyName = "ESTADO",
                    Width = 70
                }
            );
        }
        private void CargarDocentes()
        {

            try
            {
                _dtDocentes = _cn.Listar();
                dgvDocentes.DataSource = null;
                dgvDocentes.DataSource = _dtDocentes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar docentes: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarUsuariosElegibles(int? incluirUsuarioId)
        {
            try
            {
                _cargandoCombo = true;

                // 👉 ahora pasamos el rol explícito
                var dt = _cn.ListarUsuariosElegibles(incluirUsuarioId, "DOCENTE");

                cmbRol.DisplayMember = "NOMBRE";   // mostramos usuarios
                cmbRol.ValueMember = "USUARIO_ID";
                cmbRol.DataSource = dt;

                _cargandoCombo = false;
                ActualizarDatosUsuarioDesdeCombo();
            }
            catch (Exception ex)
            {
                _cargandoCombo = false;
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarDatosUsuarioDesdeCombo()
        {
            if (_cargandoCombo) return;

            var drv = cmbRol.SelectedItem as DataRowView;
            if (drv == null || drv.Row == null)
            {
                txtNombreUsuario.Text = "";
                txtCorreoUsuario.Text = "";
                return;
            }

            var table = drv.Row.Table;
            string nombreCol = table.Columns.Contains("NOMBRE") ? "NOMBRE" : null;
            string correoCol = table.Columns.Contains("CORREO") ? "CORREO" : null;

            txtNombreUsuario.Text = nombreCol != null ? Convert.ToString(drv[nombreCol]) : "";
            txtCorreoUsuario.Text = correoCol != null ? Convert.ToString(drv[correoCol]) : "";
        }
        private void cmbUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarDatosUsuarioDesdeCombo();
        }
        private void ModoNuevo()
        {
            txtDocenteId.Text = "";
            txtTitulo.Text = "";
            chkEstado.Checked = true;
            CargarUsuariosElegibles(null);
            btnEliminarDoc.Enabled = false;
            cmbRol.Focus();
        }

        private void dgvDocentes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvDocentes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = (dgvDocentes.Rows[e.RowIndex].DataBoundItem as DataRowView)?.Row;
            if (row == null) return;

            txtDocenteId.Text = Convert.ToString(row["DOCENTE_ID"]);
            var usuarioId = Convert.ToInt32(row["USUARIO_ID"]);
            txtTitulo.Text = Convert.ToString(row["TITULO"] ?? "");
            chkEstado.Checked = row["ESTADO"] != DBNull.Value && Convert.ToBoolean(row["ESTADO"]);

            // Incluir al usuario actual en el combo aunque ya esté en TB_DOCENTES
            CargarUsuariosElegibles(usuarioId);

            // Seleccionar el usuario del docente
            for (int i = 0; i < cmbRol.Items.Count; i++)
            {
                var rv = cmbRol.Items[i] as DataRowView;
                if (rv != null && Convert.ToInt32(rv["USUARIO_ID"]) == usuarioId)
                {
                    cmbRol.SelectedIndex = i;
                    break;
                }
            }

            btnEliminarDoc.Enabled = true;
        }

        private void btnNuevoDoc_Click(object sender, EventArgs e)
        {
            ModoNuevo();
        }

        private void btnGuardarDoc_Click(object sender, EventArgs e)
        {
            try
            {
                int docenteId = 0;
                if (!string.IsNullOrWhiteSpace(txtDocenteId.Text))
                    int.TryParse(txtDocenteId.Text, out docenteId);

                if (cmbRol.SelectedValue == null)
                    throw new Exception("Seleccione un usuario.");

                int usuarioId = Convert.ToInt32(cmbRol.SelectedValue);
                string titulo = (txtTitulo.Text ?? "").Trim();
                bool estado = chkEstado.Checked;

                _cn.Guardar(docenteId, usuarioId, titulo, estado);

                MessageBox.Show(docenteId == 0 ? "Docente registrado." : "Docente actualizado.",
                    "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDocentes();
                if (docenteId == 0) ModoNuevo();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                    MessageBox.Show("Ese usuario ya está registrado como Docente.", "Duplicado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (sqlEx.Number == 547)
                    MessageBox.Show("No se puede guardar por restricción referencial.", "Restricción",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Error SQL: " + sqlEx.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminarDoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDocenteId.Text)) return;

            if (MessageBox.Show("¿Eliminar el docente seleccionado?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _cn.Eliminar(Convert.ToInt32(txtDocenteId.Text));
                CargarDocentes();
                ModoNuevo();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547)
                    MessageBox.Show("No se puede eliminar: el docente tiene secciones/relaciones.",
                        "Restricción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Error SQL: " + sqlEx.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefrescarUsuarios_Click(object sender, EventArgs e)
        {
            int? incluir = null;
            if (!string.IsNullOrWhiteSpace(txtDocenteId.Text))
            {
                var drvSel = cmbRol.SelectedItem as DataRowView;
                if (drvSel != null) incluir = Convert.ToInt32(drvSel["USUARIO_ID"]);
            }
            CargarUsuariosElegibles(incluir);
        }

        private void txtBuscarDoc_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBuscarDoc.Text) && _dtDocentes != null)
            {
                var txt = txtBuscarDoc.Text.Replace("'", "''");
                _dtDocentes.DefaultView.RowFilter =
                    $"NOMBRE LIKE '%{txt}%' OR CORREO LIKE '%{txt}%'";
            }
            else if (_dtDocentes != null)
            {
                _dtDocentes.DefaultView.RowFilter = "";
            }
        }


      
    }
}
