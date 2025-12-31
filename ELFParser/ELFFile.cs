namespace ELFParser
{
    public class ELFIdentity
    {
        public string Magic;
        public ELFEnums.ELFClassType ClassType;
        public ELFEnums.ELFDataEncoding DataEncoding;
        public ELFEnums.ELFVersion HeaderVersion;
        public ELFEnums.ELFAbi OsAbi;
        public byte AbiVersion;

        public override string ToString()
        {
            return $"{OsAbi} ({AbiVersion}), x{(ClassType == ELFEnums.ELFClassType.BIT_64 ? "64" : "32")}, {(DataEncoding == ELFEnums.ELFDataEncoding.BID_ENDIAN ? "Big" : "Little")} endian";
        }
    }

    public class ELFHeaderRaw
    {
        public ELFIdentity Identity;
        public ELFEnums.ELFFileType FileType;
        public ELFEnums.ELFArchitecture Architecture;
        public uint FileVersion;
        /// <summary>
        /// The virtual address to which the system first transfers control. If the file has no associated entry point, this member holds zero.
        /// </summary>
        public ulong EntryPointAddress;
        /// <summary>
        /// The program header table's file offset in bytes. If the file has no program header table, this member holds zero.
        /// </summary>
        public ulong ProgramHeaderOffset;
        /// <summary>
        /// The section header table's file offset in bytes. If the file has no section header table, this member holds zero.
        /// </summary>
        public ulong SectionHeaderOffset;
        /// <summary>
        /// The processor-specific flags associated with the file.
        /// </summary>
        public uint Flags;
        /// <summary>
        /// The ELF header's size in bytes.
        /// </summary>
        public ushort HeaderSize;
        /// <summary>
        /// The size in bytes of one entry in the file's program header table; all entries are the same size.
        /// </summary>
        public ushort ProgramHeaderTableEntrySize;
        /// <summary>
        /// The number of entries in the program header table. Thus the product of e_phentsize and e_phnum gives the table's size in bytes. If a file has no program header table, e_phnum holds the value zero.
        /// </summary>
        public ushort ProgramHeadersCount;
        /// <summary>
        /// A section header's size in bytes. A section header is one entry in the section header table; all entries are the same size.
        /// </summary>
        public ushort SectionHeaderTableEntrySize;
        /// <summary>
        /// The number of entries in the section header table. Thus the product of e_shentsize and e_shnum gives the section header table's size in bytes. If a file has no section header table, e_shnum holds the value zero.
        /// </summary>
        public ushort SectionHeadersCount;
        /// <summary>
        /// The section header table index of the entry associated with the section name string table. If the file has no section name string table, this member holds the value 0.
        /// </summary>
        public ushort SectionNameStringSectionHeaderTableIndex;
    }

    public class ELFHeader
    {
        public ELFIdentity Identity;
        public ELFEnums.ELFFileType FileType;
        public ELFEnums.ELFArchitecture Architecture;
        public uint FileVersion;
        /// <summary>
        /// The virtual address to which the system first transfers control. If the file has no associated entry point, this member holds zero.
        /// </summary>
        public ulong EntryPointAddress;
        /// <summary>
        /// The processor-specific flags associated with the file.
        /// </summary>
        public uint Flags;
        /// <summary>
        /// The section header table index of the entry associated with the section name string table. If the file has no section name string table, this member holds the value 0.
        /// </summary>
        public ushort SectionNameStringSectionHeaderTableIndex;

        public override string ToString()
        {
            return $"{FileType}, {Architecture} ({FileVersion})";
        }
    }

    public class ELFProgramHeader
    {
        public ELFEnums.ELFProgramHeaderType Type;
        public ELFEnums.ELFProgramHeaderFlag[] Flags;
        public ulong Offset;
        public ulong VirtualAddress;
        public ulong PhysicalAddress;
        public ulong FileSize;
        public ulong MemorySize;
        public ulong Alignment;

        public override string? ToString()
        {
            return $"{Type} ({string.Join(", ", Flags)})";
        }
    }

    public class ELFSectionHeader
    {
        public uint Name;
        public ELFEnums.ELFSectionHeaderType Type;
        public ELFEnums.ELFSectionHeaderFlag[] Flags;
        public ulong VirtualAddress;
        public ulong Offset;
        public ulong Size;
        public uint Link;
        public uint Info;
        public ulong AddressAlign;
        public ulong EntrySize;

        public override string? ToString()
        {
            return $"{Type} ({string.Join(", ", Flags)})";
        }
    }

    public class ELFFile
    {
        public ELFHeader Header;
        public ELFProgramHeader[] ProgramHeaderTable;
        public ELFSectionHeader[] SectionHeaderTable;

        public override string? ToString()
        {
            return Header.ToString();
        }
    }
}