using System.Text;

namespace ParserCommon.Extensions
{
    public static class ByteArrayExtensions
    {
        extension(byte[] bytes)
        {
            public string ToUtf8String(bool removeNullTerminator = true)
            {
                if (bytes.Length == 0)
                {
                    return "";
                }
            
                return removeNullTerminator && bytes[^1] == 0
                    ? Encoding.UTF8.GetString(bytes[..^1])
                    : Encoding.UTF8.GetString(bytes);
            }
        }
    }
}