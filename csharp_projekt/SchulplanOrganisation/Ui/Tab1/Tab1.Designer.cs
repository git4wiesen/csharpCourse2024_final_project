namespace SchulplanOrganisation.Ui.Tab1
{
    partial class Tab1
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            CmdBearbeitenKlasse = new Button();
            panelButtons = new Panel();
            CmdLoeschenLehrer = new Button();
            CmdNeuerLehrer = new Button();
            CmdBearbeitenLehrer = new Button();
            CmdLoeschenSchueler = new Button();
            CmdNeuerSchueler = new Button();
            CmdBearbeitenSchueler = new Button();
            CmdRollbackDatabaseChanges = new Button();
            CmdLoeschenKlasse = new Button();
            CmdSaveDatabase = new Button();
            CmdNeueKlasse = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // CmdBearbeitenKlasse
            // 
            CmdBearbeitenKlasse.Location = new Point(6, 68);
            CmdBearbeitenKlasse.Margin = new Padding(6);
            CmdBearbeitenKlasse.Name = "CmdBearbeitenKlasse";
            CmdBearbeitenKlasse.Size = new Size(321, 49);
            CmdBearbeitenKlasse.TabIndex = 0;
            CmdBearbeitenKlasse.Text = "Klasse bearbeiten";
            CmdBearbeitenKlasse.UseVisualStyleBackColor = true;
            CmdBearbeitenKlasse.Click += CmdBearbeitenKlasse_Click;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(CmdLoeschenLehrer);
            panelButtons.Controls.Add(CmdNeuerLehrer);
            panelButtons.Controls.Add(CmdBearbeitenLehrer);
            panelButtons.Controls.Add(CmdLoeschenSchueler);
            panelButtons.Controls.Add(CmdNeuerSchueler);
            panelButtons.Controls.Add(CmdBearbeitenSchueler);
            panelButtons.Controls.Add(CmdRollbackDatabaseChanges);
            panelButtons.Controls.Add(CmdLoeschenKlasse);
            panelButtons.Controls.Add(CmdSaveDatabase);
            panelButtons.Controls.Add(CmdNeueKlasse);
            panelButtons.Controls.Add(CmdBearbeitenKlasse);
            panelButtons.Location = new Point(1606, 6);
            panelButtons.Margin = new Padding(6);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(334, 1427);
            panelButtons.TabIndex = 1;
            // 
            // CmdLoeschenLehrer
            // 
            CmdLoeschenLehrer.Location = new Point(7, 596);
            CmdLoeschenLehrer.Margin = new Padding(6);
            CmdLoeschenLehrer.Name = "CmdLoeschenLehrer";
            CmdLoeschenLehrer.Size = new Size(321, 49);
            CmdLoeschenLehrer.TabIndex = 10;
            CmdLoeschenLehrer.Text = "Lehrer löschen";
            CmdLoeschenLehrer.UseVisualStyleBackColor = true;
            CmdLoeschenLehrer.Click += CmdLoeschenLehrer_Click;
            // 
            // CmdNeuerLehrer
            // 
            CmdNeuerLehrer.Location = new Point(7, 472);
            CmdNeuerLehrer.Margin = new Padding(6);
            CmdNeuerLehrer.Name = "CmdNeuerLehrer";
            CmdNeuerLehrer.Size = new Size(321, 49);
            CmdNeuerLehrer.TabIndex = 9;
            CmdNeuerLehrer.Text = "Neuer Lehrer";
            CmdNeuerLehrer.UseVisualStyleBackColor = true;
            CmdNeuerLehrer.Click += CmdNeuerLehrer_Click;
            // 
            // CmdBearbeitenLehrer
            // 
            CmdBearbeitenLehrer.Location = new Point(7, 534);
            CmdBearbeitenLehrer.Margin = new Padding(6);
            CmdBearbeitenLehrer.Name = "CmdBearbeitenLehrer";
            CmdBearbeitenLehrer.Size = new Size(321, 49);
            CmdBearbeitenLehrer.TabIndex = 8;
            CmdBearbeitenLehrer.Text = "Lehrer bearbeiten";
            CmdBearbeitenLehrer.UseVisualStyleBackColor = true;
            CmdBearbeitenLehrer.Click += CmdBearbeitenLehrer_Click;
            // 
            // CmdLoeschenSchueler
            // 
            CmdLoeschenSchueler.Location = new Point(6, 362);
            CmdLoeschenSchueler.Margin = new Padding(6);
            CmdLoeschenSchueler.Name = "CmdLoeschenSchueler";
            CmdLoeschenSchueler.Size = new Size(321, 49);
            CmdLoeschenSchueler.TabIndex = 7;
            CmdLoeschenSchueler.Text = "Schüler löschen";
            CmdLoeschenSchueler.UseVisualStyleBackColor = true;
            CmdLoeschenSchueler.Click += CmdLoeschenSchueler_Click;
            // 
            // CmdNeuerSchueler
            // 
            CmdNeuerSchueler.Location = new Point(6, 238);
            CmdNeuerSchueler.Margin = new Padding(6);
            CmdNeuerSchueler.Name = "CmdNeuerSchueler";
            CmdNeuerSchueler.Size = new Size(321, 49);
            CmdNeuerSchueler.TabIndex = 6;
            CmdNeuerSchueler.Text = "Neuer Schüler";
            CmdNeuerSchueler.UseVisualStyleBackColor = true;
            CmdNeuerSchueler.Click += CmdNeuerSchueler_Click;
            // 
            // CmdBearbeitenSchueler
            // 
            CmdBearbeitenSchueler.Location = new Point(6, 300);
            CmdBearbeitenSchueler.Margin = new Padding(6);
            CmdBearbeitenSchueler.Name = "CmdBearbeitenSchueler";
            CmdBearbeitenSchueler.Size = new Size(321, 49);
            CmdBearbeitenSchueler.TabIndex = 5;
            CmdBearbeitenSchueler.Text = "Schüler bearbeiten";
            CmdBearbeitenSchueler.UseVisualStyleBackColor = true;
            CmdBearbeitenSchueler.Click += CmdBearbeitenSchueler_Click;
            // 
            // CmdRollbackDatabaseChanges
            // 
            CmdRollbackDatabaseChanges.Location = new Point(3, 881);
            CmdRollbackDatabaseChanges.Margin = new Padding(6);
            CmdRollbackDatabaseChanges.Name = "CmdRollbackDatabaseChanges";
            CmdRollbackDatabaseChanges.Size = new Size(321, 49);
            CmdRollbackDatabaseChanges.TabIndex = 4;
            CmdRollbackDatabaseChanges.Text = "Änderungen verwerfen";
            CmdRollbackDatabaseChanges.UseVisualStyleBackColor = true;
            CmdRollbackDatabaseChanges.Click += CmdRollbackDatabaseChanges_Click;
            // 
            // CmdLoeschenKlasse
            // 
            CmdLoeschenKlasse.Location = new Point(6, 130);
            CmdLoeschenKlasse.Margin = new Padding(6);
            CmdLoeschenKlasse.Name = "CmdLoeschenKlasse";
            CmdLoeschenKlasse.Size = new Size(321, 49);
            CmdLoeschenKlasse.TabIndex = 3;
            CmdLoeschenKlasse.Text = "Klasse löschen";
            CmdLoeschenKlasse.UseVisualStyleBackColor = true;
            CmdLoeschenKlasse.Click += CmdLoeschenKlasse_Click;
            // 
            // CmdSaveDatabase
            // 
            CmdSaveDatabase.Location = new Point(3, 819);
            CmdSaveDatabase.Margin = new Padding(6);
            CmdSaveDatabase.Name = "CmdSaveDatabase";
            CmdSaveDatabase.Size = new Size(321, 49);
            CmdSaveDatabase.TabIndex = 2;
            CmdSaveDatabase.Text = "Datenbank speichern";
            CmdSaveDatabase.UseVisualStyleBackColor = true;
            CmdSaveDatabase.Click += CmdSaveDatabase_Click;
            // 
            // CmdNeueKlasse
            // 
            CmdNeueKlasse.Location = new Point(6, 6);
            CmdNeueKlasse.Margin = new Padding(6);
            CmdNeueKlasse.Name = "CmdNeueKlasse";
            CmdNeueKlasse.Size = new Size(321, 49);
            CmdNeueKlasse.TabIndex = 1;
            CmdNeueKlasse.Text = "Neue Klasse";
            CmdNeueKlasse.UseVisualStyleBackColor = true;
            CmdNeueKlasse.Click += CmdNeueKlasse_Click;
            // 
            // Tab1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelButtons);
            Margin = new Padding(6);
            Name = "Tab1";
            Size = new Size(2210, 1440);
            Load += Tab1_Load;
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button CmdBearbeitenKlasse;
        private Panel panelButtons;
        private Button CmdNeueKlasse;
        private Button CmdRollbackDatabaseChanges;
        private Button CmdLoeschenKlasse;
        private Button CmdSaveDatabase;
        private Button CmdLoeschenLehrer;
        private Button CmdNeuerLehrer;
        private Button CmdBearbeitenLehrer;
        private Button CmdLoeschenSchueler;
        private Button CmdNeuerSchueler;
        private Button CmdBearbeitenSchueler;
    }
}
