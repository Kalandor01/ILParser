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

    public class PEHeader
    {
        
    }
    
    public class PEFile
    {
        public PEDOSHeader DOSHeader;
        public PEHeader PEHeader;
    }
}