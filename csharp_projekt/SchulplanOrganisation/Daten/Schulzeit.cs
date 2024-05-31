
namespace SchulplanOrganisation.Daten
{
    public class Schulzeit : IEquatable<Schulzeit?>
    {
        public Schultag Schultag { get; set; }
        public ZeitSlot ZeitSlot { get; set; }

        public Schulzeit(Schultag schultag, ZeitSlot zeitSlot)
        {
            Schultag = schultag;
            ZeitSlot = zeitSlot;
        }

        public override bool Equals(object? obj)
            => Equals(obj as Schulzeit);

        public bool Equals(Schulzeit? other)
            => other is not null &&
                   Schultag == other.Schultag &&
                   ZeitSlot == other.ZeitSlot;

        public override string ToString() => $"{Schultag.Name()} {ZeitSlot.Name()}";

        public override int GetHashCode()
            => HashCode.Combine(Schultag, ZeitSlot);

        public static bool operator ==(Schulzeit? left, Schulzeit? right)
            => EqualityComparer<Schulzeit>.Default.Equals(left, right);

        public static bool operator !=(Schulzeit? left, Schulzeit? right)
            => !(left == right);


        public static Schultag[] GetSchultage() => new Schultag[] { Schultag.Montag, Schultag.Dienstag, Schultag.Mittwoch, Schultag.Donnerstag, Schultag.Freitag };
        
        public static ZeitSlot[] GetZeitSlots() => new ZeitSlot[] {
            ZeitSlot.Stunde1 ,
            ZeitSlot.Stunde2 ,
            ZeitSlot.Pause1,
            ZeitSlot.Stunde3 ,
            ZeitSlot.Stunde4 ,
            ZeitSlot.Pause2,
            ZeitSlot.Stunde5 ,
            ZeitSlot.Stunde6 ,
            ZeitSlot.Pause3,
            ZeitSlot.Stunde7 ,
            ZeitSlot.Stunde8 ,
            ZeitSlot.Pause4,
            ZeitSlot.Stunde9 ,
            ZeitSlot.StundeX
        };
        public static ZeitSlot[] GetUnterrichtszeiten() => new ZeitSlot[] {
            ZeitSlot.Stunde1 ,
            ZeitSlot.Stunde2 ,
            ZeitSlot.Stunde3 ,
            ZeitSlot.Stunde4 ,
            ZeitSlot.Stunde5 ,
            ZeitSlot.Stunde6 ,
            ZeitSlot.Stunde7 ,
            ZeitSlot.Stunde8 ,
            ZeitSlot.Stunde9 ,
            ZeitSlot.StundeX
        };

        public static Schulzeit[] GetSchulzeiten() => GetSchultage()
            .SelectMany(tage => GetZeitSlots().Select(zeit => new Schulzeit(tage, zeit)))
            .ToArray();
    }

    public class SchultagComparer : IComparer<Schultag>
    {
        public int Compare(Schultag x, Schultag y) => ((int)x).CompareTo((int)y);
    }

    public class ZeitSlotComparer : IComparer<ZeitSlot>
    {
        public int Compare(ZeitSlot A, ZeitSlot B)
        {
            int valueA = 0 switch
            {
                int when A == ZeitSlot.Stunde1 => 1,
                int when A == ZeitSlot.Stunde2 => 2,
                int when A == ZeitSlot.Pause1 => 3,
                int when A == ZeitSlot.Stunde3 => 4,
                int when A == ZeitSlot.Stunde4 => 5,
                int when A == ZeitSlot.Pause2 => 6,
                int when A == ZeitSlot.Stunde5 => 7,
                int when A == ZeitSlot.Stunde6 => 8,
                int when A == ZeitSlot.Pause3 => 9,
                int when A == ZeitSlot.Stunde7 => 10,
                int when A == ZeitSlot.Stunde8 => 11,
                int when A == ZeitSlot.Pause4 => 12,
                int when A == ZeitSlot.Stunde9 => 13,
                int when A == ZeitSlot.StundeX => 14,
                _ => throw new NotImplementedException()
            };
            int valueB = 0 switch
            {
                int when B == ZeitSlot.Stunde1 => 1,
                int when B == ZeitSlot.Stunde2 => 2,
                int when B == ZeitSlot.Pause1 => 3,
                int when B == ZeitSlot.Stunde3 => 4,
                int when B == ZeitSlot.Stunde4 => 5,
                int when B == ZeitSlot.Pause2 => 6,
                int when B == ZeitSlot.Stunde5 => 7,
                int when B == ZeitSlot.Stunde6 => 8,
                int when B == ZeitSlot.Pause3 => 9,
                int when B == ZeitSlot.Stunde7 => 10,
                int when B == ZeitSlot.Stunde8 => 11,
                int when B == ZeitSlot.Pause4 => 12,
                int when B == ZeitSlot.Stunde9 => 13,
                int when B == ZeitSlot.StundeX => 14,
                _ => throw new NotImplementedException()
            };
            return valueA.CompareTo(valueB);
        }
    }

    public enum Schultag
    {
        Montag = 0,
        Dienstag = 1,
        Mittwoch = 2,
        Donnerstag = 3,
        Freitag = 4
    }

    public enum ZeitSlot
    {
        Stunde1 = 1,    // von 07:50 bis 08:35
        Stunde2 = 2,    // von 08:35 bis 09:20
        Pause1 = -1,    // von 09:20 bis 09:40
        Stunde3 = 3,    // von 09:40 bis 10:25
        Stunde4 = 4,    // von 10:25 bis 11:10
        Pause2 = -2,    // von 11:10 bis 11:30
        Stunde5 = 5,    // von 11:30 bis 12:15
        Stunde6 = 6,    // von 12:15 bis 13:00
        Pause3 = -3,    // von 13:00 bis 13:45
        Stunde7 = 7,    // von 13:45 bis 14:30
        Stunde8 = 8,    // von 14:30 bis 15:15
        Pause4 = -4,    // von 15:15 bis 15:30
        Stunde9 = 9,    // von 15:30 bis 16:15
        StundeX = 10    // von 16:15 bis 17:00
    }
}
