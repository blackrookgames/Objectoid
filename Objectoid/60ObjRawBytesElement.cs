using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Objectoid
{
    /// <summary>Represents an element with raw byte data</summary>
    public class ObjRawBytesElement : ObjAddressable, IEnumerable<byte>
    {
        #region helper

        private ArgumentOutOfRangeException NegativeLength_m(string paramName) =>
            throw new ArgumentOutOfRangeException("Length cannot be negative", paramName);

        #endregion

        #region ObjElement

        internal override void Read_m(ObjReader objReader)
        {
            //Read length
            int rawLength = objReader.ReadInt32();
            __Bytes = new byte[(rawLength < 0) ? 0 : rawLength];
            //Read data
            for (int i = 0; i < rawLength; i++)
                __Bytes[i] = objReader.ReadUInt8();
        }

        internal override void Write_m(ObjWriter objWriter)
        {
            //Write length
            objWriter.WriteInt32(__Bytes.Length);
            //Write data
            for (int i = 0; i < __Bytes.Length; i++)
                objWriter.WriteUInt8(__Bytes[i]);
        }

        #endregion

        #region ObjAddressable

        /// <inheritdoc/>
        private protected override bool Equals_m(ObjAddressable other) => Equals(other);

        /// <inheritdoc/>
        private protected override int GetHashCode_m() => GetHashCode();

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        /// <summary>Gets an enumerator for the byte data</summary>
        /// <returns>An enumerator for the byte data</returns>
        public IEnumerator<byte> GetEnumerator() => new Enumerator(this);

        private class Enumerator : IEnumerator<byte>
        { 
            public Enumerator(ObjRawBytesElement instance)
            {
                _Instance = instance;
            }

            private readonly ObjRawBytesElement _Instance;
            private int __Position = -1;

            private byte Current_p
            {
                get
                {
                    try { return _Instance.__Bytes[__Position]; }
                    catch { throw new InvalidOperationException(); }
                }
            }
            object IEnumerator.Current => Current_p;
            byte IEnumerator<byte>.Current => Current_p;

            void IEnumerator.Reset() => __Position = -1;

            bool IEnumerator.MoveNext() => (++__Position) < _Instance.__Bytes.Length;

            void IDisposable.Dispose() { }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjRawBytesElement"/></summary>
        public ObjRawBytesElement() : base(ObjType.RawBytes, true)
        {
            __Bytes = new byte[0];
        }

        /// <summary>Creates an instance of <see cref="ObjRawBytesElement"/> with a specified initial value</summary>
        /// <param name="initialLength">Initial length</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="initialLength"/> is negative</exception>
        public ObjRawBytesElement(int initialLength) : base(ObjType.RawBytes, true)
        {
            if (initialLength < 0) throw NegativeLength_m(nameof(initialLength));
            __Bytes = new byte[initialLength];
        }

        /// <summary>Creates an instance of <see cref="ObjRawBytesElement"/> with specified initial data</summary>
        /// <param name="initialData">Initial data</param>
        /// <exception cref="ArgumentNullException"><paramref name="initialData"/> is null</exception>
        public ObjRawBytesElement(IEnumerable<byte> initialData) : base(ObjType.RawBytes, true)
        {
            try { __Bytes = initialData.ToArray(); }
            catch when (initialData is null) { throw new ArgumentNullException(nameof(initialData)); }
        }

        /// <summary>Loads data from the specified enumerable collection</summary>
        /// <param name="data">Enumerable collection to load data from</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null</exception>
        public void Load(IEnumerable<byte> data)
        {
            try { __Bytes = data.ToArray(); }
            catch when (data is null) { throw new ArgumentNullException(nameof(data)); }
        }

        private byte[] __Bytes;

        /// <summary>Length</summary>
        public int Length => __Bytes.Length;

        /// <summary>Gets/sets the byte at the specified index</summary>
        /// <param name="index">Index of the byte</param>
        /// <returns>The byte at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public byte this[int index]
        {
            get
            {
                try
                {
                    return __Bytes[index];
                }
                catch when (index < 0 || index >= __Bytes.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
            set
            {
                try
                {
                    __Bytes[index] = value;
                }
                catch when (index < 0 || index >= __Bytes.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        /// <summary>Sets the length of the data</summary>
        /// <param name="length">New length of the data</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative</exception>
        public void SetLength(int length)
        {
            if (length < 0) throw NegativeLength_m(nameof(length));
            byte[] oldBytes = __Bytes;
            __Bytes = new byte[length];
            int minLength = (oldBytes.Length < length) ? oldBytes.Length : length;
            for (int i = 0; i < minLength; i++)
                __Bytes[i] = oldBytes[i];
        }
    }
}
