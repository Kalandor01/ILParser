using ParserCommon;
using ParserCommon.Extensions;

namespace PEParser
{
    public static class PEParser
    {
        #region Public methods
        public static object Parse(string filePath)
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
            var peFile = new PEFile
            {
                DOSHeader = dosHeader,
                PEHeader = ParsePEHeader(peStream, dosHeader),
            };
            return peFile;
        }
        #endregion

        #region Private methods
        private static PEDOSHeader ParseDOSHeader(Stream stream)
        {
            var header = new PEDOSHeader
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

        private static PEDOSRelocation ParseDOSRelocation(Stream stream)
        {
            var relocation = new PEDOSRelocation()
            {
                Offset = stream.ReadUInt16L(),
                Segment = stream.ReadUInt16L(),
            };
            return relocation;
        }

        private static byte[] ParseDOSProgram(Stream stream, PEDOSHeader dosHeader)
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

        private static PEHeader ParsePEHeader(Stream stream, PEDOSHeader dosHeader)
        {
            stream.Position = dosHeader.PEHeaderAddress;
            var peHeader = new PEHeader
            {
                Magic = stream.ReadBytes(4).ToUtf8String().TrimNullEnd(),
                Machine = (PEMachineType)stream.ReadUInt16L(),
                NumberOfSections = stream.ReadUInt16L(),
                TimeDateStamp = DateTimeOffset.FromUnixTimeSeconds(stream.ReadUInt32L()),
                PointerToSymbolTable = stream.ReadUInt32L(),
                NumberOfSymbols = stream.ReadUInt32L(),
                SizeOfOptionalHeader = stream.ReadUInt16L(),
                Characteristics = Utils.ParseEnumFlags<PECharacteristic>(stream.ReadUInt16L()),
            };
            peHeader.OptionalHeader = ParseOptionalPEHeader(stream, peHeader.SizeOfOptionalHeader);
            return peHeader;
        }

        private static DataDirectory ParseDataDirectory(Stream stream)
        {
            var dataDir = new DataDirectory
            {
                VirtualAddress = stream.ReadUInt32L(),
                Size = stream.ReadUInt32L(),
            };
            return dataDir;
        }

        private static PEOptionalHeader? ParseOptionalPEHeader(Stream stream, ushort headerSize)
        {
            if (headerSize == 0)
            {
                return null;
            }
            
            var imageType = (PEImageType)stream.ReadUInt16L();
            var is64Bit = imageType == PEImageType.PE32_PLUS;
            var header = new PEOptionalHeader
            {
                ImageType = imageType,
                MajorLinkerVersion = stream.ReadByteB(),
                MinorLinkerVersion = stream.ReadByteB(),
                SizeOfCode = stream.ReadUInt32L(),
                SizeOfInitializedData = stream.ReadUInt32L(),
                SizeOfUninitializedData = stream.ReadUInt32L(),
                AddressOfEntryPoint = stream.ReadUInt32L(),
                BaseOfCode = stream.ReadUInt32L(),
                BaseOfData = is64Bit ? null : stream.ReadUInt32L(),
                ImageBase = stream.ReadUInt64BitDependantL(is64Bit),
                SectionAlignment = stream.ReadUInt32L(),
                FileAlignment = stream.ReadUInt32L(),
                MajorOperatingSystemVersion = stream.ReadUInt16L(),
                MinorOperatingSystemVersion = stream.ReadUInt16L(),
                MajorImageVersion = stream.ReadUInt16L(),
                MinorImageVersion = stream.ReadUInt16L(),
                MajorSubsystemVersion = stream.ReadUInt16L(),
                MinorSubsystemVersion = stream.ReadUInt16L(),
                Win32VersionValue = stream.ReadUInt32L(),
                SizeOfImage = stream.ReadUInt32L(),
                SizeOfHeaders = stream.ReadUInt32L(),
                CheckSum = stream.ReadUInt32L(),
                Subsystem = (PESubsystemType)stream.ReadUInt16L(),
                DllCharacteristics = Utils.ParseEnumFlags<PEDllCharacteristics>(stream.ReadUInt16L()),
                SizeOfStackReserve = stream.ReadUInt64BitDependantL(is64Bit),
                SizeOfStackCommit = stream.ReadUInt64BitDependantL(is64Bit),
                SizeOfHeapReserve = stream.ReadUInt64BitDependantL(is64Bit),
                SizeOfHeapCommit = stream.ReadUInt64BitDependantL(is64Bit),
                LoaderFlags = stream.ReadUInt32L(),
                NumberOfRvaAndSizes = stream.ReadUInt32L(),
            };
            header.DataDirectory = stream.ParseArray(header.NumberOfRvaAndSizes / Constants.DATA_DIRECTORY_SIZE, ParseDataDirectory);
            return header;
        }
        #endregion
    }
}