namespace Jeu_Graph
{
    partial class FrmIntro
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
            this.lbIntro = new System.Windows.Forms.Label();
            this.lblTitre = new System.Windows.Forms.Label();
            this.lblNomJoueur = new System.Windows.Forms.Label();
            this.tbNomJoueur = new System.Windows.Forms.TextBox();
            this.lblChoixCarte = new System.Windows.Forms.Label();
            this.cbChoixCarte = new System.Windows.Forms.ComboBox();
            this.btnDemarrer = new System.Windows.Forms.Button();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbIntro
            // 
            this.lbIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbIntro.AutoSize = true;
            this.lbIntro.Font = new System.Drawing.Font("Microsoft Uighur", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIntro.Location = new System.Drawing.Point(102, 63);
            this.lbIntro.Name = "lbIntro";
            this.lbIntro.Size = new System.Drawing.Size(38, 28);
            this.lbIntro.TabIndex = 0;
            this.lbIntro.Text = "Text";
            this.lbIntro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitre
            // 
            this.lblTitre.AutoSize = true;
            this.lblTitre.Font = new System.Drawing.Font("Megrim", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitre.ForeColor = System.Drawing.Color.Black;
            this.lblTitre.Location = new System.Drawing.Point(179, 9);
            this.lblTitre.Name = "lblTitre";
            this.lblTitre.Size = new System.Drawing.Size(319, 29);
            this.lblTitre.TabIndex = 1;
            this.lblTitre.Text = "Chasseur de Monstres";
            // 
            // lblNomJoueur
            // 
            this.lblNomJoueur.AutoSize = true;
            this.lblNomJoueur.ForeColor = System.Drawing.Color.Black;
            this.lblNomJoueur.Location = new System.Drawing.Point(12, 230);
            this.lblNomJoueur.Name = "lblNomJoueur";
            this.lblNomJoueur.Size = new System.Drawing.Size(169, 13);
            this.lblNomJoueur.TabIndex = 2;
            this.lblNomJoueur.Text = "Veuillez choisir un nom, chasseur: ";
            // 
            // tbNomJoueur
            // 
            this.tbNomJoueur.Location = new System.Drawing.Point(184, 227);
            this.tbNomJoueur.Name = "tbNomJoueur";
            this.tbNomJoueur.Size = new System.Drawing.Size(130, 20);
            this.tbNomJoueur.TabIndex = 3;
            // 
            // lblChoixCarte
            // 
            this.lblChoixCarte.AutoSize = true;
            this.lblChoixCarte.ForeColor = System.Drawing.Color.Black;
            this.lblChoixCarte.Location = new System.Drawing.Point(14, 257);
            this.lblChoixCarte.Name = "lblChoixCarte";
            this.lblChoixCarte.Size = new System.Drawing.Size(154, 13);
            this.lblChoixCarte.TabIndex = 4;
            this.lblChoixCarte.Text = "Veuillez choisir votre aventure: ";
            // 
            // cbChoixCarte
            // 
            this.cbChoixCarte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChoixCarte.FormattingEnabled = true;
            this.cbChoixCarte.Location = new System.Drawing.Point(184, 254);
            this.cbChoixCarte.Name = "cbChoixCarte";
            this.cbChoixCarte.Size = new System.Drawing.Size(130, 21);
            this.cbChoixCarte.TabIndex = 5;
            // 
            // btnDemarrer
            // 
            this.btnDemarrer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDemarrer.ForeColor = System.Drawing.Color.Black;
            this.btnDemarrer.Location = new System.Drawing.Point(276, 323);
            this.btnDemarrer.Name = "btnDemarrer";
            this.btnDemarrer.Size = new System.Drawing.Size(139, 47);
            this.btnDemarrer.TabIndex = 6;
            this.btnDemarrer.Text = "Commencer";
            this.btnDemarrer.UseVisualStyleBackColor = true;
            this.btnDemarrer.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbInfo
            // 
            this.tbInfo.Enabled = false;
            this.tbInfo.ForeColor = System.Drawing.Color.Red;
            this.tbInfo.Location = new System.Drawing.Point(350, 223);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbInfo.Size = new System.Drawing.Size(330, 52);
            this.tbInfo.TabIndex = 7;
            // 
            // FrmIntro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 382);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.btnDemarrer);
            this.Controls.Add(this.cbChoixCarte);
            this.Controls.Add(this.lblChoixCarte);
            this.Controls.Add(this.tbNomJoueur);
            this.Controls.Add(this.lblNomJoueur);
            this.Controls.Add(this.lblTitre);
            this.Controls.Add(this.lbIntro);
            this.ForeColor = System.Drawing.Color.Maroon;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmIntro";
            this.Text = "Pojet de Session: Chasseur de Monstres";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmIntro_FormClosed);
            this.Load += new System.EventHandler(this.FrmIntro_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbIntro;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblNomJoueur;
        private System.Windows.Forms.TextBox tbNomJoueur;
        private System.Windows.Forms.Label lblChoixCarte;
        private System.Windows.Forms.ComboBox cbChoixCarte;
        private System.Windows.Forms.Button btnDemarrer;
        private System.Windows.Forms.TextBox tbInfo;
    }
}

