using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace SchulplanOrganisation.Ui.Tab2
{
    public partial class Tab2KlassenTable : UserControl
    {
        private readonly string dbFile;
        private readonly string klasseTable = "Klasse";
        private readonly string klassePrimaryColumns = "ID";
        private readonly string klasseSortString = "Jahrgang ASC, Stufe ASC, Name ASC";
        private readonly string[] klasseColumns = ["ID", "jahrgang", "klassenStufe", "klassenName"];
        private readonly string[] klasseColumnHeaderText = ["ID", "Jahrgang", "Stufe", "Name"];

        private DataView? dataView = null;

        public Tab2KlassenTable(string dbFile)
        {
            InitializeComponent();
            this.dbFile = dbFile;
        }

        public delegate void SelectedRowChangedHandler(int klassenId, int jahrgang, int klassenStufe, string klassenName);

        public event SelectedRowChangedHandler SelectedRowChanged
        {
            add => grid.SelectionChanged += (_, _) => OnRowChanged(value);
            remove => throw new NotImplementedException(); //grid.SelectionChanged -= value;
        }

        private void OnRowChanged(SelectedRowChangedHandler rowChangedHandler)
        {
            if ((grid.SelectedRows.Count > 0 ? grid.SelectedRows[0] : null) is not DataGridViewRow selectedRow)
            {
                return;
            }

            int klassenId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
            int jahrgang = Convert.ToInt32(selectedRow.Cells["Jahrgang"].Value);
            int klassenStufe = Convert.ToInt32(selectedRow.Cells["Stufe"].Value);
            string klassenName = (string)selectedRow.Cells["Name"].Value;
            rowChangedHandler(
                klassenId: klassenId,
                jahrgang: jahrgang,
                klassenStufe: klassenStufe,
                klassenName: klassenName
            );
        }

        private void ShowDatabaseTable_Load(object sender, EventArgs e)
        {
            LoadDatabase();
        }

        public void SelectFirstEntry()
        {
            if (grid.Rows.Count > 0)
            {
                grid.Rows[0].Selected = true;
            }
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

            try
            {
                using OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
                using OleDbCommand command = conn.CreateCommand();
                command.CommandText = string.Format(
                    "SELECT {0} FROM {1}",
                    string.Join(", ", columnInfo.Select(info => $"{info.ColumnName} AS {info.ColumnTitleText}")),
                    tableName
                );
                using OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);

                conn.Open();
                try
                {
                    DataTable dt = new();
                    dataAdapter.Fill(dt);
                    dt.PrimaryKey = [dt.Columns[klassePrimaryColumns] ?? throw new Exception("no primary key")];

                    DataView dv = this.dataView = new(dt, null, klasseSortString, DataViewRowState.CurrentRows);

                    grid.DataMember = tableName;
                    grid.DataSource = dv;

                    grid.Columns[klassePrimaryColumns].Visible = false;

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

            grid.Rows.Clear();
            grid.Columns.Clear();
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
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

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

            int newGridHeight = grid.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + grid.ColumnHeadersHeight
                + (int)(fix * (1 + grid.RowCount));
            if (((int)(grid.ScrollBars) & ((int)ScrollBars.Horizontal)) != 0)
            {
                newColumnWidth += SystemInformation.HorizontalScrollBarHeight;
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
