namespace ELFParser
{
    internal static class Constants
    {
        public const string MAGIC_STRING = "7F_ELF";
        public const uint PROGRAM_HEADER_TYPE_OS_SPECIFIC_LOW = 0x60000000;
        public const uint PROGRAM_HEADER_TYPE_OS_SPECIFIC_HIGH = 0x6FFFFFFF;
        public const uint PROGRAM_HEADER_TYPE_PROC_SPECIFIC_LOW = 0x70000000;
        public const uint PROGRAM_HEADER_TYPE_PROC_SPECIFIC_HIGH = 0x7FFFFFFF;
        public const uint SECTION_HEADER_TYPE_OS_SPECIFIC_LOW = 0x60000000;
        public const byte X64_INSTRUCTION_GROUP_MARKER_MASK = 0b11_000000;
        public const byte X64_GROUP_TYPE_MASK = 0b00_111_000;
        public const byte X64_GROUP_VALUE_MASK = 0b00_000_111;
        
        public static class SectionName
        {
            public const string UNINITIALIZED_DATA = ".bss";
            public const string VERSION_CONTROL_INFO = ".comment";
            public const string DATA = ".data";
            public const string DATA1 = ".data1";
            public const string DEBUG_INFO = ".debug";
            public const string DYNAMIC_LINKING_INFO = ".dynamic";
            public const string SYMBOL_HASH_TABLE = ".hash";
            public const string LINE_NUMBER_INFO = ".line";
            public const string NOTE = ".note";
            public const string READ_ONLY_DATA = ".rodata";
            public const string READ_ONLY_DATA1 = ".rodata1";
            public const string SECTION_NAMES = ".shstrtab";
            public const string STRINGS = ".strtab";
            public const string SYMBOL_TABLE = ".symtab";
            public const string INSTRUCTIONS = ".text";
            public const string INTERPRETER_INFO = ".interp";
            public const string NOTE_ABI_TAG = ".note.ABI-tag";
            public const string NOTE_GNU_BUILD_ID = ".note.gnu.build-id";
            public const string DYNSYM = ".dynsym";
            public const string GNU_VERSION = ".gnu.version";
            public const string GNU_VERSION_R = ".gnu.version_r";
            public const string GNU_HASH = ".gnu.hash";
            public const string DYNSTR = ".dynstr";
            public const string RELA_DYN = ".rela.dyn";
            public const string RELA_PLT = ".rela.plt";
            public const string GCC_EXCEPT_TABLE = ".gcc_except_table";
            public const string EH_FRAME_HDR = ".eh_frame_hdr";
            public const string EH_FRAME = ".eh_frame";
            public const string INIT = ".init";
            public const string FINI = ".fini";
            public const string PLT = ".plt";
            public const string TBSS = ".tbss";
            public const string FINI_ARRAY = ".fini_array";
            public const string INIT_ARRAY = ".init_array";
            public const string DATA_REL_RO = ".data.rel.ro";
            public const string GOT = ".got";
            public const string GOT_PLT = ".got.plt";
            public const string RELRO_PADDING = ".relro_padding";
            public const string TM_CLONE_TABLE = ".tm_clone_table";
            public const string GNU_DEBUGLINK = ".gnu_debuglink";
        }
    }
}