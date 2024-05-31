using SchulplanOrganisation.Ui.Tab1;
using SchulplanOrganisation.Ui.Tab2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SchulplanOrganisation.Ui
{
    public partial class MainUi : Form
    {
        private readonly string dbFile = @"..\..\..\..\schulplan_db_modify.accdb";

        private Tab1.Tab1? tab1;
        private Tab2KlassenStundenplan? showKlassenStundenplan;

        public MainUi()
        {
            InitializeComponent();
        }

        private void MainUi_Load(object sender, EventArgs e)
        {
            ResizeForm();
            ShowTab1();
            ShowKlassenStundenplan();
        }

        private void ResizeForm()
        {
            Screen screen = Screen.AllScreens[0];
            Size screenSize = screen.WorkingArea.Size;

            Location = new Point(0, 0);
            Size = new Size(screenSize.Width, screenSize.Height);

            tabs.Width = Width;
            tabs.Height = Height;
        }

        private void ShowTab1()
        {
            tab1 = new();
            tab1.DbFile = dbFile;
            tab1.Height = tabSchuelerLehrer.Height;
            tab1.Width = tabSchuelerLehrer.Width;
            tab1.Location = new Point(0, 0);
            tab1.Name = "Tab1";
            tab1.TabIndex = 0;

            tabSchuelerLehrer.SuspendLayout();
            tabSchuelerLehrer.Controls.RemoveByKey("Tab1");
            tabSchuelerLehrer.Controls.Add(tab1);
            tabSchuelerLehrer.ResumeLayout();
        }


        private void ShowKlassenStundenplan()
        {
            showKlassenStundenplan = new(dbFile);
            showKlassenStundenplan.Height = this.tabSchuelerLehrer.Height;
            showKlassenStundenplan.Width = this.tabSchuelerLehrer.Width;

            showKlassenStundenplan.Location = new Point(0, 0);
            showKlassenStundenplan.Name = "KlasseStundenplan";
            showKlassenStundenplan.TabIndex = 0;

            this.tabKlassenStundenplan.SuspendLayout();
            this.tabKlassenStundenplan.Controls.RemoveByKey("KlasseStundenplan");
            this.tabKlassenStundenplan.Controls.Add(showKlassenStundenplan);
            this.tabKlassenStundenplan.ResumeLayout();
        }

        private void MainUi_LocationChanged(object sender, EventArgs e)
        {
            //this.tabSchuelerLehrer.MainUiLocation = Location;
        }
    }
}
