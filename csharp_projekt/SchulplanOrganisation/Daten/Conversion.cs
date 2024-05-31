namespace SchulplanOrganisation.Daten
{
    public static class Conversion
    {
        public static int? ConvertToInt32(this object? value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is Int64)
            {
                Int64 value64 = Convert.ToInt64(value);
                if (
                    (Int64)Convert.ToInt64(int.MinValue) <= value64 &&
                    value64 <= (Int64)Convert.ToInt64(int.MaxValue)
                ) {
                    return (Int32)value64;
                }
                throw new Exception("value is out of bounds");
            }
            if (value is Int32)
            {
                return Convert.ToInt32(value);
            }
            if (value is Int16)
            {
                return Convert.ToInt16(value);
            }

            return null;
        }
    }
}
