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
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Data.SqlClient;

namespace PG2_SIRAF_C_
{
    public partial class frmEstudiantes : Form
    {

        private readonly clsEstudianteCN _cn = new clsEstudianteCN();


        private DataTable _dtListado;

        private FilterInfoCollection _dispositivos;
        private VideoCaptureDevice _camara;
        private bool _modoCaptura = false; 

        public frmEstudiantes()
        {
            InitializeComponent();

            this.Load += frmEstudiantes_Load;
            this.FormClosing += frmEstudiantes_FormClosing;

            if (btnNuevo != null) btnNuevo.Click += btnAgregarMatricula_Click;
            if (btnGuardar != null) btnGuardar.Click += btnGuardar_Click;
            if (btnDesactivar != null) btnDesactivar.Click += btnDesactivar_Click;
            if (btnBuscar != null) btnBuscar.Click += btnBuscar_Click;

          
            if (dgvEstudiantes != null) dgvEstudiantes.CellClick += dgvEstudiantes_CellClick;

            
            if (btnAgregar != null) btnAgregar.Click += btnAgregar_Click;
            if (btnCapturar != null) btnCapturar.Click += btnCapturar_Click;
        }

        private void frmEstudiantes_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarTurnos();
            CargarListado(string.Empty);
            DetectarCamaras();
            ModoNuevo();

            if (btnCapturar != null) btnCapturar.Text = "Capturar"; 
        }

        private void ConfigurarGrid()
        {
            if (dgvEstudiantes == null) return;
            dgvEstudiantes.AutoGenerateColumns = true;
            dgvEstudiantes.ReadOnly = true;
            dgvEstudiantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstudiantes.MultiSelect = false;
            dgvEstudiantes.AllowUserToAddRows = false;
            dgvEstudiantes.AllowUserToDeleteRows = false;
        }

        private void CargarListado(string filtro)
        {
            _dtListado = _cn.Listar(filtro);
            if (dgvEstudiantes != null) dgvEstudiantes.DataSource = _dtListado;

           
            if (dgvEstudiantes != null && dgvEstudiantes.Columns["FOTO"] != null)
                dgvEstudiantes.Columns["FOTO"].Visible = false;
        }

        private void CargarTurnos()
        {
            var dt = _cn.ListarTurnos();

           
            if (cmbTurno == null)
            {
                MessageBox.Show("cmbTurno no existe. Asegúrate de que el control sea un ComboBox con Name = cmbTurno.");
                return;
            }

            cmbTurno.DataSource = dt;
            cmbTurno.DisplayMember = "TURNO_NOMBRE";
            cmbTurno.ValueMember = "TURNO_ID";
            if (cmbTurno.Items.Count > 0) cmbTurno.SelectedIndex = 0;
        }
        

