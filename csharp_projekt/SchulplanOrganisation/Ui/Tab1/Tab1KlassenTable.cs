using SchulplanOrganisation.Ui.EditUi;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace SchulplanOrganisation.Ui.Tab1
{
    public partial class Tab1KlassenTable : UserControl
    {
        private readonly string dbFile;
        private readonly string klasseTable = "Klasse";
        private readonly string klasseSortString = "Jahrgang ASC, Stufe ASC, Name ASC";
        private readonly string[] klasseColumns = ["ID", "jahrgang", "klassenStufe", "klassenName"];
        private readonly string[] klasseColumnHeaderText = ["ID", "Jahrgang", "Stufe", "Name"];

        private OleDbConnection? conn = null;
        private OleDbDataAdapter? dataAdapter = null;
        private DataTable? dataTable = null;
        private DataView? dataView = null;

        public Tab1KlassenTable(string dbFile)
        {
            InitializeComponent();
            this.dbFile = dbFile;

            lblTitle.Text = "Klassen";
            BackColor = Color.Blue;
        }

        private void ShowDatabaseTable_Load(object sender, EventArgs e)
        {
            conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
            LoadDatabase();
        }

        public void StartNewRow()
        {
            if (
                dataView is not DataView dv
            )
            {
                return;
            }
            DataRowView drv = dv.AddNew();
            InternalEditRow(drv);
        }

        public void StartEditRow()
        {
            int rowIndex = grid.CurrentRow.Index;
            if (
                dataView is not DataView dv ||
                rowIndex < 0 ||
                dv[rowIndex] is not DataRowView drv
            )
            {
                return;
            }
            InternalEditRow(drv);
        }

        private void InternalEditRow(DataRowView drv) {
            int max = 10;
            var parent = Parent;
            while (parent != null && parent is not Form && max > 0)
            {
                parent = parent.Parent;
                max--;
            }

            EditKlasse editKlasse = new();
            editKlasse.StartEditRow(
                parentLocation: parent?.Location,
                parentSize: parent?.Size,
                drv
            );
        }

        public void DeleteRow()
        {
            int rowIndex = grid.CurrentRow.Index;
            if (
                dataView is not DataView dv ||
                rowIndex < 0 ||
                dv[rowIndex] is not DataRowView drv
            )
            {
                return;
            }

            int max = 10;
            var parent = Parent;
            while (parent != null && parent is not Form && max > 0)
            {
                parent = parent.Parent;
                max--;
            }

            if (parent is not Form mainForm) {
                return;
            }

            if (MessageBox.Show(
                mainForm,
                $"Möchtest du die Klasse {drv["Name"]} der Klassenstufe {drv["Stufe"]} aus dem Jahrgang {drv["Jahrgang"]} wirklich löschen?",
                "Klasse löschen?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes)
            {
                drv.Delete();
            }
        }

        public void SaveChanges()
        {
            if (dataTable?.GetChanges() is not DataTable changes || this.dataAdapter is not OleDbDataAdapter dataAdapter)
            {
                return;
            }

            int max = 10;
            var parent = Parent;
            while (parent != null && parent is not Form && max > 0)
            {
                parent = parent.Parent;
                max--;
            }

            if (parent is not Form mainForm)
            {
                return;
            }

            try
            {
                try
                {
                    conn?.Open();
                    dataAdapter.Update(changes);
                    dataTable?.AcceptChanges();
                }
                finally {
                    conn?.Close();
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show(
                    mainForm,
                    ex.Message,
                    $"Fehler - {ex.GetType().Name}",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                dataTable?.RejectChanges();
            }

        }

        public void RollbackChanges()
        {
            if (dataTable is not DataTable dt) {
                return;
            }
            dt.RejectChanges();
        }

        private void LoadDatabase()
        {
            string dbFile = this.dbFile.Trim();
            string tableName = klasseTable;

            string[] columnNames = klasseColumns;
            string[] columnTitleTexts = klasseColumnHeaderText;
            if (columnTitleTexts.Length < columnNames.Length)
            {
                int oldLength = columnTitleTexts.Length;
                Array.Resize(ref columnTitleTexts, columnNames.Length);
                Array.Fill(columnTitleTexts, "", oldLength, columnTitleTexts.Length - oldLength);
            }

            (string ColumnName, string ColumnTitleText)[] columnInfo = columnNames.Zip(columnTitleTexts)
                .Where(c => !string.IsNullOrWhiteSpace(c.First))
                .Select(c =>
                {
                    string columnName = c.First.Trim();
                    string columnTitle = !string.IsNullOrWhiteSpace(c.Second) ? c.Second.Trim() : columnName;

                    // https://www.meziantou.net/convert-a-char-to-upper-or-lower-case.htm
                    var result = Rune.DecodeFromUtf16(columnTitle, out var rune, out var charsConsumed);
                    if (result == System.Buffers.OperationStatus.Done && !Rune.IsUpper(rune))
                    {
                        columnTitle = Rune.ToUpperInvariant(rune) + columnTitle[charsConsumed..];
                    }
                    return (columnName, columnTitle);
                })
                .ToArray();

            if (ShowDatabaseSelectionParametersOnError(dbFile: dbFile, tableName: tableName, columnInfo: columnInfo))
            {
                return;
            }

            grid.DataSource = null;
            grid.DataMember = null;

            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.AllowDrop = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToResizeColumns = false;
            grid.AllowUserToOrderColumns = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = true;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.ScrollBars = ScrollBars.Vertical;

            if (this.conn is not OleDbConnection conn) {
                return;
            }

            try
            {
                OleDbCommand selectCommand = conn.CreateCommand();
                selectCommand.CommandText = string.Format(
                    "SELECT {0} FROM {1}",
                    string.Join(", ", columnInfo.Select(info => $"{info.ColumnName} AS {info.ColumnTitleText}")),
                    tableName
                );
                OleDbDataAdapter dataAdapter = this.dataAdapter = new OleDbDataAdapter(selectCommand);
                _ = new OleDbCommandBuilder(dataAdapter);

                DataView dv;
                conn.Open();
                try
                {
                    DataTable dt = dataTable = new();
                    dataAdapter.Fill(dt);

                    dataView = dv = new(dt, null, klasseSortString, DataViewRowState.CurrentRows);

                    grid.DataMember = tableName;
                    grid.DataSource = dv;

                    grid.Columns["ID"].Visible = false;

                    ResizeGrid();
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ShowDatabaseSelectionParametersOnError(
            string dbFile,
            string tableName,
            (string, string)[] columnInfo
        )
        {
            bool dbFileSet = !string.IsNullOrWhiteSpace(dbFile);
            bool dbFileExists = File.Exists(dbFile);
            bool tableNameSet = !string.IsNullOrWhiteSpace(tableName);
            bool columnsSet = columnInfo.Length != 0;
            if (dbFileSet && dbFileExists && tableNameSet && columnsSet)
            {
                return false;
            }

            dbFile = Path.GetFullPath(dbFile);

            grid.DataSource = null;
            grid.DataMember = null;

            grid.AllowDrop = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToResizeColumns = false;
            grid.AllowUserToOrderColumns = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = true;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.ScrollBars = ScrollBars.None;

            DataGridViewTextBoxCell templateCell = new();

            grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            grid.Columns.Add(
                new DataGridViewColumn(templateCell)
                {
                    HeaderText = "Type",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                }
            );
            grid.Columns.Add(
                new DataGridViewColumn(templateCell)
                {
                    HeaderText = columnInfo.Length != 0 ? "Columnname" : "Text",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                }
            );
            if (columnInfo.Length != 0)
            {
                grid.Columns.Add(
                    new DataGridViewColumn(templateCell)
                    {
                        HeaderText = "Text",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    }
                );
            }

            int textColumn = columnInfo.Length != 0 ? 2 : 1;

            int rId = grid.Rows.Add();
            grid.Rows[rId].Cells[0].Value = "Error";
            grid.Rows[rId].Cells[textColumn].Value = "No data to show";

            rId = grid.Rows.Add();
            grid.Rows[rId].Cells[0].Value = "File";
            grid.Rows[rId].Cells[textColumn].Value = 0 switch
            {
                int when !dbFileSet => "<not set>",
                int when !dbFileExists => "<db file does not exist>",
                _ => dbFile
            };

            rId = grid.Rows.Add();
            grid.Rows[rId].Cells[0].Value = "Table";
            grid.Rows[rId].Cells[textColumn].Value = tableNameSet ? tableName : "<not set>";

            if (columnInfo.Length == 0)
            {
                rId = grid.Rows.Add();

                grid.Rows[rId].Cells[0].Value = "Columns";
                grid.Rows[rId].Cells[1].Value = "<not set>";
            }
            else
            {
                int i = 1;
                foreach ((string ColumnName, string ColumnTitleText) in columnInfo)
                {
                    rId = grid.Rows.Add();
                    grid.Rows[rId].Cells[0].Value = $"Column {i++}";
                    grid.Rows[rId].Cells[1].Value = ColumnName;
                    grid.Rows[rId].Cells[2].Value = ColumnTitleText;
                }
            }

            ResizeGrid();

            float fix = (int)(0.04 * CreateGraphics().DpiY);
            int rowHeight = (int)(((float)grid.Height) / grid.Rows.Count - fix);
            foreach (DataGridViewRow row in grid.Rows)
            {
                row.Height = rowHeight;
                row.DividerHeight = 0;
            }

            grid.Columns[grid.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.FirstDisplayedScrollingRowIndex = 0;
            return true;
        }

        private void ResizeGrid()
        {
            if (grid.Rows.Count == 0 && grid.Columns.Count == 0)
            {
                return;
            }

            float DpiX = CreateGraphics().DpiX;
            float fix = 0.02f * DpiX;

            int newColumnWidth = grid.Columns.GetColumnsWidth(DataGridViewElementStates.Visible)
                + (int)(fix * grid.ColumnCount);
            if (((int)(grid.ScrollBars) & ((int)ScrollBars.Vertical)) != 0)
            {
                newColumnWidth += SystemInformation.VerticalScrollBarWidth;
            }

            grid.Size = new Size(
                Math.Min(
                    newColumnWidth,
                    Width - grid.Location.X - (int)(0.5 * DpiX)
                ),
                Height - grid.Location.Y - (int)(0.5 * DpiX)
            );

            Width = grid.Width + (int)(0.2 * DpiX);
        }
    }

}
