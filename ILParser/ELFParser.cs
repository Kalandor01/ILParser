using ILParser.Extensions;

namespace ILParser
{
    public static class ELFParser
    {
        #region Public methods
        public static ELFFile Parse(string filePath)
        {
            if (!Path.Exists(filePath) || Path.GetExtension(filePath) != "")
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }

            var elfStream = new ELFStream(File.OpenRead(filePath));
            var rawHeader = ParseRawHeader(elfStream);
            
            var elfFile = new ELFFile
            {
                Header = ParseHeader(rawHeader),
                ProgramHeaderTable = elfStream.ParseArray(rawHeader.ProgramHeadersCount, (long)rawHeader.ProgramHeaderOffset, ParseProgramHeader),
                SectionHeaderTable = elfStream.ParseArray(rawHeader.SectionHeadersCount, (long)rawHeader.SectionHeaderOffset, ParseSectionHeader),
            };
            return elfFile;
        }
        #endregion

        #region Private methods
        private static ELFIdentity GetIdentityAndConfigureStream(ELFStream stream)
        {
            var elfIdentity = new ELFIdentity
            {
                Magic = $"{stream.ReadBytesAsHexString(1)}_{stream.ReadBytesAsString(3)}",
                ClassType = (ELFEnums.ELFClassType)stream.ReadByteB(),
                DataEncoding = (ELFEnums.ELFDataEncoding)stream.ReadByteB(),
                HeaderVersion = (ELFEnums.ELFVersion)stream.ReadByteB(),
                OsAbi = (ELFEnums.ELFAbi)stream.ReadByteB(),
                AbiVersion = stream.ReadByteB(),
            };
            
            stream.ConfigureStream(elfIdentity);
            var padding = stream.ReadBytes(7);
            return elfIdentity;
        }

        private static ELFHeaderRaw ParseRawHeader(ELFStream stream)
        {
            var header = new ELFHeaderRaw
            {
                Identity = GetIdentityAndConfigureStream(stream),
                FileType = (ELFEnums.ELFFileType)stream.ReadUInt16(),
                Architecture = (ELFEnums.ELFArchitecture)stream.ReadUInt16(),
                FileVersion = stream.ReadUInt32(),
                EntryPointAddress = stream.ReadArchitectureDependant(),
                ProgramHeaderOffset = stream.ReadArchitectureDependant(),
                SectionHeaderOffset = stream.ReadArchitectureDependant(),
                Flags = stream.ReadUInt32(),
                HeaderSize = stream.ReadUInt16(),
                ProgramHeaderTableEntrySize = stream.ReadUInt16(),
                ProgramHeadersCount = stream.ReadUInt16(),
                SectionHeaderTableEntrySize = stream.ReadUInt16(),
                SectionHeadersCount = stream.ReadUInt16(),
                SectionNameStringSectionHeaderTableIndex = stream.ReadUInt16(),
            };
            return header;
        }
        
        private static ELFHeader ParseHeader(ELFHeaderRaw rawHeader)
        {
            var header = new ELFHeader
            {
                Identity = rawHeader.Identity,
                FileType = rawHeader.FileType,
                Architecture = rawHeader.Architecture,
                FileVersion = rawHeader.FileVersion,
                EntryPointAddress = rawHeader.EntryPointAddress,
                Flags = rawHeader.Flags,
                SectionNameStringSectionHeaderTableIndex = rawHeader.SectionNameStringSectionHeaderTableIndex,
            };
            return header;
        }

        private static ELFEnums.ELFProgramHeaderFlag[] ParseProgramHeaderFlags(uint flags)
        {
            return Enum.GetValues<ELFEnums.ELFProgramHeaderFlag>().Where(f => ((uint)f & flags) != 0).ToArray();
        }

        private static ELFEnums.ELFSectionHeaderFlag[] ParseSectionHeaderFlags(ulong flags)
        {
            return Enum.GetValues<ELFEnums.ELFSectionHeaderFlag>().Where(f => ((ulong)f & flags) != 0).ToArray();
        }

        private static ELFProgramHeader ParseProgramHeader(ELFStream stream)
        {
            var type = (ELFEnums.ELFProgramHeaderType)stream.ReadUInt32();
            var flags = 0u;
            if (stream.Is64Bit)
            {
                flags = stream.ReadUInt32();
            }
            
            var programHeader = new ELFProgramHeader
            {
                Type = type,
                Offset = stream.ReadArchitectureDependant(),
                VirtualAddress = stream.ReadArchitectureDependant(),
                PhysicalAddress = stream.ReadArchitectureDependant(),
                FileSize = stream.ReadArchitectureDependant(),
                MemorySize = stream.ReadArchitectureDependant(),
            };

            if (!stream.Is64Bit)
            {
                flags = stream.ReadUInt32();
            }

            programHeader.Flags = ParseProgramHeaderFlags(flags);
            programHeader.Alignment = stream.ReadArchitectureDependant();
            
            return programHeader;
        }

        private static ELFSectionHeader ParseSectionHeader(ELFStream stream)
        {
            var sectionHeader = new ELFSectionHeader
            {
                Name = stream.ReadUInt32(),
                Type = (ELFEnums.ELFSectionHeaderType)stream.ReadUInt32(),
                Flags = ParseSectionHeaderFlags(stream.ReadArchitectureDependant()),
                VirtualAddress = stream.ReadArchitectureDependant(),
                Offset = stream.ReadArchitectureDependant(),
                Size = stream.ReadArchitectureDependant(),
                Link = stream.ReadUInt32(),
                Info = stream.ReadUInt32(),
                AddressAlign = stream.ReadArchitectureDependant(),
                EntrySize = stream.ReadArchitectureDependant(),
            };
            return sectionHeader;
        }
        #endregion
    }
}