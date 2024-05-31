using SchulplanOrganisation.Ui.EditUi;

namespace SchulplanOrganisation.Ui.Tab1
{
    public partial class Tab1 : UserControl
    {
        public string? DbFile { get; set; }

        private Tab1KlassenTable? tableKlassen;
        private Tab1SchuelerTable? tableSchueler;
        private Tab1LehrerTable? tableLehrer;

        public Tab1()
        {
            InitializeComponent();
        }

        private void Tab1_Load(object sender, EventArgs e)
        {
            ShowKlassenTable();
            ShowSchuelerTable();
            ShowLehrerTable();

            float dpiX = CreateGraphics().DpiX;
            float dpiY = CreateGraphics().DpiY;
            panelButtons.Location = new Point(
                Width - panelButtons.Width - (int)(0.8 * dpiX),
                (int)(1 * dpiY)
            );
            panelButtons.Height = Height - (int)(1.2 * dpiY);

        }

        private void ShowKlassenTable()
        {
            if (DbFile is not string dbFile)
            {
                return;
            }
            tableKlassen = new(dbFile);
            tableKlassen.Height = Height;

            tableKlassen.Location = new Point(0, 0);
            tableKlassen.Name = "ShowKlasse";
            tableKlassen.TabIndex = 0;

            SuspendLayout();
            Controls.RemoveByKey("ShowKlasse");
            Controls.Add(tableKlassen);
            ResumeLayout();
        }

        private void ShowSchuelerTable()
        {
            if (
                this.tableKlassen is not Tab1KlassenTable showKlassen ||
                DbFile is not string dbFile
            )
            {
                return;
            }

            tableSchueler = new(dbFile);
            tableSchueler.Height = Height;

            float dpiX = CreateGraphics().DpiX;

            tableSchueler.Location = new Point(
                showKlassen.Location.X + showKlassen.Width + (int)(0.2 * dpiX),
                0
            );
            tableSchueler.Name = "ShowSchueler";
            tableSchueler.TabIndex = 0;

            SuspendLayout();
            Controls.RemoveByKey("ShowSchueler");
            Controls.Add(tableSchueler);
            ResumeLayout();
        }

        private void ShowLehrerTable()
        {
            if (
                this.tableSchueler is not Tab1SchuelerTable showSchueler ||
                DbFile is not string dbFile
            )
            {
                return;
            }

            this.tableLehrer = new(dbFile);
            this.tableLehrer.Height = Height;

            float dpiX = CreateGraphics().DpiX;

            this.tableLehrer.Location = new Point(
                showSchueler.Location.X + showSchueler.Width + (int)(0.2 * dpiX),
                0
            );
            this.tableLehrer.Name = "ShowLehrer";
            this.tableLehrer.TabIndex = 0;

            SuspendLayout();
            Controls.RemoveByKey("ShowLehrer");
            Controls.Add(this.tableLehrer);
            ResumeLayout();
        }

        private void CmdNeueKlasse_Click(object sender, EventArgs e)
        {
            tableKlassen?.StartNewRow();
        }

        private void CmdBearbeitenKlasse_Click(object sender, EventArgs e)
        {
            tableKlassen?.StartEditRow();
        }

        private void CmdLoeschenKlasse_Click(object sender, EventArgs e)
        {
            tableKlassen?.DeleteRow();
        }

        private void CmdNeuerSchueler_Click(object sender, EventArgs e)
        {
            tableSchueler?.StartNewRow();
        }

        private void CmdBearbeitenSchueler_Click(object sender, EventArgs e)
        {
            tableSchueler?.StartEditRow();
        }

        private void CmdLoeschenSchueler_Click(object sender, EventArgs e)
        {
            tableSchueler?.DeleteRow();
        }

        private void CmdNeuerLehrer_Click(object sender, EventArgs e)
        {
            tableLehrer?.StartNewRow();
        }

        private void CmdBearbeitenLehrer_Click(object sender, EventArgs e)
        {
            tableLehrer?.StartEditRow();
        }

        private void CmdLoeschenLehrer_Click(object sender, EventArgs e)
        {
            tableLehrer?.DeleteRow();
        }

        private void CmdSaveDatabase_Click(object sender, EventArgs e)
        {
            tableKlassen?.SaveChanges();
            tableSchueler?.SaveChanges();
            tableLehrer?.SaveChanges();
        }

        private void CmdRollbackDatabaseChanges_Click(object sender, EventArgs e)
        {
            tableKlassen?.RollbackChanges();
            tableSchueler?.RollbackChanges();
            tableLehrer?.RollbackChanges();
        }
    }
}
