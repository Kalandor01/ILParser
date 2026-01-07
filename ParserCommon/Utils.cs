using System.Numerics;

namespace ParserCommon
{
    public static class Utils
    {
        public static TE[] ParseEnumFlagsFromNum<TE, TN>(TN num)
            where TE : struct, Enum
            where TN : IBinaryInteger<TN>
        {
            return Enum.GetValues<TE>()
                .Where(v => ((TN)(object)v & num) != TN.Zero)
                .ToArray();
        }
        
        public static TE[] ParseEnumFlags<TE>(ushort num)
            where TE : struct, Enum
        {
            return ParseEnumFlagsFromNum<TE, ushort>(num);
        }
        
        public static TE[] ParseEnumFlags<TE>(uint num)
            where TE : struct, Enum
        {
            return ParseEnumFlagsFromNum<TE, uint>(num);
        }
    }
}