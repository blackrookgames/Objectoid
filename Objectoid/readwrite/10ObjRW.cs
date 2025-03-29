using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objectoid
{
    /// <summary>Base class for <see cref="ObjReader"/> and <see cref="ObjWriter"/></summary>
    internal abstract partial class ObjRW
    {
        /// <summary>Constructor for <see cref="ObjRW"/></summary>
        /// <param name="stream">Stream</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support seeking</exception>
        private protected ObjRW(Stream stream)
        {
            try
            {
                if (!stream.CanSeek)
                    throw new ArgumentException("The stream does not support seeking.", nameof(stream));
                _Stream = stream;
            }
            catch when (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
        }

        private readonly Stream _Stream;
        /// <summary>Stream the <see cref="ObjRW"/> is using</summary>
        public Stream Stream => _Stream;

        /// <summary>Whether or not the computer architecture stores data in little-endian</summary>
        private readonly bool _SystemIsLittleEndian;

        #region IntIsLittleEndian

        private bool fIntIsLittleEndian;
        /// <summary>Whether or not integers are stored as little-endian</summary>
        public bool IntIsLittleEndian => fIntIsLittleEndian;

        /// <summary>Sets <see cref="IntIsLittleEndian"/> to the specified value</summary>
        /// <param name="value">Whether or not integers should be stored as little-endian</param>
        private protected void SetIntIsLittleEndian_m(bool value) => fIntIsLittleEndian = value;
        #endregion

        #region FloatIsLittleEndian

        private bool fFloatIsLittleEndian;
        /// <summary>Whether or not floats are stored as little-endian</summary>
        public bool FloatIsLittleEndian => fFloatIsLittleEndian;
        /// <summary>Sets <see cref="FloatIsLittleEndian"/> to the specified value</summary>
        /// <param name="value">Whether or not floats should be stored as little-endian</param>
        private protected void SetFloatIsLittleEndian_m(bool value) => fFloatIsLittleEndian = value;

        #endregion

        /// <summary>Writes a value type</summary>
        /// <param name="type">Value type</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteType(ValueType type) => WriteUInt8((byte)type);


        /// <summary>Reads a value type</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public ValueType ReadType() => (ValueType)ReadUInt8();
    }
}
