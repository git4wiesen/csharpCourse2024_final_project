namespace SchulplanOrganisation.Ui
{
    partial class MainUi
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabs = new TabControl();
            tabSchuelerLehrer = new TabPage();
            tabKlassenStundenplan = new TabPage();
            tabs.SuspendLayout();
            SuspendLayout();
            // 
            // tabs
            // 
            tabs.Controls.Add(tabSchuelerLehrer);
            tabs.Controls.Add(tabKlassenStundenplan);
            tabs.Location = new Point(0, 1);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(935, 448);
            tabs.TabIndex = 0;
            // 
            // tabSchuelerLehrer
            // 
            tabSchuelerLehrer.Location = new Point(4, 24);
            tabSchuelerLehrer.Name = "tabSchuelerLehrer";
            tabSchuelerLehrer.Padding = new Padding(3);
            tabSchuelerLehrer.Size = new Size(927, 420);
            tabSchuelerLehrer.TabIndex = 0;
            tabSchuelerLehrer.Text = "Klassen / Schüler / Lehrer";
            tabSchuelerLehrer.UseVisualStyleBackColor = true;
            // 
            // tabKlassenStundenplan
            // 
            tabKlassenStundenplan.Location = new Point(4, 24);
            tabKlassenStundenplan.Name = "tabKlassenStundenplan";
            tabKlassenStundenplan.Padding = new Padding(3);
            tabKlassenStundenplan.Size = new Size(927, 420);
            tabKlassenStundenplan.TabIndex = 1;
            tabKlassenStundenplan.Text = "KlassenStundenplan";
            tabKlassenStundenplan.UseVisualStyleBackColor = true;
            // 
            // MainUi
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(934, 449);
            Controls.Add(tabs);
            Name = "MainUi";
            Text = "Schulorganisation";
            Load += MainUi_Load;
            LocationChanged += MainUi_LocationChanged;
            tabs.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabs;
        private TabPage tabSchuelerLehrer;
        private TabPage tabKlassenStundenplan;
    }
}
