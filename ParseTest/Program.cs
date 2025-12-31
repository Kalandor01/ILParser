namespace ParseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testFilesFolderPath = Path.GetFullPath("../../../../TestCreator/bin/Debug/net10.0");
            var testFileName = "TestCreator";
            var testFilePath = Path.Join(testFilesFolderPath, testFileName);
            
            var elfFile = ELFParser.ELFParser.Parse(testFilePath);
        }
    }
}
