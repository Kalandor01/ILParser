using ParserCommon;
using ParserCommon.Extensions;

namespace PEParser
{
    public static class PEParser
    {
        #region Public methods
        public static PEFileRaw Parse(string filePath)
        {
            if (
                !Path.Exists(filePath) ||
                Path.GetExtension(filePath).ToLower() is not (Constants.EXE_EXTENSION or Constants.DLL_EXTENSION)
            )
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }

            var peStream = File.OpenRead(filePath);
            
            var dosHeader = ParseDOSHeader(peStream);
            var peHeader = ParsePEHeader(peStream, dosHeader);
            var sectionHeaders = peStream.ParseArray(peHeader.SectionCount, ParseSectionHeader);
            
            var peFile = new PEFileRaw
            {
                DOSHeader = dosHeader,
                PEHeader = peHeader,
                SectionHeaders = sectionHeaders,
            };
            return peFile;
        }

        public static PEFile Resolve(PEFileRaw rawFile)
        {
            var peFile = new PEFile
            {
                DOSHeader = ResolveDOSHeader(rawFile.DOSHeader),
                PEHeader = ResolvePEHeader(rawFile.PEHeader),
                SectionHeaders = rawFile.SectionHeaders.Select(ResolveSectionHeader).ToArray(),
            };
            return peFile;
        }
        #endregion

        #region Private methods
        #region Stream parsing
        private static PEDOSHeaderRaw ParseDOSHeader(Stream stream)
        {
            var header = new PEDOSHeaderRaw
            {
                Magic = stream.ReadBytes(2).ToUtf8String(),
                ByteCountOnLastPage = stream.ReadUInt16L(),
                PageCount = stream.ReadUInt16L(),
                RelocationCount = stream.ReadUInt16L(),
                HeaderSizeInParagraphs = stream.ReadUInt16L(),
                MinExtraParagraphs = stream.ReadUInt16L(),
                MaxExtraParagraphs = stream.ReadUInt16L(),
                SsValue = stream.ReadUInt16L(),
                SpValue = stream.ReadUInt16L(),
                Checksum = stream.ReadUInt16L(),
                IpValue = stream.ReadUInt16L(),
                CsValue = stream.ReadUInt16L(),
                RelocationTableAddress = stream.ReadUInt16L(),
                OverlayNumber = stream.ReadUInt16L(),
                Reserved = stream.ParseArray(4, s => s.ReadUInt16L()),
                OemId = stream.ReadUInt16L(),
                OemInfo = stream.ReadUInt16L(),
                Reserved2 = stream.ParseArray(10, s => s.ReadUInt16L()),
                PEHeaderAddress = stream.ReadUInt32L(),
            };
            
            header.Relocations = stream.ParseArray(header.RelocationCount, header.RelocationTableAddress, ParseDOSRelocation);
            header.DOSProgramBytes = ParseDOSProgram(stream, header);
            header.RichHeader = ParseRichHeader(header.DOSProgramBytes, out var richStart);
            if (richStart != header.DOSProgramBytes.Length)
            {
                header.DOSProgramBytes = header.DOSProgramBytes[..richStart];
            }
            return header;
        }

        private static PEDOSRelocationRaw ParseDOSRelocation(Stream stream)
        {
            var relocation = new PEDOSRelocationRaw()
            {
                Offset = stream.ReadUInt16L(),
                Segment = stream.ReadUInt16L(),
            };
            return relocation;
        }

        private static byte[] ParseDOSProgram(Stream stream, PEDOSHeaderRaw dosHeader)
        {
            var programStart = dosHeader.RelocationTableAddress + Constants.DOS_RELOCATION_SIZE * dosHeader.RelocationCount;
            return dosHeader.PEHeaderAddress > programStart
                ? stream.ReadBytes((uint)(dosHeader.PEHeaderAddress - programStart), programStart)
                : [];
        }

        private static (string name, ushort majorVersion, ushort minorVersion) ParseBuildId(ushort buildId)
        {
            return buildId switch
            {
                >= 0x00fd and < 0x010f => ("Visual Studio 2015", 14, 00),
                >= 0x00eb and < 0x00fd => ("Visual Studio 2013", 12, 10),
                >= 0x00d9 and < 0x00eb => ("Visual Studio 2013", 12, 00),
                >= 0x00c7 and < 0x00d9 => ("Visual Studio 2012", 11, 00),
                >= 0x00b5 and < 0x00c7 => ("Visual Studio 2010", 10, 10),
                >= 0x0098 and < 0x00b5 => ("Visual Studio 2010", 10, 00),
                >= 0x0083 and < 0x0098 => ("Visual Studio 2008", 09, 00),
                >= 0x006d and < 0x0083 => ("Visual Studio 2005", 08, 00),
                >= 0x005a and < 0x006d => ("Visual Studio 2003", 07, 10),
                1 => ("Visual Studio", 00, 00),
                _ => ("[UNKNOWN]", buildId, 0),
            };
        }

