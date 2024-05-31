using System.Text.RegularExpressions;

namespace SchulplanOrganisation.Daten
{
    public abstract partial class Person: IDbElement
    {
        private string nachname;
        private string ersterVorname;
        private string andereVornamen;
        private DateTime geburtstag;
        private IStundenplan stundenplan;

#pragma warning disable CS8618
        public Person(
            string nachname,
            string ersterVorname,
            string zweiterVorname,
            DateTime geburtstag
        )
#pragma warning restore CS8618
        {
            Nachname = nachname;
            ErsterVorname = ersterVorname;
            AndereVornamen = zweiterVorname;
            Geburtstag = geburtstag;
        }

        public long DbSchluessel { get; set; }

        public string Nachname { 
            get => nachname;
            set
            {
                string nachname = value.Trim();
                if (!string.IsNullOrWhiteSpace(nachname)) {
                    this.nachname = nachname;
                }
            }
        }
        public string ErsterVorname {
            get => ersterVorname;
            set
            {
                string vorname = value.Trim();
                if (RegexWhitespace().IsMatch(vorname))
                {
                    throw new ArgumentException("only first forename");
                }
                this.ersterVorname = vorname;
            }
        }
        public string AndereVornamen {
            get => andereVornamen;
            set {
                andereVornamen = RegexWhitespace().Replace(value.Trim(), " ");
            }
        }

        protected virtual void CheckGeburtstag(DateTime geburtstag) {}

        public DateTime Geburtstag {
            get => geburtstag;
            set {
                DateTime geburtstag = value.Date;
                CheckGeburtstag(geburtstag);
                this.geburtstag = geburtstag;
            }
        }

        protected virtual void CheckStundenplan(IStundenplan stundenplan) { }
        protected virtual void OnChangeStundenplan(IStundenplan stundenplan) { }

        public IStundenplan Stundenplan {
            get => stundenplan;
            set {
                CheckStundenplan(value);
                OnChangeStundenplan(value);
                stundenplan = value;
            }
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex RegexWhitespace();
    }
}
