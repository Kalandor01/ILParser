namespace ELFParser
{
    public static class ELFEnums
    {
        public enum ELFClassType : byte
        {
            /// <summary>
            /// Invalid class.
            /// </summary>
            NONE = 0,
            /// <summary>
            /// 32-bit objects.
            /// </summary>
            BIT_32 = 1,
            /// <summary>
            /// 64-bit objects.
            /// </summary>
            BIT_64 = 2,
        }

        public enum ELFDataEncoding : byte
        {
            /// <summary>
            /// Invalid data encoding.
            /// </summary>
            NONE = 0,
            /// <summary>
            /// Little endian encoding.
            /// </summary>
            LITTLE_ENDIAN = 1,
            /// <summary>
            /// Big endian encoding.
            /// </summary>
            BID_ENDIAN = 2,
        }
    
        public enum ELFVersion : byte
        {
            /// <summary>
            /// Invalid version.
            /// </summary>
            NONE = 0,
            /// <summary>
            /// Current version.
            /// </summary>
            CURRENT = 1,
        }
    
        public enum ELFAbi : byte
        {
            SYSTEM_V = 0x00,
            HP_UX = 0x01,
            NET_BSD = 0x02,
            LINUX = 0x03,
            GNU_HURD = 0x04,
            SOLARIS = 0x06,
            AIX_MONTEREY = 0x07,
            IRIX = 0x08,
            FREE_BSD = 0x09,
            TRU_64 = 0x0A,
            NOVELL_MODESTO = 0x0B,
            OPEN_BSD = 0x0C,
            OPEN_VMS = 0x0D,
            NON_STOP_KERNEL = 0x0E,
            AROS = 0x0F,
            FENIX_OS = 0x10,
            NUXI_CLOUD_ABI = 0x11,
            STRATUS_TECHNOLOGIES_OPEN_VOS = 0x12,
        }
    
        public enum ELFFileType : ushort
        {
            /// <summary>
            /// No file type.
            /// </summary>
            NONE = 0,
            /// <summary>
            /// Relocatable file.
            /// </summary>
            RELOCATABLE = 1,
            /// <summary>
            /// Executable file.
            /// </summary>
            EXECUTABLE = 2,
            /// <summary>
            /// Shared object file.
            /// </summary>
            SHARED_OBJECT = 3,
            /// <summary>
            /// Core file.
            /// </summary>
            CORE = 4,
            /// <summary>
            /// Operating system specific types start.
            /// </summary>
            R_LOW_OS = 0xFE00,
            /// <summary>
            /// Operating system specific types end.
            /// </summary>
            R_HI_OS = 0xFEFF,
            /// <summary>
            /// Processor specific types start.
            /// </summary>
            R_LOW_PROC = 0xFF00,
            /// <summary>
            /// Processor specific types end.
            /// </summary>
            R_HI_PROC = 0xFFFF,
        }
    
        public enum ELFArchitecture : ushort
        {
            /// <summary>
            /// No specific instruction set.
            /// </summary>
            NONE = 0x00,
            AT_AND_T_WE_32100 = 0x01,
            SPARC = 0x02,
            X86 = 0x03,
            MOTOROLA_68000_M68k = 0x04,
            MOTOROLA_88000_M88k = 0x05,
            INTEL_MCU = 0x06,
            INTEL_80860 = 0x07,
            MIPS = 0x08,
            IBM_SYSTEM_370 = 0x09,
            MIPS_RS3000_LITTLE_ENDIAN = 0x0A,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_0B = 0x0B,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_0C = 0x0C,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_0D = 0x0D,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_0E = 0x0E,
            HEWLETT_PACKARD_PA_RISC = 0x0F,
            INTEL_80960 = 0x13,
            POWER_PC = 0x14,
            POWER_PC_64_BIT = 0x15,
            S390_AND_S390X = 0x16,
            IBM_SPU_SPC = 0x17,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_18 = 0x18,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_19 = 0x19,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_20 = 0x20,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_21 = 0x21,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_22 = 0x22,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            RESERVED_23 = 0x23,
            NEC_V800 = 0x24,
            FUJITSU_FR20 = 0x25,
            TRW_RH_32 = 0x26,
            MOTOROLA_RCE = 0x27,
            ARM_UP_TO_ARMV7_OR_AARCH32 = 0x28,
            DIGITAL_ALPHA = 0x29,
            SUPERH = 0x2A,
            SPARC_V9 = 0x2B,
            SIEMENS_TRICORE_EMBEDDED_PROCESSOR = 0x2C,
            ARGONAUT_RISC_CORE = 0x2D,
            HITACHI_H8_300 = 0x2E,
            HITACHI_H8_300H = 0x2F,
            HITACHI_H8S = 0x30,
            HITACHI_H8_500 = 0x31,
            IA_64 = 0x32,
            STANFORD_MIPS_X = 0x33,
            MOTOROLA_COLDFIRE = 0x34,
            MOTOROLA_M68HC12 = 0x35,
            FUJITSU_MMA_MULTIMEDIA_ACCELERATOR = 0x36,
            SIEMENS_PCP = 0x37,
            SONY_NCPU_EMBEDDED_RISC_PROCESSOR = 0x38,
            DENSO_NDR1_MICROPROCESSOR = 0x39,
            MOTOROLA_STAR_CORE_PROCESSOR = 0x3A,
            TOYOTA_ME16_PROCESSOR = 0x3B,
            STMICROELECTRONICS_ST100_PROCESSOR = 0x3C,
            ADVANCED_LOGIC_CORP_TINYJ_EMBEDDED_PROCESSOR_FAMILY = 0x3D,
            AMD_X86_64 = 0x3E,
            SONY_DSP_PROCESSOR = 0x3F,
            DIGITAL_EQUIPMENT_CORP_PDP_10 = 0x40,
            DIGITAL_EQUIPMENT_CORP_PDP_11 = 0x41,
            SIEMENS_FX66_MICROCONTROLLER = 0x42,
            STMICROELECTRONICS_ST9_8_OR_16_BIT_MICROCONTROLLER = 0x43,
            STMICROELECTRONICS_ST7_8_BIT_MICROCONTROLLER = 0x44,
            MOTOROLA_MC68HC16_MICROCONTROLLER = 0x45,
            MOTOROLA_MC68HC11_MICROCONTROLLER = 0x46,
            MOTOROLA_MC68HC08_MICROCONTROLLER = 0x47,
            MOTOROLA_MC68HC05_MICROCONTROLLER = 0x48,
            SILICON_GRAPHICS_SVX = 0x49,
            STMICROELECTRONICS_ST19_8_BIT_MICROCONTROLLER = 0x4A,
            DIGITAL_VAX = 0x4B,
            AXIS_COMMUNICATIONS_32_BIT_EMBEDDED_PROCESSOR = 0x4C,
            INFINEON_TECHNOLOGIES_32_BIT_EMBEDDED_PROCESSOR = 0x4D,
            ELEMENT_14_64_BIT_DSP_PROCESSOR = 0x4E,
            LSI_LOGIC_16_BIT_DSP_PROCESSOR = 0x4F,
            TMS320C6000_FAMILY = 0x8C,
            MCST_ELBRUS_E2K = 0xAF,
            ARM_64_BITS_ARMV8_AARCH64 = 0xB7,
            ZILOG_Z80 = 0xDC,
            RISC_V = 0xF3,
            BERKELEY_PACKET_FILTER = 0xF7,
            WDC_65C816 = 0x101,
            LOONGARCH = 0x102,
        }
        
        public enum ELFProgramHeaderType : uint
        {
            /// <summary>
            /// Program header table entry unused.
            /// </summary>
            NULL = 0x00000000,
            /// <summary>
            /// Loadable segment.
            /// </summary>
            LOAD = 0x00000001,
            /// <summary>
            /// Dynamic linking information.
            /// </summary>
            DYNAMIC = 0x00000002,
            /// <summary>
            /// Interpreter information.
            /// </summary>
            INTERP = 0x00000003,
            /// <summary>
            /// Auxiliary information.
            /// </summary>
            NOTE = 0x00000004,
            /// <summary>
            /// Reserved.
            /// </summary>
            SHLIB = 0x00000005,
            /// <summary>
            /// Segment containing program header table itself.
            /// </summary>
            PHDR = 0x00000006,
            /// <summary>
            /// Thread-Local Storage template.
            /// </summary>
            TLS = 0x00000007,
            /// <summary>
            /// Value to indicate an operating system specific type.
            /// </summary>
            OS_SPECIFIC = 0x68888888,
            GNU_EH_FRAME = 0x6474E550,
            GNU_STACK = 0x6474E551,
            GNU_RELRO = 0x6474E552,
            /// <summary>
            /// Value to indicate a processor system specific type.
            /// </summary>
            PROC_SPECIFIC = 0x78888888,
        }
        
        public enum ELFSectionHeaderType : uint
        {
            /// <summary>
            /// Section header table entry unused
            /// </summary>
            NULL = 0x0,
            /// <summary>
            /// Program data
            /// </summary>
            PROGBITS = 0x1,
            /// <summary>
            /// Symbol table
            /// </summary>
            SYMTAB = 0x2,
            /// <summary>
            /// String table
            /// </summary>
            STRTAB = 0x3,
            /// <summary>
            /// Relocation entries with addends
            /// </summary>
            RELA = 0x4,
            /// <summary>
            /// Symbol hash table
            /// </summary>
            HASH = 0x5,
            /// <summary>
            /// Dynamic linking information
            /// </summary>
            DYNAMIC = 0x6,
            /// <summary>
            /// Notes
            /// </summary>
            NOTE = 0x7,
            /// <summary>
            /// Program space with no data (bss)
            /// </summary>
            NOBITS = 0x8,
            /// <summary>
            /// Relocation entries, no addends
            /// </summary>
            REL = 0x9,
            /// <summary>
            /// Reserved
            /// </summary>
            SHLIB = 0x0A,
            /// <summary>
            /// Dynamic linker symbol table
            /// </summary>
            DYNSYM = 0x0B,
            /// <summary>
            /// Array of constructors
            /// </summary>
            INIT_ARRAY = 0x0E,
            /// <summary>
            /// Array of destructors
            /// </summary>
            FINI_ARRAY = 0x0F,
            /// <summary>
            /// Array of pre-constructors
            /// </summary>
            PREINIT_ARRAY = 0x10,
            /// <summary>
            /// Section group
            /// </summary>
            GROUP = 0x11,
            /// <summary>
            /// Extended section indices
            /// </summary>
            SYMTAB_SHNDX = 0x12,
            /// <summary>
            /// Number of defined types.
            /// </summary>
            NUM = 0x13,
            /// <summary>
            /// Value to indicate an operating system specific type.
            /// </summary>
            OS_SPECIFIC = 0x70000000,
            GNU_HASH = 0x6FFFFFF6,
            GNU_VERNEED = 0x6FFFFFFE,
            GNU_VERSION = 0x6FFFFFFF,
        }
        
        [Flags]
        public enum ELFProgramHeaderFlag : uint
        {
            EXECUTABLE = 0x1,
            WRITABLE = 0x2,
            READABLE = 0x4,
        }
        
        [Flags]
        public enum ELFSectionHeaderFlag : ulong
        {
            /// <summary>
            /// Writable.
            /// </summary>
            WRITE = 0x1,
            /// <summary>
            /// Occupies memory during execution.
            /// </summary>
            ALLOC = 0x2,
            /// <summary>
            /// Executable.
            /// </summary>
            EXECUTABLE = 0x4,
            /// <summary>
            /// Might be merged.
            /// </summary>
            MERGE = 0x10,
            /// <summary>
            /// Contains null-terminated strings.
            /// </summary>
            STRINGS = 0x20,
            /// <summary>
            /// 'sh_info' contains SHT index.
            /// </summary>
            INFO_LINK = 0x40,
            /// <summary>
            /// Preserve order after combining.
            /// </summary>
            LINK_ORDER = 0x80,
            /// <summary>
            /// Non-standard OS specific handling required.
            /// </summary>
            OS_NONCONFORMING = 0x100,
            /// <summary>
            /// Section is member of a group.
            /// </summary>
            GROUP = 0x200,
            /// <summary>
            /// Section hold thread-local data.
            /// </summary>
            TLS = 0x400,
            /// <summary>
            /// OS-specific.
            /// </summary>
            MASK_OS = 0x0FF00000,
            /// <summary>
            /// Processor-specific.
            /// </summary>
            MASK_PROC = 0xF0000000,
            /// <summary>
            /// Special ordering requirement (Solaris).
            /// </summary>
            ORDERED = 0x4000000,
            /// <summary>
            /// Section is excluded unless referenced or allocated (Solaris).
            /// </summary>
            EXCLUDE = 0x8000000,
        }
        
        public enum GnuAbiVersionNoteSectionOSType
        {
            Linux = 0,
        }
        
        public enum X64Instruction : byte
        {
            /// <summary>
            /// r += r<br/>
            /// args: r
            /// </summary>
            ADD = 0x01,
            /// <summary>
            /// r = r xor r<br/>
            /// args: r
            /// </summary>
            XOR = 0x31,
            /// <summary>
            /// Extension of r/m field, base field, or opcode reg field.<br/>
            /// args: inner_instruction<br/>
            /// [PREFIX]
            /// </summary>
            EXTEND_OPCODES = 0x41,
            /// <summary>
            /// 64 bit operand size.<br/>
            /// args: inner_instruction<br/>
            /// [PREFIX]
            /// </summary>
            OP_64 = 0x48,
            /// <summary>
            /// <see cref="OP_64"/> + <see cref="EXTEND_OPCODES"/><br/>
            /// args: inner_instruction<br/>
            /// [PREFIX]
            /// </summary>
            REX_B64 = 0x49,
            /// <summary>
            /// Pop value from the stack to %rbx.
            /// </summary>
            POP_RBX = 0x5B,
            /// <summary>
            /// Pop value from the stack to %rbp.
            /// </summary>
            POP_RBP = 0x5D,
            /// <summary>
            /// op r i<br/>
            /// args: group_value, r, i<br/>
            /// [GROUP](0-7): add, or, add_with_cary, subtract_with_borrow, and, subtract, xor, compare
            /// </summary>
            GROUP_83 = 0x83,
            NOP = 0x90,
            RET = 0xC3,
            INT3 = 0xCC,
        }

        public enum ELFX64InstructionGroupValue : byte
        {
            ADD = 0b00_000_000,
            OR = 0b00_001_000,
            ADD_WITH_CARY = 0b00_010_000,
            SUBTRACT_WITH_BORROW = 0b00_011_000,
            AND = 0b00_100_000,
            SUBTRACT = 0b00_101_000,
            XOR = 0b00_110_000,
            COMPARE = 0b00_111_000,
            NO_GROUP = 255,
        }
        
        public enum ELFX64Register : byte
        {
            ACCUMULATOR = 0b00_000_000,             // RAX
            COUNTER = 0b00_000_001,                 // RCX
            EXTENDED_ACCUMULATOR = 0b00_000_010,    // RDX
            BASE_INDEX = 0b00_000_011,              // RBX
            STACK_POINTER = 0b00_000_100,           // RSP
            STACK_BASE_POINTER = 0b00_000_101,      // RBP
            SOURCE_INDEX = 0b00_000_110,            // RSI
            DESTINATION_INDEX = 0b00_000_111,       // RDI
            ADDITIONAL_REGISTER_8 = 8,              // R8
            ADDITIONAL_REGISTER_9 = 9,              // R9
            ADDITIONAL_REGISTER_10 = 10,            // R10
            ADDITIONAL_REGISTER_11 = 11,            // R11
            ADDITIONAL_REGISTER_12 = 12,            // R12
            ADDITIONAL_REGISTER_13 = 13,            // R13
            ADDITIONAL_REGISTER_14 = 14,            // R14
            ADDITIONAL_REGISTER_15 = 15,            // R15
        }
        
        // 64 bit       32 bit          16 bit          8 bit MSB   8 bit LSB       Notes                                       Found
        // RAX          EAX             AX              AH          AL              Accumulator                                 : F
        // RBX          EBX             BX              BH          BL              Base index (arrays)                         : F
        // RCX          ECX             CX              CH          CL              Counter (loops, strings)                    : F
        // RDX          EDX             DX              DH          DL              Extend accumulator                          : F
        // RSI          ESI             SI                          SIL             Source index (strings)                      : F
        // RDI          EDI             DI                          DIL             Destination index (strings)                 : F
        // RSP          ESP             SP                          SPL             Stack pointer (top of stack)                : F
        // RBP          EBP             BP                          BPL             Stack base pointer (bottom of stack)        : F
        // R8 to R15    R8D to R15D     R8W to R15W                 R8B to R15B     8 additional general purpose registers      : F?
        // RIP          EIP             IP                                          Instruction pointer (program counter)       : 
        // RFLAGS       EFLAGS          FLAGS                                       Flags register                              : 
        
        // 128 bit: XMM0 to XMM15 => 16 floating-point registers
    }
}