using System.Buffers.Binary;
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

            public string ToHexString()
            {
                return Convert.ToHexString(bytes);
            }
            
            public ushort AsUInt16L()
            {
                return BinaryPrimitives.ReadUInt16LittleEndian(bytes);
            }
            
            public ushort AsUInt16B()
            {
                return BinaryPrimitives.ReadUInt16BigEndian(bytes);
            }

            public uint AsUInt32L()
            {
                return BinaryPrimitives.ReadUInt32LittleEndian(bytes);
            }

            public uint AsUInt32B()
            {
                return BinaryPrimitives.ReadUInt32BigEndian(bytes);
            }

            public ulong AsUInt64L()
            {
                return BinaryPrimitives.ReadUInt64LittleEndian(bytes);
            }

            public ulong AsUInt64B()
            {
                return BinaryPrimitives.ReadUInt64BigEndian(bytes);
            }
        }
    }
}