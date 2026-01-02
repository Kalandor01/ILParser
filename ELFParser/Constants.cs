namespace ELFParser
{
    public class Constants
    {
        public class ELF
        {
            public const string MAGIC_STRING = "7F_ELF";
            public const uint PROGRAM_HEADER_TYPE_OS_SPECIFIC_LOW = 0x60000000;
            public const uint PROGRAM_HEADER_TYPE_OS_SPECIFIC_HIGH = 0x6FFFFFFF;
            public const uint PROGRAM_HEADER_TYPE_PROC_SPECIFIC_LOW = 0x70000000;
            public const uint PROGRAM_HEADER_TYPE_PROC_SPECIFIC_HIGH = 0x7FFFFFFF;
            public const uint SECTION_HEADER_TYPE_OS_SPECIFIC_LOW = 0x60000000;
        }
    }
}