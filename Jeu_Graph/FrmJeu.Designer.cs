namespace Jeu_Graph
{
    partial class FrmJeu
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
            this.pnlCarte = new System.Windows.Forms.Panel();
            this.gbInfoJoueur = new System.Windows.Forms.GroupBox();
            this.lblHPChiffre = new System.Windows.Forms.Label();
            this.pbHP = new System.Windows.Forms.ProgressBar();
            this.lblScoreChiffre = new System.Windows.Forms.Label();
            this.lblJoueurNom = new System.Windows.Forms.Label();
            this.lblNiveauChiffre = new System.Windows.Forms.Label();
            this.lblCarteNom = new System.Windows.Forms.Label();
            this.lblCarte = new System.Windows.Forms.Label();
            this.lblNiveau = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblHP = new System.Windows.Forms.Label();
            this.lblNom = new System.Windows.Forms.Label();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.btnRecommencer = new System.Windows.Forms.Button();
            this.lblLose = new System.Windows.Forms.Label();
            this.btnNiveauSuivant = new System.Windows.Forms.Button();
            this.lblWin = new System.Windows.Forms.Label();
            this.tbInfoAction = new System.Windows.Forms.TextBox();
            this.timerMonstre = new System.Windows.Forms.Timer(this.components);
            this.gbInfoJoueur.SuspendLayout();
            this.gbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCarte
            // 
            this.pnlCarte.AutoSize = true;
            this.pnlCarte.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlCarte.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlCarte.Location = new System.Drawing.Point(0, 0);
            this.pnlCarte.Name = "pnlCarte";
            this.pnlCarte.Size = new System.Drawing.Size(0, 0);
            this.pnlCarte.TabIndex = 0;
            this.pnlCarte.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCarte_Paint);
            // 
            // gbInfoJoueur
            // 
            this.gbInfoJoueur.Controls.Add(this.lblHPChiffre);
            this.gbInfoJoueur.Controls.Add(this.pbHP);
            this.gbInfoJoueur.Controls.Add(this.lblScoreChiffre);
            this.gbInfoJoueur.Controls.Add(this.lblJoueurNom);
            this.gbInfoJoueur.Controls.Add(this.lblNiveauChiffre);
            this.gbInfoJoueur.Controls.Add(this.lblCarteNom);
            this.gbInfoJoueur.Controls.Add(this.lblCarte);
            this.gbInfoJoueur.Controls.Add(this.lblNiveau);
            this.gbInfoJoueur.Controls.Add(this.lblScore);
            this.gbInfoJoueur.Controls.Add(this.lblHP);
            this.gbInfoJoueur.Controls.Add(this.lblNom);
            this.gbInfoJoueur.Font = new System.Drawing.Font("Kristen ITC", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInfoJoueur.Location = new System.Drawing.Point(59, 230);
            this.gbInfoJoueur.Name = "gbInfoJoueur";
            this.gbInfoJoueur.Size = new System.Drawing.Size(342, 171);
            this.gbInfoJoueur.TabIndex = 1;
            this.gbInfoJoueur.TabStop = false;
            this.gbInfoJoueur.Text = "Infos Partie";
            // 
            // lblHPChiffre
            // 
            this.lblHPChiffre.AutoSize = true;
            this.lblHPChiffre.BackColor = System.Drawing.Color.Transparent;
            this.lblHPChiffre.Font = new System.Drawing.Font("Kristen ITC", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHPChiffre.Location = new System.Drawing.Point(45, 109);
            this.lblHPChiffre.Name = "lblHPChiffre";
            this.lblHPChiffre.Size = new System.Drawing.Size(24, 18);
            this.lblHPChiffre.TabIndex = 10;
            this.lblHPChiffre.Text = "30";
            // 
            // pbHP
            // 
            this.pbHP.BackColor = System.Drawing.SystemColors.Control;
            this.pbHP.Location = new System.Drawing.Point(78, 109);
            this.pbHP.Maximum = 30;
            this.pbHP.Name = "pbHP";
            this.pbHP.Size = new System.Drawing.Size(177, 18);
            this.pbHP.TabIndex = 9;
            // 
            // lblScoreChiffre
            // 
            this.lblScoreChiffre.AutoSize = true;
            this.lblScoreChiffre.Location = new System.Drawing.Point(67, 136);
            this.lblScoreChiffre.Name = "lblScoreChiffre";
            this.lblScoreChiffre.Size = new System.Drawing.Size(17, 18);
            this.lblScoreChiffre.TabIndex = 8;
            this.lblScoreChiffre.Text = "0";
            // 
            // lblJoueurNom
            // 
            this.lblJoueurNom.AutoSize = true;
            this.lblJoueurNom.Location = new System.Drawing.Point(58, 81);
            this.lblJoueurNom.Name = "lblJoueurNom";
            this.lblJoueurNom.Size = new System.Drawing.Size(59, 18);
            this.lblJoueurNom.TabIndex = 7;
            this.lblJoueurNom.Text = "Joueur";
            // 
            // lblNiveauChiffre
            // 
            this.lblNiveauChiffre.AutoSize = true;
            this.lblNiveauChiffre.Location = new System.Drawing.Point(75, 54);
            this.lblNiveauChiffre.Name = "lblNiveauChiffre";
            this.lblNiveauChiffre.Size = new System.Drawing.Size(15, 18);
            this.lblNiveauChiffre.TabIndex = 6;
            this.lblNiveauChiffre.Text = "1";
            // 
            // lblCarteNom
            // 
            this.lblCarteNom.AutoSize = true;
            this.lblCarteNom.Location = new System.Drawing.Point(67, 27);
            this.lblCarteNom.Name = "lblCarteNom";
            this.lblCarteNom.Size = new System.Drawing.Size(50, 18);
            this.lblCarteNom.TabIndex = 5;
            this.lblCarteNom.Text = "Carte";
            // 
            // lblCarte
            // 
            this.lblCarte.AutoSize = true;
            this.lblCarte.Location = new System.Drawing.Point(6, 27);
            this.lblCarte.Name = "lblCarte";
            this.lblCarte.Size = new System.Drawing.Size(55, 18);
            this.lblCarte.TabIndex = 4;
            this.lblCarte.Text = "Carte:";
            // 
            // lblNiveau
            // 
            this.lblNiveau.AutoSize = true;
            this.lblNiveau.Location = new System.Drawing.Point(6, 54);
            this.lblNiveau.Name = "lblNiveau";
            this.lblNiveau.Size = new System.Drawing.Size(63, 18);
            this.lblNiveau.TabIndex = 3;
            this.lblNiveau.Text = "Niveau:";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(6, 136);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(55, 18);
            this.lblScore.TabIndex = 2;
            this.lblScore.Text = "Score:";
            // 
            // lblHP
            // 
            this.lblHP.AutoSize = true;
            this.lblHP.Location = new System.Drawing.Point(6, 109);
            this.lblHP.Name = "lblHP";
            this.lblHP.Size = new System.Drawing.Size(35, 18);
            this.lblHP.TabIndex = 1;
            this.lblHP.Text = "HP:";
            // 
            // lblNom
            // 
            this.lblNom.AutoSize = true;
            this.lblNom.Location = new System.Drawing.Point(6, 81);
            this.lblNom.Name = "lblNom";
            this.lblNom.Size = new System.Drawing.Size(46, 18);
            this.lblNom.TabIndex = 0;
            this.lblNom.Text = "Nom:";
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.btnRecommencer);
            this.gbInfo.Controls.Add(this.lblLose);
            this.gbInfo.Controls.Add(this.btnNiveauSuivant);
            this.gbInfo.Controls.Add(this.lblWin);
            this.gbInfo.Controls.Add(this.tbInfoAction);
            this.gbInfo.Font = new System.Drawing.Font("Kristen ITC", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInfo.Location = new System.Drawing.Point(493, 230);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(200, 171);
            this.gbInfo.TabIndex = 2;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "Infos Actions";
            // 
            // btnRecommencer
            // 
            this.btnRecommencer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecommencer.Enabled = false;
            this.btnRecommencer.Location = new System.Drawing.Point(43, 62);
            this.btnRecommencer.Name = "btnRecommencer";
            this.btnRecommencer.Size = new System.Drawing.Size(114, 46);
            this.btnRecommencer.TabIndex = 4;
            this.btnRecommencer.Text = "Recommencer";
            this.btnRecommencer.UseVisualStyleBackColor = true;
            this.btnRecommencer.Visible = false;
            this.btnRecommencer.Click += new System.EventHandler(this.btnRecommencer_Click);
            // 
            // lblLose
            // 
            this.lblLose.AutoSize = true;
            this.lblLose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblLose.Location = new System.Drawing.Point(3, 21);
            this.lblLose.Name = "lblLose";
            this.lblLose.Size = new System.Drawing.Size(48, 18);
            this.lblLose.TabIndex = 3;
            this.lblLose.Text = "label1";
            this.lblLose.Visible = false;
            // 
            // btnNiveauSuivant
            // 
            this.btnNiveauSuivant.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNiveauSuivant.Enabled = false;
            this.btnNiveauSuivant.Location = new System.Drawing.Point(24, 81);
            this.btnNiveauSuivant.Name = "btnNiveauSuivant";
            this.btnNiveauSuivant.Size = new System.Drawing.Size(114, 46);
            this.btnNiveauSuivant.TabIndex = 2;
            this.btnNiveauSuivant.Text = "Niveau Suivant";
            this.btnNiveauSuivant.UseVisualStyleBackColor = true;
            this.btnNiveauSuivant.Visible = false;
            this.btnNiveauSuivant.Click += new System.EventHandler(this.btnNiveauSuivant_Click);
            // 
            // lblWin
            // 
            this.lblWin.AutoSize = true;
            this.lblWin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWin.ForeColor = System.Drawing.Color.Green;
            this.lblWin.Location = new System.Drawing.Point(3, 21);
            this.lblWin.Name = "lblWin";
            this.lblWin.Size = new System.Drawing.Size(48, 18);
            this.lblWin.TabIndex = 1;
            this.lblWin.Text = "label1";
            this.lblWin.Visible = false;
            // 
            // tbInfoAction
            // 
            this.tbInfoAction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbInfoAction.Enabled = false;
            this.tbInfoAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInfoAction.Location = new System.Drawing.Point(6, 24);
            this.tbInfoAction.Multiline = true;
            this.tbInfoAction.Name = "tbInfoAction";
            this.tbInfoAction.ReadOnly = true;
            this.tbInfoAction.Size = new System.Drawing.Size(194, 141);
            this.tbInfoAction.TabIndex = 0;
            // 
            // timerMonstre
            // 
            this.timerMonstre.Interval = 2000;
            this.timerMonstre.Tick += new System.EventHandler(this.timerMonstre_Tick);
            // 
            // FrmJeu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(847, 443);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.gbInfoJoueur);
            this.Controls.Add(this.pnlCarte);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmJeu";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmJeu_FormClosed);
            this.Load += new System.EventHandler(this.FrmJeu_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmJeu_KeyDown);
            this.gbInfoJoueur.ResumeLayout(false);
            this.gbInfoJoueur.PerformLayout();
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCarte;
        private System.Windows.Forms.GroupBox gbInfoJoueur;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblHP;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.Label lblNiveau;
        private System.Windows.Forms.Label lblCarte;
        private System.Windows.Forms.ProgressBar pbHP;
        private System.Windows.Forms.Label lblScoreChiffre;
        private System.Windows.Forms.Label lblJoueurNom;
        private System.Windows.Forms.Label lblNiveauChiffre;
        private System.Windows.Forms.Label lblCarteNom;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.TextBox tbInfoAction;
        private System.Windows.Forms.Label lblHPChiffre;
        private System.Windows.Forms.Label lblWin;
        private System.Windows.Forms.Button btnNiveauSuivant;
        private System.Windows.Forms.Button btnRecommencer;
        private System.Windows.Forms.Label lblLose;
        private System.Windows.Forms.Timer timerMonstre;
    }
}