using System.Numerics;

namespace ParserCommon.Extensions
{
    public static class StreamExtension
    {
        extension(Stream stream)
        {
            public void ThrowIfNotEnOfStream()
            {
                if (stream.Position != stream.Length)
                {
                    throw new EndOfStreamException();
                }
            }

            public T ParsedOrThrow<T>(T result)
            {
                stream.ThrowIfNotEnOfStream();
                return result;
            }
            
            public byte[] ReadBytes(int count)
            {
                var bytes = new byte[count];
                return stream.Read(bytes) >= count
                    ? bytes
                    : throw new EndOfStreamException();
            }
            
            public byte[] ReadBytes(ulong count)
            {
                if (count <= int.MaxValue)
                {
                    return stream.ReadBytes((int)count);
                }

                var tempArrays = new List<byte[]>();
                do
                {
                    var toRead = count > int.MaxValue ? int.MaxValue : (int)count;
                    
                    var tempArray = new byte[toRead];
                    if (stream.Read(tempArray) < toRead)
                    {
                        throw new EndOfStreamException();
                    }
                    
                    tempArrays.Add(tempArray);
                    count -= (ulong)toRead;
                } while (count > 0);
                return tempArrays.SelectMany(a => a).ToArray();
            }
            
            public byte[] ReadBytes(int count, long position)
            {
                var oldPos = stream.Position;
                stream.Position = position;
                var res = stream.ReadBytes(count);
                stream.Position = oldPos;
                return res;
            }
            
            public byte[] ReadBytes(ulong count, long position)
            {
                var oldPos = stream.Position;
                stream.Position = position;
                var res = stream.ReadBytes(count);
                stream.Position = oldPos;
                return res;
            }

            public byte[] Align(int alignment)
            {
                var alignmentOffset = alignment - (int)(stream.Position % alignment);
                return alignment > 1 && alignmentOffset != alignment
                    ? stream.ReadBytes(alignmentOffset)
                    : [];
            }

            public byte[] ReadBytesAndAlign(int count, int alignment)
            {
                var res = stream.ReadBytes(count);
                stream.Align(alignment);
                return res;
            }

            public string ReadBytesAsHexString(int count)
            {
                return stream.ReadBytes(count).ToHexString();
            }

            public string ReadBytesAsString(int count)
            {
                return stream.ReadBytes(count).ToUtf8String();
            }
            
            public byte ReadByteB()
            {
                return (byte)stream.ReadByte();
            }

            public ushort ReadUInt16L()
            {
                return stream.ReadBytes(2).AsUInt16L();
            }

            public ushort ReadUInt16B()
            {
                return stream.ReadBytes(2).AsUInt16B();
            }

            public uint ReadUInt32L()
            {
                return stream.ReadBytes(4).AsUInt32L();
            }

            public uint ReadUInt32B()
            {
                return stream.ReadBytes(4).AsUInt32B();
            }
            
            public int ReadInt32L()
            {
                return stream.ReadBytes(4).AsInt32L();
            }

            public ulong ReadUInt64L()
            {
                return stream.ReadBytes(8).AsUInt64L();
            }

            public ulong ReadUInt64B()
            {
                return stream.ReadBytes(8).AsUInt64B();
            }

            public ulong ReadUInt64BitDependantL(bool is64Bit)
            {
                return is64Bit
                    ? stream.ReadUInt64L()
                    : stream.ReadUInt32L();
            }
        }
        
        extension<T, TS, TC>(TS stream)
            where TS : Stream
            where TC : INumber<TC>
        {
            public T[] ParseArray(TC count, Func<TS, T> itemProcessor)
            {
                var list = new List<T>();
                for (var x = TC.Zero; x < count; x++)
                {
                    var item = itemProcessor(stream);
                    list.Add(item);
                }
                return list.ToArray();
            }
            
            public T[] ParseArray(TC count, long offset, Func<TS, T> itemProcessor)
            {
                var oldPosition = stream.Position;
                stream.Position = offset;
                var res = stream.ParseArray(count, itemProcessor);
                stream.Position = oldPosition;
                return res;
            }
        }
    }
}