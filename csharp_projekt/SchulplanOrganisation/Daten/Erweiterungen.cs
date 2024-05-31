using System.Data;
using System.Data.Common;

namespace SchulplanOrganisation.Daten
{
    public static class Erweiterungen
    {
        public static string Name(this Gebaeude gebaeude) => gebaeude switch
        {
            Gebaeude.GebaeudeEins => "G1",
            Gebaeude.GebaeudeZwei => "G2",
            Gebaeude.Sporthalle => "Sporthalle",
            Gebaeude.Sonstiges => "Sonstiges",
            _ => throw new NotImplementedException()
        };

        public static string Name(this Schultag schultag) => schultag switch
        {
            Schultag.Montag => "Montag",
            Schultag.Dienstag => "Dienstag",
            Schultag.Mittwoch => "Mittwoch",
            Schultag.Donnerstag => "Donnerstag",
            Schultag.Freitag => "Freitag",
            _ => throw new NotImplementedException()
        };

        public static string Name(this ZeitSlot zeitSlot) => zeitSlot switch
        {
            ZeitSlot.Stunde1 => "1. Stunde",
            ZeitSlot.Stunde2 => "2. Stunde",
            ZeitSlot.Pause1 => "1. Pause",
            ZeitSlot.Stunde3 => "3. Stunde",
            ZeitSlot.Stunde4 => "4. Stunde",
            ZeitSlot.Pause2 => "2. Pause",
            ZeitSlot.Stunde5 => "5. Stunde",
            ZeitSlot.Stunde6 => "6. Stunde",
            ZeitSlot.Pause3 => "Lange Pause",
            ZeitSlot.Stunde7 => "7. Stunde",
            ZeitSlot.Stunde8 => "8. Stunde",
            ZeitSlot.Pause4 => "4. Pause",
            ZeitSlot.Stunde9 => "9. Stunde",
            ZeitSlot.StundeX => "10. Stunde",
            _ => throw new NotImplementedException()
        };

        public static bool IsStunde(this ZeitSlot zeitSlot) => ((int)zeitSlot) > 0;

        public static bool IsPause(this ZeitSlot zeitSlot) => ((int)zeitSlot) < 0;

        public static TimeOnly Start(this ZeitSlot zeitSlot) => zeitSlot switch
        {
            ZeitSlot.Stunde1 => new TimeOnly(07, 50, 00),
            ZeitSlot.Stunde2 => new TimeOnly(08, 35, 00),
            ZeitSlot.Pause1 => new TimeOnly(09, 20, 00),
            ZeitSlot.Stunde3 => new TimeOnly(09, 40, 00),
            ZeitSlot.Stunde4 => new TimeOnly(10, 25, 00),
            ZeitSlot.Pause2 => new TimeOnly(11, 10, 00),
            ZeitSlot.Stunde5 => new TimeOnly(11, 30, 00),
            ZeitSlot.Stunde6 => new TimeOnly(12, 15, 00),
            ZeitSlot.Pause3 => new TimeOnly(13, 00, 00),
            ZeitSlot.Stunde7 => new TimeOnly(13, 45, 00),
            ZeitSlot.Stunde8 => new TimeOnly(14, 30, 00),
            ZeitSlot.Pause4 => new TimeOnly(15, 15, 00),
            ZeitSlot.Stunde9 => new TimeOnly(15, 30, 00),
            ZeitSlot.StundeX => new TimeOnly(16, 15, 00),
            _ => throw new NotImplementedException()
        };

        public static TimeOnly Ende(this ZeitSlot zeitSlot) => zeitSlot switch
        {
            ZeitSlot.Stunde1 => ZeitSlot.Stunde2.Start(),
            ZeitSlot.Stunde2 => ZeitSlot.Pause1.Start(),
            ZeitSlot.Pause1 => ZeitSlot.Stunde3.Start(),
            ZeitSlot.Stunde3 => ZeitSlot.Stunde4.Start(),
            ZeitSlot.Stunde4 => ZeitSlot.Pause2.Start(),
            ZeitSlot.Pause2 => ZeitSlot.Stunde5.Start(),
            ZeitSlot.Stunde5 => ZeitSlot.Stunde6.Start(),
            ZeitSlot.Stunde6 => ZeitSlot.Pause3.Start(),
            ZeitSlot.Pause3 => ZeitSlot.Stunde7.Start(),
            ZeitSlot.Stunde7 => ZeitSlot.Stunde8.Start(),
            ZeitSlot.Stunde8 => ZeitSlot.Pause4.Start(),
            ZeitSlot.Pause4 => ZeitSlot.Stunde9.Start(),
            ZeitSlot.Stunde9 => ZeitSlot.StundeX.Start(),
            ZeitSlot.StundeX => new TimeOnly(17, 00, 00),
            _ => throw new NotImplementedException()
        };

        public static long GetLong(this DbDataReader reader, string name)
        {
            Type type = reader.GetFieldType(name);
            if (type == typeof(Int64))
            {
                return reader.GetInt64(name);
            }
            else if (type == typeof(Int32))
            {
                return reader.GetInt32(name);
            }
            else if (type == typeof(Int16))
            {
                return reader.GetInt16(name);
            }
            else
            {
                throw new Exception($"unsupported type: {type}");
            }
        }

        public static int GetInt(this DbDataReader reader, string name)
        {
            Type type = reader.GetFieldType(name);
            if (type == typeof(Int64))
            {
                long lValue = reader.GetInt64(name);
                if (lValue < ((long)int.MinValue) || lValue > ((long)int.MaxValue))
                {
                    throw new Exception("long value does not fit into an int value");
                }
                return (int)lValue;
            }
            else if (type == typeof(Int32))
            {
                return reader.GetInt32(name);
            }
            else if (type == typeof(Int16))
            {
                return reader.GetInt16(name);
            }
            else
            {
                throw new Exception($"unsupported type: {type}");
            }
        }
    }
}
