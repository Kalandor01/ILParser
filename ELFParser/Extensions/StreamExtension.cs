namespace ELFParser.Extensions
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
                return BitConverter.ToString(stream.ReadBytes(count));
            }

            public string ReadBytesAsString(int count)
            {
                return stream.ReadBytes(count).ToUtf8String();
            }
            
            public byte ReadByteB()
            {
                return (byte)stream.ReadByte();
            }
        }
    }
}