using System.Text;
using ILParser.Extensions;

namespace ILParser
{
    public static class ELFParser
    {
        public static ELFFile Parse(string filePath)
        {
            if (!Path.Exists(filePath) || Path.GetExtension(filePath) != "")
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }

            var elfStream = File.OpenRead(filePath);
            var elfFile = new ELFFile
            {
                Magic = $"{elfStream.ReadBytesAsHexString(1)}_{Encoding.UTF8.GetString(elfStream.ReadBytes(3))}",
            };
            return elfFile;
        }
    }
}