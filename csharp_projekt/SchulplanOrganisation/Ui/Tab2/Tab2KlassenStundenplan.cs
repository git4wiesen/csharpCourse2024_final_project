using SchulplanOrganisation.Daten;
using SchulplanOrganisation.Ui.EditUi;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace SchulplanOrganisation.Ui.Tab2
{
    public partial class Tab2KlassenStundenplan : UserControl
    {
        private readonly string dbFile;

        private Tab2KlassenTable? tab2KlassenTable;

        private OleDbConnection? conn = null;
        private OleDbDataAdapter? dataAdapter = null;
        private DataTable? dataTable = null;
        private DataView? dataView = null;

        public Tab2KlassenStundenplan(
            string dbFile
        )
        {
            InitializeComponent();
            this.dbFile = dbFile;

            lblTitle.Text = "Klassen Stundenplan";
            BackColor = Color.Green;
        }

        public int? KlassenId { get; private set; }


        private void ShowDatabaseTable_Load(object sender, EventArgs e)
        {
            ShowKlassenTable();
            ShowKlassenStundenplan();

            float dpiX = CreateGraphics().DpiX;
            float dpiY = CreateGraphics().DpiY;
            panelButtons.Location = new Point(
                Width - panelButtons.Width - (int)(0.8 * dpiX),
                (int)(1 * dpiY)
            );
            panelButtons.Height = Height - (int)(1.2 * dpiY);
        }

        private void StartEditCell()
        {
            //if (
            //    dataView is not DataView dv
            //)
            //{
            //    return;
            //}
            //DataRowView drv = dv[""];

            int max = 10;
            var parent = Parent;
            while (parent != null && parent is not Form && max > 0)
            {
                parent = parent.Parent;
                max--;
            }

            EditStundenplanEintrag editStundenplanEintrag = new();
            editStundenplanEintrag.StartEditRow(
                parentLocation: parent?.Location,
                parentSize: parent?.Size
                //drv
            );
        }

        private void DeleteCell()
        {
            ShowNotImplementedMessageBox(function: "DeleteCell()");
        }

        private void SaveChanges()
        {
            ShowNotImplementedMessageBox(function: "SaveChanges()");
        }

        private void RollbackChanges()
        {
            ShowNotImplementedMessageBox(function: "RollbackChanges()");
        }

        private void ShowNotImplementedMessageBox(string function) {

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

            MessageBox.Show(
                mainForm,
                $"Die Funktionalität '{function}' ist derzeit nicht implementiert.",
                "Nicht implementiert",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void ShowKlassenTable()
        {
            tab2KlassenTable = new(dbFile);

            tab2KlassenTable.SelectedRowChanged += SelectedRowChangedHandler;

            Point lblTitleLocation = lblTitle.Location;
            Point lblKlasseValueLocation = lblKlasseValue.Location;
            tab2KlassenTable.Location = new Point(lblTitleLocation.X, lblKlasseValueLocation.Y);

            tab2KlassenTable.Height = Height - lblKlasseValueLocation.Y;

            tab2KlassenTable.Name = "Tab2Klasse";
            tab2KlassenTable.TabIndex = 0;

            SuspendLayout();
            Controls.RemoveByKey("Tab2Klasse");
            Controls.Add(tab2KlassenTable);
            ResumeLayout();

            float dpiX = CreateGraphics().DpiX;
            lblKlasseValue.Location = new Point(
                tab2KlassenTable.Location.X + tab2KlassenTable.Width + (int)(0.2 * dpiX),
                lblKlasseValue.Location.Y
            );
            gridKlassenstundenPlan.Location = new Point(
                tab2KlassenTable.Location.X + tab2KlassenTable.Width + (int)(0.2 * dpiX),
                gridKlassenstundenPlan.Location.Y
            );

            tab2KlassenTable.SelectFirstEntry();
        }

        private void SelectedRowChangedHandler(int klassenId, int jahrgang, int klassenStufe, string klassenName)
        {
            KlassenId = klassenId;
            ShowKlassenStundenplan();
        }

        public void ShowKlassenStundenplan()
        {
            string dbFile = this.dbFile.Trim();
            int? klassenId = KlassenId;
            if (klassenId == null || !File.Exists(dbFile))
            {
                ShowLeerenStundenplan();
            }
            else if (klassenId is int kid)
            {
                ShowKlassenStundenplan(kid);
            }
        }

        private void FormatGrid()
        {
            gridKlassenstundenPlan.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridKlassenstundenPlan.AllowDrop = false;
            gridKlassenstundenPlan.AllowUserToAddRows = false;
            gridKlassenstundenPlan.AllowUserToDeleteRows = false;
            gridKlassenstundenPlan.AllowUserToResizeRows = false;
            gridKlassenstundenPlan.AllowUserToResizeColumns = false;
            gridKlassenstundenPlan.AllowUserToOrderColumns = false;
            gridKlassenstundenPlan.SelectionMode = DataGridViewSelectionMode.CellSelect;
            gridKlassenstundenPlan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridKlassenstundenPlan.RowHeadersVisible = false;
            gridKlassenstundenPlan.ColumnHeadersVisible = true;
            gridKlassenstundenPlan.MultiSelect = false;
            gridKlassenstundenPlan.ReadOnly = true;
            gridKlassenstundenPlan.ScrollBars = ScrollBars.Vertical;
        }

        private void ShowLeerenStundenplan()
        {
            FormatGrid();


            string[] nameSchultag = Schulzeit.GetSchultage().Select(z => z.Name()).ToArray();
            string[] columnTitleTexts = ["Slotname", "Anfang", "Ende", .. nameSchultag];
            ZeitSlot[] zeitSlots = Schulzeit.GetZeitSlots();

            DataTable dt = new();
            dt.Namespace = "KlassenStundenplan";
            Array.ForEach(columnTitleTexts, h => dt.Columns.Add(h));
            Array.ForEach(zeitSlots, zeit =>
            {
                dt.Rows.Add(zeit.Name(), zeit.Start(), zeit.Ende());
            });

            gridKlassenstundenPlan.DataSource = null;
            gridKlassenstundenPlan.DataMember = "KlassenStundenplan";
            gridKlassenstundenPlan.DataSource = dt;

            for (int i = 0; i < gridKlassenstundenPlan.Columns.Count; i++)
            {
                gridKlassenstundenPlan.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            AdjustGrid();
        }

        private void UpdateKlassenText(bool loading)
        {
            if (KlassenId is not int klassenId)
            {
                lblKlasseValue.Text = "<Keine Klasse ausgewählt>";
                return;
            }

            try
            {
                OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
                try
                {
                    conn.Open();

                    {
                        using OleDbCommand selectKlasse = conn.CreateCommand();
                        selectKlasse.CommandText = "SELECT klassenName, jahrgang FROM Klasse WHERE ID = @id";
                        selectKlasse.Parameters.Add("@id", OleDbType.Integer).Value = klassenId;

                        using var readerKlassenInfo = selectKlasse.ExecuteReader();
                        readerKlassenInfo.Read();
                        lblKlasseValue.Text = $"Klasse: {readerKlassenInfo.GetString("klassenName")} - Jahrgang: {readerKlassenInfo.GetInt("jahrgang")}{(loading ? " (loading...)" : "")}";
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error - {ex.GetType().Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ShowKlassenStundenplan(int klassenId)
        {
            FormatGrid();

            UpdateKlassenText(loading: true);

            Task.Run(() =>
            {
                string dbFile = this.dbFile.Trim();

                string[] nameSchultag = Schulzeit.GetSchultage().Select(z => z.Name()).ToArray();
                string[] columnTitleTexts = ["Slotname", "Anfang", "Ende", .. nameSchultag];
                ZeitSlot[] zeitSlots = Schulzeit.GetZeitSlots();

                var klassenStundenplan = CreateKlassenStundenplanData();

                try
                {
                    OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
                    try
                    {
                        conn.Open();
                        {
                            using OleDbCommand selectStundenplanEintrag = conn.CreateCommand();
                            selectStundenplanEintrag.CommandText = "SELECT " +
                                "KlassenStundenplan.ID AS KSPID, " +
                                "StundenplanEintrag.ID AS SEID, " +
                                "Lehrer.ID AS LID, " +
                                "Person.ID AS PID, " +
                                "Unterrichtsfach.ID AS UFID, " +
                                "Lehrort.ID AS OrtId, " +
                                "StundenplanEintrag.schultag AS schultag, StundenplanEintrag.zeitslot AS zeitslot, " +
                                "Person.nachname AS nachname, Person.ersterVorname AS ersterVorname, Person.andereVornamen AS andereVornamen, " +
                                "Unterrichtsfach.FachName AS fach, " +
                                "Lehrort.IdGebaeude AS ortGebaeudeId, " +
                                "Lehrort.IdStockwerk AS ortStockwerkId, " +
                                "Lehrort.IdRaumNummer AS ortRaumId, " +
                                "Lehrort.IdName AS ortIdName, " +
                                "Lehrort.OrtName AS ortName" +
                                " FROM " +
                                "KlassenStundenplan, StundenplanEintrag, Lehrer, Person, Unterrichtsfach, Lehrort " +
                                "WHERE " +
                                "KlassenStundenplan.klasse = @klasse AND " +
                                "KlassenStundenplan.eintrag = StundenplanEintrag.ID AND " +
                                "StundenplanEintrag.lehrer = Lehrer.ID AND Lehrer.Person = Person.ID AND " +
                                "StundenplanEintrag.fach = Unterrichtsfach.ID AND " +
                                "StundenplanEintrag.lehrort = Lehrort.ID";
                            selectStundenplanEintrag.Parameters.Add("@klasse", OleDbType.Integer).Value = klassenId;

                            //OleDbDataAdapter dataAdapter = this.dataAdapter = new OleDbDataAdapter(selectStundenplanEintrag);
                            //DataTable dt = this.dataTable = new();
                            //dataAdapter.Fill(dt);
                            //this.dataView = new(dt);

                            using var readerStundenplanEintrag = selectStundenplanEintrag.ExecuteReader();
                            while (readerStundenplanEintrag.Read())
                            {
                                var schultag = (Schultag)readerStundenplanEintrag.GetInt("schultag");
                                var zeitslot = (ZeitSlot)readerStundenplanEintrag.GetInt("zeitslot");
                                var nachname = readerStundenplanEintrag.GetString("nachname");
                                var ersterVorname = readerStundenplanEintrag.GetString("ersterVorname");
                                var andereVornamen = readerStundenplanEintrag.GetString("andereVornamen");
                                Gebaeude gebaeude = (Gebaeude)readerStundenplanEintrag.GetInt("ortGebaeudeId");
                                int? ortStockwerkId = !readerStundenplanEintrag.IsDBNull("ortStockwerkId")
                                    ? readerStundenplanEintrag.GetInt("ortStockwerkId") : null;
                                int? ortRaumId = !readerStundenplanEintrag.IsDBNull("ortRaumId")
                                    ? readerStundenplanEintrag.GetInt("ortRaumId") : null;
                                string? ortIdName = !readerStundenplanEintrag.IsDBNull("ortIdName")
                                    ? readerStundenplanEintrag.GetString("ortIdName") : null;
                                string? ortName = !readerStundenplanEintrag.IsDBNull("ortName")
                                    ? readerStundenplanEintrag.GetString("ortName") : null;

                                int KlassenStundenplanId = readerStundenplanEintrag.GetInt("KSPID");
                                int StundenplanEintragId = readerStundenplanEintrag.GetInt("SEID");
                                int LehrerId = readerStundenplanEintrag.GetInt("LID");
                                int PersonId = readerStundenplanEintrag.GetInt("PID");
                                int UnterrichtsfachId = readerStundenplanEintrag.GetInt("UFID");
                                int OrtId = readerStundenplanEintrag.GetInt("OrtId");
                                Schulzeit zeitpunkt = new(schultag, zeitslot);
                                string fach = readerStundenplanEintrag.GetString("fach");
                                string lehrer = nachname + ", " + string.Join(
                                    " ",
                                    new string[] { ersterVorname, andereVornamen }
                                        .Where(s => !string.IsNullOrWhiteSpace(s))
                                        .ToArray()
                                );
                                var lehrort = new Lehrort(
                                    gebaeude,
                                    idStockWerk: ortStockwerkId,
                                    idRaumNummer: ortRaumId,
                                    idName: ortIdName,
                                    name: ortName
                                );

                                int row1 = Array.FindIndex(klassenStundenplan, s => s is not null && zeitpunkt.ZeitSlot == s[0].zeitpunkt.ZeitSlot);
                                int col1 = Array.FindIndex(klassenStundenplan[0], s => zeitpunkt.Schultag == s.zeitpunkt.Schultag);

                                klassenStundenplan[row1][col1] = (
                                    KlassenStundenplanId,
                                    StundenplanEintragId,
                                    LehrerId,
                                    PersonId,
                                    UnterrichtsfachId,
                                    OrtId,
                                    zeitpunkt,
                                    fach,
                                    lehrer,
                                    lehrort
                                );
                            }
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Invoke(() =>
                        MessageBox.Show(ex.Message, $"Error - {ex.GetType().Name}", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    );
                    return;
                }

                DataTable dtKlassen = new();
                dtKlassen.Namespace = "KlassenStundenplan";
                Array.ForEach(columnTitleTexts, h => dtKlassen.Columns.Add(h));

                int row = 0;
                Array.ForEach(zeitSlots, zeit =>
                {
                    object[] other = klassenStundenplan[row++].Select(s =>
                    {
                        string strLehrort = "";
                        if (s.lehrort is Lehrort lehrort)
                        {
                            strLehrort = lehrort.Name ?? "";
                        }
                        return string.Join(Environment.NewLine, [s.fach ?? "", s.lehrer ?? "", strLehrort]);
                    }).ToArray();
                    dtKlassen.Rows.Add([zeit.Name(), zeit.Start(), zeit.Ende(), .. other]);
                });

                gridKlassenstundenPlan.Invoke(() =>
                {
                    UpdateKlassenText(loading: false);

                    int? selectedRowNull = null, selectedColNull = null;
                    {
                        if ((gridKlassenstundenPlan.SelectedCells.Count > 0 ? gridKlassenstundenPlan.SelectedCells[0] : null) is DataGridViewCell selectedCell)
                        {
                            selectedRowNull = selectedCell.RowIndex;
                            selectedColNull = selectedCell.ColumnIndex;
                        }
                    }

                    gridKlassenstundenPlan.DataSource = null;
                    gridKlassenstundenPlan.DataMember = "KlassenStundenplan";
                    gridKlassenstundenPlan.DataSource = dtKlassen;

                    if ((selectedRowNull is int selectedRow) && (selectedColNull is int selectedCol))
                    {
                        if (
                            selectedRow < gridKlassenstundenPlan.Rows.Count &&
                            selectedCol < gridKlassenstundenPlan.Columns.Count
                        )
                        {
                            gridKlassenstundenPlan.Rows[selectedRow].Cells[selectedCol].Selected = true;
                        }
                    }

                    AdjustGrid();
                });
            });
        }

        private void ResizeGrid()
        {
            for (int i = 0; i < gridKlassenstundenPlan.Columns.Count; i++)
            {
                gridKlassenstundenPlan.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (gridKlassenstundenPlan.Rows.Count == 0 && gridKlassenstundenPlan.Columns.Count == 0)
            {
                return;
            }

            float DpiX = CreateGraphics().DpiX;
            float fix = 0.02f * DpiX;

            int newColumnWidth = gridKlassenstundenPlan.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + (int)(fix * gridKlassenstundenPlan.ColumnCount);
            if (((int)(gridKlassenstundenPlan.ScrollBars) & ((int)ScrollBars.Vertical)) != 0)
            {
                newColumnWidth += SystemInformation.VerticalScrollBarWidth;
            }

            int newGridHeight = gridKlassenstundenPlan.Rows.GetRowsHeight(DataGridViewElementStates.Displayed)
                + gridKlassenstundenPlan.ColumnHeadersHeight
                + (int)(fix * (gridKlassenstundenPlan.RowCount));
            if (((int)(gridKlassenstundenPlan.ScrollBars) & ((int)ScrollBars.Horizontal)) != 0)
            {
                newGridHeight += SystemInformation.HorizontalScrollBarHeight;
            }

            if (newColumnWidth > Width - gridKlassenstundenPlan.Location.X - (int)(0.5 * DpiX))
            {
                if (((int)(gridKlassenstundenPlan.ScrollBars) & ((int)ScrollBars.Horizontal)) == 0)
                {
                    gridKlassenstundenPlan.ScrollBars |= ScrollBars.Horizontal;
                    newGridHeight += SystemInformation.HorizontalScrollBarHeight;
                }

                gridKlassenstundenPlan.Size = new Size(
                    Width - gridKlassenstundenPlan.Location.X - (int)(0.5 * DpiX),
                    Math.Min(
                        newGridHeight,
                        Height - gridKlassenstundenPlan.Location.Y - (int)(0.5 * DpiX)
                    )
                );
            }
            else
            {
                gridKlassenstundenPlan.Size = new Size(
                    newColumnWidth,
                    Math.Min(
                        newGridHeight,
                        Height - gridKlassenstundenPlan.Location.Y - (int)(0.5 * DpiX)
                    )
                );
            }
        }

        private void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustGrid();
        }

        private void AdjustGrid()
        {
            float dpiY = CreateGraphics().DpiY;

            for (int row2 = 0; row2 < gridKlassenstundenPlan.Rows.Count; row2++)
            {
                for (int col2 = 0; col2 < gridKlassenstundenPlan.Columns.Count; col2++)
                {
                    gridKlassenstundenPlan.Rows[row2].Cells[col2].Style.WrapMode = DataGridViewTriState.True;
                }
            }

            gridKlassenstundenPlan.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            Schultag[] tage = Schulzeit.GetSchultage();
            ZeitSlot[] zeitSlots = Schulzeit.GetZeitSlots();
            for (int i = 0; i < gridKlassenstundenPlan.Rows.Count; i++)
            {
                var tag = gridKlassenstundenPlan.Rows[i].Cells[3].Tag;
                var slot = zeitSlots[i];

                if (tag == null)
                {
                    int x = 0;
                    foreach (var schultag in tage)
                    {
                        gridKlassenstundenPlan.Rows[i].Cells[3 + x].Tag = ((
                            int? KlassenStundenplanId,
                            int? StundenplanEintragId,
                            int? LehrerId,
                            int? PersonId,
                            int? UnterrichtsfachId,
                            int? OrtId,
                            Schulzeit zeitpunkt,
                            string? fach,
                            string? lehrer,
                            Lehrort? lehrort
                        ))(null, null, null, null, null, null, new Schulzeit(schultag, slot), null, null, null);
                        x++;
                    }
                }

                gridKlassenstundenPlan.Rows[i].Height = slot.IsStunde() ? (int)(0.8 * dpiY) : (int)(0.3 * dpiY);
                gridKlassenstundenPlan.Rows[i].DefaultCellStyle.BackColor = slot.IsStunde() ? Color.LightBlue : Color.PaleVioletRed;
            }

            gridKlassenstundenPlan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            float dpiX = CreateGraphics().DpiX;
            for (int i = 0; i < gridKlassenstundenPlan.Columns.Count; i++)
            {
                var column = gridKlassenstundenPlan.Columns[i];
                string title = column.Name;

                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                if (title == "Slotname")
                {
                    column.Width = (int)(dpiX);
                }
                else if (title == "Anfang" || title == "Ende")
                {
                    column.Width = (int)(0.8 * dpiX);
                }
                else
                {
                    column.Width = (int)(2 * dpiX);
                }
            }

            ResizeGrid();
        }

        private (
                int? KlassenStundenplanId,
                int? StundenplanEintragId,
                int? LehrerId,
                int? PersonId,
                int? UnterrichtsfachId,
                int? OrtId,
                Schulzeit zeitpunkt,
                string? fach,
                string? lehrer,
                Lehrort? lehrort
        )[][] CreateKlassenStundenplanData() => Schulzeit.GetZeitSlots().Select(schulzeit =>
            Schulzeit.GetSchultage().Select(schultag =>
                ((
                    int? KlassenStundenplanId,
                    int? StundenplanEintragId,
                    int? LehrerId,
                    int? PersonId,
                    int? UnterrichtsfachId,
                    int? OrtId,
                    Schulzeit zeitpunkt,
                    string? fach,
                    string? lehrer,
                    Lehrort? lehrort
                ))
                (null, null, null, null, null, null, new Schulzeit(schultag, schulzeit), null, null, null)
            ).ToArray()
        ).ToArray();

        private void CmdBearbeitenEintrag_Click(object sender, EventArgs e)
        {
            StartEditCell();
        }

        private void CmdLoeschenEintrag_Click(object sender, EventArgs e)
        {
            DeleteCell();
        }

        private void CmdSaveDatabase_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void CmdRollbackDatabaseChanges_Click(object sender, EventArgs e)
        {
            RollbackChanges();
        }
    }

}