        private static RichHeader? ParseRichHeader(byte[] dosProgramBytes, out int richHeaderBytesStart)
        {
            richHeaderBytesStart = dosProgramBytes.Length;
            if (dosProgramBytes.Length <= Constants.RICH_HEADER_MIN_LENGTH)
            {
                return null;
            }

            var richHeaderEnd = dosProgramBytes[^16..^8];
            var richMagic = richHeaderEnd[..4].ToUtf8String();
            if (richMagic != Constants.RICH_MAGIC)
            {
                return null;
            }
            
            var richKey = richHeaderEnd[4..];

            var foundDanMagic = false;
            var danData = new List<(ushort typeOrProductId, ushort buildId, uint useCount)>();
            
            var potentialDanData = dosProgramBytes[..^16];
            var danDataStream = new MemoryStream(potentialDanData);
            danDataStream.Position = danDataStream.Length - Constants.DAN_DATA_SIZE;
            while (danDataStream.Position >= Constants.DAN_DATA_SIZE)
            {
                var decodedData = danDataStream.ReadBytes(Constants.DAN_DATA_SIZE)
                    .Select((d, x) => (byte)(d ^ richKey[x % richKey.Length]))
                    .ToArray();
                var maybeDanStr = decodedData[..4].ToUtf8String();
                if (
                    maybeDanStr == Constants.DAN_MAGIC &&
                    danData.Last() is { typeOrProductId: 0, buildId: 0, useCount: 0 })
                {
                    foundDanMagic = true;
                    break;
                }
                
                danData.Add((
                    decodedData[..2].AsUInt16L(),
                    decodedData[2..4].AsUInt16L(),
                    decodedData[4..8].AsUInt32L()
                ));
                danDataStream.Position = Math.Max(0, danDataStream.Position - Constants.DAN_DATA_SIZE * 2);
            }

            if (!foundDanMagic)
            {
                return null;
            }
            
            var richHeader = new RichHeader()
            {
                RichMagic = richMagic,
                DanMagic = Constants.DAN_MAGIC,
                DanDatas = danData
                    .AsEnumerable()
                    .Reverse()
                    .Skip(1)
                    .Select(dd => (dd.typeOrProductId, build: ParseBuildId(dd.buildId), dd.useCount))
                    .Select(dd => new DanData
                    {
                        ProductType = (PEDOSProductType)dd.typeOrProductId,
                        BuildName = dd.build.name,
                        BuildMajorVersion = dd.build.majorVersion,
                        BuildMinorVersion = dd.build.minorVersion,
                        UseCount = dd.useCount,
                    })
                    .ToArray(),
            };
            
            richHeaderBytesStart = dosProgramBytes.Length - 
                (Constants.RICH_HEADER_MIN_LENGTH + Constants.DAN_DATA_SIZE * richHeader.DanDatas.Length);
            return richHeader;
        }

        private static PEHeaderRaw ParsePEHeader(Stream stream, PEDOSHeaderRaw dosHeader)
        {
            stream.Position = dosHeader.PEHeaderAddress;
            var peHeader = new PEHeaderRaw
            {
                Magic = stream.ReadBytes(4).ToUtf8String().TrimNullEnd(),
                Machine = (PEMachineType)stream.ReadUInt16L(),
                SectionCount = stream.ReadUInt16L(),
                CreatedTime = DateTimeOffset.FromUnixTimeSeconds(stream.ReadUInt32L()).DateTime,
                SymbolTableAddress = stream.ReadUInt32L(),
                SymbolCount = stream.ReadUInt32L(),
                OptionalHeaderSize = stream.ReadUInt16L(),
                Flags = Utils.ParseEnumFlags<PEFlag>(stream.ReadUInt16L()),
            };
            peHeader.OptionalHeader = ParseOptionalPEHeader(stream, peHeader.OptionalHeaderSize);
            return peHeader;
        }

        private static PEDataDirectory ParseDataDirectory(Stream stream, int directoryIndex)
        {
            var dataDir = new PEDataDirectory
            {
                Type = (PEDataDirectoryType)directoryIndex,
                MemoryAddressOffset = stream.ReadUInt32L(),
                Size = stream.ReadUInt32L(),
            };
            return dataDir;
        }

