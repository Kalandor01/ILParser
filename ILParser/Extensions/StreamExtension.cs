using System.Buffers.Binary;
using System.Numerics;
using System.Text;

namespace ILParser.Extensions
{
    public static class StreamExtension
    {
        extension(Stream stream)
        {
            public byte[] ReadBytes(int count)
            {
                var bytes = new byte[count];
                return stream.Read(bytes) >= count
                    ? bytes
                    : throw new EndOfStreamException();
            }

            public byte[] ReadBytesAligned(int count, int alignment)
            {
                var alignmentOffset = alignment - (int)(stream.Position % alignment);
                if (alignment > 1 && alignmentOffset != alignment)
                {
                    var alignmentBytes = stream.ReadBytes(alignmentOffset);
                }
                return stream.ReadBytes(count);
            }

            public string ReadBytesAsHexString(int count)
            {
                return BitConverter.ToString(stream.ReadBytes(count));
            }

            public string ReadBytesAsString(int count)
            {
                return Encoding.UTF8.GetString(stream.ReadBytes(count));
            }
            
            public byte ReadByteB()
            {
                return (byte)stream.ReadByte();
            }

            // public ushort ReadUInt16(ELFEnums.ELFDataEncoding encoding)
            // {
            //     var bytes = stream.ReadBytesAligned(2, 2);
            //     return encoding == ELFEnums.ELFDataEncoding.LITTLE_ENDIAN
            //         ? BinaryPrimitives.ReadUInt16LittleEndian(bytes)
            //         : BinaryPrimitives.ReadUInt16BigEndian(bytes);
            // }
            
            // public short ReadInt16(ELFEnums.ELFDataEncoding encoding)
            // {
            //     return BinaryPrimitives.ReadInt16LittleEndian(stream.ReadBytes(2));
            // }
            
            // public uint ReadUInt32(ELFEnums.ELFDataEncoding encoding)
            // {
            //     var bytes = stream.ReadBytesAligned(4, 4);
            //     return encoding == ELFEnums.ELFDataEncoding.LITTLE_ENDIAN
            //         ? BinaryPrimitives.ReadUInt32LittleEndian(bytes)
            //         : BinaryPrimitives.ReadUInt32BigEndian(bytes);
            // }
            
            // public int ReadInt32(ELFEnums.ELFDataEncoding encoding)
            // {
            //     var bytes = stream.ReadBytesAligned(4, 4);
            //     return encoding == ELFEnums.ELFDataEncoding.LITTLE_ENDIAN
            //         ? BinaryPrimitives.ReadInt32LittleEndian(bytes)
            //         : BinaryPrimitives.ReadInt32BigEndian(bytes);
            // }
            
            // public float ReadFloat()
            // {
            //     return BinaryPrimitives.ReadSingleLittleEndian(stream.ReadBytes(4));
            // }
            //
            // public ulong ReadUInt64()
            // {
            //     return BinaryPrimitives.ReadUInt64LittleEndian(stream.ReadBytes(8));
            // }
            //
            // public long ReadInt64()
            // {
            //     return BinaryPrimitives.ReadInt32LittleEndian(stream.ReadBytes(8));
            // }
            //
            // public double ReadDouble()
            // {
            //     return BinaryPrimitives.ReadDoubleLittleEndian(stream.ReadBytes(8));
            // }

            // public T[] ParseArray<T, TN>(TN count, Func<Stream, T> itemProcessor)
            //     where TN : INumber<TN>
            // {
            //     var list = new List<T>();
            //     for (var x = TN.Zero; x < count; x++)
            //     {
            //         var item = itemProcessor(stream);
            //         list.Add(item);
            //     }
            //     return list.ToArray();
            // }

            // public T[] ParseArray<T>(Func<Stream, T> itemProcessor)
            // {
            //     return stream.ParseArray(stream.ReadUInt16(), itemProcessor);
            // }
        }
    }
}