using SchulplanOrganisation.Daten;
using SchulplanOrganisation.MyOledb;
using SchulplanOrganisation.Ui.EditUi;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace SchulplanOrganisation.Ui.Tab1
{
    public partial class Tab1SchuelerTable : UserControl
    {
        private readonly string dbFile;
        private readonly string personTable = "Person";
        private readonly string schuelerTable = "Schueler";
        private readonly string klasseSortString = "'Nachname' ASC, 'Erster Vorname' ASC, 'Andere Vornamen' ASC, 'Geburtstag' ASC";
        private readonly string[] klasseColumns = ["Schueler.ID", "Person.ID", "Person.nachname", "Person.ersterVorname", "Person.andereVornamen", "Person.geburtstag"];
        private readonly string[] klasseColumnHeaderText = ["SID", "PID", "Nachname", "Erster Vorname", "Andere Vornamen", "Geburtstag"];

        private OleDbConnection? conn = null;
        private OleDbDataAdapter? dataAdapter = null;
        private DataTable? dataTable = null;
        private DataView? dataView = null;

        public Tab1SchuelerTable(string dbFile)
        {
            InitializeComponent();
            this.dbFile = dbFile;

            lblTitle.Text = "Schueler";

            BackColor = Color.Red;
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

        private void InternalEditRow(DataRowView drv)
        {
            int max = 10;
            var parent = Parent;
            while (parent != null && parent is not Form && max > 0)
            {
                parent = parent.Parent;
                max--;
            }

            EditSchuelerLehrer editSchuelerLehrer = new();
            editSchuelerLehrer.StartEditRow(
                "Schüler",
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

            if (parent is not Form mainForm)
            {
                return;
            }

            string andereVornamen = DBNull.Value != drv["'Andere Vornamen'"] ? (string)drv["'Andere Vornamen'"] : "";
            if (MessageBox.Show(
                mainForm,
                $"Möchtest du den Schüler '{drv["'Nachname'"]}, {drv["'Erster Vorname'"]}{(!string.IsNullOrWhiteSpace(andereVornamen) ? $" {andereVornamen}" : "")}' wirklich löschen?",
                "Schüler löschen?",
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

            if (this.conn is not OleDbConnection conn)
            {
                return;
            }

            try
            {
                try
                {
                    conn.Open();

                    OleDbTransaction transactionChanges = conn.BeginTransaction();
                    try
                    {
                        var changed = false;
                        foreach (DataRow row in changes.Rows)
                        {

                            switch (row.RowState)
                            {
                                case DataRowState.Added:
                                    {
                                        using OleDbCommand commandUpdatePerson = conn.CreateCommand();
                                        commandUpdatePerson.Transaction = transactionChanges;
                                        commandUpdatePerson.CommandText = "INSERT INTO Person " +
                                            "(nachname, ersterVorname, andereVornamen, geburtstag) VALUES " +
                                            "(@nachname, @ersterVorname, @andereVornamen, @geburtstag)";
                                        commandUpdatePerson.Parameters.Add("@nachname", OleDbType.VarChar).Value = row["'Nachname'", DataRowVersion.Current];
                                        commandUpdatePerson.Parameters.Add("@ersterVorname", OleDbType.VarChar).Value = row["'Erster Vorname'", DataRowVersion.Current];
                                        commandUpdatePerson.Parameters.Add("@andereVornamen", OleDbType.VarChar).Value = row["'Andere Vornamen'", DataRowVersion.Current];
                                        commandUpdatePerson.Parameters.Add("@geburtstag", OleDbType.DBDate).Value = row["'Geburtstag'", DataRowVersion.Current];
                                        commandUpdatePerson.ExecuteNonQuery();

                                        using OleDbCommand commandGetPersonId = conn.CreateCommandSelectIdAfterInsert(transaction: transactionChanges);
                                        if (commandGetPersonId.ExecuteScalar().ConvertToInt32() is not int pid)
                                        {
                                            throw new Exception("Retrieving person id failed");
                                        }

                                        using OleDbCommand commandUpdateSchueler = conn.CreateCommand();
                                        commandUpdateSchueler.Transaction = transactionChanges;
                                        commandUpdateSchueler.CommandText = "INSERT INTO Schueler (Person) VALUES (@PID)";
                                        commandUpdateSchueler.Parameters.Add("@PID", OleDbType.Integer).Value = pid;
                                        commandUpdateSchueler.ExecuteNonQuery();

                                        changed = true;
                                        break;
                                    }
                                case DataRowState.Modified:
                                    {
                                        using OleDbCommand commandUpdatePerson = conn.CreateCommand();
                                        commandUpdatePerson.Transaction = transactionChanges;
                                        commandUpdatePerson.CommandText = "UPDATE Person SET " +
                                            "nachname = @nachname, " +
                                            "ersterVorname = @ersterVorname, " +
                                            "andereVornamen = @andereVornamen, " +
                                            "geburtstag = @geburtstag " +
                                            "WHERE ID = @PID";
                                        commandUpdatePerson.Parameters.Add("@nachname", OleDbType.VarChar).Value = row["'Nachname'", DataRowVersion.Current];
                                        commandUpdatePerson.Parameters.Add("@ersterVorname", OleDbType.VarChar).Value = row["'Erster Vorname'", DataRowVersion.Current];
                                        commandUpdatePerson.Parameters.Add("@andereVornamen", OleDbType.VarChar).Value = row["'Andere Vornamen'", DataRowVersion.Current];
                                        commandUpdatePerson.Parameters.Add("@geburtstag", OleDbType.DBDate).Value = row["'Geburtstag'", DataRowVersion.Current];
                                        commandUpdatePerson.Parameters.Add("@PID", OleDbType.Integer).Value = row["'PID'", DataRowVersion.Current];
                                        commandUpdatePerson.ExecuteNonQuery();
                                        changed = true;
                                        break;
                                    }
                                case DataRowState.Deleted:
                                    {
                                        using OleDbCommand commandDeleteSchueler = conn.CreateCommand();
                                        commandDeleteSchueler.Transaction = transactionChanges;
                                        commandDeleteSchueler.CommandText = "DELETE FROM Schueler WHERE ID = @SID";
                                        commandDeleteSchueler.Parameters.Add("@SID", OleDbType.Integer).Value = row["'SID'", DataRowVersion.Original];
                                        commandDeleteSchueler.ExecuteNonQuery();

                                        using OleDbCommand commandDeletePerson = conn.CreateCommand();
                                        commandDeletePerson.Transaction = transactionChanges;
                                        commandDeletePerson.CommandText = "DELETE FROM Person WHERE ID = @PID";
                                        commandDeletePerson.Parameters.Add("@PID", OleDbType.Integer).Value = row["'PID'", DataRowVersion.Original];
                                        commandDeletePerson.ExecuteNonQuery();
                                        changed = true;
                                        break;
                                    }
                                default:
                                    continue;
                            }
                        }
                        if (changed)
                        {
                            transactionChanges.Commit();
                            dataTable?.AcceptChanges();
                        }
                        else
                        {
                            throw new Exception("Could not apply changes to the database");
                        }
                    }
                    catch (Exception)
                    {
                        transactionChanges.Rollback();
                        throw;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
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
            if (dataTable is not DataTable dt)
            {
                return;
            }
            dt.RejectChanges();
        }

        private void LoadDatabase()
        {
            string dbFile = this.dbFile.Trim();
            string tableName1 = personTable;
            string tableName2 = schuelerTable;

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

            if (ShowDatabaseSelectionParametersOnError(dbFile: dbFile))
            {
                return;
            }

            grid.Rows.Clear();
            grid.Columns.Clear();
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

            if (this.conn is not OleDbConnection conn)
            {
                return;
            }

            try
            {
                OleDbCommand selectCommand = conn.CreateCommand();
                selectCommand.CommandText = string.Format(
                    "SELECT {0} FROM {1}, {2} WHERE Person.ID = Schueler.Person",
                    string.Join(", ", columnInfo.Select(info => $"{info.ColumnName} AS '{info.ColumnTitleText}'")),
                    tableName1, tableName2
                );
                OleDbDataAdapter dataAdapter = this.dataAdapter = new OleDbDataAdapter(selectCommand);

                DataView dv;
                conn.Open();
                try
                {
                    DataTable dt = dataTable = new();
                    dataAdapter.Fill(dt);
                    dt.TableName = "schueler";

                    dv = dataView = new(dt, null, klasseSortString, DataViewRowState.CurrentRows);

                    grid.DataMember = "schueler";
                    grid.DataSource = dv;

                    grid.Columns["'SID'"].Visible = false;
                    grid.Columns["'PID'"].Visible = false;

                    grid.Columns["'Nachname'"].HeaderText = "Nachname";
                    grid.Columns["'Erster Vorname'"].HeaderText = "Erster Vorname";
                    grid.Columns["'Andere Vornamen'"].HeaderText = "Andere Vornamen";
                    grid.Columns["'Geburtstag'"].HeaderText = "Geburtstag";

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
            string dbFile
        )
        {
            bool dbFileSet = !string.IsNullOrWhiteSpace(dbFile);
            bool dbFileExists = File.Exists(dbFile);
            if (dbFileSet && dbFileExists)
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
                    HeaderText = "Text",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                }
            );

            int rId = grid.Rows.Add();
            grid.Rows[rId].Cells[0].Value = "Error";
            grid.Rows[rId].Cells[1].Value = "No data to show";

            rId = grid.Rows.Add();
            grid.Rows[rId].Cells[0].Value = "File";
            grid.Rows[rId].Cells[1].Value = 0 switch
            {
                int when !dbFileSet => "<not set>",
                int when !dbFileExists => "<db file does not exist>",
                _ => dbFile
            };

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
                Math.Min(
                    newGridHeight,
                    Height - grid.Location.Y - (int)(0.5 * DpiX)
                )
            );

            Width = grid.Location.X + grid.Width + (int)(0.2 * DpiX);
        }
    }

}
