using System.Text;

namespace ELFParser
{
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

    public class ELFProgramHeaderRaw
    {
        public ELFEnums.ELFProgramHeaderType Type => GetTypeFromTypeNum(TypeNum);
        public uint TypeNum;
        public ELFEnums.ELFProgramHeaderFlag[] Flags;
        public ulong Offset;
        public ulong VirtualAddress;
        public ulong PhysicalAddress;
        public ulong FileSize;
        public ulong MemorySize;
        public ulong Alignment;
        public byte[] Data;
        
        public static ELFEnums.ELFProgramHeaderType GetTypeFromTypeNum(uint typeInt)
        {
            return typeInt switch
            {
                >= Constants.ELF.PROGRAM_HEADER_TYPE_OS_SPECIFIC_LOW and <= Constants.ELF.PROGRAM_HEADER_TYPE_OS_SPECIFIC_HIGH =>
                    Enum.IsDefined((ELFEnums.ELFProgramHeaderType)typeInt)
                        ? (ELFEnums.ELFProgramHeaderType)typeInt
                        : ELFEnums.ELFProgramHeaderType.OS_SPECIFIC,
                >= Constants.ELF.PROGRAM_HEADER_TYPE_PROC_SPECIFIC_LOW and <= Constants.ELF.PROGRAM_HEADER_TYPE_PROC_SPECIFIC_HIGH =>
                    Enum.IsDefined((ELFEnums.ELFProgramHeaderType)typeInt)
                        ? (ELFEnums.ELFProgramHeaderType)typeInt
                        : ELFEnums.ELFProgramHeaderType.PROC_SPECIFIC,
                _ => (ELFEnums.ELFProgramHeaderType)typeInt,
            };
        }

        public override string? ToString()
        {
            return $"{Type} ({string.Join(", ", Flags)})";
        }
    }

    public class ELFSectionHeaderRaw
    {
        public uint Name;
        public ELFEnums.ELFSectionHeaderType Type => GetTypeFromTypeNum(TypeNum);
        public uint TypeNum;
        public ELFEnums.ELFSectionHeaderFlag[] Flags;
        public ulong VirtualAddress;
        public ulong Offset;
        public ulong Size;
        public uint Link;
        public uint Info;
        public ulong AddressAlign;
        public ulong EntrySize;
        public byte[] Data;
        
        public static ELFEnums.ELFSectionHeaderType GetTypeFromTypeNum(uint typeInt)
        {
            return typeInt switch
            {
                >= Constants.ELF.SECTION_HEADER_TYPE_OS_SPECIFIC_LOW =>
                    Enum.IsDefined((ELFEnums.ELFSectionHeaderType)typeInt)
                        ? (ELFEnums.ELFSectionHeaderType)typeInt
                        : ELFEnums.ELFSectionHeaderType.OS_SPECIFIC,
                _ => (ELFEnums.ELFSectionHeaderType)typeInt,
            };
        }

        public override string? ToString()
        {
            return $"{Type} ({string.Join(", ", Flags)})";
        }
    }

    public class ELFFileRaw
    {
        public ELFHeaderRaw Header;
        public ELFProgramHeaderRaw[] ProgramHeaderTable;
        public ELFSectionHeaderRaw[] SectionHeaderTable;

        public string? ResolveStringByIndex(uint stringStartIndex)
        {
            return GetNullTerminatedString(SectionHeaderTable[Header.SectionNameStringSectionHeaderTableIndex].Data, stringStartIndex);
        }

        public override string? ToString()
        {
            return Header.ToString();
        }

        private static string? GetNullTerminatedString(byte[] bytes, uint startIndex)
        {
            var strBytes = new List<byte>();

            var b = bytes[startIndex];
            while (b != 0)
            {
                strBytes.Add(b);
                startIndex++;
                b = bytes[startIndex];
            }
            return strBytes.Count != 0
                ? Encoding.UTF8.GetString(strBytes.ToArray())
                : null;
        }
    }
}