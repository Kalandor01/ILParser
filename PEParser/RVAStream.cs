namespace PEParser
{
    public class RVAStream
    {
        private readonly PESectionHeaderRaw[] _sections;
        
        public bool Is64Bit { get; private set; }
        
        public RVAStream(PESectionHeaderRaw[] sections, bool is64Bit)
        {
            _sections = sections;
            Is64Bit = is64Bit;
        }
        
        public PESectionHeaderRaw? RVAToSection(uint rva)
        {
            return _sections.FirstOrDefault(s =>
                s.SectionMemoryAddress <= rva &&
                s.SectionMemoryAddress + s.SectionMemorySize >= rva
            );
        }
        
        public byte[]? RVAToBytes(uint rva)
        {
            var found = RVAToSection(rva);
            if (found is null)
            {
                return null;
            }
            
            var beginAddress = (int)(rva - found.SectionMemoryAddress);
            return found.Data[beginAddress..];
        }
        
        public byte[]? RVAToBytes(uint rva, uint size)
        {
            var found = RVAToSection(rva);
            if (found is null)
            {
                return null;
            }
            
            var beginAddress = (int)(rva - found.SectionMemoryAddress);
            var endAddress = (int)(beginAddress + size);
            
            return found.Data[beginAddress..endAddress];
        }
    }
}
