using SchulplanOrganisation.Daten;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestSchulplanOrganisation
{
    /**
     * Füllt die Datenbank mit Arbeitsdaten.
     * 
     * Ausführungsreihenfolge bei leerer Datenbank:
     * 
     * 1. TestClearAndFillPersonenTable()
     * 2. TestClearAndFillUnterrichtsFach()
     * 3. TestClearAndFillLehrort()
     * 4. TestClearAndFillKlasse()
     * 5. TestClearAndFillLehrerUnterricht()
     * 6. TestClearAndFillKlassenStundenplan()
     * 
     */

    //[Ignore]
    [TestClass]
    public partial class UnitTestSetupSchulorganisationDatabase
    {
        private readonly string csvLehrer = Path.GetFullPath(@"..\..\..\..\Lehrer_data.csv");
        private readonly string csvSchueler = Path.GetFullPath(@"..\..\..\..\Schueler_data.csv");
        private readonly string dbFile = Path.GetFullPath(@"..\..\..\..\schulplan_db_modify.accdb");

        private static readonly string[] formats = new string[] {
            "M/d/yyyy", "MM/d/yyyy", "M/dd/yyyy", "MM/dd/yyyy"
        };

        [Ignore]
        [TestMethod]
        public void TestClearAndFillPersonenTable()
        {
            if (!File.Exists(csvLehrer) || !File.Exists(csvSchueler))
            {
                Assert.Fail("Die Personen-Dateien 'Lehrer_data.csv' und 'Schueler_data.csv' sind nicht vorhanden");
            }

            if (!File.Exists(dbFile))
            {
                Assert.Fail("Die Datenbank 'schulplan_db_modify.accdb' existiert nicht.");
            }

            using OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
            try
            {
                conn.Open();
                using OleDbTransaction mainTransaction = conn.BeginTransaction();
                try
                {
                    using OleDbCommand cmdClearPersonTable = conn.CreateCommand();
                    cmdClearPersonTable.CommandText = "DELETE FROM Person";
                    cmdClearPersonTable.Transaction = mainTransaction;
                    cmdClearPersonTable.ExecuteNonQuery();

                    using OleDbCommand cmdClearLehrerTable = conn.CreateCommand();
                    cmdClearLehrerTable.CommandText = "DELETE FROM Lehrer";
                    cmdClearLehrerTable.Transaction = mainTransaction;
                    cmdClearLehrerTable.ExecuteNonQuery();

                    using OleDbCommand cmdClearSchuelerTable = conn.CreateCommand();
                    cmdClearSchuelerTable.CommandText = "DELETE FROM Schueler";
                    cmdClearSchuelerTable.Transaction = mainTransaction;
                    cmdClearSchuelerTable.ExecuteNonQuery();

                    ProcessCsvFile(
                        conn: conn,
                        mainTransaction: mainTransaction,
                        csvFile: csvLehrer,
                        roleTable: "Lehrer"
                    );
                    ProcessCsvFile(
                        conn: conn,
                        mainTransaction: mainTransaction,
                        csvFile: csvSchueler,
                        roleTable: "Schueler"
                    );

                    mainTransaction.Commit();
                    Debug.WriteLine("Die Daten aus 'Lehrer_data.csv' und aus 'Schueler_data.csv' wurden erfolgreich in die Datenbank 'schulplan_db_modify.accdb' importiert");
                }
                catch (Exception ex)
                {
                    mainTransaction.Rollback();
                    Assert.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Ignore]
        [TestMethod]
        public void TestClearAndFillUnterrichtsFach()
        {
            if (!File.Exists(dbFile))
            {
                Assert.Fail("Die Datenbank 'schulplan_db_modify.accdb' existiert nicht.");
            }

            string[] klassenFaecher = [
                "Deutsch",
                "Englisch",
                "Kunst",
                "Musik",
                "Erdkunde",
                "Geschichte",
                "Politik- und Wirtschaftslehre",
                "Arbeitslehre",
                "Mathematik",
                "Informatik",
                "Biologie",
                "Chemie",
                "Physik",
                "Sport"
            ];

            string[] wahlFaecher = [
                "Schönschrift AG",
                "Handball AG",
                "Forscher AG",
                "Finanzielle Bildung AG",
                "Acrylmalerei AG",
                "Europa AG",
                "Dele - Spanisch Kurs",
                "Delf - Französisch Kurs",
                "Darts AG",
                "Courage AG",
                "Coaching AG",
                "Chor AG",
                "Cambridge Advanced English Kurs",
                "Bienen AG",
                "Lese-Scouts AG",
                "Besser lesen! AG",
                "Autorenwerkstatt AG",
                "Hausaufgaben-Betreuung",
                "Keyboard AG",
                "Kunst AG",
                "LEGO-Robotik AG",
                "Lese AG - Deutsch",
                "Lese AG - Englisch",
                "Leichtathletik AG",
                "Licht- und Ton-AG",
                "Ruder AG",
                "Schach AG",
                "Schulgarten AG",
                "Tanz AG",
                "Musical AG",
                "Technik AG"
            ];

            string[] drittSprachFach = [
                "Französisch",
                "Latein",
                "Spanisch"
            ];

            string[] religionEthikFaecher = [
                "Ethik",
                "Religion"
            ];

            using OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
            try
            {
                conn.Open();
                using OleDbTransaction mainTransaction = conn.BeginTransaction();
                try
                {
                    using OleDbCommand cmdClearPersonTable = conn.CreateCommand();
                    cmdClearPersonTable.Transaction = mainTransaction;
                    cmdClearPersonTable.CommandText = "DELETE FROM Unterrichtsfach";
                    cmdClearPersonTable.ExecuteNonQuery();

                    foreach (
                        (string[] faecher, FachArt fachart) in new (string[] faecher, FachArt art)[] {
                            (klassenFaecher, FachArt.KlassenFach),
                            (wahlFaecher, FachArt.WahlFach),
                            (drittSprachFach, FachArt.DrittSprachenFach),
                            (religionEthikFaecher, FachArt.WeltanschauungsFach)
                        }
                    )
                    {
                        foreach (string fach in faecher)
                        {
                            using OleDbCommand cmdInsertUnterichtsFach = conn.CreateCommand();
                            cmdInsertUnterichtsFach.Transaction = mainTransaction;
                            cmdInsertUnterichtsFach.CommandText = "INSERT INTO Unterrichtsfach (FachName, FachArt) VALUES (@FachName, @FachArt)";
                            cmdInsertUnterichtsFach.Parameters.Add("@FachName", OleDbType.VarChar).Value = fach;
                            cmdInsertUnterichtsFach.Parameters.Add("@FachArt", OleDbType.Integer).Value = fachart;
                            cmdInsertUnterichtsFach.ExecuteNonQuery();
                        }
                    }

                    mainTransaction.Commit();
                }
                catch (Exception ex)
                {
                    mainTransaction.Rollback();
                    Assert.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Ignore]
        [TestMethod]
        public void TestClearAndFillLehrort()
        {
            if (!File.Exists(dbFile))
            {
                Assert.Fail("Die Datenbank 'schulplan_db_modify.accdb' existiert nicht.");
            }

            Random wuerfel = new Random();

            using OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
            try
            {
                conn.Open();
                using OleDbTransaction mainTransaction = conn.BeginTransaction();
                try
                {
                    using OleDbCommand cmdClearLehrortTable = conn.CreateCommand();
                    cmdClearLehrortTable.Transaction = mainTransaction;
                    cmdClearLehrortTable.CommandText = "DELETE FROM Lehrort";
                    cmdClearLehrortTable.ExecuteNonQuery();

                    List<(
                        Gebaeude gebaeudeId,
                        int? stockwerkId,
                        int? raumId,
                        string? nameId,
                        string? name
                    )> listLehrraeume = [];
                    listLehrraeume.AddRange(Enumerable.Range(0, 4).SelectMany(stockwerk => Enumerable.Range(1, 300).SelectMany(raum =>
                        new (Gebaeude, int?, int?, string?, string?)[] {
                            (Gebaeude.GebaeudeEins, stockwerk, raum, null, null),
                            (Gebaeude.GebaeudeZwei, stockwerk, raum, null, null),
                        }
                    )));
                    listLehrraeume.Add((Gebaeude.Sporthalle, null, null, "HalleEnterprise", "Halle Enterprise"));
                    listLehrraeume.Add((Gebaeude.Sporthalle, null, null, "HalleGondor", "Halle Gondor"));
                    listLehrraeume.Add((Gebaeude.Sporthalle, null, null, "HalleHogwarts", "Halle Hogwarts"));
                    listLehrraeume.Add((Gebaeude.Sonstiges, null, null, "Bibliothek", "Bibliothek"));
                    listLehrraeume = listLehrraeume.OrderBy(l => wuerfel.Next()).ToList();

                    foreach (var lehrraum in listLehrraeume)
                    {
                        using OleDbCommand cmdInsertLehrort = conn.CreateCommand();
                        cmdInsertLehrort.Transaction = mainTransaction;
                        cmdInsertLehrort.CommandText = "INSERT INTO Lehrort " +
                            "(IdGebaeude, IdStockwerk, IdRaumNummer, IdName, OrtName) VALUES " +
                            "(@IdGebaeude, @IdStockwerk, @IdRaumNummer, @IdName, @OrtName)";
                        cmdInsertLehrort.Parameters.Add("@IdGebaeude", OleDbType.Integer).Value = lehrraum.gebaeudeId;
                        cmdInsertLehrort.Parameters.Add("@IdStockwerk", OleDbType.Integer).Value = ((object?)lehrraum.stockwerkId) ?? DBNull.Value;
                        cmdInsertLehrort.Parameters.Add("@IdRaumNummer", OleDbType.Integer).Value = ((object?)lehrraum.raumId) ?? DBNull.Value;
                        cmdInsertLehrort.Parameters.Add("@IdName", OleDbType.VarChar).Value = ((object?)lehrraum.nameId) ?? DBNull.Value;
                        cmdInsertLehrort.Parameters.Add("@OrtName", OleDbType.VarChar).Value = ((object?)lehrraum.name) ?? DBNull.Value;

                        cmdInsertLehrort.ExecuteNonQuery();
                    }

                    mainTransaction.Commit();
                }
                catch (Exception ex)
                {
                    mainTransaction.Rollback();
                    Assert.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Ignore]
        [TestMethod]
        public void TestClearAndFillLehrerUnterricht()
        {
            if (!File.Exists(dbFile))
            {
                Assert.Fail("Die Datenbank 'schulplan_db_modify.accdb' existiert nicht.");
            }

            Random wuerfel = new Random();
            List<(int ID, string fachName, int fachArt)> listKlassenFaecher = [];
            List<(int ID, string fachName, int fachArt)> listWahlFaecher = [];
            List<int> listLehrerIds = [];

            using OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
            try
            {
                conn.Open();
                using OleDbTransaction mainTransaction = conn.BeginTransaction();
                try
                {
                    using OleDbCommand cmdSelectUnterichtsfachTable = conn.CreateCommand();
                    cmdSelectUnterichtsfachTable.Transaction = mainTransaction;
                    cmdSelectUnterichtsfachTable.CommandText = "SELECT ID, FachName, FachArt FROM Unterrichtsfach";

                    using var readUnterichtsFach = cmdSelectUnterichtsfachTable.ExecuteReader();
                    while (readUnterichtsFach.Read())
                    {
                        int ID = readUnterichtsFach.GetInt32("ID");
                        string fachName = readUnterichtsFach.GetString("FachName");
                        FachArt fachArt = (FachArt)(int)readUnterichtsFach.GetInt16("FachArt");
                        if (fachArt == FachArt.None)
                        {
                            continue;
                        }
                        else if (fachArt == FachArt.WahlFach)
                        {
                            listWahlFaecher.Add((ID, fachName, (int)FachArt.WahlFach));
                        }
                        else
                        {
                            listKlassenFaecher.Add((ID, fachName, (int)FachArt.WahlFach));
                        }
                    }

                    if (listKlassenFaecher.Count + listWahlFaecher.Count == 0)
                    {
                        Assert.Fail("Es gibt keine Unterrichtsfächer.");
                    }
                    listKlassenFaecher = listKlassenFaecher.OrderBy(x => wuerfel.Next()).ToList();
                    listWahlFaecher = listWahlFaecher.OrderBy(x => wuerfel.Next()).ToList();

                    using OleDbCommand cmdSelectLehrerTable = conn.CreateCommand();
                    cmdSelectLehrerTable.Transaction = mainTransaction;
                    cmdSelectLehrerTable.CommandText = "SELECT ID FROM Lehrer";

                    using var readLehrerIds = cmdSelectLehrerTable.ExecuteReader();
                    while (readLehrerIds.Read())
                    {
                        listLehrerIds.Add(readLehrerIds.GetInt32("ID"));
                    }
                    if (listLehrerIds.Count == 0)
                    {
                        Assert.Fail("Es gibt keine Lehrer.");
                    }
                    listLehrerIds = listLehrerIds.OrderBy(x => wuerfel.Next()).ToList();

                    int i = 0;
                    List<(int lehrerId, int unterrichtsFachid)> listLehrerUntericht = [];
                    foreach (var lehrerId in listLehrerIds)
                    {
                        bool has3rd = (wuerfel.Next() & 1) != 0;
                        listLehrerUntericht.Add((lehrerId, listKlassenFaecher[(i) % listKlassenFaecher.Count].ID));
                        listLehrerUntericht.Add((lehrerId, listKlassenFaecher[(i + 1) % listKlassenFaecher.Count].ID));
                        if (has3rd)
                        {
                            listLehrerUntericht.Add((lehrerId, listKlassenFaecher[(i + 2) % listKlassenFaecher.Count].ID));
                            i++;
                        }
                        i += 2;
                    }

                    foreach (var wahlfach in listWahlFaecher)
                    {
                        int index = wuerfel.Next(listLehrerIds.Count);
                        int lehrerId = listLehrerIds[index];
                        listLehrerIds.RemoveAt(index);

                        listLehrerUntericht.Add((lehrerId, wahlfach.ID));
                    }

                    using OleDbCommand cmdClearLehrerUntericht = conn.CreateCommand();
                    cmdClearLehrerUntericht.Transaction = mainTransaction;
                    cmdClearLehrerUntericht.CommandText = "DELETE FROM LehrerUnterricht";
                    cmdClearLehrerUntericht.ExecuteNonQuery();

                    foreach (var lehrerUnterricht in listLehrerUntericht)
                    {
                        using OleDbCommand cmdInsertLehrerUnterricht = conn.CreateCommand();
                        cmdInsertLehrerUnterricht.Transaction = mainTransaction;
                        cmdInsertLehrerUnterricht.CommandText = "INSERT INTO LehrerUnterricht (Lehrer, Unterrichtsfach) VALUES (@LehrerId, @UnterrichtsId)";
                        cmdInsertLehrerUnterricht.Parameters.Add("@LehrerId", OleDbType.Integer).Value = lehrerUnterricht.lehrerId;
                        cmdInsertLehrerUnterricht.Parameters.Add("@UnterrichtsId", OleDbType.Integer).Value = lehrerUnterricht.unterrichtsFachid;
                        cmdInsertLehrerUnterricht.ExecuteNonQuery();
                    }

                    mainTransaction.Commit();
                }
                catch (Exception ex)
                {
                    mainTransaction.Rollback();
                    Assert.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Ignore]
        [TestMethod]
        public void TestClearAndFillKlasse()
        {
            if (!File.Exists(dbFile))
            {
                Assert.Fail("Die Datenbank 'schulplan_db_modify.accdb' existiert nicht.");
            }
            Random wuerfel = new();

            using OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
            try
            {
                conn.Open();
                using OleDbTransaction mainTransaction = conn.BeginTransaction();

                try
                {
                    using OleDbCommand cmdClearSchuelerKlasse = conn.CreateCommand();
                    cmdClearSchuelerKlasse.Transaction = mainTransaction;
                    cmdClearSchuelerKlasse.CommandText = "DELETE FROM SchuelerKlasse";
                    cmdClearSchuelerKlasse.ExecuteNonQuery();

                    using OleDbCommand cmdClearKlasse = conn.CreateCommand();
                    cmdClearKlasse.Transaction = mainTransaction;
                    cmdClearKlasse.CommandText = "DELETE FROM Klasse";
                    cmdClearKlasse.ExecuteNonQuery();

                    using OleDbCommand cmdSchuelerGeburtstag = conn.CreateCommand();
                    cmdSchuelerGeburtstag.Transaction = mainTransaction;
                    cmdSchuelerGeburtstag.CommandText = "SELECT Person.ID AS PID, Schueler.ID AS SID, Person.geburtstag AS geburtstag FROM Schueler, Person WHERE Schueler.person = Person.ID";

                    List<(int PID, int SID, DateTime geburtstag)> schuelerGeburtstagsListe = [];

                    OleDbDataReader reader = cmdSchuelerGeburtstag.ExecuteReader();
                    while (reader.Read())
                    {
                        int pid = reader.GetInt32("PID");
                        int sid = reader.GetInt32("SID");
                        DateTime geburtstag = reader.GetDateTime("geburtstag").Date;

                        schuelerGeburtstagsListe.Add((PID: pid, SID: sid, geburtstag: geburtstag));
                    }
                    schuelerGeburtstagsListe = schuelerGeburtstagsListe.OrderBy(s => wuerfel.Next()).ToList();

                    if (schuelerGeburtstagsListe.Count == 0)
                    {
                        Assert.Fail("Keine Schüler");
                    }

                    int anzahl = (int)Math.Round(((float)schuelerGeburtstagsListe.Count) / 30.0, 0, MidpointRounding.AwayFromZero);

                    var klassen = Enumerable.Range(5, 8).SelectMany(stufe =>
                        "abcd".Select(c =>
                            ((int? klassenId, int jahr, int stufe, string klassenName, int schuelerCount))
                            (null, 2018, stufe, $"{stufe}{c}", 0)
                        )
                    ).ToArray();

                    if (klassen.Length > anzahl)
                    {
                        Array.Resize(ref klassen, anzahl);
                    }
                    wuerfel.Shuffle(klassen);

                    int klassenIndex = -1;
                    int targetKlassenSize = -1;

                    bool addAtRandom = false;
                    for (int schuelerIndex = 0; schuelerIndex < schuelerGeburtstagsListe.Count; schuelerIndex++)
                    {
                        if (!addAtRandom && (klassenIndex == -1 || klassen[klassenIndex].schuelerCount == targetKlassenSize))
                        {
                            klassenIndex++;
                            if (
                                klassenIndex == klassen.Length ||
                                schuelerGeburtstagsListe.Count - schuelerIndex < 25
                            )
                            {
                                addAtRandom = true;
                            }
                            else
                            {
                                if (schuelerGeburtstagsListe.Count - schuelerIndex >= 32)
                                {
                                    targetKlassenSize = wuerfel.Next(25, 32);
                                }
                                else
                                {
                                    targetKlassenSize = schuelerGeburtstagsListe.Count - schuelerIndex;
                                }
                            }
                        }

                        int selectedKlassenIndex;
                        (int? klassenId, int klassenJahrgang, int klassenStufe, string klassenName, int schuelerCount) infoKlasse;

                        if (!addAtRandom)
                        {
                            selectedKlassenIndex = klassenIndex;
                        }
                        else
                        {
                            int countFree = klassen.Where(k => k.schuelerCount < 31).Count();
                            if (countFree == 0)
                            {
                                selectedKlassenIndex = wuerfel.Next(klassen.Length);
                            }
                            else
                            {
                                int skipCount = wuerfel.Next(countFree);
                                if (skipCount == 0)
                                {
                                    selectedKlassenIndex = klassen.Select((s, index) => (s, index)).Where(k => k.s.schuelerCount < 31).First().index;
                                }
                                else
                                {
                                    selectedKlassenIndex = klassen.Select((s, index) => (s, index)).Where(k => k.s.schuelerCount < 31).Skip(skipCount).First().index;
                                }
                            }
                        }
                        infoKlasse = klassen[selectedKlassenIndex];

                        infoKlasse.schuelerCount++;

                        if (infoKlasse.klassenId == null)
                        {
                            using OleDbCommand cmdInsertKlasse = conn.CreateCommand();
                            cmdInsertKlasse.Transaction = mainTransaction;
                            cmdInsertKlasse.CommandText = "INSERT INTO Klasse " +
                                "(jahrgang, klassenStufe, klassenName) VALUES " +
                                "(@jahrgang, @klassenStufe, @klassenName)";
                            cmdInsertKlasse.Parameters.Add("@jahrgang", OleDbType.Integer).Value = infoKlasse.klassenJahrgang;
                            cmdInsertKlasse.Parameters.Add("@klassenStufe", OleDbType.Integer).Value = infoKlasse.klassenStufe;
                            cmdInsertKlasse.Parameters.Add("@klassenName", OleDbType.VarChar).Value = infoKlasse.klassenName;
                            cmdInsertKlasse.ExecuteNonQuery();

                            infoKlasse.klassenId = GetIdAfterInsert(conn, mainTransaction);
                        }
                        klassen[selectedKlassenIndex] = infoKlasse;

                        (int personPid, int schuelerId, DateTime geburtstag) infoSchueler = schuelerGeburtstagsListe[schuelerIndex];

                        int alter = (infoKlasse.klassenStufe >= 5 && infoKlasse.klassenStufe < 13) ? (infoKlasse.klassenStufe + wuerfel.Next(6, 8)) : -1;
                        infoSchueler.geburtstag = new(infoKlasse.klassenJahrgang - alter - (infoSchueler.geburtstag.Month > 9 ? 1 : 0), infoSchueler.geburtstag.Month, infoSchueler.geburtstag.Day);

                        schuelerGeburtstagsListe[schuelerIndex] = infoSchueler;

                        if (alter > 0)
                        {
                            using OleDbCommand cmdUpdateGeburtstag = conn.CreateCommand();
                            cmdUpdateGeburtstag.Transaction = mainTransaction;
                            cmdUpdateGeburtstag.CommandText = "UPDATE Person SET geburtstag = @geburtstag WHERE ID = @pid";
                            cmdUpdateGeburtstag.Parameters.Add("@geburtstag", OleDbType.DBDate).Value = infoSchueler.geburtstag.Date;
                            cmdUpdateGeburtstag.Parameters.Add("@pid", OleDbType.Integer).Value = infoSchueler.personPid;
                            cmdUpdateGeburtstag.ExecuteNonQuery();
                        }

                        using OleDbCommand cmdInsertSchuelerKlasse = conn.CreateCommand();
                        cmdInsertSchuelerKlasse.Transaction = mainTransaction;
                        cmdInsertSchuelerKlasse.CommandText = "INSERT INTO SchuelerKlasse (Klasse, Schueler) VALUES (@klassenId, @schuelerId)";
                        cmdInsertSchuelerKlasse.Parameters.Add("@klassenId", OleDbType.Integer).Value = infoKlasse.klassenId;
                        cmdInsertSchuelerKlasse.Parameters.Add("@schuelerId", OleDbType.Integer).Value = infoSchueler.schuelerId;
                        cmdInsertSchuelerKlasse.ExecuteNonQuery();
                    }

                    mainTransaction.Commit();

                    Console.WriteLine("Die Schüler wurden erfolgreich auf die Klassen verteilt.");
                }
                catch (Exception ex)
                {
                    mainTransaction.Rollback();
                    Assert.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Ignore]
        [TestMethod]
        public void TestClearAndFillKlassenStundenplan()
        {
            if (!File.Exists(dbFile))
            {
                Assert.Fail("Die Datenbank 'schulplan_db_modify.accdb' existiert nicht.");
            }

            Random wuerfel = new Random();

            using OleDbConnection conn = new($@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{dbFile}'");
            try
            {
                conn.Open();
                using OleDbTransaction mainTransaction = conn.BeginTransaction();
                try
                {
                    using OleDbCommand cmdSelectKlasseStundenplanEintragId = conn.CreateCommand();
                    cmdSelectKlasseStundenplanEintragId.Transaction = mainTransaction;
                    cmdSelectKlasseStundenplanEintragId.CommandText = "SELECT eintrag FROM KlassenStundenplan";

                    using var readerUnterrichtKlasseId = cmdSelectKlasseStundenplanEintragId.ExecuteReader();
                    using var transactionClearStundenPlan = mainTransaction.Begin();
                    try
                    {
                        while (readerUnterrichtKlasseId.Read())
                        {
                            int id = readerUnterrichtKlasseId.GetInt("unterricht");

                            using OleDbCommand cmdDeleteStundenplanEintragById = conn.CreateCommand();
                            cmdDeleteStundenplanEintragById.Transaction = transactionClearStundenPlan;
                            cmdDeleteStundenplanEintragById.CommandText = "DELETE FROM StundenplanEintrag WHERE ID = @eintrag";
                            cmdDeleteStundenplanEintragById.Parameters.Add("@eintrag", OleDbType.Integer).Value = id;
                            cmdDeleteStundenplanEintragById.ExecuteNonQuery();
                        }

                        using OleDbCommand cmdClearKlasseStundenplan = conn.CreateCommand();
                        cmdClearKlasseStundenplan.Transaction = transactionClearStundenPlan;
                        cmdClearKlasseStundenplan.CommandText = "DELETE FROM KlassenStundenplan";
                        cmdClearKlasseStundenplan.ExecuteNonQuery();

                        transactionClearStundenPlan.Commit();
                    }
                    catch (Exception ex)
                    {
                        transactionClearStundenPlan.Rollback();
                        Assert.Fail(ex.Message);
                    }

                    using OleDbCommand cmdSelectKlasseId = conn.CreateCommand();
                    cmdSelectKlasseId.Transaction = mainTransaction;
                    cmdSelectKlasseId.CommandText = "SELECT ID FROM Klasse";

                    List<int> listKlassenIds = [];
                    using DbDataReader readerKlasseId = cmdSelectKlasseId.ExecuteReader();
                    while (readerKlasseId.Read())
                    {
                        listKlassenIds.Add(readerKlasseId.GetInt("ID"));
                    }

                    using OleDbCommand cmdSelectLehrort = conn.CreateCommand();
                    cmdSelectLehrort.Transaction = mainTransaction;
                    cmdSelectLehrort.CommandText = "SELECT ID, IdGebaeude FROM Lehrort";

                    List<(int ID, Gebaeude idGebaeude)> listLehrort = [];
                    using DbDataReader readerLeerort = cmdSelectLehrort.ExecuteReader();
                    while (readerLeerort.Read())
                    {
                        int ID = readerLeerort.GetInt("ID");
                        Gebaeude idGebaeude = (Gebaeude)readerLeerort.GetInt("IdGebaeude");
                        listLehrort.Add((ID, idGebaeude));
                    }

                    using OleDbCommand cmdSelectLehrerUnterrichtsfach = conn.CreateCommand();
                    cmdSelectLehrerUnterrichtsfach.Transaction = mainTransaction;
                    cmdSelectLehrerUnterrichtsfach.CommandText = "SELECT " +
                        "LehrerUnterricht.Lehrer AS lehrer, LehrerUnterricht.Unterrichtsfach AS fach, Unterrichtsfach.FachName as fachName, Unterrichtsfach.FachArt AS fachArt " +
                        "FROM LehrerUnterricht, Unterrichtsfach " +
                        "WHERE LehrerUnterricht.Unterrichtsfach = Unterrichtsfach.ID";

                    List <(int lehrer, int fach, string fachname, FachArt fachArt)> listLehrerUnterricht = [];

                    using var readerLehrerUnterrichtsfach = cmdSelectLehrerUnterrichtsfach.ExecuteReader();
                    while (readerLehrerUnterrichtsfach.Read())
                    {
                        int lehrer = readerLehrerUnterrichtsfach.GetInt("lehrer");
                        int fach = readerLehrerUnterrichtsfach.GetInt("fach");
                        string fachName = readerLehrerUnterrichtsfach.GetString("fachName");
                        FachArt fachArt = (FachArt)readerLehrerUnterrichtsfach.GetInt("fachArt");
                        listLehrerUnterricht.Add((lehrer, fach, fachName, fachArt));
                    }
                    listLehrerUnterricht = listLehrerUnterricht.OrderBy(s => wuerfel.Next()).ToList();

                    // Fächer:
                    // o 14 Klassen-Fächer
                    // o 31 Wahl-Fächer
                    // o 3 3.-Spachen-Fächer
                    // o 2 Weltanschauungsfächer
                    List<(int lehrer, int fach, string fachname, FachArt fachArt)> listLehrerKlassenfach
                        = listLehrerUnterricht.Where(s => s.fachArt == FachArt.KlassenFach).ToList();

                    List<(int lehrer, int fach, string fachname, FachArt fachArt)> listLehrerDrittSprachenfach
                        = listLehrerUnterricht.Where(s => s.fachArt == FachArt.DrittSprachenFach).ToList();

                    List<(int lehrer, int fach, string fachname, FachArt fachArt)> listLehrerWeltanschauungsFaecher
                        = listLehrerUnterricht.Where(s => s.fachArt == FachArt.WeltanschauungsFach).ToList();

                    List<(int, string)> listKlassenFaecher = listLehrerKlassenfach.GroupBy(s => s.fach).Select(s => {
                        var item = s.First();
                        return (item.fach, item.fachname);
                    }).ToList();
                    List<(int, string)> listDrittSprachenFaecher = listLehrerKlassenfach.GroupBy(s => s.fach).Select(s => {
                        var item = s.First();
                        return (item.fach, item.fachname);
                    }).ToList();
                    List<(int, string)> listWeltanschauungsFaecher = listLehrerKlassenfach.GroupBy(s => s.fach).Select(s => {
                        var item = s.First();
                        return (item.fach, item.fachname);
                    }).ToList();

                    List<(Schulzeit zeit, int lehrer, int fach, int klasse, int lehrort)> listStundenplanEintraege = [];
                    foreach (int klasse in listKlassenIds)
                    {
                        List<Schulzeit> schulzeiten = Schulzeit.GetSchultage().SelectMany(tag =>
                            Schulzeit.GetUnterrichtszeiten().Select(slot => new Schulzeit(tag, slot))
                        ).ToList();

                        // Klassen-Unterricht
                        foreach ((int fach, string fachName) klassenFach in listKlassenFaecher) {
                            if (schulzeiten.Count == 0)
                            {
                                break;
                            }

                            int lehrort;
                            if ("sport".Equals(klassenFach.fachName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var list = listLehrort.Where(l => l.idGebaeude == Gebaeude.Sporthalle).ToList();
                                lehrort = list[wuerfel.Next(list.Count)].ID;
                            }
                            else
                            {
                                var list = listLehrort.Where(l => l.idGebaeude == Gebaeude.GebaeudeEins || l.idGebaeude == Gebaeude.GebaeudeZwei).ToList();
                                lehrort = list[wuerfel.Next(list.Count)].ID;
                            }

                            List<int> listFachLehrer = listLehrerKlassenfach.Where(s => s.fach == klassenFach.fach).Select(s => s.lehrer).ToList();
                            int lehrer = listFachLehrer[wuerfel.Next(listFachLehrer.Count)];

                            int selected = wuerfel.Next(schulzeiten.Count);
                            Schulzeit schulzeit1 = schulzeiten[selected];
                            schulzeiten.RemoveAt(selected);

                            listStundenplanEintraege.Add((
                                schulzeit1, lehrer, klassenFach.fach, klasse, lehrort
                            ));

                            if (schulzeiten.Count != 0)
                            {
                                selected = wuerfel.Next(schulzeiten.Count);
                                Schulzeit schulzeit2 = schulzeit2 = schulzeiten[selected];
                                schulzeiten.RemoveAt(selected);

                                listStundenplanEintraege.Add((
                                    schulzeit2, lehrer, klassenFach.fach, klasse, lehrort
                                ));
                            }
                        }

                        // 3. Fremdsprache && Weltanschauung
                        foreach (var listFaecher in new List<(int, string)>[] { listDrittSprachenFaecher, listWeltanschauungsFaecher }){
                            int selected = wuerfel.Next(schulzeiten.Count);
                            Schulzeit schulzeit1 = schulzeiten[selected];
                            schulzeiten.RemoveAt(selected);

                            Schulzeit? schulzeit2 = null;
                            if (schulzeiten.Count != 0)
                            {
                                selected = wuerfel.Next(schulzeiten.Count);
                                schulzeit2 = schulzeit2 = schulzeiten[selected];
                                schulzeiten.RemoveAt(selected);
                            }

                            foreach ((int fach, string fachName) klassenFach in listFaecher)
                            {
                                List<int> listFachLehrer = listLehrerKlassenfach.Where(s => s.fach == klassenFach.fach).Select(s => s.lehrer).ToList();
                                int lehrer = listFachLehrer[wuerfel.Next(listFachLehrer.Count)];
                                int lehrort;
                                {
                                    var list = listLehrort.Where(l => l.idGebaeude == Gebaeude.GebaeudeEins || l.idGebaeude == Gebaeude.GebaeudeZwei).ToList();
                                    lehrort = list[wuerfel.Next(list.Count)].ID;
                                }

                                listStundenplanEintraege.Add((
                                    schulzeit1, lehrer, klassenFach.fach, klasse, lehrort
                                ));

                                if (schulzeit2 != null)
                                {
                                    listStundenplanEintraege.Add((
                                        schulzeit2, lehrer, klassenFach.fach, klasse, lehrort
                                    ));
                                }
                            }
                        }
                    }

                    var transactionUpdateKlassenStundenplan = mainTransaction.Begin();
                    try
                    {
                        foreach (var entry in listStundenplanEintraege)
                        {
                            using OleDbCommand cmdInsertStundenplanEintrag = conn.CreateCommand();
                            cmdInsertStundenplanEintrag.Transaction = transactionUpdateKlassenStundenplan;
                            cmdInsertStundenplanEintrag.CommandText = "INSERT INTO StundenplanEintrag " +
                                "(schultag, zeitslot, lehrer, fach, klasse, lehrort) VALUES " +
                                "(@schultag, @zeitslot, @lehrer, @fach, @klasse, @lehrort)";
                            cmdInsertStundenplanEintrag.Parameters.Add("@schultag", OleDbType.Integer).Value = entry.zeit.Schultag;
                            cmdInsertStundenplanEintrag.Parameters.Add("@zeitslot", OleDbType.Integer).Value = entry.zeit.ZeitSlot;
                            cmdInsertStundenplanEintrag.Parameters.Add("@lehrer", OleDbType.Integer).Value = entry.lehrer;
                            cmdInsertStundenplanEintrag.Parameters.Add("@fach", OleDbType.Integer).Value = entry.fach;
                            cmdInsertStundenplanEintrag.Parameters.Add("@klasse", OleDbType.Integer).Value = entry.klasse;
                            cmdInsertStundenplanEintrag.Parameters.Add("@lehrort", OleDbType.Integer).Value = entry.lehrort;
                            cmdInsertStundenplanEintrag.ExecuteNonQuery();

                            int eintragId = GetIdAfterInsert(conn, transactionUpdateKlassenStundenplan);

                            using OleDbCommand cmdInsertKlassenStundenplan = conn.CreateCommand();
                            cmdInsertKlassenStundenplan.Transaction = transactionUpdateKlassenStundenplan;
                            cmdInsertKlassenStundenplan.CommandText = "INSERT INTO KlassenStundenplan " +
                                "(eintrag, klasse) VALUES " +
                                "(@eintrag, @klasse)";
                            cmdInsertKlassenStundenplan.Parameters.Add("@eintrag", OleDbType.Integer).Value = eintragId;
                            cmdInsertKlassenStundenplan.Parameters.Add("@klasse", OleDbType.Integer).Value = entry.klasse;
                            cmdInsertKlassenStundenplan.ExecuteNonQuery();
                        }
                        transactionUpdateKlassenStundenplan.Commit();
                    }
                    catch (Exception ex) {
                        transactionUpdateKlassenStundenplan.Rollback();
                        throw;
                    }

                    mainTransaction.Commit();
                }
                catch (Exception ex)
                {
                    mainTransaction.Rollback();
                    Assert.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private void ProcessCsvFile(
            OleDbConnection conn,
            OleDbTransaction mainTransaction,
            string csvFile,
            string roleTable
        )
        {
            bool modified = false;
            Regex regexWhitespace = RegexWhitespace();
            Regex regexDatum = RegexDatum();

            int lineNumber = 1;
            bool first = true;
            var (count, indexGivenName, indexSurname, indexBirthday) = (0, -1, -1, -1);
            foreach (string line in File.ReadLines(csvFile, Encoding.UTF8))
            {
                string ersterVorname, andereVornamen, nachnamen;
                DateTime dateGeburtstag;

                try
                {
                    string[] columns = line.Split(';');
                    if (columns.Length <= count)
                    {
                        continue;
                    }

                    if (first)
                    {
                        first = false;
                        indexGivenName = Array.FindIndex(columns, c => c.Trim().Equals("givenname", StringComparison.InvariantCultureIgnoreCase));
                        indexSurname = Array.FindIndex(columns, c => c.Trim().Equals("surname", StringComparison.InvariantCultureIgnoreCase));
                        indexBirthday = Array.FindIndex(columns, c => c.Trim().Equals("birthday", StringComparison.InvariantCultureIgnoreCase));

                        if (indexGivenName == -1 || indexSurname == -1 || indexBirthday == -1)
                        {
                            throw new FileFormatException($"Invalid file format: {csvFile}");
                        }
                        count = new int[] { indexGivenName, indexSurname, indexBirthday }.Max();
                        continue;
                    }

                    string vornamen = columns[indexGivenName].Trim();
                    nachnamen = columns[indexSurname].Trim();
                    string strGeburtstag = columns[indexBirthday].Trim();
                    if (
                        string.IsNullOrWhiteSpace(vornamen) ||
                        string.IsNullOrWhiteSpace(nachnamen) ||
                        string.IsNullOrWhiteSpace(strGeburtstag) ||
                        !regexDatum.IsMatch(strGeburtstag)
                    )
                    {
                        Debug.WriteLine($"Skip invalid entry (line {lineNumber}): '{line.Trim()}'");
                        continue;
                    }

                    string?[] splitVornamen = regexWhitespace.Replace(vornamen, " ").Split(' ', 2);
                    Array.Resize(ref splitVornamen, 2);

                    ersterVorname = splitVornamen[0] ?? "";
                    andereVornamen = splitVornamen[1] ?? "";

                    dateGeburtstag = DateTime.ParseExact(
                        strGeburtstag,
                        formats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None
                    );
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Skip invalid entry (line {lineNumber}): '{line.Trim()}'");
                    continue;
                }
                finally
                {
                    lineNumber++;
                }

                using OleDbTransaction insertTransaction = mainTransaction.Begin();
                try
                {
                    using OleDbCommand cmdInsertPerson = conn.CreateCommand();
                    cmdInsertPerson.Transaction = insertTransaction;
                    cmdInsertPerson.CommandText = "INSERT INTO Person (" +
                        "nachname, ersterVorname, andereVornamen, geburtstag" +
                    ") VALUES (@nachname, @ersterVorname, @andereVornamen, @geburtstag)";
                    cmdInsertPerson.Parameters.Add("@nachname", OleDbType.VarChar).Value = nachnamen;
                    cmdInsertPerson.Parameters.Add("@ersterVorname", OleDbType.VarChar).Value = ersterVorname;
                    cmdInsertPerson.Parameters.Add("@andereVornamen", OleDbType.VarChar).Value = andereVornamen;
                    cmdInsertPerson.Parameters.Add("@geburtstag", OleDbType.Date).Value = dateGeburtstag.Date;

                    cmdInsertPerson.ExecuteNonQuery();

                    int PersonId = GetIdAfterInsert(conn, insertTransaction);

                    using OleDbCommand cmdSavePersonId = conn.CreateCommand();
                    cmdSavePersonId.Transaction = insertTransaction;
                    cmdSavePersonId.CommandText = $"INSERT INTO {roleTable} (Person) VALUES (@PersonID)";
                    cmdSavePersonId.Parameters.Add("@PersonId", OleDbType.Integer).Value = PersonId;
                    cmdSavePersonId.ExecuteNonQuery();

                    insertTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    insertTransaction.Rollback();
                }

                modified = true;
            }

            if (indexGivenName == -1 || indexSurname == -1 || indexBirthday == -1 || !modified)
            {
                throw new FileFormatException($"Invalid file format: {csvFile}");
            }
        }

        private int GetIdAfterInsert(OleDbConnection conn, OleDbTransaction transaction)
        {
            using OleDbCommand cmdGetIdAfterInsert = conn.CreateCommand();
            cmdGetIdAfterInsert.Transaction = transaction;
            cmdGetIdAfterInsert.CommandText = "Select @@Identity";
            return (int)(cmdGetIdAfterInsert.ExecuteScalar() ?? throw new Exception()); ;
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex RegexWhitespace();

        [GeneratedRegex(@"^[0-9]?[0-9]/[0-9]?[0-9]/[0-9][0-9][0-9][0-9]$")]
        private static partial Regex RegexDatum();
    }
}
