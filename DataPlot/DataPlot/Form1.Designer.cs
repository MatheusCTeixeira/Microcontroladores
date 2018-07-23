namespace DataPlot
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.trbTemperaturaControle = new System.Windows.Forms.TrackBar();
            this.lblTemperaturaMin = new System.Windows.Forms.Label();
            this.lblTemperaturaMax = new System.Windows.Forms.Label();
            this.Serial = new System.IO.Ports.SerialPort(this.components);
            this.menuConfiguracao = new System.Windows.Forms.MenuStrip();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPorta = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trbTemperaturaControle)).BeginInit();
            this.menuConfiguracao.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(718, 242);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(12, 278);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(390, 204);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(425, 278);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(390, 204);
            this.panel3.TabIndex = 2;
            // 
            // trbTemperaturaControle
            // 
            this.trbTemperaturaControle.AutoSize = false;
            this.trbTemperaturaControle.Location = new System.Drawing.Point(756, 51);
            this.trbTemperaturaControle.Maximum = 60;
            this.trbTemperaturaControle.Minimum = 35;
            this.trbTemperaturaControle.Name = "trbTemperaturaControle";
            this.trbTemperaturaControle.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trbTemperaturaControle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trbTemperaturaControle.RightToLeftLayout = true;
            this.trbTemperaturaControle.Size = new System.Drawing.Size(32, 185);
            this.trbTemperaturaControle.TabIndex = 3;
            this.trbTemperaturaControle.Value = 35;
            this.trbTemperaturaControle.ValueChanged += new System.EventHandler(this.trbTemperaturaControle_ValueChanged);
            // 
            // lblTemperaturaMin
            // 
            this.lblTemperaturaMin.AutoSize = true;
            this.lblTemperaturaMin.Font = new System.Drawing.Font("OCR A Std", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemperaturaMin.ForeColor = System.Drawing.Color.Blue;
            this.lblTemperaturaMin.Location = new System.Drawing.Point(736, 250);
            this.lblTemperaturaMin.Name = "lblTemperaturaMin";
            this.lblTemperaturaMin.Size = new System.Drawing.Size(93, 20);
            this.lblTemperaturaMin.TabIndex = 4;
            this.lblTemperaturaMin.Text = "MIN ºC";
            // 
            // lblTemperaturaMax
            // 
            this.lblTemperaturaMax.AutoSize = true;
            this.lblTemperaturaMax.Font = new System.Drawing.Font("OCR A Std", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemperaturaMax.ForeColor = System.Drawing.Color.Red;
            this.lblTemperaturaMax.Location = new System.Drawing.Point(736, 28);
            this.lblTemperaturaMax.Name = "lblTemperaturaMax";
            this.lblTemperaturaMax.Size = new System.Drawing.Size(93, 20);
            this.lblTemperaturaMax.TabIndex = 5;
            this.lblTemperaturaMax.Text = "MAX ºC";
            // 
            // Serial
            // 
            this.Serial.Parity = System.IO.Ports.Parity.Even;
            this.Serial.ReadTimeout = 15;
            // 
            // menuConfiguracao
            // 
            this.menuConfiguracao.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sobreToolStripMenuItem});
            this.menuConfiguracao.Location = new System.Drawing.Point(0, 0);
            this.menuConfiguracao.Name = "menuConfiguracao";
            this.menuConfiguracao.Size = new System.Drawing.Size(821, 24);
            this.menuConfiguracao.TabIndex = 6;
            this.menuConfiguracao.Text = "menuStrip1";
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.sobreToolStripMenuItem.Text = "Sobre";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 493);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Porta:";
            // 
            // cmbPorta
            // 
            this.cmbPorta.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbPorta.FormattingEnabled = true;
            this.cmbPorta.Location = new System.Drawing.Point(53, 490);
            this.cmbPorta.Name = "cmbPorta";
            this.cmbPorta.Size = new System.Drawing.Size(221, 21);
            this.cmbPorta.Sorted = true;
            this.cmbPorta.TabIndex = 8;
            this.cmbPorta.SelectedIndexChanged += new System.EventHandler(this.cmbPorta_SelectedIndexChanged);
            this.cmbPorta.Click += new System.EventHandler(this.cmbPorta_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 515);
            this.Controls.Add(this.cmbPorta);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTemperaturaMax);
            this.Controls.Add(this.lblTemperaturaMin);
            this.Controls.Add(this.trbTemperaturaControle);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuConfiguracao);
            this.MainMenuStrip = this.menuConfiguracao;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trbTemperaturaControle)).EndInit();
            this.menuConfiguracao.ResumeLayout(false);
            this.menuConfiguracao.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TrackBar trbTemperaturaControle;
        private System.Windows.Forms.Label lblTemperaturaMin;
        private System.Windows.Forms.Label lblTemperaturaMax;
        private System.IO.Ports.SerialPort Serial;
        private System.Windows.Forms.MenuStrip menuConfiguracao;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPorta;
    }
}

