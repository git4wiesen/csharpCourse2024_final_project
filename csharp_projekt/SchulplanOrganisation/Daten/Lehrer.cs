namespace SchulplanOrganisation.Daten
{
    public class Lehrer: Person
    {
        private static readonly DateTime MAX_GEBURTSTAG = new(DateTime.Today.Year - 21, 1, 1);

        private readonly HashSet<Unterichtsfach> lehrfaecher = [];

        public Lehrer(
            string nachname = "",
            string ersterVorname = "",
            string andereVorname = ""
        ) : this(
            nachname: nachname,
            ersterVorname: ersterVorname,
            andereVorname: andereVorname,
            geburtstag: new DateTime(2003, 1, 1)
        )
        {
        }

        public Lehrer(
            DateTime geburtstag,
            string nachname = "",
            string ersterVorname = "",
            string andereVorname = ""
        ) : base(
            nachname: nachname,
            ersterVorname: ersterVorname,
            zweiterVorname: andereVorname,
            geburtstag: geburtstag
        )
        {
            Stundenplan = new Stundenplan();
        }

        public void AddLehrfach(Unterichtsfach lehrfach) => lehrfaecher.Add(lehrfach);

        public void RemoveLehrfach(Unterichtsfach lehrfach) => lehrfaecher.Remove(lehrfach);

        protected override void CheckGeburtstag(DateTime geburtstag)
        {
            if (MAX_GEBURTSTAG.CompareTo(geburtstag) == -1)
            {
                throw new ArgumentException("Diese Person ist zu jung um als Lehrer zu arbeiten!");
            }
        }

        protected override void CheckStundenplan(IStundenplan stundenplan)
        {
            if (stundenplan is not Daten.Stundenplan) {
                throw new ArgumentException("ein Lehrer kann nur einen normalen Stundenplan haben", nameof(Stundenplan));
            }
        }
    }
}
