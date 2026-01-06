namespace PEParser
{
    internal class Constants
    {
        public const string EXE_EXTENSION = ".exe";
        public const string DLL_EXTENSION = ".dll";
        public const string DOS_MAGIC = "MZ";
        public const ushort DOS_HEADER_SIZE = 64;
        public const ushort DOS_RELOCATION_SIZE = sizeof(ushort) * 2;
        public const string RICH_MAGIC = "Rich";
        public const string DAN_MAGIC = "DanS";
        public const int RICH_HEADER_MIN_LENGTH = 32;
        public const int DAN_DATA_SIZE = 8;
    }
}