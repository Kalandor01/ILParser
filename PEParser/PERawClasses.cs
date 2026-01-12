using ParserCommon.Extensions;

namespace PEParser
{
    public class PEDOSHeaderRaw
    {
        public string Magic;
        public ushort ByteCountOnLastPage;
        public ushort PageCount;
        public ushort RelocationCount;
        public ushort HeaderSizeInParagraphs;
        public ushort MinExtraParagraphs;
        public ushort MaxExtraParagraphs;
        public ushort SsValue;
        public ushort SpValue;
        public ushort Checksum;
        public ushort IpValue;
        public ushort CsValue;
        public ushort RelocationTableAddress;
        public ushort OverlayNumber;
        public ushort[] Reserved;
        public ushort OemId;
        public ushort OemInfo;
        public ushort[] Reserved2;
        public uint PEHeaderAddress;
        public PEDOSRelocationRaw[] Relocations;
        public byte[] DOSProgramBytes;
        public string DOSProgramStr => DOSProgramBytes.ToUtf8String();
        public RichHeader? RichHeader;
    }

    public class PEDOSRelocationRaw
    {
        public ushort Offset;
        public ushort Segment;
    }
    
    public class PEDataDirectoryRaw
    {
        public PEDataDirectoryType Type;
        public uint RVA;
        public uint Size;
        
        public override string? ToString()
        {
            return $"{Type} Address: {RVA}, Size: {Size}";
        }
    }

    public class PEOptionalHeaderRaw
    {
        public PEImageType ImageType;
        public bool Is64Bit => ImageType == PEImageType.PE32_PLUS;
        public byte LinkerMajorVersion;
        public byte LinkerMinorVersion;
        public uint TextSectionSize;
        public uint DataSectionsSize;
        public uint UninitializedDataSectionsSize;
        public uint EntryPointRVA;
        public uint TextSectionRVA;
        public uint? DataSectionRVA;
        public ulong ImageBaseMemoryAddress;
        public uint SectionMemoryAlignment;
        public uint SectionAlignment;
        public ushort OSMajorVersion;
        public ushort OSMinorVersion;
        public ushort ImageMajorVersion;
        public ushort ImageMinorVersion;
        public ushort SubsystemMajorVersion;
        public ushort SubsystemMinorVersion;
        public uint Win32VersionValue;
        public uint ImageSize;
        public uint HeadersSize;
        public uint CheckSum;
        public PESubsystemType Subsystem;
        public PEDllFlag[] DllFlags;
        public ulong StackReserveSize;
        public ulong StackCommitSize;
        public ulong HeapReserveSize;
        public ulong HeapCommitSize;
        public uint LoaderFlags;
        public uint DataDirectoryCount;
        public PEDataDirectoryRaw[] DataDirectories;

        public override string? ToString()
        {
            return $"{ImageType} ({string.Join(", ", DllFlags)})";
        }
    }

    public class PEHeaderRaw
    {
        public string Magic;
        public PEMachineType Machine;
        public ushort SectionCount;
        public DateTime CreatedTime;
        public uint SymbolTableAddress;
        public uint SymbolCount;
        public ushort OptionalHeaderSize;
        public PEFlag[] Flags;
        public PEOptionalHeaderRaw? OptionalHeader;

        public override string? ToString()
        {
            return $"{Machine} ({string.Join(", ", Flags)}) {CreatedTime}";
        }
    }
    
    public class PESectionRelocationRaw
    {
        public uint PageRVA;
        public uint BlockSize;
        public PERelocationFixup[] Fixups;
    }

    public class PESectionHeaderRaw
    {
        public string Name;
        public uint SectionMemorySize;
        public uint SectionMemoryAddress;
        public uint SectionSize;
        public uint SectionAddress;
        public uint RelocationsAddress;
        public uint COFFLineNumbersAddress;
        public ushort RelocationCount;
        public ushort COFFLineNumberCount;
        public PESectionFlag[] Flags;
        public byte[] Data;
        public PESectionRelocationRaw[] Relocations;
        public string DataStr => Data.ToUtf8String();

        public override string? ToString()
        {
            return $"{Name} ({string.Join(", ", Flags)})";
        }
    }
    
    public class PESymbolRaw
    {
        
    }
    
    public class PECLIHeaderRaw
    {
        public uint HeaderSize;
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
        public ulong CodeManagerTable;
        /// <summary>
        /// RVA of an array of locations in the file that contain an array of function pointers (e.g., vtable slots), see below.
        /// </summary>
        public ulong VTableFixups;
        public ulong ExportAddressTableJumps;
        public ulong ManagedNativeHeader;
    }
    
    public class PEFileRaw
    {
        public PEDOSHeaderRaw DOSHeader;
        public PEHeaderRaw PEHeader;
        public PESectionHeaderRaw[] SectionHeaders;
        public PESymbolRaw[] Symbols;
    }
}