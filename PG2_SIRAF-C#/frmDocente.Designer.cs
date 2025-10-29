namespace PG2_SIRAF_C_
{
    partial class frmDocente
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGuardarPrereq = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtBuscarDoc = new System.Windows.Forms.TextBox();
            this.btnNuevoDoc = new System.Windows.Forms.Button();
            this.btnRefrescarUsuarios = new System.Windows.Forms.Button();
            this.btnEliminarDoc = new System.Windows.Forms.Button();
            this.btnGuardarDoc = new System.Windows.Forms.Button();
            this.dgvDocentes = new System.Windows.Forms.DataGridView();
            this.txtCorreoUsuario = new System.Windows.Forms.TextBox();
            this.txtNombreUsuario = new System.Windows.Forms.TextBox();
            this.txtDocenteId = new System.Windows.Forms.TextBox();
            this.chkEstado = new System.Windows.Forms.CheckBox();
            this.Cooreo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.Desempeño = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocentes)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGuardarPrereq
            // 
            this.btnGuardarPrereq.Location = new System.Drawing.Point(110, 197);
            this.btnGuardarPrereq.Name = "btnGuardarPrereq";
            this.btnGuardarPrereq.Size = new System.Drawing.Size(99, 29);
            this.btnGuardarPrereq.TabIndex = 37;
            this.btnGuardarPrereq.Text = "Guardar Pre-Rq";
            this.btnGuardarPrereq.UseVisualStyleBackColor = true;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(307, 410);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(129, 13);
            this.lblTotal.TabIndex = 36;
            this.lblTotal.Text = "REGISTRO DE CURSOS";
            // 
            // txtBuscarDoc
            // 
            this.txtBuscarDoc.Location = new System.Drawing.Point(167, 403);
            this.txtBuscarDoc.Name = "txtBuscarDoc";
            this.txtBuscarDoc.Size = new System.Drawing.Size(100, 20);
            this.txtBuscarDoc.TabIndex = 35;
            this.txtBuscarDoc.TextChanged += new System.EventHandler(this.txtBuscarDoc_TextChanged);
            // 
            // btnNuevoDoc
            // 
            this.btnNuevoDoc.Location = new System.Drawing.Point(215, 197);
            this.btnNuevoDoc.Name = "btnNuevoDoc";
            this.btnNuevoDoc.Size = new System.Drawing.Size(99, 27);
            this.btnNuevoDoc.TabIndex = 34;
            this.btnNuevoDoc.Text = "Nuevo";
            this.btnNuevoDoc.UseVisualStyleBackColor = true;
            this.btnNuevoDoc.Click += new System.EventHandler(this.btnNuevoDoc_Click);
            // 
            // btnRefrescarUsuarios
            // 
            this.btnRefrescarUsuarios.Location = new System.Drawing.Point(320, 199);
            this.btnRefrescarUsuarios.Name = "btnRefrescarUsuarios";
            this.btnRefrescarUsuarios.Size = new System.Drawing.Size(71, 25);
            this.btnRefrescarUsuarios.TabIndex = 33;
            this.btnRefrescarUsuarios.Text = "Actualizar";
            this.btnRefrescarUsuarios.UseVisualStyleBackColor = true;
            this.btnRefrescarUsuarios.Click += new System.EventHandler(this.btnRefrescarUsuarios_Click);
            // 
            // btnEliminarDoc
            // 
            this.btnEliminarDoc.Location = new System.Drawing.Point(382, 382);
            this.btnEliminarDoc.Name = "btnEliminarDoc";
            this.btnEliminarDoc.Size = new System.Drawing.Size(99, 22);
            this.btnEliminarDoc.TabIndex = 32;
            this.btnEliminarDoc.Text = "Eliminar";
            this.btnEliminarDoc.UseVisualStyleBackColor = true;
            this.btnEliminarDoc.Click += new System.EventHandler(this.btnEliminarDoc_Click);
            // 
            // btnGuardarDoc
            // 
            this.btnGuardarDoc.Location = new System.Drawing.Point(397, 198);
            this.btnGuardarDoc.Name = "btnGuardarDoc";
            this.btnGuardarDoc.Size = new System.Drawing.Size(84, 27);
            this.btnGuardarDoc.TabIndex = 31;
            this.btnGuardarDoc.Text = "Guardar";
            this.btnGuardarDoc.UseVisualStyleBackColor = true;
            this.btnGuardarDoc.Click += new System.EventHandler(this.btnGuardarDoc_Click);
            // 
            // dgvDocentes
            // 
            this.dgvDocentes.AllowUserToAddRows = false;
            this.dgvDocentes.AllowUserToDeleteRows = false;
            this.dgvDocentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocentes.Location = new System.Drawing.Point(110, 226);
            this.dgvDocentes.Name = "dgvDocentes";
            this.dgvDocentes.ReadOnly = true;
            this.dgvDocentes.Size = new System.Drawing.Size(326, 150);
            this.dgvDocentes.TabIndex = 30;
            this.dgvDocentes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocentes_CellClick);
            this.dgvDocentes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocentes_CellContentClick);
            // 
            // txtCorreoUsuario
            // 
            this.txtCorreoUsuario.Location = new System.Drawing.Point(246, 113);
            this.txtCorreoUsuario.Name = "txtCorreoUsuario";
            this.txtCorreoUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtCorreoUsuario.TabIndex = 28;
            // 
            // txtNombreUsuario
            // 
            this.txtNombreUsuario.Location = new System.Drawing.Point(119, 114);
            this.txtNombreUsuario.Name = "txtNombreUsuario";
            this.txtNombreUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtNombreUsuario.TabIndex = 27;
            // 
            // txtDocenteId
            // 
            this.txtDocenteId.Location = new System.Drawing.Point(119, 58);
            this.txtDocenteId.Name = "txtDocenteId";
            this.txtDocenteId.Size = new System.Drawing.Size(100, 20);
            this.txtDocenteId.TabIndex = 26;
            // 
            // chkEstado
            // 
            this.chkEstado.AutoSize = true;
            this.chkEstado.Location = new System.Drawing.Point(119, 174);
            this.chkEstado.Name = "chkEstado";
            this.chkEstado.Size = new System.Drawing.Size(56, 17);
            this.chkEstado.TabIndex = 25;
            this.chkEstado.Text = "Activo";
            this.chkEstado.UseVisualStyleBackColor = true;
            // 
            // Cooreo
            // 
            this.Cooreo.AutoSize = true;
            this.Cooreo.Location = new System.Drawing.Point(243, 97);
            this.Cooreo.Name = "Cooreo";
            this.Cooreo.Size = new System.Drawing.Size(32, 13);
            this.Cooreo.TabIndex = 23;
            this.Cooreo.Text = "Email";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Nombre ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(268, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "REGISTRO DE DOCENTE";
            // 
            // cmbRol
            // 
            this.cmbRol.FormattingEnabled = true;
            this.cmbRol.Location = new System.Drawing.Point(246, 58);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(121, 21);
            this.cmbRol.TabIndex = 38;
            this.cmbRol.SelectedIndexChanged += new System.EventHandler(this.cmbUsuario_SelectedIndexChanged);
            // 
            // txtTitulo
            // 
            this.txtTitulo.Location = new System.Drawing.Point(371, 114);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(100, 20);
            this.txtTitulo.TabIndex = 39;
            // 
            // Desempeño
            // 
            this.Desempeño.AutoSize = true;
            this.Desempeño.Location = new System.Drawing.Point(372, 97);
            this.Desempeño.Name = "Desempeño";
            this.Desempeño.Size = new System.Drawing.Size(64, 13);
            this.Desempeño.TabIndex = 40;
            this.Desempeño.Text = "Desempeño";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(243, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Rol";
            // 
            // frmDocente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Desempeño);
            this.Controls.Add(this.txtTitulo);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.btnGuardarPrereq);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.txtBuscarDoc);
            this.Controls.Add(this.btnNuevoDoc);
            this.Controls.Add(this.btnRefrescarUsuarios);
            this.Controls.Add(this.btnEliminarDoc);
            this.Controls.Add(this.btnGuardarDoc);
            this.Controls.Add(this.dgvDocentes);
            this.Controls.Add(this.txtCorreoUsuario);
            this.Controls.Add(this.txtNombreUsuario);
            this.Controls.Add(this.txtDocenteId);
            this.Controls.Add(this.chkEstado);
            this.Controls.Add(this.Cooreo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmDocente";
            this.Text = "frmDocente";
            this.Load += new System.EventHandler(this.frmDocente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocentes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGuardarPrereq;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtBuscarDoc;
        private System.Windows.Forms.Button btnNuevoDoc;
        private System.Windows.Forms.Button btnRefrescarUsuarios;
        private System.Windows.Forms.Button btnEliminarDoc;
        private System.Windows.Forms.Button btnGuardarDoc;
        private System.Windows.Forms.DataGridView dgvDocentes;
        private System.Windows.Forms.TextBox txtCorreoUsuario;
        private System.Windows.Forms.TextBox txtNombreUsuario;
        private System.Windows.Forms.TextBox txtDocenteId;
        private System.Windows.Forms.CheckBox chkEstado;
        private System.Windows.Forms.Label Cooreo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.Label Desempeño;
        private System.Windows.Forms.Label label4;
    }
}