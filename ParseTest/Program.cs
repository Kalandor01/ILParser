namespace ParseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testFilesFolderPath = Path.GetFullPath("../../../../TestCreator/bin/Debug/net10.0");
            const string testFileName = "TestCreator";
            var testFilePath = Path.Join(testFilesFolderPath, testFileName);
            
            var rawElfFile = ELFParser.ELFParser.Parse(testFilePath);
            var elfFile = ELFParser.ELFParser.Resolve(rawElfFile);
        }
    }
}
