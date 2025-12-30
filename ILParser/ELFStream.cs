using System.Buffers.Binary;
using System.Numerics;
using ILParser.Extensions;

namespace ILParser
{
    public class ELFStream : Stream
    {
        private readonly Stream _stream;
        private bool _isLittleEndian;
        public bool Is64Bit { get; private set; }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public ELFStream(Stream stream)
        {
            _stream = stream;
            _isLittleEndian = true;
            Is64Bit = true;
        }

        #region Public methods
        public void ConfigureStream(ELFIdentity identity)
        {
            _isLittleEndian = identity.DataEncoding == ELFEnums.ELFDataEncoding.LITTLE_ENDIAN;
            Is64Bit = identity.ClassType == ELFEnums.ELFClassType.BIT_64;
        }
        

        /// <summary>
        /// Elf32_Half
        /// </summary>
        public ushort ReadUInt16()
        {
            var bytes = _stream.ReadBytes(2);
            return _isLittleEndian
                ? BinaryPrimitives.ReadUInt16LittleEndian(bytes)
                : BinaryPrimitives.ReadUInt16BigEndian(bytes);
        }
        
        /// <summary>
        /// Elf32_Addr, Elf32_Off, Elf32_Word
        /// </summary>
        public uint ReadUInt32()
        {
            var bytes = _stream.ReadBytes(4);
            return _isLittleEndian
                ? BinaryPrimitives.ReadUInt32LittleEndian(bytes)
                : BinaryPrimitives.ReadUInt32BigEndian(bytes);
        }
            
        /// <summary>
        /// Elf32_Sword
        /// </summary>
        public int ReadInt32()
        {
            var bytes = _stream.ReadBytes(4);
            return _isLittleEndian
                ? BinaryPrimitives.ReadInt32LittleEndian(bytes)
                : BinaryPrimitives.ReadInt32BigEndian(bytes);
        }
        
        public ulong ReadUInt64()
        {
            var bytes = _stream.ReadBytes(8);
            return _isLittleEndian
                ? BinaryPrimitives.ReadUInt64LittleEndian(bytes)
                : BinaryPrimitives.ReadUInt64BigEndian(bytes);
        }

        public ulong ReadArchitectureDependant()
        {
            return Is64Bit
                ? ReadUInt64()
                : ReadUInt32();
        }
        
        public T[] ParseArray<T, TN>(TN count, Func<ELFStream, T> itemProcessor)
            where TN : INumber<TN>
        {
            var list = new List<T>();
            for (var x = TN.Zero; x < count; x++)
            {
                var item = itemProcessor(this);
                list.Add(item);
            }
            return list.ToArray();
        }
        
        public T[] ParseArray<T, TC>(TC count, long offset, Func<ELFStream, T> itemProcessor)
            where TC : INumber<TC>
        {
            var oldPosition = Position;
            Position = offset;
            var res = ParseArray(count, itemProcessor);
            Position = oldPosition;
            return res;
        }
        #endregion

        #region Overrides
        public override void Flush()
        {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }
        #endregion
    }
}