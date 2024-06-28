using System;
using System.IO;

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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipIntBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 2) < 2)
                    throw new EndOfStreamException();
                if (FlipIntBytes_p) Array.Reverse(_Buffer);
                int index = FlipIntBytes_p ? (_Buffer.Length - 2) : 0;
                return BitConverter.ToUInt16(_Buffer, index);
            }
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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipIntBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 2) < 2)
                    throw new EndOfStreamException();
                if (FlipIntBytes_p) Array.Reverse(_Buffer);
                int index = FlipIntBytes_p ? (_Buffer.Length - 2) : 0;
                return BitConverter.ToInt16(_Buffer, index);
            }
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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipIntBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 4) < 4)
                    throw new EndOfStreamException();
                if (FlipIntBytes_p) Array.Reverse(_Buffer);
                int index = FlipIntBytes_p ? (_Buffer.Length - 4) : 0;
                return BitConverter.ToUInt32(_Buffer, index);
            }
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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipIntBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 4) < 4)
                    throw new EndOfStreamException();
                if (FlipIntBytes_p) Array.Reverse(_Buffer);
                int index = FlipIntBytes_p ? (_Buffer.Length - 4) : 0;
                return BitConverter.ToInt32(_Buffer, index);
            }
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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipIntBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 8) < 8)
                    throw new EndOfStreamException();
                if (FlipIntBytes_p) Array.Reverse(_Buffer);
                int index = FlipIntBytes_p ? (_Buffer.Length - 8) : 0;
                return BitConverter.ToUInt64(_Buffer, index);
            }
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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipIntBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 8) < 8)
                    throw new EndOfStreamException();
                if (FlipIntBytes_p) Array.Reverse(_Buffer);
                int index = FlipIntBytes_p ? (_Buffer.Length - 8) : 0;
                return BitConverter.ToInt64(_Buffer, index);
            }
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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipFloatBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 4) < 4)
                    throw new EndOfStreamException();
                if (FlipFloatBytes_p) Array.Reverse(_Buffer);
                int index = FlipFloatBytes_p ? (_Buffer.Length - 4) : 0;
                return BitConverter.ToSingle(_Buffer, index);
            }
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
                byte[] buffer = BitConverter.GetBytes(value);
                if (FlipFloatBytes_p) Array.Reverse(buffer);
                _Stream.Write(buffer, 0, buffer.Length);
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
                if (_Stream.Read(_Buffer, 0, 8) < 8)
                    throw new EndOfStreamException();
                if (FlipFloatBytes_p) Array.Reverse(_Buffer);
                int index = FlipFloatBytes_p ? (_Buffer.Length - 8) : 0;
                return BitConverter.ToDouble(_Buffer, index);
            }
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
            catch (IOException e) { throw e; }
            catch (ObjectDisposedException e) { throw e; }
        }
    }
}

