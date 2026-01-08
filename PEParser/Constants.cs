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
        public const string PE_MAGIC = "PE";
        
        public class SectionName
        {
            /// <summary>
            /// Contains the executable code of the program.
            /// </summary>
            public const string TEXT = ".text";
            /// <summary>
            /// Contains the initialized data.
            /// </summary>
            public const string DATA = ".data";
            /// <summary>
            /// Contains read-only initialized data.
            /// </summary>
            public const string RDATA = ".rdata";
            /// <summary>
            /// 
            /// </summary>
            public const string PDATA = ".pdata";
            /// <summary>
            /// Contains the import tables.
            /// </summary>
            public const string IDATA = ".idata";
            /// <summary>
            /// Contains the export tables.
            /// </summary>
            public const string EDATA = ".edata";
            /// <summary>
            /// Contains image relocation information.
            /// </summary>
            public const string RELOC = ".reloc";
            /// <summary>
            /// Contains resources used by the program, these include images, icons or even embedded binaries.
            /// </summary>
            public const string RSRC = ".rsrc";
            /// <summary>
            /// Contains uninitialized data.
            /// </summary>
            public const string BSS = ".bss";
            /// <summary>
            /// (Thread Local Storage), provides storage for every executing thread of the program.
            /// </summary>
            public const string TLS = ".tls";
        }
    }
}