        private static PEOptionalHeaderRaw? ParseOptionalPEHeader(Stream stream, ushort headerSize)
        {
            if (headerSize == 0)
            {
                return null;
            }
            
            var imageType = (PEImageType)stream.ReadUInt16L();
            var is64Bit = imageType == PEImageType.PE32_PLUS;
            var header = new PEOptionalHeaderRaw
            {
                ImageType = imageType,
                LinkerMajorVersion = stream.ReadByteB(),
                LinkerMinorVersion = stream.ReadByteB(),
                TextSectionSize = stream.ReadUInt32L(),
                DataSectionsSize = stream.ReadUInt32L(),
                UninitializedDataSectionsSize = stream.ReadUInt32L(),
                EntryPointAddress = stream.ReadUInt32L(),
                TextSectionMemoryAddress = stream.ReadUInt32L(),
                DataSectionMemoryAddress = is64Bit ? null : stream.ReadUInt32L(),
                ImageBaseMemoryAddress = stream.ReadUInt64BitDependantL(is64Bit),
                SectionMemoryAlignment = stream.ReadUInt32L(),
                SectionAlignment = stream.ReadUInt32L(),
                OSMajorVersion = stream.ReadUInt16L(),
                OSMinorVersion = stream.ReadUInt16L(),
                ImageMajorVersion = stream.ReadUInt16L(),
                ImageMinorVersion = stream.ReadUInt16L(),
                SubsystemMajorVersion = stream.ReadUInt16L(),
                SubsystemMinorVersion = stream.ReadUInt16L(),
                Win32VersionValue = stream.ReadUInt32L(),
                ImageSize = stream.ReadUInt32L(),
                HeadersSize = stream.ReadUInt32L(),
                CheckSum = stream.ReadUInt32L(),
                Subsystem = (PESubsystemType)stream.ReadUInt16L(),
                DllFlags = Utils.ParseEnumFlags<PEDllFlag>(stream.ReadUInt16L()),
                StackReserveSize = stream.ReadUInt64BitDependantL(is64Bit),
                StackCommitSize = stream.ReadUInt64BitDependantL(is64Bit),
                HeapReserveSize = stream.ReadUInt64BitDependantL(is64Bit),
                HeapCommitSize = stream.ReadUInt64BitDependantL(is64Bit),
                LoaderFlags = stream.ReadUInt32L(),
                DataDirectoryCount = stream.ReadUInt32L(),
            };

            var dataDirectories = new List<PEDataDirectory>();
            for (var x = 0; x < header.DataDirectoryCount; x++)
            {
                dataDirectories.Add(ParseDataDirectory(stream, x));
            }
            header.DataDirectories = dataDirectories.ToArray();
            return header;
        }

        private static PESectionHeaderRaw ParseSectionHeader(FileStream stream)
        {
            var sectionHeader = new PESectionHeaderRaw
            {
                Name = stream.ReadBytes(8).ToUtf8String().TrimNullEnd(),
                SectionMemorySize = stream.ReadUInt32L(),
                SectionMemoryAddress = stream.ReadUInt32L(),
                SectionSize = stream.ReadUInt32L(),
                SectionAddress = stream.ReadUInt32L(),
                RelocationsAddress = stream.ReadUInt32L(),
                COFFLineNumbersAddress = stream.ReadUInt32L(),
                RelocationCount = stream.ReadUInt16L(),
                COFFLineNumberCount = stream.ReadUInt16L(),
                Flags = Utils.ParseEnumFlags<PESectionFlag>(stream.ReadUInt32L()),
            };
            sectionHeader.Data = stream.ReadBytes(sectionHeader.SectionSize, sectionHeader.SectionAddress);
            
            if (sectionHeader.RelocationsAddress != 0 || sectionHeader.COFFLineNumbersAddress != 0)
            {
                throw new ArgumentException("Relocation table or COFF line number table parsing is not implemented!");
            }
            return sectionHeader;
        }
        #endregion

        #region Raw resolving
        private static PEDOSRelocation ResolveDOSRelocation(PEDOSRelocationRaw rawRelocation)
        {
            var relocation = new PEDOSRelocation()
            {
                Offset = rawRelocation.Offset,
                Segment = rawRelocation.Segment,
            };
            return relocation;
        }
        
        private static PEDOSHeader ResolveDOSHeader(PEDOSHeaderRaw rawHeader)
        {
            var header = new PEDOSHeader
            {
                Magic = rawHeader.Magic,
                MinExtraParagraphs = rawHeader.MinExtraParagraphs,
                MaxExtraParagraphs = rawHeader.MaxExtraParagraphs,
                SsValue = rawHeader.SsValue,
                SpValue = rawHeader.SpValue,
                Checksum = rawHeader.Checksum,
                IpValue = rawHeader.IpValue,
                CsValue = rawHeader.CsValue,
                OverlayNumber = rawHeader.OverlayNumber,
                OemId = rawHeader.OemId,
                OemInfo = rawHeader.OemInfo,
                Relocations = rawHeader.Relocations.Select(ResolveDOSRelocation).ToArray(),
                DOSProgramBytes = rawHeader.DOSProgramBytes,
                RichHeader = rawHeader.RichHeader,
            };
            return header;
        }

