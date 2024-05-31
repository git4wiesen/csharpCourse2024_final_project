namespace SchulplanOrganisation.Daten
{
    public class Lehrort : IEquatable<Lehrort?>
    {
        public Gebaeude IdGebaeude { get; private set; }
        public int? IdStockwerk { get; private set; }
        public int? IdRaumNummer { get; private set; }
        public string? IdName { get; private set; }

        private string? name = null;
        public string? Name {
            get {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name;
                }
                return $"{IdGebaeude.Name()}-S{($"{IdStockwerk}" ?? "<?>")}-R{($"{IdRaumNummer}" ?? "<?>")}";
            }
            set => name = value; }

        public string Id {
            get => 0 switch
            {
                int when IdGebaeude != Gebaeude.Sonstiges && IdRaumNummer == -1
                    => $"name.{IdGebaeude.Name().ToLowerInvariant()}-s{IdStockwerk}-r{IdName}",
                int when IdGebaeude != Gebaeude.Sonstiges && IdRaumNummer != -1
                    => $"room.{IdGebaeude.Name().ToLowerInvariant()}-s{IdStockwerk}-{IdRaumNummer}",
                int when IdGebaeude == Gebaeude.Sonstiges
                    => $"sonstiges.{IdName.ToLowerInvariant()}",
                _ => throw new NotImplementedException()
            };
        }

        public Lehrort(
            Gebaeude idGebaeude,
            int? idStockWerk,
            int? idRaumNummer,
            string? idName,
            string? name
        )
        {
            IdGebaeude = idGebaeude;
            IdStockwerk = idStockWerk;
            IdRaumNummer = idRaumNummer;
            IdName = idName;
            Name = name;
        }
        public static Lehrort CreateLehrort(
            string idName,
            string name = ""
        )
        {
            if (string.IsNullOrWhiteSpace(idName))
            {
                throw new ArgumentException("argument is null or blank", nameof(idName));
            }
            idName = idName.Trim();
            name = !string.IsNullOrWhiteSpace(name) ? name.Trim() : idName;

            return new Lehrort(
                idGebaeude: Gebaeude.Sonstiges,
                idStockWerk: null,
                idRaumNummer: null,
                idName: idName,
                name: name
            );
        }

        public static Lehrort CreateLehrort(
            Gebaeude idGebaeude,
            string idName,
            string? name = null,
            int? idStockWerk = null
        )
        {
            if (
                idGebaeude != Gebaeude.GebaeudeEins &&
                idGebaeude != Gebaeude.GebaeudeZwei &&
                idGebaeude != Gebaeude.Sporthalle
            ) {
                throw new ArgumentException("illegal Gebaeude type", nameof(idGebaeude));
            }

            if (string.IsNullOrWhiteSpace(idName))
            {
                throw new ArgumentException("argument is null or blank", nameof(idName));
            }
            idName = idName.Trim();
            name = !string.IsNullOrWhiteSpace(name) ? name.Trim() : idName;

            return new Lehrort(
                idGebaeude: idGebaeude,
                idStockWerk: idStockWerk,
                idRaumNummer: null,
                idName: idName,
                name: name
            );
        }

        public static Lehrort CreateLehrort(
            Gebaeude idGebaeude,
            int idRaumNummer,
            int idStockWerk = 0,
            string name = ""
        )
        {
            if (
                idGebaeude != Gebaeude.GebaeudeEins &&
                idGebaeude != Gebaeude.GebaeudeZwei
            )
            {
                throw new ArgumentException("illegal Gebaeude type", nameof(idGebaeude));
            }
            ArgumentOutOfRangeException.ThrowIfNegative(idRaumNummer, nameof(idRaumNummer));
            name = !string.IsNullOrWhiteSpace(name) ? name.Trim() : $"{idGebaeude.Name()}-S{idStockWerk}-R{idRaumNummer}";

            return new Lehrort(
                idGebaeude: idGebaeude,
                idStockWerk: idStockWerk,
                idRaumNummer: idRaumNummer,
                idName: "",
                name: name
            );
        }

        public override bool Equals(object? obj)
            => Equals(obj as Lehrort);

        public bool Equals(Lehrort? other)
            => other is not null &&
                   IdGebaeude == other.IdGebaeude &&
                   IdStockwerk == other.IdStockwerk &&
                   IdRaumNummer == other.IdRaumNummer &&
                   IdName == other.IdName;
        public override int GetHashCode()
            => HashCode.Combine(IdGebaeude, IdStockwerk, IdRaumNummer, IdName);

        public static bool operator ==(Lehrort? left, Lehrort? right)
            => EqualityComparer<Lehrort>.Default.Equals(left, right);

        public static bool operator !=(Lehrort? left, Lehrort? right)
            => !(left == right);

        public override string ToString()
            => $"Lehrort[{Id}] => {Name}";
    }
}
