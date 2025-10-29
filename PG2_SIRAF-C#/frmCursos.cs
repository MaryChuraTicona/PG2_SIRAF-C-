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
using System.Data.SqlClient;
using CAPA_DE_DATOS;

namespace PG2_SIRAF_C_
{
    public partial class frmCursos : Form
    {
        private readonly clsCursoCN _cn = new clsCursoCN();
       
        private DataTable _dtPrereq;


        public frmCursos()
        {
            InitializeComponent();
            this.Load += frmCursos_Load;
            dgvCursos.CellClick += dgvCursos_CellClick;
            if (btnGuardar != null) btnGuardar.Click += btnGuardar_Click;
            if (btnEliminar != null) btnEliminar.Click += btnEliminar_Click;
            if (btnNuevo != null) btnNuevo.Click += btnNuevo_Click;
            if (btnGuardarPrereq != null) btnGuardarPrereq.Click += btnGuardarPrereq_Click;
            if (txtBuscar != null) txtBuscar.TextChanged += txtBuscar_TextChanged;
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && btnGuardar?.Enabled == true)
                {
                    e.SuppressKeyPress = true;
                    btnGuardar.PerformClick();
                }
            };
        }

        private void frmCursos_Load(object sender, EventArgs e)
        {
            try
            {
                var cx = new clsConexion();
                using (var cn = cx.mtdAbrirConexion()) { /* ok */ }
                cx.mtdCerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Conn FAIL: " + ex.Message, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            ConfigurarGridCursos();
            ConfigurarGridPrereq();

            CargarCursos();


            ModoNuevo();
        }

        private void ConfigurarGridCursos()
        {
            if (dgvCursos.Columns.Count > 0) return;

            dgvCursos.AutoGenerateColumns = false;
            dgvCursos.AllowUserToAddRows = false;
            dgvCursos.MultiSelect = false;
            dgvCursos.ReadOnly = true;
            dgvCursos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCursos.RowHeadersVisible = false;

            dgvCursos.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    Name = "colId",
                    HeaderText = "ID",
                    DataPropertyName = "CURSO_ID",
                    Width = 70
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colCodigo",
                    HeaderText = "Código",
                    DataPropertyName = "CODIGO",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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
                    Name = "colCreditos",
                    HeaderText = "Créditos",
                    DataPropertyName = "CREDITOS",
                    Width = 90
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

        private void ConfigurarGridPrereq()
        {
            if (dgvPrereq.Columns.Count > 0) return;

            dgvPrereq.AutoGenerateColumns = false;
            dgvPrereq.AllowUserToAddRows = false;
            dgvPrereq.MultiSelect = false;
            dgvPrereq.ReadOnly = false; // Edita el check
            dgvPrereq.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPrereq.RowHeadersVisible = false;

            dgvPrereq.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    Name = "colPreId",
                    HeaderText = "ID",
                    DataPropertyName = "CURSO_ID",
                    Visible = false
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colPreNombre",
                    HeaderText = "Curso",
                    DataPropertyName = "NOMBRE",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    ReadOnly = true
                },
                new DataGridViewCheckBoxColumn
                {
                    Name = "colTiene",
                    HeaderText = "Es prerrequisito",
                    DataPropertyName = "TIENE",
                    Width = 140
                }
            );
        }

       
        private void CargarCursos()
        {
            try
            {
                var dt = _cn.Listar(); // SELECT * FROM TB_CURSOS ORDER BY NOMBRE
                dgvCursos.DataSource = null;
                dgvCursos.AutoGenerateColumns = false;
                dgvCursos.DataSource = dt;
                if (lblTotal != null) lblTotal.Text = $"Total: {dt?.Rows.Count ?? 0}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cursos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPrerequisitos()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    dgvPrereq.DataSource = null;
                    _dtPrereq = null;
                    return;
                }

                int cursoId = int.Parse(txtId.Text);
                _dtPrereq = _cn.ListarPrereq(cursoId);   // CURSO_ID, NOMBRE, TIENE (0/1 o bool)

                // Normaliza a bool
                if (_dtPrereq.Columns["TIENE"].DataType != typeof(bool))
                {
                    var colBool = new DataColumn("TIENE_BOOL", typeof(bool));
                    _dtPrereq.Columns.Add(colBool);
                    foreach (DataRow r in _dtPrereq.Rows)
                        r["TIENE_BOOL"] = Convert.ToInt32(r["TIENE"]) == 1;
                    _dtPrereq.Columns.Remove("TIENE");
                    colBool.ColumnName = "TIENE";
                }

                // Copia estado original
                if (!_dtPrereq.Columns.Contains("ORIG"))
                    _dtPrereq.Columns.Add("ORIG", typeof(bool));
                foreach (DataRow r in _dtPrereq.Rows)
                    r["ORIG"] = (bool)r["TIENE"];

                dgvPrereq.DataSource = _dtPrereq;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar prerrequisitos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /* ========== UI HELPERS ========== */
        private void ModoNuevo()
        {
            if (txtId != null) txtId.Text = "";
            if (txtCodigo != null) txtCodigo.Text = "";
            if (txtNombre != null) txtNombre.Text = "";
            if (nudCreditos != null) nudCreditos.Value = 0;
            if (chkActivo != null) chkActivo.Checked = true;
            txtCodigo?.Focus();

            if (btnEliminar != null) btnEliminar.Enabled = false;

            dgvPrereq.DataSource = null;
            _dtPrereq = null;
        }

        private void dgvCursos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var rowView = dgvCursos.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (rowView == null) return;

            var row = rowView.Row;
            txtId.Text = row["CURSO_ID"]?.ToString();
            txtCodigo.Text = row["CODIGO"]?.ToString();
            txtNombre.Text = row["NOMBRE"]?.ToString();

            // Créditos
            try
            {
                var cred = row["CREDITOS"];
                decimal credVal = 0;
                if (cred != DBNull.Value) decimal.TryParse(cred.ToString(), out credVal);
                nudCreditos.Value = Math.Max(0, Math.Min(1000, credVal));
            }
            catch { nudCreditos.Value = 0; }

            // Estado
            chkActivo.Checked = row["ESTADO"] != DBNull.Value && Convert.ToBoolean(row["ESTADO"]);

            if (btnEliminar != null) btnEliminar.Enabled = true;

            // Cargar prerequisitos del seleccionado
            CargarPrerequisitos();

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var codigo = (txtCodigo.Text ?? "").Trim();
                var nombre = (txtNombre.Text ?? "").Trim();
                var creditos = (int)nudCreditos.Value;
                var estado = chkActivo.Checked;

                if (string.IsNullOrWhiteSpace(codigo)) throw new Exception("Ingrese el CÓDIGO del curso.");
                if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese el NOMBRE del curso.");
                if (creditos < 0) throw new Exception("Los CRÉDITOS no pueden ser negativos.");

                int id = 0;
                if (!string.IsNullOrWhiteSpace(txtId.Text))
                    id = int.Parse(txtId.Text);

                _cn.Guardar(id, codigo, nombre, creditos, estado);

                MessageBox.Show(id == 0 ? "Curso creado correctamente." : "Curso actualizado correctamente.",
                                "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarCursos();

                if (id == 0)
                    ModoNuevo();
                else
                    CargarPrerequisitos(); // refresca por si cambió nombre/código
            }
            catch (SqlException sqlEx)
            {
                // 2627 / 2601: clave duplicada (índice único en CODIGO)
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                    MessageBox.Show("El CÓDIGO ya existe. Ingrese otro.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Error SQL: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) return;

            if (MessageBox.Show("¿Eliminar el curso seleccionado?", "Confirmar",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _cn.Eliminar(int.Parse(txtId.Text));
                CargarCursos();
                ModoNuevo();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547) // FK constraint
                    MessageBox.Show("No se puede eliminar: el curso tiene secciones y/o dependencias.",
                                    "Restricción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Error SQL: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ModoNuevo();
          
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (dgvCursos.DataSource is DataTable dt)
            {
                var text = (txtBuscar.Text ?? "").Replace("'", "''");
                dt.DefaultView.RowFilter =
                    string.IsNullOrWhiteSpace(text)
                    ? ""
                    : $"CODIGO LIKE '%{text}%' OR NOMBRE LIKE '%{text}%'";
            }
        }

        private void btnGuardarPrereq_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || _dtPrereq == null) return;

            int cursoId = int.Parse(txtId.Text);

            try
            {
                foreach (DataRow r in _dtPrereq.Rows)
                {
                    bool nuevo = (bool)r["TIENE"];
                    bool orig = (bool)r["ORIG"];
                    int preId = Convert.ToInt32(r["CURSO_ID"]);

                    if (nuevo != orig)
                    {
                        if (nuevo) _cn.AgregarPrereq(cursoId, preId);
                        else _cn.QuitarPrereq(cursoId, preId);
                    }
                }

                CargarPrerequisitos();
                MessageBox.Show("Prerrequisitos actualizados.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                    MessageBox.Show("Ya existe ese prerrequisito.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Error SQL: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar prerrequisitos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

