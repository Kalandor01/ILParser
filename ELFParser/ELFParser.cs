using ParserCommon.Extensions;

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
            
            elfFile.EntryProgramHeaderIndex = elfFile.Header.EntryPointAddress != 0
                ? elfFile.ProgramHeaderTable.IndexOf(
                    elfFile.ProgramHeaderTable.First(ph => ph.VirtualAddress == elfFile.Header.EntryPointAddress)
                )
                : -1;
            
            return elfFile;
        }
        
        public static ELFFile Resolve(ELFFileRaw rawFile)
        {
            var header = ParseHeader(rawFile.Header);
            var elfFile = new ELFFile
            {
                Header = header,
                ProgramHeaders = [.. rawFile.ProgramHeaderTable.Select(h => ParseProgramHeader(rawFile, h))],
                SectionHeaders = [.. rawFile.SectionHeaderTable.Select(h => ParseSectionHeader(rawFile, header, h))],
            };
            elfFile.EntryPointHeader = rawFile.EntryProgramHeaderIndex != -1
                ? elfFile.ProgramHeaders[rawFile.EntryProgramHeaderIndex]
                : null;
            
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

        private static ELFProgramHeader ParseProgramHeader(ELFFileRaw rawFile, ELFProgramHeaderRaw rawHeader)
        {
            var header = new ELFProgramHeader
            {
                TypeNum = rawHeader.TypeNum,
                Flags = rawHeader.Flags,
            };
            header.Data = ParseProgramHeaderData(rawFile.Header.Identity, header, rawHeader.Data);
            return header;
        }

        private static ELFSectionHeader ParseSectionHeader(ELFFileRaw rawFile, ELFHeader elfHeader, ELFSectionHeaderRaw rawHeader)
        {
            var header = new ELFSectionHeader
            {
                Name = rawFile.ResolveStringByIndex(rawHeader.Name),
                TypeNum = rawHeader.TypeNum,
                Flags = rawHeader.Flags,
                Info = rawHeader.Info,
            };
            
            header.Data = ParseSectionHeaderData(elfHeader, header, rawHeader.Data);
            return header;
        }

        #region Program header data parsing
        private static AELFNoteInfo ResolveNoteInfo(ELFIdentity identity, ELFStream noteStream)
        {
            var note = ParseNoteInfo(noteStream);
            
            var dataStream = identity.CreateElfStream(note.Descriptor);
            return (note.Name, note.Type) switch
            {
                ("GNU", 1) => new ELFGnuAbiVersionNoteInfo(dataStream.ParseArray(4, s => s.ReadUInt32())),
                ("GNU", 3) => new ELFGnuBuildIdNoteInfo(note.Descriptor),
                _ => note,
            };
        }
        
        private static AELFNoteInfo ResolveNoteInfo(ELFIdentity identity, byte[] noteBytes)
        {
            return ResolveNoteInfo(identity, identity.CreateElfStream(noteBytes));
        }
        
        private static ELFUnknownNoteInfo ParseNoteInfo(ELFStream stream)
        {
            var nameS = stream.ReadUInt32();
            var descS = stream.ReadUInt32();
            var type = stream.ReadUInt32();
            
            var note = new ELFUnknownNoteInfo
            {
                Name = stream.ReadBytesAndAlign((int)nameS, 4).ToUtf8String(),
                Type = type,
                Descriptor = stream.ReadBytes(descS),
            };
            return note;
        }

        private static AELFNoteInfo[] ParseNoteSection(ELFIdentity identity, byte[] bytes)
        {
            var stream = identity.CreateElfStream(bytes);
            var nodes = new List<AELFNoteInfo>();
            
            while (stream.Length - stream.Position >= 3 * sizeof(uint))
            {
                nodes.Add(ResolveNoteInfo(identity, stream));
            }
            
            return stream.ParsedOrThrow(nodes.ToArray());
        }
        #endregion

        private static object? ParseProgramHeaderData(ELFIdentity identity, ELFProgramHeader header, byte[] data)
        {
            return header.Type switch
            {
                ELFEnums.ELFProgramHeaderType.NULL => null,
                ELFEnums.ELFProgramHeaderType.LOAD => data,                     // TODO: unprocessed
                ELFEnums.ELFProgramHeaderType.DYNAMIC => data,                  // TODO: unprocessed
                ELFEnums.ELFProgramHeaderType.INTERP => data.ToUtf8String(),
                ELFEnums.ELFProgramHeaderType.NOTE => ParseNoteSection(identity, data),
                ELFEnums.ELFProgramHeaderType.SHLIB => data,
                ELFEnums.ELFProgramHeaderType.PHDR => data,                     // TODO: unprocessed
                ELFEnums.ELFProgramHeaderType.TLS => data,                      // TODO: unprocessed (empty?)
                ELFEnums.ELFProgramHeaderType.GNU_EH_FRAME => data,             // TODO: unprocessed
                ELFEnums.ELFProgramHeaderType.GNU_STACK => data,                // TODO: unprocessed (empty?)
                ELFEnums.ELFProgramHeaderType.GNU_RELRO => data,                // TODO: unprocessed
                ELFEnums.ELFProgramHeaderType.OS_SPECIFIC => data,
                ELFEnums.ELFProgramHeaderType.PROC_SPECIFIC => data,
                _ => throw new ArgumentOutOfRangeException(nameof(header.Type), header.Type, null),
            };
        }

        private static object? ParseSectionHeaderData(ELFHeader elfHeader, ELFSectionHeader header, byte[] data)
        {
            return header.Name switch
            {
                null => null,
                Constants.SectionName.INTERPRETER_INFO => data.ToUtf8String(),
                Constants.SectionName.NOTE_ABI_TAG => (ELFGnuAbiVersionNoteInfo)ResolveNoteInfo(elfHeader.Identity, data),
                Constants.SectionName.NOTE_GNU_BUILD_ID => (ELFGnuBuildIdNoteInfo)ResolveNoteInfo(elfHeader.Identity, data),
                // Constants.SectionName.DYNSYM => data,
                // Constants.SectionName.GNU_VERSION => data,
                // Constants.SectionName.GNU_VERSION_R => data,
                // Constants.SectionName.GNU_HASH => data,
                // Constants.SectionName.DYNSTR => data,
                // Constants.SectionName.RELA_DYN => data,
                // Constants.SectionName.RELA_PLT => data,
                // Constants.SectionName.GCC_EXCEPT_TABLE => data,
                // Constants.SectionName.EH_FRAME_HDR => data,
                // Constants.SectionName.EH_FRAME => data,
                Constants.SectionName.INIT => ParseAssemblyInstructions(elfHeader, data),
                Constants.SectionName.FINI => ParseAssemblyInstructions(elfHeader, data),
                // Constants.SectionName.PLT => data,
                // Constants.SectionName.TBSS => data,
                // Constants.SectionName.FINI_ARRAY => data,
                // Constants.SectionName.INIT_ARRAY => data,
                // Constants.SectionName.DATA_REL_RO => data,
                // Constants.SectionName.GOT => data,
                // Constants.SectionName.GOT_PLT => data,
                // Constants.SectionName.RELRO_PADDING => data,
                // Constants.SectionName.TM_CLONE_TABLE => data,
                // Constants.SectionName.GNU_DEBUGLINK => data,
                // Constants.SectionName.UNINITIALIZED_DATA => data,
                // Constants.SectionName.VERSION_CONTROL_INFO => data,
                // Constants.SectionName.DATA => data,
                // Constants.SectionName.DATA1 => data,
                // Constants.SectionName.DEBUG_INFO => data,
                // Constants.SectionName.DYNAMIC_LINKING_INFO => data,
                // Constants.SectionName.SYMBOL_HASH_TABLE => data,
                // Constants.SectionName.LINE_NUMBER_INFO => data,
                // Constants.SectionName.NOTE => data,
                // Constants.SectionName.READ_ONLY_DATA => data,
                // Constants.SectionName.READ_ONLY_DATA1 => data,
                Constants.SectionName.SECTION_NAMES => data.ToUtf8String().Split((char)0).Where(s => s.Length != 0).ToArray(),
                // Constants.SectionName.STRINGS => data,
                // Constants.SectionName.SYMBOL_TABLE => data,
                Constants.SectionName.INSTRUCTIONS => ParseAssemblyInstructions(elfHeader, data),
                // _ => throw new ArgumentOutOfRangeException(nameof(header.Name), header.Name, null),
                _ => data,
            };
        }

        private static object ExtendOperandIfNeeded(byte operand, bool wideOperands, bool extendOperands)
        {
            return extendOperands
                ? wideOperands
                    ? operand.ExtendInt64()
                    : operand.ExtendInt32()
                : operand;
        }

        private static ELFEnums.ELFX64InstructionGroupValue ParseGroup(byte opcode, out byte value)
        {
            if ((opcode & Constants.X64_INSTRUCTION_GROUP_MARKER_MASK) == 0)
            {
                value = opcode;
                return ELFEnums.ELFX64InstructionGroupValue.NO_GROUP;
                throw new ArgumentException("Opcode is nor a group value!", nameof(opcode));
            }

            var maskedOpcode = opcode & Constants.X64_GROUP_TYPE_MASK;
            
            value = (byte)(opcode & Constants.X64_GROUP_VALUE_MASK);
            return Enum.GetValues<ELFEnums.ELFX64InstructionGroupValue>()
                .First(v => (byte)v == maskedOpcode);
        }

        private static object?[] ParseX64MaybeGroupArgs(
            MemoryStream stream,
            byte groupInstruction,
            bool wideOperands = false,
            bool extendOperands = false
        )
        {
            var groupType = ParseGroup(groupInstruction, out var value);
            if (groupType != ELFEnums.ELFX64InstructionGroupValue.NO_GROUP)
            {
                return [groupType, (ELFEnums.ELFX64Register)value, ExtendOperandIfNeeded(stream.ReadByteB(), wideOperands, extendOperands)];
            }

            return [ELFEnums.ELFX64InstructionGroupValue.NO_GROUP, ParseX64AsmInstruction(stream, value, wideOperands, extendOperands)];
        }

        private static X64AsmInstruction ParseX64AsmInstruction(
            MemoryStream stream,
            byte instructionOpcode,
            bool wideOperands = false,
            bool extendOperands = false
        )
        {
            var s = "0x" + Convert.ToString(instructionOpcode, 16).ToUpper().PadLeft(2, '0');
            
            var opcode = (ELFEnums.X64Instruction)instructionOpcode;
            var inst = new X64AsmInstruction(opcode);

            inst.Arguments = opcode switch
            {
                ELFEnums.X64Instruction.POP_RBX or ELFEnums.X64Instruction.POP_RBP or ELFEnums.X64Instruction.NOP or
                    ELFEnums.X64Instruction.RET or ELFEnums.X64Instruction.INT3
                    => [],
                ELFEnums.X64Instruction.EXTEND_OPCODES
                    => [ParseX64AsmInstruction(stream, stream.ReadByteB(),  wideOperands, true)],
                ELFEnums.X64Instruction.OP_64
                    => [ParseX64AsmInstruction(stream, stream.ReadByteB(), true, extendOperands)],
                ELFEnums.X64Instruction.REX_B64
                    => [ParseX64AsmInstruction(stream, stream.ReadByteB(), true, true)],
                ELFEnums.X64Instruction.ADD or ELFEnums.X64Instruction.XOR
                    => [stream.ReadByteB()],
                ELFEnums.X64Instruction.GROUP_83
                    => ParseX64MaybeGroupArgs(stream, stream.ReadByteB(), wideOperands, extendOperands),
                // _ => throw new ArgumentOutOfRangeException(nameof(opcode), opcode, null),
                _ => [],
            };
            
            return inst;
        }

        private static IAsmInstruction ParseAsmInstruction(ELFHeader header, MemoryStream stream, byte instructionOpcode)
        {
            return header.Architecture switch
            {
                ELFEnums.ELFArchitecture.X86 or ELFEnums.ELFArchitecture.AMD_X86_64 => ParseX64AsmInstruction(stream, instructionOpcode),
                _ => throw new ArgumentOutOfRangeException(nameof(header.Architecture), header.Architecture, "Unsupported architecture!"),
            };
        }

        private static IAsmInstruction[] ParseAssemblyInstructions(ELFHeader header, byte[] data)
        {
            var stream = new MemoryStream(data);

            var instructions = new List<IAsmInstruction>();
            while (stream.Length != stream.Position)
            {
                var instruction = stream.ReadByteB();
                instructions.Add(ParseAsmInstruction(header, stream, instruction));
            }

            return stream.ParsedOrThrow(instructions.ToArray());
        }

        #endregion
        #endregion
    }
}