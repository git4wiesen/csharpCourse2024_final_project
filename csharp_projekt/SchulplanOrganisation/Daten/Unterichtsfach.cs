namespace SchulplanOrganisation.Daten
{
    public class Unterichtsfach: IDbElement, IEquatable<Unterichtsfach?>
    {
        private string name;

#pragma warning disable CS8618
        public Unterichtsfach(string name)
        {
#pragma warning restore CS8618
            Name = name;
        }
        public long DbSchluessel { get; set; }

        public string Name {
            get => name;
            private set {
                string name = value?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(name)) {
                    throw new ArgumentException("value is null or blank", nameof(Name));
                }
                this.name = name;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as Unterichtsfach);

        public bool Equals(Unterichtsfach? other) => other is not null && name == other.name;

        public override int GetHashCode() => name.GetHashCode();

        public static bool operator ==(Unterichtsfach? left, Unterichtsfach? right)
            => EqualityComparer<Unterichtsfach>.Default.Equals(left, right);

        public static bool operator !=(Unterichtsfach? left, Unterichtsfach? right)
            => !(left == right);
    }
}
