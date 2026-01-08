namespace ParseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testProjectFolderPath = Path.GetFullPath("../../../../TestCreator/bin");
            const string TEST_FILE_FOLDER_LINUX = "Debug/net10.0";
            const string RELEASES_FOLDER = "Release/net10.0/";
            const string TEST_FILE_FOLDER_LINUX_SINGLE = RELEASES_FOLDER + "linux-x64/publish";
            const string TEST_FILE_FOLDER_WIN = RELEASES_FOLDER + "win-x64";
            const string TEST_FILE_NAME = "TestCreator";
            const string ELF_EXT = "";
            const string EXE_EXT = ".exe";
            const string DLL_EXT = ".dll";
            
            // var testElfFilePath = Path.Join(testProjectFolderPath, TEST_FILE_FOLDER_LINUX_SINGLE, TEST_FILE_NAME + ELF_EXT);
            // var rawElfFile = ELFParser.ELFParser.Parse(testElfFilePath);
            // var elfFile = ELFParser.ELFParser.Resolve(rawElfFile);
            // ELFInterpreter.RumFromEntrypoint(elfFile);
            
            var testPeFilePath = Path.Join(testProjectFolderPath, TEST_FILE_FOLDER_WIN, TEST_FILE_NAME + EXE_EXT);
            var peRawFile = PEParser.PEParser.Parse(testPeFilePath);
            var peFile = PEParser.PEParser.Resolve(peRawFile);
            
            
            var testPeFilePath2 = Path.Join(testProjectFolderPath, TEST_FILE_FOLDER_WIN, TEST_FILE_NAME + DLL_EXT);
            var peRawFile2 = PEParser.PEParser.Parse(testPeFilePath2);
            var peFile2 = PEParser.PEParser.Resolve(peRawFile2);
        }
    }
}
