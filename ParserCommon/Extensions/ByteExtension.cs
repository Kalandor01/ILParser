namespace ParserCommon.Extensions
{
    public static class ByteExtension
    {
        extension(byte by)
        {
            public int ExtendInt32()
            {
                const int EXTENSION = unchecked((int)0xFFFF_FF00);
                return by >= 128 ? EXTENSION + by : by;
            }

            public long ExtendInt64()
            {
                const long EXTENSION = unchecked((long)0xFFFF_FFFF_FFFF_FF00);
                return by >= 128 ? EXTENSION + by : by;
            }
        }
    }
}