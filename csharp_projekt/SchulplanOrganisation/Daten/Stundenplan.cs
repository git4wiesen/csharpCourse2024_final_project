namespace SchulplanOrganisation.Daten
{
    public interface IStundenplan {
        public Unterricht? Get(Schulzeit zeit);
        public void Remove(Schulzeit zeit);
        public void Set(Schulzeit zeit, Unterricht unterricht);
    };

    public sealed class Stundenplan: IStundenplan
    {
        private readonly Dictionary<Schulzeit, Unterricht> eintraege = [];

        public Unterricht? Get(Schulzeit zeit) => eintraege[zeit];
        public void Remove(Schulzeit zeit) => eintraege.Remove(zeit);
        public void Set(Schulzeit zeit, Unterricht unterricht) {
            if(eintraege.ContainsKey(zeit))
            {
                throw new Exception("Andere Unterrichtseinheit ist bereits gesetzt"); 
            }
            eintraege[zeit] = unterricht;
        }
    }

    public sealed class SchuelerStundenplan(
        Schueler schueler,
        Klasse klasse
    ) : IStundenplan
    {
        private readonly Stundenplan wahlfachStundenplan = new();

        public Schueler Schueler { get; set; } = schueler;

        public Klasse Klasse { get; set; } = klasse;

        public Stundenplan KlassenStundenplan { get => Klasse.Stundenplan; }

        public Stundenplan WahlfachStundenplan { get => wahlfachStundenplan; }

        public Unterricht? Get(Schulzeit zeit) => Klasse.Stundenplan.Get(zeit) ?? wahlfachStundenplan.Get(zeit);
        public void Remove(Schulzeit zeit)
        {
            if (Klasse.Stundenplan.Get(zeit) != null) {
                throw new InvalidOperationException("Klassen-Unterrichtseinheiten dürfen hier nicht entfernt werden");
            }
            wahlfachStundenplan.Remove(zeit);
        }
        public void Set(Schulzeit zeit, Unterricht unterricht)
        {
            if (Klasse.Stundenplan.Get(zeit) != null)
            {
                throw new InvalidOperationException("Klassen-Unterrichtseinheiten dürfen hier nicht überlagert werden");
            }
            wahlfachStundenplan.Set(zeit, unterricht);
        }

    }
}
