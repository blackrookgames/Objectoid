using System;
using System.IO;
using Rookie.IO;

namespace Objectoid
{
    internal partial class ObjRW
    {
        private static readonly byte[] _Buffer = new byte[8];

        /// <summary>Writes an unsigned 8-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteUInt8(byte value)
        {
            try
            {
                _Stream.WriteByte(value);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads an unsigned 8-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public byte ReadUInt8()
        {
            try
            {
                if (_Stream.Read(_Buffer, 0, 1) < 1)
                    throw new EndOfStreamException();
                byte value = _Buffer[0];
                return value;
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes a signed 8-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteInt8(sbyte value)
        {
            try
            {
                _Stream.WriteByte((byte)value);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads a signed 8-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public sbyte ReadInt8()
        {
            try
            {
                if (_Stream.Read(_Buffer, 0, 1) < 1)
                    throw new EndOfStreamException();
                byte value = _Buffer[0];
                return (sbyte)value;
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes an unsigned 16-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteUInt16(ushort value)
        {
            try
            {
                _Stream.WriteUInt16(_Buffer, value, IntIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads an unsigned 16-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public ushort ReadUInt16()
        {
            try
            {
                return _Stream.ReadUInt16(_Buffer, IntIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes a signed 16-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteInt16(short value)
        {
            try
            {
                _Stream.WriteInt16(_Buffer, value, IntIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads a signed 16-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public short ReadInt16()
        {
            try
            {
                return _Stream.ReadInt16(_Buffer, IntIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes an unsigned 32-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteUInt32(uint value)
        {
            try
            {
                _Stream.WriteUInt32(_Buffer, value, IntIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads an unsigned 32-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public uint ReadUInt32()
        {
            try
            {
                return _Stream.ReadUInt32(_Buffer, IntIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes a signed 32-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteInt32(int value)
        {
            try
            {
                _Stream.WriteInt32(_Buffer, value, IntIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads a signed 32-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public int ReadInt32()
        {
            try
            {
                return _Stream.ReadInt32(_Buffer, IntIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes an unsigned 64-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteUInt64(ulong value)
        {
            try
            {
                _Stream.WriteUInt64(_Buffer, value, IntIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads an unsigned 64-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public ulong ReadUInt64()
        {
            try
            {
                return _Stream.ReadUInt64(_Buffer, IntIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes a signed 64-bit integer value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteInt64(long value)
        {
            try
            {
                _Stream.WriteInt64(_Buffer, value, IntIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads a signed 64-bit integer value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public long ReadInt64()
        {
            try
            {
                return _Stream.ReadInt64(_Buffer, IntIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes a single-precision floating-point value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteSingle(float value)
        {
            try
            {
                _Stream.WriteSingle(_Buffer, value, FloatIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads a single-precision floating-point value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public float ReadSingle()
        {
            try
            {
                return _Stream.ReadSingle(_Buffer, FloatIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes a double-precision floating-point value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteDouble(double value)
        {
            try
            {
                _Stream.WriteDouble(_Buffer, value, FloatIsLittleEndian);
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads a double-precision floating-point value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public double ReadDouble()
        {
            try
            {
                return _Stream.ReadDouble(_Buffer, FloatIsLittleEndian);
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Writes a boolean value</summary>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteBool(bool value)
        {
            try
            {
                _Stream.WriteByte((byte)(value ? 1 : 0));
            }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }

        /// <summary>Reads a boolean value</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public bool ReadBool()
        {
            try
            {
                if (_Stream.Read(_Buffer, 0, 1) < 1)
                    throw new EndOfStreamException();
                byte value = _Buffer[0];
                return (value & 1) == 1;
            }
            catch (EndOfStreamException e) { throw e; }
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }
    }
}

