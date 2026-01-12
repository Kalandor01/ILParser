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
        public const ulong LOOKUP_TABLE_TYPE_MASK_32 = 0x80000000;
        public const ulong LOOKUP_TABLE_TYPE_MASK_64 = 0x8000000000000000;
        public const ushort SECTION_RELOCATION_FIELDS_UPPER_MASK = 0xFFFF;
        public const ushort SECTION_RELOCATION_FIELDS_LOWER_MASK = 0xFFFF;
        
        public class SectionName
        {
            /// <summary>
            /// Contains the executable code of the program.
            /// </summary>
            public const string INSTRUCTIONS = ".text";
            /// <summary>
            /// Contains the initialized data.
            /// </summary>
            public const string DATA = ".data";
            /// <summary>
            /// Contains read-only initialized data.
            /// </summary>
            public const string READ_ONLY_DATA = ".rdata";
            /// <summary>
            /// Contains platform specific data data.
            /// </summary>
            public const string PLATFORM_SPEC_DATA = ".pdata";
            /// <summary>
            /// Contains the import tables.
            /// </summary>
            public const string IMPORT_DATA = ".idata";
            /// <summary>
            /// Contains the export tables.
            /// </summary>
            public const string EXPORT_DATA = ".edata";
            /// <summary>
            /// Contains image relocation information.
            /// </summary>
            public const string RELOCATION_INFO = ".reloc";
            /// <summary>
            /// Contains resources used by the program, these include images, icons or even embedded binaries.
            /// </summary>
            public const string RESOURCES = ".rsrc";
            /// <summary>
            /// Contains uninitialized data.
            /// </summary>
            public const string UNINITIALIZED_DATA = ".bss";
            /// <summary>
            /// (Thread Local Storage), provides storage for every executing thread of the program.
            /// </summary>
            public const string THREAD_LOCAL_STORAGE = ".tls";
        }
    }
}