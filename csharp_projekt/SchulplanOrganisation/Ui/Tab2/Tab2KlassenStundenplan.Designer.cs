namespace SchulplanOrganisation.Ui.Tab2
{
    partial class Tab2KlassenStundenplan
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
            gridKlassenstundenPlan = new DataGridView();
            lblTitle = new Label();
            lblKlasseValue = new Label();
            panelButtons = new Panel();
            CmdRollbackDatabaseChanges = new Button();
            CmdSaveDatabase = new Button();
            CmdBearbeitenEintrag = new Button();
            CmdLoeschenEintrag = new Button();
            ((System.ComponentModel.ISupportInitialize)gridKlassenstundenPlan).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // gridKlassenstundenPlan
            // 
            gridKlassenstundenPlan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridKlassenstundenPlan.Location = new Point(418, 93);
            gridKlassenstundenPlan.Name = "gridKlassenstundenPlan";
            gridKlassenstundenPlan.RowHeadersWidth = 82;
            gridKlassenstundenPlan.Size = new Size(1261, 747);
            gridKlassenstundenPlan.TabIndex = 0;
            gridKlassenstundenPlan.DataBindingComplete += Grid_DataBindingComplete;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(14, 14);
            lblTitle.Margin = new Padding(2, 0, 2, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(59, 30);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Title";
            // 
            // lblKlasseValue
            // 
            lblKlasseValue.BackColor = SystemColors.Window;
            lblKlasseValue.BorderStyle = BorderStyle.FixedSingle;
            lblKlasseValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblKlasseValue.Location = new Point(418, 51);
            lblKlasseValue.Name = "lblKlasseValue";
            lblKlasseValue.Size = new Size(751, 34);
            lblKlasseValue.TabIndex = 3;
            lblKlasseValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(CmdRollbackDatabaseChanges);
            panelButtons.Controls.Add(CmdSaveDatabase);
            panelButtons.Controls.Add(CmdBearbeitenEintrag);
            panelButtons.Controls.Add(CmdLoeschenEintrag);
            panelButtons.Location = new Point(1508, 51);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(180, 669);
            panelButtons.TabIndex = 4;
            // 
            // CmdRollbackDatabaseChanges
            // 
            CmdRollbackDatabaseChanges.Location = new Point(3, 142);
            CmdRollbackDatabaseChanges.Name = "CmdRollbackDatabaseChanges";
            CmdRollbackDatabaseChanges.Size = new Size(173, 23);
            CmdRollbackDatabaseChanges.TabIndex = 4;
            CmdRollbackDatabaseChanges.Text = "Änderungen verwerfen";
            CmdRollbackDatabaseChanges.UseVisualStyleBackColor = true;
            CmdRollbackDatabaseChanges.Click += CmdRollbackDatabaseChanges_Click;
            // 
            // CmdSaveDatabase
            // 
            CmdSaveDatabase.Location = new Point(3, 113);
            CmdSaveDatabase.Name = "CmdSaveDatabase";
            CmdSaveDatabase.Size = new Size(173, 23);
            CmdSaveDatabase.TabIndex = 2;
            CmdSaveDatabase.Text = "Datenbank speichern";
            CmdSaveDatabase.UseVisualStyleBackColor = true;
            CmdSaveDatabase.Click += CmdSaveDatabase_Click;
            // 
            // CmdBearbeitenEintrag
            // 
            CmdBearbeitenEintrag.Location = new Point(3, 3);
            CmdBearbeitenEintrag.Name = "CmdBearbeitenEintrag";
            CmdBearbeitenEintrag.Size = new Size(173, 23);
            CmdBearbeitenEintrag.TabIndex = 1;
            CmdBearbeitenEintrag.Text = "Bearbeite Eintrag";
            CmdBearbeitenEintrag.UseVisualStyleBackColor = true;
            CmdBearbeitenEintrag.Click += CmdBearbeitenEintrag_Click;
            // 
            // CmdLoeschenEintrag
            // 
            CmdLoeschenEintrag.Location = new Point(3, 32);
            CmdLoeschenEintrag.Name = "CmdLoeschenEintrag";
            CmdLoeschenEintrag.Size = new Size(173, 23);
            CmdLoeschenEintrag.TabIndex = 0;
            CmdLoeschenEintrag.Text = "Lösche Eintrag";
            CmdLoeschenEintrag.UseVisualStyleBackColor = true;
            CmdLoeschenEintrag.Click += CmdLoeschenEintrag_Click;
            // 
            // Tab2KlassenStundenplan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelButtons);
            Controls.Add(lblKlasseValue);
            Controls.Add(lblTitle);
            Controls.Add(gridKlassenstundenPlan);
            Name = "Tab2KlassenStundenplan";
            Size = new Size(1723, 860);
            Load += ShowDatabaseTable_Load;
            ((System.ComponentModel.ISupportInitialize)gridKlassenstundenPlan).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gridKlassenstundenPlan;
        private Label lblTitle;
        private Label lblKlasseValue;
        private Panel panelButtons;
        private Button CmdRollbackDatabaseChanges;
        private Button CmdLoeschenKlasse;
        private Button CmdSaveDatabase;
        private Button CmdBearbeitenEintrag;
        private Button CmdLoeschenEintrag;
    }
}
