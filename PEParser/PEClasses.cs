using ParserCommon.Extensions;

namespace PEParser
{
    public class DanData
    {
        public PEDOSProductType ProductType;
        public string BuildName;
        public ushort BuildMajorVersion;
        public ushort BuildMinorVersion;
        public uint UseCount;

        public override string? ToString()
        {
            return $"Product: {ProductType}, Build: {BuildName} {BuildMajorVersion}.{BuildMinorVersion}, Use count: {UseCount}";
        }
    }
    
    public class RichHeader
    {
        public string RichMagic;
        public string DanMagic;
        public DanData[] DanDatas;
    }
    
    public class PEDOSHeader
    {
        public string Magic;
        public ushort MinExtraParagraphs;
        public ushort MaxExtraParagraphs;
        public ushort SsValue;
        public ushort SpValue;
        public ushort Checksum;
        public ushort IpValue;
        public ushort CsValue;
        public ushort OverlayNumber;
        public ushort OemId;
        public ushort OemInfo;
        public PEDOSRelocation[] Relocations;
        public byte[] DOSProgramBytes;
        public string DOSProgramStr => DOSProgramBytes.ToUtf8String();
        public RichHeader? RichHeader;
    }

    public class PEDOSRelocation
    {
        public ushort Offset;
        public ushort Segment;
    }

    public class PEDataDirectory
    {
        public PEDataDirectoryType Type;
        public uint RVA;
        public uint Size;
        public object? Data;

        public override string? ToString()
        {
            return $"{Type}: {Data}";
        }
    }

    public class PEOptionalHeader
    {
        public PEImageType ImageType;
        public bool Is64Bit => ImageType == PEImageType.PE32_PLUS;
        public byte LinkerMajorVersion;
        public byte LinkerMinorVersion;
        public uint TextSectionSize;
        public uint DataSectionsSize;
        public uint UninitializedDataSectionsSize;
        public uint EntryPointAddress;
        public uint TextSectionMemoryAddress;
        public uint? DataSectionMemoryAddress;
        public ulong ImageBaseMemoryAddress;
        public uint SectionMemoryAlignment;
        public uint SectionAlignment;
        public ushort OSMajorVersion;
        public ushort OSMinorVersion;
        public ushort ImageMajorVersion;
        public ushort ImageMinorVersion;
        public ushort SubsystemMajorVersion;
        public ushort SubsystemMinorVersion;
        public uint CheckSum;
        public PESubsystemType Subsystem;
        public PEDllFlag[] DllFlags;
        public ulong StackReserveSize;
        public ulong StackCommitSize;
        public ulong HeapReserveSize;
        public ulong HeapCommitSize;
        public PEDataDirectory[] DataDirectories;

        public override string? ToString()
        {
            return $"{ImageType} ({string.Join(", ", DllFlags)})";
        }
    }

    public class PEHeader
    {
        public string Magic;
        public PEMachineType Machine;
        public DateTime CreatedTime;
        public PEFlag[] Flags;
        public PEOptionalHeader? OptionalHeader;

        public override string? ToString()
        {
            return $"{Machine} ({string.Join(", ", Flags)}) {CreatedTime}";
        }
    }
    
    #region Section data classes
    public class PEHintName
    {
        public ushort Hint;
        public string Name;
        
        public override string? ToString()
        {
            return $"{Name} ({Hint})";
        }
    }
    
    public class PEImportLookupTable
    {
        public bool OrdinalAndNotName;
        public ushort OrdinalNumber;
        public PEHintName? HintName;
        
        public override string? ToString()
        {
            return $"{(OrdinalAndNotName ? OrdinalNumber : HintName)}";
        }
    }
    
    public class PEImportDirectoryTable
    {
        public PEImportLookupTable[] ImportLookupTable;
        public int DateTimeStamp;
        public bool Bound => DateTimeStamp == -1;
        public uint FirstForwarderReferenceIndex;
        public string? Name;
        public PEImportLookupTable[] ImportAddressTable;
        
        public override string? ToString()
        {
            return Name;
        }
    }
    #endregion
    
    public class PERelocationFixup
    {
        /// <summary>
        /// Value indicating which type of fixup is to be applied (described above)
        /// </summary>
        public byte FixupType;
        /// <summary>
        /// Offset from starting address specified in the Page RVA field for the block. This offset specifies where the fixup is to be applied.
        /// </summary>
        public ushort Offset;
        
        public override string? ToString()
        {
            return $"Type: {FixupType} +{Offset}";
        }
    }
    
    public class PESectionRelocation
    {
        public uint PageRVA;
        public PERelocationFixup[] Fixups;
    }

    public class PESectionHeader
    {
        public string Name;
        public uint SectionMemorySize;
        public uint SectionMemoryAddress;
        public PESectionRelocation[] Relocations;
        public PESectionFlag[] Flags;
        public object Data;

        public override string? ToString()
        {
            return $"{Name} ({string.Join(", ", Flags)})";
        }
    }
    
    public class PESymbol
    {
        
    }
    
    public class PECLIHeader
    {
        public ushort RuntimeVersionMajor;
        public ushort RuntimeVersionMinor;
        /// <summary>
        /// RVA and size of the physical metadata.
        /// </summary>
        public ulong MetaData;
        public uint Flags;
        /// <summary>
        /// Token for the MethodDef or File of the entry point for the image
        /// </summary>
        public uint EntryPointToken;
        /// <summary>
        /// RVA and size of implementation-specific resources.
        /// </summary>
        public ulong Resources;
        /// <summary>
        /// RVA of the hash data for this PE file used by the CLI loader for binding and versioning
        /// </summary>
        public ulong StrongNameSignature;
        /// <summary>
        /// RVA of an array of locations in the file that contain an array of function pointers (e.g., vtable slots), see below.
        /// </summary>
        public ulong VTableFixups;
    }
    
    public class PEFile
    {
        public PEDOSHeader DOSHeader;
        public PEHeader PEHeader;
        public PESectionHeader[] SectionHeaders;
        public PESymbol[] Symbols;
    }
}