        private void DetectarCamaras()
        {
            try
            {
                _dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            }
            catch
            {
                _dispositivos = null;
            }
        }
        private void btnAgregarMatricula_Click(object sender, EventArgs e)
        {
            ModoNuevo();

           
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo?.Text;
                string nombres = txtNombres?.Text;
                string apellidos = txtApellidos?.Text;
                string documento = txtDocumento?.Text;
                string ciclo = txtCiclo?.Text;

                int turnoId = 0;
                if (cmbTurno != null && cmbTurno.SelectedValue != null)
                    turnoId = Convert.ToInt32(cmbTurno.SelectedValue);

                bool estado = chkActivo != null ? chkActivo.Checked : true;

                byte[] fotoBytes = GetFotoBytes(); // puede ser null

                int alumnoIdActual = AlumnoActualId();

                if (alumnoIdActual <= 0)
                {
                    // INSERT
                    int nuevoId = _cn.Insertar(codigo, nombres, apellidos, documento, ciclo, turnoId, estado, fotoBytes);
                    MessageBox.Show("Alumno registrado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarListado(txtBuscar?.Text ?? string.Empty);
                    SeleccionarFilaPorId(nuevoId);
                    if (txtAlumnoID != null) txtAlumnoID.Text = nuevoId.ToString();
                    ModoEdicion();
                }
                else
                {
                    // UPDATE
                    bool ok = _cn.Actualizar(alumnoIdActual, codigo, nombres, apellidos, documento, ciclo, turnoId, estado, fotoBytes);
                    if (ok)
                    {
                        MessageBox.Show("Alumno actualizado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarListado(txtBuscar?.Text ?? string.Empty);
                        SeleccionarFilaPorId(alumnoIdActual);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
           
        private void btnQuitarMatricula_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvEstudiantes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvEstudiantes == null) return;
            var row = dgvEstudiantes.Rows[e.RowIndex];

            txtAlumnoID?.SetText(row.Cells["ALUMNO_ID"]?.Value?.ToString());
            txtCodigo?.SetText(row.Cells["CODIGO"]?.Value?.ToString());
            txtNombres?.SetText(row.Cells["NOMBRES"]?.Value?.ToString());
            txtApellidos?.SetText(row.Cells["APELLIDOS"]?.Value?.ToString());
            txtDocumento?.SetText(row.Cells["DOCUMENTO"]?.Value == null ? "" : row.Cells["DOCUMENTO"].Value.ToString());
            txtCiclo?.SetText(row.Cells["CICLO"]?.Value?.ToString());

            if (cmbTurno != null && row.Cells["TURNO_ID"]?.Value != null)
                cmbTurno.SelectedValue = Convert.ToInt32(row.Cells["TURNO_ID"].Value);

            if (chkActivo != null && row.Cells["ESTADO"]?.Value != null)
                chkActivo.Checked = Convert.ToBoolean(row.Cells["ESTADO"].Value);

            // Foto desde BD
            int alumnoId = AlumnoActualId();
            var bytes = _cn.ObtenerFoto(alumnoId);
            SetFotoFromBytes(bytes);

            ModoEdicion();
        }

        private int AlumnoActualId()
        {
            int id = 0;
            if (txtAlumnoID != null) int.TryParse(txtAlumnoID.Text, out id);
            return id;
        }

        private void SeleccionarFilaPorId(int alumnoId)
        {
            if (dgvEstudiantes == null || dgvEstudiantes.Rows.Count == 0) return;
            foreach (DataGridViewRow r in dgvEstudiantes.Rows)
            {
                if (r.Cells["ALUMNO_ID"] != null && r.Cells["ALUMNO_ID"].Value != null)
                {
                    int idRow = Convert.ToInt32(r.Cells["ALUMNO_ID"].Value);
                    if (idRow == alumnoId)
                    {
                        r.Selected = true;
                        try
                        {
                            dgvEstudiantes.CurrentCell = r.Cells[0];
                            dgvEstudiantes.FirstDisplayedScrollingRowIndex = r.Index;
                        }
                        catch { }
                        break;
                    }
                }
            }
        }

        private void ModoNuevo()
        {
            LimpiarFormulario();
            if (btnDesactivar != null) btnDesactivar.Enabled = false;
            if (txtCodigo != null) txtCodigo.Focus();
        }

        private void ModoEdicion()
        {
            if (btnDesactivar != null) btnDesactivar.Enabled = true;
        }

        private void LimpiarFormulario()
        {
            if (txtAlumnoID != null) txtAlumnoID.Text = "";
            if (txtCodigo != null) txtCodigo.Text = "";
            if (txtNombres != null) txtNombres.Text = "";
            if (txtApellidos != null) txtApellidos.Text = "";
            if (txtDocumento != null) txtDocumento.Text = "";
            if (txtCiclo != null) txtCiclo.Text = "";
            if (cmbTurno != null && cmbTurno.Items.Count > 0) cmbTurno.SelectedIndex = 0;
            if (chkActivo != null) chkActivo.Checked = true;

            if (picFoto != null && picFoto.Image != null) { picFoto.Image.Dispose(); picFoto.Image = null; }
            if (picCamara != null && picCamara.Image != null) { picCamara.Image.Dispose(); picCamara.Image = null; }

            _modoCaptura = false;
            DetenerCamara();
            if (btnCapturar != null) btnCapturar.Text = "Capturar";
        }

       
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = (txtBuscar != null) ? txtBuscar.Text : string.Empty;
            CargarListado(filtro);
        }

        

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            int alumnoId = AlumnoActualId();
            if (alumnoId <= 0) { MessageBox.Show("Selecciona un alumno.", "Aviso"); return; }

            if (MessageBox.Show("¿Deseas DESACTIVAR este alumno?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (_cn.Desactivar(alumnoId))
                    {
                        MessageBox.Show("Alumno desactivado.", "OK");
                        CargarListado((txtBuscar != null) ? txtBuscar.Text : string.Empty);
                        ModoNuevo();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "No se pudo desactivar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Seleccionar foto";
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var img = Image.FromFile(ofd.FileName);
                        if (picFoto != null && picFoto.Image != null) picFoto.Image.Dispose();
                        if (picFoto != null) picFoto.Image = img;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se pudo cargar la imagen: " + ex.Message);
                    }
                }
            }
        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_modoCaptura)
                {
                    
                    IniciarCamaraSiHaceFalta();
                    _modoCaptura = true;
                    if (btnCapturar != null) btnCapturar.Text = "Tomar foto";
                    return;
                }

               
                if (picCamara == null || picCamara.Image == null)
                {
                    MessageBox.Show("La cámara no está lista todavía.");
                    return;
                }

                if (picFoto != null && picFoto.Image != null) picFoto.Image.Dispose();
                if (picFoto != null) picFoto.Image = (Bitmap)picCamara.Image.Clone();

                
                DetenerCamara();
                _modoCaptura = false;
                if (btnCapturar != null) btnCapturar.Text = "Capturar";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cámara", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // fallback
                DetenerCamara();
                _modoCaptura = false;
                if (btnCapturar != null) btnCapturar.Text = "Capturar";
            }
        }
        private void IniciarCamaraSiHaceFalta()
        {
            if (_camara != null && _camara.IsRunning) return;
            if (_dispositivos == null || (_dispositivos != null && _dispositivos.Count == 0))
                throw new Exception("No se encontró ninguna cámara.");

            // Usar la PRIMERA cámara
            _camara = new VideoCaptureDevice(_dispositivos[0].MonikerString);
            _camara.NewFrame += Camara_NewFrame;
            _camara.Start();
        }

        private void Camara_NewFrame(object sender, NewFrameEventArgs e)
        {
            try
            {
                Bitmap bmp = (Bitmap)e.Frame.Clone();
                if (picCamara != null)
                {
                    if (picCamara.InvokeRequired)
                    {
                        picCamara.Invoke(new Action(() =>
                        {
                            if (picCamara.Image != null) picCamara.Image.Dispose();
                            picCamara.Image = (Bitmap)bmp.Clone();
                        }));
                    }
                    else
                    {
                        if (picCamara.Image != null) picCamara.Image.Dispose();
                        picCamara.Image = (Bitmap)bmp.Clone();
                    }
                }
                bmp.Dispose();
            }
            catch { }
        }

        private void DetenerCamara()
        {
            try
            {
                if (_camara != null && _camara.IsRunning)
                {
                    _camara.SignalToStop();
                    _camara.WaitForStop();
                }
                _camara = null;
            }
            catch { }
        }

        private void frmEstudiantes_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetenerCamara();
        }

        private byte[] GetFotoBytes()
        {
            if (picFoto == null || picFoto.Image == null) return null;
            using (var ms = new MemoryStream())
            {
                // Guardamos como JPEG para tamaño razonable
                picFoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void SetFotoFromBytes(byte[] bytes)
        {
            if (picFoto == null)
                return;

            if (bytes == null || bytes.Length == 0)
            {
                if (picFoto.Image != null) { picFoto.Image.Dispose(); picFoto.Image = null; }
                return;
            }
            using (var ms = new MemoryStream(bytes))
            {
                if (picFoto.Image != null) picFoto.Image.Dispose();
                picFoto.Image = Image.FromStream(ms);
            }
        }

       

    }
    
    internal static class ControlExtensions
    {
        public static void SetText(this Control c, string value)
        {
            if (c == null) return;
            c.Text = value ?? string.Empty;
        }
    }
}
    