        private static PEOptionalHeader? ResolveOptionalPEHeader(PEOptionalHeaderRaw? rawHeader)
        {
            if (rawHeader is null)
            {
                return null;
            }
            
            var header = new PEOptionalHeader
            {
                ImageType = rawHeader.ImageType,
                LinkerMajorVersion = rawHeader.LinkerMajorVersion,
                LinkerMinorVersion = rawHeader.LinkerMinorVersion,
                TextSectionSize = rawHeader.TextSectionSize,
                DataSectionsSize = rawHeader.DataSectionsSize,
                UninitializedDataSectionsSize = rawHeader.UninitializedDataSectionsSize,
                EntryPointAddress = rawHeader.EntryPointAddress,
                TextSectionMemoryAddress = rawHeader.TextSectionMemoryAddress,
                DataSectionMemoryAddress = rawHeader.DataSectionMemoryAddress,
                ImageBaseMemoryAddress = rawHeader.ImageBaseMemoryAddress,
                SectionMemoryAlignment = rawHeader.SectionMemoryAlignment,
                SectionAlignment = rawHeader.SectionAlignment,
                OSMajorVersion = rawHeader.OSMajorVersion,
                OSMinorVersion = rawHeader.OSMinorVersion,
                ImageMajorVersion = rawHeader.ImageMajorVersion,
                ImageMinorVersion = rawHeader.ImageMinorVersion,
                SubsystemMajorVersion = rawHeader.SubsystemMajorVersion,
                SubsystemMinorVersion = rawHeader.SubsystemMinorVersion,
                CheckSum = rawHeader.CheckSum,
                Subsystem = rawHeader.Subsystem,
                DllFlags = rawHeader.DllFlags,
                StackReserveSize = rawHeader.StackReserveSize,
                StackCommitSize = rawHeader.StackCommitSize,
                HeapReserveSize = rawHeader.HeapReserveSize,
                HeapCommitSize = rawHeader.HeapCommitSize,
                DataDirectories = rawHeader.DataDirectories.Where(dd => dd.Size != 0).ToArray(),
            };
            return header;
        }

        private static PEHeader ResolvePEHeader(PEHeaderRaw rawHeader)
        {
            var header = new PEHeader
            {
                Magic = rawHeader.Magic,
                Machine = rawHeader.Machine,
                CreatedTime = rawHeader.CreatedTime,
                SymbolTableAddress = rawHeader.SymbolTableAddress,
                SymbolCount = rawHeader.SymbolCount,
                Flags = rawHeader.Flags,
                OptionalHeader = ResolveOptionalPEHeader(rawHeader.OptionalHeader),
            };
            return header;
        }

        #region Resolve section data
        private static PEImportDirectoryTable[] ResolveImportDataSection(Stream stream)
        {
            var tables = new List<PEImportDirectoryTable>();
            
            PEImportDirectoryTable table;
            do
            {
                table = new PEImportDirectoryTable
                {
                    ImportLookupTableAddress = stream.ReadUInt32L(),
                    DateTimeStamp = stream.ReadUInt32L(),
                    FirstForwarderReferenceIndex = stream.ReadUInt32L(),
                    NameAddress = stream.ReadUInt32L(),
                    ImportAddressTableAddress = stream.ReadUInt32L(),
                };
                tables.Add(table);
            } while (table.NameAddress != 0);
            return tables.ToArray();
        }
        #endregion

        private static object ResolveSectionData(string name, byte[] data)
        {
            var str = data.ToUtf8String();
            var stream = new MemoryStream(data);
            return name switch
            {
                // Constants.SectionName.TEXT => ,
                // Constants.SectionName.DATA => ,
                // Constants.SectionName.RDATA => ,
                // Constants.SectionName.PDATA => ,
                Constants.SectionName.IDATA => ResolveImportDataSection(stream),
                // Constants.SectionName.RELOC => ,
                // Constants.SectionName.RSRC => ,
                _ => data,
            };
        }

        private static PESectionHeader ResolveSectionHeader(PESectionHeaderRaw rawHeader)
        {
            var header = new PESectionHeader
            {
                Name = rawHeader.Name,
                SectionMemorySize = rawHeader.SectionMemorySize,
                SectionMemoryAddress = rawHeader.SectionMemoryAddress,
                Flags = rawHeader.Flags,
                Data = ResolveSectionData(rawHeader.Name, rawHeader.Data),
            };
            return header;
        }
        #endregion
        #endregion
    }
}
