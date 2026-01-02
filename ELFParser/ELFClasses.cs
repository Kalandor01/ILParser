using System.Text;

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
        public ELFEnums.ELFProgramHeaderType Type => ELFProgramHeaderRaw.GetTypeFromTypeNum(TypeNum);
        public uint TypeNum;
        public ELFEnums.ELFProgramHeaderFlag[] Flags;
        public object Data;
        public string DataStr => Data is byte[] bytes ? Encoding.UTF8.GetString(bytes) : "";

        public override string? ToString()
        {
            return $"{Type} ({string.Join(", ", Flags)})";
        }
    }

    public class ELFSectionHeader
    {
        public string? Name;
        public ELFEnums.ELFSectionHeaderType Type => ELFSectionHeaderRaw.GetTypeFromTypeNum(TypeNum);
        public uint TypeNum;
        public ELFEnums.ELFSectionHeaderFlag[] Flags;
        public ELFSectionHeader? Link;
        public uint Info;
        public object? Data;
        public string DataStr => Data is byte[] bytes ? Encoding.UTF8.GetString(bytes) : "";

        public override string? ToString()
        {
            return $"{Type}: {Name} ({string.Join(", ", Flags)})";
        }
    }

    public class ELFFile
    {
        public ELFHeader Header;
        public ELFProgramHeader[] ProgramHeaders;
        public ELFSectionHeader[] SectionHeaders;

        public override string? ToString()
        {
            return Header.ToString();
        }
    }
}