namespace SchulplanOrganisation.Daten
{
    public class Schueler : Person
    {
        private static readonly DateTime MAX_GEBURTSTAG = new(DateTime.Today.Year - 6, 9, 30);

        private Klasse klasse;

        public Schueler(
            Klasse klasse,
            string nachname = "",
            string ersterVorname = "",
            string andereVorname = ""
        ) : this(
            klasse: klasse,
            nachname: nachname,
            ersterVorname: ersterVorname,
            andereVorname: andereVorname,
            geburtstag: new(2018, 1, 1)
        )
        {
        }
        public Schueler(
            Klasse klasse,
            DateTime geburtstag,
            string nachname = "",
            string ersterVorname = "",
            string andereVorname = ""
        ) : base(
            nachname,
            ersterVorname,
            andereVorname,
            geburtstag
        )
        {
            this.klasse = klasse;
            klasse.Hinzufuegen(this);
            Stundenplan = new SchuelerStundenplan(this, klasse);
        }

        public Klasse Klasse {
            get => klasse;
            set {
                klasse = value;
                ((SchuelerStundenplan)Stundenplan).Klasse = value;
            }
        }

        protected override void CheckGeburtstag(DateTime geburtstag)
        {
            if (MAX_GEBURTSTAG.CompareTo(geburtstag) == -1)
            {
                throw new ArgumentException("Das Kind ist zu jung für die Schule!");
            }
        }

        protected override void CheckStundenplan(IStundenplan stundenplan)
        {
            if (stundenplan is not SchuelerStundenplan)
            {
                throw new ArgumentException("ein Schüler muss einen Schüler-Stundenplan haben", nameof(Stundenplan));
            }
        }

        protected override void OnChangeStundenplan(IStundenplan stundenplan)
        {
            if (stundenplan is SchuelerStundenplan schuelerStundenplan)
            {
                Klasse altKlasse = schuelerStundenplan.Klasse;
                if(!Object.ReferenceEquals(Klasse, altKlasse))
                {
                    altKlasse.Entfernen(this);
                    Klasse.Hinzufuegen(this);
                    schuelerStundenplan.Klasse = Klasse;
                }
            }
        }
    }
}
