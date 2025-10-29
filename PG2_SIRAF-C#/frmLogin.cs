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
    public partial class frmLogin : Form
    {
        private readonly clsUsuarioCN usuarioCN = new clsUsuarioCN();
        public frmLogin()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtContrasena.UseSystemPasswordChar = true;
            txtUsuario.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var res = usuarioCN.AutenticarUsuario(txtUsuario.Text, txtContrasena.Text);

            if (!res.EsValido)
            {
                MessageBox.Show(res.Mensaje, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.SelectAll();
                txtContrasena.Focus();
                return;
            }

            var principal = new frmPrincipal(res); // pasa clsResultadoLogin
            principal.Show();
            this.Hide();
        }
    }
    }
    
