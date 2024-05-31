using SchulplanOrganisation.Daten;
using System.Data;

namespace SchulplanOrganisation.Ui.EditUi
{
    public partial class EditKlasse : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        private Point? ParentLocation { get; set; }
        private Size? ParentSize { get; set; }

        public EditKlasse()
        {
            InitializeComponent();
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;

            numBuchstabe.Minimum = 0;
            numBuchstabe.Maximum = 'z' - 'a';
        }

        /**
         * Hide Close button (source: https://stackoverflow.com/a/7301828)
         */
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void EditKlasse_Load(object sender, EventArgs e)
        {
            if (
                ParentSize is Size parentSize &&
                ParentLocation is Point parentLocation)
            {
                Location = new Point(
                    parentLocation.X + (parentSize.Width - Width) / 2,
                    parentLocation.Y + (parentSize.Height - Height) / 2
                );
            }
            else
            {
                Screen screen = Screen.AllScreens[0];
                Size screenSize = screen.WorkingArea.Size;
                Location = new Point(
                    (screenSize.Width - Width) / 2,
                    (screenSize.Height - Height) / 2
                );
            }
        }

        public void StartEditRow(
            Point? parentLocation,
            Size? parentSize,
            DataRowView drv
        )
        {
            ParentLocation = parentLocation;
            ParentSize = parentSize;

            try
            {
                int jahrgang = drv["Jahrgang"].ConvertToInt32() ?? DateTime.Today.Year;
                int klassenStufe = drv["Stufe"].ConvertToInt32() ?? 5;

                string strName = (DBNull.Value != drv["Name"] ? (string?)drv["Name"] : null) ?? "";
                if (string.IsNullOrWhiteSpace(strName))
                {
                    strName = "5a";
                }

                char klassenBuchstabe = strName[^1];
                if (strName != $"{klassenStufe}{klassenBuchstabe}" || klassenBuchstabe < 'a' || 'z' < klassenBuchstabe)
                {
                    drv.CancelEdit();
                    return;
                }

                numJahrgang.Value = jahrgang;
                numStufe.Value = klassenStufe;
                numBuchstabe.Value = klassenBuchstabe - 'a';
            }
            catch (Exception)
            {
                drv.CancelEdit();
                return;
            }

            if (ShowDialog() == DialogResult.OK)
            {
                try
                {
                    drv.BeginEdit();

                    int jahrgang = Convert.ToInt32(numJahrgang.Value);
                    int klassenStufe = Convert.ToInt32(numStufe.Value);
                    char klassenBuchstabe = (char)('a' + Math.Abs(Convert.ToInt32(numBuchstabe.Value)) % ('z' - 'a' + 1));

                    drv["Jahrgang"] = $"{jahrgang}";
                    drv["Stufe"] = $"{klassenStufe}";
                    drv["Name"] = $"{klassenStufe}{klassenBuchstabe}";
                }
                catch
                {
                    drv.CancelEdit();
                }
                finally
                {
                    drv.EndEdit();
                }
            }
            else
            {
                drv.CancelEdit();
            }
        }

        private void CmdOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
