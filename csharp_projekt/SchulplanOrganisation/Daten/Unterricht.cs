namespace SchulplanOrganisation.Daten
{
    public abstract class Unterricht(
        Schulzeit schulzeit,
        Lehrer lehrer,
        Unterichtsfach unterichtsfach
    ): IDbElement
    {
        public long DbSchluessel { get; set; }

        public Schulzeit Schulzeit { get; set; } = schulzeit;

        public Lehrer Lehrer { get; set; } = lehrer;

        public Unterichtsfach Unterichtsfach { get; set; } = unterichtsfach;

        public virtual Klasse? Klasse { get => null; set { } }

        public virtual ISet<Schueler>? Teilnehmer() => null;
    }

    public class KlassenUnterricht : Unterricht
    {
        private Klasse klasse;

#pragma warning disable CS8618
        public KlassenUnterricht(
            Schulzeit schulzeit,
            Lehrer lehrer,
            Unterichtsfach unterichtsfach,
            Klasse klasse
        ) : base(schulzeit, lehrer, unterichtsfach)
#pragma warning restore CS8618
        {
            Klasse = klasse;
        }

        public override Klasse? Klasse
        {
            get => klasse;
            set => klasse = value ?? throw new ArgumentNullException(nameof(Klasse));
        }

        public override ISet<Schueler>? Teilnehmer() => klasse.KlassenKameraden();
    }

    public class WahlUnterricht : Unterricht
    {
        private HashSet<Schueler> teilnehmer = new();

        public WahlUnterricht(
            Schulzeit schulzeit,
            Lehrer lehrer,
            Unterichtsfach unterichtsfach
        ) : base(schulzeit, lehrer, unterichtsfach)
        {
        }

        public override ISet<Schueler>? Teilnehmer() => teilnehmer;

        public void Hinzufuegen(Schueler schueler) => teilnehmer.Add(schueler);

        public void Entfernen(Schueler schueler) => teilnehmer.Remove(schueler);
    }
}
