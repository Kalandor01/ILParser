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

    public class DataDirectory
    {
        public uint VirtualAddress;
        public uint Size;

        public override string? ToString()
        {
            return $"Address: {VirtualAddress}, Size: {Size}";
        }
    }

    public class PEOptionalHeader
    {
        public PEImageType ImageType;
        public bool Is64Bit => ImageType == PEImageType.PE32_PLUS;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public uint SizeOfCode;
        public uint SizeOfInitializedData;
        public uint SizeOfUninitializedData;
        public uint AddressOfEntryPoint;
        public uint BaseOfCode;
        public uint? BaseOfData;
        public ulong ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion;
        public ushort MinorSubsystemVersion;
        public uint Win32VersionValue;
        public uint SizeOfImage;
        public uint SizeOfHeaders;
        public uint CheckSum;
        public PESubsystemType Subsystem;
        public PEDllCharacteristics[] DllCharacteristics;
        public ulong SizeOfStackReserve;
        public ulong SizeOfStackCommit;
        public ulong SizeOfHeapReserve;
        public ulong SizeOfHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRvaAndSizes;
        public DataDirectory[] DataDirectory;

        public override string? ToString()
        {
            return $"{ImageType} ({string.Join(", ", DllCharacteristics)})";
        }
    }

    public class PEHeader
    {
        public string Magic;
        public PEMachineType Machine;
        public ushort NumberOfSections;
        public DateTimeOffset TimeDateStamp;
        public uint PointerToSymbolTable;
        public uint NumberOfSymbols;
        public ushort SizeOfOptionalHeader;
        public PECharacteristic[] Characteristics;
        public PEOptionalHeader? OptionalHeader;

        public override string? ToString()
        {
            return $"{Machine} ({string.Join(", ", Characteristics)}) {TimeDateStamp}";
        }
    }
    
    public class PEFile
    {
        public PEDOSHeader DOSHeader;
        public PEHeader PEHeader;
    }
}