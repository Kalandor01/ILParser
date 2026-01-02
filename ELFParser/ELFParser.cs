using ELFParser.Extensions;

namespace ELFParser
{
    public static class ELFParser
    {
        #region Public methods
        public static ELFFileRaw Parse(string filePath)
        {
            if (!Path.Exists(filePath) || Path.GetExtension(filePath) != "")
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }

            var elfStream = new ELFStream(File.OpenRead(filePath));
            var rawHeader = ParseHeaderRaw(elfStream);
            
            var elfFile = new ELFFileRaw
            {
                Header = rawHeader,
                ProgramHeaderTable = elfStream.ParseArray(rawHeader.ProgramHeadersCount, (long)rawHeader.ProgramHeaderOffset, ParseProgramHeaderRaw),
                SectionHeaderTable = elfStream.ParseArray(rawHeader.SectionHeadersCount, (long)rawHeader.SectionHeaderOffset, ParseSectionHeaderRaw),
            };
            return elfFile;
        }
        
        public static ELFFile Resolve(ELFFileRaw rawFile)
        {
            var elfFile = new ELFFile
            {
                Header = ParseHeader(rawFile.Header),
                ProgramHeaders = rawFile.ProgramHeaderTable.Select(h => ParseProgramHeader(rawFile, h)).ToArray(),
                SectionHeaders =  rawFile.SectionHeaderTable
                    .Select(h => ParseSectionHeader(rawFile, h))
                    .ToArray(),
            };
            
            ResolveSectionLinks(rawFile.SectionHeaderTable, elfFile.SectionHeaders);
            return elfFile;
        }
        #endregion

        #region Private methods
        #region Stream parsing
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

        private static ELFHeaderRaw ParseHeaderRaw(ELFStream stream)
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

        private static ELFEnums.ELFProgramHeaderFlag[] ParseProgramHeaderFlags(uint flags)
        {
            return Enum.GetValues<ELFEnums.ELFProgramHeaderFlag>().Where(f => ((uint)f & flags) != 0).ToArray();
        }

        private static ELFEnums.ELFSectionHeaderFlag[] ParseSectionHeaderFlags(ulong flags)
        {
            return Enum.GetValues<ELFEnums.ELFSectionHeaderFlag>().Where(f => ((ulong)f & flags) != 0).ToArray();
        }

        private static ELFProgramHeaderRaw ParseProgramHeaderRaw(ELFStream stream)
        {
            var type = stream.ReadUInt32();
            var flags = 0u;
            if (stream.Is64Bit)
            {
                flags = stream.ReadUInt32();
            }
            
            var programHeader = new ELFProgramHeaderRaw
            {
                TypeNum = type,
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
            programHeader.Data = stream.ReadBytes(programHeader.FileSize, (long)programHeader.Offset);
            
            return programHeader;
        }

        private static ELFSectionHeaderRaw ParseSectionHeaderRaw(ELFStream stream)
        {
            var sectionHeader = new ELFSectionHeaderRaw
            {
                Name = stream.ReadUInt32(),
                TypeNum = stream.ReadUInt32(),
                Flags = ParseSectionHeaderFlags(stream.ReadArchitectureDependant()),
                VirtualAddress = stream.ReadArchitectureDependant(),
                Offset = stream.ReadArchitectureDependant(),
                Size = stream.ReadArchitectureDependant(),
                Link = stream.ReadUInt32(),
                Info = stream.ReadUInt32(),
                AddressAlign = stream.ReadArchitectureDependant(),
                EntrySize = stream.ReadArchitectureDependant(),
            };
            sectionHeader.Data = stream.ReadBytes(sectionHeader.Size, (long)sectionHeader.Offset);
            return sectionHeader;
        }
        #endregion

        #region Parse raw values
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

        private static ELFProgramHeader ParseProgramHeader(ELFFileRaw rawFile, ELFProgramHeaderRaw rawHeader)
        {
            var header = new ELFProgramHeader
            {
                TypeNum = rawHeader.TypeNum,
                Flags = rawHeader.Flags,
                Data = rawHeader.Data,
            };
            return header;
        }

        private static ELFSectionHeader ParseSectionHeader(ELFFileRaw rawFile, ELFSectionHeaderRaw rawHeader)
        {
            var header = new ELFSectionHeader
            {
                Name = rawFile.ResolveStringByIndex(rawHeader.Name),
                TypeNum = rawHeader.TypeNum,
                Flags = rawHeader.Flags,
                Info = rawHeader.Info,
                Data = rawHeader.Data,
            };
            return header;
        }

        private static void ResolveSectionLinks(ELFSectionHeaderRaw[] rawSections, ELFSectionHeader[] sections)
        {
            for (var x = 0; x < sections.Length; x++)
            {
                var sectionLink = rawSections[x].Link;
                sections[x].Link = sectionLink != 0
                    ? sections[sectionLink]
                    : null;
            }
        }
        #endregion
        #endregion
    }
}