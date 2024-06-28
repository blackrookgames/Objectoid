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

                _SystemIsLittleEndian = BitConverter.IsLittleEndian;
                UpdateFlipIntBytes_m();
                UpdateFlipFloatBytes_m();
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

        private bool fFlipIntBytes;
        /// <summary>Whether or not integer bytes should be flipped when reading/writing</summary>
        private protected bool FlipIntBytes_p => fFlipIntBytes;

        /// <summary>Updates the value of <see cref="FlipIntBytes_p"/></summary>
        private void UpdateFlipIntBytes_m() => fFlipIntBytes = _SystemIsLittleEndian != fIntIsLittleEndian;

        /// <summary>Sets <see cref="IntIsLittleEndian"/> to the specified value</summary>
        /// <param name="value">Whether or not integers should be stored as little-endian</param>
        private protected void SetIntIsLittleEndian_m(bool value)
        {
            fIntIsLittleEndian = value;
            UpdateFlipIntBytes_m();
        }

        #endregion

        #region FloatIsLittleEndian

        private bool fFloatIsLittleEndian;
        /// <summary>Whether or not floats are stored as little-endian</summary>
        public bool FloatIsLittleEndian => fFloatIsLittleEndian;

        private bool fFlipFloatBytes;
        /// <summary>Whether or not float bytes should be flipped when reading/writing</summary>
        private protected bool FlipFloatBytes_p => fFlipFloatBytes;

        /// <summary>Updates the value of <see cref="FlipFloatBytes_p"/></summary>
        private void UpdateFlipFloatBytes_m() => fFlipFloatBytes = _SystemIsLittleEndian != fFloatIsLittleEndian;

        /// <summary>Sets <see cref="FloatIsLittleEndian"/> to the specified value</summary>
        /// <param name="value">Whether or not floats should be stored as little-endian</param>
        private protected void SetFloatIsLittleEndian_m(bool value)
        {
            fFloatIsLittleEndian = value;
            UpdateFlipFloatBytes_m();
        }

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
