using ELFParser.Extensions;

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

        public ELFStream CreateElfStream(byte[] bytes)
        {
            var elfStream = new ELFStream(new MemoryStream(bytes));
            elfStream.ConfigureStream(this);
            return elfStream;
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

    #region Program header data classes
    public abstract class AELFNoteInfo
    {
        public string Name;
        public uint Type;

        public override string? ToString()
        {
            return $"{Name} ({Type})";
        }
    }
    
    public class ELFUnknownNoteInfo : AELFNoteInfo
    {
        public byte[] Descriptor;
    }
    
    public class ELFGnuAbiVersionNoteInfo : AELFNoteInfo
    {
        public ELFEnums.GnuAbiVersionNoteSectionOSType OsVersion;
        public uint MajorVersion;
        public uint MinorVersion;
        public uint PatchVersion;

        public ELFGnuAbiVersionNoteInfo(ELFEnums.GnuAbiVersionNoteSectionOSType osVersion, uint majorVersion, uint minorVersion, uint patchVersion)
        {
            Name = "GNU";
            Type = 1;
            OsVersion = osVersion;
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            PatchVersion = patchVersion;
        }

        public ELFGnuAbiVersionNoteInfo(uint[] nums)
            :this((ELFEnums.GnuAbiVersionNoteSectionOSType)nums[0], nums[1], nums[2], nums[3]) { }

        public override string? ToString()
        {
            return $"OS: {OsVersion}, ABI: {MajorVersion}.{MinorVersion}.{PatchVersion}";
        }
    }
    
    public class ELFGnuBuildIdNoteInfo : AELFNoteInfo
    {
        public string BuildId;

        public ELFGnuBuildIdNoteInfo(byte[] buildId)
        {
            Name = "GNU";
            Type = 3;
            BuildId = Convert.ToHexString(buildId);
        }

        public override string? ToString()
        {
            return $"BuildId: {BuildId}";
        }
    }
    #endregion

    public class ELFProgramHeader
    {
        public ELFEnums.ELFProgramHeaderType Type => ELFProgramHeaderRaw.GetTypeFromTypeNum(TypeNum);
        public uint TypeNum;
        public ELFEnums.ELFProgramHeaderFlag[] Flags;
        public object? Data;
        public string DataStr => Data is byte[] bytes ? bytes.ToUtf8String() : "";
        public string DataHex => Data is byte[] bytes ? BitConverter.ToString(bytes) : "";

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
        public string DataStr => Data is byte[] bytes ? bytes.ToUtf8String() : "";
        public string DataHex => Data is byte[] bytes ? BitConverter.ToString(bytes) : "";

        public override string? ToString()
        {
            return $"{Type}: {Name} ({string.Join(", ", Flags)})";
        }
    }

    public class ELFFile
    {
        public ELFHeader Header;
        public ELFProgramHeader? EntryPointHeader;
        public ELFProgramHeader[] ProgramHeaders;
        public ELFSectionHeader[] SectionHeaders;

        public override string? ToString()
        {
            return Header.ToString();
        }
    }

    public interface IAsmInstruction
    {
        public byte InstructionOpcode { get; }
        public object?[] Arguments { get; }
    }

    public class X64AsmInstruction : IAsmInstruction
    {
        public ELFEnums.X64Instruction Instruction;
        public byte InstructionOpcode => (byte)Instruction;
        public object?[] Arguments { get; set; }

        public X64AsmInstruction(ELFEnums.X64Instruction instruction)
        {
            Instruction = instruction;
        }

        public override string? ToString()
        {
            return $"{Instruction}{(Arguments.Length > 0 ? $" ({string.Join(", ", Arguments)})" : "")}";
        }
    }
}