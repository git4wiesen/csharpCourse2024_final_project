using System.Text.RegularExpressions;

namespace SchulplanOrganisation.Daten
{
    public partial class Klasse: IDbElement
    {
        private int jahrgang;
        private int klassenStufe;
        private string klassenName;
        private HashSet<Schueler> klassenKameraden = [];
        private Stundenplan stundenplan;

        public Klasse(
            int jahrgang = 2024,
            int klassenStufe = 1,
            string klassenName = ""
        ): this(
            jahrgang: jahrgang,
            klassenStufe: klassenStufe,
            klassenName: klassenName,
            stundenplan: new()
        )
        {
        }

#pragma warning disable CS8618
        public Klasse(
            Stundenplan stundenplan,
            int jahrgang = 2024,
            int klassenStufe = 1,
            string klassenName = ""
        )
#pragma warning restore CS8618
        {
            Jahrgang = jahrgang;
            KlassenStufe = klassenStufe;
            KlassenName = klassenName;
            Stundenplan = stundenplan;
        }

        public long DbSchluessel { get; set; }

        public int Jahrgang
        {
            get => jahrgang;
            set => jahrgang = value >= 1960 && value < 3000
                ? value
                : throw new ArgumentOutOfRangeException(nameof(Jahrgang));
        }

        public int KlassenStufe
        {
            get => klassenStufe;
            set => klassenStufe = value > 0 && value < 13
                ? value :
                throw new ArgumentOutOfRangeException(nameof(KlassenStufe));
        }

        public string KlassenName
        {
            get => klassenName;
            set => klassenName = RegexKlassenname().IsMatch(value.Trim())
                ? value.Trim().ToLowerInvariant()
                : throw new ArgumentOutOfRangeException(nameof(KlassenName));
        }
        private void CheckStundenplan(IStundenplan stundenplan)
        {
            if(stundenplan is not Daten.Stundenplan) {
                throw new ArgumentException("Eine Klasse kann nur normale Stundenpläne nutzen.", nameof(Stundenplan));
            }
        }

        public Stundenplan Stundenplan {
            get => stundenplan;
            set {
                CheckStundenplan(value);
                stundenplan = value;
            }
        }

        public ISet<Schueler>? KlassenKameraden() => klassenKameraden;

        public void Hinzufuegen(Schueler schueler) => klassenKameraden.Add(schueler);

        public void Entfernen(Schueler schueler) => klassenKameraden.Remove(schueler);

        [GeneratedRegex(@"[-a-zA-Z0-9]*")]
        private static partial Regex RegexKlassenname();
    }
}
