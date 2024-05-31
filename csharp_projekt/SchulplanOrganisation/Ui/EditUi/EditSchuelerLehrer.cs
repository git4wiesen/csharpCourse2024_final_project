using System.Data;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace SchulplanOrganisation.Ui.EditUi
{
    public partial class EditSchuelerLehrer : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        private const int MAX_CHARS_NACHNAME = 64;
        private const int MAX_CHARS_ERSTER_VORNAME = 64;
        private const int MAX_CHARS_ANDERE_VORNAME = 255;

        private Point? ParentLocation { get; set; }
        private Size? ParentSize { get; set; }

        public EditSchuelerLehrer()
        {
            InitializeComponent();
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;

            txtNachname.MaxLength = MAX_CHARS_NACHNAME;
            txtErsterVorname.MaxLength = MAX_CHARS_ERSTER_VORNAME;
            txtAndereVornamen.MaxLength = MAX_CHARS_ANDERE_VORNAME;

            dateGeburtstag.Format = DateTimePickerFormat.Custom;
            dateGeburtstag.CustomFormat = $"                          {DateTimeFormatInfo.GetInstance(CultureInfo.GetCultureInfo("de")).LongDatePattern}";
            dateGeburtstag.Value = new DateTime(2015, 1, 1);
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
            string personType,
            Point? parentLocation,
            Size? parentSize,
            DataRowView drv
        )
        {
            Text = $"{personType} editieren";

            ParentLocation = parentLocation;
            ParentSize = parentSize;

            try
            {
                string nachname = drv["'Nachname'"] != DBNull.Value ? (string)drv["'Nachname'"] : "";
                string ersterVorname = drv["'Erster Vorname'"] != DBNull.Value ? (string)drv["'Erster Vorname'"] : "";
                string andereVornamen = drv["'Andere Vornamen'"] != DBNull.Value ? (string)drv["'Andere Vornamen'"] : "";
                DateTime geburtstag = drv["'Geburtstag'"] != DBNull.Value ? (DateTime)drv["'Geburtstag'"] : new DateTime(2015, 1, 1);

                txtNachname.Text = nachname;
                txtErsterVorname.Text = ersterVorname;
                txtAndereVornamen.Text = andereVornamen;
                dateGeburtstag.Value = geburtstag;
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
                    drv["'Nachname'"] = txtNachname.Text.Trim();
                    drv["'Erster Vorname'"] = txtErsterVorname.Text.Trim();
                    drv["'Andere Vornamen'"] = txtAndereVornamen.Text.Trim();
                    drv["'Geburtstag'"] = dateGeburtstag.Value.Date;

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
