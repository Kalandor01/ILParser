namespace PEParser
{
    public enum PEDOSProductType : ushort
    {
        UNKNOWN = 0x0000,
		IMPORT0 = 0x0001,
		LINKER510 = 0x0002,
		CVTOMF510 = 0x0003,
		LINKER600 = 0x0004,
		CVTOMF600 = 0x0005,
		CVTRES500 = 0x0006,
		UTC11_BASIC = 0x0007,
		UTC11_C = 0x0008,
		UTC12_BASIC = 0x0009,
		UTC12_C = 0x000a,
		UTC12_CPP = 0x000b,
		ALIASOBJ60 = 0x000c,
		VISUALBASIC60 = 0x000d,
		MASM613 = 0x000e,
		MASM710 = 0x000f,
		LINKER511 = 0x0010,
		CVTOMF511 = 0x0011,
		MASM614 = 0x0012,
		LINKER512 = 0x0013,
		CVTOMF512 = 0x0014,
		UTC12_C_STD = 0x0015,
		UTC12_CPP_STD = 0x0016,
		UTC12_C_BOOK = 0x0017,
		UTC12_CPP_BOOK = 0x0018,
		IMPLIB700 = 0x0019,
		CVTOMF700 = 0x001a,
		UTC13_BASIC = 0x001b,
		UTC13_C = 0x001c,
		UTC13_CPP = 0x001d,
		LINKER610 = 0x001e,
		CVTOMF610 = 0x001f,
		LINKER601 = 0x0020,
		CVTOMF601 = 0x0021,
		UTC12_1_BASIC = 0x0022,
		UTC12_1_C = 0x0023,
		UTC12_1_CPP = 0x0024,
		LINKER620 = 0x0025,
		CVTOMF620 = 0x0026,
		ALIASOBJ70 = 0x0027,
		LINKER621 = 0x0028,
		CVTOMF621 = 0x0029,
		MASM615 = 0x002a,
		UTC13_LTCG_C = 0x002b,
		UTC13_LTCG_CPP = 0x002c,
		MASM620 = 0x002d,
		ILASM100 = 0x002e,
		UTC12_2_BASIC = 0x002f,
		UTC12_2_C = 0x0030,
		UTC12_2_CPP = 0x0031,
		UTC12_2_C_STD = 0x0032,
		UTC12_2_CPP_STD = 0x0033,
		UTC12_2_C_BOOK = 0x0034,
		UTC12_2_CPP_BOOK = 0x0035,
		IMPLIB622 = 0x0036,
		CVTOMF622 = 0x0037,
		CVTRES501 = 0x0038,
		UTC13_C_STD = 0x0039,
		UTC13_CPP_STD = 0x003a,
		CVTPGD1300 = 0x003b,
		LINKER622 = 0x003c,
		LINKER700 = 0x003d,
		EXPORT622 = 0x003e,
		EXPORT700 = 0x003f,
		MASM700 = 0x0040,
		UTC13_POGO_I_C = 0x0041,
		UTC13_POGO_I_CPP = 0x0042,
		UTC13_POGO_O_C = 0x0043,
		UTC13_POGO_O_CPP = 0x0044,
		CVTRES700 = 0x0045,
		CVTRES710P = 0x0046,
		LINKER710P = 0x0047,
		CVTOMF710P = 0x0048,
		EXPORT710P = 0x0049,
		IMPLIB710P = 0x004a,
		MASM710P = 0x004b,
		UTC1310P_C = 0x004c,
		UTC1310P_CPP = 0x004d,
		UTC1310P_C_STD = 0x004e,
		UTC1310P_CPP_STD = 0x004f,
		UTC1310P_LTCG_C = 0x0050,
		UTC1310P_LTCG_CPP = 0x0051,
		UTC1310P_POGO_I_C = 0x0052,
		UTC1310P_POGO_I_CPP = 0x0053,
		UTC1310P_POGO_O_C = 0x0054,
		UTC1310P_POGO_O_CPP = 0x0055,
		LINKER624 = 0x0056,
		CVTOMF624 = 0x0057,
		EXPORT624 = 0x0058,
		IMPLIB624 = 0x0059,
		LINKER710 = 0x005a,
		CVTOMF710 = 0x005b,
		EXPORT710 = 0x005c,
		IMPLIB710 = 0x005d,
		CVTRES710 = 0x005e,
		UTC1310_C = 0x005f,
		UTC1310_CPP = 0x0060,
		UTC1310_C_STD = 0x0061,
		UTC1310_CPP_STD = 0x0062,
		UTC1310_LTCG_C = 0x0063,
		UTC1310_LTCG_CPP = 0x0064,
		UTC1310_POGO_I_C = 0x0065,
		UTC1310_POGO_I_CPP = 0x0066,
		UTC1310_POGO_O_C = 0x0067,
		UTC1310_POGO_O_CPP = 0x0068,
		ALIASOBJ710 = 0x0069,
		ALIASOBJ710P = 0x006a,
		CVTPGD1310 = 0x006b,
		CVTPGD1310P = 0x006c,
		UTC1400_C = 0x006d,
		UTC1400_CPP = 0x006e,
		UTC1400_C_STD = 0x006f,
		UTC1400_CPP_STD = 0x0070,
		UTC1400_LTCG_C = 0x0071,
		UTC1400_LTCG_CPP = 0x0072,
		UTC1400_POGO_I_C = 0x0073,
		UTC1400_POGO_I_CPP = 0x0074,
		UTC1400_POGO_O_C = 0x0075,
		UTC1400_POGO_O_CPP = 0x0076,
		CVTPGD1400 = 0x0077,
		LINKER800 = 0x0078,
		CVTOMF800 = 0x0079,
		EXPORT800 = 0x007a,
		IMPLIB800 = 0x007b,
		CVTRES800 = 0x007c,
		MASM800 = 0x007d,
		ALIASOBJ800 = 0x007e,
		PHOENIXPRERELEASE = 0x007f,
		UTC1400_CVTCIL_C = 0x0080,
		UTC1400_CVTCIL_CPP = 0x0081,
		UTC1400_LTCG_MSIL = 0x0082,
		UTC1500_C = 0x0083,
		UTC1500_CPP = 0x0084,
		UTC1500_C_STD = 0x0085,
		UTC1500_CPP_STD = 0x0086,
		UTC1500_CVTCIL_C = 0x0087,
		UTC1500_CVTCIL_CPP = 0x0088,
		UTC1500_LTCG_C = 0x0089,
		UTC1500_LTCG_CPP = 0x008a,
		UTC1500_LTCG_MSIL = 0x008b,
		UTC1500_POGO_I_C = 0x008c,
		UTC1500_POGO_I_CPP = 0x008d,
		UTC1500_POGO_O_C = 0x008e,
		UTC1500_POGO_O_CPP = 0x008f,
		CVTPGD1500 = 0x0090,
		LINKER900 = 0x0091,
		EXPORT900 = 0x0092,
		IMPLIB900 = 0x0093,
		CVTRES900 = 0x0094,
		MASM900 = 0x0095,
		ALIASOBJ900 = 0x0096,
		RESOURCE = 0x0097,
		ALIASOBJ1000 = 0x0098,
		CVTPGD1600 = 0x0099,
		CVTRES1000 = 0x009a,
		EXPORT1000 = 0x009b,
		IMPLIB1000 = 0x009c,
		LINKER1000 = 0x009d,
		MASM1000 = 0x009e,
		PHX1600_C = 0x009f,
		PHX1600_CPP = 0x00a0,
		PHX1600_CVTCIL_C = 0x00a1,
		PHX1600_CVTCIL_CPP = 0x00a2,
		PHX1600_LTCG_C = 0x00a3,
		PHX1600_LTCG_CPP = 0x00a4,
		PHX1600_LTCG_MSIL = 0x00a5,
		PHX1600_POGO_I_C = 0x00a6,
		PHX1600_POGO_I_CPP = 0x00a7,
		PHX1600_POGO_O_C = 0x00a8,
		PHX1600_POGO_O_CPP = 0x00a9,
		UTC1600_C = 0x00aa,
		UTC1600_CPP = 0x00ab,
		UTC1600_CVTCIL_C = 0x00ac,
		UTC1600_CVTCIL_CPP = 0x00ad,
		UTC1600_LTCG_C = 0x00ae,
		UTC1600_LTCG_CPP = 0x00af,
		UTC1600_LTCG_MSIL = 0x00b0,
		UTC1600_POGO_I_C = 0x00b1,
		UTC1600_POGO_I_CPP = 0x00b2,
		UTC1600_POGO_O_C = 0x00b3,
		UTC1600_POGO_O_CPP = 0x00b4,
		ALIASOBJ1010 = 0x00b5,
		CVTPGD1610 = 0x00b6,
		CVTRES1010 = 0x00b7,
		EXPORT1010 = 0x00b8,
		IMPLIB1010 = 0x00b9,
		LINKER1010 = 0x00ba,
		MASM1010 = 0x00bb,
		UTC1610_C = 0x00bc,
		UTC1610_CPP = 0x00bd,
		UTC1610_CVTCIL_C = 0x00be,
		UTC1610_CVTCIL_CPP = 0x00bf,
		UTC1610_LTCG_C = 0x00c0,
		UTC1610_LTCG_CPP = 0x00c1,
		UTC1610_LTCG_MSIL = 0x00c2,
		UTC1610_POGO_I_C = 0x00c3,
		UTC1610_POGO_I_CPP = 0x00c4,
		UTC1610_POGO_O_C = 0x00c5,
		UTC1610_POGO_O_CPP = 0x00c6,
		ALIASOBJ1100 = 0x00c7,
		CVTPGD1700 = 0x00c8,
		CVTRES1100 = 0x00c9,
		EXPORT1100 = 0x00ca,
		IMPLIB1100 = 0x00cb,
		LINKER1100 = 0x00cc,
		MASM1100 = 0x00cd,
		UTC1700_C = 0x00ce,
		UTC1700_CPP = 0x00cf,
		UTC1700_CVTCIL_C = 0x00d0,
		UTC1700_CVTCIL_CPP = 0x00d1,
		UTC1700_LTCG_C = 0x00d2,
		UTC1700_LTCG_CPP = 0x00d3,
		UTC1700_LTCG_MSIL = 0x00d4,
		UTC1700_POGO_I_C = 0x00d5,
		UTC1700_POGO_I_CPP = 0x00d6,
		UTC1700_POGO_O_C = 0x00d7,
		UTC1700_POGO_O_CPP = 0x00d8,
		ALIASOBJ1200 = 0x00d9,
		CVTPGD1800 = 0x00da,
		CVTRES1200 = 0x00db,
		EXPORT1200 = 0x00dc,
		IMPLIB1200 = 0x00dd,
		LINKER1200 = 0x00de,
		MASM1200 = 0x00df,
		UTC1800_C = 0x00e0,
		UTC1800_CPP = 0x00e1,
		UTC1800_CVTCIL_C = 0x00e2,
		UTC1800_CVTCIL_CPP = 0x00e3,
		UTC1800_LTCG_C = 0x00e4,
		UTC1800_LTCG_CPP = 0x00e5,
		UTC1800_LTCG_MSIL = 0x00e6,
		UTC1800_POGO_I_C = 0x00e7,
		UTC1800_POGO_I_CPP = 0x00e8,
		UTC1800_POGO_O_C = 0x00e9,
		UTC1800_POGO_O_CPP = 0x00ea,
		ALIASOBJ1210 = 0x00eb,
		CVTPGD1810 = 0x00ec,
		CVTRES1210 = 0x00ed,
		EXPORT1210 = 0x00ee,
		IMPLIB1210 = 0x00ef,
		LINKER1210 = 0x00f0,
		MASM1210 = 0x00f1,
		UTC1810_C = 0x00f2,
		UTC1810_CPP = 0x00f3,
		UTC1810_CVTCIL_C = 0x00f4,
		UTC1810_CVTCIL_CPP = 0x00f5,
		UTC1810_LTCG_C = 0x00f6,
		UTC1810_LTCG_CPP = 0x00f7,
		UTC1810_LTCG_MSIL = 0x00f8,
		UTC1810_POGO_I_C = 0x00f9,
		UTC1810_POGO_I_CPP = 0x00fa,
		UTC1810_POGO_O_C = 0x00fb,
		UTC1810_POGO_O_CPP = 0x00fc,
		ALIASOBJ1400 = 0x00fd,
		CVTPGD1900 = 0x00fe,
		CVTRES1400 = 0x00ff,
		EXPORT1400 = 0x0100,
		IMPLIB1400 = 0x0101,
		LINKER1400 = 0x0102,
		MASM1400 = 0x0103,
		UTC1900_C = 0x0104,
		UTC1900_CPP = 0x0105,
		UTC1900_CVTCIL_C = 0x0106,
		UTC1900_CVTCIL_CPP = 0x0107,
		UTC1900_LTCG_C = 0x0108,
		UTC1900_LTCG_CPP = 0x0109,
		UTC1900_LTCG_MSIL = 0x010a,
		UTC1900_POGO_I_C = 0x010b,
		UTC1900_POGO_I_CPP = 0x010c,
		UTC1900_POGO_O_C = 0x010d,
		UTC1900_POGO_O_CPP = 0x010e,
    }

    public enum PEMachineType : ushort
    {
	    /// <summary>
	    /// The content of this field is assumed to be applicable to any machine type
	    /// </summary>
	    UNKNOWN = 0x0,
		/// <summary>
		/// Alpha AXP, 32-bit address space
		/// </summary>
		ALPHA = 0x184,
		/// <summary>
		/// Alpha 64, 64-bit address space, or AXP 64
		/// </summary>
		ALPHA64_AXP64 = 0x284,
		/// <summary>
		/// Matsushita AM33
		/// </summary>
		AM33 = 0x1d3,
		/// <summary>
		/// x64
		/// </summary>
		AMD64 = 0x8664,
		/// <summary>
		/// ARM little endian
		/// </summary>
		ARM = 0x1c0,
		/// <summary>
		/// ARM64 little endian
		/// </summary>
		ARM64 = 0xaa64,
		/// <summary>
		/// ABI that enables interoperability between native ARM64 and emulated x64 code.
		/// </summary>
		ARM64EC = 0xA641,
		/// <summary>
		/// Binary format that allows both native ARM64 and ARM64EC code to coexist in the same file.
		/// </summary>
		ARM64X = 0xA64E,
		/// <summary>
		/// ARM Thumb-2 little endian
		/// </summary>
		ARMNT = 0x1c4,
		/// <summary>
		/// EFI byte code
		/// </summary>
		EBC = 0xebc,
		/// <summary>
		/// Intel 386 or later processors and compatible processors
		/// </summary>
		I386 = 0x14c,
		/// <summary>
		/// Intel Itanium processor family
		/// </summary>
		IA64 = 0x200,
		/// <summary>
		/// LoongArch 32-bit processor family
		/// </summary>
		LOONGARCH32 = 0x6232,
		/// <summary>
		/// LoongArch 64-bit processor family
		/// </summary>
		LOONGARCH64 = 0x6264,
		/// <summary>
		/// Mitsubishi M32R little endian
		/// </summary>
		M32R = 0x9041,
		/// <summary>
		/// MIPS16
		/// </summary>
		MIPS16 = 0x266,
		/// <summary>
		/// MIPS with FPU
		/// </summary>
		MIPSFPU = 0x366,
		/// <summary>
		/// MIPS16 with FPU
		/// </summary>
		MIPSFPU16 = 0x466,
		/// <summary>
		/// Power PC little endian
		/// </summary>
		POWERPC = 0x1f0,
		/// <summary>
		/// Power PC with floating point support
		/// </summary>
		POWERPCFP = 0x1f1,
		/// <summary>
		/// MIPS I compatible 32-bit big endian
		/// </summary>
		R3000BE = 0x160,
		/// <summary>
		/// MIPS I compatible 32-bit little endian
		/// </summary>
		R3000 = 0x162,
		/// <summary>
		/// MIPS III compatible 64-bit little endian
		/// </summary>
		R4000 = 0x166,
		/// <summary>
		/// MIPS IV compatible 64-bit little endian
		/// </summary>
		R10000 = 0x168,
		/// <summary>
		/// RISC-V 32-bit address space
		/// </summary>
		RISCV32 = 0x5032,
		/// <summary>
		/// RISC-V 64-bit address space
		/// </summary>
		RISCV64 = 0x5064,
		/// <summary>
		/// RISC-V 128-bit address space
		/// </summary>
		RISCV128 = 0x5128,
		/// <summary>
		/// Hitachi SH3
		/// </summary>
		SH3 = 0x1a2,
		/// <summary>
		/// Hitachi SH3 DSP
		/// </summary>
		SH3DSP = 0x1a3,
		/// <summary>
		/// Hitachi SH4
		/// </summary>
		SH4 = 0x1a6,
		/// <summary>
		/// Hitachi SH5
		/// </summary>
		SH5 = 0x1a8,
		/// <summary>
		/// Thumb
		/// </summary>
		THUMB = 0x1c2,
		/// <summary>
		/// MIPS little-endian WCE v2
		/// </summary>
		WCEMIPSV2 = 0x169,
    }

    [Flags]
    public enum PEFlag : ushort
    {
	    /// <summary>
	    /// Image only, Windows CE, and Microsoft Windows NT and later. This indicates that the file does not contain base relocations and must therefore be loaded at its preferred base address. If the base address is not available, the loader reports an error. The default behavior of the linker is to strip base relocations from executable (EXE) files.
	    /// </summary>
		RELOCS_STRIPPED = 0x0001,
		/// <summary>
		/// Image only. This indicates that the image file is valid and can be run. If this flag is not set, it indicates a linker error.
		/// </summary>
		EXECUTABLE_IMAGE = 0x0002,
		/// <summary>
		/// COFF line numbers have been removed. This flag is deprecated and should be zero.
		/// </summary>
		LINE_NUMS_STRIPPED = 0x0004,
		/// <summary>
		/// COFF symbol table entries for local symbols have been removed. This flag is deprecated and should be zero.
		/// </summary>
		LOCAL_SYMS_STRIPPED = 0x0008,
		/// <summary>
		/// Obsolete. Aggressively trim working set. This flag is deprecated for Windows 2000 and later and must be zero.
		/// </summary>
		AGGRESSIVE_WS_TRIM = 0x0010,
		/// <summary>
		/// Application can handle > 2-GB addresses.
		/// </summary>
		LARGE_ADDRESS_AWARE = 0x0020,
		/// <summary>
		/// This flag is reserved for future use.
		/// </summary>
		RESERVED = 0x0040,
		/// <summary>
		/// Little endian: the least significant bit (LSB) precedes the most significant bit (MSB) in memory. This flag is deprecated and should be zero.
		/// </summary>
		BYTES_REVERSED_LO = 0x0080,
		/// <summary>
		/// Machine is based on a 32-bit-word architecture.
		/// </summary>
		MACHINE_32BIT = 0x0100,
		/// <summary>
		/// Debugging information is removed from the image file.
		/// </summary>
		DEBUG_STRIPPED = 0x0200,
		/// <summary>
		/// If the image is on removable media, fully load it and copy it to the swap file.
		/// </summary>
		REMOVABLE_RUN_FROM_SWAP = 0x0400,
		/// <summary>
		/// If the image is on network media, fully load it and copy it to the swap file.
		/// </summary>
		NET_RUN_FROM_SWAP = 0x0800,
		/// <summary>
		/// The image file is a system file, not a user program.
		/// </summary>
		SYSTEM = 0x1000,
		/// <summary>
		/// The image file is a dynamic-link library (DLL). Such files are considered executable files for almost all purposes, although they cannot be directly run.
		/// </summary>
		DLL = 0x2000,
		/// <summary>
		/// The file should be run only on a uniprocessor machine.
		/// </summary>
		UP_SYSTEM_ONLY = 0x4000,
		/// <summary>
		/// Big endian: the MSB precedes the LSB in memory. This flag is deprecated and should be zero.
		/// </summary>
		BYTES_REVERSED_HI = 0x8000,
    }

    public enum PEImageType : ushort
    {
	    /// <summary>
	    /// Identifies the image as a ROM image.
	    /// </summary>
	    ROM = 0x107,
    	/// <summary>
		/// Identifies the image as a PE32 executable.
		/// </summary>
		PE32 = 0x10B,
    	/// <summary>
		/// Identifies the image as a PE32+ executable.
		/// </summary>
		PE32_PLUS = 0x20B,
    }

    public enum PESubsystemType : ushort
    {
	    /// <summary>
		/// An unknown subsystem
		/// </summary>
		UNKNOWN = 0,
	    /// <summary>
		/// Device drivers and native Windows processes
		/// </summary>
		NATIVE = 1,
	    /// <summary>
		/// The Windows graphical user interface (GUI) subsystem
		/// </summary>
		WINDOWS_GUI = 2,
	    /// <summary>
		/// The Windows character subsystem
		/// </summary>
		WINDOWS_CUI = 3,
	    /// <summary>
		/// The OS/2 character subsystem
		/// </summary>
		OS2_CUI = 5,
	    /// <summary>
		/// The Posix character subsystem
		/// </summary>
		POSIX_CUI = 7,
	    /// <summary>
		/// Native Win9x driver
		/// </summary>
		NATIVE_WINDOWS = 8,
	    /// <summary>
		/// Windows CE
		/// </summary>
		WINDOWS_CE_GUI = 9,
	    /// <summary>
		/// An Extensible Firmware Interface (EFI) application
		/// </summary>
		EFI_APPLICATION = 10,
	    /// <summary>
		/// An EFI driver with boot services
		/// </summary>
		EFI_BOOT_SERVICE_DRIVER = 11,
	    /// <summary>
		/// An EFI driver with run-time services
		/// </summary>
		EFI_RUNTIME_DRIVER = 12,
	    /// <summary>
		/// An EFI ROM image
		/// </summary>
		EFI_ROM = 13,
	    /// <summary>
		/// XBOX
		/// </summary>
		XBOX = 14,
	    /// <summary>
		/// Windows boot application.
		/// </summary>
		WINDOWS_BOOT_APPLICATION = 16,
    }

    [Flags]
    public enum PEDllFlag : ushort
    {
	    /// <summary>
		/// Reserved, must be zero.
		/// </summary>
		RESERVED_1 = 0x0001,
	    /// <summary>
		/// Reserved, must be zero.
		/// </summary>
		RESERVED_2 = 0x0002,
	    /// <summary>
		/// Reserved, must be zero.
		/// </summary>
		RESERVED_3 = 0x0004,
	    /// <summary>
		/// Reserved, must be zero.
		/// </summary>
		RESERVED_4 = 0x0008,
	    /// <summary>
		/// Image can handle a high entropy 64-bit virtual address space.
		/// </summary>
		HIGH_ENTROPY_VA = 0x0020,
	    /// <summary>
		/// DLL can be relocated at load time.
		/// </summary>
		DYNAMIC_BASE = 0x0040,
	    /// <summary>
		/// Code Integrity checks are enforced.
		/// </summary>
	    FORCE_INTEGRITY = 0x0080,
	    /// <summary>
		/// Image is NX compatible.
		/// </summary>
		NX_COMPAT = 0x0100,
	    /// <summary>
		/// Isolation aware, but do not isolate the image.
		/// </summary>
		NO_ISOLATION = 0x0200,
	    /// <summary>
		/// Does not use structured exception (SE) handling. No SE handler may be called in this image.
		/// </summary>
		NO_SEH = 0x0400,
	    /// <summary>
		/// Do not bind the image.
		/// </summary>
		NO_BIND = 0x0800,
	    /// <summary>
		/// Image must execute in an AppContainer.
		/// </summary>
		APP_CONTAINER = 0x1000,
	    /// <summary>
		/// A WDM driver.
		/// </summary>
		WDM_DRIVER = 0x2000,
	    /// <summary>
		/// Image supports Control Flow Guard.
		/// </summary>
		GUARD_CF = 0x4000,
	    /// <summary>
		/// Terminal Server aware.
		/// </summary>
		TERMINAL_SERVER_AWARE = 0x8000,
    }

    public enum PEDataDirectoryType
    {
		/// <summary>
		/// Export Directory
		/// </summary>
		EXPORT = 0,
		/// <summary>
		/// Import Directory
		/// </summary>
		IMPORT = 1,
		/// <summary>
		/// Resource Directory
		/// </summary>
		RESOURCE = 2,
		/// <summary>
		/// Exception Directory
		/// </summary>
		EXCEPTION = 3,
		/// <summary>
		/// Security Directory
		/// </summary>
		SECURITY = 4,
		/// <summary>
		/// Base Relocation Table
		/// </summary>
		BASE_RELOC = 5,
		/// <summary>
		/// Debug Directory
		/// </summary>
		DEBUG = 6,
		/// <summary>
		/// Architecture Specific Data<br/>
		/// X86 usage: copyright
		/// </summary>
		ARCHITECTURE_OR_COPYRIGHT = 7,
		/// <summary>
		/// RVA of GP
		/// </summary>
		GLOBAL_PTR = 8,
		/// <summary>
		/// TLS Directory
		/// </summary>
		TLS = 9,
		/// <summary>
		/// Load Configuration Directory
		/// </summary>
		LOAD_CONFIG = 10,
		/// <summary>
		/// Bound Import Directory in headers
		/// </summary>
		BOUND_IMPORT = 11,
		/// <summary>
		/// Import Address Table
		/// </summary>
		IMPORT_ADDRESS_TABLE = 12,
		/// <summary>
		/// Delay Load Import Descriptors
		/// </summary>
		DELAY_IMPORT = 13,
		/// <summary>
		/// COM Runtime descriptor / .NET header
		/// </summary>
		DOTNET_HEADER = 14,
		RESERVED = 15,
    }

    [Flags]
    public enum PESectionFlag : uint
    {
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		RESERVED_1 = 0x00000000,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		RESERVED_2 = 0x00000001,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		RESERVED_3 = 0x00000002,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		RESERVED_4 = 0x00000004,
		/// <summary>
		/// The section should not be padded to the next boundary. This flag is obsolete and is replaced by ALIGN_1BYTES. This is valid only for object files.
		/// </summary>
		TYPE_NO_PAD = 0x00000008,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		RESERVED_5 = 0x00000010,
		/// <summary>
		/// The section contains executable code.
		/// </summary>
		CNT_CODE = 0x00000020,
		/// <summary>
		/// The section contains initialized data.
		/// </summary>
		CNT_INITIALIZED_DATA = 0x00000040,
		/// <summary>
		/// The section contains uninitialized data.
		/// </summary>
		CNT_UNINITIALIZED_DATA = 0x00000080,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		LNK_OTHER = 0x00000100,
		/// <summary>
		/// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
		/// </summary>
		LNK_INFO = 0x00000200,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		RESERVED_6 = 0x00000400,
		/// <summary>
		/// The section will not become part of the image. This is valid only for object files.
		/// </summary>
		LNK_REMOVE = 0x00000800,
		/// <summary>
		/// The section contains COMDAT data. For more information, see COMDAT Sections (Object Only). This is valid only for object files.
		/// </summary>
		LNK_COM_DAT = 0x00001000,
		/// <summary>
		/// The section contains data referenced through the global pointer (GP).
		/// </summary>
		GP_REL = 0x00008000,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		MEM_PURGEABLE_OR_16BIT = 0x00020000,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		MEM_LOCKED = 0x00040000,
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		MEM_PRELOAD = 0x00080000,
		/// <summary>
		/// Align data on a 1-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_1BYTES = 0x00100000,
		/// <summary>
		/// Align data on a 2-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_2BYTES = 0x00200000,
		/// <summary>
		/// Align data on a 4-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_4BYTES = 0x00300000,
		/// <summary>
		/// Align data on an 8-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_8BYTES = 0x00400000,
		/// <summary>
		/// Align data on a 16-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_16BYTES = 0x00500000,
		/// <summary>
		/// Align data on a 32-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_32BYTES = 0x00600000,
		/// <summary>
		/// Align data on a 64-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_64BYTES = 0x00700000,
		/// <summary>
		/// Align data on a 128-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_128BYTES = 0x00800000,
		/// <summary>
		/// Align data on a 256-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_256BYTES = 0x00900000,
		/// <summary>
		/// Align data on a 512-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_512BYTES = 0x00A00000,
		/// <summary>
		/// Align data on a 1024-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_1024BYTES = 0x00B00000,
		/// <summary>
		/// Align data on a 2048-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_2048BYTES = 0x00C00000,
		/// <summary>
		/// Align data on a 4096-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_4096BYTES = 0x00D00000,
		/// <summary>
		/// Align data on an 8192-byte boundary. Valid only for object files.
		/// </summary>
		ALIGN_8192BYTES = 0x00E00000,
		/// <summary>
		/// The section contains extended relocations.
		/// </summary>
		LNK_NRELOC_OVFL = 0x01000000,
		/// <summary>
		/// The section can be discarded as needed.
		/// </summary>
		MEM_DISCARDABLE = 0x02000000,
		/// <summary>
		/// The section cannot be cached.
		/// </summary>
		MEM_NOT_CACHED = 0x04000000,
		/// <summary>
		/// The section is not pageable.
		/// </summary>
		MEM_NOT_PAGED = 0x08000000,
		/// <summary>
		/// The section can be shared in memory.
		/// </summary>
		MEM_SHARED = 0x10000000,
		/// <summary>
		/// The section can be executed as code.
		/// </summary>
		MEM_EXECUTE = 0x20000000,
		/// <summary>
		/// The section can be read.
		/// </summary>
		MEM_READ = 0x40000000,
		/// <summary>
		/// The section can be written to.
		/// </summary>
		MEM_WRITE = 0x80000000,
    }
}
