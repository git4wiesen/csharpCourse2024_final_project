namespace SchulplanOrganisation.Ui.Tab1
{
    partial class Tab1KlassenTable
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
            grid = new DataGridView();
            lblTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            // grid
            // 
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.Location = new Point(26, 113);
            grid.Margin = new Padding(6, 6, 6, 6);
            grid.Name = "grid";
            grid.RowHeadersWidth = 82;
            grid.Size = new Size(1428, 766);
            grid.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(26, 29);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(114, 59);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Title";
            // 
            // ShowDatabaseTable
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblTitle);
            Controls.Add(grid);
            Margin = new Padding(6, 6, 6, 6);
            Name = "ShowDatabaseTable";
            Size = new Size(1560, 1009);
            Load += ShowDatabaseTable_Load;
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView grid;
        private Label lblTitle;
    }
}
